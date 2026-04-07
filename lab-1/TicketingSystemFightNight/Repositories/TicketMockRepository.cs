using System;
using System.Collections.Generic;
using System.Linq;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Repositories
{
    public class TicketMockRepository
    {
        private readonly List<Ticket> _data;

        public TicketMockRepository()
        {
            var eventRepo = new EventMockRepository();
            var events = eventRepo.GetAll();

            _data = new List<Ticket>
            {
                new Ticket(1, events[0], "Premium", 5, "A01", 550m, DateTime.Now.AddDays(-1), true),
                new Ticket(2, events[0], "Premium", 5, "A02", 550m, DateTime.Now.AddDays(-1), true),
                new Ticket(3, events[1], "Main", 2, "B12", 180m, DateTime.Now.AddDays(-2), false)
            };
        }

        public List<Ticket> GetAll() => _data;
        public Ticket? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    }
}
