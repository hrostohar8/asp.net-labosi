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
    [Route("api/weightclass")]
    [Authorize]
    public class WeightClassApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public WeightClassApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q)
        {
            var weightClasses = _context.WeightClasses
                .Where(w => w.DeletedAt == null &&
                            (string.IsNullOrEmpty(q) || w.Name.Contains(q)))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(weightClasses);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var weightClass = _context.WeightClasses
                .FirstOrDefault(w => w.Id == id && w.DeletedAt == null);

            if (weightClass == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(weightClass));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create([FromBody] WeightClass weightClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WeightClasses.Add(weightClass);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = weightClass.Id }, ToDTO(weightClass));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(int id, [FromBody] WeightClass weightClass)
        {
            if (id != weightClass.Id)
            {
                return BadRequest();
            }

            var existing = _context.WeightClasses.FirstOrDefault(w => w.Id == id && w.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = weightClass.Name;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return Ok(ToDTO(existing));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var existing = _context.WeightClasses.FirstOrDefault(w => w.Id == id && w.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private WeightClassDTO ToDTO(WeightClass weightClass) => new WeightClassDTO
        {
            Id = weightClass.Id,
            Name = weightClass.Name
        };
    }
}
