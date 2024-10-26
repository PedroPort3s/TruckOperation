using Models;

namespace UnitTests.TestData
{
    internal static class TrucksTestDataGenerator
    {
        public static Truck CreateTruckInstance(int year, string color, string chassisCode, int modelId, int plantId, Guid? id = null) 
        {
            return new Truck
            {
                Id = id == null ? Guid.NewGuid() : id.Value,
                YearOfManufacture = year,
                Color = color,
                ChassisCode = chassisCode,
                ModelId = modelId,
                PlantId = plantId
            };
        }
    }
}
