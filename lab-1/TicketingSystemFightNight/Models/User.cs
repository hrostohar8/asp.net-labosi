using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public int LoyaltyPoints { get; set; }
        public bool IsVip { get; set; }
        public string MemberLevel { get; set; } = null!;

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public User() { }

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
