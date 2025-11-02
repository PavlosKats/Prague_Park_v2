using Prague_Park_v2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prague_Park_v2.Utils
{
    // Utility class to optimise parking of Mc vehicles
    public class McOptimiser
    {
        public static void Optimise(ParkingGarage garage)
        {
            // Collect all parked Mc vehicles
            var allMcs = garage.Garage
                .SelectMany(spot => spot.ParkedVehicles)
                .OfType<Mc>()
                .ToList();

            if (allMcs.Count == 0)
            {
                Console.WriteLine("No motorcycle vehicles in the parking.");
                return;

            }

            // Remove all Mcs from their current spots
            foreach (var spot in garage.Garage)
            {
                spot.ParkedVehicles.RemoveAll(v => v is Mc);
                spot.AvailableSize = spot.Size - spot.ParkedVehicles.Sum(v => v.Size);
            }

            // Only use empty spots for grouping
            var emptySpots = garage.Garage
                .Where(spot => spot.ParkedVehicles.Count == 0)
                .ToList();

            //Track moves for user feedback
            var movedMcs = new List<(string LicensePlate, int SpotNumber)>();

            // Try to park all Mcs together in as few spots as possible
            var mcQueue = new Queue<Mc>(allMcs);

            foreach (var spot in emptySpots)
            {
                int spotCapacity = spot.AvailableSize;
                int filled = 0;
                while (filled < spotCapacity && mcQueue.Count > 0)
                {
                    var mc = mcQueue.Dequeue();
                    spot.AddVehicle(mc);
                    movedMcs.Add((mc.LicensePlate ?? "Unknown", spot.SpotNumber));
                    filled++;
                }
                if (mcQueue.Count == 0)
                    break;
            }

            //Show results to user
            if (movedMcs.Count > 0)
            {
                Console.WriteLine("Motorcycles moved and grouped:");
                foreach (var move in movedMcs)
                {
                    Console.WriteLine($" - MC {move.LicensePlate} moved to parking spot { move.SpotNumber}");
                }
            }
            // If there are leftover Mcs, print a warning
            if (mcQueue.Count > 0)
            {
                Console.WriteLine("Warning: Not enough empty spots to group all motorcycles together.");
            }
        }
    }
}
