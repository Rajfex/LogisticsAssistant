using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using LogisticsAssistant.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LogisticAssistant.Tests
{
    public class AccountServiceTest
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private IHttpContextAccessor GetHttpContextAccessor()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("Id", "1")
            }));

            var httpContext = new DefaultHttpContext
            {
                User = user
            };

            var mock = new Mock<IHttpContextAccessor>();
            mock.Setup(x => x.HttpContext).Returns(httpContext);

            return mock.Object;
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUser()
        {
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();
            var accountService = new AccountService(context, httpContextAccessor);

            var result = await accountService.RegisterAsync("testuser", "password123");

            Assert.True(result);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == "testuser");
            Assert.NotNull(user);
            Assert.True(BCrypt.Net.BCrypt.Verify("password123", user.PasswordHashed));
        }
    }
}
