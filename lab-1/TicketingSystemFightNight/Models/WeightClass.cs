using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TicketingSystemFightNight.Models
{
    public class WeightClass
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv kategorije težine je obavezan.")]
        [StringLength(100, ErrorMessage = "Naziv kategorije težine ne smije biti duži od 100 znakova.")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Fighter> Fighters { get; set; } = new List<Fighter>();
        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}