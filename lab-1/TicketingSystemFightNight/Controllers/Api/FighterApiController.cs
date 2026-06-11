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
    [Route("api/fighter")]
    [Authorize]
    public class FighterApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public FighterApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q)
        {
            var fighters = _context.Fighters
                .Include(f => f.WeightClass)
                .Include(f => f.Organization)
                .Where(f => f.DeletedAt == null &&
                            (string.IsNullOrEmpty(q) || f.Name.Contains(q) || f.Nickname.Contains(q) || f.Country.Contains(q)))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(fighters);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var fighter = _context.Fighters
                .Include(f => f.WeightClass)
                .Include(f => f.Organization)
                .Include(f => f.MatchesAsFighter1)
                .Include(f => f.MatchesAsFighter2)
                .FirstOrDefault(f => f.Id == id && f.DeletedAt == null);

            if (fighter == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(fighter));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create([FromBody] Fighter fighter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Fighters.Add(fighter);
            _context.SaveChanges();

            var created = _context.Fighters
                .Include(f => f.WeightClass)
                .Include(f => f.Organization)
                .FirstOrDefault(f => f.Id == fighter.Id)!;

            return CreatedAtAction(nameof(GetById), new { id = fighter.Id }, ToDTO(created));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(int id, [FromBody] Fighter fighter)
        {
            if (id != fighter.Id)
            {
                return BadRequest();
            }

            var existing = _context.Fighters.FirstOrDefault(f => f.Id == id && f.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = fighter.Name;
            existing.Nickname = fighter.Nickname;
            existing.WeightClassId = fighter.WeightClassId;
            existing.OrganizationId = fighter.OrganizationId;
            existing.Country = fighter.Country;
            existing.Wins = fighter.Wins;
            existing.Losses = fighter.Losses;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            var updated = _context.Fighters
                .Include(f => f.WeightClass)
                .Include(f => f.Organization)
                .FirstOrDefault(f => f.Id == id)!;

            return Ok(ToDTO(updated));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var existing = _context.Fighters.FirstOrDefault(f => f.Id == id && f.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private FighterDTO ToDTO(Fighter fighter) => new FighterDTO
        {
            Id = fighter.Id,
            Name = fighter.Name,
            Nickname = fighter.Nickname,
            Country = fighter.Country,
            Wins = fighter.Wins,
            Losses = fighter.Losses,
            WeightClass = new WeightClassDTO
            {
                Id = fighter.WeightClass?.Id ?? 0,
                Name = fighter.WeightClass?.Name ?? string.Empty
            },
            Organization = new FightOrganizationDTO
            {
                Id = fighter.Organization?.Id ?? 0,
                Name = fighter.Organization?.Name ?? string.Empty
            }
        };
    }
}
