using System;
using System.Collections.Generic;
using System.Linq;
using TollFeeCalculator.Core.Interfaces;

namespace TollFeeCalculator.Core.Rules
{
    public class DayRule : Rule
    {
        private SortedList<TimeSpan, double> _tollFeeSchedule = new SortedList<TimeSpan, double>();
        private bool _isConfigured = false;

        public DayRule(IEnumerable<IVehicle> handledVehicles = null, IEqualityComparer<IVehicle> vehicleComparer = null)
            :base(handledVehicles, vehicleComparer)
        {

        }

        public DayRule AddTime(TimeSpan time, double tollFee)
        {
            if (_isConfigured)
            {
                throw new InvalidOperationException("Rule is already configured.");
            }

            if(time.Days >= 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(time)} should be less than 1 day.");
            }

            if(_tollFeeSchedule.Count > 0 && _tollFeeSchedule.Keys.Last() >= time)
            {
                throw new ArgumentException($"{nameof(time)} should be greater than previously added {nameof(time)}.");
            }

            if(tollFee < 0)
            {
                throw new ArgumentException($"{nameof(tollFee)} should be equal or greater then 0.");
            }

            _tollFeeSchedule.Add(time, tollFee);

            return this;
        }

        public DayRule EndConfiguration()
        {
            if(_tollFeeSchedule.Count == 0)
            {
                throw new InvalidOperationException("Cannot end configuration before at least one time is not added.");
            }

            var lastTollFee = _tollFeeSchedule.Values.Last();
            _tollFeeSchedule.Add(new TimeSpan(0, 0, 0), lastTollFee);

            _isConfigured = true;

            return this;
        }

        protected override double InternalGetTollFee(IVehicle vehicle, DateTime date)
        {
            if (!_isConfigured)
            {
                throw new InvalidOperationException($"Call {nameof(EndConfiguration)}() before call {nameof(GetTollFee)}");
            }

            var time = date.TimeOfDay;

            return _tollFeeSchedule.Reverse()
                .SkipWhile(pair => pair.Key > time)
                .First()
                .Value;
        }
    }
}
