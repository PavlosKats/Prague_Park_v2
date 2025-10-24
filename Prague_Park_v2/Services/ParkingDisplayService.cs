using Prague_Park_v2.Models;
using System;

namespace Prague_Park_v2.Services
{
    public static class ParkingDisplayService
    {
        public static void ShowGarageMap(ParkingGarage garage)
        {
            Console.WriteLine("Parking Garage Map:");
            foreach (var spot in garage.Garage)
            {
                string status = spot.ParkedVehicles.Count > 0
                    ? $"Occupied by: {string.Join(", ", spot.ParkedVehicles.Select(v => $"{v.LicensePlate} ({v.GetType().Name})"))}"
                    : "Empty";
                Console.WriteLine($"Spot {spot.SpotNumber}: {status} (Free space: {spot.AvailableSize}/{spot.Size})");
            }
        }
    }
}