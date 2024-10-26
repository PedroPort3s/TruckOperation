using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Handlers;
using WebApi.Controllers;
using Models;

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
                new Truck 
                { 
                    Id = Guid.NewGuid(), 
                    YearOfManufacture = 2020, 
                    ChassisCode = "ABC123", 
                    Color = "Red", 
                    ModelId = 1, 
                    PlantId = 1 
                },
                new Truck 
                { 
                    Id = Guid.NewGuid(), 
                    YearOfManufacture = 2021, 
                    ChassisCode = "DEF456", 
                    Color = "Blue", 
                    ModelId = 2, 
                    PlantId = 2 
                }
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
            context.Trucks.Add(new Truck 
            { 
                Id = truckId, 
                YearOfManufacture = 2020, 
                ChassisCode = "ABC123", 
                Color = "Red", 
                ModelId = 1, 
                PlantId = 1 
            });
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

        [Fact]
        public async Task PostTruck_AddsNewTruck()
        {
            var context = GetInMemoryDbContext("TestDatabase_PostTruck");
            MockModelsAndPlants(context);
            await context.SaveChangesAsync();

            var controller = new TrucksController(context);
            var newTruck = new Truck
            {
                Id = Guid.NewGuid(),
                YearOfManufacture = 2022,
                ChassisCode = "GHI789",
                Color = "Green",
                ModelId = 1,
                PlantId = 1
            };

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
            var truckId = Guid.NewGuid();
            var initialTruck = new Truck
            {
                Id = truckId,
                YearOfManufacture = 2020,
                ChassisCode = "ABC123",
                Color = "Red",
                ModelId = 1,
                PlantId = 1
            };

            var updatedTruck = new Truck
            {
                Id = initialTruck.Id,
                YearOfManufacture = 2021,
                ChassisCode = "XYZ987",
                Color = "Blue",
                ModelId = initialTruck.ModelId,
                PlantId = initialTruck.PlantId
            };

            MockModelsAndPlants(context);

            context.Trucks.Add(initialTruck);
            await context.SaveChangesAsync();

            var controller = new TrucksController(context);
            context.Entry(initialTruck).State = EntityState.Detached;

            var result = await controller.PutTruck(truckId, updatedTruck);

            Assert.IsType<OkObjectResult>(result);
            var truckInDb = await context.Trucks.FindAsync(truckId);
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

        [Fact]
        public async Task DeleteTruck_RemovesTruck()
        {
            var context = GetInMemoryDbContext("TestDatabase_DeleteTruck");
            var truckId = Guid.NewGuid();
            context.Trucks.Add(new Truck
            {
                Id = truckId,
                YearOfManufacture = 2020,
                ChassisCode = "ABC123",
                Color = "Red",
                ModelId = 1,
                PlantId = 1
            });
            await context.SaveChangesAsync();

            var controller = new TrucksController(context);
            var result = await controller.DeleteTruck(truckId);

            Assert.IsType<NoContentResult>(result);
            var truckInDb = await context.Trucks.FindAsync(truckId);
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
