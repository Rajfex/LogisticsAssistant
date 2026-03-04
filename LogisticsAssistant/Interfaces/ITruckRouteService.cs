using LogisticsAssistant.Models;

namespace LogisticsAssistant.Interfaces
{
    public interface ITruckRouteService
    {
        Task<bool> CreateTruckRouteAsync(TruckRouteViewModel truckRouteView);
        Task<Dictionary<int, string>> GetAllTrucks();
        Task<List<TruckRoute>> GetAllTruckRoutesAsync();
        Task<TruckRoute> GetTruckRouteByIdAsync(int id);
    }
}
