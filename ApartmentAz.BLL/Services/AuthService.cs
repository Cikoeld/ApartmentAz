using ApartmentAz.BLL.DTOs.Auth;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApartmentAz.BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _config;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration config,
        IRefreshTokenRepository refreshTokenRepository,
        IEmailService emailService,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _refreshTokenRepository = refreshTokenRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result<AuthResultDto>> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return Result<AuthResultDto>.Failure("User with this email already exists.");

        var user = new AppUser
        {
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result<AuthResultDto>.Failure(errors);
        }

        await _userManager.AddToRoleAsync(user, "User");
        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        var (accessToken, expires) = GenerateAccessToken(user, roles);
        var refreshToken = await GenerateRefreshTokenAsync(user.Id);

        return Result<AuthResultDto>.Success(new AuthResultDto
        {
            Succeeded = true,
            UserId = user.Id,
            Token = accessToken,
            RefreshToken = refreshToken,
            TokenExpires = expires,
            Roles = roles
        });
    }

    public async Task<Result<AuthResultDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Result<AuthResultDto>.Failure("Invalid email or password.");

        if (user.IsBanned)
            return Result<AuthResultDto>.Failure("Your account has been banned.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Result<AuthResultDto>.Failure("Invalid email or password.");

        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        var (accessToken, expires) = GenerateAccessToken(user, roles);
        var refreshToken = await GenerateRefreshTokenAsync(user.Id);

        return Result<AuthResultDto>.Success(new AuthResultDto
        {
            Succeeded = true,
            UserId = user.Id,
            Token = accessToken,
            RefreshToken = refreshToken,
            TokenExpires = expires,
            Roles = roles
        });
    }

    public async Task<Result<AuthResultDto>> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (storedToken == null)
            return Result<AuthResultDto>.Failure("Invalid refresh token.");

        if (!storedToken.IsActive)
            return Result<AuthResultDto>.Failure("Refresh token is expired or revoked.");

        // Revoke the old refresh token
        storedToken.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.SaveChangesAsync();

        // Generate new tokens
        var user = storedToken.User;
        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        var (newAccessToken, expires) = GenerateAccessToken(user, roles);
        var newRefreshToken = await GenerateRefreshTokenAsync(user.Id);

        return Result<AuthResultDto>.Success(new AuthResultDto
        {
            Succeeded = true,
            UserId = user.Id,
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenExpires = expires,
            Roles = roles
        });
    }

    public async Task<Result<bool>> RevokeTokenAsync(string refreshToken)
    {
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (storedToken == null || !storedToken.IsActive)
            return Result<bool>.Failure("Invalid or already revoked token.");

        storedToken.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> RevokeAllTokensAsync(Guid userId)
    {
        await _refreshTokenRepository.RevokeAllForUserAsync(userId);
        return Result<bool>.Success(true);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<bool>.Success(true);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebUtility.UrlEncode(token);
        var encodedEmail = WebUtility.UrlEncode(dto.Email);
        var resetUrl = $"https://localhost:7090/reset-password?email={encodedEmail}&token={encodedToken}";

        try
        {
            await _emailService.SendPasswordResetEmailAsync(dto.Email, resetUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", dto.Email);
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Result<bool>.Failure("Invalid password reset request.");

        var decodedToken = WebUtility.UrlDecode(dto.Token);
        var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

        if (!resetResult.Succeeded)
        {
            var errors = string.Join("; ", resetResult.Errors.Select(e => e.Description));
            return Result<bool>.Failure(errors);
        }

        return Result<bool>.Success(true);
    }

    private (string Token, DateTime Expires) GenerateAccessToken(AppUser user, List<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Short-lived access token (15 minutes)
        var accessTokenMinutes = int.TryParse(_config["Jwt:AccessTokenMinutes"], out var m) ? m : 15;
        var expires = DateTime.UtcNow.AddMinutes(accessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer:   _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims:   claims,
            expires:  expires,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    private async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        var refreshTokenDays = int.TryParse(_config["Jwt:RefreshTokenDays"], out var d) ? d : 7;

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays),
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(refreshToken);
        return refreshToken.Token;
    }
}
