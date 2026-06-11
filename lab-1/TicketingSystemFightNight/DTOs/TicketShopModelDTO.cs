namespace TicketingSystemFightNight.DTOs
{
    public class TicketShopModelDTO
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
    }
}
