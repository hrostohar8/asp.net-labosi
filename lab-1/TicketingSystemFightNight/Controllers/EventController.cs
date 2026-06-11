using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly VjezbaDbContext _context;

        public EventController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()            //prikaz svih događaja, sortirano po datumu opadajuće
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

        [Authorize(Roles = "Admin,Manager")]
        [ActionName("Create")]
        public IActionResult CreateGet()   //prikaz forme za kreiranje događaja
        {
            ViewBag.ArenaList = new SelectList(_context.Arenas.Where(a => a.DeletedAt == null).ToList(), "Id", "Name");
            ViewBag.OrganizationList = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(EventCreateViewModel model)  //submittanje forme za kreiranje događaja
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

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)   //prikaz detalja događaja
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

            ViewBag.EventAttachments = await _context.EventAttachments
                .Where(a => a.EventId == id && a.DeletedAt == null)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(@event);
        }

        [Authorize(Roles = "Admin,Manager")]
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

            ViewBag.EventAttachments = await _context.EventAttachments
                .Where(a => a.EventId == id && a.DeletedAt == null)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(EventEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ArenaList = new SelectList(_context.Arenas.Where(a => a.DeletedAt == null).ToList(), "Id", "Name", model.VenueId);
                ViewBag.OrganizationList = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name", model.OrganizationId);
                ViewBag.SelectedVenue = model.VenueId > 0
                    ? await _context.Arenas.AsNoTracking().Where(a => a.Id == model.VenueId).Select(a => new { a.Id, a.Name }).FirstOrDefaultAsync()
                    : null;
                ViewBag.EventAttachments = await _context.EventAttachments
                    .Where(a => a.EventId == model.Id && a.DeletedAt == null)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();
                return View(model);
            }

            var @event = await _context.Events.FindAsync(model.Id);
            if (@event == null || @event.DeletedAt != null)
                return NotFound();

            @event.Name = model.Name;
            @event.OrganizationId = model.OrganizationId;
            @event.City = model.City;
            @event.Date = model.Date;
            @event.Time = model.Time;
            @event.VenueId = model.VenueId;
            @event.Description = model.Description;
            @event.BaseTicketPrice = model.BaseTicketPrice;
            @event.TicketsSold = model.TicketsSold;
            @event.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Događaj je uspješno ažuriran!";
            return RedirectToAction(nameof(Index));
        }

        // Upload attachment - called by Dropzone asynchronously
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UploadAttachment(int eventId, IFormFile file)
        {
            var eventEntity = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.DeletedAt == null);

            if (eventEntity == null)
                return NotFound();

            if (file == null || file.Length == 0)
                return BadRequest("Datoteka je prazna.");

            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("Datoteka je prevelika. Maksimalna veličina je 10MB.");

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".doc", ".docx", ".txt", ".zip" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return BadRequest("Tip datoteke nije dozvoljen.");

            var uploadsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot", "uploads", "events", eventId.ToString());

            Directory.CreateDirectory(uploadsPath);

            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new EventAttachment
            {
                EventId = eventId,
                FileName = uniqueFileName,
                OriginalFileName = file.FileName,
                FilePath = "/uploads/events/" + eventId + "/" + uniqueFileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                CreatedAt = DateTime.UtcNow
            };

            _context.EventAttachments.Add(attachment);
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = attachment.Id, fileName = file.FileName });
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAttachments(int eventId)
        {
            var attachments = await _context.EventAttachments
                .Where(a => a.EventId == eventId && a.DeletedAt == null)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return PartialView("_AttachmentList", attachments);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            var attachment = await _context.EventAttachments
                .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null);

            if (attachment == null)
                return NotFound();

            var physicalPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                attachment.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            attachment.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SearchEvents(string term)  // This method is used for AJAX search in dropdowns
        {
            try
            {
                var q = _context.Events.AsNoTracking().Where(e => e.DeletedAt == null);
                if (!string.IsNullOrWhiteSpace(term))
                {
                    var t = term.Trim().ToLower();
                    q = q.Where(e => (e.Name != null && e.Name.ToLower().Contains(t))
                                  || (e.City != null && e.City.ToLower().Contains(t)));
                }

                var events = await q
                    .Include(e => e.Venue)
                    .Include(e => e.Organization)
                    .Include(e => e.Matches)
                    .OrderByDescending(e => e.Date)
                    .Take(50)
                    .ToListAsync();

                var results = events.Select(e => new {
                    Id = e.Id,
                    Text = e.Name,
                    Description = e.Name + " | " + e.Date.ToString("dd.MM.yyyy")
                        + (e.Venue != null ? " | " + e.Venue.Name : "")
                        + (e.Organization != null ? " | " + e.Organization.Name : ""),
                    City = e.City,
                    Date = e.Date.ToString("dd.MM.yyyy"),
                    Time = e.Time.ToString(@"hh\:mm"),
                    Venue = e.Venue != null ? e.Venue.Name : "",
                    Organization = e.Organization != null ? e.Organization.Name : "",
                    TicketsSold = e.TicketsSold,
                    MatchesCount = e.Matches != null ? e.Matches.Count : 0
                });

                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new[] { new { Id = 0, Text = "", Description = "Greška pri pretrazi: " + ex.Message } });
            }
        }
    }
}
