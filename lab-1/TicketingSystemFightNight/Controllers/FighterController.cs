using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class FighterController : Controller
    {
        private readonly FighterMockRepository _repository;

        public FighterController(FighterMockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var fighters = _repository.GetAll();
            return View(fighters);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var fighter = _repository.GetById(id);
            if (fighter == null)
                return NotFound();
            return View(fighter);
        }
    }
}
