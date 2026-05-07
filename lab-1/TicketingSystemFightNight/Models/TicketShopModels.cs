using System.Collections.Generic;
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
}
