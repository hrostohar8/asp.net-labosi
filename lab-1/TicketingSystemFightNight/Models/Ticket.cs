using System;

namespace TicketingSystemFightNight.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Event Event { get; set; }
        public string Section { get; set; }
        public int Row { get; set; }
        public string Seat { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsVip { get; set; }

        public Ticket(int id, Event event_, string section, int row, string seat, decimal price, DateTime purchaseDate, bool isVip)
        {
            Id = id;
            Event = event_;
            Section = section;
            Row = row;
            Seat = seat;
            Price = price;
            PurchaseDate = purchaseDate;
            IsVip = isVip;
        }
    }
}