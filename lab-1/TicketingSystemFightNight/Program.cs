using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Default port 5000 may already be occupied on this machine.
// Use an alternate port so the app can start reliably.
builder.WebHost.UseUrls("http://localhost:5001");

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<VjezbaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("VjezbaDbContext")));

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("ApplicationDbContext")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// Register mock repositories for Home and Dashboard controllers to avoid DI failures.
builder.Services.AddSingleton<ArenaMockRepository>();
builder.Services.AddSingleton<FighterMockRepository>();
builder.Services.AddSingleton<EventMockRepository>();
builder.Services.AddSingleton<MatchMockRepository>();
builder.Services.AddSingleton<TicketMockRepository>();
builder.Services.AddSingleton<UserMockRepository>();
builder.Services.AddSingleton<CartMockRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "home",
    pattern: "pocetna",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


