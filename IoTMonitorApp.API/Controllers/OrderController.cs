using IoTMonitorApp.API.Dto.Order;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/order
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // GET: api/order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto dto)
        {
            var createdOrder = await _orderService.CreateAsync(dto);

            // giả sử CreateAsync trả về OrderDto sau khi tạo thành công
            return CreatedAtAction(
                nameof(GetById),
                new { id = createdOrder.Id },
                createdOrder
            );
        }


        // PUT: api/order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderDto dto)
        {
            if (id != dto.Id) return BadRequest("Order ID không khớp");

            var updated = await _orderService.UpdateAsync(dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _orderService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
