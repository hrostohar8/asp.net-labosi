using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Helpers;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Models.ViewModels;

namespace TicketingSystemFightNight.Controllers
{
    [Route("ulaznice")]
    [Route("Ticket")]
    [Authorize]
    public class TicketController : Controller
    {
        private readonly VjezbaDbContext _context;
        private const string SessionCartKey = "TicketCart";

        public TicketController(VjezbaDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult Index()
        {
            var model = new TicketShopViewModel
            {
                Offers = GetTicketOffers(),
                CartItems = GetCartItems()
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost("dodaj-u-kosaricu")]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int offerId)
        {
            var offer = GetTicketOffers().FirstOrDefault(x => x.Id == offerId);
            if (offer == null)
            {
                return NotFound();
            }

            var cartItems = GetCartItems();
            cartItems.Add(offer);
            HttpContext.Session.SetObject(SessionCartKey, cartItems);

            TempData["CartMessage"] = $"Karta '{offer.Title}' je dodana u košaricu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("manage")]
        public async Task<IActionResult> Manage()
        {
            var tickets = await _context.Tickets
                .Where(t => t.DeletedAt == null)
                .Include(t => t.Event)
                .Include(t => t.Cart)
                .OrderByDescending(t => t.PurchaseDate)
                .ToListAsync();

            return View(tickets);
        }

        [HttpGet("Create")]
        [ActionName("Create")]
        public IActionResult CreateGet()
        {
            ViewBag.Events = new SelectList(_context.Events.Where(e => e.DeletedAt == null).ToList(), "Id", "Name");
            ViewBag.Carts = new SelectList(_context.Carts.Where(c => c.DeletedAt == null).ToList(), "Id", "Id");
            return View();
        }

        [HttpPost("Create")]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(TicketCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Events = new SelectList(_context.Events.Where(e => e.DeletedAt == null).ToList(), "Id", "Name", model.EventId);
                ViewBag.Carts = new SelectList(_context.Carts.Where(c => c.DeletedAt == null).ToList(), "Id", "Id", model.CartId);
                ViewBag.SelectedEvent = model.EventId > 0
                    ? await _context.Events.AsNoTracking().Where(e => e.Id == model.EventId).Select(e => new { e.Id, e.Name }).FirstOrDefaultAsync()
                    : null;
                return View(model);
            }

            var ticket = new Ticket
            {
                EventId = model.EventId,
                CartId = model.CartId,
                Section = model.Section,
                Row = model.Row,
                Seat = model.Seat,
                Price = model.Price,
                PurchaseDate = model.PurchaseDate,
                IsVip = model.IsVip,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Karta je uspješno kreirana!";
            return RedirectToAction(nameof(Manage));
        }

        [HttpGet("detalji/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _context.Tickets
                .Where(t => t.DeletedAt == null)
                .Include(t => t.Event)
                .Include(t => t.Cart)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
                return NotFound();
            return View(ticket);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> EditGet(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null || ticket.DeletedAt != null)
                return NotFound();

            ViewBag.Events = new SelectList(_context.Events.Where(e => e.DeletedAt == null).ToList(), "Id", "Name", ticket.EventId);
            var ev = await _context.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == ticket.EventId);
            ViewBag.SelectedEvent = ev != null ? new { Id = ev.Id, Text = ev.Name } : null;
            ViewBag.Carts = new SelectList(_context.Carts.Where(c => c.DeletedAt == null).ToList(), "Id", "Id", ticket.CartId);

            var model = new TicketEditViewModel
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                CartId = ticket.CartId,
                Section = ticket.Section,
                Row = ticket.Row,
                Seat = ticket.Seat,
                Price = ticket.Price,
                PurchaseDate = ticket.PurchaseDate,
                IsVip = ticket.IsVip
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null || ticket.DeletedAt != null)
                return NotFound();

            var ok = await TryUpdateModelAsync(
                ticket,
                "",
                t => t.EventId,
                t => t.CartId,
                t => t.Section,
                t => t.Row,
                t => t.Seat,
                t => t.Price,
                t => t.PurchaseDate,
                t => t.IsVip);

            if (!ok || !ModelState.IsValid)
            {
                ViewBag.Events = new SelectList(_context.Events.Where(e => e.DeletedAt == null).ToList(), "Id", "Name", ticket.EventId);
                ViewBag.Carts = new SelectList(_context.Carts.Where(c => c.DeletedAt == null).ToList(), "Id", "Id", ticket.CartId);
                ViewBag.SelectedEvent = ticket.EventId > 0
                    ? await _context.Events.AsNoTracking().Where(e => e.Id == ticket.EventId).Select(e => new { e.Id, e.Name }).FirstOrDefaultAsync()
                    : null;

                var model = new TicketEditViewModel
                {
                    Id = ticket.Id,
                    EventId = ticket.EventId,
                    CartId = ticket.CartId,
                    Section = ticket.Section,
                    Row = ticket.Row,
                    Seat = ticket.Seat,
                    Price = ticket.Price,
                    PurchaseDate = ticket.PurchaseDate,
                    IsVip = ticket.IsVip
                };
                return View(model);
            }

            ticket.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Karta je uspješno ažurirana!";
            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            ticket.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Karta je uspješno obrisana!";
            return RedirectToAction(nameof(Manage));
        }

        private List<TicketOffer> GetCartItems()
        {
            return HttpContext.Session.GetObject<List<TicketOffer>>(SessionCartKey) ?? new List<TicketOffer>();
        }

        private static List<TicketOffer> GetTicketOffers()
        {
            return new List<TicketOffer>
            {
                new TicketOffer
                {
                    Id = 1,
                    Title = "VIP Ringside",
                    EventName = "Fight Night FNF 29",
                    Location = "Ljubljana Arena, Slovenija",
                    Section = "Premium",
                    Row = 1,
                    Seat = "A1",
                    Price = 450m,
                    IsVip = true,
                    Description = "Najbolja mjesta uz ring sa uključenim VIP uslugama."
                },
                new TicketOffer
                {
                    Id = 2,
                    Title = "Lounge Experience",
                    EventName = "Fight Night FNF 29",
                    Location = "Ljubljana Arena, Slovenija",
                    Section = "Lounge",
                    Row = 5,
                    Seat = "B10",
                    Price = 320m,
                    IsVip = true,
                    Description = "Udobna lounge sjedala sa boljim pogledom i pićem dobrodošlice."
                },
                new TicketOffer
                {
                    Id = 3,
                    Title = "Standard Seat",
                    EventName = "Fight Night FNF 29",
                    Location = "Ljubljana Arena, Slovenija",
                    Section = "C",
                    Row = 12,
                    Seat = "D20",
                    Price = 180m,
                    IsVip = false,
                    Description = "Dobra vidljivost i pristupačna cijena za gledanje borbi."
                },
                new TicketOffer
                {
                    Id = 4,
                    Title = "Upper Deck",
                    EventName = "Fight Night FNF 29",
                    Location = "Ljubljana Arena, Slovenija",
                    Section = "E",
                    Row = 20,
                    Seat = "F25",
                    Price = 120m,
                    IsVip = false,
                    Description = "Najpovoljnija opcija za navijače koji žele energiju i dobar pogled."
                }
            };
        }
    }
}
