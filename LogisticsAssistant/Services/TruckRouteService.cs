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
            var truck = await _context.Trucks
                .FirstOrDefaultAsync(t => t.Id == truckRouteView.TruckId);

            if (truck == null)
                return false;

            var lastRoute = await _context.Routes
                .Where(r => r.TruckId == truckRouteView.TruckId)
                .OrderByDescending(r => r.Date)
                .FirstOrDefaultAsync();

            if (lastRoute != null)
            {
                if (lastRoute.TruckVmax <= 0 || lastRoute.BreakFrequency <= 0)
                    return false;

                double travelMinutes = ((double)lastRoute.Distance / lastRoute.TruckVmax) * 60;

                int numberOfBreaks = (int)(travelMinutes / lastRoute.BreakFrequency);
                double breakMinutes = numberOfBreaks * lastRoute.TruckDriverBreak;

                double totalMinutes = travelMinutes + breakMinutes;

                if (double.IsInfinity(totalMinutes) || double.IsNaN(totalMinutes))
                    return false;

                DateTime lastRouteEnd = lastRoute.Date.AddMinutes(totalMinutes);
                DateTime earliestNextRoute = lastRouteEnd.AddMinutes(truck.DriverBreak);

                if (truckRouteView.Date < earliestNextRoute)
                    return false;
            }

            var newRoute = new TruckRoute
            {
                TruckId = truck.Id,
                Distance = truckRouteView.Distance,
                BreakFrequency = truckRouteView.BreakFrequency,
                Date = truckRouteView.Date,
                TruckVmax = truck.Vmax,
                TruckDriverBreak = truck.DriverBreak
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
