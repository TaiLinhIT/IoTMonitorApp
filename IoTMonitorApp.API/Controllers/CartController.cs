using IoTMonitorApp.API.Dto.Cart;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IoTMonitorApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var cart = await _cartService.GetCartByUserAsync(userId.Value);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] AddToCartDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var cart = await _cartService.AddItemAsync(userId.Value, dto.ProductId, dto.Quantity);
            return Ok(cart);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateCartItemDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var cart = await _cartService.UpdateItemAsync(userId.Value, dto.ProductId, dto.Quantity);
            return Ok(cart);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveItem([FromQuery] Guid productId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var cart = await _cartService.RemoveItemAsync(userId.Value, productId);
            return Ok(cart);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _cartService.ClearCartAsync(userId.Value);
            return result ? Ok("Cart cleared") : NotFound("Cart not found");
        }

        private Guid? GetUserId()
        {
            try
            {

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : (Guid?)null;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
