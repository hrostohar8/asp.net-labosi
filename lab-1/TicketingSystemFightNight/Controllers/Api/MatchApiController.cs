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
    [Route("api/match")]
    [Authorize]
    public class MatchApiController : ControllerBase
    {
        private readonly VjezbaDbContext _context;

        public MatchApiController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q)
        {
            var matches = _context.Matches
                .Include(m => m.Fighter1).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter1).ThenInclude(f => f.Organization)
                .Include(m => m.Fighter2).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter2).ThenInclude(f => f.Organization)
                .Include(m => m.WeightClass)
                .Include(m => m.Event).ThenInclude(e => e.Venue)
                .Include(m => m.Event).ThenInclude(e => e.Organization)
                .Where(m => m.DeletedAt == null &&
                            (string.IsNullOrEmpty(q) || m.Referee.Contains(q) || m.Status.Contains(q)))
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(matches);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var match = _context.Matches
                .Include(m => m.Fighter1).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter1).ThenInclude(f => f.Organization)
                .Include(m => m.Fighter2).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter2).ThenInclude(f => f.Organization)
                .Include(m => m.WeightClass)
                .Include(m => m.Event).ThenInclude(e => e.Venue)
                .Include(m => m.Event).ThenInclude(e => e.Organization)
                .FirstOrDefault(m => m.Id == id && m.DeletedAt == null);

            if (match == null)
            {
                return NotFound();
            }

            return Ok(ToDTO(match));
        }

        [AllowAnonymous]
        [HttpGet("byevent/{eventId}")]
        public IActionResult GetByEvent(int eventId)
        {
            var matches = _context.Matches
                .Include(m => m.Fighter1).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter1).ThenInclude(f => f.Organization)
                .Include(m => m.Fighter2).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter2).ThenInclude(f => f.Organization)
                .Include(m => m.WeightClass)
                .Include(m => m.Event).ThenInclude(e => e.Venue)
                .Include(m => m.Event).ThenInclude(e => e.Organization)
                .Where(m => m.DeletedAt == null && m.EventId == eventId)
                .ToList()
                .Select(ToDTO)
                .ToList();

            return Ok(matches);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create([FromBody] Match match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Matches.Add(match);
            _context.SaveChanges();

            var created = _context.Matches
                .Include(m => m.Fighter1).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter1).ThenInclude(f => f.Organization)
                .Include(m => m.Fighter2).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter2).ThenInclude(f => f.Organization)
                .Include(m => m.WeightClass)
                .Include(m => m.Event).ThenInclude(e => e.Venue)
                .Include(m => m.Event).ThenInclude(e => e.Organization)
                .FirstOrDefault(m => m.Id == match.Id)!;

            return CreatedAtAction(nameof(GetById), new { id = match.Id }, ToDTO(created));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(int id, [FromBody] Match match)
        {
            if (id != match.Id)
            {
                return BadRequest();
            }

            var existing = _context.Matches.FirstOrDefault(m => m.Id == id && m.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Fighter1Id = match.Fighter1Id;
            existing.Fighter2Id = match.Fighter2Id;
            existing.WeightClassId = match.WeightClassId;
            existing.EventId = match.EventId;
            existing.RoundLimit = match.RoundLimit;
            existing.Championship = match.Championship;
            existing.Referee = match.Referee;
            existing.Status = match.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            var updated = _context.Matches
                .Include(m => m.Fighter1).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter1).ThenInclude(f => f.Organization)
                .Include(m => m.Fighter2).ThenInclude(f => f.WeightClass)
                .Include(m => m.Fighter2).ThenInclude(f => f.Organization)
                .Include(m => m.WeightClass)
                .Include(m => m.Event).ThenInclude(e => e.Venue)
                .Include(m => m.Event).ThenInclude(e => e.Organization)
                .FirstOrDefault(m => m.Id == id)!;

            return Ok(ToDTO(updated));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var existing = _context.Matches.FirstOrDefault(m => m.Id == id && m.DeletedAt == null);
            if (existing == null)
            {
                return NotFound();
            }

            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();
        }

        private MatchDTO ToDTO(Match match) => new MatchDTO
        {
            Id = match.Id,
            RoundLimit = match.RoundLimit,
            Championship = match.Championship,
            Referee = match.Referee,
            Status = match.Status,
            Fighter1 = new FighterDTO
            {
                Id = match.Fighter1?.Id ?? 0,
                Name = match.Fighter1?.Name ?? string.Empty,
                Nickname = match.Fighter1?.Nickname ?? string.Empty,
                Country = match.Fighter1?.Country ?? string.Empty,
                Wins = match.Fighter1?.Wins ?? 0,
                Losses = match.Fighter1?.Losses ?? 0,
                WeightClass = new WeightClassDTO
                {
                    Id = match.Fighter1?.WeightClass?.Id ?? 0,
                    Name = match.Fighter1?.WeightClass?.Name ?? string.Empty
                },
                Organization = new FightOrganizationDTO
                {
                    Id = match.Fighter1?.Organization?.Id ?? 0,
                    Name = match.Fighter1?.Organization?.Name ?? string.Empty
                }
            },
            Fighter2 = new FighterDTO
            {
                Id = match.Fighter2?.Id ?? 0,
                Name = match.Fighter2?.Name ?? string.Empty,
                Nickname = match.Fighter2?.Nickname ?? string.Empty,
                Country = match.Fighter2?.Country ?? string.Empty,
                Wins = match.Fighter2?.Wins ?? 0,
                Losses = match.Fighter2?.Losses ?? 0,
                WeightClass = new WeightClassDTO
                {
                    Id = match.Fighter2?.WeightClass?.Id ?? 0,
                    Name = match.Fighter2?.WeightClass?.Name ?? string.Empty
                },
                Organization = new FightOrganizationDTO
                {
                    Id = match.Fighter2?.Organization?.Id ?? 0,
                    Name = match.Fighter2?.Organization?.Name ?? string.Empty
                }
            },
            WeightClass = new WeightClassDTO
            {
                Id = match.WeightClass?.Id ?? 0,
                Name = match.WeightClass?.Name ?? string.Empty
            },
            Event = new EventDTO
            {
                Id = match.Event?.Id ?? 0,
                Name = match.Event?.Name ?? string.Empty,
                City = match.Event?.City ?? string.Empty,
                Date = match.Event?.Date ?? DateTime.MinValue,
                Time = match.Event?.Time.ToString(@"hh\:mm") ?? string.Empty,
                Description = match.Event?.Description ?? string.Empty,
                BaseTicketPrice = match.Event?.BaseTicketPrice ?? 0m,
                TicketsSold = match.Event?.TicketsSold ?? 0,
                Venue = new ArenaDTO
                {
                    Id = match.Event?.Venue?.Id ?? 0,
                    Name = match.Event?.Venue?.Name ?? string.Empty,
                    City = match.Event?.Venue?.City ?? string.Empty,
                    Capacity = match.Event?.Venue?.Capacity ?? 0,
                    Address = match.Event?.Venue?.Address ?? string.Empty,
                    IsIndoor = match.Event?.Venue?.IsIndoor ?? false,
                    OpenedYear = match.Event?.Venue?.OpenedYear ?? 0,
                    CreatedAt = match.Event?.Venue?.CreatedAt ?? DateTime.MinValue
                },
                Organization = new FightOrganizationDTO
                {
                    Id = match.Event?.Organization?.Id ?? 0,
                    Name = match.Event?.Organization?.Name ?? string.Empty
                }
            }
        };
    }
}
