using StudentPortalPracticeTwo.Components.Services.Interfaces;

namespace StudentPortalPracticeTwo.Components.Services.EmailServices;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string html)
    {
        
    }
}