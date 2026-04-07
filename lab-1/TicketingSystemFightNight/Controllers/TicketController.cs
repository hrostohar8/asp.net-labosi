using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class TicketController : Controller
    {
        private readonly TicketMockRepository _repository;

        public TicketController(TicketMockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var tickets = _repository.GetAll();
            return View(tickets);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var ticket = _repository.GetById(id);
            if (ticket == null)
                return NotFound();
            return View(ticket);
        }
    }
}
