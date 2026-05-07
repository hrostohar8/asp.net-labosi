using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class ArenaController : Controller
    {
        private readonly IRepository<Arena> _repository;

        public ArenaController(IRepository<Arena> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var arenas = await _repository.GetAllAsync();
            return View(arenas);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var arena = await _repository.GetByIdAsync(id);
            if (arena == null)
                return NotFound();
            return View(arena);
        }
    }
}
