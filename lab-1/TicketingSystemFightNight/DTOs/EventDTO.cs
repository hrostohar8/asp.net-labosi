using System;

namespace TicketingSystemFightNight.DTOs
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Time { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal BaseTicketPrice { get; set; }
        public int TicketsSold { get; set; }
        public ArenaDTO Venue { get; set; } = null!;
        public FightOrganizationDTO Organization { get; set; } = null!;
    }
}
