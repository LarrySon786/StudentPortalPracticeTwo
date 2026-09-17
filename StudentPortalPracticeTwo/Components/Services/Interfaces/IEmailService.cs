namespace StudentPortalPracticeTwo.Components.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string recipientEmail, string recipientName, string emailSubject, string htmlContent);
}

