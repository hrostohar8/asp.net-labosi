using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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
                new Arena(3, "Arena Rijeka", "Rijeka", 10000, "Trg Riječke rezolucije 2", true, 2014)
            };

            var fighters = new[]
            {
                new Fighter { Id = 1, Name = "Jon Jones", Nickname = "Bones", WeightClassId = 7, OrganizationId = 1, Country = "USA", Wins = 26, Losses = 1 },
                new Fighter { Id = 2, Name = "Francis Ngannou", Nickname = "The Predator", WeightClassId = 8, OrganizationId = 1, Country = "Cameroon", Wins = 16, Losses = 3 },
                new Fighter { Id = 3, Name = "Alexander Gustafsson", Nickname = "The Mauler", WeightClassId = 7, OrganizationId = 1, Country = "Sweden", Wins = 18, Losses = 7 },
                new Fighter { Id = 4, Name = "Daniel Cormier", Nickname = "DC", WeightClassId = 7, OrganizationId = 1, Country = "USA", Wins = 22, Losses = 3 },
                new Fighter { Id = 5, Name = "Stipe Miocic", Nickname = "The Croatian Sensation", WeightClassId = 8, OrganizationId = 1, Country = "USA", Wins = 20, Losses = 4 },
                new Fighter { Id = 6, Name = "Junior Dos Santos", Nickname = "Cigano", WeightClassId = 8, OrganizationId = 1, Country = "Brazil", Wins = 21, Losses = 9 },
                new Fighter { Id = 7, Name = "Conor McGregor", Nickname = "The Notorious", WeightClassId = 4, OrganizationId = 1, Country = "Ireland", Wins = 22, Losses = 6 },
                new Fighter { Id = 8, Name = "Khabib Nurmagomedov", Nickname = "The Eagle", WeightClassId = 4, OrganizationId = 1, Country = "Russia", Wins = 29, Losses = 0 }
            };

            var events = new[]
            {
                new Event { Id = 1, Name = "UFC 300", OrganizationId = 1, City = "Zagreb", Date = DateTime.Today.AddDays(25), Time = TimeSpan.FromHours(19), VenueId = 1, Description = "Glavni UFC spektakl u Hrvatskoj", BaseTicketPrice = 250m, TicketsSold = 12850 },
                new Event { Id = 2, Name = "Bellator 295", OrganizationId = 4, City = "Split", Date = DateTime.Today.AddDays(40), Time = TimeSpan.FromHours(19), VenueId = 2, Description = "Najbolji Bellator mečevi u Dalmaciji", BaseTicketPrice = 180m, TicketsSold = 9800 },
                new Event { Id = 3, Name = "KSW 74", OrganizationId = 2, City = "Rijeka", Date = DateTime.Today.AddDays(60), Time = TimeSpan.FromHours(19), VenueId = 3, Description = "KSW ekskluziva manje od sat vremena od Rijeke", BaseTicketPrice = 150m, TicketsSold = 7600 }
            };

            var matches = new[]
            {
                new Match { Id = 1, Fighter1Id = 1, Fighter2Id = 3, WeightClassId = 7, EventId = 1, RoundLimit = 5, Championship = true, Referee = "Herb Dean", Status = "Scheduled" },
                new Match { Id = 2, Fighter1Id = 2, Fighter2Id = 5, WeightClassId = 8, EventId = 1, RoundLimit = 5, Championship = false, Referee = "John McCarthy", Status = "Scheduled" },
                new Match { Id = 3, Fighter1Id = 7, Fighter2Id = 8, WeightClassId = 4, EventId = 2, RoundLimit = 5, Championship = true, Referee = "Marc Goddard", Status = "Scheduled" },
                new Match { Id = 4, Fighter1Id = 4, Fighter2Id = 6, WeightClassId = 7, EventId = 3, RoundLimit = 5, Championship = false, Referee = "Yves Lavigne", Status = "Scheduled" }
            };

            var users = new[]
            {
                new User(1, "Hrvoje Rostohar", "hrvoje@example.com", "+385911234567", new DateTime(1990, 5, 15), 150, true, "VIP"),
                new User(2, "Ana Kovač", "ana@example.com", "+385912345678", new DateTime(1988, 3, 22), 200, false, "Gold"),
                new User(3, "Marko Horvat", "marko@example.com", "+385913456789", new DateTime(1992, 7, 10), 75, false, "Silver")
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
