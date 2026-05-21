using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Repositories;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRepository<Fighter> _fighterRepo;
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<Arena> _arenaRepo;
        private readonly IRepository<Match> _matchRepo;
        private readonly IRepository<Ticket> _ticketRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Cart> _cartRepo;

        public DashboardController(
            IRepository<Fighter> fighterRepo,
            IRepository<Event> eventRepo,
            IRepository<Arena> arenaRepo,
            IRepository<Match> matchRepo,
            IRepository<Ticket> ticketRepo,
            IRepository<User> userRepo,
            IRepository<Cart> cartRepo)
        {
            _fighterRepo = fighterRepo;
            _eventRepo = eventRepo;
            _arenaRepo = arenaRepo;
            _matchRepo = matchRepo;
            _ticketRepo = ticketRepo;
            _userRepo = userRepo;
            _cartRepo = cartRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fighters = await _fighterRepo.GetAllAsync();
            var events = await _eventRepo.GetAllAsync();
            var arenas = await _arenaRepo.GetAllAsync();
            var matches = await _matchRepo.GetAllAsync();
            var tickets = await _ticketRepo.GetAllAsync();
            var users = await _userRepo.GetAllAsync();
            var carts = await _cartRepo.GetAllAsync();

            var viewModel = new DashboardViewModel
            {
                TotalFighters = fighters.Count,
                TotalEvents = events.Count,
                TotalArenas = arenas.Count,
                TotalMatches = matches.Count,
                TotalTickets = tickets.Count,
                TotalUsers = users.Count,
                TotalCarts = carts.Count,

                FightersByOrganization = fighters
                    .GroupBy(f => f.Organization)
                    .Select(g => new OrganizationStats { Organization = g.Key, Count = g.Count() })
                    .ToList(),

                UpcomingEvents = events
                    .Where(e => e.Date >= DateTime.Today)
                    .OrderBy(e => e.Date)
                    .Take(5)
                    .ToList(),

                TopFighters = fighters
                    .OrderByDescending(f => f.Wins - f.Losses)
                    .ThenByDescending(f => f.Wins)
                    .Take(5)
                    .ToList(),

                RecentTickets = tickets
                    .OrderByDescending(t => t.PurchaseDate)
                    .Take(5)
                    .ToList()
            };

            return View(viewModel);
        }
    }

    public class DashboardViewModel
    {
        public int TotalFighters { get; set; }
        public int TotalEvents { get; set; }
        public int TotalArenas { get; set; }
        public int TotalMatches { get; set; }
        public int TotalTickets { get; set; }
        public int TotalUsers { get; set; }
        public int TotalCarts { get; set; }

        public List<OrganizationStats> FightersByOrganization { get; set; } = new();
        public List<Event> UpcomingEvents { get; set; } = new();
        public List<Fighter> TopFighters { get; set; } = new();
        public List<Ticket> RecentTickets { get; set; } = new();
    }

    public class OrganizationStats
    {
        public FightOrganization Organization { get; set; }
        public int Count { get; set; }
    }
}