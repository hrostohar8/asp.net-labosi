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
    [Route("api/ticket")]
    [Authorize]
    public class TicketApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public TicketApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q)
        {
            var tickets = _context.Tickets
                .Include(t => t.Event).ThenInclude(e => e.Venue)
                .Include(t => t.Event).ThenInclude(e => e.Organization)
                .Where(t => t.DeletedAt == null &&
                            (string.IsNullOrEmpty(q) || t.Section.Contains(q) || t.Seat.Contains(q)))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(tickets);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var ticket = _context.Tickets
                .Include(t => t.Event).ThenInclude(e => e.Venue)
                .Include(t => t.Event).ThenInclude(e => e.Organization)
                .Include(t => t.Cart)
                .FirstOrDefault(t => t.Id == id && t.DeletedAt == null);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(ticket));
        }

        [AllowAnonymous]
        [HttpGet("byevent/{eventId}")]
        public IActionResult GetByEvent(int eventId)
        {
            var tickets = _context.Tickets
                .Include(t => t.Event).ThenInclude(e => e.Venue)
                .Include(t => t.Event).ThenInclude(e => e.Organization)
                .Where(t => t.DeletedAt == null && t.EventId == eventId)
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(tickets);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create([FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            var created = _context.Tickets
                .Include(t => t.Event).ThenInclude(e => e.Venue)
                .Include(t => t.Event).ThenInclude(e => e.Organization)
                .FirstOrDefault(t => t.Id == ticket.Id)!;

            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ToDTO(created));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(int id, [FromBody] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            var existing = _context.Tickets.FirstOrDefault(t => t.Id == id && t.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.EventId = ticket.EventId;
            existing.CartId = ticket.CartId;
            existing.Section = ticket.Section;
            existing.Row = ticket.Row;
            existing.Seat = ticket.Seat;
            existing.Price = ticket.Price;
            existing.PurchaseDate = ticket.PurchaseDate;
            existing.IsVip = ticket.IsVip;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            var updated = _context.Tickets
                .Include(t => t.Event).ThenInclude(e => e.Venue)
                .Include(t => t.Event).ThenInclude(e => e.Organization)
                .FirstOrDefault(t => t.Id == id)!;

            return Ok(ToDTO(updated));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var existing = _context.Tickets.FirstOrDefault(t => t.Id == id && t.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private TicketDTO ToDTO(Ticket ticket) => new TicketDTO
        {
            Id = ticket.Id,
            Section = ticket.Section,
            Row = ticket.Row,
            Seat = ticket.Seat,
            Price = ticket.Price,
            PurchaseDate = ticket.PurchaseDate,
            IsVip = ticket.IsVip,
            Event = new EventDTO
            {
                Id = ticket.Event?.Id ?? 0,
                Name = ticket.Event?.Name ?? string.Empty,
                City = ticket.Event?.City ?? string.Empty,
                Date = ticket.Event?.Date ?? DateTime.MinValue,
                Time = ticket.Event?.Time.ToString(@"hh\:mm") ?? string.Empty,
                Description = ticket.Event?.Description ?? string.Empty,
                BaseTicketPrice = ticket.Event?.BaseTicketPrice ?? 0m,
                TicketsSold = ticket.Event?.TicketsSold ?? 0,
                Venue = new ArenaDTO
                {
                    Id = ticket.Event?.Venue?.Id ?? 0,
                    Name = ticket.Event?.Venue?.Name ?? string.Empty,
                    City = ticket.Event?.Venue?.City ?? string.Empty,
                    Capacity = ticket.Event?.Venue?.Capacity ?? 0,
                    Address = ticket.Event?.Venue?.Address ?? string.Empty,
                    IsIndoor = ticket.Event?.Venue?.IsIndoor ?? false,
                    OpenedYear = ticket.Event?.Venue?.OpenedYear ?? 0,
                    CreatedAt = ticket.Event?.Venue?.CreatedAt ?? DateTime.MinValue
                },
                Organization = new FightOrganizationDTO
                {
                    Id = ticket.Event?.Organization?.Id ?? 0,
                    Name = ticket.Event?.Organization?.Name ?? string.Empty
                }
            }
        };
    }
}
