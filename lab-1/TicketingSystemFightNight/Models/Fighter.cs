using System;

namespace TicketingSystemFightNight.Models
{
    public class Fighter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public WeightClass WeightClass { get; set; }
        public FightOrganization Organization { get; set; }
        public string Country { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public Fighter(int id, string name, string nickname, WeightClass weightClass, FightOrganization organization, string country, int wins, int losses)
        {
            Id = id;
            Name = name;
            Nickname = nickname;
            WeightClass = weightClass;
            Organization = organization;
            Country = country;
            Wins = wins;
            Losses = losses;
        }
    }
}