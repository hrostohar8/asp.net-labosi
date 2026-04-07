using System.Collections.Generic;
using System.Linq;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Repositories
{
    public class UserMockRepository
    {
        private readonly List<User> _data;

        public UserMockRepository()
        {
            _data = new List<User>
            {
                new User(1, "Ana Marin", "ana.marin@example.com", "+385912345678", new System.DateTime(1996, 6, 14), 690, true, "Gold"),
                new User(2, "Ivan Horvat", "ivan.horvat@example.com", "+385912331234", new System.DateTime(1990, 3, 22), 240, false, "Silver")
            };
        }

        public List<User> GetAll() => _data;
        public User? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    }
}
