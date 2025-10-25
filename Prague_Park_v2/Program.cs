using Prague_Park_v2.Models;
using Prague_Park_v2.Services;
using Prague_Park_v2.Utils;
using System;
using System.IO;

namespace Prague_Park_v2
{
    internal class Program
    {
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

            bool running = true;
            // Menu choices
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("1) Park Vehicle");
                Console.WriteLine("2) Remove a Vehicle");
                Console.WriteLine("3) Show parking garage map");
                Console.WriteLine("4) Show vehicle parking prices");
                Console.WriteLine("5) Change vehicle parking Prices");
                Console.WriteLine("6) Change vehicle parking size");
                Console.WriteLine("7) Save config & data");
                Console.WriteLine("8) Exit app");
                Console.Write("\nChoice: ");
                var choice = Console.ReadLine();
                Console.WriteLine();

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
                        Console.WriteLine("Configured vehicle prices:");
                        if (config.VehicleTypes != null)
                        {
                            foreach (var vt in config.VehicleTypes)
                            {
                                Console.WriteLine($" - {vt.Type}: Size = {vt.Size}, PricePerHour = {vt.PricePerHour}");
                            }
                        }
                        break;

                    case "5":
                        Console.Write("Vehicle type to edit (Car or Mc): ");
                        string? type = Console.ReadLine();
                        Console.Write("New price per hour: ");
                        if (int.TryParse(Console.ReadLine(), out int newPrice) && !string.IsNullOrWhiteSpace(type))
                        {
                            if (ConfigManager.SetPriceForType(config, type!, newPrice))
                            {
                                ConfigManager.Save(configPath, config);
                                Console.WriteLine("Price updated and saved to appconfig.json.");
                            }
                            else
                            {
                                Console.WriteLine("Vehicle type not found in config.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                        break;

                    case "6":
                        Console.Write("Enter new number of parking spots: ");
                        string? newSizeInput = Console.ReadLine();
                        if (int.TryParse(newSizeInput, out int newSize) && newSize > 0)
                        {
                            // 1. Collect all parked vehicles
                            var allVehicles = garage.Garage
                                .SelectMany(spot => spot.ParkedVehicles)
                                .ToList();

                            // 2. Update config and create new garage
                            config.GarageSize = newSize;
                            var newGarage = new ParkingGarage(newSize);

                            // 3. Attempt to re-park each vehicle
                            foreach (var vehicle in allVehicles)
                            {
                                if (!newGarage.TryParkVehicle(vehicle, out int spot))
                                {
                                    Console.WriteLine($"Warning: Could not re-park vehicle {vehicle.LicensePlate} (type: {vehicle.GetType().Name}) due to insufficient space.");
                                }
                            }

                            garage = newGarage;
                            ConfigManager.Save(configPath, config);
                            Console.WriteLine($"Garage size updated to {newSize}, vehicles transferred, and config saved to appconfig.json.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Garage size must be a positive integer.");
                        }
                        break;

                    case "7":

                        // Save runtime data and config and exit
                        garage.SaveToFile(dataPath);
                        ConfigManager.Save(configPath, config);
                        Console.WriteLine("Saved data and config. Exiting.");

                        // Show updated config file contents
                        Console.WriteLine("\nUpdated appconfig.json:");
                        if (File.Exists(configPath))
                        {
                            string configContents = File.ReadAllText(configPath);
                            Console.WriteLine(configContents);
                        }
                        else
                        {
                            Console.WriteLine("Config file not found.");
                        }

                        Console.WriteLine("\nUpdated configuration:");
                        ConfigDisplayService.PrintConfig(config);
                        Console.WriteLine("Returning to menu");
                        break;

                    case "8":
                        running = false;
                        Console.WriteLine("Exiting.");
                        break;
                       
                    default:
                        Console.WriteLine("Unknown choice.");
                        break;
                }
            }
        }
    }
}