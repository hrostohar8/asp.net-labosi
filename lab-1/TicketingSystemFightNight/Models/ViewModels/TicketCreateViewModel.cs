using System;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class TicketCreateViewModel
    {
        [Required(ErrorMessage = "Odaberite događaj")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite događaj")]
        [Display(Name = "Događaj")]
        public int EventId { get; set; }

        [Display(Name = "Košarica")]
        public int? CartId { get; set; }

        [Required(ErrorMessage = "Dionica je obavezna")]
        [StringLength(50, ErrorMessage = "Dionica ne smije biti duža od 50 znakova")]
        [Display(Name = "Dionica")]
        public string Section { get; set; } = null!;

        [Required(ErrorMessage = "Red je obavezan")]
        [Range(1, 1000, ErrorMessage = "Red mora biti između 1 i 1000")]
        [Display(Name = "Red")]
        public int Row { get; set; }

        [Required(ErrorMessage = "Sjedalo je obavezno")]
        [StringLength(20, ErrorMessage = "Sjedalo ne smije biti duže od 20 znakova")]
        [Display(Name = "Sjedalo")]
        public string Seat { get; set; } = null!;

        [Required(ErrorMessage = "Cijena je obavezna")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Cijena mora biti pozitivna")]
        [Display(Name = "Cijena")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Datum kupnje je obavezan")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum kupnje")]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "VIP karta")]
        public bool IsVip { get; set; }
    }
}