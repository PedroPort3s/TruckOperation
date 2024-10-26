using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Handlers;
using WebApi.Controllers;
using Models;

namespace UnitTests
{
    public class PlantsControllerTests
    {
        private TruckDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TruckDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new TruckDbContext(options);
        }

        [Fact]
        public async Task GetPlants_ReturnsAllPlants()
        {  
            var context = GetInMemoryDbContext("TestDatabase_GetPlants");
            context.Plants.AddRange(new List<Plant>
            {
                new Plant { Id = 1, CountryName = "Brasil" },
                new Plant { Id = 2, CountryName = "França" }
            });
            await context.SaveChangesAsync();

            var controller = new PlantsController(context);
            
            var result = await controller.GetPlants();
            var plants = result.Value.ToList();

            Assert.NotNull(plants);
            Assert.Equal(2, plants.Count);
        }

        [Fact]
        public async Task GetPlant_ReturnsPlantById()
        {  
            var context = GetInMemoryDbContext("TestDatabase_GetPlant");
            context.Plants.Add(new Plant { Id = 1, CountryName = "Brasil" });
            await context.SaveChangesAsync();

            var controller = new PlantsController(context);
            
            var result = await controller.GetPlant(1);
            var plant = result.Value;

            Assert.NotNull(plant);
            Assert.Equal(1, plant.Id);
            Assert.Equal("Brasil", plant.CountryName);
        }

        [Fact]
        public async Task GetPlant_ReturnsNotFound_ForInvalidId()
        {  
            var context = GetInMemoryDbContext("TestDatabase_GetPlantNotFound");
            var controller = new PlantsController(context);
            
            var result = await controller.GetPlant(99);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}