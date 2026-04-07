using System.Collections.Generic;
using System.Linq;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Repositories
{
    public class MatchMockRepository
    {
        private readonly List<Match> _data;

        public MatchMockRepository()
        {
            var events = new List<Event>
            {
                new Event(1, "UFC 300", FightOrganization.UFC, "Zagreb", DateTime.Today.AddDays(25), TimeSpan.FromHours(19), new Arena(1, "Arena Zagreb", "Zagreb", 15000, "Ulica Grada Vukovara 269a", true, 2008), "Glavni UFC spektakl u Hrvatskoj", 250m, 12850),
                new Event(2, "Bellator 295", FightOrganization.BELLATOR, "Split", DateTime.Today.AddDays(40), TimeSpan.FromHours(19), new Arena(2, "Arena Split", "Split", 12000, "Ulica Domovinskog rata 2", false, 2019), "Najbolji Bellator mečevi u Dalmaciji", 180m, 9800)
            };

            var fighters = new List<Fighter>
            {
                new Fighter(1, "Israel Adesanya", "The Last Stylebender", WeightClass.Middleweight, FightOrganization.UFC, "Nigeria/New Zealand", 23, 1),
                new Fighter(2, "Robert Whittaker", "The Reaper", WeightClass.Middleweight, FightOrganization.UFC, "Australia", 23, 5),
                new Fighter(3, "Conor McGregor", "Notorious", WeightClass.Lightweight, FightOrganization.UFC, "Ireland", 22, 6),
                new Fighter(4, "Khabib Nurmagomedov", "The Eagle", WeightClass.Lightweight, FightOrganization.UFC, "Russia", 29, 0)
            };

            _data = new List<Match>
            {
                new Match(1, fighters[0], fighters[1], WeightClass.Middleweight, events[0], 5, false, "Herb Dean", "Scheduled"),
                new Match(2, fighters[2], fighters[3], WeightClass.Lightweight, events[0], 5, true, "John McCarthy", "Title fight"),
                new Match(3, fighters[1], fighters[3], WeightClass.Lightweight, events[1], 5, false, "Jason Herzog", "Scheduled")
            };
        }

        public List<Match> GetAll() => _data;
        public Match? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    }
}
