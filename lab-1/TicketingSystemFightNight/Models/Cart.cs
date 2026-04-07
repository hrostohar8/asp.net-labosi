using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketingSystemFightNight.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
        public DateTime CreatedAt { get; set; }
        public string? DiscountCode { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsPaid { get; set; }

        public decimal TotalPrice => Tickets.Sum(t => t.Price) * (1 - DiscountPercent / 100m);

        public Cart(int id, User user, string? discountCode = null, decimal discountPercent = 0m)
        {
            Id = id;
            User = user;
            CreatedAt = DateTime.Now;
            DiscountCode = discountCode;
            DiscountPercent = discountPercent;
            IsPaid = false;
        }
    }
}