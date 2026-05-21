using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class WeightClassCreateViewModel
    {
        [Required(ErrorMessage = "Naziv kategorije težine je obavezan.")]
        public string Name { get; set; } = null!;
    }
}
