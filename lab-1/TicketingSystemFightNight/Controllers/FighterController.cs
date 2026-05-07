using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    [Route("borci")]
    public class FighterController : Controller
    {
        private readonly IRepository<Fighter> _repository;

        public FighterController(IRepository<Fighter> repository)
        {
            _repository = repository;
        }

        [HttpGet("")]
        [HttpGet("svi")]
        public async Task<IActionResult> Index()
        {
            var fighters = await _repository.GetAllAsync();
            return View(fighters);
        }

        [HttpGet("detalji/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var fighter = await _repository.GetByIdAsync(id);
            if (fighter == null)
                return NotFound();
            return View(fighter);
        }
    }
}
