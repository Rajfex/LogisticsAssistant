using System.Security.Claims;
using System.Security.Principal;
using LogisticsAssistant.Interfaces;
using LogisticsAssistant.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LogisticsAssistant.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }   

        public async Task<bool> RegisterAsync(string name, string password)
        {
            string secureHash = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Name = name,
                PasswordHashed = secureHash
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> LoginAsync(string name, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == name);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHashed))
                return false;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Id", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return true;
        }

        public async Task LogoutAsync() =>
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    }
}
