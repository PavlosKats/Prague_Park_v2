using Prague_Park_v2.Models;
using Spectre.Console;
using System.Linq;

namespace Prague_Park_v2.Services
{
    internal class ResizeParkingSize
    {
        /// <summary>
        /// Resizes the parking garage according to user input, preserving vehicles.
        /// Returns the new ParkingGarage instance.
        /// </summary>
        public static ParkingGarage Resize(ParkingGarage garage)
        {
            int newSize = AnsiConsole.Ask<int>("[yellow]Enter new number of parking spots:[/]");
            if (newSize <= 0)
            {
                AnsiConsole.MarkupLine("[red]Garage size must be a positive integer.[/]");
                return garage;
            }

            // Collect all vehicles from current spots
            var allVehicles = garage.Garage.SelectMany(spot => spot.ParkedVehicles).ToList();

            // Create new garage, copying spot sizes
            var newGarage = new ParkingGarage(newSize);
            for (int i = 0; i < newSize; i++)
            {
                int sizeToUse = (i < garage.Garage.Count)
                    ? garage.Garage[i].Size
                    : garage.Garage.Last().Size; // Use last spot's size for extra spots

                newGarage.Garage[i].Size = sizeToUse;
                newGarage.Garage[i].AvailableSize = sizeToUse;
            }

            int notParked = 0;  
            foreach (var vehicle in allVehicles)
            {
                if (!newGarage.TryParkVehicle(vehicle, out int spot))
                {
                    notParked++;
                    AnsiConsole.MarkupLine($"[red]Warning: Could not re-park vehicle {vehicle.LicensePlate} (type: {vehicle.GetType().Name}) due to insufficient space.[/]");
                }
            }

            AnsiConsole.MarkupLine($"[green]Garage size updated to {newSize}, vehicles transferred.[/]");
            if (notParked > 0)
                AnsiConsole.MarkupLine($"[red]{notParked} vehicle(s) could not be re-parked.[/]");

            return newGarage;
        }
    }
}