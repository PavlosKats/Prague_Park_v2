using Spectre.Console;
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
        public void CheckoutVehicle(DateTime departureTime)
        {
            TimeSpan duration = departureTime - ArrivalTime;
            int totalMinutes = (int)duration.TotalMinutes;
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            double totalHours;
            double totalPrice;
            string freeMessage = "";

            if (totalMinutes <= 10)
            {
                totalHours = 0;
                totalPrice = 0;
                freeMessage = "[green]Parking is free![/] (Duration under 10 minutes)\n";
            }
            else
            {
                // Subtract the free 10 minutes, then round up to the next hour
                int billableMinutes = totalMinutes - 10;
                totalHours = Math.Ceiling(billableMinutes / 60.0);
                totalPrice = totalHours * PricePerHour;
            }

            var panel = new Panel(
                $"[bold yellow]Vehicle Checkout[/]\n" +
                $"[green]License Plate:[/] [white]{LicensePlate}[/]\n" +
                $"[blue]Total Duration:[/] [white]{hours}[/] hours and [white]{minutes}[/] minute(s)\n" +
                freeMessage +
                $"[yellow]Total Price:[/] [white]{totalPrice}[/] currency units"
            )
            .Header("[bold green]Checkout Summary[/]", Justify.Center)
            .Border(BoxBorder.Rounded)
            .Padding(1, 1)
            .Expand();

            AnsiConsole.Write(panel);
        }
    }
}
