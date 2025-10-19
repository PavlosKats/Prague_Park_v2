using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            LicensePlate = licensePlate;
        }

        public string GetInfo()
        {
            return $"License Plate: {LicensePlate}, Size: {Size}, Arrival Time: {ArrivalTime}, Price Per Hour: {PricePerHour}";
        }

        public void CheckoutVehicle(DateTime departureTime)
        {
            TimeSpan duration = departureTime - ArrivalTime;
            double totalHours = Math.Ceiling(duration.TotalHours);
            double totalPrice = totalHours * PricePerHour;
            Console.WriteLine($"Vehicle with License Plate: {LicensePlate} is checking out.");
            Console.WriteLine($"Total Duration: {totalHours} hours");
            Console.WriteLine($"Total Price: {totalPrice} currency units");
        }
    }
}
