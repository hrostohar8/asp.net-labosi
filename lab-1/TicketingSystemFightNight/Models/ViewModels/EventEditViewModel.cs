using System;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class EventEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv eventa je obavezan")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Naziv mora biti između 3 i 200 znakova")]
        [Display(Name = "Naziv eventa")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Organizacija je obavezna")]
        [Range(1, int.MaxValue, ErrorMessage = "Organizacija je obavezna")]
        [Display(Name = "Organizacija")]
        public int OrganizationId { get; set; }

        [Required(ErrorMessage = "Grad je obavezan")]
        [StringLength(100, ErrorMessage = "Grad ne smije biti duži od 100 znakova")]
        [Display(Name = "Grad")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Datum je obavezan")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Vrijeme je obavezno")]
        [DataType(DataType.Time)]
        [Display(Name = "Vrijeme")]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Odaberite arenu")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite arenu")]
        [Display(Name = "Arena")]
        public int VenueId { get; set; }

        [Display(Name = "Opis")]
        [StringLength(1000, ErrorMessage = "Opis ne smije biti duži od 1000 znakova")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Cijena karte je obavezna")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Cijena mora biti pozitivna")]
        [Display(Name = "Osnovna cijena karte")]
        public decimal BaseTicketPrice { get; set; }

        [Required(ErrorMessage = "Broj prodanih karata je obavezan")]
        [Range(0, int.MaxValue, ErrorMessage = "Broj prodanih karata mora biti nenegativan")]
        [Display(Name = "Prodano karata")]
        public int TicketsSold { get; set; }
    }
}
