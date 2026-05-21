using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Helpers;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    public class FighterController : Controller
    {
        private readonly VjezbaDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FighterController(VjezbaDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var fighters = await _context.Fighters
                .Where(f => f.DeletedAt == null)
                .Include(f => f.Organization)
                .Include(f => f.WeightClass)
                .OrderBy(f => f.Name)
                .ToListAsync();

            foreach (var fighter in fighters)
            {
                fighter.ImageUrl = FighterImageHelper.GetFighterImageUrl(_environment, fighter);
            }

            return View(fighters);
        }

        [ActionName("Create")]
        public IActionResult CreateGet()
        {
            ViewBag.Organizations = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name");
            ViewBag.WeightClasses = new SelectList(_context.WeightClasses.Where(w => w.DeletedAt == null).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(FighterCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Organizations = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name", model.OrganizationId);
                ViewBag.WeightClasses = new SelectList(_context.WeightClasses.Where(w => w.DeletedAt == null).ToList(), "Id", "Name", model.WeightClassId);
                return View(model);
            }

            var fighter = new Fighter
            {
                Name = model.Name,
                Nickname = model.Nickname,
                WeightClassId = model.WeightClassId,
                OrganizationId = model.OrganizationId,
                Country = model.Country,
                Wins = model.Wins,
                Losses = model.Losses,
                CreatedAt = DateTime.UtcNow
            };

            _context.Fighters.Add(fighter);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Borac je uspješno kreiran!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var fighter = await _context.Fighters
                .Where(f => f.DeletedAt == null)
                .Include(f => f.Organization)
                .Include(f => f.WeightClass)
                .Include(f => f.MatchesAsFighter1)
                .Include(f => f.MatchesAsFighter2)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fighter == null)
                return NotFound();

            fighter.ImageUrl = FighterImageHelper.GetFighterImageUrl(_environment, fighter);
            return View(fighter);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> EditGet(int id)
        {
            var fighter = await _context.Fighters
                .Where(f => f.DeletedAt == null)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fighter == null)
                return NotFound();

            ViewBag.Organizations = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name", fighter.OrganizationId);
            ViewBag.WeightClasses = new SelectList(_context.WeightClasses.Where(w => w.DeletedAt == null).ToList(), "Id", "Name", fighter.WeightClassId);

            var model = new FighterEditViewModel
            {
                Id = fighter.Id,
                Name = fighter.Name,
                Nickname = fighter.Nickname,
                WeightClassId = fighter.WeightClassId,
                OrganizationId = fighter.OrganizationId,
                Country = fighter.Country,
                Wins = fighter.Wins,
                Losses = fighter.Losses
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var fighter = await _context.Fighters.FindAsync(id);
            if (fighter == null || fighter.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                fighter,
                "",
                f => f.Name,
                f => f.Nickname,
                f => f.WeightClassId,
                f => f.OrganizationId,
                f => f.Country,
                f => f.Wins,
                f => f.Losses);

            if (!ok || !ModelState.IsValid)
            {
                ViewBag.Organizations = new SelectList(_context.FightOrganizations.Where(o => o.DeletedAt == null).ToList(), "Id", "Name", fighter.OrganizationId);
                ViewBag.WeightClasses = new SelectList(_context.WeightClasses.Where(w => w.DeletedAt == null).ToList(), "Id", "Name", fighter.WeightClassId);

                var model = new FighterEditViewModel
                {
                    Id = fighter.Id,
                    Name = fighter.Name,
                    Nickname = fighter.Nickname,
                    WeightClassId = fighter.WeightClassId,
                    OrganizationId = fighter.OrganizationId,
                    Country = fighter.Country,
                    Wins = fighter.Wins,
                    Losses = fighter.Losses
                };

                return View(model);
            }

            fighter.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Borac je uspješno ažuriran!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var fighter = await _context.Fighters.FindAsync(id);
            if (fighter == null)
                return NotFound();

            fighter.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Borac je uspješno obrisan!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchFighters(string term, int? weightClassId)
        {
            try
            {
                var q = _context.Fighters.AsNoTracking().Where(f => f.DeletedAt == null);
                if (!string.IsNullOrWhiteSpace(term))
                {
                    var t = term.Trim().ToLower();
                    q = q.Where(f => f.Name.ToLower().Contains(t) || f.Nickname.ToLower().Contains(t) || f.Country.ToLower().Contains(t));
                }
                if (weightClassId != null)
                {
                    q = q.Where(f => f.WeightClassId == weightClassId.Value);
                }

                var results = await q
                    .Include(f => f.WeightClass)
                    .OrderBy(f => f.Name)
                    .Take(10)
                    .Select(f => new {
                        Id = f.Id,
                        Text = f.Name,
                        Description = f.Name + " '" + f.Nickname + "' | " + f.Country + " | " + (f.WeightClass != null ? f.WeightClass.Name : "")
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
