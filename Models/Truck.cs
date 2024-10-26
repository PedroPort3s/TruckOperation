namespace Models
{
    public sealed class Truck
    {
        public Guid Id { get; set; }
        public int YearOfManufacture { get; set; }
        public string ChassisCode { get; set; }
        public string Color { get; set; }

        public int ModelId { get; set; }
        public Model Model { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
    }
}
