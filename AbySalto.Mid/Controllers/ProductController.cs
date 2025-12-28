using AbySalto.Mid.Application.Common.Exceptions;
using AbySalto.Mid.Application.Favorites;
using AbySalto.Mid.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IFavoriteService _favoriteService;

    public ProductController(IProductService productService, IFavoriteService favoriteService)
    {
        _productService = productService;
        _favoriteService = favoriteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int limit = 30, [FromQuery] int skip = 0, [FromQuery] string? sortBy = null,
        [FromQuery] string? orderBy = null)
    {
        var result = await _productService.GetProductsAsync(limit, skip, sortBy, orderBy);
        return result is null ? NoContent() : Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost("{id:int}/favorite")]
    public async Task<IActionResult> AddToFavorites(int id)
    {
        var cmd = new AddFavoriteCommand(id);
        try
        {
            var result = await _favoriteService.AddFavoriteAsync(cmd);
            return CreatedAtAction(nameof(GetProductById), new { id = result.ProductId }, result);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}
