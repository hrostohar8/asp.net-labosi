namespace TicketingSystemFightNight.Models
{
    public class Match
    {
        public int Id { get; set; }
        public Fighter Fighter1 { get; set; }
        public Fighter Fighter2 { get; set; }
        public WeightClass WeightClass { get; set; }
        public Event Event { get; set; }
        public int RoundLimit { get; set; }
        public bool Championship { get; set; }
        public string Referee { get; set; }
        public string Status { get; set; }

        public Match(int id, Fighter fighter1, Fighter fighter2, WeightClass weightClass, Event event_, int roundLimit, bool championship, string referee, string status)
        {
            Id = id;
            Fighter1 = fighter1;
            Fighter2 = fighter2;
            WeightClass = weightClass;
            Event = event_;
            RoundLimit = roundLimit;
            Championship = championship;
            Referee = referee;
            Status = status;
        }
    }
}