using System;
using System.Collections.Generic;

namespace TicketingSystemFightNight.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FightOrganization Organization { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Arena Venue { get; set; }
        public string Description { get; set; }
        public decimal BaseTicketPrice { get; set; }
        public int TicketsSold { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();

        public Event(int id, string name, FightOrganization organization, string city, DateTime date, TimeSpan time, Arena venue, string description, decimal baseTicketPrice, int ticketsSold)
        {
            Id = id;
            Name = name;
            Organization = organization;
            City = city;
            Date = date;
            Time = time;
            Venue = venue;
            Description = description;
            BaseTicketPrice = baseTicketPrice;
            TicketsSold = ticketsSold;
        }
    }
}