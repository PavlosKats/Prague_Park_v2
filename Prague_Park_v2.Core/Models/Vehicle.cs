using System.Text.RegularExpressions;

namespace Prague_Park_v2.Core.Models
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

        // Simple license plate validation: 3 letters followed by 3 digits, case insensitive
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

        // Calculate parking duration and price upon checkout
        public CheckoutInfo CalculateCheckout(DateTime departureTime)
        {
            TimeSpan duration = departureTime - ArrivalTime;
            int totalMinutes = (int)duration.TotalMinutes;
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            double totalHours;
            double totalPrice;
            bool isFree = false;

            if (totalMinutes <= 10)
            {
                totalHours = 0;
                totalPrice = 0;
                isFree = true;
            }
            else
            {
                // Subtract the free 10 minutes, then round up to the next hour
                int billableMinutes = totalMinutes - 10;
                totalHours = Math.Ceiling(billableMinutes / 60.0);
                totalPrice = totalHours * PricePerHour;
            }

            return new CheckoutInfo
            {
                LicensePlate = LicensePlate,
                Hours = hours,
                Minutes = minutes,
                TotalPrice = totalPrice,
                IsFree = isFree
            };
        }
    }

    public class CheckoutInfo
    {
        public string? LicensePlate { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public double TotalPrice { get; set; }
        public bool IsFree { get; set; }
    }
}
