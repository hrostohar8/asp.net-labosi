using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystemFightNight.Models
{
    public class Fighter
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime borca je obavezno")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ime mora biti između 2 i 100 znakova")]
        public string Name { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Nadimak ne smije biti duži od 100 znakova")]
        public string Nickname { get; set; } = null!;

        [ForeignKey("WeightClass")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite težinsku kategoriju")]
        public int WeightClassId { get; set; }
        public virtual WeightClass WeightClass { get; set; } = null!;

        [ForeignKey("Organization")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite organizaciju")]
        public int OrganizationId { get; set; }
        public virtual FightOrganization Organization { get; set; } = null!;

        [Required(ErrorMessage = "Država je obavezna")]
        [StringLength(100, ErrorMessage = "Naziv države ne smije biti duži od 100 znakova")]
        public string Country { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Broj pobjeda ne može biti negativan")]
        public int Wins { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Broj poraza ne može biti negativan")]
        public int Losses { get; set; }

        [NotMapped]
        public string ImageUrl { get; set; } = "/images/fighter-placeholder.svg";

        public virtual ICollection<Match> MatchesAsFighter1 { get; set; } = new List<Match>();
        public virtual ICollection<Match> MatchesAsFighter2 { get; set; } = new List<Match>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Fighter() { }

        public Fighter(int id, string name, string nickname, int weightClassId, int organizationId, string country, int wins, int losses)
        {
            Id = id;
            Name = name;
            Nickname = nickname;
            WeightClassId = weightClassId;
            OrganizationId = organizationId;
            Country = country;
            Wins = wins;
            Losses = losses;
        }
    }
} 
