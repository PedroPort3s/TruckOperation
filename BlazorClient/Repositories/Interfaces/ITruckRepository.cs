using Models;

namespace BlazorClient.Repositories.Interfaces
{
    public interface ITruckRepository
    {
        Task<Truck[]> GetTrucksAsync();
        Task<Truck> GetTruckAsync(Guid id);
        Task<Truck> PostTruckAsync(Truck truck);
        Task<Truck> PutTruckAsync(Truck truck);
        Task<bool> DeleteTruckAsync(Guid id);
    }
}