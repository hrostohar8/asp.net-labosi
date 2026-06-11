using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingSystemFightNight.Data;

namespace TicketingSystemFightNight.Tests.Helpers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private static int _dbCounter = 0;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var testSettings = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:VjezbaDbContext"] = "TestConnection",
                    ["Authentication:Google:ClientId"] = "test-client-id",
                    ["Authentication:Google:ClientSecret"] = "test-client-secret"
                };
                config.AddInMemoryCollection(testSettings);
            });

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<VjezbaDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                var dbName = "TestDb_" + Interlocked.Increment(ref _dbCounter);
                services.AddDbContext<VjezbaDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });

            builder.UseEnvironment("Development");
        }
    }
}
