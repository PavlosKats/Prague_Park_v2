using Prague_Park_v2.Core.Models;


namespace Prague_Park_v2.Core.Services
{
    /// <summary>
    /// Provides functionality to create vehicle instances based on a specified type.
    /// </summary>
    /// <remarks>The <see cref="VehicleFactory"/> class uses configuration data to determine the supported
    /// vehicle types and their associated properties. It supports dependency injection of an <see cref="AppConfig"/>
    /// instance to retrieve the configuration. This factory ensures that only supported vehicle types can be created,
    /// and it initializes the vehicle's properties such as size, price per hour, and arrival time.</remarks>
    public class VehicleFactory
    {
        // Dependency injection of configuration
        private readonly AppConfig _config;

        // Constructor
        public VehicleFactory(AppConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        // Create vehicle based on type
        public Vehicle Create(string type, string? licensePlate)
        {
            if ( string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Vehicle type must be provided", nameof(type));

            var vt = _config.VehicleTypes?.FirstOrDefault(v => string.Equals(v.Type, type, StringComparison.OrdinalIgnoreCase));

            if (vt == null)
                throw new InvalidOperationException($"Unsupported vehicle type: {type}");

            Vehicle vehicle = vt.Type.ToLowerInvariant() switch
            {
                "car" => new Car(licensePlate),
                "mc" => new Mc(licensePlate),
                // Add more types here 
                _ => throw new InvalidOperationException($"Unsupported vehicle type: {type}")
            };


            vehicle.Size = vt.Size;
            vehicle.PricePerHour = vt.PricePerHour;
            vehicle.ArrivalTime = DateTime.Now;
            return vehicle;
        }

    }
}
