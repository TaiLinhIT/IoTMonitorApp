using IoTMonitorApp.API.Dto.PaymentDto;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var payments = await _paymentService.GetByOrderIdAsync(orderId);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentDto dto)
        {
            var createdPayment = await _paymentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdPayment.Id }, createdPayment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PaymentDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Payment ID mismatch");

            var result = await _paymentService.UpdateAsync(dto);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _paymentService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _paymentService.UpdatePaymentStatusAsync(id, status);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
