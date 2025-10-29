using Spectre.Console;
using Prague_Park_v2.Models;
using Prague_Park_v2.Services;
using Prague_Park_v2.Utils;
using System;
using System.IO;

namespace Prague_Park_v2
{
    internal class Program
    {

        //fix currency display

        //Implement two unit tests with MSTest
        //Fix comments.
        //Create readme.md
        //Write a summary/reflection.
        //Find out why $type of vehicle is problematic when saving and loading.

        static void Main(string[] args)
        {
            string baseDir = AppContext.BaseDirectory;
            string configPath = Path.Combine(baseDir, "appconfig.json");
            string dataPath = Path.Combine(baseDir, "parking_garage_data.json");

            // Load config (contains prices)
            AppConfig config = ConfigManager.Load(configPath);

            // Create factory from config
            var factory = new VehicleFactory(config);

            // Load or create garage using configured size
            var garage = ParkingGarage.LoadFromFile(dataPath, config.GarageSize);
;

            bool running = true;
            // Menu choices
            while (running)
            {
                AnsiConsole.Clear();
                var title = new FigletText("Prague Parking V2")
                    .Centered()
                    .Color(Color.Green);
                AnsiConsole.WriteLine();

                //Menu
                var menuContent =
                    "[bold]1)[/] Park Vehicle\n" +
                    "[bold]2)[/] Remove a Vehicle\n" +
                    "[bold]3)[/] Show parking garage map\n" +
                    "[bold]4)[/] Optimise Motorcycle vehicles\n" +
                    "[bold]──────────────────────────────[/]\n" +
                    "[bold]5)[/] Show vehicle parking prices\n" +
                    "[bold]6)[/] Change vehicle parking Prices\n" +
                    "[bold]7)[/] Change vehicle parking size\n" +
                    "[bold]8)[/] Save config & data\n" +
                    "[bold]──────────────────────────────[/]\n" +
                    "[bold]9)[/] Exit app";

                // Show menu in a panel
                var panel = new Panel(menuContent)
                    .Header("[yellow]Welcome to Prague Parking App[/]", Justify.Center)
                    .Border(BoxBorder.Rounded)
                    .Padding(1, 1)
                    .Expand();

                AnsiConsole.Write(panel);

                // Prompt for input
                AnsiConsole.Markup("[grey]Type the number of your choice and press Enter:[/] ");
                string? typedInput = Console.ReadLine();

                string choice = typedInput?.Trim();


                switch (choice)
                {
                    case "1":
                        ParkingService.ParkVehicle(garage, factory);
                        break;

                    case "2":
                        ParkingService.RemoveVehicle(garage);
                        break;

                    case "3":
                        ParkingDisplayService.ShowGarageMap(garage);
                        break;
                    case "4":
                        McOptimiser.Optimise(garage);
                        break;

                    case "5":
                        
                        if (config.VehicleTypes != null && config.VehicleTypes.Count > 0)
                        {
                            var priceTable = new Table()
                                .Border(TableBorder.Rounded)
                                .Title("[yellow]Vehicle Parking Prices[/]")
                                .AddColumn("[bold]Type[/]")
                                .AddColumn("[bold]Size[/]")
                                .AddColumn("[bold]Price Per Hour[/]");

                            foreach (var vt in config.VehicleTypes)
                            {
                                priceTable.AddRow(
                                    $"[green]{vt.Type}[/]",
                                    $"[blue]{vt.Size}[/]",
                                    $"[yellow]{vt.PricePerHour}[/]"
                                );
                            }

                            AnsiConsole.Write(priceTable);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]No vehicle types configured.[/]");
                        }
                        break;

                    case "6":
                        AnsiConsole.Markup("[yellow]Vehicle type to edit (Car or Mc):[/] ");
                        string? type = Console.ReadLine();
                        AnsiConsole.Markup("[yellow]New price per hour:[/] ");
                        if (int.TryParse(Console.ReadLine(), out int newPrice) && !string.IsNullOrWhiteSpace(type))
                        {
                            if (ConfigManager.SetPriceForType(config, type!, newPrice))
                            {
                                ConfigManager.Save(configPath, config);
                                AnsiConsole.MarkupLine("[green]Price updated and saved to appconfig.json.[/]");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[red]Vehicle type not found in config.[/]");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Invalid input.[/]");
                        }
                        break;

                    case "7":
                        AnsiConsole.Markup("[yellow]Enter new number of parking spots:[/] ");
                        string? newSizeInput = Console.ReadLine();
                        if (int.TryParse(newSizeInput, out int newSize) && newSize > 0)
                        {
                            var allVehicles = garage.Garage
                                .SelectMany(spot => spot.ParkedVehicles)
                                .ToList();

                            config.GarageSize = newSize;
                            var newGarage = new ParkingGarage(newSize);

                            foreach (var vehicle in allVehicles)
                            {
                                if (!newGarage.TryParkVehicle(vehicle, out int spot))
                                {
                                    AnsiConsole.MarkupLine($"[red]Warning: Could not re-park vehicle {vehicle.LicensePlate} (type: {vehicle.GetType().Name}) due to insufficient space.[/]");
                                }
                            }

                            garage = newGarage;
                            ConfigManager.Save(configPath, config);
                            AnsiConsole.MarkupLine($"[green]Garage size updated to {newSize}, vehicles transferred, and config saved to appconfig.json.[/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Invalid input. Garage size must be a positive integer.[/]");
                        }
                        break;


                    case "8":

                        ConfigManager.Save(configPath, config);
                        AnsiConsole.MarkupLine("[green]Saved data and config.[/]");
                        AnsiConsole.MarkupLine("\n[bold]Updated configuration:[/]");
                        ConfigDisplayService.PrintConfig(config);
                        AnsiConsole.MarkupLine("[yellow]Returning to menu[/]");
                        break;

                    case "9":
                        garage.SaveToFile(dataPath);
                        ConfigManager.Save(configPath, config);
                        running = false;
                        AnsiConsole.MarkupLine("[bold red]Exiting.[/]");
                        break;

                    

                    default:
                        AnsiConsole.MarkupLine("[red]Unknown choice.[/]");
                        break;

                }
                if (running)
                {
                    AnsiConsole.MarkupLine("\n[grey]Press any key to return to menu...[/]");
                    Console.ReadKey(true);
                }
            }
        }
    }
}