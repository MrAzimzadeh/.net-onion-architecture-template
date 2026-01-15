using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.WebAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public abstract class BaseController : Controller
{
}