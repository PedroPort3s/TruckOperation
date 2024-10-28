using BlazorClient.Helpers;
using BlazorClient.Repositories.Interfaces;
using Models;

namespace BlazorClient.Repositories
{
    public class PlantRepository : IPlantRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _requestUrl;

        public PlantRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _requestUrl = $"{_httpClient.BaseAddress}api/plants";
        }

        public async Task<Plant> GetPlantAsync(int id)
        {
            var apiResponse = await _httpClient.GetAsync($"{_requestUrl}/{id}");

            return await HttpResponseHelper.DeserializeResponseAndThrowIfNotSuccess<Plant>(apiResponse);
        }

        public async Task<Plant[]> GetPlantsAsync()
        {
            var apiResponse = await _httpClient.GetAsync(_requestUrl);

            return await HttpResponseHelper.DeserializeResponseAndThrowIfNotSuccess<Plant[]>(apiResponse);
        }
    }
}
