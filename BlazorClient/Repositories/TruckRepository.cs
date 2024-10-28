using BlazorClient.Helpers;
using BlazorClient.Repositories.Interfaces;
using Models;
using Models.Exceptions;
using Models.Responses;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace BlazorClient.Repositories
{
    public class TruckRepository : ITruckRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _requestUrl;

        public TruckRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _requestUrl = $"{_httpClient.BaseAddress}api/trucks";
        }

        public async Task<bool> DeleteTruckAsync(Guid id)
        {
            var apiResponse = await _httpClient.DeleteAsync($"{_requestUrl}/{id}");

            return apiResponse.IsSuccessStatusCode;
        }

        public async Task<Truck[]> GetTrucksAsync()
        {
            var apiResponse = await _httpClient.GetAsync(_requestUrl);

            return await HttpResponseHelper.DeserializeResponseAndThrowIfNotSuccess<Truck[]>(apiResponse);
        }

        public async Task<Truck> GetTruckAsync(Guid id)
        {
            var apiResponse = await _httpClient.GetAsync($"{_requestUrl}/{id}");

            return await HttpResponseHelper.DeserializeResponseAndThrowIfNotSuccess<Truck>(apiResponse);
        }

        public async Task<Truck> PostTruckAsync(Truck truck)
        {
            string jsonRequestBody = JsonConvert.SerializeObject(truck);
            HttpContent content = new StringContent(jsonRequestBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var apiResponse = await _httpClient.PostAsync(_requestUrl, content);

            return await HttpResponseHelper.DeserializeResponseAndThrowIfNotSuccess<Truck>(apiResponse);
        }

        public async Task<Truck> PutTruckAsync(Truck truck)
        {
            string jsonRequestBody = JsonConvert.SerializeObject(truck);

            HttpContent content = new StringContent(jsonRequestBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var apiResponse = await _httpClient.PutAsync($"{_requestUrl}/{truck.Id}", content);

            return await HttpResponseHelper.DeserializeResponseAndThrowIfNotSuccess<Truck>(apiResponse);
        }
    }
}