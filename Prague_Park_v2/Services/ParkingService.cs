using Prague_Park_v2.Core.Models;
using Prague_Park_v2.Core.Services;
using Spectre.Console;

namespace Prague_Park_v2.Services
{
    public static class ParkingService
    {
        // Handles parking a vehicle in the garage
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

        // Handles removing a vehicle from the garage
        public static void RemoveVehicle(ParkingGarage garage)
        {
            Console.WriteLine("License plate to remove: ");
            string? plate = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(plate))
            {
                Console.WriteLine("License plate is required");
                return;
            }

            if(garage.TryRemoveVehicle(plate, out var removed) && removed != null)
            {
                Console.WriteLine($"Removed {removed.LicensePlate}. Checkout info: ");
                
                // Calculate checkout and display with Spectre.Console
                var checkoutInfo = removed.CalculateCheckout(DateTime.Now);
                DisplayCheckout(checkoutInfo);
            }
            else
            {
                Console.WriteLine("Vehicle not found.");
            }
        }

        private static void DisplayCheckout(CheckoutInfo info)
        {
            string freeMessage = info.IsFree ? "[green]Parking is free![/] (Duration under 10 minutes)\n" : "";
            
            var panel = new Panel(
                $"[bold yellow]Vehicle Checkout[/]\n" +
                $"[green]License Plate:[/] [white]{info.LicensePlate}[/]\n" +
                $"[blue]Total Duration:[/] [white]{info.Hours}[/] hours and [white]{info.Minutes}[/] minute(s)\n" +
                freeMessage +
                $"[yellow]Total Price:[/] [white]{info.TotalPrice}[/] currency units"
            )
            .Header("[bold green]Checkout Summary[/]", Justify.Center)
            .Border(BoxBorder.Rounded)
            .Padding(1, 1)
            .Expand();

            AnsiConsole.Write(panel);
        }
    }
}