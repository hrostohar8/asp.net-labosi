using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace TicketingSystemFightNight.Tests.Helpers
{
    public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
    {
        public TestAuthHandler(IOptionsMonitor<TestAuthHandlerOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var opts = OptionsMonitor.CurrentValue;

            if (!opts.IsAuthenticated)
                return Task.FromResult(AuthenticateResult.Fail("Not authenticated"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, opts.UserId),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, opts.UserRole)
            };

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class TestAuthHandlerOptions : AuthenticationSchemeOptions
    {
        public bool IsAuthenticated { get; set; } = false;
        public string UserRole { get; set; } = "Admin";
        public string UserId { get; set; } = "test-user-id";
    }
}
