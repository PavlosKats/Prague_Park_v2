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
    }
}
