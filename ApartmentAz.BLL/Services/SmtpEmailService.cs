using ApartmentAz.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ApartmentAz.BLL.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public SmtpEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetUrl)
    {
        var host = _configuration["Email:SmtpHost"];
        var senderEmail = _configuration["Email:SenderEmail"];

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(senderEmail))
            throw new InvalidOperationException("Email settings are not configured.");

        var senderName = _configuration["Email:SenderName"] ?? "ApartmentAz";
        var username = _configuration["Email:Username"];
        var password = _configuration["Email:Password"];
        var enableSsl = bool.TryParse(_configuration["Email:EnableSsl"], out var ssl) && ssl;
        var port = int.TryParse(_configuration["Email:SmtpPort"], out var smtpPort) ? smtpPort : 587;

        using var message = new MailMessage
        {
            From = new MailAddress(senderEmail, senderName),
            Subject = "Reset your password",
            Body = $"Please reset your password by clicking this link: {resetUrl}",
            IsBodyHtml = false
        };

        message.To.Add(toEmail);

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = enableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false
        };

        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            client.Credentials = new NetworkCredential(username, password);

        await client.SendMailAsync(message);
    }
}
