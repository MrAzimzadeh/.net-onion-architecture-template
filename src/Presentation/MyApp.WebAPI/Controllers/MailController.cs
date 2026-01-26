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


    [HttpPost("DataSeeder")]
    public async Task<IActionResult> DataSeeder()
    {

       string[] email =  [""];

        for (int i = 0; i < email.Length; i++)
        {   
            var request = new SendMailCommand
            {
                ToAddresses = new List<string> { email[i] },
                Subject = "Test Mail",
                Body = "This is a test mail",
            };
            var result = await mediator.Send(request);
        }
        return StatusCode(200, "DataSeeder is done");
    }
}