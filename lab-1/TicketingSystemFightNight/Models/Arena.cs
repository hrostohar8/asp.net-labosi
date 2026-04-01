using System;

namespace TicketingSystemFightNight.Models
{
    public class Arena
    {
        public string Name { get; set; }
        public string City { get; set; }
        public int Capacity { get; set; }

        public Arena(string name, string city, int capacity)
        {
            Name = name;
            City = city;
            Capacity = capacity;
        }
    }
}