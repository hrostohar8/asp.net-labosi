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
        public string Name { get; set; } = null!;
        public FightOrganization Organization { get; set; }
        public string City { get; set; } = null!;
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        [ForeignKey("Venue")]
        public int VenueId { get; set; }
        public virtual Arena Venue { get; set; } = null!;

        public string Description { get; set; } = null!;
        public decimal BaseTicketPrice { get; set; }
        public int TicketsSold { get; set; }
        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        public Event() { }

        public Event(int id, string name, FightOrganization organization, string city, DateTime date, TimeSpan time, Arena venue, string description, decimal baseTicketPrice, int ticketsSold)
        {
            Id = id;
            Name = name;
            Organization = organization;
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
