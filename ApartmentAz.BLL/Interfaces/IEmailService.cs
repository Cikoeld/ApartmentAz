namespace ApartmentAz.BLL.Interfaces;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string toEmail, string resetUrl);
}
