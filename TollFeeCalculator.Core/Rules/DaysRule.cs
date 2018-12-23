using System;
using System.Collections.Generic;
using System.Linq;
using TollFeeCalculator.Core.Interfaces;

namespace TollFeeCalculator.Core.Rules
{
    public class DaysRule : Rule
    {
        private readonly double _tollFee;
        private readonly HashSet<DateTime> _handledDates;

        public DaysRule(double tollFee, IEnumerable<DateTime> handledDates, IEnumerable<IVehicle> handledVehicles = null, IEqualityComparer<IVehicle> vehicleComparer = null)
            : base(handledVehicles, vehicleComparer)
        {
            if (tollFee < 0)
            {
                throw new ArgumentException($"{tollFee} is less then 0.");
            }

            if (handledDates == null)
            {
                throw new ArgumentNullException(nameof(handledDates));
            }

            if (!handledDates.Any())
            {
                throw new ArgumentException($"{nameof(handledDates)} should be not empty.");
            }

            _tollFee = tollFee;
            _handledDates = new HashSet<DateTime>(handledDates.Select(d => d.Date));
        }

        protected override double InternalGetTollFee(IVehicle vehicle, DateTime date)
        {
            if (_handledDates.Contains(date.Date))
            {
                return _tollFee;
            }

            return GetNextTollFee(vehicle, date);
        }
    }
}
