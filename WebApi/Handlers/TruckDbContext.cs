using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApi.Handlers
{
    public class TruckDbContext : DbContext
    {
        public TruckDbContext(DbContextOptions<TruckDbContext> options) : base(options)
        {
        }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Model> Models { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            PopulatePlantData(modelBuilder);
            PopulateModelData(modelBuilder);

            modelBuilder.Entity<Truck>()
                .HasOne(truck => truck.Plant)
                .WithOne()
                .HasForeignKey<Truck>(truck => truck.PlantId);

            modelBuilder.Entity<Truck>()
                .HasOne(truck => truck.Model)
                .WithOne()
                .HasForeignKey<Truck>(truck => truck.ModelId);
        }

        private void PopulatePlantData(ModelBuilder modelBuilder)
        {
            List<Plant> plants = new List<Plant>();

            string[] countries = { "Brasil", "França", "Estados Unidos", "Suécia" };

            for (int i = 1; i <= countries.Length; i++) 
            { 
                plants.Add(new Plant { Id = i, CountryName = countries[i - 1] });
            }

            modelBuilder.Entity<Plant>().HasData(plants);
        }

        private void PopulateModelData(ModelBuilder modelBuilder)
        {
            List<Model> models = new List<Model>();

            string[] modelNames = { "FH", "FM", "VM" };

            for (int i = 1; i <= modelNames.Length; i++) 
            {
                models.Add(new Model { Id = i, Name = modelNames[i - 1] });
            }

            modelBuilder.Entity<Model>().HasData(models);
        }
    }
}
