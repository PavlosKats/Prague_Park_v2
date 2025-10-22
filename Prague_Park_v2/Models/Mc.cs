using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Park_v2.Models
{
    public class Mc: Vehicle
    {
        public Mc(string? licensePlate, int size = 1, int pricePerHour = 20): base(licensePlate)
        {
            Size = size;
            PricePerHour = pricePerHour;
            ArrivalTime = DateTime.Now;
        }
    }
}
