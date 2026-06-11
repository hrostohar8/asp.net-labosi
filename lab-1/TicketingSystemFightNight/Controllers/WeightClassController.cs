using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    [Authorize]
    public class WeightClassController : Controller
    {
        private readonly VjezbaDbContext _context;

        public WeightClassController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var weightClasses = await _context.WeightClasses
                .Where(w => w.DeletedAt == null)
                .OrderBy(w => w.Name)
                .ToListAsync();

            return View(weightClasses);
        }

        [ActionName("Create")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult CreateGet()
        {
            return View(new WeightClassCreateViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(WeightClassCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var weightClass = new WeightClass
            {
                Name = model.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.WeightClasses.Add(weightClass);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kategorija težine je uspješno kreirana!";
            return RedirectToAction(nameof(Index));
        }

        [ActionName("Edit")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> EditGet(int id)
        {
            var weightClass = await _context.WeightClasses
                .Where(w => w.DeletedAt == null)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (weightClass == null)
                return NotFound();

            var model = new WeightClassEditViewModel
            {
                Id = weightClass.Id,
                Name = weightClass.Name
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var weightClass = await _context.WeightClasses.FindAsync(id);
            if (weightClass == null || weightClass.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                weightClass,
                "",
                w => w.Name);

            if (!ok || !ModelState.IsValid)
            {
                var model = new WeightClassEditViewModel
                {
                    Id = weightClass.Id,
                    Name = weightClass.Name
                };

                return View(model);
            }

            weightClass.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kategorija težine je uspješno ažurirana!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var weightClass = await _context.WeightClasses.FindAsync(id);
            if (weightClass == null)
                return NotFound();

            weightClass.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kategorija težine je uspješno obrisana!";
            return RedirectToAction(nameof(Index));
        }
    }
}
