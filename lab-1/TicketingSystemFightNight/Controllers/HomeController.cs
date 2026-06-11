using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TicketingSystemFightNight.Helpers;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<Fighter> _fighterRepo;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IRepository<Event> eventRepo, IRepository<Fighter> fighterRepo, IWebHostEnvironment environment)
        {
            _eventRepo = eventRepo;
            _fighterRepo = fighterRepo;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var upcomingEvents = await _eventRepo.GetAllAsync(query =>
                query.Include(e => e.Venue)
                     .Where(e => e.Date >= DateTime.Today)
                     .OrderBy(e => e.Date));

            var upcomingEvent = upcomingEvents.FirstOrDefault();
            if (upcomingEvent == null)
            {
                upcomingEvent = (await _eventRepo.GetAllAsync(query =>
                    query.Include(e => e.Venue).OrderByDescending(e => e.Date)))
                    .FirstOrDefault();
            }

            var fighters = await _fighterRepo.GetAllAsync(query =>
                query.Where(f => f.Name == "Matej Batinić" || f.Name == "Jakob Nedoh"));

            var fighter1 = fighters.FirstOrDefault(f => f.Name == "Matej Batinić");
            var fighter2 = fighters.FirstOrDefault(f => f.Name == "Jakob Nedoh");

            if (fighter1 != null)
            {
                fighter1.ImageUrl = FighterImageHelper.GetFighterImageUrl(_environment, fighter1);
            }

            if (fighter2 != null)
            {
                fighter2.ImageUrl = FighterImageHelper.GetFighterImageUrl(_environment, fighter2);
            }

            return View(new HomeViewModel
            {
                UpcomingEvent = upcomingEvent,
                Fighter1 = fighter1,
                Fighter2 = fighter2,
                Fighter1ImageUrl = fighter1?.ImageUrl ?? "/images/fighter-placeholder.svg",
                Fighter2ImageUrl = fighter2?.ImageUrl ?? "/images/fighter-placeholder.svg"
            });
        }
    }
}
