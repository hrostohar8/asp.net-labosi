using System.Collections.Generic;
using System.Linq;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Repositories
{
    public class FighterMockRepository
    {
        private readonly List<Fighter> _data;

        public FighterMockRepository()
        {
            _data = new List<Fighter>
            {
                new Fighter(1, "Israel Adesanya", "The Last Stylebender", WeightClass.Middleweight, FightOrganization.UFC, "Nigeria/New Zealand", 23, 1),
                new Fighter(2, "Kamaru Usman", "The Nigerian Nightmare", WeightClass.Welterweight, FightOrganization.UFC, "Nigeria", 19, 3),
                new Fighter(3, "Valentina Shevchenko", "The Bullet", WeightClass.Flyweight, FightOrganization.UFC, "Kyrgyzstan", 24, 3),
                new Fighter(4, "Conor McGregor", "Notorious", WeightClass.Lightweight, FightOrganization.UFC, "Ireland", 22, 6),
                new Fighter(5, "Max Holloway", "Blessed", WeightClass.Featherweight, FightOrganization.UFC, "USA", 23, 7),
                new Fighter(6, "Jon Jones", "Bones", WeightClass.LightHeavyweight, FightOrganization.UFC, "USA", 26, 1)
            };
        }

        public List<Fighter> GetAll() => _data;
        public Fighter? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    }
}
