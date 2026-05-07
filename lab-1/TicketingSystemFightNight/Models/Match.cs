using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystemFightNight.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Fighter1")]
        public int Fighter1Id { get; set; }
        public virtual Fighter Fighter1 { get; set; } = null!;

        [ForeignKey("Fighter2")]
        public int Fighter2Id { get; set; }
        public virtual Fighter Fighter2 { get; set; } = null!;

        public WeightClass WeightClass { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; } = null!;

        public int RoundLimit { get; set; }
        public bool Championship { get; set; }
        public string Referee { get; set; } = null!;
        public string Status { get; set; } = null!;

        public Match() { }

        public Match(int id, Fighter fighter1, Fighter fighter2, WeightClass weightClass, Event @event, int roundLimit, bool championship, string referee, string status)
        {
            Id = id;
            Fighter1 = fighter1;
            Fighter1Id = fighter1.Id;
            Fighter2 = fighter2;
            Fighter2Id = fighter2.Id;
            WeightClass = weightClass;
            Event = @event;
            EventId = @event.Id;
            RoundLimit = roundLimit;
            Championship = championship;
            Referee = referee;
            Status = status;
        }
    }
}
