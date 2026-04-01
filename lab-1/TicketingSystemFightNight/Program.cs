using System;
using System.Linq;
using System.Collections.Generic;
using TicketingSystemFightNight.Models;

// Kreiramo objektni model i popunjavamo podacima:
var arena1 = new Arena("Arena Zagreb", "Zagreb", 15000);
var arena2 = new Arena("Arena Split", "Split", 12000);
var arena3 = new Arena("Arena Rijeka", "Rijeka", 10000);

var fighters = new List<Fighter>
{
    new Fighter("Israel Adesanya", WeightClass.Middleweight, FightOrganization.UFC),
    new Fighter("Kamaru Usman", WeightClass.Welterweight, FightOrganization.UFC),
    new Fighter("Valentina Shevchenko", WeightClass.Flyweight, FightOrganization.UFC),
    new Fighter("Conor McGregor", WeightClass.Lightweight, FightOrganization.UFC),
    new Fighter("Max Holloway", WeightClass.Featherweight, FightOrganization.UFC),
    new Fighter("Jon Jones", WeightClass.LightHeavyweight, FightOrganization.UFC),
    new Fighter("Francis Ngannou", WeightClass.Heavyweight, FightOrganization.UFC),
    new Fighter("Petr Yan", WeightClass.Bantamweight, FightOrganization.UFC)
};

var events = new List<Event>
{
    new Event("UFC 300", FightOrganization.UFC, "Zagreb", DateTime.Today.AddDays(25), TimeSpan.FromHours(19), arena1),
    new Event("Bellator 295", FightOrganization.BELLATOR, "Split", DateTime.Today.AddDays(40), TimeSpan.FromHours(19), arena2),
    new Event("KSW 74", FightOrganization.KSW, "Rijeka", DateTime.Today.AddDays(60), TimeSpan.FromHours(19), arena3)
};

var matches = new List<Match>
{
    new Match(fighters[0], fighters[1], WeightClass.Middleweight, events[0]),
    new Match(fighters[3], fighters[2], WeightClass.Lightweight, events[0]),
    new Match(fighters[4], fighters[7], WeightClass.Featherweight, events[1]),
    new Match(fighters[5], fighters[6], WeightClass.Heavyweight, events[1]),
    new Match(fighters[1], fighters[2], WeightClass.Welterweight, events[2])
};

foreach (var match in matches)
{
    match.Event.Matches.Add(match);
}

var user = new User("Ana Marin", "ana.marin@example.com");
var cart = new Cart(user);
cart.Tickets.Add(new Ticket(events[0], "A01", 250m));
cart.Tickets.Add(new Ticket(events[0], "A02", 250m));

// LINQ upiti
Console.WriteLine("=== Eventi po gradu ===");
var eventsByCity = events.OrderBy(e => e.City);
foreach (var ev in eventsByCity)
{
    Console.WriteLine($"{ev.Name} ({ev.Organization}) - {ev.City} - {ev.Date:dd.MM.yyyy}");
}

Console.WriteLine("\n=== Welterweight mecevi ===");
var welter = matches.Where(m => m.WeightClass == WeightClass.Welterweight);
foreach (var m in welter)
{
    Console.WriteLine($"{m.Fighter1.Name} vs {m.Fighter2.Name} @ {m.Event.Name}");
}

Console.WriteLine($"\n=== Kosarica za {cart.User.Name} ===");
Console.WriteLine($"Ukupno karata: {cart.Tickets.Count}");
Console.WriteLine($"Ukupna cijena: {cart.Tickets.Sum(t => t.Price):C}");

Console.WriteLine("Gotovo. Projekat je popunjen i LINQ radi.");



