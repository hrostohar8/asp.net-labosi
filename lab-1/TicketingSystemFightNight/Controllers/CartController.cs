using Microsoft.AspNetCore.Mvc;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class CartController : Controller
    {
        private readonly CartMockRepository _repository;

        public CartController(CartMockRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var carts = _repository.GetAll();
            return View(carts);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var cart = _repository.GetById(id);
            if (cart == null)
                return NotFound();
            return View(cart);
        }
    }
}
