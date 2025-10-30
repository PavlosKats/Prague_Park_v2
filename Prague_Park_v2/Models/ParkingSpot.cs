using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Park_v2.Models
{
    /// <summary>
    /// Represents a parking spot that can accommodate vehicles based on size and height constraints.
    /// </summary>
    /// <remarks>A parking spot tracks its size, height, and the vehicles currently parked in it.  It provides
    /// functionality to add or remove vehicles, check if a vehicle can fit,  and verify if a vehicle with a specific
    /// license plate is parked in the spot.</remarks>
    public class ParkingSpot
    {
        public int Size { get; set; }
        public int Height { get; set; }
        public int AvailableSize { get; set; }
        public int SpotNumber { get; set; }

        // List of vehicles currently parked in this spot
        public List<Vehicle> ParkedVehicles { get; set; } = new();
        public ParkingSpot() { }

        // Constructor to initialize parking spot with a specific spot number
        public ParkingSpot( int spotNumber)
        {

            SpotNumber = spotNumber;

            AvailableSize = Size;
            
            ParkedVehicles = new List<Vehicle>();
        }

        // Adds a vehicle to the parking spot if it fits
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
