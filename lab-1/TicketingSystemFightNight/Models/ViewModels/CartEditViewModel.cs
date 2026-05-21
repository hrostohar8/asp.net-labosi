using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class CartEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Odaberite korisnika")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite korisnika")]
        [Display(Name = "Korisnik")]
        public int UserId { get; set; }

        [StringLength(50, ErrorMessage = "Kod za popust ne smije biti duži od 50 znakova")]
        [Display(Name = "Kod za popust")]
        public string? DiscountCode { get; set; }

        [Range(0, 100, ErrorMessage = "Postotak popusta mora biti između 0 i 100")]
        [Display(Name = "Postotak popusta")]
        public decimal DiscountPercent { get; set; }

        [Display(Name = "Je li plaćeno")]
        public bool IsPaid { get; set; }
    }
}
