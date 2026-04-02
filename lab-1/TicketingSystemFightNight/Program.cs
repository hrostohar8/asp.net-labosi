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
    new Fighter("Petr Yan", WeightClass.Bantamweight, FightOrganization.UFC),
    new Fighter("Robert Whittaker", WeightClass.Middleweight, FightOrganization.UFC),
    new Fighter("Colby Covington", WeightClass.Welterweight, FightOrganization.UFC),
    new Fighter("Amanda Nunes", WeightClass.Flyweight, FightOrganization.UFC),
    new Fighter("Khabib Nurmagomedov", WeightClass.Lightweight, FightOrganization.UFC),
    new Fighter("Alexander Volkanovski", WeightClass.Featherweight, FightOrganization.UFC),
    new Fighter("Daniel Cormier", WeightClass.LightHeavyweight, FightOrganization.UFC),
    new Fighter("Stipe Miocic", WeightClass.Heavyweight, FightOrganization.UFC),
    new Fighter("TJ Dillashaw", WeightClass.Bantamweight, FightOrganization.UFC),
    // BELLATOR borci
    new Fighter("Patricio Freire", WeightClass.Featherweight, FightOrganization.BELLATOR),
    new Fighter("AJ McKee", WeightClass.Featherweight, FightOrganization.BELLATOR),
    new Fighter("Sergei Kharitonov", WeightClass.Heavyweight, FightOrganization.BELLATOR),
    new Fighter("Alexander Volkov", WeightClass.Heavyweight, FightOrganization.BELLATOR),
    // KSW borci
    new Fighter("Roberto Soldić", WeightClass.Welterweight, FightOrganization.KSW),
    new Fighter("Mamed Khalidov", WeightClass.Welterweight, FightOrganization.KSW)
};

var events = new List<Event>
{
    new Event("UFC 300", FightOrganization.UFC, "Zagreb", DateTime.Today.AddDays(25), TimeSpan.FromHours(19), arena1),
    new Event("Bellator 295", FightOrganization.BELLATOR, "Split", DateTime.Today.AddDays(40), TimeSpan.FromHours(19), arena2),
    new Event("KSW 74", FightOrganization.KSW, "Rijeka", DateTime.Today.AddDays(60), TimeSpan.FromHours(19), arena3)
};

var matches = new List<Match>
{
    new Match(fighters[0], fighters[8], WeightClass.Middleweight, events[0]),
    new Match(fighters[3], fighters[11], WeightClass.Lightweight, events[0]),
    new Match(fighters[16], fighters[17], WeightClass.Featherweight, events[1]),
    new Match(fighters[18], fighters[19], WeightClass.Heavyweight, events[1]),
    new Match(fighters[20], fighters[21], WeightClass.Welterweight, events[2])
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


