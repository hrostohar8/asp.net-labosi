using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TicketingSystemFightNight.Models
{
    public class TicketOffer
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string EventName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Section { get; set; } = null!;
        public int Row { get; set; }
        public string Seat { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsVip { get; set; }
        public string Description { get; set; } = null!;

        public string PriceDisplay => Price.ToString("C");
    }

    public class TicketShopViewModel
    {
        public List<TicketOffer> Offers { get; set; } = new();
        public List<TicketOffer> CartItems { get; set; } = new();
        public decimal CartTotal => CartItems.Sum(x => x.Price);
        public int CartCount => CartItems.Count;
    }

    public class CartPageViewModel
    {
        public List<TicketOffer> CartItems { get; set; } = new();
        public decimal CartTotal => CartItems.Sum(x => x.Price);
        public int CartCount => CartItems.Count;
    }

    public class TicketShopModels
    {
        [Key]
        public int Id { get; set; }

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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
