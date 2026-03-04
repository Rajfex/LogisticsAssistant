using System.Security.Claims;
using LogisticsAssistant.Interfaces;
using LogisticsAssistant.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAssistant.Services
{
    public class TrucksService : ITrucksService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrucksService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CreateTruckAsync(TruckViewModel truckView)
        {
            var user = _httpContextAccessor.HttpContext.User;

            var newTruck = new Truck
            {
                LicensePlate = truckView.LicensePlate,
                Vmax = truckView.Vmax,
                DriverBreak = truckView.DriverBreak,

                UserId = Convert.ToInt32(user.FindFirst("Id").Value)
            };

            _context.Add(newTruck);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTruckAsync(int id)
        {
            var truck = await _context.Trucks.FirstOrDefaultAsync(t => t.Id == id);
            if (truck == null)
            {
                return false;
            }
            _context.Trucks.Remove(truck);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTruckAsync(int id, TruckViewModel truckView)
        {
            var truck = await _context.Trucks.FirstOrDefaultAsync(t => t.Id == id);
            if (truck == null) return false;

            truck.LicensePlate = truckView.LicensePlate;
            truck.Vmax = truckView.Vmax;
            truck.DriverBreak = truckView.DriverBreak;

            _context.Trucks.Update(truck);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
