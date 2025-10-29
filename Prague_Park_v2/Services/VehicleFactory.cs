using Prague_Park_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Park_v2.Services
{
    
    public class VehicleFactory
    {
        private readonly AppConfig _config;
        
        public VehicleFactory(AppConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
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
