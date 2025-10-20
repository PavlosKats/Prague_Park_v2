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

        public List<Vehicle> ParkedVehicles { get; set; }

        public ParkingSpot( int spotNumber)
        {
            Size = 4; // TODO read from config

            Height = 2; // TODO read from config

            SpotNumber = spotNumber;

            AvailableSize = Size;

            ParkedVehicles = new List<Vehicle>();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            ParkedVehicles.Add(vehicle);
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
