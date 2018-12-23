using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator.Core.Interfaces;

namespace TollFeeCalculator.Infrastructure.Vehicles
{
    public class Vehicle : IVehicle
    {
        public string Name { get; }
        public int VehicleCode { get; }

        public Vehicle(string name, int vehicleCode)
        {
            Name = name;
            VehicleCode = vehicleCode;
        }
    }
}
