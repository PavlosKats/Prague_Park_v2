

using System.Text.Json;
using Prague_Park_v2.Core.Models;

namespace Prague_Park_v2.Core.Services
{
    public static class ConfigManager
    {
        // Load configuration from JSON file, or create default if not found
        public static AppConfig Load(string path)
        {
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!File.Exists(path))
            {
                var defaultCfg = new AppConfig
                {
                    GarageSize = 100,
                    VehicleTypes = new List<VehicleTypeConfig>
                        {
                            new VehicleTypeConfig { Type = "Car", Size = 4, PricePerHour = 100 },
                            new VehicleTypeConfig { Type = "Mc",  Size = 2, PricePerHour = 50  }
                        },
                    ParkingSpot = new ParkingSpotConfig { Size = 4, Height = 2 }
                };
                try
                {
                    var dir = Path.GetDirectoryName(path);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    Save(path, defaultCfg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating default config file: {ex.Message}");
                }

                return defaultCfg;
            }

            var json = File.ReadAllText(path);
            
            var cfg = JsonSerializer.Deserialize<AppConfig>(json, opts)
                      ?? throw new InvalidOperationException("Failed to deserialize app config");
            return cfg;
        }

        // Save configuration to JSON file
        public static void Save(string path, AppConfig config)
        {
            var opts = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(config, opts);
            File.WriteAllText(path, json);
        }

        public static int? GetPriceForType(AppConfig config, string type)
        {
            var vt = config.VehicleTypes?.FirstOrDefault(v => string.Equals(v.Type, type, StringComparison.OrdinalIgnoreCase));
            return vt?.PricePerHour;
        }

        public static bool SetPriceForType(AppConfig config, string type, int newPrice)
        {
            var vt = config.VehicleTypes?.FirstOrDefault(v => string.Equals(v.Type, type, StringComparison.OrdinalIgnoreCase));
            if (vt == null) return false;
            vt.PricePerHour = newPrice;
            return true;
        }
    }
}