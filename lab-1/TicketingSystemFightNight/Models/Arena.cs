using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models
{
    public class Arena
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Capacity { get; set; }
        public string Address { get; set; } = null!;
        public bool IsIndoor { get; set; }
        public int OpenedYear { get; set; }

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();

        public Arena() { }

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
