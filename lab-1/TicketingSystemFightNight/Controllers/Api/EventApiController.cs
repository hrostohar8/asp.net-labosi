using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.DTOs;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/event")]
    [Authorize]
    public class EventApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public EventApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q)
        {
            var events = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Organization)
                .Where(e => e.DeletedAt == null &&
                            (string.IsNullOrEmpty(q) || e.Name.Contains(q) || e.City.Contains(q) || e.Description.Contains(q)))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(events);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var @event = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Organization)
                .Include(e => e.Matches)
                .FirstOrDefault(e => e.Id == id && e.DeletedAt == null);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(@event));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Events.Add(@event);
            _context.SaveChanges();

            var created = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Organization)
                .FirstOrDefault(e => e.Id == @event.Id)!;

            return CreatedAtAction(nameof(GetById), new { id = @event.Id }, ToDTO(created));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            var existing = _context.Events.FirstOrDefault(e => e.Id == id && e.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = @event.Name;
            existing.OrganizationId = @event.OrganizationId;
            existing.City = @event.City;
            existing.Date = @event.Date;
            existing.Time = @event.Time;
            existing.VenueId = @event.VenueId;
            existing.Description = @event.Description;
            existing.BaseTicketPrice = @event.BaseTicketPrice;
            existing.TicketsSold = @event.TicketsSold;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            var updated = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Organization)
                .FirstOrDefault(e => e.Id == id)!;

            return Ok(ToDTO(updated));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.Events.FirstOrDefault(e => e.Id == id && e.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private EventDTO ToDTO(Event @event) => new EventDTO
        {
            Id = @event.Id,
            Name = @event.Name,
            City = @event.City,
            Date = @event.Date,
            Time = @event.Time.ToString(@"hh\:mm"),
            Description = @event.Description,
            BaseTicketPrice = @event.BaseTicketPrice,
            TicketsSold = @event.TicketsSold,
            Venue = new ArenaDTO
            {
                Id = @event.Venue?.Id ?? 0,
                Name = @event.Venue?.Name ?? string.Empty,
                City = @event.Venue?.City ?? string.Empty,
                Capacity = @event.Venue?.Capacity ?? 0,
                Address = @event.Venue?.Address ?? string.Empty,
                IsIndoor = @event.Venue?.IsIndoor ?? false,
                OpenedYear = @event.Venue?.OpenedYear ?? 0,
                CreatedAt = @event.Venue?.CreatedAt ?? DateTime.MinValue
            },
            Organization = new FightOrganizationDTO
            {
                Id = @event.Organization?.Id ?? 0,
                Name = @event.Organization?.Name ?? string.Empty
            }
        };
    }
}
