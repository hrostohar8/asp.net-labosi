using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class MatchEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Odaberite prvog borca")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite prvog borca")]
        [Display(Name = "Borac 1")]
        public int Fighter1Id { get; set; }

        [Required(ErrorMessage = "Odaberite drugog borca")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite drugog borca")]
        [Display(Name = "Borac 2")]
        public int Fighter2Id { get; set; }

        [Required(ErrorMessage = "Odaberite težinsku kategoriju")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite težinsku kategoriju")]
        [Display(Name = "Težinska kategorija")]
        public int WeightClassId { get; set; }

        [Required(ErrorMessage = "Odaberite događaj")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite događaj")]
        [Display(Name = "Događaj")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Broj rundi je obavezan")]
        [Range(1, 12, ErrorMessage = "Broj rundi mora biti između 1 i 12")]
        [Display(Name = "Broj rundi")]
        public int RoundLimit { get; set; }

        [Display(Name = "Natjecanje za pojas")]
        public bool Championship { get; set; }

        [Required(ErrorMessage = "Ime sudca je obavezno")]
        [StringLength(150, ErrorMessage = "Ime sudca ne smije biti duže od 150 znakova")]
        [Display(Name = "Sudac")]
        public string Referee { get; set; } = null!;

        [Required(ErrorMessage = "Status meča je obavezan")]
        [StringLength(100, ErrorMessage = "Status ne smije biti duži od 100 znakova")]
        [Display(Name = "Status")]
        public string Status { get; set; } = null!;
    }
}