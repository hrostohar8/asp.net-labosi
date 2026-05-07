using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository<User> _repository;

        public UserController(IRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _repository.GetAllAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return View(user);
        }
    }
}
