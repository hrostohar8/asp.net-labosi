namespace TicketingSystemFightNight.DTOs
{
    public class FighterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public WeightClassDTO WeightClass { get; set; } = null!;
        public FightOrganizationDTO Organization { get; set; } = null!;
    }
}
