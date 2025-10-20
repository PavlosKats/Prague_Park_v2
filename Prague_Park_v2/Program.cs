using Prague_Park_v2.Models;

namespace Prague_Park_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var garage = new ParkingGarage(size:10);

            var car = new Car("ABC123");
            Console.WriteLine($"Attempting to park {car.LicensePlate} size {car.Size}");

            if (garage.TryParkVehicle(car, out int spotNumber))
            {
                Console.WriteLine($"Parked {car.LicensePlate} at spot {spotNumber}");
            }
            else
            {
                Console.WriteLine($"Failed to park {car.LicensePlate}");
            }
        }
    }
}
