using System;
using System.Collections.Generic;
using System.Linq;
using TollFeeCalculator.Core.Interfaces;

namespace TollFeeCalculator.Core
{
    public class Calculator : ITollFeeCalculator
    {
        private readonly ITollFeeConfiguration _configuration;

        public Calculator(ITollFeeConfiguration configuration)
        {
            _configuration = configuration;
        }

        public double CalculateTollFee(IVehicle vehicle, IEnumerable<DateTime> dates)
        {
            var orderedDates = dates.OrderBy(d => d)
                .ToList();

            if(orderedDates.Count == 0)
            {
                return 0;
            }

            var rule = _configuration.Rule;
            var tollFeeInterval = _configuration.TollFeeInterval;

            var prevPayedDate = DateTime.MinValue;
            var enterDayDate = DateTime.MinValue;
            var totalTollFee = 0.0;
            var dayTollFee = 0.0;            
            foreach (var date in orderedDates)
            {
                if(enterDayDate.Day != date.Day)
                {
                    enterDayDate = date;
                    totalTollFee += GetClearDayTollFee(dayTollFee);
                    dayTollFee = 0.0;
                }

                var diff = date - prevPayedDate;
                if(diff < tollFeeInterval)
                {
                    continue;
                }

                prevPayedDate = date;
                dayTollFee += rule.GetTollFee(vehicle, date);
            }

            return totalTollFee + GetClearDayTollFee(dayTollFee);
        }

        private double GetClearDayTollFee(double dayTollFee)
        {
            return Math.Min(dayTollFee, _configuration.DayTollFeeMaximum);
        }
    }
}
