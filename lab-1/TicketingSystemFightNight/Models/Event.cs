using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystemFightNight.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv događaja je obavezan")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Naziv mora biti između 3 i 200 znakova")]
        public string Name { get; set; } = null!;

        [ForeignKey("Organization")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite organizaciju")]
        public int OrganizationId { get; set; }
        public virtual FightOrganization? Organization { get; set; }

        [Required(ErrorMessage = "Grad je obavezan")]
        [StringLength(100, ErrorMessage = "Grad ne smije biti duži od 100 znakova")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Datum je obavezan")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Vrijeme je obavezno")]
        public TimeSpan Time { get; set; }

        [ForeignKey("Venue")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite arenu")]
        public int VenueId { get; set; }
        public virtual Arena? Venue { get; set; }

        [StringLength(1000, ErrorMessage = "Opis ne smije biti duži od 1000 znakova")]
        public string Description { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Cijena mora biti pozitivna")]
        public decimal BaseTicketPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Broj prodanih karata mora biti nenegativan")]
        public int TicketsSold { get; set; }
        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public virtual ICollection<EventAttachment> Attachments { get; set; } = new List<EventAttachment>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Event() { }

        public Event(int id, string name, int organizationId, string city, DateTime date, TimeSpan time, Arena venue, string description, decimal baseTicketPrice, int ticketsSold)
        {
            Id = id;
            Name = name;
            OrganizationId = organizationId;
            City = city;
            Date = date;
            Time = time;
            Venue = venue;
            VenueId = venue.Id;
            Description = description;
            BaseTicketPrice = baseTicketPrice;
            TicketsSold = ticketsSold;
        }
    }
} 
