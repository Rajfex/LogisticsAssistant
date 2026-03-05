using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LogisticsAssistant.Models;
using LogisticsAssistant.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LogisticsAssistant.Tests
{
    public class TrucksServiceTests
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
        public async Task CreateTruckAsync_ShouldAddTruckToDatabase()
        {
            var context = GetDbContext();
            var httpContext = GetHttpContextAccessor();

            var service = new TrucksService(context, httpContext);

            var model = new TruckViewModel
            {
                LicensePlate = "KK12332",
                Vmax = 120,
                DriverBreak = 45
            };

            var result = await service.CreateTruckAsync(model);

            Assert.True(result);
            Assert.Single(context.Trucks);

            var truck = context.Trucks.First();
            Assert.Equal("KK12332", truck.LicensePlate);
            Assert.Equal(120, truck.Vmax);
        }

        [Fact]
        public async Task DeleteTruckAsync_ShouldRemoveTruck_WhenTruckExists()
        {
            var context = GetDbContext();
            var httpContext = GetHttpContextAccessor();

            context.Trucks.Add(new Truck
            {
                Id = 1,
                LicensePlate = "KK12332",
                Vmax = 100,
                DriverBreak = 30,
                UserId = 1
            });

            await context.SaveChangesAsync();

            var service = new TrucksService(context, httpContext);

            var result = await service.DeleteTruckAsync(1);

            Assert.True(result);
            Assert.Empty(context.Trucks);
        }

        [Fact]
        public async Task DeleteTruckAsync_ShouldReturnFalse_WhenTruckDoesNotExist()
        {
            var context = GetDbContext();
            var httpContext = GetHttpContextAccessor();

            var service = new TrucksService(context, httpContext);

            var result = await service.DeleteTruckAsync(99);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateTruckAsync_ShouldUpdateTruck_WhenTruckExists()
        {
            var context = GetDbContext();
            var httpContext = GetHttpContextAccessor();

            context.Trucks.Add(new Truck
            {
                Id = 1,
                LicensePlate = "OLD123",
                Vmax = 100,
                DriverBreak = 30,
                UserId = 1
            });

            await context.SaveChangesAsync();

            var service = new TrucksService(context, httpContext);

            var model = new TruckViewModel
            {
                LicensePlate = "NEW123",
                Vmax = 130,
                DriverBreak = 50
            };

            var result = await service.UpdateTruckAsync(1, model);

            var truck = context.Trucks.First();

            Assert.True(result);
            Assert.Equal("NEW123", truck.LicensePlate);
            Assert.Equal(130, truck.Vmax);
            Assert.Equal(50, truck.DriverBreak);
        }

        [Fact]
        public async Task UpdateTruckAsync_ShouldReturnFalse_WhenTruckDoesNotExist()
        {
            var context = GetDbContext();
            var httpContext = GetHttpContextAccessor();

            var service = new TrucksService(context, httpContext);

            var model = new TruckViewModel
            {
                LicensePlate = "TEST",
                Vmax = 120,
                DriverBreak = 45
            };

            var result = await service.UpdateTruckAsync(99, model);

            Assert.False(result);
        }
    }
}