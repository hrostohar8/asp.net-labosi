using System.Collections.Generic;

namespace TicketingSystemFightNight.Models
{
    public class Cart
    {
        public User User { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();

        public Cart(User user)
        {
            User = user;
        }
    }
}