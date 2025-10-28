using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Prague_Park_v2.Models
{
    public class Vehicle
    {
        public string? LicensePlate { get; set; }

        public int Size { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int PricePerHour { get; set; }

        public Vehicle(string? licensePlate)
        {
            if(!IsValidLicensePlate(licensePlate))
            {
                throw new ArgumentException("Invalid license plate format.",nameof(licensePlate));
            }
            LicensePlate = licensePlate;
        }

        private static bool IsValidLicensePlate(string? plate)
        {
            if (string.IsNullOrWhiteSpace(plate))
                return false;
            var pattern = @"^[A-Za-zÅÄÖØÆåäöøæ]{3}[0-9]{3}$";
            return Regex.IsMatch(plate, pattern, RegexOptions.IgnoreCase);
        }

        public string GetInfo()
        {
            return $"License Plate: {LicensePlate}, Size: {Size}, Arrival Time: {ArrivalTime}, Price Per Hour: {PricePerHour}";
        }

        public void CheckoutVehicle(DateTime departureTime)
        {
            TimeSpan duration = departureTime - ArrivalTime;
            double totalHours = Math.Ceiling(duration.TotalHours);
            double totalMinutes = Math.Ceiling(duration.TotalMinutes);
            double totalPrice = totalHours * PricePerHour;
            Console.WriteLine($"Vehicle with License Plate: {LicensePlate} is checking out.");
            Console.WriteLine($"Total Duration: {totalHours} hours and {totalMinutes} minute/s");
            Console.WriteLine($"Total Price: {totalPrice} currency units");
        }
    }
}
