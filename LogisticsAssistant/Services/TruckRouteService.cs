using LogisticsAssistant.Interfaces;
using LogisticsAssistant.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAssistant.Services
{
    public class TruckRouteService : ITruckRouteService
    {
        private readonly AppDbContext _context;
        public TruckRouteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTruckRouteAsync(TruckRouteViewModel truckRouteView)
        {
            var truckExists = await _context.Trucks.AnyAsync(t => t.Id == truckRouteView.TruckId);
            if (!truckExists)
            {
                return false;
            }

            var newRoute = new TruckRoute
            {
                TruckId = truckRouteView.TruckId,
                BreakFrequency = truckRouteView.BreakFrequency,
                Distance = truckRouteView.Distance,
                Date = DateTime.Now
            };

            _context.Routes.Add(newRoute);
            await _context.SaveChangesAsync();

            return true;
        }

        public Task<Dictionary<int, string>> GetAllTrucks()
        {
            var trucks = _context.Trucks.ToDictionary(t => t.Id, t => t.LicensePlate);
            return Task.FromResult(trucks);
        }

        public async Task<List<TruckRoute>> GetAllTruckRoutesAsync()
        {
            var data = await _context.Routes.Include(r => r.Truck).ToListAsync();

            return data;
        }

        public async Task<TruckRoute> GetTruckRouteByIdAsync(int id)
        {
            return await _context.Routes
                .Include(r => r.Truck)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }


}
