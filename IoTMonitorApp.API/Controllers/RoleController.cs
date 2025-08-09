using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }
    }
}
