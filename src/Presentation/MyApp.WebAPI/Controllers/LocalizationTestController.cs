using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Common.Constants;
using MyApp.Application.Common.Interfaces;

namespace MyApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocalizationTestController : ControllerBase
{
    private readonly ILocalizationService _localization;

    public LocalizationTestController(ILocalizationService localization)
    {
        _localization = localization;
    }

    [HttpGet("say-hello")]
    public IActionResult SayHello()
    {
        var message = _localization[LocalizationConstants.SayHello];
        return Ok(new { Message = message });
    }
}
