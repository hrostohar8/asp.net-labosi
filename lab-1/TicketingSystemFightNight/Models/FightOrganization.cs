using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TicketingSystemFightNight.Models
{
    public class FightOrganization
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv organizacije je obavezan.")]
        [StringLength(200, ErrorMessage = "Naziv organizacije ne smije biti duži od 200 znakova.")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Fighter> Fighters { get; set; } = new List<Fighter>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}