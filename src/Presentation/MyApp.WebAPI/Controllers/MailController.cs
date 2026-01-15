using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Features.Mail.Commands.SendMail;

namespace MyApp.WebAPI.Controllers;


/// <summary>
/// Mail Controller
/// </summary>

public class MailController(IMediator mediator) : BaseController
{


    [HttpPost]
    public async Task<IActionResult> SendMail(SendMailCommand request)
    {
        var result = await mediator.Send(request);
        return StatusCode((int)result.HttpStatusCode, result);
    }
}