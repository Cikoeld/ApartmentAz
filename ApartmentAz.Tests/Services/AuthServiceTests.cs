using ApartmentAz.BLL.DTOs.Auth;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Services;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ApartmentAz.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepoMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly IConfiguration _config;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        // UserManager mock
        var store = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        // SignInManager mock
        _signInManagerMock = new Mock<SignInManager<AppUser>>(
            _userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
            null!, null!, null!, null!);

        _refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _loggerMock = new Mock<ILogger<AuthService>>();

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "SuperSecretKeyForTesting12345678901234567890",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:AccessTokenMinutes"] = "15",
                ["Jwt:RefreshTokenDays"] = "7"
            })
            .Build();

        _sut = new AuthService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _config,
            _refreshTokenRepoMock.Object,
            _emailServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ExistingEmail_ReturnsFailure()
    {
        // Arrange
        _userManagerMock.Setup(u => u.FindByEmailAsync("existing@test.com"))
            .ReturnsAsync(new AppUser { Email = "existing@test.com", FullName = "Test" });

        var dto = new RegisterDto
        {
            FullName = "Test",
            Email = "existing@test.com",
            Password = "Test123"
        };

        // Act
        var result = await _sut.RegisterAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("already exists", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RegisterAsync_ValidData_ReturnsSuccessWithTokens()
    {
        // Arrange
        var dto = new RegisterDto
        {
            FullName = "New User",
            Email = "new@test.com",
            Password = "Test123"
        };

        _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
            .ReturnsAsync((AppUser?)null);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), dto.Password))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<AppUser>(), "User"))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(["User"]);
        _refreshTokenRepoMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.RegisterAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data!.Token);
        Assert.NotNull(result.Data.RefreshToken);
        Assert.True(result.Data.Succeeded);
        Assert.Contains("User", result.Data.Roles);
    }

    [Fact]
    public async Task LoginAsync_InvalidEmail_ReturnsFailure()
    {
        // Arrange
        _userManagerMock.Setup(u => u.FindByEmailAsync("wrong@test.com"))
            .ReturnsAsync((AppUser?)null);

        var dto = new LoginDto { Email = "wrong@test.com", Password = "Test123" };

        // Act
        var result = await _sut.LoginAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Invalid", result.ErrorMessage!);
    }

    [Fact]
    public async Task LoginAsync_BannedUser_ReturnsFailure()
    {
        // Arrange
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = "banned@test.com",
            FullName = "Banned User",
            IsBanned = true
        };

        _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
            .ReturnsAsync(user);

        var dto = new LoginDto { Email = user.Email, Password = "Test123" };

        // Act
        var result = await _sut.LoginAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("banned", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsSuccessWithTokens()
    {
        // Arrange
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = "valid@test.com",
            UserName = "valid@test.com",
            FullName = "Valid User",
            IsBanned = false
        };

        _userManagerMock.Setup(u => u.FindByEmailAsync(user.Email))
            .ReturnsAsync(user);
        _signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(user, "Test123", false))
            .ReturnsAsync(SignInResult.Success);
        _userManagerMock.Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(["User"]);
        _refreshTokenRepoMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        var dto = new LoginDto { Email = user.Email, Password = "Test123" };

        // Act
        var result = await _sut.LoginAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data!.Token);
        Assert.NotNull(result.Data.RefreshToken);
        Assert.True(result.Data.TokenExpires > DateTime.UtcNow);
    }

    [Fact]
    public async Task RefreshTokenAsync_InvalidToken_ReturnsFailure()
    {
        // Arrange
        _refreshTokenRepoMock.Setup(r => r.GetByTokenAsync("invalid"))
            .ReturnsAsync((RefreshToken?)null);

        // Act
        var result = await _sut.RefreshTokenAsync("invalid");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Invalid", result.ErrorMessage!);
    }

    [Fact]
    public async Task RefreshTokenAsync_ExpiredToken_ReturnsFailure()
    {
        // Arrange
        var expired = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "expired-token",
            UserId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddDays(-1),
            CreatedAt = DateTime.UtcNow.AddDays(-8)
        };

        _refreshTokenRepoMock.Setup(r => r.GetByTokenAsync("expired-token"))
            .ReturnsAsync(expired);

        // Act
        var result = await _sut.RefreshTokenAsync("expired-token");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("expired", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RefreshTokenAsync_ValidToken_ReturnsNewTokens()
    {
        // Arrange
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = "user@test.com",
            UserName = "user@test.com",
            FullName = "Test User"
        };

        var storedToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "valid-refresh-token",
            UserId = user.Id,
            User = user,
            ExpiresAt = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        };

        _refreshTokenRepoMock.Setup(r => r.GetByTokenAsync("valid-refresh-token"))
            .ReturnsAsync(storedToken);
        _refreshTokenRepoMock.Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);
        _refreshTokenRepoMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);
        _userManagerMock.Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(["User"]);

        // Act
        var result = await _sut.RefreshTokenAsync("valid-refresh-token");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data!.Token);
        Assert.NotNull(result.Data.RefreshToken);
        Assert.NotEqual("valid-refresh-token", result.Data.RefreshToken);
    }

    [Fact]
    public async Task RevokeAllTokensAsync_CallsRepository()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _refreshTokenRepoMock.Setup(r => r.RevokeAllForUserAsync(userId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.RevokeAllTokensAsync(userId);

        // Assert
        Assert.True(result.IsSuccess);
        _refreshTokenRepoMock.Verify(r => r.RevokeAllForUserAsync(userId), Times.Once);
    }
}
