using MyApp.Application.Common.DTOs;
using MyApp.Application.Common.DTOs.Mail;

namespace MyApp.Application.Common.Interfaces.Mail;

public interface IMailService
{
    Task SendEmailAsync(MailRequest request);
}
