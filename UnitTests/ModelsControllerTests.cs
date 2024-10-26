using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Handlers;
using WebApi.Controllers;
using Models;

namespace UnitTests
{
    public class ModelsControllerTests
    {
        private TruckDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TruckDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new TruckDbContext(options);
        }

        [Fact]
        public async Task GetModels_ReturnsAllModels()
        {   
            var context = GetInMemoryDbContext("TestDatabase_GetModels");
            context.Models.AddRange(new List<Model>
            {
                new Model { Id = 1, Name = "Test Model A" },
                new Model { Id = 2, Name = "Test Model B" }
            });
            await context.SaveChangesAsync();

            var controller = new ModelsController(context);

            var result = await controller.GetModels();
            var models = result.Value.ToList();
            
            Assert.NotNull(models);
            Assert.Equal(2, models.Count);
        }

        [Fact]
        public async Task GetModel_ReturnsModelById()
        {   
            var context = GetInMemoryDbContext("TestDatabase_GetModel");
            context.Models.Add(new Model { Id = 1, Name = "Test Model A" });
            await context.SaveChangesAsync();

            var controller = new ModelsController(context);
            
            var result = await controller.GetModel(1);
            var model = result.Value;

            Assert.NotNull(model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Test Model A", model.Name);
        }

        [Fact]
        public async Task GetModel_ReturnsNotFound_ForInvalidId()
        {   
            var context = GetInMemoryDbContext("TestDatabase_GetModelNotFound");
            var controller = new ModelsController(context);
            
            var result = await controller.GetModel(99);
            
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}