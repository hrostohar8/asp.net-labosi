using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class FighterCreateViewModel
    {
        [Required(ErrorMessage = "Ime borca je obavezno")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ime mora biti između 2 i 100 znakova")]
        [Display(Name = "Ime")]
        public string Name { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Nadimak ne smije biti duži od 100 znakova")]
        [Display(Name = "Nadimak")]
        public string Nickname { get; set; } = null!;

        [Required(ErrorMessage = "Težinska kategorija je obavezna")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite težinsku kategoriju")]
        [Display(Name = "Težinska kategorija")]
        public int WeightClassId { get; set; }

        [Required(ErrorMessage = "Organizacija je obavezna")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite organizaciju")]
        [Display(Name = "Organizacija")]
        public int OrganizationId { get; set; }

        [Required(ErrorMessage = "Država je obavezna")]
        [StringLength(100, ErrorMessage = "Naziv države ne smije biti duži od 100 znakova")]
        [Display(Name = "Država")]
        public string Country { get; set; } = null!;

        [Required(ErrorMessage = "Broj pobjeda je obavezan")]
        [Range(0, 100, ErrorMessage = "Broj pobjeda ne može biti negativan")]
        [Display(Name = "Pobjede")]
        public int Wins { get; set; }

        [Required(ErrorMessage = "Broj poraza je obavezan")]
        [Range(0, 100, ErrorMessage = "Broj poraza ne može biti negativan")]
        [Display(Name = "Porazi")]
        public int Losses { get; set; }
    }
}
