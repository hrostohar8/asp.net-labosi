using System;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class UserEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ime mora biti između 2 i 100 znakova")]
        [Display(Name = "Ime")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "E-mail je obavezan")]
        [EmailAddress(ErrorMessage = "Unesite ispravan e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Telefon je obavezan")]
        [Phone(ErrorMessage = "Unesite ispravan telefonski broj")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Datum rođenja je obavezan")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum rođenja")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Broj bodova je obavezan")]
        [Range(0, int.MaxValue, ErrorMessage = "Broj bodova ne može biti negativan")]
        [Display(Name = "Bodovi vjernosti")]
        public int LoyaltyPoints { get; set; }

        [Display(Name = "VIP član")]
        public bool IsVip { get; set; }

        [Required(ErrorMessage = "Razina članstva je obavezna")]
        [StringLength(100, ErrorMessage = "Razina članstva ne smije biti duža od 100 znakova")]
        [Display(Name = "Razina članstva")]
        public string MemberLevel { get; set; } = null!;
    }
}