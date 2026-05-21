using Microsoft.EntityFrameworkCore;
using TicketingSystemFightNight.Data;
using TicketingSystemFightNight.Repositories;

var builder = WebApplication.CreateBuilder(args);

var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
if (!string.IsNullOrEmpty(urls))
{
    builder.WebHost.UseUrls(urls);
}

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<VjezbaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("VjezbaDbContext")));

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("ApplicationDbContext")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// Session support for dummy cart state in the ticket shop.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseSession();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "home",
    pattern: "pocetna",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


