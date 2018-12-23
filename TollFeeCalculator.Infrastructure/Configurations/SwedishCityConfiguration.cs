using System;
using System.Collections.Generic;
using TollFeeCalculator.Core.Interfaces;
using TollFeeCalculator.Core.Rules;
using TollFeeCalculator.Infrastructure.Vehicles;
using System.Linq;

namespace TollFeeCalculator.Infrastructure.Configurations
{
    public class SwedishCityConfiguration : Configuration
    {
        private enum VehicleType
        {
            Motorbike = 1000,
            Tractor = 1001,
            Emergency = 1002,
            Diplomat = 1003,
            Foreign = 1004,
            Military = 1005,
            Tourist = 1006,
            School = 1007
        }

        private static readonly IDictionary<VehicleType, IVehicle> _vehicles
            = Enum.GetValues(typeof(VehicleType))
                  .OfType<VehicleType>()
                  .ToDictionary(v => v, v => new Vehicle(v.ToString(), (int)v) as IVehicle);

        public SwedishCityConfiguration()
            :base(CreateRule(), 60.0, TimeSpan.FromHours(1))
        {

        }

        private static IRule CreateRule()
        {
            var vehicleEqualityComparer = new VehicleEqualityComparer();

            var rootRule = new WeekDaysRule(0.0, new[] { DayOfWeek.Saturday, DayOfWeek.Sunday });
            rootRule
                .AddRule(new FixedRule(0.0, new[] {
                    _vehicles[VehicleType.Motorbike],
                    _vehicles[VehicleType.Tractor],
                    _vehicles[VehicleType.Emergency],
                    _vehicles[VehicleType.Diplomat],
                    _vehicles[VehicleType.Foreign],
                    _vehicles[VehicleType.Military],
                }, vehicleEqualityComparer))
                .AddRule(new DaysRule(0.0, new[] {
                    new DateTime(2013, 1, 1),
                    new DateTime(2013, 3, 28),
                    new DateTime(2013, 3, 29),
                    new DateTime(2013, 4, 1),
                    new DateTime(2013, 4, 30),
                    new DateTime(2013, 5, 1),
                    new DateTime(2013, 5, 8),
                    new DateTime(2013, 5, 9),
                    new DateTime(2013, 6, 5),
                    new DateTime(2013, 6, 6),
                    new DateTime(2013, 6, 21),
                    new DateTime(2013, 11, 1),
                    new DateTime(2013, 12, 24),
                    new DateTime(2013, 12, 25),
                    new DateTime(2013, 12, 26),
                    new DateTime(2013, 12, 31),
                }))
                .AddRule(new MonthRule(0.0, 7))
                .AddRule(new DayRule()
                            .AddTime(new TimeSpan(6, 0, 0), 9)
                            .AddTime(new TimeSpan(6, 30, 0), 16)
                            .AddTime(new TimeSpan(7, 0, 0), 22)
                            .AddTime(new TimeSpan(8, 0, 0), 16)
                            .AddTime(new TimeSpan(8, 30, 0), 9)
                            .AddTime(new TimeSpan(15, 0, 0), 16)
                            .AddTime(new TimeSpan(15, 30, 0), 22)
                            .AddTime(new TimeSpan(17, 0, 0), 16)
                            .AddTime(new TimeSpan(18, 0, 0), 9)
                            .AddTime(new TimeSpan(18, 30, 0), 0)
                            .EndConfiguration());

            return rootRule;
        }
    }
}