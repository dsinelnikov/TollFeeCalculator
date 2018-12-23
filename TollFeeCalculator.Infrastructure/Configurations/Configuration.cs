using System;
using System.Collections.Generic;
using TollFeeCalculator.Core.Interfaces;

namespace TollFeeCalculator.Infrastructure.Configurations
{
    public class Configuration : ITollFeeConfiguration
    {
        public IRule Rule { get; }

        public double DayTollFeeMaximum { get; }

        public TimeSpan TollFeeInterval { get; }

        public Configuration(IRule rule, double dayTollFeeMaximum, TimeSpan tollFeeInterval)
        {
            Rule = rule;
            DayTollFeeMaximum = dayTollFeeMaximum;
            TollFeeInterval = tollFeeInterval;
        }
    }
}
