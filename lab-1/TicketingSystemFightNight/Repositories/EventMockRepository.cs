using System.Collections.Generic;
using System.Linq;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Repositories
{
    public class EventMockRepository
    {
        private readonly List<Event> _data;

        public EventMockRepository()
        {
            var arenas = new List<Arena>
            {
                new Arena(1, "Arena Zagreb", "Zagreb", 15000, "Ulica Grada Vukovara 269a", true, 2008),
                new Arena(2, "Arena Split", "Split", 12000, "Ulica Domovinskog rata 2", false, 2019),
                new Arena(3, "Arena Rijeka", "Rijeka", 10000, "Trg Riječke rezolucije 2", true, 2014)
            };

            _data = new List<Event>
            {
                new Event(1, "UFC 300", FightOrganization.UFC, "Zagreb", DateTime.Today.AddDays(25), TimeSpan.FromHours(19), arenas[0], "Glavni UFC spektakl u Hrvatskoj", 250m, 12850),
                new Event(2, "Bellator 295", FightOrganization.BELLATOR, "Split", DateTime.Today.AddDays(40), TimeSpan.FromHours(19), arenas[1], "Najbolji Bellator mečevi u Dalmaciji", 180m, 9800),
                new Event(3, "KSW 74", FightOrganization.KSW, "Rijeka", DateTime.Today.AddDays(60), TimeSpan.FromHours(19), arenas[2], "KSW ekskluziva manje od sat vremena od Rijeke", 150m, 7600)
            };
        }

        public List<Event> GetAll() => _data;
        public Event? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    }
}
