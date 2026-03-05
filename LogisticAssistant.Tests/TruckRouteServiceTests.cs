using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using LogisticsAssistant.Models;
using LogisticsAssistant.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LogisticAssistant.Tests
{
    public class TruckRouteServiceTests
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
        public async Task CreateTruckRouteAsync_ShouldReturnTrue_WhenTruckExists()
        {
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();
            context.Trucks.Add(new Truck { Id = 1, LicensePlate = "ABC123", Vmax = 100, DriverBreak = 15 });
            context.SaveChanges();
            var service = new TruckRouteService(context);
            var truckRouteView = new TruckRouteViewModel
            {
                TruckId = 1,
                Distance = 200,
                BreakFrequency = 2
            };

            var result = await service.CreateTruckRouteAsync(truckRouteView);

            Assert.True(result);
            Assert.Single(context.Routes);

            var truckRoutes = context.Routes.First();
            Assert.Equal(1, truckRoutes.TruckId);
            Assert.Equal(200, truckRoutes.Distance);
            Assert.Equal(2, truckRoutes.BreakFrequency);
        }

        [Fact]
        public async Task CreateTruckRouteAsync_ShouldReturnFalse_WhenTruckDoesNotExist()
        {
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();
            var service = new TruckRouteService(context);
            var truckRouteView = new TruckRouteViewModel
            {
                TruckId = 999,
                Distance = 200,
                BreakFrequency = 2
            };
            var result = await service.CreateTruckRouteAsync(truckRouteView);
            Assert.False(result);
            Assert.Empty(context.Routes);

        }

        [Fact]
        public async Task GetAllTrucks_ShouldReturnAllTrucks()
        {
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();

            context.Trucks.Add(new Truck { Id = 1, LicensePlate = "ABC123", Vmax = 100, DriverBreak = 15 });
            context.Trucks.Add(new Truck { Id = 2, LicensePlate = "DEF456", Vmax = 120, DriverBreak = 20 });
            context.SaveChanges();

            var service = new TruckRouteService(context);
            var result = await service.GetAllTrucks();

            Assert.Equal(2, result.Count);
            Assert.Equal("ABC123", result[1]);
            Assert.Equal("DEF456", result[2]);
        }

        [Fact]
        public async Task GetAllTruckRoutesAsync_ShouldReturnAllTruckRoutes()
        {
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();

            context.Trucks.Add(new Truck { Id = 1, LicensePlate = "ABC123", Vmax = 100, DriverBreak = 15 });
            context.Trucks.Add(new Truck { Id = 2, LicensePlate = "DEF456", Vmax = 120, DriverBreak = 20 });
            context.Routes.Add(new TruckRoute { Id = 1, TruckId = 1, Distance = 200, BreakFrequency = 2 });
            context.Routes.Add(new TruckRoute { Id = 2, TruckId = 2, Distance = 300, BreakFrequency = 3 });
            context.SaveChanges();

            var service = new TruckRouteService(context);
            var result = await service.GetAllTruckRoutesAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].TruckId);
            Assert.Equal(200, result[0].Distance);
            Assert.Equal(2, result[0].BreakFrequency);
            Assert.Equal(2, result[1].TruckId);
            Assert.Equal(300, result[1].Distance);
            Assert.Equal(3, result[1].BreakFrequency);
        }

        [Fact]
        public async Task GetTruckRouteByIdAsync_ShouldReturnCorrectTruckRoute()
        {
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();

            context.Trucks.Add(new Truck { Id = 1, LicensePlate = "ABC123", Vmax = 100, DriverBreak = 15 });
            context.Routes.Add(new TruckRoute { Id = 1, TruckId = 1, Distance = 200, BreakFrequency = 2 });
            context.SaveChanges();

            var service = new TruckRouteService(context);
            var result = await service.GetTruckRouteByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.TruckId);
            Assert.Equal(200, result.Distance);
            Assert.Equal(2, result.BreakFrequency);
        }

        [Fact]
        public async Task GetTruckRouteByIdAsync_ShouldReturnNull_WhenTruckRouteDoesNotExist()
        {
            var context = GetDbContext();
            var httpContextAccessor = GetHttpContextAccessor();
            var service = new TruckRouteService(context);
            var result = await service.GetTruckRouteByIdAsync(999);

            Assert.Null(result);
        }
    }
}
