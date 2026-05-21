using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystemFightNight.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Fighter1")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite prvog borca")]
        public int Fighter1Id { get; set; }
        public virtual Fighter Fighter1 { get; set; } = null!;

        [ForeignKey("Fighter2")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite drugog borca")]
        public int Fighter2Id { get; set; }
        public virtual Fighter Fighter2 { get; set; } = null!;

        [ForeignKey("WeightClass")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite težinsku kategoriju")]
        public int WeightClassId { get; set; }
        public virtual WeightClass WeightClass { get; set; } = null!;

        [ForeignKey("Event")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite događaj")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; } = null!;

        [Range(1, 12, ErrorMessage = "Broj rundi mora biti između 1 i 12")]
        public int RoundLimit { get; set; }
        public bool Championship { get; set; }
        [Required(ErrorMessage = "Ime sudca je obavezno")]
        [StringLength(150, ErrorMessage = "Ime sudca ne smije biti duže od 150 znakova")]
        public string Referee { get; set; } = null!;
        [Required(ErrorMessage = "Status meča je obavezan")]
        [StringLength(100, ErrorMessage = "Status ne smije biti duži od 100 znakova")]
        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Match() { }

        public Match(int id, Fighter fighter1, Fighter fighter2, int weightClassId, Event @event, int roundLimit, bool championship, string referee, string status)
        {
            Id = id;
            Fighter1 = fighter1;
            Fighter1Id = fighter1.Id;
            Fighter2 = fighter2;
            Fighter2Id = fighter2.Id;
            WeightClassId = weightClassId;
            Event = @event;
            EventId = @event.Id;
            RoundLimit = roundLimit;
            Championship = championship;
            Referee = referee;
            Status = status;
        }
    }
}
