using StudentPortalPracticeTwo.Components.Services.Interfaces;
using System;
using System.Threading.Tasks;
using brevo_csharp.Api;
using brevo_csharp.Client;
using brevo_csharp.Model;

namespace StudentPortalPracticeTwo.Components.Services.EmailServices;

public class EmailService : IEmailService
{
    readonly private string _api_key;
    readonly private string _sender_email; //This is the email that sends CSU emails
    readonly private string _from = "CSU Admissions"; // name of the sender listed on email

    public EmailService()
    {
        _api_key = Environment.GetEnvironmentVariable("BREVO_API_KEY")
            ?? throw new InvalidOperationException("BREVO_API_KEY is not configured.");

        _sender_email = Environment.GetEnvironmentVariable("BREVO_SENDER_EMAIL")
            ?? throw new InvalidOperationException("BREVO_SENDER_EMAIL is not configured.");
    }

    public System.Threading.Tasks.Task SendEmailAsync(string recipientEmail, string recipientName, string subject, string html)
    {
        // EMAIL CONNECTION | FROM | TO
        brevo_csharp.Client.Configuration.Default.ApiKey["api-key"] = _api_key;
        var apiInstance = new TransactionalEmailsApi();

        // SENDER
        var sender = new SendSmtpEmailSender(_from, _sender_email);
        // RECIPIENT
        var recipient = new SendSmtpEmailTo(recipientEmail, recipientName);

        // SENDS THE EMAIL
        var email = new SendSmtpEmail()
        {
            Sender = sender,
            To = new List<SendSmtpEmailTo>
            {
                recipient
            },
            Subject = subject,
            HtmlContent = html,
            TextContent = HtmlToPlainText(html) // turns html into plain text for non-supported html email readers
        };

        var response = apiInstance.SendTransacEmail(email);

        // Error Handling
        Console.WriteLine(response.MessageId);
        return System.Threading.Tasks.Task.CompletedTask;
    }

    private string HtmlToPlainText(string html)
    {
        return "Please view this email in an HTML-compatible email client.";
    }
}