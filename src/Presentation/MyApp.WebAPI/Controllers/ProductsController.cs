using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Common.Interfaces;

namespace MyApp.WebAPI.Controllers;

/// <summary>
/// Products Controller - uses MyApp.Domain Product entity
/// </summary>
public class ProductsController(ILocalizationService localization) : BaseController
{

    /// <summary>
    /// Example endpoint using MyApp Product entity
    /// Note: You would create corresponding CQRS commands/queries for Product
    /// </summary>
    [HttpGet]
    public IActionResult GetProducts()
    {
        // TODO: Implement GetAllProductsQuery similar to Orders
        return Ok(new
        {
            Message = localization["Products"],
            Info = "Create GetAllProductsQuery and GetAllProductsQueryHandler to implement"
        });
    }
}
