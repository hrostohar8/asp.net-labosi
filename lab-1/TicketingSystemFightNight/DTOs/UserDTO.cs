using System;

namespace TicketingSystemFightNight.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public int LoyaltyPoints { get; set; }
        public bool IsVip { get; set; }
        public string MemberLevel { get; set; } = null!;
    }
}
