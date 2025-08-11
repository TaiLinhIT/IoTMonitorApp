using IoTMonitorApp.API.Dto.Cart;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/cart
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await _cartService.GetAllAsync();
            return Ok(carts);
        }

        // GET: api/cart/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        // GET: api/cart/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var carts = await _cartService.GetCartsByUserIdAsync(userId);
            return Ok(carts);
        }

        // POST: api/cart
        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody] CartDto dto)
        {
            await _cartService.AddCartAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/cart
        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromBody] CartDto dto)
        {
            var result = await _cartService.UpdateCartAsync(dto);
            if (result == "Cart not found") return NotFound(result);
            return Ok(result);
        }

        // DELETE: api/cart/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var success = await _cartService.DeleteCartAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        // POST: api/cart/{cartId}/item
        [HttpPost("{cartId}/item")]
        public async Task<IActionResult> AddItemToCart(int cartId, [FromBody] CartItem item)
        {
            var result = await _cartService.AddItemToCartAsync(cartId, item);
            if (result.Contains("not found")) return NotFound(result);
            return Ok(result);
        }

        // DELETE: api/cart/{cartId}/item/{itemId}
        [HttpDelete("{cartId}/item/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int cartId, int itemId)
        {
            var result = await _cartService.RemoveItemFromCartAsync(cartId, itemId);
            if (result.Contains("not found")) return NotFound(result);
            return Ok(result);
        }
    }
}
