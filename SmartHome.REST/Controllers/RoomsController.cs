using Microsoft.AspNetCore.Mvc;
using SmartHome.Common;
using SmartHome.Infrastructure.Models;
using System.Runtime.InteropServices;

namespace SmartHome.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Шлях: api/rooms
    public class RoomsController : ControllerBase
    {
        private readonly ICrudServiceAsync<RoomModel> _roomService;

        public RoomsController(ICrudServiceAsync<RoomModel> roomService)
        {
            _roomService = roomService;
        }

        // Отримати всі кімнати (Read All)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomModel>>> GetAll()
        {
            var rooms = await _roomService.ReadAllAsync();
            return Ok(rooms); // Повертає 200 OK 
        }

        // Отримати кімнату за ID (Read)
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomModel>> Get(Guid id)
        {
            var room = await _roomService.ReadAsync(id);
            if (room == null) return NotFound(); // Повертає 404, якщо не знайдено 
            return Ok(room);
        }

        // Створити нову кімнату (Create)
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RoomModel room)
        {
            await _roomService.CreateAsync(room);
            await _roomService.SaveAsync();
            [cite_start]// Повертає 201 Created та шлях до нового ресурсу 
            return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
        }

        // Видалити кімнату (Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var room = await _roomService.ReadAsync(id);
            if (room == null) return NotFound();

            await _roomService.RemoveAsync(room);
            await _roomService.SaveAsync();
            return NoContent(); // Повертає 204 No Content
        }
    }
}