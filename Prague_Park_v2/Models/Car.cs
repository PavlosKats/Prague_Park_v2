using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Park_v2.Models
{
    public class Car: Vehicle
    {
        public Car(string? licensePlate) : base(licensePlate)
        {
            Size = 2;
            PricePerHour = 40;
            ArrivalTime = DateTime.Now;
        }
    }
}
