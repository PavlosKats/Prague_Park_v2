using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Park_v2.Models
{
    public class Car: Vehicle
    {
        public Car(string? licensePlate, int size = 2, int pricePerHour = 40) : base(licensePlate)
        {
            Size = size;
            PricePerHour = pricePerHour;
            ArrivalTime = DateTime.Now;
        }
    }
}
