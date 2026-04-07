using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Repositories;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Controllers
{
    public class DashboardController : Controller
    {
        private readonly FighterMockRepository _fighterRepo;
        private readonly EventMockRepository _eventRepo;
        private readonly ArenaMockRepository _arenaRepo;
        private readonly MatchMockRepository _matchRepo;
        private readonly TicketMockRepository _ticketRepo;
        private readonly UserMockRepository _userRepo;
        private readonly CartMockRepository _cartRepo;

        public DashboardController(
            FighterMockRepository fighterRepo,
            EventMockRepository eventRepo,
            ArenaMockRepository arenaRepo,
            MatchMockRepository matchRepo,
            TicketMockRepository ticketRepo,
            UserMockRepository userRepo,
            CartMockRepository cartRepo)
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
        public IActionResult Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalFighters = _fighterRepo.GetAll().Count,
                TotalEvents = _eventRepo.GetAll().Count,
                TotalArenas = _arenaRepo.GetAll().Count,
                TotalMatches = _matchRepo.GetAll().Count,
                TotalTickets = _ticketRepo.GetAll().Count,
                TotalUsers = _userRepo.GetAll().Count,
                TotalCarts = _cartRepo.GetAll().Count,

                FightersByOrganization = _fighterRepo.GetAll()
                    .GroupBy(f => f.Organization)
                    .Select(g => new OrganizationStats { Organization = g.Key, Count = g.Count() })
                    .ToList(),

                UpcomingEvents = _eventRepo.GetAll()
                    .Where(e => e.Date >= DateTime.Today)
                    .OrderBy(e => e.Date)
                    .Take(5)
                    .ToList(),

                TopFighters = _fighterRepo.GetAll()
                    .OrderByDescending(f => f.Wins - f.Losses)
                    .ThenByDescending(f => f.Wins)
                    .Take(5)
                    .ToList(),

                RecentTickets = _ticketRepo.GetAll()
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