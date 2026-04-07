using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class MatchController : Controller
    {
        private readonly MatchMockRepository _repository;

        public MatchController(MatchMockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var matches = _repository.GetAll();
            return View(matches);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var match = _repository.GetById(id);
            if (match == null)
                return NotFound();
            return View(match);
        }
    }
}
