using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Data
{
    public class VjezbaDbContext : DbContext
    {
        public VjezbaDbContext(DbContextOptions<VjezbaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Arena> Arenas => Set<Arena>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Fighter> Fighters => Set<Fighter>();
        public DbSet<Match> Matches => Set<Match>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Fighter1)
                .WithMany(f => f.MatchesAsFighter1)
                .HasForeignKey(m => m.Fighter1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Fighter2)
                .WithMany(f => f.MatchesAsFighter2)
                .HasForeignKey(m => m.Fighter2Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            var arenas = new[]
            {
                new Arena(1, "Arena Zagreb", "Zagreb", 15000, "Ulica Grada Vukovara 269a", true, 2008),
                new Arena(2, "Arena Split", "Split", 12000, "Ulica Domovinskog rata 2", false, 2019),
                new Arena(3, "Arena Rijeka", "Rijeka", 10000, "Trg Riječke rezolucije 2", true, 2014)
            };

            var fighters = new[]
            {
                new Fighter(1, "Jon Jones", "Bones", WeightClass.LightHeavyweight, FightOrganization.UFC, "USA", 26, 1),
                new Fighter(2, "Francis Ngannou", "The Predator", WeightClass.Heavyweight, FightOrganization.UFC, "Cameroon", 16, 3),
                new Fighter(3, "Alexander Gustafsson", "The Mauler", WeightClass.LightHeavyweight, FightOrganization.UFC, "Sweden", 18, 7),
                new Fighter(4, "Daniel Cormier", "DC", WeightClass.LightHeavyweight, FightOrganization.UFC, "USA", 22, 3),
                new Fighter(5, "Stipe Miocic", "The Croatian Sensation", WeightClass.Heavyweight, FightOrganization.UFC, "USA", 20, 4),
                new Fighter(6, "Amanda Nunes", "The Lioness", WeightClass.Bantamweight, FightOrganization.UFC, "Brazil", 21, 5)
            };

            var events = new[]
            {
                new Event { Id = 1, Name = "UFC Fight Night Zagreb", Organization = FightOrganization.UFC, City = "Zagreb", Date = new DateTime(2024, 6, 15), Time = TimeSpan.FromHours(19), VenueId = 1, Description = "Main event: Jones vs Gustafsson", BaseTicketPrice = 250m, TicketsSold = 12850 },
                new Event { Id = 2, Name = "UFC Fight Night Split", Organization = FightOrganization.UFC, City = "Split", Date = new DateTime(2024, 7, 20), Time = TimeSpan.FromHours(19), VenueId = 2, Description = "Main event: Ngannou vs Miocic", BaseTicketPrice = 180m, TicketsSold = 9800 },
                new Event { Id = 3, Name = "UFC Fight Night Rijeka", Organization = FightOrganization.UFC, City = "Rijeka", Date = new DateTime(2024, 8, 10), Time = TimeSpan.FromHours(19), VenueId = 3, Description = "Main event: Cormier vs Gustafsson", BaseTicketPrice = 150m, TicketsSold = 7600 }
            };

            var matches = new[]
            {
                new Match { Id = 1, Fighter1Id = 1, Fighter2Id = 3, WeightClass = WeightClass.LightHeavyweight, EventId = 1, RoundLimit = 5, Championship = true, Referee = "Herb Dean", Status = "Scheduled" },
                new Match { Id = 2, Fighter1Id = 2, Fighter2Id = 5, WeightClass = WeightClass.Heavyweight, EventId = 2, RoundLimit = 5, Championship = false, Referee = "John McCarthy", Status = "Scheduled" },
                new Match { Id = 3, Fighter1Id = 4, Fighter2Id = 3, WeightClass = WeightClass.LightHeavyweight, EventId = 3, RoundLimit = 5, Championship = false, Referee = "Yves Lavigne", Status = "Scheduled" }
            };

            var users = new[]
            {
                new User(1, "admin", "admin@example.com", "+385911234567", new DateTime(1990, 5, 15), 150, true, "Administrator"),
                new User(2, "john_doe", "john.doe@example.com", "+385912345678", new DateTime(1988, 3, 22), 200, false, "Regular User")
            };

            modelBuilder.Entity<Arena>().HasData(arenas);
            modelBuilder.Entity<Fighter>().HasData(fighters);
            modelBuilder.Entity<Event>().HasData(events);
            modelBuilder.Entity<Match>().HasData(matches);
            modelBuilder.Entity<User>().HasData(users);
        }
    }
}