using Prague_Park_v2.Models;
using Spectre.Console;
using System;

namespace Prague_Park_v2.Utils
{
    public static class ConfigDisplayService
    {
        public static void PrintConfig(AppConfig config)
        {
            //Console.WriteLine("==== Current Configuration ====");
            //Console.WriteLine($"Garage Size: {config.GarageSize}");
            //if (config.ParkingSpot != null)
            //{
            //    Console.WriteLine($"Parking Spot Size: {config.ParkingSpot.Size}");
            //    Console.WriteLine($"Parking Spot Height: {config.ParkingSpot.Height}");
            //}
            //Console.WriteLine("Vehicle Types:");
            //if (config.VehicleTypes != null)
            //{
            //    foreach (var vt in config.VehicleTypes)
            //    {
            //        Console.WriteLine($" - {vt.Type}: Size = {vt.Size}, PricePerHour = {vt.PricePerHour}");
            //    }
            //}
            //Console.WriteLine("==============================");

            var table = new Table()
                .Border(TableBorder.Rounded)
                .Title("[yellow]Current Configuration[/]")
                .AddColumn("[bold]Property[/]")
                .AddColumn("[bold]Value[/]");

            table.AddRow("Garage Size", config.GarageSize.ToString());

            if (config.ParkingSpot != null)
            {
                table.AddRow("Parking Spot Size", config.ParkingSpot.Size.ToString());
                table.AddRow("Parking Spot Height", config.ParkingSpot.Height.ToString());
            }

            if (config.VehicleTypes != null && config.VehicleTypes.Count > 0)
            {
                foreach (var vt in config.VehicleTypes)
                {
                    table.AddRow(
                        $"Vehicle Type: [green]{vt.Type}[/]",
                        $"Size = [blue]{vt.Size}[/], PricePerHour = [yellow]{vt.PricePerHour}[/]"
                    );
                }
            }
            else
            {
                table.AddRow("Vehicle Types", "[red]None configured[/]");
            }

            AnsiConsole.Write(table);
        }
    }
    
}
