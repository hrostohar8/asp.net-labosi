using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class TicketController : Controller
    {
        private readonly IRepository<Ticket> _repository;

        public TicketController(IRepository<Ticket> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tickets = await _repository.GetAllAsync(query =>
                query.Include(t => t.Event).Include(t => t.Cart));
            return View(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _repository.GetByIdAsync(id, query =>
                query.Include(t => t.Event).Include(t => t.Cart));
            if (ticket == null)
                return NotFound();
            return View(ticket);
        }
    }
}
