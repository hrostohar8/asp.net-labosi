using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystemFightNight.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Event")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite događaj")]
        public int EventId { get; set; }
        public virtual Event? Event { get; set; }

        [ForeignKey("Cart")]
        public int? CartId { get; set; }
        public virtual Cart? Cart { get; set; }

        [Required(ErrorMessage = "Dionica je obavezna")]
        [StringLength(50, ErrorMessage = "Dionica ne smije biti duža od 50 znakova")]
        public string Section { get; set; } = null!;

        [Range(1, 1000, ErrorMessage = "Red mora biti između 1 i 1000")]
        public int Row { get; set; }

        [Required(ErrorMessage = "Sjedalo je obavezno")]
        [StringLength(20, ErrorMessage = "Sjedalo ne smije biti duže od 20 znakova")]
        public string Seat { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Cijena mora biti pozitivna")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Datum kupnje je obavezan")]
        public DateTime PurchaseDate { get; set; }
        public bool IsVip { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Ticket() { }

        public Ticket(int id, Event @event, string section, int row, string seat, decimal price, DateTime purchaseDate, bool isVip)
        {
            Id = id;
            Event = @event;
            EventId = @event?.Id ?? 0;
            Section = section;
            Row = row;
            Seat = seat;
            Price = price;
            PurchaseDate = purchaseDate;
            IsVip = isVip;
        }
    }
}
