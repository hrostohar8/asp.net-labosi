using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class ArenaCreateViewModel
    {
        [Required(ErrorMessage = "Naziv arene je obavezan")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Naziv mora biti između 3 i 200 znakova")]
        [Display(Name = "Naziv arene")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Grad je obavezan")]
        [StringLength(100, ErrorMessage = "Naziv grada ne smije biti duži od 100 znakova")]
        [Display(Name = "Grad")]
        public string City { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Adresa ne smije biti duža od 200 znakova")]
        [Display(Name = "Adresa")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Kapacitet je obavezan")]
        [Range(100, 100000, ErrorMessage = "Kapacitet mora biti između 100 i 100000")]
        [Display(Name = "Kapacitet")]
        public int Capacity { get; set; }

        [Display(Name = "Unutarnja arena")]
        public bool IsIndoor { get; set; }

        [Required(ErrorMessage = "Godina otvaranja je obavezna")]
        [Range(1800, 2100, ErrorMessage = "Godina otvaranja mora biti između 1800. i 2100.")]
        [Display(Name = "Godina otvaranja")]
        public int OpenedYear { get; set; }
    }
}
