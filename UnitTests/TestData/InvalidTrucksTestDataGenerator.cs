using System.Collections;

namespace UnitTests.TestData
{
    public class InvalidTrucksTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { TrucksTestDataGenerator.CreateTruckInstance(0, string.Empty, string.Empty, 0, 0) },
            new object[] { TrucksTestDataGenerator.CreateTruckInstance(2024, string.Empty, string.Empty, 0, 0) },
            new object[] { TrucksTestDataGenerator.CreateTruckInstance(2024, "Red", string.Empty, 0, 0) },
            new object[] { TrucksTestDataGenerator.CreateTruckInstance(2024, "Red", "ABC123", 1, 0) },
            new object[] { TrucksTestDataGenerator.CreateTruckInstance(2024, "Red", "ABC123", 1, 1) }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
