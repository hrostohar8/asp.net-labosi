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
    [Route("api/fightorganization")]
    [Authorize]
    public class FightOrganizationApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public FightOrganizationApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q)
        {
            var organizations = _context.FightOrganizations
                .Where(o => o.DeletedAt == null &&
                            (string.IsNullOrEmpty(q) || o.Name.Contains(q)))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(organizations);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var organization = _context.FightOrganizations
                .FirstOrDefault(o => o.Id == id && o.DeletedAt == null);

            if (organization == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(organization));
        }

        [HttpPost]
        public IActionResult Create([FromBody] FightOrganization organization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FightOrganizations.Add(organization);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = organization.Id }, ToDTO(organization));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FightOrganization organization)
        {
            if (id != organization.Id)
            {
                return BadRequest();
            }

            var existing = _context.FightOrganizations.FirstOrDefault(o => o.Id == id && o.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = organization.Name;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return Ok(ToDTO(existing));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.FightOrganizations.FirstOrDefault(o => o.Id == id && o.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private FightOrganizationDTO ToDTO(FightOrganization organization) => new FightOrganizationDTO
        {
            Id = organization.Id,
            Name = organization.Name
        };
    }
}
