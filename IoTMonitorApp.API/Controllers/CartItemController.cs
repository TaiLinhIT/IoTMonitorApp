using IoTMonitorApp.API.Dto.CartItem;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        // GET: api/CartItem
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _cartItemService.GetAllAsync();
            return Ok(items);
        }

        // GET: api/CartItem/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _cartItemService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/CartItem
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartItemDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdItem = await _cartItemService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
        }

        // PUT: api/CartItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CartItemDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var success = await _cartItemService.UpdateAsync(dto);
            if (!success) return NotFound();

            return NoContent();
        }

        // DELETE: api/CartItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _cartItemService.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
