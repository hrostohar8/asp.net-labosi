using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    [Authorize]
    public class FightOrganizationController : Controller
    {
        private readonly VjezbaDbContext _context;

        public FightOrganizationController(VjezbaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var organizations = await _context.FightOrganizations
                .Where(o => o.DeletedAt == null)
                .OrderBy(o => o.Name)
                .ToListAsync();

            return View(organizations);
        }

        [ActionName("Create")]
        public IActionResult CreateGet()
        {
            return View(new FightOrganizationCreateViewModel());
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(FightOrganizationCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var organization = new FightOrganization
            {
                Name = model.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.FightOrganizations.Add(organization);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Organizacija je uspješno kreirana!";
            return RedirectToAction(nameof(Index));
        }

        [ActionName("Edit")]
        public async Task<IActionResult> EditGet(int id)
        {
            var organization = await _context.FightOrganizations
                .Where(o => o.DeletedAt == null)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (organization == null)
                return NotFound();

            var model = new FightOrganizationEditViewModel
            {
                Id = organization.Id,
                Name = organization.Name
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var organization = await _context.FightOrganizations.FindAsync(id);
            if (organization == null || organization.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                organization,
                "",
                o => o.Name);

            if (!ok || !ModelState.IsValid)
            {
                var model = new FightOrganizationEditViewModel
                {
                    Id = organization.Id,
                    Name = organization.Name
                };

                return View(model);
            }

            organization.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Organizacija je uspješno ažurirana!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var organization = await _context.FightOrganizations.FindAsync(id);
            if (organization == null)
                return NotFound();

            organization.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Organizacija je uspješno obrisana!";
            return RedirectToAction(nameof(Index));
        }
    }
}
