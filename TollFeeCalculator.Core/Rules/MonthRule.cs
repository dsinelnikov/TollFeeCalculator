using System;
using System.Collections.Generic;
using TollFeeCalculator.Core.Interfaces;

namespace TollFeeCalculator.Core.Rules
{
    public class MonthRule : Rule
    {
        private readonly double _tollFee;
        private readonly int _monthNumber;        

        public MonthRule(double tollFee, int monthNumber, IEnumerable<IVehicle> handledVehicles = null, IEqualityComparer<IVehicle> vehicleComparer = null)
            : base(handledVehicles, vehicleComparer)
        {
            _tollFee = tollFee;
            _monthNumber = monthNumber;
        }

        protected override double InternalGetTollFee(IVehicle vehicle, DateTime date)
        {
            if (date.Month == _monthNumber)
            {
                return _tollFee;
            }

            return GetNextTollFee(vehicle, date);
        }
    }
}
