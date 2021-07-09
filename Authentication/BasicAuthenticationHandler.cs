
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace Happy5ChatTest.Authentication
{
    public class BasicAuthenticationHandler :  AuthenticationHandler<AuthenticationSchemeOptions>, IAuthHandler
    {
        private readonly IUserService _userService;

        public BasicAuthenticationHandler(IUserService userService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string username = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                username = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();
                string passwordHashed = HashPasswordSHA256(password);

                if (!_userService.ValidateUser(username, passwordHashed))
                    throw new ArgumentException("Invalid credentials");

            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed:{ex.Message}");
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.NameIdentifier,_userService.GetUserId(username))
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        public async Task<string> HandleRegistration(string username, string password)
        {
            try
            {
                string hashedPassword = HashPasswordSHA256(password);
                if (!_userService.RegisterUser(username, hashedPassword))
                    throw new ArgumentException("User Exist");
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";   
            }
            return "User Registred";
        }
        protected static string HashPasswordSHA256(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();

            }
        }
    }

    public interface IAuthHandler
    {
          Task<string> HandleRegistration(string username, string password);
    }
}
