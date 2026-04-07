using System;

namespace TicketingSystemFightNight.Models
{
    public class Arena
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Capacity { get; set; }
        public string Address { get; set; }
        public bool IsIndoor { get; set; }
        public int OpenedYear { get; set; }

        public Arena(int id, string name, string city, int capacity, string address, bool isIndoor, int openedYear)
        {
            Id = id;
            Name = name;
            City = city;
            Capacity = capacity;
            Address = address;
            IsIndoor = isIndoor;
            OpenedYear = openedYear;
        }
    }
}