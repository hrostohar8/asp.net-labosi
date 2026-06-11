using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingSystemFightNight.Data;

namespace TicketingSystemFightNight.Tests.Helpers
{
    public class AuthenticatedWebApplicationFactory : WebApplicationFactory<Program>
    {
        private static int _dbCounter = 0;
        public string DbName { get; } = "AuthTestDb_" + Interlocked.Increment(ref _dbCounter);

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

                services.AddDbContext<VjezbaDbContext>(options =>
                    options.UseInMemoryDatabase(DbName));

                // Remove any existing auth and replace with test scheme
                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(
                        "TestScheme", options =>
                        {
                            options.IsAuthenticated = false;
                            options.UserRole = "Admin";
                        });
            });

            builder.UseEnvironment("Development");
        }

        public HttpClient CreateAdminClient()
        {
            var client = CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Auth", "Admin");
            return client;
        }

        public HttpClient CreateManagerClient()
        {
            var client = CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Auth", "Manager");
            return client;
        }

        public HttpClient CreateAnonClient()
        {
            return CreateClient(); // no header = not authenticated
        }
    }
}
