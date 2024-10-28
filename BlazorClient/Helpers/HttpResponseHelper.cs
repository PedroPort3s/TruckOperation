using Models.Exceptions;
using Models.Responses;
using Newtonsoft.Json;

namespace BlazorClient.Helpers
{
    public static class HttpResponseHelper
    {
        internal static async Task<T> DeserializeResponseAndThrowIfNotSuccess<T>(HttpResponseMessage apiResponse)
        {
            string jsonResponse = await apiResponse.Content.ReadAsStringAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<TrucksApiErrorResponse>(jsonResponse);
                throw new ApiResponseException(errorResponse.Title);
            }
        }
    }
}
