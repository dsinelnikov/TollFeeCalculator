using System;
using System.Collections.Generic;
using TollFeeCalculator.Core.Interfaces;

namespace TollFeeCalculator.Core.Rules
{
    public class FixedRule : Rule
    {
        private readonly double _tollFee;

        public FixedRule(double tollFee, IEnumerable<IVehicle> handledVehicles = null, IEqualityComparer<IVehicle> vehicleComparer = null)
            : base(handledVehicles, vehicleComparer)
        {
            _tollFee = tollFee;
        }

        protected override double InternalGetTollFee(IVehicle vehicle, DateTime date)
        {
            return _tollFee;
        }
    }
}
