using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemFightNight.Models
{
    public class Arena
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv arene je obavezan")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Naziv mora biti između 3 i 200 znakova")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Grad je obavezan")]
        [StringLength(100, ErrorMessage = "Naziv grada ne smije biti duži od 100 znakova")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Kapacitet je obavezan")]
        [Range(100, 100000, ErrorMessage = "Kapacitet mora biti između 100 i 100000")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Adresa je obavezna")]
        [StringLength(200, ErrorMessage = "Adresa ne smije biti duža od 200 znakova")]
        public string Address { get; set; } = null!;

        public bool IsIndoor { get; set; }

        [Required(ErrorMessage = "Godina otvaranja je obavezna")]
        [Range(1800, 2100, ErrorMessage = "Godina otvaranja mora biti između 1800. i 2100.")]
        public int OpenedYear { get; set; }

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Arena() { }

        public Arena(int id, string name, string city, int capacity, string address, bool isIndoor, int openedYear)
        {
            Id = id;
            Name = name;
            City = city;
            Capacity = capacity;
            Address = address;
            IsIndoor = isIndoor;
            OpenedYear = openedYear;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
