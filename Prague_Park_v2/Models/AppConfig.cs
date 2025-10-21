

namespace Prague_Park_v2.Models
{
    public class AppConfig
    {
        public int GarageSize { get; set; }
        public List<VehicleTypeConfig> VehicleTypes { get; set; } = new();
        public ParkingSpotConfig ParkingSpot { get; set; } = new();
    }

    public class VehicleTypeConfig
    {
        public string Type { get; set; } = string.Empty;
        public int Size { get; set; }
        public int PricePerHour { get; set; }
    }

    public class ParkingSpotConfig
    {
        public int Size { get; set; }
        public int Height { get; set; }
    }
}
