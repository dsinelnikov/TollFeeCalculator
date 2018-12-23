using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Core.Interfaces
{
    public interface IRule
    {
        double GetTollFee(IVehicle vehicle, DateTime date);
    }
}
