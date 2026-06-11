using System;

namespace TicketingSystemFightNight.DTOs
{
    public class ArenaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Capacity { get; set; }
        public string Address { get; set; } = null!;
        public bool IsIndoor { get; set; }
        public int OpenedYear { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
