namespace MyApp.Application.Common.DTOs.Mail;

public class MailRequest
{
    public List<string> ToAddresses { get; set; } = new();
    public List<string> CcAddresses { get; set; } = new();
    public List<string> BccAddresses { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<MailAttachment> Attachments { get; set; } = new();
}
