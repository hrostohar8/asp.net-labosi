using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace TicketingSystemFightNight.Tests.Helpers
{
    public class TestAuthHandlerOptions : AuthenticationSchemeOptions
    {
        public bool IsAuthenticated { get; set; } = false;
        public string UserRole { get; set; } = "Admin";
        public string UserId { get; set; } = "test-user-id";
    }

    public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
    {
        public TestAuthHandler(IOptionsMonitor<TestAuthHandlerOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("X-Test-Auth", out var roleHeader))
                return Task.FromResult(AuthenticateResult.Fail("No auth header"));

            var role = roleHeader.ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
