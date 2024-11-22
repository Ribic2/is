using Microsoft.AspNetCore.Identity.UI.Services;

namespace GigaJira.Models;

public class EmailSender: IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Implement your email sending logic here
        // For demonstration purposes, this example does nothing
        Console.WriteLine($"Sending email to {email} with subject {subject}");
        return Task.CompletedTask;
    }
}