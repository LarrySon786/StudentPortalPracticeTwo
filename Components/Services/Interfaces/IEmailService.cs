namespace StudentPortalPracticeTwo.Components.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string recipientEmail, string emailSubject, string htmlContent);
}

