using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models.ViewModels
{
    public class TicketShopModelsCreateViewModel
    {
        [Required(ErrorMessage = "Naslov ponude je obavezan.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Naziv događaja je obavezan.")]
        public string EventName { get; set; } = null!;

        [Required(ErrorMessage = "Lokacija je obavezna.")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "Sekcija je obavezna.")]
        public string Section { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Red je obavezan i mora biti pozitivan.")]
        public int Row { get; set; }

        [Required(ErrorMessage = "Sjedalo je obavezno.")]
        public string Seat { get; set; } = null!;

        [Range(0.01, 10000, ErrorMessage = "Cijena mora biti veća od 0.")]
        public decimal Price { get; set; }

        public bool IsVip { get; set; }

        [Required(ErrorMessage = "Opis je obavezan.")]
        public string Description { get; set; } = null!;
    }
}
