using Prague_Park_v2.Models;
using Prague_Park_v2.Services;

namespace Prague_Park_v2.Tests

{
    [TestClass]
    public sealed class ConfigManagerTests
    {
        // Generates a temporary file path for testing
        private static string TempConfigPath()
        {
            var dir = Path.Combine(Path.GetTempPath(), "PragueParkTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, "appconfig.json");
        }

        [TestMethod]
        public void SaveThenLoad_PersistsData()
        {
            // Arrange
            var path = TempConfigPath();
            try
            {
                var cfg = new AppConfig
                {
                    GarageSize = 42,
                    VehicleTypes = new List<VehicleTypeConfig>
                    {
                        new VehicleTypeConfig { Type = "Truck", Size = 3, PricePerHour = 200 }
                    },
                    ParkingSpot = new ParkingSpotConfig { Size = 10, Height = 5 }
                };

                // Ensure directory exists before Save
                var dir = Path.GetDirectoryName(path);
                //create dir if not exists
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

                // Act
                ConfigManager.Save(path, cfg);
                var loaded = ConfigManager.Load(path);

                // Assert
                Assert.IsNotNull(loaded);
                Assert.AreEqual(42, loaded.GarageSize);
                Assert.IsNotNull(loaded.VehicleTypes);
                Assert.AreEqual(1, loaded.VehicleTypes.Count);
                Assert.AreEqual("Truck", loaded.VehicleTypes[0].Type);
                Assert.AreEqual(200, loaded.VehicleTypes[0].PricePerHour);
            }
            finally
            {
                // Clean up
                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir)) Directory.Delete(dir, recursive: true);
            }
        }
    }
}
