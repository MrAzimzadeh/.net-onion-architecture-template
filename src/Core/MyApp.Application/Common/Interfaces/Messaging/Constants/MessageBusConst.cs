namespace MyApp.Application.Common.Messaging.Constants;

public class MessageBusConst
{
    public const string MailExchange = "mail-exchange";
    public const string MailQueName = "mail-queue";
    public const string MailRoutingKey = "mail-routing-key";

    public const string DeadLetterExchange = "mail-dead-letter-exchange";
    public const string DeadLetterQueue = "mail-dead-letter-queue";
    public const string DeadLetterRoutingKey = "mail-dead-letter-routing-key";
    

}