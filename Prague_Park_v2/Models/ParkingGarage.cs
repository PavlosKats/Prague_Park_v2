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

        public static ParkingGarage LoadFromFile(string filePath, int garageSize)
        {
            if (garageSize <= 0)
            {
                throw new ArgumentException("Garage size must be greater than zero.", nameof(garageSize));
            }

            if (!File.Exists(filePath))
            {
              return new ParkingGarage(garageSize);
            }

            try
            {
                var json = File.ReadAllText(filePath);
                var loaded = JsonSerializer.Deserialize<ParkingGarage>(json);

                if (loaded == null || loaded.Size <= 0)
                {
                    return new ParkingGarage(garageSize);
                }

                // If the persisted Garage list is missing or does not match the persisted Size,
                // recreate spots to match the persisted Size
                if (loaded.Garage == null || loaded.Garage.Count != loaded.Size)
                {
                    loaded.Garage = new List<ParkingSpot>(loaded.Size);
                    for (int i = 0; i < loaded.Size; i++)
                    {
                        loaded.Garage.Add(new ParkingSpot(i + 1));
                    }
                }

                // Ensure each spot has a valid SpotNumber and recompute AvailableSize
                for (int i = 0; i < loaded.Garage.Count; i++)
                {
                    var spot = loaded.Garage[i];
                    spot.SpotNumber = i + 1;

                    var parkedTotal = (spot.ParkedVehicles ?? new List<Vehicle>()).Sum(v => v?.Size ?? 0);
                    // if Size is invalid (e.g. zero), fall back to a sensible default (4)
                    if (spot.Size <= 0)
                    {
                        spot.Size = 4;
                    }
                    spot.AvailableSize = Math.Max(0, spot.Size - parkedTotal);
                }

                return loaded;
            }
            catch (Exception)
            {

                return new ParkingGarage(garageSize);
            }

            
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

            if (Garage == null || Garage.Count == 0) return false;

            var spot = Garage.FirstOrDefault(s => s.CanFitVehicle(vehicle));
            if (spot == null) return false;

            // set arrival time when parking
            vehicle.ArrivalTime = DateTime.Now;

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
