using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prague_Park_v2.Models;
using Prague_Park_v2.Services;
using System;
using System.IO;
using Spectre.Console;
using System.Linq;

namespace Prague_Park_v2.Tests
{
    [TestClass]
    public class MoveVehicleServiceTests
    {
        [TestMethod]
        public void MoveVehicle_ValidMove_VehicleIsMoved()
        {
            // Arrange
            var garage = new ParkingGarage(2);
            var vehicle = new Vehicle("ABC123") { Size = 1 };
            garage.Garage[0].Size = 2;
            garage.Garage[1].Size = 2;
            garage.Garage[0].AvailableSize = 2;
            garage.Garage[1].AvailableSize = 2;
            garage.Garage[0].AddVehicle(vehicle);

            // Simulate user input for license plate and destination spot
            var input = new StringReader("ABC123\n2\n");
            Console.SetIn(input);

            // Redirect output to suppress Spectre.Console output in test
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            MoveVehicleService.MoveVehicle(garage);

            // Assert
            Assert.IsFalse(garage.Garage[0].ParkedVehicles.Any(v => v.LicensePlate == "ABC123"));
            Assert.IsTrue(garage.Garage[1].ParkedVehicles.Any(v => v.LicensePlate == "ABC123"));
        }
    }
}