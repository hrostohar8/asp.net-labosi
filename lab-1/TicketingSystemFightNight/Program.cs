using TicketingSystemFightNight.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


