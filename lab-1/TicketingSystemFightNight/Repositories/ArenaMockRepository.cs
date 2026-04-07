using System.Collections.Generic;
using System.Linq;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Repositories
{
    public class ArenaMockRepository
    {
        private readonly List<Arena> _data;

        public ArenaMockRepository()
        {
            _data = new List<Arena>
            {
                new Arena(1, "Arena Zagreb", "Zagreb", 15000, "Ulica Grada Vukovara 269a", true, 2008),
                new Arena(2, "Arena Split", "Split", 12000, "Ulica Domovinskog rata 2", false, 2019),
                new Arena(3, "Arena Rijeka", "Rijeka", 10000, "Trg Riječke rezolucije 2", true, 2014)
            };
        }

        public List<Arena> GetAll() => _data;
        public Arena? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    }
}
