using Models;

namespace BlazorClient.Repositories.Interfaces
{
    public interface IPlantRepository
    {
        Task<Plant[]> GetPlantsAsync();
        Task<Plant> GetPlantAsync(int id);
    }
}