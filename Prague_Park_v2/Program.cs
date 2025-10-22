
using System;
using System.IO;
using Prague_Park_v2.Models;
using Prague_Park_v2.Services;

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

            // Small console menu: view/edit prices
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1) Show prices");
                Console.WriteLine("2) Change price");
                Console.WriteLine("3) Park a vehicle (demo)");
                Console.WriteLine("4) Remove a vehicle (demo)");
                Console.WriteLine("5) Save config & data and exit");
                Console.Write("Choice: ");
                var choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "1")
                {
                    Console.WriteLine("Configured vehicle prices:");
                    if (config.VehicleTypes != null)
                    {
                        foreach (var vt in config.VehicleTypes)
                        {
                            Console.WriteLine($" - {vt.Type}: Size={vt.Size}, PricePerHour={vt.PricePerHour}");
                        }
                    }
                }
                else if (choice == "2")
                {
                    Console.Write("Vehicle type to edit (e.g. Car): ");
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
                }
                else if (choice == "3")
                {
                    Console.Write("Vehicle type to park (Car/Mc): ");
                    string? type = Console.ReadLine();
                    Console.Write("License plate: ");
                    string? plate = Console.ReadLine();

                    var vehicle = factory.Create(type ?? "Car", plate);
                    if (garage.TryParkVehicle(vehicle, out int spot))
                    {
                        Console.WriteLine($"Parked {vehicle.LicensePlate} (price {vehicle.PricePerHour}/h) at spot {spot}.");
                    }
                    else
                    {
                        Console.WriteLine("No space available for this vehicle.");
                    }
                }
                else if (choice == "4")
                {
                    Console.Write("License plate to remove: ");
                    string? plate = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(plate) && garage.TryRemoveVehicle(plate!, out var removed))
                    {
                        Console.WriteLine($"Removed {removed?.LicensePlate}. Checkout info:");
                        removed?.CheckoutVehicle(DateTime.Now);
                    }
                    else
                    {
                        Console.WriteLine("Vehicle not found.");
                    }
                }
                else if (choice == "5")
                {
                    // Save runtime data and config and exit
                    garage.SaveToFile(dataPath);
                    ConfigManager.Save(configPath, config);
                    Console.WriteLine("Saved data and config. Exiting.");
                    break;
                }
                else
                {
                    Console.WriteLine("Unknown choice.");
                }
            }
        }
    }
}