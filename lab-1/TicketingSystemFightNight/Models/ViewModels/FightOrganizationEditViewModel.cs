using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class FightOrganizationEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv organizacije je obavezan.")]
        public string Name { get; set; } = null!;
    }
}
