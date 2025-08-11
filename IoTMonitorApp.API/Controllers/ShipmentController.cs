using IoTMonitorApp.API.Dto.Shipment;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shipments = await _shipmentService.GetAllAsync();
            return Ok(shipments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shipment = await _shipmentService.GetByIdAsync(id);
            if (shipment == null)
                return NotFound();

            return Ok(shipment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var shipments = await _shipmentService.GetByOrderIdAsync(orderId);
            return Ok(shipments);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShipmentDto dto)
        {
            var createdShipment = await _shipmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdShipment.Id }, createdShipment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ShipmentDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Shipment ID mismatch");

            var result = await _shipmentService.UpdateAsync(dto);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _shipmentService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _shipmentService.UpdateShipmentStatusAsync(id, status);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
