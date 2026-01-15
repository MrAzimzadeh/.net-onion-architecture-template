using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Features.Storage.Commonds.UploadFileCommon;
using MyApp.Application.Features.Storage.Queries.GetFileCommands;

namespace MyApp.WebAPI.Controllers;

/// <summary>
/// Upload Controller - Upload management
/// </summary>
public class UploadController(IMediator mediator) : BaseController
{

    /// <summary>
    /// Get file
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(GetFileCommand query)
    {
        var result = await mediator.Send(query);
        return StatusCode((int)result.HttpStatusCode, result);
    }


    /// <summary>
    /// Upload file
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post(UploadFileCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.HttpStatusCode, result);
    }

}