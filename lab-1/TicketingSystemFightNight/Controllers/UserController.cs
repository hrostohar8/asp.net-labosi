using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly VjezbaDbContext _context;

        public UserController(VjezbaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Where(u => u.DeletedAt == null)
                .OrderBy(u => u.Name)
                .ToListAsync();

            return View(users);
        }

        [ActionName("Create")]
        public IActionResult CreateGet()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                BirthDate = model.BirthDate,
                LoyaltyPoints = model.LoyaltyPoints,
                IsVip = model.IsVip,
                MemberLevel = model.MemberLevel,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Korisnik je uspješno kreiran!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.Users
                .Where(u => u.DeletedAt == null)
                .Include(u => u.Carts)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> EditGet(int id)
        {
            var user = await _context.Users
                .Where(u => u.DeletedAt == null)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            var model = new UserEditViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                BirthDate = user.BirthDate,
                LoyaltyPoints = user.LoyaltyPoints,
                IsVip = user.IsVip,
                MemberLevel = user.MemberLevel
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                user,
                "",
                u => u.Name,
                u => u.Email,
                u => u.Phone,
                u => u.BirthDate,
                u => u.LoyaltyPoints,
                u => u.IsVip,
                u => u.MemberLevel);

            if (!ok || !ModelState.IsValid)
            {
                var model = new UserEditViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    BirthDate = user.BirthDate,
                    LoyaltyPoints = user.LoyaltyPoints,
                    IsVip = user.IsVip,
                    MemberLevel = user.MemberLevel
                };
                return View(model);
            }

            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Korisnik je uspješno ažuriran!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Korisnik je uspješno obrisan!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string term)     // This method is used for AJAX search in dropdowns
        {
            try
            {
                var q = _context.Users.AsNoTracking().Where(u => u.DeletedAt == null);
                if (!string.IsNullOrWhiteSpace(term))
                {
                    var t = term.Trim().ToLower();
                    q = q.Where(u => u.Email.ToLower().Contains(t) || u.Name.ToLower().Contains(t));
                }

                var results = await q.OrderBy(u => u.Name).Take(10).Select(u => new {
                    Id = u.Id,
                    Text = u.Name,
                    Description = u.Name + " | " + u.Email
                }).ToListAsync();

                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new[] { new { Id = 0, Text = "", Description = "Greška pri pretrazi: " + ex.Message } });
            }
        }
    }
}
