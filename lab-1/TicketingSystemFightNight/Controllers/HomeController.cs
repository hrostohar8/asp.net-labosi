using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class HomeController : Controller
    {
        private readonly FighterMockRepository _fighterRepo;
        private readonly EventMockRepository _eventRepo;
        private readonly ArenaMockRepository _arenaRepo;

        public HomeController(FighterMockRepository fighterRepo, EventMockRepository eventRepo, ArenaMockRepository arenaRepo)
        {
            _fighterRepo = fighterRepo;
            _eventRepo = eventRepo;
            _arenaRepo = arenaRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
