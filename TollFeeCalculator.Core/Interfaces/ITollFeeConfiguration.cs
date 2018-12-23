using System;
using System.Collections.Generic;

namespace TollFeeCalculator.Core.Interfaces
{
    public interface ITollFeeConfiguration
    {
        IRule Rule { get; }

        double DayTollFeeMaximum { get; }

        TimeSpan TollFeeInterval { get; }
    }
}
