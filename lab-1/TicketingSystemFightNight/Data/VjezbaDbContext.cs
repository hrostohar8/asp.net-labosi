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
        public DbSet<FightOrganization> FightOrganizations => Set<FightOrganization>();
        public DbSet<WeightClass> WeightClasses => Set<WeightClass>();
        public DbSet<TicketShopModels> TicketShopModels => Set<TicketShopModels>();

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

            var fightOrganizations = new[]
            {
                new FightOrganization { Id = 1, Name = "UFC" },
                new FightOrganization { Id = 2, Name = "KSW" },
                new FightOrganization { Id = 3, Name = "FNC" },
                new FightOrganization { Id = 4, Name = "BELLATOR" },
                new FightOrganization { Id = 5, Name = "ONE FC" }
            };

            var weightClasses = new[]
            {
                new WeightClass { Id = 1, Name = "Flyweight" },
                new WeightClass { Id = 2, Name = "Bantamweight" },
                new WeightClass { Id = 3, Name = "Featherweight" },
                new WeightClass { Id = 4, Name = "Lightweight" },
                new WeightClass { Id = 5, Name = "Welterweight" },
                new WeightClass { Id = 6, Name = "Middleweight" },
                new WeightClass { Id = 7, Name = "LightHeavyweight" },
                new WeightClass { Id = 8, Name = "Heavyweight" }
            };

            var arenas = new[]
            {
                new Arena(1, "Arena Zagreb", "Zagreb", 15000, "Ulica Grada Vukovara 269a", true, 2008),
                new Arena(2, "Arena Split", "Split", 12000, "Ulica Domovinskog rata 2", false, 2019),
                new Arena(3, "Arena Rijeka", "Rijeka", 10000, "Trg Riječke rezolucije 2", true, 2014),
                new Arena(4, "Ljubljana Arena", "Ljubljana", 14000, "Štefanova cesta 6", true, 2002)
            };

            var fighters = new[]
            {
                new Fighter { Id = 1, Name = "Jon Jones", Nickname = "Bones", WeightClassId = 7, OrganizationId = 1, Country = "USA", Wins = 26, Losses = 1 },
                new Fighter { Id = 2, Name = "Francis Ngannou", Nickname = "The Predator", WeightClassId = 8, OrganizationId = 1, Country = "Cameroon", Wins = 16, Losses = 3 },
                new Fighter { Id = 3, Name = "Alexander Gustafsson", Nickname = "The Mauler", WeightClassId = 7, OrganizationId = 1, Country = "Sweden", Wins = 18, Losses = 7 },
                new Fighter { Id = 4, Name = "Daniel Cormier", Nickname = "DC", WeightClassId = 7, OrganizationId = 1, Country = "USA", Wins = 22, Losses = 3 },
                new Fighter { Id = 5, Name = "Stipe Miocic", Nickname = "The Croatian Sensation", WeightClassId = 8, OrganizationId = 1, Country = "USA", Wins = 20, Losses = 4 },
                new Fighter { Id = 6, Name = "Amanda Nunes", Nickname = "The Lioness", WeightClassId = 2, OrganizationId = 1, Country = "Brazil", Wins = 21, Losses = 5 },
                new Fighter { Id = 7, Name = "Matej Batinić", Nickname = "The Slovenian Storm", WeightClassId = 6, OrganizationId = 3, Country = "Slovenia", Wins = 18, Losses = 2 },
                new Fighter { Id = 8, Name = "Jakob Nedoh", Nickname = "The Ljubljana Lion", WeightClassId = 6, OrganizationId = 3, Country = "Slovenia", Wins = 17, Losses = 3 }
            };

            var events = new[]
            {
                new Event { Id = 1, Name = "UFC Fight Night Zagreb", OrganizationId = 1, City = "Zagreb", Date = new DateTime(2024, 6, 15), Time = TimeSpan.FromHours(19), VenueId = 1, Description = "Main event: Jones vs Gustafsson", BaseTicketPrice = 250m, TicketsSold = 12850 },
                new Event { Id = 2, Name = "UFC Fight Night Split", OrganizationId = 1, City = "Split", Date = new DateTime(2024, 7, 20), Time = TimeSpan.FromHours(19), VenueId = 2, Description = "Main event: Ngannou vs Miocic", BaseTicketPrice = 180m, TicketsSold = 9800 },
                new Event { Id = 3, Name = "UFC Fight Night Rijeka", OrganizationId = 1, City = "Rijeka", Date = new DateTime(2024, 8, 10), Time = TimeSpan.FromHours(19), VenueId = 3, Description = "Main event: Cormier vs Gustafsson", BaseTicketPrice = 150m, TicketsSold = 7600 },
                new Event { Id = 4, Name = "FNF 29 Ljubljana", OrganizationId = 3, City = "Ljubljana", Date = new DateTime(2026, 4, 11), Time = new TimeSpan(19, 0, 0), VenueId = 4, Description = "Main event: Batinić vs Nedoh", BaseTicketPrice = 220m, TicketsSold = 5600 }
            };

            var matches = new[]
            {
                new Match { Id = 1, Fighter1Id = 1, Fighter2Id = 3, WeightClassId = 7, EventId = 1, RoundLimit = 5, Championship = true, Referee = "Herb Dean", Status = "Scheduled" },
                new Match { Id = 2, Fighter1Id = 2, Fighter2Id = 5, WeightClassId = 8, EventId = 2, RoundLimit = 5, Championship = false, Referee = "John McCarthy", Status = "Scheduled" },
                new Match { Id = 3, Fighter1Id = 4, Fighter2Id = 3, WeightClassId = 7, EventId = 3, RoundLimit = 5, Championship = false, Referee = "Yves Lavigne", Status = "Scheduled" },
                new Match { Id = 4, Fighter1Id = 7, Fighter2Id = 8, WeightClassId = 6, EventId = 4, RoundLimit = 5, Championship = false, Referee = "Slavko Kosanović", Status = "Scheduled" }
            };

            var users = new[]
            {
                new User(1, "admin", "admin@example.com", "+385911234567", new DateTime(1990, 5, 15), 150, true, "Administrator"),
                new User(2, "john_doe", "john.doe@example.com", "+385912345678", new DateTime(1988, 3, 22), 200, false, "Regular User")
            };

            modelBuilder.Entity<FightOrganization>().HasData(fightOrganizations);
            modelBuilder.Entity<WeightClass>().HasData(weightClasses);
            modelBuilder.Entity<Arena>().HasData(arenas);
            modelBuilder.Entity<Fighter>().HasData(fighters);
            modelBuilder.Entity<Event>().HasData(events);
            modelBuilder.Entity<Match>().HasData(matches);
            modelBuilder.Entity<User>().HasData(users);
        }
    }
}