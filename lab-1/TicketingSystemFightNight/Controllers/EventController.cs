using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    public class EventController : Controller
    {
        private readonly VjezbaDbContext _context;

        public EventController(VjezbaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Where(e => e.DeletedAt == null)
                .Include(e => e.Venue)
                .Include(e => e.Organization)
                .Include(e => e.Matches)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return View(events);
        }

        [ActionName("Create")]
        public IActionResult CreateGet()
        {
            ViewBag.ArenaList = new SelectList(_context.Arenas.Where(a => a.DeletedAt == null).ToList(), "Id", "Name");
            ViewBag.OrganizationList = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(EventCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ArenaList = new SelectList(_context.Arenas.Where(a => a.DeletedAt == null).ToList(), "Id", "Name", model.VenueId);
                ViewBag.OrganizationList = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name", model.OrganizationId);
                ViewBag.SelectedVenue = model.VenueId > 0
                    ? await _context.Arenas.AsNoTracking().Where(a => a.Id == model.VenueId).Select(a => new { a.Id, a.Name }).FirstOrDefaultAsync()
                    : null;
                return View(model);
            }

            var eventEntity = new Event
            {
                Name = model.Name,
                OrganizationId = model.OrganizationId,
                City = model.City,
                Date = model.Date,
                Time = model.Time,
                VenueId = model.VenueId,
                Description = model.Description,
                BaseTicketPrice = model.BaseTicketPrice,
                TicketsSold = model.TicketsSold,
                CreatedAt = DateTime.UtcNow
            };

            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Događaj je uspješno kreiran!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var @event = await _context.Events
                .Where(e => e.DeletedAt == null)
                .Include(e => e.Venue)
                .Include(e => e.Organization)
                .Include(e => e.Matches).ThenInclude(m => m.Fighter1)
                .Include(e => e.Matches).ThenInclude(m => m.Fighter2)
                .Include(e => e.Tickets)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
                return NotFound();

            return View(@event);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> EditGet(int id)
        {
            var @event = await _context.Events
                .Where(e => e.DeletedAt == null)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
                return NotFound();

            ViewBag.ArenaList = new SelectList(_context.Arenas.Where(a => a.DeletedAt == null).ToList(), "Id", "Name", @event.VenueId);
            var arena = await _context.Arenas.AsNoTracking().FirstOrDefaultAsync(a => a.Id == @event.VenueId);
            ViewBag.SelectedVenue = arena != null ? new { Id = arena.Id, Text = arena.Name } : null;
            ViewBag.OrganizationList = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name", @event.OrganizationId);

            var model = new EventEditViewModel
            {
                Id = @event.Id,
                Name = @event.Name,
                OrganizationId = @event.OrganizationId,
                City = @event.City,
                Date = @event.Date,
                Time = @event.Time,
                VenueId = @event.VenueId,
                Description = @event.Description,
                BaseTicketPrice = @event.BaseTicketPrice,
                TicketsSold = @event.TicketsSold
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null || @event.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                @event,
                "",
                e => e.Name,
                e => e.OrganizationId,
                e => e.City,
                e => e.Date,
                e => e.Time,
                e => e.VenueId,
                e => e.Description,
                e => e.BaseTicketPrice,
                e => e.TicketsSold);

            if (!ok || !ModelState.IsValid)
            {
                ViewBag.ArenaList = new SelectList(_context.Arenas.Where(a => a.DeletedAt == null).ToList(), "Id", "Name", @event.VenueId);
                ViewBag.OrganizationList = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name", @event.OrganizationId);
                ViewBag.SelectedVenue = @event.VenueId > 0
                    ? await _context.Arenas.AsNoTracking().Where(a => a.Id == @event.VenueId).Select(a => new { a.Id, a.Name }).FirstOrDefaultAsync()
                    : null;
                var model = new EventEditViewModel
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    OrganizationId = @event.OrganizationId,
                    City = @event.City,
                    Date = @event.Date,
                    Time = @event.Time,
                    VenueId = @event.VenueId,
                    Description = @event.Description,
                    BaseTicketPrice = @event.BaseTicketPrice,
                    TicketsSold = @event.TicketsSold
                };
                return View(model);
            }

            @event.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Događaj je uspješno ažuriran!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return NotFound();

            @event.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Događaj je uspješno obrisan!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchEvents(string term)
        {
            try
            {
                var q = _context.Events.AsNoTracking().Where(e => e.DeletedAt == null);
                if (!string.IsNullOrWhiteSpace(term))
                {
                    var t = term.Trim().ToLower();
                    q = q.Where(e => e.Name.ToLower().Contains(t) || e.City.ToLower().Contains(t));
                }

                var results = await q
                    .Include(e => e.Venue)
                    .Include(e => e.Organization)
                    .OrderByDescending(e => e.Date)
                    .Take(10)
                    .Select(e => new {
                        Id = e.Id,
                        Text = e.Name,
                        Description = e.Name + " | " + e.Date.ToString()
                            + (e.Venue != null ? " | " + e.Venue.Name : "")
                            + (e.Organization != null ? " | " + e.Organization.Name : "")
                    })
                    .ToListAsync();

                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new[] { new { Id = 0, Text = "", Description = "Greška pri pretrazi: " + ex.Message } });
            }
        }
    }
}
