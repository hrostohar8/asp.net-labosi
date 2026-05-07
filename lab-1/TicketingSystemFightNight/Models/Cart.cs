using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TicketingSystemFightNight.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public DateTime CreatedAt { get; set; }
        public string? DiscountCode { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsPaid { get; set; }

        public decimal TotalPrice => Tickets.Sum(t => t.Price) * (1 - DiscountPercent / 100m);

        public Cart() { CreatedAt = DateTime.Now; }

        public Cart(int id, User user, string? discountCode = null, decimal discountPercent = 0m)
        {
            Id = id;
            User = user;
            UserId = user.Id;
            CreatedAt = DateTime.Now;
            DiscountCode = discountCode;
            DiscountPercent = discountPercent;
            IsPaid = false;
        }
    }
}
