using System;
using System.Collections.Generic;
using TollFeeCalculator.Core.Interfaces;

namespace TollFeeCalculator.Core.Rules
{
    public abstract class Rule : IRule
    {
        private IRule _nextRule;
        private HashSet<IVehicle> _handledVehicles;

        public Rule(IEnumerable<IVehicle> handledVehicles, IEqualityComparer<IVehicle> vehicleComparer)
        {
            if (handledVehicles != null)
            {
                _handledVehicles = new HashSet<IVehicle>(handledVehicles, vehicleComparer);
            }
            else
            {
                _handledVehicles = new HashSet<IVehicle>(vehicleComparer);
            }
        }

        public double GetTollFee(IVehicle vehicle, DateTime date)
        {
            if (_handledVehicles.Count != 0 && !_handledVehicles.Contains(vehicle))
            {
                return GetNextTollFee(vehicle, date);
            }

            return InternalGetTollFee(vehicle, date);
        }

        internal void SetNextRule(IRule nextRule)
        {
            _nextRule = nextRule;
        }

        protected abstract double InternalGetTollFee(IVehicle vehicle, DateTime date);

        protected double GetNextTollFee(IVehicle vehicle, DateTime date)
        {
            return _nextRule?.GetTollFee(vehicle, date) ?? 0;
        }
    }
}
