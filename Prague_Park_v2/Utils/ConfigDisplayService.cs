using Prague_Park_v2.Models;
using System;

namespace Prague_Park_v2.Utils
{
    public static class ConfigDisplayService
    {
        public static void PrintConfig(AppConfig config)
        {
            Console.WriteLine("==== Current Configuration ====");
            Console.WriteLine($"Garage Size: {config.GarageSize}");
            if (config.ParkingSpot != null)
            {
                Console.WriteLine($"Parking Spot Size: {config.ParkingSpot.Size}");
                Console.WriteLine($"Parking Spot Height: {config.ParkingSpot.Height}");
            }
            Console.WriteLine("Vehicle Types:");
            if (config.VehicleTypes != null)
            {
                foreach (var vt in config.VehicleTypes)
                {
                    Console.WriteLine($" - {vt.Type}: Size = {vt.Size}, PricePerHour = {vt.PricePerHour}");
                }
            }
            Console.WriteLine("==============================");
        }
    }
}
