using System.Collections.Generic;
using TollFeeCalculator.Core.Interfaces;

namespace TollFeeCalculator.Infrastructure.Vehicles
{
    public class VehicleEqualityComparer : IEqualityComparer<IVehicle>
    {
        public bool Equals(IVehicle x, IVehicle y)
        {
            var v1 = x as Vehicle;
            if (v1 == null) return false;

            var v2 = y as Vehicle;
            if (v2 == null) return false;

            return v1.VehicleCode == v2.VehicleCode;
        }

        public int GetHashCode(IVehicle obj)
        {
            return (obj as Vehicle)?.VehicleCode ?? int.MinValue;
        }
    }
}
