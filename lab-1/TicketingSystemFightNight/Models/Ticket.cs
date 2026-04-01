using System;

namespace TicketingSystemFightNight.Models
{
    public class Ticket
    {
        public Event Event { get; set; }
        public string Seat { get; set; }
        public decimal Price { get; set; }

        public Ticket(Event event_, string seat, decimal price)
        {
            Event = event_;
            Seat = seat;
            Price = price;
        }
    }
}