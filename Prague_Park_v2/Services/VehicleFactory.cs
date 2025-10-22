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

            if ( vt == null)
            {
                return type.ToLowerInvariant() switch
                {
                    "car" => new Car(licensePlate),
                    "mc" => new Mc(licensePlate),
                    _ => throw new InvalidOperationException($"Unsupported vehicle type: {type}")
                };
            };

            return vt.Type.Equals("Car", StringComparison.OrdinalIgnoreCase) 
                ?new Car(licensePlate, vt.Size, vt.PricePerHour)
                : vt.Type.Equals("Mc", StringComparison.OrdinalIgnoreCase)
                    ? new Mc(licensePlate, vt.Size, vt.PricePerHour)
                    : new Vehicle(licensePlate) { Size = vt.Size, PricePerHour = vt.PricePerHour, ArrivalTime = DateTime.Now };

        }

    }
}
