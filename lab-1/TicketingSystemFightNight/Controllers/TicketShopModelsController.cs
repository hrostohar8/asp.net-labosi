using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    [Authorize]
    public class TicketShopModelsController : Controller
    {
        private readonly VjezbaDbContext _context;

        public TicketShopModelsController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var offers = await _context.TicketShopModels
                .Where(o => o.DeletedAt == null)
                .OrderBy(o => o.Title)
                .ToListAsync();

            return View(offers);
        }

        [ActionName("Create")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult CreateGet()
        {
            return View(new TicketShopModelsCreateViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(TicketShopModelsCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var offer = new TicketShopModels
            {
                Title = model.Title,
                EventName = model.EventName,
                Location = model.Location,
                Section = model.Section,
                Row = model.Row,
                Seat = model.Seat,
                Price = model.Price,
                IsVip = model.IsVip,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.TicketShopModels.Add(offer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ponuda je uspješno kreirana!";
            return RedirectToAction(nameof(Index));
        }

        [ActionName("Edit")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> EditGet(int id)
        {
            var offer = await _context.TicketShopModels
                .Where(o => o.DeletedAt == null)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null)
                return NotFound();

            var model = new TicketShopModelsEditViewModel
            {
                Id = offer.Id,
                Title = offer.Title,
                EventName = offer.EventName,
                Location = offer.Location,
                Section = offer.Section,
                Row = offer.Row,
                Seat = offer.Seat,
                Price = offer.Price,
                IsVip = offer.IsVip,
                Description = offer.Description
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var offer = await _context.TicketShopModels.FindAsync(id);
            if (offer == null || offer.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                offer,
                "",
                o => o.Title,
                o => o.EventName,
                o => o.Location,
                o => o.Section,
                o => o.Row,
                o => o.Seat,
                o => o.Price,
                o => o.IsVip,
                o => o.Description);

            if (!ok || !ModelState.IsValid)
            {
                var model = new TicketShopModelsEditViewModel
                {
                    Id = offer.Id,
                    Title = offer.Title,
                    EventName = offer.EventName,
                    Location = offer.Location,
                    Section = offer.Section,
                    Row = offer.Row,
                    Seat = offer.Seat,
                    Price = offer.Price,
                    IsVip = offer.IsVip,
                    Description = offer.Description
                };

                return View(model);
            }

            offer.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ponuda je uspješno ažurirana!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var offer = await _context.TicketShopModels.FindAsync(id);
            if (offer == null)
                return NotFound();

            offer.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ponuda je uspješno obrisana!";
            return RedirectToAction(nameof(Index));
        }
    }
}
