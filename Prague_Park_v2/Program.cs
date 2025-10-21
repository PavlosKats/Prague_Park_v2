using Prague_Park_v2.Models;

namespace Prague_Park_v2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dataFile = "parking_garage_data.json";
            var garage = ParkingGarage.LoadFromFile(dataFile);


            //
            //
            //App logic here
            //
            //



            // Save the garage state before exiting
            garage.SaveToFile(dataFile);
        }
    }
}
