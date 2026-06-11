using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Tests.Helpers
{
    public static class TestDataSeeder
    {
        public static async Task<AppUser> CreateAdminUserAsync(IServiceScope scope)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in new[] { "Admin", "Manager", "Staff" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var email = "testadmin@test.com";
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FullName = "Test Admin",
                    OIB = "12345678901",
                    JMBG = "1234567890123",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Admin123!");
                await userManager.AddToRoleAsync(user, "Admin");
            }
            return user;
        }

        public static async Task<Arena> CreateArenaAsync(VjezbaDbContext db)
        {
            var arena = new Arena
            {
                Name = "Test Arena",
                City = "Zagreb",
                Capacity = 5000,
                Address = "Test Street 1",
                IsIndoor = true,
                OpenedYear = 2010,
                CreatedAt = DateTime.UtcNow
            };
            db.Arenas.Add(arena);
            await db.SaveChangesAsync();
            return arena;
        }

        public static async Task<FightOrganization> CreateOrganizationAsync(VjezbaDbContext db)
        {
            var org = new FightOrganization
            {
                Name = "Test UFC",
                CreatedAt = DateTime.UtcNow
            };
            db.FightOrganizations.Add(org);
            await db.SaveChangesAsync();
            return org;
        }

        public static async Task<WeightClass> CreateWeightClassAsync(VjezbaDbContext db)
        {
            var wc = new WeightClass
            {
                Name = "Lightweight",
                CreatedAt = DateTime.UtcNow
            };
            db.WeightClasses.Add(wc);
            await db.SaveChangesAsync();
            return wc;
        }

        public static async Task<Fighter> CreateFighterAsync(VjezbaDbContext db)
        {
            var org = await CreateOrganizationAsync(db);
            var wc = await CreateWeightClassAsync(db);
            var fighter = new Fighter
            {
                Name = "Test Fighter",
                Nickname = "The Test",
                Country = "Croatia",
                WeightClassId = wc.Id,
                OrganizationId = org.Id,
                Wins = 10,
                Losses = 2,
                CreatedAt = DateTime.UtcNow
            };
            db.Fighters.Add(fighter);
            await db.SaveChangesAsync();
            return fighter;
        }

        public static async Task<Event> CreateEventAsync(VjezbaDbContext db)
        {
            var arena = await CreateArenaAsync(db);
            var org = await CreateOrganizationAsync(db);
            var eventEntity = new Event
            {
                Name = "Test Event",
                City = "Zagreb",
                OrganizationId = org.Id,
                VenueId = arena.Id,
                Date = DateTime.UtcNow.AddMonths(1),
                Time = TimeSpan.FromHours(19),
                Description = "Test event description",
                BaseTicketPrice = 100m,
                TicketsSold = 0,
                CreatedAt = DateTime.UtcNow
            };
            db.Events.Add(eventEntity);
            await db.SaveChangesAsync();
            return eventEntity;
        }

        public static async Task<Models.User> CreateUserAsync(VjezbaDbContext db)
        {
            var user = new Models.User
            {
                Name = "Test User",
                Email = "testuser@test.com",
                Phone = "+385911234567",
                BirthDate = new DateTime(1990, 1, 1),
                LoyaltyPoints = 100,
                IsVip = false,
                MemberLevel = "Regular",
                CreatedAt = DateTime.UtcNow
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return user;
        }

        public static async Task<Ticket> CreateTicketAsync(VjezbaDbContext db)
        {
            var eventEntity = await CreateEventAsync(db);
            var ticket = new Ticket
            {
                EventId = eventEntity.Id,
                Section = "A",
                Row = 1,
                Seat = "1",
                Price = 150m,
                PurchaseDate = DateTime.UtcNow,
                IsVip = false,
                CreatedAt = DateTime.UtcNow
            };
            db.Tickets.Add(ticket);
            await db.SaveChangesAsync();
            return ticket;
        }

        public static async Task<Cart> CreateCartAsync(VjezbaDbContext db)
        {
            var user = await CreateUserAsync(db);
            var cart = new Cart
            {
                UserId = user.Id,
                DiscountCode = null,
                DiscountPercent = 0m,
                IsPaid = false,
                CreatedAt = DateTime.UtcNow
            };
            db.Carts.Add(cart);
            await db.SaveChangesAsync();
            return cart;
        }

        public static async Task<Match> CreateMatchAsync(VjezbaDbContext db)
        {
            var eventEntity = await CreateEventAsync(db);
            var fighter1 = await CreateFighterAsync(db);
            var fighter2 = await CreateFighterAsync(db);
            var wc = await CreateWeightClassAsync(db);
            var match = new Match
            {
                Fighter1Id = fighter1.Id,
                Fighter2Id = fighter2.Id,
                WeightClassId = wc.Id,
                EventId = eventEntity.Id,
                RoundLimit = 5,
                Championship = false,
                Referee = "Test Referee",
                Status = "Scheduled",
                CreatedAt = DateTime.UtcNow
            };
            db.Matches.Add(match);
            await db.SaveChangesAsync();
            return match;
        }

        public static async Task<TicketShopModels> CreateTicketShopModelAsync(VjezbaDbContext db)
        {
            var tsm = new TicketShopModels
            {
                Title = "VIP Package",
                EventName = "Test Fight Night",
                Location = "Zagreb",
                Section = "VIP",
                Row = 1,
                Seat = "1",
                Price = 500m,
                IsVip = true,
                Description = "VIP ticket package",
                CreatedAt = DateTime.UtcNow
            };
            db.TicketShopModels.Add(tsm);
            await db.SaveChangesAsync();
            return tsm;
        }
    }
}
