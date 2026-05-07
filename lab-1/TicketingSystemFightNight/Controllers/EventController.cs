using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    [Route("dogadaji")]
    public class EventController : Controller
    {
        private readonly IRepository<Event> _repository;
        private readonly IRepository<Arena> _arenaRepository;

        public EventController(IRepository<Event> repository, IRepository<Arena> arenaRepository)
        {
            _repository = repository;
            _arenaRepository = arenaRepository;
        }

        [HttpGet("")]
        [HttpGet("svi")]
        public async Task<IActionResult> Index()
        {
            var events = await _repository.GetAllAsync(query =>
                query.Include(e => e.Venue).Include(e => e.Matches));
            return View(events);
        }

        [HttpGet("detalji/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var @event = await _repository.GetByIdAsync(id, query =>
                query.Include(e => e.Venue)
                     .Include(e => e.Matches).ThenInclude(m => m.Fighter1)
                     .Include(e => e.Matches).ThenInclude(m => m.Fighter2)
                     .Include(e => e.Tickets));
            if (@event == null)
                return NotFound();
            return View(@event);
        }

        [HttpGet("uredi/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var @event = await _repository.GetByIdAsync(id);
            if (@event == null)
                return NotFound();

            ViewBag.ArenaList = new SelectList(await _arenaRepository.GetAllAsync(), "Id", "Name", @event.VenueId);
            return View(@event);
        }

        [HttpPost("uredi/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event eventModel)
        {
            if (id != eventModel.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.ArenaList = new SelectList(await _arenaRepository.GetAllAsync(), "Id", "Name", eventModel.VenueId);
                return View(eventModel);
            }

            var existingEvent = await _repository.GetByIdAsync(id);
            if (existingEvent == null)
                return NotFound();

            existingEvent.Name = eventModel.Name;
            existingEvent.Organization = eventModel.Organization;
            existingEvent.City = eventModel.City;
            existingEvent.Date = eventModel.Date;
            existingEvent.Time = eventModel.Time;
            existingEvent.VenueId = eventModel.VenueId;
            existingEvent.Description = eventModel.Description;
            existingEvent.BaseTicketPrice = eventModel.BaseTicketPrice;
            existingEvent.TicketsSold = eventModel.TicketsSold;

            await _repository.UpdateAsync(existingEvent);
            await _repository.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = existingEvent.Id });
        }
    }
}
