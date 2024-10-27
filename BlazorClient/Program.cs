using BlazorClient.Repositories;
using BlazorClient.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7265") });

            AddRepositories(builder);

            await builder.Build().RunAsync();
        }

        private static void AddRepositories(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddTransient<IModelRepository, ModelRepository>();
            builder.Services.AddTransient<ITruckRepository, TruckRepository>();
            builder.Services.AddTransient<IPlantRepository, PlantRepository>();
        }
    }
}
