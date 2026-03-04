using LogisticsAssistant.Models;

namespace LogisticsAssistant.Interfaces
{
    public interface ITrucksService
    {
        Task<bool> CreateTruckAsync(TruckViewModel truckView);
        Task<bool> DeleteTruckAsync(int id);
        Task<bool> UpdateTruckAsync(int id, TruckViewModel truckView);
    }
}
