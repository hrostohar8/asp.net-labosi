using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models
{
    public class Fighter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Nickname { get; set; } = null!;
        public WeightClass WeightClass { get; set; }
        public FightOrganization Organization { get; set; }
        public string Country { get; set; } = null!;
        public int Wins { get; set; }
        public int Losses { get; set; }

        public virtual ICollection<Match> MatchesAsFighter1 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesAsFighter2 { get; set; } = new List<Match>();

        public Fighter() { }

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
