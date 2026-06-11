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
    [Route("api/ticketshopmodel")]
    [Authorize]
    public class TicketShopModelApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public TicketShopModelApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q)
        {
            var offers = _context.TicketShopModels
                .Where(o => o.DeletedAt == null &&
                            (string.IsNullOrEmpty(q) || o.Title.Contains(q) || o.EventName.Contains(q) || o.Location.Contains(q) || o.Section.Contains(q)))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(offers);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var offer = _context.TicketShopModels
                .FirstOrDefault(o => o.Id == id && o.DeletedAt == null);

            if (offer == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(offer));
        }

        [HttpPost]
        public IActionResult Create([FromBody] TicketShopModels offer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TicketShopModels.Add(offer);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = offer.Id }, ToDTO(offer));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TicketShopModels offer)
        {
            if (id != offer.Id)
            {
                return BadRequest();
            }

            var existing = _context.TicketShopModels.FirstOrDefault(o => o.Id == id && o.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Title = offer.Title;
            existing.EventName = offer.EventName;
            existing.Location = offer.Location;
            existing.Section = offer.Section;
            existing.Row = offer.Row;
            existing.Seat = offer.Seat;
            existing.Price = offer.Price;
            existing.IsVip = offer.IsVip;
            existing.Description = offer.Description;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(ToDTO(existing));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.TicketShopModels.FirstOrDefault(o => o.Id == id && o.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private TicketShopModelDTO ToDTO(TicketShopModels offer) => new TicketShopModelDTO
        {
            Id = offer.Id,
            Title = offer.Title,
            EventName = offer.EventName,
            Location = offer.Location,
            Section = offer.Section,
            Row = offer.Row,
            Seat = offer.Seat,
            Price = offer.Price,
            IsVip = offer.IsVip,
            Description = offer.Description
        };
    }
}
