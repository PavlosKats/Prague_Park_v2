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
        public int AvailableSize { get; set; }
        public int SpotNumber { get; set; }

        public List<Vehicle> ParkedVehicles { get; set; } = new();
        public ParkingSpot() { }

        public ParkingSpot( int spotNumber)
        {

            SpotNumber = spotNumber;

            AvailableSize = Size;

            ParkedVehicles = new List<Vehicle>();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if(vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (!CanFitVehicle(vehicle))
            {
                throw new InvalidOperationException("Vehicle does not fit in the parking spot.");
            }
            ParkedVehicles.Add(vehicle);
            AvailableSize -= vehicle.Size;
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return;
            var removed = ParkedVehicles.Remove(vehicle);
            if (removed)
            {
                AvailableSize += vehicle.Size;
            }
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
