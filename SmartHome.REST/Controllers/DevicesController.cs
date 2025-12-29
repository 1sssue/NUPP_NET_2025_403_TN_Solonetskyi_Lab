using Microsoft.AspNetCore.Mvc;
using SmartHome.Common;
using SmartHome.Infrastructure.Models;
using System.Runtime.InteropServices;

namespace SmartHome.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Шлях: api/devices
    public class DevicesController : ControllerBase
    {
        private readonly ICrudServiceAsync<SmartDeviceModel> _deviceService;

        public DevicesController(ICrudServiceAsync<SmartDeviceModel> deviceService)
        {
            _deviceService = deviceService;
        }

        [cite_start]// Отримати список пристроїв із пагінацією 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SmartDeviceModel>>> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            [cite_start]// Використання асинхронного методу з пагінацією 
            var devices = await _deviceService.ReadAllAsync(page, size);
            return Ok(devices);
        }

        // Оновлення пристрою (Update)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SmartDeviceModel device)
        {
            if (id != device.Id) return BadRequest();

            var success = await _deviceService.UpdateAsync(device);
            if (!success) return NotFound();

            await _deviceService.SaveAsync();
            return Ok(device);
        }
    }
}