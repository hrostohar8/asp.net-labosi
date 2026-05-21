using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class FightOrganizationCreateViewModel
    {
        [Required(ErrorMessage = "Naziv organizacije je obavezan.")]
        public string Name { get; set; } = null!;
    }
}
