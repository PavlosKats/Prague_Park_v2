using Prague_Park_v2.Models;
using Spectre.Console;
using System;

namespace Prague_Park_v2.Utils
{
    public static class ConfigDisplayService
    {
        public static void PrintConfig(AppConfig config)
        {

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
