using MediatR;
using MyApp.Application.Common.DTOs.Mail;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Mail.Commands.SendMail;

/// <summary>
/// Command to login user with device information
/// </summary>
public record SendMailCommand : IRequest<Result<bool>>
{
    public List<string> ToAddresses { get; init; } = new();
    public List<string>? CcAddresses { get; init; } = new();
    public List<string>? BccAddresses { get; init; } = new();
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public List<MailAttachment>? Attachments { get; init; } = new();
}