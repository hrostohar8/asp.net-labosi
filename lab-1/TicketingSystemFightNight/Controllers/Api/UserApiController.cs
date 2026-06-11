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
    [Route("api/user")]
    [Authorize(Roles = "Admin")]
    public class UserApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public UserApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q)
        {
            var users = _context.Users
                .Where(u => u.DeletedAt == null &&
                            (string.IsNullOrEmpty(q) || u.Name.Contains(q) || u.Email.Contains(q) || u.MemberLevel.Contains(q)))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == id && u.DeletedAt == null);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(user));
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, ToDTO(user));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var existing = _context.Users.FirstOrDefault(u => u.Id == id && u.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.Phone = user.Phone;
            existing.BirthDate = user.BirthDate;
            existing.LoyaltyPoints = user.LoyaltyPoints;
            existing.IsVip = user.IsVip;
            existing.MemberLevel = user.MemberLevel;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return Ok(ToDTO(existing));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.Users.FirstOrDefault(u => u.Id == id && u.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private UserDTO ToDTO(User user) => new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            BirthDate = user.BirthDate,
            LoyaltyPoints = user.LoyaltyPoints,
            IsVip = user.IsVip,
            MemberLevel = user.MemberLevel
        };
    }
}
