using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class WeightClassEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv kategorije težine je obavezan.")]
        public string Name { get; set; } = null!;
    }
}
