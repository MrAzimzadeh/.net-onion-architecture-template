using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.Interfaces;
using MyApp.Application.Common.Interfaces.Mail;
using MyApp.Application.Common.DTOs.Mail;

namespace MyApp.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;

    public MailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(MailRequest request)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
        
        // To
        if (request.ToAddresses != null)
        {
            foreach (var to in request.ToAddresses)
            {
                email.To.Add(MailboxAddress.Parse(to));
            }
        }
        
        // CC
        if (request.CcAddresses != null)
        {
            foreach (var cc in request.CcAddresses)
            {
                email.Cc.Add(MailboxAddress.Parse(cc));
            }
        }

        // BCC (Private CC)
        if (request.BccAddresses != null)
        {
            foreach (var bcc in request.BccAddresses)
            {
                email.Bcc.Add(MailboxAddress.Parse(bcc));
            }
        }

        email.Subject = request.Subject;

        var builder = new BodyBuilder();
        builder.HtmlBody = request.Body;

        // Attachments
        if (request.Attachments != null)
        {
            foreach (var file in request.Attachments)
            {
                if (file.Content.Length > 0)
                {
                    // If ContentType is provided, parse it, otherwise let MailKit detect or default
                    if (!string.IsNullOrEmpty(file.ContentType))
                    {
                        builder.Attachments.Add(file.FileName, file.Content, ContentType.Parse(file.ContentType));
                    }
                    else
                    {
                        builder.Attachments.Add(file.FileName, file.Content);
                    }
                }
            }
        }

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        // Use Auto to automatically negotiate the appropriate SSL/TLS protocol
        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto);
        
        if (!string.IsNullOrEmpty(_mailSettings.Password))
        {
             await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
        }
       
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
