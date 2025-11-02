using Prague_Park_v2.Core.Models;
using Spectre.Console;
using System;
using System.Linq;

namespace Prague_Park_v2.Services
{
    public class MoveVehicleService
    {
        public static void MoveVehicle(ParkingGarage garage)
        {
            // Ask for license plate
            var plate = AnsiConsole.Ask<string>("[yellow]Enter the license plate of the vehicle to move:[/]");
            if (string.IsNullOrWhiteSpace(plate))
            {
                AnsiConsole.MarkupLine("[red]License plate is required.[/]");
                return;
            }

            // Find the vehicle and its current spot
            var currentSpot = garage.Garage.FirstOrDefault(s => s.CheckLicensePlate(plate));
            if (currentSpot == null)
            {
                AnsiConsole.MarkupLine($"[red]Vehicle with license plate {plate} not found in the garage.[/]");
                return;
            }
            var vehicle = currentSpot.ParkedVehicles.FirstOrDefault(v => v.LicensePlate == plate);
            if (vehicle == null)
            {
                AnsiConsole.MarkupLine($"[red]Vehicle with license plate {plate} not found in the garage.[/]");
                return;
            }

            // Ask user for destination spot number
            int destinationSpotNumber = AnsiConsole.Ask<int>("[yellow]Enter the destination spot number:[/]");
            var destinationSpot = garage.Garage.FirstOrDefault(s => s.SpotNumber == destinationSpotNumber);

            if (destinationSpot == null)
            {
                AnsiConsole.MarkupLine($"[red]Spot {destinationSpotNumber} does not exist.[/]");
                return;
            }
            if (destinationSpot.SpotNumber == currentSpot.SpotNumber)
            {
                AnsiConsole.MarkupLine("[red]Vehicle is already in the selected spot.[/]");
                return;
            }
            if (!destinationSpot.CanFitVehicle(vehicle))
            {
                AnsiConsole.MarkupLine($"[red]Spot {destinationSpotNumber} does not have enough space for this vehicle (required: {vehicle.Size}, available: {destinationSpot.AvailableSize}).[/]");
                return;
            }

            // Move vehicle
            currentSpot.RemoveVehicle(vehicle);
            destinationSpot.AddVehicle(vehicle);

            AnsiConsole.MarkupLine(
                $"[green]Vehicle {vehicle.LicensePlate} moved from spot {currentSpot.SpotNumber} to spot {destinationSpot.SpotNumber}.[/]"
            );
        }
    }
}