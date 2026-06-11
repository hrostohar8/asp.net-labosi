namespace TicketingSystemFightNight.DTOs
{
    public class MatchDTO
    {
        public int Id { get; set; }
        public int RoundLimit { get; set; }
        public bool Championship { get; set; }
        public string Referee { get; set; } = null!;
        public string Status { get; set; } = null!;
        public FighterDTO Fighter1 { get; set; } = null!;
        public FighterDTO Fighter2 { get; set; } = null!;
        public WeightClassDTO WeightClass { get; set; } = null!;
        public EventDTO Event { get; set; } = null!;
    }
}
