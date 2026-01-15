using MediatR;
using Microsoft.Extensions.Logging;
using MyApp.Application.Common.Interfaces.Messaging;
using MyApp.Application.Common.Messaging.Constants;
using MyApp.Domain.Common;

namespace MyApp.Application.Features.Mail.Commands.SendMail;


/// <summary>
/// Handler for SendMail
/// </summary>
public class SendMailCommandHandler(IMessageBus publisher, ILogger<SendMailCommandHandler> _logger) : IRequestHandler<SendMailCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(SendMailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await publisher.PublishAsync(
                message: request,
                exchangeName: MessageBusConst.MailExchange,
                routingKey: MessageBusConst.MailRoutingKey);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending email via SendMailCommandHandler.");
            return Result.Failure<bool>(message: "Send Failed");
        }
    }
}