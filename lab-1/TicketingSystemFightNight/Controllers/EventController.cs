using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class EventController : Controller
    {
        private readonly EventMockRepository _repository;

        public EventController(EventMockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var events = _repository.GetAll();
            return View(events);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var @event = _repository.GetById(id);
            if (@event == null)
                return NotFound();
            return View(@event);
        }
    }
}
