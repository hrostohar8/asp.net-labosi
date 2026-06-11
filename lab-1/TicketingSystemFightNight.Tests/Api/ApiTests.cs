using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.DTOs;
using TicketingSystemFightNight.Models;
using TicketingSystemFightNight.Tests.Helpers;
using Xunit;

namespace TicketingSystemFightNight.Tests.Api
{
    public class ArenaApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public ArenaApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private async Task<Arena> CreateArenaInDb()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            return await TestDataSeeder.CreateArenaAsync(db);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithList()
        {
            await CreateArenaInDb();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/arena");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<ArenaDTO>>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ReturnsArena_WhenExists()
        {
            var arena = await CreateArenaInDb();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync($"/api/arena/{arena.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<ArenaDTO>();
            result!.Id.Should().Be(arena.Id);
            result.Name.Should().Be(arena.Name);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/arena/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesArena_WhenValidModel()
        {
            var client = _factory.CreateAdminClient();
            var newArena = new { Name = "Nova Arena", City = "Split", Capacity = 8000, Address = "Splitska ulica 1", IsIndoor = false, OpenedYear = 2020 };
            var response = await client.PostAsJsonAsync("/api/arena", newArena);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var result = await response.Content.ReadFromJsonAsync<ArenaDTO>();
            result!.Name.Should().Be("Nova Arena");
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenInvalidModel()
        {
            var client = _factory.CreateAdminClient();
            var invalidArena = new { Name = "", City = "", Capacity = -1, Address = "", OpenedYear = 0 };
            var response = await client.PostAsJsonAsync("/api/arena", invalidArena);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_UpdatesArena_WhenExists()
        {
            var arena = await CreateArenaInDb();
            var client = _factory.CreateAdminClient();
            var updated = new { Id = arena.Id, Name = "Updated Arena", City = "Rijeka", Capacity = 6000, Address = "Nova adresa 5", IsIndoor = true, OpenedYear = 2015 };
            var response = await client.PutAsJsonAsync($"/api/arena/{arena.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<ArenaDTO>();
            result!.Name.Should().Be("Updated Arena");
        }

        [Fact]
        public async Task Put_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var updated = new { Id = 99999, Name = "ValidName", City = "ValidCity", Capacity = 1000, Address = "ValidAddress", IsIndoor = false, OpenedYear = 2000 };
            var response = await client.PutAsJsonAsync("/api/arena/99999", updated);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            var arena = await CreateArenaInDb();
            var adminClient = _factory.CreateAdminClient();
            var deleteResponse = await adminClient.DeleteAsync($"/api/arena/{arena.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var anonClient = _factory.CreateAnonClient();
            var getResponse = await anonClient.GetAsync($"/api/arena/{arena.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/arena/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAll_WithSearch_ReturnsFilteredResults()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            db.Arenas.Add(new Arena { Name = "Search Arena Zagreb", City = "Zagreb", Capacity = 5000, Address = "Addr", IsIndoor = true, OpenedYear = 2010, CreatedAt = DateTime.UtcNow });
            await db.SaveChangesAsync();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/arena?q=Search+Arena+Zagreb");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<ArenaDTO>>();
            result.Should().NotBeNull();
            result!.Should().Contain(a => a.Name.Contains("Zagreb") || a.City.Contains("Zagreb"));
        }
    }

    public class FighterApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public FighterApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private async Task<Fighter> SeedFighter()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            return await TestDataSeeder.CreateFighterAsync(db);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithFighters()
        {
            await SeedFighter();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/fighter");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<FighterDTO>>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ReturnsFighter_WhenExists()
        {
            var fighter = await SeedFighter();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync($"/api/fighter/{fighter.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<FighterDTO>();
            result!.Id.Should().Be(fighter.Id);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/fighter/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesFighter_WhenValid()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var org = await TestDataSeeder.CreateOrganizationAsync(db);
            var wc = await TestDataSeeder.CreateWeightClassAsync(db);
            var client = _factory.CreateAdminClient();
            var newFighter = new { Name = "New Fighter", Nickname = "The New", Country = "USA", WeightClassId = wc.Id, OrganizationId = org.Id, Wins = 5, Losses = 1 };
            var response = await client.PostAsJsonAsync("/api/fighter", newFighter);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var result = await response.Content.ReadFromJsonAsync<FighterDTO>();
            result!.Name.Should().Be("New Fighter");
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenInvalidModel()
        {
            var client = _factory.CreateAdminClient();
            var invalid = new { Name = "", Country = "", WeightClassId = 0, OrganizationId = 0 };
            var response = await client.PostAsJsonAsync("/api/fighter", invalid);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_UpdatesFighter_WhenExists()
        {
            var fighter = await SeedFighter();
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var org = await db.FightOrganizations.FindAsync(fighter.OrganizationId);
            var wc = await db.WeightClasses.FindAsync(fighter.WeightClassId);
            var client = _factory.CreateAdminClient();
            var updated = new { Id = fighter.Id, Name = "Updated Fighter", Nickname = "Updated", Country = "Croatia", WeightClassId = wc!.Id, OrganizationId = org!.Id, Wins = 15, Losses = 3 };
            var response = await client.PutAsJsonAsync($"/api/fighter/{fighter.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<FighterDTO>();
            result!.Name.Should().Be("Updated Fighter");
        }

        [Fact]
        public async Task Put_Returns404_WhenNotExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var org = await TestDataSeeder.CreateOrganizationAsync(db);
            var wc = await TestDataSeeder.CreateWeightClassAsync(db);
            var client = _factory.CreateAdminClient();
            var updated = new { Id = 99999, Name = "ValidName", Nickname = "ValidNick", Country = "ValidCountry", WeightClassId = wc.Id, OrganizationId = org.Id, Wins = 0, Losses = 0 };
            var response = await client.PutAsJsonAsync("/api/fighter/99999", updated);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            var fighter = await SeedFighter();
            var adminClient = _factory.CreateAdminClient();
            var response = await adminClient.DeleteAsync($"/api/fighter/{fighter.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var anonClient = _factory.CreateAnonClient();
            var getResponse = await anonClient.GetAsync($"/api/fighter/{fighter.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/fighter/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    public class EventApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public EventApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private async Task<Event> SeedEvent()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            return await TestDataSeeder.CreateEventAsync(db);
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            await SeedEvent();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/event");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ReturnsEvent_WhenExists()
        {
            var ev = await SeedEvent();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync($"/api/event/{ev.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<EventDTO>();
            result!.Id.Should().Be(ev.Id);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/event/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesEvent_WhenValid()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var arena = await TestDataSeeder.CreateArenaAsync(db);
            var org = await TestDataSeeder.CreateOrganizationAsync(db);
            var client = _factory.CreateAdminClient();
            var newEvent = new { Name = "New Fight Night", City = "Zagreb", OrganizationId = org.Id, VenueId = arena.Id, Date = DateTime.UtcNow.AddMonths(2), Time = TimeSpan.FromHours(19), Description = "Great event", BaseTicketPrice = 120m, TicketsSold = 0 };
            var response = await client.PostAsJsonAsync("/api/event", newEvent);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenInvalidModel()
        {
            var client = _factory.CreateAdminClient();
            var invalid = new { Name = "", City = "", OrganizationId = 0, VenueId = 0 };
            var response = await client.PostAsJsonAsync("/api/event", invalid);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_UpdatesEvent_WhenExists()
        {
            var ev = await SeedEvent();
            var client = _factory.CreateAdminClient();
            var updated = new { Id = ev.Id, Name = "Updated Event", City = "Split", OrganizationId = ev.OrganizationId, VenueId = ev.VenueId, Date = DateTime.UtcNow.AddMonths(3), Time = TimeSpan.FromHours(20), Description = "Updated desc", BaseTicketPrice = 200m, TicketsSold = 100 };
            var response = await client.PutAsJsonAsync($"/api/event/{ev.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Put_Returns404_WhenNotExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var arena = await TestDataSeeder.CreateArenaAsync(db);
            var org = await TestDataSeeder.CreateOrganizationAsync(db);
            var client = _factory.CreateAdminClient();
            var updated = new { Id = 99999, Name = "ValidEventName", City = "ValidCity", OrganizationId = org.Id, VenueId = arena.Id, Date = DateTime.UtcNow, Time = TimeSpan.FromHours(19), Description = "ValidDescription", BaseTicketPrice = 0m, TicketsSold = 0 };
            var response = await client.PutAsJsonAsync("/api/event/99999", updated);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            var ev = await SeedEvent();
            var adminClient = _factory.CreateAdminClient();
            var deleteResponse = await adminClient.DeleteAsync($"/api/event/{ev.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var anonClient = _factory.CreateAnonClient();
            var getResponse = await anonClient.GetAsync($"/api/event/{ev.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/event/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    public class MatchApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public MatchApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private async Task<Match> SeedMatch()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            return await TestDataSeeder.CreateMatchAsync(db);
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            await SeedMatch();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/match");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ReturnsMatch_WhenExists()
        {
            var match = await SeedMatch();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync($"/api/match/{match.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<MatchDTO>();
            result!.Id.Should().Be(match.Id);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/match/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesMatch_WhenValid()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var ev = await TestDataSeeder.CreateEventAsync(db);
            var f1 = await TestDataSeeder.CreateFighterAsync(db);
            var f2 = await TestDataSeeder.CreateFighterAsync(db);
            var wc = await TestDataSeeder.CreateWeightClassAsync(db);
            var client = _factory.CreateAdminClient();
            var newMatch = new { Fighter1Id = f1.Id, Fighter2Id = f2.Id, WeightClassId = wc.Id, EventId = ev.Id, RoundLimit = 3, Championship = false, Referee = "Test Referee", Status = "Scheduled" };
            var response = await client.PostAsJsonAsync("/api/match", newMatch);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Put_UpdatesMatch_WhenExists()
        {
            var match = await SeedMatch();
            var client = _factory.CreateAdminClient();
            var updated = new { Id = match.Id, Fighter1Id = match.Fighter1Id, Fighter2Id = match.Fighter2Id, WeightClassId = match.WeightClassId, EventId = match.EventId, RoundLimit = 5, Championship = true, Referee = "Updated Referee", Status = "Completed" };
            var response = await client.PutAsJsonAsync($"/api/match/{match.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Put_Returns404_WhenNotExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var ev = await TestDataSeeder.CreateEventAsync(db);
            var f1 = await TestDataSeeder.CreateFighterAsync(db);
            var f2 = await TestDataSeeder.CreateFighterAsync(db);
            var wc = await TestDataSeeder.CreateWeightClassAsync(db);
            var client = _factory.CreateAdminClient();
            var updated = new { Id = 99999, Fighter1Id = f1.Id, Fighter2Id = f2.Id, WeightClassId = wc.Id, EventId = ev.Id, RoundLimit = 3, Championship = false, Referee = "X", Status = "X" };
            var response = await client.PutAsJsonAsync("/api/match/99999", updated);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            var match = await SeedMatch();
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync($"/api/match/{match.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/match/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetByEvent_ReturnsMatchesForEvent()
        {
            var match = await SeedMatch();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync($"/api/match/byevent/{match.EventId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<MatchDTO>>();
            result.Should().NotBeNull();
            result!.Should().Contain(m => m.Id == match.Id);
        }
    }

    public class TicketApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public TicketApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private async Task<Ticket> SeedTicket()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            return await TestDataSeeder.CreateTicketAsync(db);
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            await SeedTicket();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/ticket");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ReturnsTicket_WhenExists()
        {
            var ticket = await SeedTicket();
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync($"/api/ticket/{ticket.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<TicketDTO>();
            result!.Id.Should().Be(ticket.Id);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/ticket/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesTicket_WhenValid()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var ev = await TestDataSeeder.CreateEventAsync(db);
            var client = _factory.CreateAdminClient();
            var newTicket = new { EventId = ev.Id, Section = "B", Row = 2, Seat = "5", Price = 200m, PurchaseDate = DateTime.UtcNow, IsVip = true };
            var response = await client.PostAsJsonAsync("/api/ticket", newTicket);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Put_UpdatesTicket_WhenExists()
        {
            var ticket = await SeedTicket();
            var client = _factory.CreateAdminClient();
            var updated = new { Id = ticket.Id, EventId = ticket.EventId, Section = "C", Row = 3, Seat = "10", Price = 250m, PurchaseDate = DateTime.UtcNow, IsVip = false };
            var response = await client.PutAsJsonAsync($"/api/ticket/{ticket.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            var ticket = await SeedTicket();
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync($"/api/ticket/{ticket.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var anonClient = _factory.CreateAnonClient();
            var getResponse = await anonClient.GetAsync($"/api/ticket/{ticket.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/ticket/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    public class WeightClassApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public WeightClassApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            await TestDataSeeder.CreateWeightClassAsync(db);
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/weightclass");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ReturnsWeightClass_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var wc = await TestDataSeeder.CreateWeightClassAsync(db);
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync($"/api/weightclass/{wc.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/weightclass/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesWeightClass_WhenValid()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.PostAsJsonAsync("/api/weightclass", new { Name = "Super Heavyweight" });
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenInvalidModel()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.PostAsJsonAsync("/api/weightclass", new { Name = "" });
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_UpdatesWeightClass_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var wc = await TestDataSeeder.CreateWeightClassAsync(db);
            var client = _factory.CreateAdminClient();
            var updated = new { Id = wc.Id, Name = "Updated Class" };
            var response = await client.PutAsJsonAsync($"/api/weightclass/{wc.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var wc = await TestDataSeeder.CreateWeightClassAsync(db);
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync($"/api/weightclass/{wc.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/weightclass/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    public class FightOrganizationApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public FightOrganizationApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            await TestDataSeeder.CreateOrganizationAsync(db);
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/fightorganization");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ReturnsOrg_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var org = await TestDataSeeder.CreateOrganizationAsync(db);
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync($"/api/fightorganization/{org.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/fightorganization/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesOrganization_WhenValid()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.PostAsJsonAsync("/api/fightorganization", new { Name = "ONE Championship" });
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenInvalidModel()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.PostAsJsonAsync("/api/fightorganization", new { Name = "" });
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_UpdatesOrganization_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var org = await TestDataSeeder.CreateOrganizationAsync(db);
            var client = _factory.CreateAdminClient();
            var updated = new { Id = org.Id, Name = "Updated Org" };
            var response = await client.PutAsJsonAsync($"/api/fightorganization/{org.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var org = await TestDataSeeder.CreateOrganizationAsync(db);
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync($"/api/fightorganization/{org.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/fightorganization/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    public class UserApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public UserApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsOk_ForAdmin()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            await TestDataSeeder.CreateUserAsync(db);
            var client = _factory.CreateAdminClient();
            var response = await client.GetAsync("/api/user");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ReturnsUser_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var user = await TestDataSeeder.CreateUserAsync(db);
            var client = _factory.CreateAdminClient();
            var response = await client.GetAsync($"/api/user/{user.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<UserDTO>();
            result!.Id.Should().Be(user.Id);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.GetAsync("/api/user/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesUser_WhenValid()
        {
            var client = _factory.CreateAdminClient();
            var newUser = new { Name = "New User", Email = "newuser@test.com", Phone = "+385911111111", BirthDate = new DateTime(1995, 5, 5), LoyaltyPoints = 0, IsVip = false, MemberLevel = "Bronze" };
            var response = await client.PostAsJsonAsync("/api/user", newUser);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Put_UpdatesUser_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var user = await TestDataSeeder.CreateUserAsync(db);
            var client = _factory.CreateAdminClient();
            var updated = new { Id = user.Id, Name = "Updated User", Email = "updated@test.com", Phone = "+385922222222", BirthDate = new DateTime(1990, 1, 1), LoyaltyPoints = 200, IsVip = true, MemberLevel = "Gold" };
            var response = await client.PutAsJsonAsync($"/api/user/{user.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var user = await TestDataSeeder.CreateUserAsync(db);
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync($"/api/user/{user.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/user/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    public class CartApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public CartApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.GetAsync("/api/cart");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ReturnsCart_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var cart = await TestDataSeeder.CreateCartAsync(db);
            var client = _factory.CreateAdminClient();
            var response = await client.GetAsync($"/api/cart/{cart.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<CartDTO>();
            result!.Id.Should().Be(cart.Id);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.GetAsync("/api/cart/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesCart_WhenValid()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var user = await TestDataSeeder.CreateUserAsync(db);
            var client = _factory.CreateAdminClient();
            var newCart = new { UserId = user.Id, DiscountCode = (string?)null, DiscountPercent = 0m, IsPaid = false };
            var response = await client.PostAsJsonAsync("/api/cart", newCart);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Put_UpdatesCart_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var cart = await TestDataSeeder.CreateCartAsync(db);
            var client = _factory.CreateAdminClient();
            var updated = new { Id = cart.Id, UserId = cart.UserId, DiscountCode = "SAVE10", DiscountPercent = 10m, IsPaid = false };
            var response = await client.PutAsJsonAsync($"/api/cart/{cart.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var cart = await TestDataSeeder.CreateCartAsync(db);
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync($"/api/cart/{cart.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/cart/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    public class TicketShopModelApiTests : IClassFixture<AuthenticatedWebApplicationFactory>
    {
        private readonly AuthenticatedWebApplicationFactory _factory;

        public TicketShopModelApiTests(AuthenticatedWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            await TestDataSeeder.CreateTicketShopModelAsync(db);
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/ticketshopmodel");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ReturnsModel_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var tsm = await TestDataSeeder.CreateTicketShopModelAsync(db);
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync($"/api/ticketshopmodel/{tsm.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAnonClient();
            var response = await client.GetAsync("/api/ticketshopmodel/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_CreatesModel_WhenValid()
        {
            var client = _factory.CreateAdminClient();
            var newModel = new { Title = "Gold Package", EventName = "Championship Night", Location = "Zagreb Arena", Section = "Gold", Row = 1, Seat = "10", Price = 350m, IsVip = false, Description = "Gold tier package" };
            var response = await client.PostAsJsonAsync("/api/ticketshopmodel", newModel);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenInvalidModel()
        {
            var client = _factory.CreateAdminClient();
            var invalid = new { Title = "", EventName = "", Location = "", Section = "", Row = 0, Seat = "", Price = -1m, Description = "" };
            var response = await client.PostAsJsonAsync("/api/ticketshopmodel", invalid);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_UpdatesModel_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var tsm = await TestDataSeeder.CreateTicketShopModelAsync(db);
            var client = _factory.CreateAdminClient();
            var updated = new { Id = tsm.Id, Title = "Updated Package", EventName = "Updated Night", Location = "Split Arena", Section = "Platinum", Row = 2, Seat = "5", Price = 700m, IsVip = true, Description = "Updated description" };
            var response = await client.PutAsJsonAsync($"/api/ticketshopmodel/{tsm.Id}", updated);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_SoftDeletes_WhenExists()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VjezbaDbContext>();
            var tsm = await TestDataSeeder.CreateTicketShopModelAsync(db);
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync($"/api/ticketshopmodel/{tsm.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Returns404_WhenNotExists()
        {
            var client = _factory.CreateAdminClient();
            var response = await client.DeleteAsync("/api/ticketshopmodel/99999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
