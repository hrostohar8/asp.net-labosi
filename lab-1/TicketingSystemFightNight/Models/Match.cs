namespace TicketingSystemFightNight.Models
{
    public class Match
    {
        public Fighter Fighter1 { get; set; }
        public Fighter Fighter2 { get; set; }
        public WeightClass WeightClass { get; set; }
        public Event Event { get; set; }

        public Match(Fighter fighter1, Fighter fighter2, WeightClass weightClass, Event event_)
        {
            Fighter1 = fighter1;
            Fighter2 = fighter2;
            WeightClass = weightClass;
            Event = event_;
        }
    }
}