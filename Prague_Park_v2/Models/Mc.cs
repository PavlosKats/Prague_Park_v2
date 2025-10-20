using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Park_v2.Models
{
    internal class Mc: Vehicle
    {
        public Mc(string? licensePlate): base(licensePlate)
        {
            Size = 1;
            PricePerHour = 20;
            ArrivalTime = DateTime.Now;
        }
    }
}
