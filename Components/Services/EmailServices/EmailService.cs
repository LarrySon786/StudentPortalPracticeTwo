using Microsoft.AspNetCore.Http.HttpResults;
using SendGrid;
using SendGrid.Helpers.Mail;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StudentPortalPracticeTwo.Components.Services.EmailServices;

public class EmailService : IEmailService
{
    readonly private string _api_key;
    readonly private string _sender_email; //This is the email that sends CSU emails
    readonly private string _from = "CSU Admissions"; // name of the sender listed on email

    public EmailService()
    {
        _api_key = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")!;
        _sender_email = Environment.GetEnvironmentVariable("SENDGRID_SENDER_EMAIL")!;
    }

    public async Task SendEmailAsync(string recipientEmail, string recipientName, string subject, string html)
    {
        // EMAIL CONNECTION | FROM | TO
        var client = new SendGridClient(_api_key);
        var from = new EmailAddress(_sender_email, _from);
        var to = new EmailAddress(recipientEmail, recipientName);

        // EMAIL BODY CONTENT
        var plainTextContent = HtmlToPlainText(html);

        // SENDS THE EMAIL
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, html);
        var response = await client.SendEmailAsync(msg);

        // Error Handling
        Console.WriteLine(response.StatusCode);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Email failed: {response.StatusCode}");
        }
    }

    private string HtmlToPlainText(string html)
    {
        return "Please view this email in an HTML-compatible email client.";
    }
}