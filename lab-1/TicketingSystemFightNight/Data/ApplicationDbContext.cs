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
                new Fighter(6, "Junior Dos Santos", "Cigano", WeightClass.Heavyweight, FightOrganization.UFC, "Brazil", 21, 9),
                new Fighter(7, "Conor McGregor", "The Notorious", WeightClass.Lightweight, FightOrganization.UFC, "Ireland", 22, 6),
                new Fighter(8, "Khabib Nurmagomedov", "The Eagle", WeightClass.Lightweight, FightOrganization.UFC, "Russia", 29, 0)
            };

            var events = new[]
            {
                new Event(1, "UFC 300", FightOrganization.UFC, "Zagreb", DateTime.Today.AddDays(25), TimeSpan.FromHours(19), arenas[0], "Glavni UFC spektakl u Hrvatskoj", 250m, 12850),
                new Event(2, "Bellator 295", FightOrganization.BELLATOR, "Split", DateTime.Today.AddDays(40), TimeSpan.FromHours(19), arenas[1], "Najbolji Bellator mečevi u Dalmaciji", 180m, 9800),
                new Event(3, "KSW 74", FightOrganization.KSW, "Rijeka", DateTime.Today.AddDays(60), TimeSpan.FromHours(19), arenas[2], "KSW ekskluziva manje od sat vremena od Rijeke", 150m, 7600)
            };

            var matches = new[]
            {
                new Match(1, fighters[0], fighters[2], WeightClass.LightHeavyweight, events[0], 5, true, "Herb Dean", "Scheduled"),
                new Match(2, fighters[1], fighters[4], WeightClass.Heavyweight, events[0], 5, false, "John McCarthy", "Scheduled"),
                new Match(3, fighters[6], fighters[7], WeightClass.Lightweight, events[1], 5, true, "Marc Goddard", "Scheduled"),
                new Match(4, fighters[3], fighters[5], WeightClass.LightHeavyweight, events[2], 5, false, "Yves Lavigne", "Scheduled")
            };

            var users = new[]
            {
                new User(1, "Hrvoje Rostohar", "hrvoje@example.com", "+385911234567", new DateTime(1990, 5, 15), 150, true, "VIP"),
                new User(2, "Ana Kovač", "ana@example.com", "+385912345678", new DateTime(1988, 3, 22), 200, false, "Gold"),
                new User(3, "Marko Horvat", "marko@example.com", "+385913456789", new DateTime(1992, 7, 10), 75, false, "Silver")
            };

            modelBuilder.Entity<Arena>().HasData(arenas);
            modelBuilder.Entity<Fighter>().HasData(fighters);
            modelBuilder.Entity<Event>().HasData(events);
            modelBuilder.Entity<Match>().HasData(matches);
            modelBuilder.Entity<User>().HasData(users);
        }
    }
}
