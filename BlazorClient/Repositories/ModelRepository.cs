using BlazorClient.Helpers;
using BlazorClient.Repositories.Interfaces;
using Models;

namespace BlazorClient.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _requestUrl;

        public ModelRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _requestUrl = $"{_httpClient.BaseAddress}api/models";
        }

        public async Task<Model> GetModelAsync(int id)
        {
            var apiResponse = await _httpClient.GetAsync($"{_requestUrl}/{id}");

            return await HttpResponseHelper.DeserializeResponseAndThrowIfNotSuccess<Model>(apiResponse);
        }

        public async Task<Model[]> GetModelsAsync()
        {
            var apiResponse = await _httpClient.GetAsync(_requestUrl);

            return await HttpResponseHelper.DeserializeResponseAndThrowIfNotSuccess<Model[]>(apiResponse);
        }
    }
}
