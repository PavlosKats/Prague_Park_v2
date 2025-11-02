

namespace Prague_Park_v2.Core.Models
{
    /// <summary>
    /// Represents the application configuration settings, typically deserialized from an appconfig.json file.
    /// </summary>
    /// <remarks>This class provides configuration options for the application, including garage size, vehicle
    /// type settings,  and parking spot configurations. It is designed to be used as a model for deserializing
    /// structured configuration data.</remarks>
    public class AppConfig
    {
        public int GarageSize { get; set; }
        public List<VehicleTypeConfig>? VehicleTypes { get; set; }
        public ParkingSpotConfig? ParkingSpot { get; set; }
    }

    public class VehicleTypeConfig
    {
        public string? Type { get; set; } 
        public int Size { get; set; }
        public int PricePerHour { get; set; }
    }

    public class ParkingSpotConfig
    {
        public int Size { get; set; }
        public int Height { get; set; }
    }
}
