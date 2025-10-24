using Prague_Park_v2.Models;

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

            try
            {
                var vehicle = factory.Create(type, plate);
                if (garage.TryParkVehicle(vehicle, out int spot))
                {
                    Console.WriteLine($"Vehicle {vehicle.LicensePlate} parked at spot {spot}.");
                }
                else
                {
                    Console.WriteLine("No space available for this vehicle.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}