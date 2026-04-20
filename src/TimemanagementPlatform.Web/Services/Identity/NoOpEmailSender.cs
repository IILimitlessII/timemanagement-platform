using Microsoft.AspNetCore.Identity.UI.Services;

namespace TimemanagementPlatform.Web.Services.Identity;

public sealed class NoOpEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.CompletedTask;
    }
}

