using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Handlers;
using WebApi.Controllers;
using Models;
using UnitTests.TestData;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace UnitTests
{
    public class TrucksControllerTests
    {
        private TruckDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TruckDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new TruckDbContext(options);
        }

        [Fact]
        public async Task GetTrucks_ReturnsAllTrucks()
        {
            var context = GetInMemoryDbContext("TestDatabase_GetTrucks");
            context.Trucks.AddRange(new List<Truck>
            {
                TrucksTestDataGenerator.CreateTruckInstance(year: 2020,color: "Red",chassisCode: "ABC123",modelId: 1 ,plantId: 1),
                TrucksTestDataGenerator.CreateTruckInstance(year: 2021,color: "Blue",chassisCode: "DEF456",modelId: 2 ,plantId: 2)
            });
            await context.SaveChangesAsync();

            var controller = new TrucksController(context);

            var result = await controller.GetTrucks();
            var trucks = result.Value.ToList();

            Assert.NotNull(trucks);
            Assert.Equal(2, trucks.Count);
        }

        [Fact]
        public async Task GetTruck_ReturnsTruckById()
        {
            var context = GetInMemoryDbContext("TestDatabase_GetTruck");
            var truckId = Guid.NewGuid();
            context.Trucks.Add(TrucksTestDataGenerator.CreateTruckInstance(year: 2020, color: "Red", chassisCode: "ABC123", modelId: 1, plantId: 1, id: truckId));
            await context.SaveChangesAsync();

            var controller = new TrucksController(context);

            var result = await controller.GetTruck(truckId);
            var truck = result.Value;

            Assert.NotNull(truck);
            Assert.Equal(truckId, truck.Id);
        }

        [Fact]
        public async Task GetTruck_ReturnsNotFound_ForInvalidId()
        {
            var context = GetInMemoryDbContext("TestDatabase_GetTruckNotFound");
            var controller = new TrucksController(context);

            var result = await controller.GetTruck(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Theory]
        [ClassData(typeof(InvalidTrucksTestDataGenerator))]
        public async Task PostTruck_AddsNewTruckWithoutRequiredFields(Truck truck)
        {
            var context = GetInMemoryDbContext("TestDatabase_PostBatchInvalidTruck");
            
            var controller = new TrucksController(context);

            var exception = await Assert.ThrowsAsync<BadHttpRequestException>(async () => 
            {
                _ = await controller.PostTruck(truck);
            });

            Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        }
        
        [Fact]
        public async Task PostTruck_AddsNewTruck()
        {
            var context = GetInMemoryDbContext("TestDatabase_PostTruck");
            MockModelsAndPlants(context);
            await context.SaveChangesAsync();

            var controller = new TrucksController(context);
            var newTruck = TrucksTestDataGenerator.CreateTruckInstance(year: 2022, color: "Green", chassisCode: "GHI789", modelId: 1, plantId: 1);
            
            var result = await controller.PostTruck(newTruck);
            var createdResult = result.Result as CreatedAtActionResult;
            var truck = createdResult.Value as Truck;

            Assert.NotNull(createdResult);
            Assert.NotNull(truck);
            Assert.Equal(newTruck.Id, truck.Id);
        }

        [Fact]
        public async Task PutTruck_UpdatesExistingTruck()
        {
            var context = GetInMemoryDbContext("TestDatabase_PutTruck");
            
            var initialTruck = TrucksTestDataGenerator.CreateTruckInstance(year: 2020, color: "Red", chassisCode: "ABC123", modelId: 1, plantId: 1);

            var updatedTruck = TrucksTestDataGenerator.CreateTruckInstance(year: 2021, color: "Blue", chassisCode: "XYZ987", modelId: initialTruck.ModelId, plantId: initialTruck.PlantId, id: initialTruck.Id);
            
            MockModelsAndPlants(context);

            context.Trucks.Add(initialTruck);
            await context.SaveChangesAsync();

            var controller = new TrucksController(context);
            context.Entry(initialTruck).State = EntityState.Detached;

            var result = await controller.PutTruck(initialTruck.Id, updatedTruck);

            Assert.IsType<OkObjectResult>(result);
            var truckInDb = await context.Trucks.FindAsync(initialTruck.Id);
            Assert.Equal(2021, truckInDb.YearOfManufacture);
            Assert.Equal("XYZ987", truckInDb.ChassisCode);
            Assert.Equal("Blue", truckInDb.Color);
        }

        [Fact]
        public async Task PutTruck_UpdatesNotExistingTruck()
        {
            var context = GetInMemoryDbContext("TestDatabase_PutTruck");
            var truckId = Guid.NewGuid();

            var truckNotExistent = new Truck
            {
                Id = truckId
            };

            var controller = new TrucksController(context);

            var result = await controller.PutTruck(truckId, truckNotExistent);

            Assert.IsType<BadRequestResult>(result);
        }
        
        [Fact]
        public async Task PutTruck_UpdatesDifferentTruckIdInQueryAndBody()
        {
            var context = GetInMemoryDbContext("TestDatabase_PutTruck");
            var truckId = Guid.NewGuid();

            var truckNotExistent = new Truck
            {
                Id = Guid.NewGuid()
            };

            var controller = new TrucksController(context);

            var result = await controller.PutTruck(truckId, truckNotExistent);

            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [ClassData(typeof(InvalidTrucksTestDataGenerator))]
        public async Task PutTruck_EditTruckWithoutRequiredFields(Truck truck)
        {
            var context = GetInMemoryDbContext("TestDatabase_PutBatchInvalidTruck");

            context.Trucks.Add(truck);

            await context.SaveChangesAsync();

            var controller = new TrucksController(context);

            var exception = await Assert.ThrowsAsync<BadHttpRequestException>(async () =>
            {
                _ = await controller.PutTruck(truck.Id, truck);
            });

            Assert.Equal((int)HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Fact]
        public async Task DeleteTruck_RemovesTruck()
        {
            var context = GetInMemoryDbContext("TestDatabase_DeleteTruck");

            var truck = TrucksTestDataGenerator.CreateTruckInstance(year: 2020, color: "Red", chassisCode: "ABC123", modelId: 1, plantId: 1);

            context.Trucks.Add(truck);

            await context.SaveChangesAsync();

            var controller = new TrucksController(context);
            var result = await controller.DeleteTruck(truck.Id);

            Assert.IsType<NoContentResult>(result);
            var truckInDb = await context.Trucks.FindAsync(truck.Id);
            Assert.Null(truckInDb);
        }
        
        [Fact]
        public async Task DeleteTruck_NotExistingTruck()
        {
            var context = GetInMemoryDbContext("TestDatabase_DeleteTruck");
            var truckId = Guid.NewGuid();

            var controller = new TrucksController(context);
            var result = await controller.DeleteTruck(truckId);

            Assert.IsType<NotFoundResult>(result);
        }

        private void MockModelsAndPlants(TruckDbContext context)
        {
            context.Models.Add(new Model { Id = 1, Name = "Model A" });
            context.Plants.Add(new Plant { Id = 1, CountryName = "Brasil" });
        }
    }
}
