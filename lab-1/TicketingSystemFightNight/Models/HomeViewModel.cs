namespace TicketingSystemFightNight.Models
{
    public class HomeViewModel
    {
        public Event? UpcomingEvent { get; set; }
        public Fighter? Fighter1 { get; set; }
        public Fighter? Fighter2 { get; set; }
        public string? Fighter1ImageUrl { get; set; }
        public string? Fighter2ImageUrl { get; set; }
    }
}
