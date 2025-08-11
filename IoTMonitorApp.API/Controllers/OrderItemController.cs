using IoTMonitorApp.API.Dto.OrderItem;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        // GET: api/orderitem
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _orderItemService.GetAllAsync();
            return Ok(items);
        }

        // GET: api/orderitem/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _orderItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        // POST: api/orderitem
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _orderItemService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/orderitem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderItemDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id in URL and DTO do not match");

            var success = await _orderItemService.UpdateAsync(dto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/orderitem/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _orderItemService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
