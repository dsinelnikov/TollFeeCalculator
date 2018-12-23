using System;
using System.Collections.Generic;
using TollFeeCalculator.Core.Interfaces;
using System.Linq;

namespace TollFeeCalculator.Core.Rules
{
    public class WeekDaysRule : Rule
    {
        private readonly double _tollFee;
        private readonly HashSet<DayOfWeek> _days;

        public WeekDaysRule(double tollFee, IEnumerable<DayOfWeek> days, IEnumerable<IVehicle> handledVehicles = null, IEqualityComparer<IVehicle> vehicleComparer = null)
            : base(handledVehicles, vehicleComparer)
        {
            if(tollFee < 0)
            {
                throw new ArgumentException($"{tollFee} is less then 0.");
            }

            if(days == null)
            {
                throw new ArgumentNullException(nameof(days));
            }

            if (!days.Any())
            {
                throw new ArgumentException($"{nameof(days)} should be not empty.");
            }

            _tollFee = tollFee;
            _days = new HashSet<DayOfWeek>(days);
        }

        protected override double InternalGetTollFee(IVehicle vehicle, DateTime date)
        {
            if (_days.Contains(date.DayOfWeek))
            {
                return _tollFee;
            }
             
            return GetNextTollFee(vehicle, date); 
        }
    }
}
