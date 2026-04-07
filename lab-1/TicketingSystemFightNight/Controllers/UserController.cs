using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class UserController : Controller
    {
        private readonly UserMockRepository _repository;

        public UserController(UserMockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _repository.GetAll();
            return View(users);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = _repository.GetById(id);
            if (user == null)
                return NotFound();
            return View(user);
        }
    }
}
