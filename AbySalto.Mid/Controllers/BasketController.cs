using AbySalto.Mid.Application.Carts;
using AbySalto.Mid.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        private readonly ICartService _cartService;

        public BasketController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart(CancellationToken cancellation)
        {
            var cart = await _cartService.GetCartAsync(cancellation);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command, CancellationToken cancellation)
        {
            var cart = await _cartService.AddToCartAsync(command, cancellation);
            return Ok(cart);
        }

        [HttpDelete("items/{productId:int}")]
        public async Task<IActionResult> RemoveFromCart(int productId, CancellationToken cancellation)
        {
            var cart = await _cartService.RemoveFromCartAsync(productId, cancellation);
            return Ok(cart);
        }
    }
}
