using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.DTOs;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/arena")]
    [Authorize]
    public class ArenaApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public ArenaApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q)
        {
            var arenas = _context.Arenas
                .Where(a => a.DeletedAt == null &&
                            (string.IsNullOrEmpty(q) || a.Name.Contains(q) || a.City.Contains(q)))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(arenas);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var arena = _context.Arenas
                .FirstOrDefault(a => a.Id == id && a.DeletedAt == null);

            if (arena == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(arena));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Arena arena)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Arenas.Add(arena);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = arena.Id }, ToDTO(arena));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Arena arena)
        {
            if (id != arena.Id)
            {
                return BadRequest();
            }

            var existing = _context.Arenas.FirstOrDefault(a => a.Id == id && a.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = arena.Name;
            existing.City = arena.City;
            existing.Capacity = arena.Capacity;
            existing.Address = arena.Address;
            existing.IsIndoor = arena.IsIndoor;
            existing.OpenedYear = arena.OpenedYear;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(ToDTO(existing));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.Arenas.FirstOrDefault(a => a.Id == id && a.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private ArenaDTO ToDTO(Arena arena) => new ArenaDTO
        {
            Id = arena.Id,
            Name = arena.Name,
            City = arena.City,
            Capacity = arena.Capacity,
            Address = arena.Address,
            IsIndoor = arena.IsIndoor,
            OpenedYear = arena.OpenedYear,
            CreatedAt = arena.CreatedAt
        };
    }
}
