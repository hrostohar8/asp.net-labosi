using System;

namespace TicketingSystemFightNight.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int LoyaltyPoints { get; set; }
        public bool IsVip { get; set; }
        public string MemberLevel { get; set; }

        public User(int id, string name, string email, string phone, DateTime birthDate, int loyaltyPoints, bool isVip, string memberLevel)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            BirthDate = birthDate;
            LoyaltyPoints = loyaltyPoints;
            IsVip = isVip;
            MemberLevel = memberLevel;
        }
    }
}