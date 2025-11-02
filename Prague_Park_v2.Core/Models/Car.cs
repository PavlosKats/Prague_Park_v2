using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Park_v2.Core.Models
{
    public class Car: Vehicle
    {
        public Car(string? licensePlate) : base(licensePlate) { }

        public Car(string? licensePlate, int size, int pricePerHour) : base(licensePlate)
        {
            Size = size;
            PricePerHour = pricePerHour;
            ArrivalTime = DateTime.Now;
        }
    }
}
