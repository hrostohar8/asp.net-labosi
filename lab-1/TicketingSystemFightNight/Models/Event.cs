using System;
using System.Collections.Generic;

namespace TicketingSystemFightNight.Models
{
    public class Event
    {
        public string Name { get; set; }
        public FightOrganization Organization { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Arena Venue { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();

        public Event(string name, FightOrganization organization, string city, DateTime date, TimeSpan time, Arena venue)
        {
            Name = name;
            Organization = organization;
            City = city;
            Date = date;
            Time = time;
            Venue = venue;
        }
    }
}