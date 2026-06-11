using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    [Authorize]
    public class MatchController : Controller
    {
        private readonly VjezbaDbContext _context;

        public MatchController(VjezbaDbContext context)
        {
            _context = context;
        }

        private void PopulateMatchDropdowns(int? selectedEventId = null, int? selectedWeightClassId = null, int? selectedFighterId = null)
        {
            ViewBag.Events = new SelectList(_context.Events.Where(e => e.DeletedAt == null).ToList(), "Id", "Name", selectedEventId);
            ViewBag.FighterList = _context.Fighters
                .Where(f => f.DeletedAt == null)
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name })
                .ToList();
            ViewBag.WeightClasses = new SelectList(_context.WeightClasses.Where(w => w.DeletedAt == null).ToList(), "Id", "Name", selectedWeightClassId);
            ViewBag.FightersJson = _context.Fighters
                .Where(f => f.DeletedAt == null)
                .Select(f => new { f.Id, f.Name, f.WeightClassId })
                .ToList();
        }

        private async Task PopulateMatchAutocompleteSelections(int? fighter1Id, int? fighter2Id, int? eventId)
        {
            ViewBag.SelectedFighter1 = fighter1Id.HasValue && fighter1Id.Value > 0
                ? await _context.Fighters.AsNoTracking().Where(f => f.Id == fighter1Id.Value).Select(f => new { f.Id, f.Name }).FirstOrDefaultAsync()
                : null;
            ViewBag.SelectedFighter2 = fighter2Id.HasValue && fighter2Id.Value > 0
                ? await _context.Fighters.AsNoTracking().Where(f => f.Id == fighter2Id.Value).Select(f => new { f.Id, f.Name }).FirstOrDefaultAsync()
                : null;
            ViewBag.SelectedEvent = eventId.HasValue && eventId.Value > 0
                ? await _context.Events.AsNoTracking().Where(e => e.Id == eventId.Value).Select(e => new { e.Id, e.Name }).FirstOrDefaultAsync()
                : null;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var matches = await _context.Matches
                .Where(m => m.DeletedAt == null)
                .Include(m => m.Fighter1)
                .Include(m => m.Fighter2)
                .Include(m => m.Event)
                .Include(m => m.WeightClass)
                .OrderBy(m => m.Event.Date)
                .ToListAsync();

            return View(matches);
        }

        [ActionName("Create")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult CreateGet()
        {
            PopulateMatchDropdowns();
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(MatchCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateMatchDropdowns(model.EventId, model.WeightClassId, model.Fighter1Id);
                await PopulateMatchAutocompleteSelections(model.Fighter1Id, model.Fighter2Id, model.EventId);
                return View(model);
            }

            var match = new Match
            {
                Fighter1Id = model.Fighter1Id,
                Fighter2Id = model.Fighter2Id,
                WeightClassId = model.WeightClassId,
                EventId = model.EventId,
                RoundLimit = model.RoundLimit,
                Championship = model.Championship,
                Referee = model.Referee,
                Status = model.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Meč je uspješno kreiran!";
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var match = await _context.Matches
                .Where(m => m.DeletedAt == null)
                .Include(m => m.Fighter1)
                .Include(m => m.Fighter2)
                .Include(m => m.Event)
                .Include(m => m.WeightClass)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
                return NotFound();

            return View(match);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ApiStats(int id)
        {
            var match = await _context.Matches
                .Where(m => m.DeletedAt == null)
                .Include(m => m.Fighter1)
                .Include(m => m.Fighter2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null) return NotFound();

            var fighters = new[] { match.Fighter1, match.Fighter2 }.Select(f => new {
                name = f.Name,
                // height/weight/reach not available in model - provide zeros
                height = 0,
                weight = 0,
                reach = 0,
                wins = f.Wins,
                losses = f.Losses
            }).ToList();

            var weightMap = await _context.WeightClasses.ToDictionaryAsync(w => w.Id, w => w.Name);
            var distributionRaw = await _context.Matches
                .Where(m => m.DeletedAt == null)
                .GroupBy(m => m.WeightClassId)
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .ToListAsync();

            var weightDistribution = distributionRaw.Select(d => new { Label = weightMap.ContainsKey(d.Key) ? weightMap[d.Key] : "Unknown", Value = d.Count }).ToList();

            return Json(new { fighters, weightDistribution });
        }

        [Authorize(Roles = "Admin,Manager")]
        [ActionName("Edit")]
        public async Task<IActionResult> EditGet(int id)
        {
            var match = await _context.Matches
                .Where(m => m.DeletedAt == null)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
                return NotFound();

            PopulateMatchDropdowns(match.EventId, match.WeightClassId, match.Fighter1Id);

            // Provide selected display values for autocomplete partials
            var f1 = await _context.Fighters.AsNoTracking().FirstOrDefaultAsync(f => f.Id == match.Fighter1Id);
            var f2 = await _context.Fighters.AsNoTracking().FirstOrDefaultAsync(f => f.Id == match.Fighter2Id);
            var ev = await _context.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == match.EventId);
            ViewBag.SelectedFighter1 = f1 != null ? new { Id = f1.Id, Text = f1.Name } : null;
            ViewBag.SelectedFighter2 = f2 != null ? new { Id = f2.Id, Text = f2.Name } : null;
            ViewBag.SelectedEvent = ev != null ? new { Id = ev.Id, Text = ev.Name } : null;

            var model = new MatchEditViewModel
            {
                Id = match.Id,
                Fighter1Id = match.Fighter1Id,
                Fighter2Id = match.Fighter2Id,
                WeightClassId = match.WeightClassId,
                EventId = match.EventId,
                RoundLimit = match.RoundLimit,
                Championship = match.Championship,
                Referee = match.Referee,
                Status = match.Status
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null || match.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                match,
                "",
                m => m.Fighter1Id,
                m => m.Fighter2Id,
                m => m.WeightClassId,
                m => m.EventId,
                m => m.RoundLimit,
                m => m.Championship,
                m => m.Referee,
                m => m.Status);

            if (!ok || !ModelState.IsValid)
            {
                PopulateMatchDropdowns(match.EventId, match.WeightClassId, match.Fighter1Id);
                await PopulateMatchAutocompleteSelections(match.Fighter1Id, match.Fighter2Id, match.EventId);

                var model = new MatchEditViewModel
                {
                    Id = match.Id,
                    Fighter1Id = match.Fighter1Id,
                    Fighter2Id = match.Fighter2Id,
                    WeightClassId = match.WeightClassId,
                    EventId = match.EventId,
                    RoundLimit = match.RoundLimit,
                    Championship = match.Championship,
                    Referee = match.Referee,
                    Status = match.Status
                };

                return View(model);
            }

            match.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Meč je uspješno ažuriran!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                return NotFound();

            match.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Meč je uspješno obrisan!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchMatches(string term)
        {
            try
            {
                 IQueryable<Match> q = _context.Matches.AsNoTracking().Where(m => m.DeletedAt == null);
                 q = q.Include(m => m.Fighter1)
                     .Include(m => m.Fighter2)
                     .Include(m => m.Event)
                     .Include(m => m.WeightClass);

                if (!string.IsNullOrWhiteSpace(term))
                {
                    var t = term.Trim().ToLower();
                    q = q.Where(m => (m.Fighter1 != null && m.Fighter1.Name != null && m.Fighter1.Name.ToLower().Contains(t))
                                  || (m.Fighter2 != null && m.Fighter2.Name != null && m.Fighter2.Name.ToLower().Contains(t))
                                  || (m.Event != null && m.Event.Name != null && m.Event.Name.ToLower().Contains(t))
                                  || (m.Status != null && m.Status.ToLower().Contains(t)) );
                }

                var matches = await q.OrderBy(m => m.Event.Date).Take(50).ToListAsync();

                var results = matches.Select(m => new {
                    Id = m.Id,
                    Title = (m.Fighter1 != null ? m.Fighter1.Name : "") + " vs " + (m.Fighter2 != null ? m.Fighter2.Name : ""),
                    Fighter1 = m.Fighter1 != null ? m.Fighter1.Name : "",
                    Fighter2 = m.Fighter2 != null ? m.Fighter2.Name : "",
                    WeightClass = m.WeightClass != null ? m.WeightClass.Name : "",
                    Event = m.Event != null ? m.Event.Name : "",
                    RoundLimit = m.RoundLimit,
                    Championship = m.Championship,
                    Referee = m.Referee,
                    Status = m.Status
                });

                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new[] { new { Id = 0, Title = "", Description = "Greška pri pretrazi: " + ex.Message } });
            }
        }
    }
}
