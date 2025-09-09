using IoTMonitorApp.API.Dto.Checkout;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IoTMonitorApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutDraftController : ControllerBase
    {
        private readonly ICheckoutDraftService _checkoutDraftService;

        public CheckoutDraftController(ICheckoutDraftService checkoutDraftService)
        {
            _checkoutDraftService = checkoutDraftService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CheckoutDraftCreateDto dto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            if (dto == null || dto.Items == null || dto.Items.Count == 0)
                return BadRequest("Dữ liệu không hợp lệ");

            var draft = await _checkoutDraftService.CreateDraftAsync(dto, userId);
            return Ok(new { id = draft.Id });
        }

        [HttpGet("{draftId}")]
        public async Task<IActionResult> Get(int draftId)
        {
            return Ok(new { id = draftId });
        }
    }
}
