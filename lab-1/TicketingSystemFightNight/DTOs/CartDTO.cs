using System.Collections.Generic;

namespace TicketingSystemFightNight.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string? DiscountCode { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsPaid { get; set; }
        public decimal TotalPrice { get; set; }
        public UserDTO User { get; set; } = null!;
        public List<TicketDTO> Tickets { get; set; } = new();
    }
}
