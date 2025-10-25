using Prague_Park_v2.Models;
using System.Linq;

namespace Prague_Park_v2.Services
{
    public static class ParkingService
    {
        public static void ParkVehicle(ParkingGarage garage, VehicleFactory factory)
        {
            Console.Write("Vehicle type to park (Car/Mc): ");
            string? type = Console.ReadLine();
            Console.Write("License plate: ");
            string? plate = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(plate))
            {
                Console.WriteLine("Vehicle type and license plate are required.");
                return;
            }
            //check duplicate license plates
            bool duplicate = garage.Garage.Any(spot =>
                spot.ParkedVehicles.Any(v => v.LicensePlate?.Equals(plate, StringComparison.OrdinalIgnoreCase) == true));

            if (duplicate)
            {
                Console.WriteLine("This license plate is already parked in the garage.");
                return;
            }

            try
            {
                var vehicle = factory.Create(type, plate);
                if (garage.TryParkVehicle(vehicle, out int spot))
                {
                    Console.WriteLine($"Vehicle {vehicle.LicensePlate} parked at spot {spot}.");
                }

                else
                {
                    Console.WriteLine("This license plate is already parked or no space available for this vehicle.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void RemoveVehicle(ParkingGarage garage)
        {
            Console.WriteLine("License plate to remove: ");
            string? plate = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(plate))
            {
                Console.WriteLine("License plate is required");
                return;
            }

            if(garage.TryRemoveVehicle(plate, out var removed))
            {
                Console.WriteLine($"Removed {removed?.LicensePlate}. Checkout info: ");
                removed?.CheckoutVehicle(DateTime.Now);
            }
            else
            {
                Console.WriteLine("Vehicle not found.");
            }
        }
    }
}