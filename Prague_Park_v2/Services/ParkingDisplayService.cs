using Prague_Park_v2.Models;
using Spectre.Console;
using System;

namespace Prague_Park_v2.Services
{
    public static class ParkingDisplayService
    {
        public static void ShowGarageMap(ParkingGarage garage)
        {
            int totalSpots = garage.Size;
            int columns = 10;
            int rows = (int)Math.Ceiling(totalSpots / (double)columns);

            var table = new Table()
                .Border(TableBorder.Rounded)
                .Collapse()
                .HideHeaders();

            //add columns
            for (int c = 0; c < rows; c++)
                table.AddColumn("");

            int spotIndex = 0;
            for (int r = 0; r < rows; r++)
            {
                var row = new List<Markup>();
                for (int c = 0;c < columns; c++)
                {
                    if (spotIndex < totalSpots)
                    {
                        var spot = garage.Garage[spotIndex];
                        string cellContent;
                        if (spot.ParkedVehicles.Count == 0)
                        {
                            // Free spot: show spot number and green square
                            cellContent = $"[grey]{spot.SpotNumber}[/]\n[green]■[/]";
                        }
                        else
                        {
                            // Occupied: show spot number and all vehicles, color-coded
                            var vehiclesMarkup = spot.ParkedVehicles.Select(v =>
                            {
                                string color = v.Size  switch
                                {
                                    2 => "yellow",   // e.g., Mc
                                    4 => "blue",     // e.g., Car
                                    6 => "magenta",  // larger vehicle
                                    8 => "red",      // even larger
                                    _ => "grey"      // unknown size
                                };
                                return $"[{color}]{v.LicensePlate}[/]";
                            });
                            cellContent = $"[grey]{spot.SpotNumber}[/]\n{string.Join(", ", vehiclesMarkup)}";
                        }
                        row.Add(new Markup(cellContent));
                        spotIndex++;
                    }
                    else
                    {
                        row.Add(new Markup(" "));
                    }
                }
                table.AddRow(row.ToArray());
            }
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[green]■[/] = Free spot, [blue]LicensePlate[/] = Car, [yellow]LicensePlate[/] = Mc, [red]LicensePlate[/] = Other");

        }
    }
}