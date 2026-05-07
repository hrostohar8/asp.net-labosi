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
        public int EventId { get; set; }
        public virtual Event Event { get; set; } = null!;

        [ForeignKey("Cart")]
        public int? CartId { get; set; }
        public virtual Cart? Cart { get; set; }

        public string Section { get; set; } = null!;
        public int Row { get; set; }
        public string Seat { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsVip { get; set; }

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
