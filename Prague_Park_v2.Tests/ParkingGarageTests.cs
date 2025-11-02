using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prague_Park_v2.Core.Models;
using Prague_Park_v2.Core.Services;


namespace Prague_Park_v2.Tests
{
    [TestClass]
    public class ParkingGarageTests
    {
        [TestMethod]
        public void TryParkVehicle_ParksVehicleInFirstAvailableSpot()
        {
            // Arrange
            var garage = new ParkingGarage(2);
            garage.Garage[0].Size = 2;
            garage.Garage[1].Size = 2;
            garage.Garage[0].AvailableSize = 2;
            garage.Garage[1].AvailableSize = 2;
            var vehicle = new Vehicle("XYZ789") { Size = 1 };

            // Act
            bool parked = garage.TryParkVehicle(vehicle, out int spotNumber);

            // Assert
            Assert.IsTrue(parked);
            Assert.AreEqual(1, spotNumber);
            Assert.IsTrue(garage.Garage[0].ParkedVehicles.Contains(vehicle));
        
        }
    }
}