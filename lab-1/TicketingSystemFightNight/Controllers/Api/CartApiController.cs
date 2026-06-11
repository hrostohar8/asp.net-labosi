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
    [Route("api/cart")]
    [Authorize]
    public class CartApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public CartApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] int? userId)
        {
            var carts = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Tickets).ThenInclude(t => t.Event).ThenInclude(e => e.Venue)
                .Include(c => c.Tickets).ThenInclude(t => t.Event).ThenInclude(e => e.Organization)
                .Where(c => c.DeletedAt == null && (!userId.HasValue || c.UserId == userId.Value))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(carts);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cart = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Tickets).ThenInclude(t => t.Event).ThenInclude(e => e.Venue)
                .Include(c => c.Tickets).ThenInclude(t => t.Event).ThenInclude(e => e.Organization)
                .FirstOrDefault(c => c.Id == id && c.DeletedAt == null);

            if (cart == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(cart));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create([FromBody] Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Carts.Add(cart);
            _context.SaveChanges();

            var created = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Tickets).ThenInclude(t => t.Event).ThenInclude(e => e.Venue)
                .Include(c => c.Tickets).ThenInclude(t => t.Event).ThenInclude(e => e.Organization)
                .FirstOrDefault(c => c.Id == cart.Id)!;

            return CreatedAtAction(nameof(GetById), new { id = cart.Id }, ToDTO(created));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(int id, [FromBody] Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            var existing = _context.Carts.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.UserId = cart.UserId;
            existing.DiscountCode = cart.DiscountCode;
            existing.DiscountPercent = cart.DiscountPercent;
            existing.IsPaid = cart.IsPaid;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            var updated = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Tickets).ThenInclude(t => t.Event).ThenInclude(e => e.Venue)
                .Include(c => c.Tickets).ThenInclude(t => t.Event).ThenInclude(e => e.Organization)
                .FirstOrDefault(c => c.Id == id)!;

            return Ok(ToDTO(updated));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var existing = _context.Carts.FirstOrDefault(c => c.Id == id && c.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private CartDTO ToDTO(Cart cart)
        {
            var tickets = cart.Tickets
                .Select(t => new TicketDTO
                {
                    Id = t.Id,
                    Section = t.Section,
                    Row = t.Row,
                    Seat = t.Seat,
                    Price = t.Price,
                    PurchaseDate = t.PurchaseDate,
                    IsVip = t.IsVip,
                    Event = new EventDTO
                    {
                        Id = t.Event?.Id ?? 0,
                        Name = t.Event?.Name ?? string.Empty,
                        City = t.Event?.City ?? string.Empty,
                        Date = t.Event?.Date ?? DateTime.MinValue,
                        Time = t.Event?.Time.ToString(@"hh\:mm") ?? string.Empty,
                        Description = t.Event?.Description ?? string.Empty,
                        BaseTicketPrice = t.Event?.BaseTicketPrice ?? 0m,
                        TicketsSold = t.Event?.TicketsSold ?? 0,
                        Venue = new ArenaDTO
                        {
                            Id = t.Event?.Venue?.Id ?? 0,
                            Name = t.Event?.Venue?.Name ?? string.Empty,
                            City = t.Event?.Venue?.City ?? string.Empty,
                            Capacity = t.Event?.Venue?.Capacity ?? 0,
                            Address = t.Event?.Venue?.Address ?? string.Empty,
                            IsIndoor = t.Event?.Venue?.IsIndoor ?? false,
                            OpenedYear = t.Event?.Venue?.OpenedYear ?? 0,
                            CreatedAt = t.Event?.Venue?.CreatedAt ?? DateTime.MinValue
                        },
                        Organization = new FightOrganizationDTO
                        {
                            Id = t.Event?.Organization?.Id ?? 0,
                            Name = t.Event?.Organization?.Name ?? string.Empty
                        }
                    }
                })
                .ToList();

            return new CartDTO
            {
                Id = cart.Id,
                DiscountCode = cart.DiscountCode,
                DiscountPercent = cart.DiscountPercent,
                IsPaid = cart.IsPaid,
                TotalPrice = tickets.Sum(t => t.Price) * (1 - cart.DiscountPercent / 100m),
                User = new UserDTO
                {
                    Id = cart.User.Id,
                    Name = cart.User.Name,
                    Email = cart.User.Email,
                    Phone = cart.User.Phone,
                    BirthDate = cart.User.BirthDate,
                    LoyaltyPoints = cart.User.LoyaltyPoints,
                    IsVip = cart.User.IsVip,
                    MemberLevel = cart.User.MemberLevel
                },
                Tickets = tickets
            };
        }
    }
}
