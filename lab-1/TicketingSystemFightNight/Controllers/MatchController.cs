using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class MatchController : Controller
    {
        private readonly IRepository<Match> _repository;

        public MatchController(IRepository<Match> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var matches = await _repository.GetAllAsync(query =>
                query.Include(m => m.Fighter1).Include(m => m.Fighter2).Include(m => m.Event));
            return View(matches);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var match = await _repository.GetByIdAsync(id, query =>
                query.Include(m => m.Fighter1).Include(m => m.Fighter2).Include(m => m.Event));
            if (match == null)
                return NotFound();
            return View(match);
        }
    }
}
