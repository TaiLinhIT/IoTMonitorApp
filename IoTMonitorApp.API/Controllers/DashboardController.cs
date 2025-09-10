using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet]
        public IActionResult GetDashboard()
        {
            _dashboardService.GetDashboard();
            return Ok(new { message = "Dashboard data" });
        }
    }
}
