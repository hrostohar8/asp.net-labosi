using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class ArenaController : Controller
    {
        private readonly ArenaMockRepository _repository;

        public ArenaController(ArenaMockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var arenas = _repository.GetAll();
            return View(arenas);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var arena = _repository.GetById(id);
            if (arena == null)
                return NotFound();
            return View(arena);
        }
    }
}
