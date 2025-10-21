using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace Prague_Park_v2.Models
{
    public class ParkingGarage

    {

        public List<ParkingSpot> Garage { get; set; } = new();

        public void SaveToFile(string filePath)
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static ParkingGarage LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
              return new ParkingGarage();
            }
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<ParkingGarage>(json) ?? new ParkingGarage();
        }

        public int Size { get; set; }

        public ParkingGarage() { }

        public ParkingGarage(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentException("Garage size must be greater than zero.");
            }
            Size = size;

            Garage = new List<ParkingSpot>(size);
            for (int i = 0; i < Size; i++)
            {
                Garage.Add(new ParkingSpot(i + 1));
            }
        }

        public bool TryParkVehicle(Vehicle vehicle, out int spotNumber)
        {
            spotNumber = -1;
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }
            var spot = Garage.FirstOrDefault(s => s.CanFitVehicle(vehicle));
            if (spot == null) return false;

            if (!spot.CanFitVehicle(vehicle)) return false;

            spot.AddVehicle(vehicle);
            spotNumber = spot.SpotNumber;
            return true;
        }

        // try to remove vehicle by license plate
        public bool TryRemoveVehicle(string licensePlate, out Vehicle? removed)
        {
            removed = null;
            if (string.IsNullOrEmpty(licensePlate)) return false;

            var spot = Garage.FirstOrDefault(s => s.CheckLicensePlate(licensePlate));
            if (spot == null) return false;
            removed = spot.ParkedVehicles.FirstOrDefault(v => v.LicensePlate == licensePlate);
            if (removed == null) return false;
            spot.RemoveVehicle(removed);
            return true;
        }

        //utility: number of spots with at least one free size unit
        public int SpotsWithFreeSpace()
        {
            return Garage.Count(s => s.AvailableSize > 0);
        }

        public override string ToString()
        {
            return $"Parking Garage: Size = {Size}, Spots with Free Space = {SpotsWithFreeSpace()}";
        }
    }
}
