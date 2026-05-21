using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Helpers;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    public class CartController : Controller
    {
        private readonly VjezbaDbContext _context;
        private const string SessionCartKey = "TicketCart";

        public CartController(VjezbaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Current()
        {
            var cartItems = HttpContext.Session.GetObject<List<TicketOffer>>(SessionCartKey) ?? new List<TicketOffer>();
            var model = new CartPageViewModel
            {
                CartItems = cartItems
            };
            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var carts = await _context.Carts
                .Where(c => c.DeletedAt == null)
                .Include(c => c.User)
                .Include(c => c.Tickets).ThenInclude(t => t.Event)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(carts);
        }

        [ActionName("Create")]
        public IActionResult CreateGet()
        {
            ViewBag.Users = new SelectList(_context.Users.Where(u => u.DeletedAt == null).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CartCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(_context.Users.Where(u => u.DeletedAt == null).ToList(), "Id", "Name", model.UserId);
                ViewBag.SelectedUser = model.UserId > 0
                    ? await _context.Users.AsNoTracking().Where(u => u.Id == model.UserId).Select(u => new { u.Id, u.Name }).FirstOrDefaultAsync()
                    : null;
                return View(model);
            }

            var cart = new Cart
            {
                UserId = model.UserId,
                DiscountCode = model.DiscountCode,
                DiscountPercent = model.DiscountPercent,
                IsPaid = model.IsPaid,
                CreatedAt = DateTime.UtcNow
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Košarica je uspješno kreirana!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var cart = await _context.Carts
                .Where(c => c.DeletedAt == null)
                .Include(c => c.User)
                .Include(c => c.Tickets).ThenInclude(t => t.Event)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
                return NotFound();
            return View(cart);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> EditGet(int id)
        {
            var cart = await _context.Carts
                .Where(c => c.DeletedAt == null)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
                return NotFound();

            ViewBag.Users = new SelectList(_context.Users.Where(u => u.DeletedAt == null).ToList(), "Id", "Name", cart.UserId);
            var usr = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == cart.UserId);
            ViewBag.SelectedUser = usr != null ? new { Id = usr.Id, Text = usr.Name } : null;
            var model = new CartEditViewModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                DiscountCode = cart.DiscountCode,
                DiscountPercent = cart.DiscountPercent,
                IsPaid = cart.IsPaid
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null || cart.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                cart,
                "",
                c => c.UserId,
                c => c.DiscountCode,
                c => c.DiscountPercent,
                c => c.IsPaid);

            if (!ok || !ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(_context.Users.Where(u => u.DeletedAt == null).ToList(), "Id", "Name", cart.UserId);
                ViewBag.SelectedUser = cart.UserId > 0
                    ? await _context.Users.AsNoTracking().Where(u => u.Id == cart.UserId).Select(u => new { u.Id, u.Name }).FirstOrDefaultAsync()
                    : null;
                var model = new CartEditViewModel
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    DiscountCode = cart.DiscountCode,
                    DiscountPercent = cart.DiscountPercent,
                    IsPaid = cart.IsPaid
                };
                return View(model);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Košarica je uspješno ažurirana!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
                return NotFound();

            cart.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Košarica je uspješno obrisana!";
            return RedirectToAction(nameof(Index));
        }
    }
}
