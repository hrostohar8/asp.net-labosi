using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ime mora biti između 2 i 100 znakova")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "E-mail je obavezan")]
        [EmailAddress(ErrorMessage = "Unesite ispravan e-mail")]
        [StringLength(100, ErrorMessage = "E-mail ne smije biti duži od 100 znakova")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Telefon je obavezan")]
        [Phone(ErrorMessage = "Unesite ispravan telefonski broj")]
        [StringLength(20, ErrorMessage = "Telefon ne smije biti duži od 20 znakova")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Datum rođenja je obavezan")]
        public DateTime BirthDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Broj bodova ne može biti negativan")]
        public int LoyaltyPoints { get; set; }

        public bool IsVip { get; set; }

        [Required(ErrorMessage = "Razina članstva je obavezna")]
        [StringLength(100, ErrorMessage = "Razina članstva ne smije biti duža od 100 znakova")]
        public string MemberLevel { get; set; } = null!;

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

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
