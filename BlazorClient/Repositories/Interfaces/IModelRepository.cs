using Models;

namespace BlazorClient.Repositories.Interfaces
{
    public interface IModelRepository
    {
        Task<Model[]> GetModelsAsync();
        Task<Model> GetModelAsync(int id);
    }
}