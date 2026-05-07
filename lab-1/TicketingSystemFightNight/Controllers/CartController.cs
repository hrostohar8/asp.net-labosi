using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Repositories;

namespace TicketingSystemFightNight.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository<Cart> _repository;

        public CartController(IRepository<Cart> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var carts = await _repository.GetAllAsync(query =>
                query.Include(c => c.User).Include(c => c.Tickets).ThenInclude(t => t.Event));
            return View(carts);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var cart = await _repository.GetByIdAsync(id, query =>
                query.Include(c => c.User).Include(c => c.Tickets).ThenInclude(t => t.Event));
            if (cart == null)
                return NotFound();
            return View(cart);
        }
    }
}
