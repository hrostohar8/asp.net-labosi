using System.Collections.Generic;
using System.Linq;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Repositories
{
    public class CartMockRepository
    {
        private readonly List<Cart> _data;

        public CartMockRepository()
        {
            var userRepo = new UserMockRepository();
            var eventRepo = new EventMockRepository();
            var ticketRepo = new TicketMockRepository();

            var user = userRepo.GetById(1);
            var tickets = ticketRepo.GetAll();

            _data = new List<Cart>();
            
            if (user != null)
            {
                _data.Add(new Cart(1, user, "FIGHT10", 10m)
                {
                    Tickets = tickets
                });
            }
        }

        public List<Cart> GetAll() => _data;
        public Cart? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    }
}
