using System;

namespace TicketingSystemFightNight.DTOs
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Section { get; set; } = null!;
        public int Row { get; set; }
        public string Seat { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsVip { get; set; }
        public EventDTO Event { get; set; } = null!;
    }
}
