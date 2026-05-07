using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Helpers;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    [Route("borci")]
    public class FighterController : Controller
    {
        private readonly IRepository<Fighter> _repository;
        private readonly IWebHostEnvironment _environment;

        public FighterController(IRepository<Fighter> repository, IWebHostEnvironment environment)
        {
            _repository = repository;
            _environment = environment;
        }

        [HttpGet("")]
        [HttpGet("svi")]
        public async Task<IActionResult> Index()
        {
            var fighters = await _repository.GetAllAsync();
            foreach (var fighter in fighters)
            {
                fighter.ImageUrl = FighterImageHelper.GetFighterImageUrl(_environment, fighter);
            }
            return View(fighters);
        }

        [HttpGet("detalji/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var fighter = await _repository.GetByIdAsync(id);
            if (fighter == null)
                return NotFound();

            fighter.ImageUrl = FighterImageHelper.GetFighterImageUrl(_environment, fighter);
            return View(fighter);
        }
    }
}
