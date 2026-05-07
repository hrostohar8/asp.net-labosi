using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TicketingSystemFightNight.Helpers;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    [Route("ulaznice")]
    public class TicketController : Controller
    {
        private readonly IRepository<Ticket> _repository;
        private const string SessionCartKey = "TicketCart";

        public TicketController(IRepository<Ticket> repository)
        {
            _repository = repository;
        }

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

        [HttpGet("detalji/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _repository.GetByIdAsync(id, query =>
                query.Include(t => t.Event).Include(t => t.Cart));
            if (ticket == null)
                return NotFound();
            return View(ticket);
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
