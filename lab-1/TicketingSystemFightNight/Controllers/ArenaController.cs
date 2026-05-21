using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    public class ArenaController : Controller
    {
        private readonly VjezbaDbContext _context;

        public ArenaController(VjezbaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var arenas = await _context.Arenas
                .Where(a => a.DeletedAt == null)
                .OrderBy(a => a.Name)
                .ToListAsync();

            return View(arenas);
        }

        [ActionName("Create")]
        public IActionResult CreateGet()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(ArenaCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var arena = new Arena
            {
                Name = model.Name,
                City = model.City,
                Address = model.Address,
                Capacity = model.Capacity,
                IsIndoor = model.IsIndoor,
                OpenedYear = model.OpenedYear,
                CreatedAt = DateTime.UtcNow
            };

            _context.Arenas.Add(arena);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Arena je uspješno kreirana!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchArenas(string term)
        {
            try
            {
                var q = _context.Arenas.AsNoTracking().Where(a => a.DeletedAt == null);
                if (!string.IsNullOrWhiteSpace(term))
                {
                    var t = term.Trim().ToLower();
                    q = q.Where(a => a.Name.ToLower().Contains(t) || a.City.ToLower().Contains(t));
                }

                var results = await q.OrderBy(a => a.Name).Take(10).Select(a => new {
                    Id = a.Id,
                    Text = a.Name,
                    Description = a.Name + " | " + a.City + " | Capacity: " + a.Capacity
                }).ToListAsync();

                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new[] { new { Id = 0, Text = "", Description = "Greška pri pretrazi: " + ex.Message } });
            }
        }

        [ActionName("Edit")]
        public async Task<IActionResult> EditGet(int id)
        {
            var arena = await _context.Arenas
                .Where(a => a.DeletedAt == null)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (arena == null)
                return NotFound();

            var model = new ArenaEditViewModel
            {
                Id = arena.Id,
                Name = arena.Name,
                City = arena.City,
                Address = arena.Address,
                Capacity = arena.Capacity,
                IsIndoor = arena.IsIndoor,
                OpenedYear = arena.OpenedYear
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var arena = await _context.Arenas.FindAsync(id);
            if (arena == null || arena.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                arena,
                "",
                a => a.Name,
                a => a.City,
                a => a.Address,
                a => a.Capacity,
                a => a.IsIndoor,
                a => a.OpenedYear);

            if (!ok || !ModelState.IsValid)
            {
                var viewModel = new ArenaEditViewModel
                {
                    Id = arena.Id,
                    Name = arena.Name,
                    City = arena.City,
                    Address = arena.Address,
                    Capacity = arena.Capacity,
                    IsIndoor = arena.IsIndoor,
                    OpenedYear = arena.OpenedYear
                };

                return View(viewModel);
            }

            arena.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Arena je uspješno ažurirana!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var arena = await _context.Arenas.FindAsync(id);
            if (arena == null)
                return NotFound();

            arena.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Arena je uspješno obrisana!";
            return RedirectToAction(nameof(Index));
        }
    }
}
