using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Park_v2.Models
{
    public class ParkingSpot
    {
        public int Size { get; set; }

        public int Height { get; set; }

        public int AvailableSize { get; private set; }

        public int SpotNumber { get; set; }

        private readonly List<Vehicle> _parkedVehicles;
        public IReadOnlyList<Vehicle> ParkedVehicles => _parkedVehicles.AsReadOnly();

        public ParkingSpot( int spotNumber)
        {
            Size = 4; // TODO read from config

            Height = 2; // TODO read from config

            SpotNumber = spotNumber;

            AvailableSize = Size;

            _parkedVehicles = new List<Vehicle>();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if(vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (!CanFitVehicle(vehicle))
            {
                throw new InvalidOperationException("Vehicle does not fit in the parking spot.");
            }
            if ( vehicle)
            _parkedVehicles.Add(vehicle);
            AvailableSize -= vehicle.Size;
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            ParkedVehicles.Remove(vehicle);
            AvailableSize += vehicle.Size;
        }

        public bool CanFitVehicle(Vehicle vehicle)
        {
            return AvailableSize >= vehicle.Size;
        }

        public bool CheckLicensePlate(string licensePlate)
        {
            return ParkedVehicles.Any(v => v.LicensePlate == licensePlate);
        }

    }
}
