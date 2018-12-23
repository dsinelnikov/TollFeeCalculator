using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator.Core.Interfaces;
using TollFeeCalculator.Core.Rules;
using Xunit;

namespace TollFeeCalculator.Core.Tests.Rules
{
    public class WeekDaysRuleTests
    {
        [Fact]
        public void GetTollFee_Handle_Specified_Day()
        {
            var expectedTollFee = 10;
            var vehicle = new Mock<IVehicle>();
            var date = new DateTime(2018, 12, 22);

            var rule = new WeekDaysRule(expectedTollFee, new[] { date.DayOfWeek});

            var actualTollFee = rule.GetTollFee(vehicle.Object, new DateTime(2018, 12, 22));
            
            Assert.Equal(expectedTollFee, actualTollFee, 0);
        }

        [Fact]
        public void GetTollFee_Do_Not_Handle_Unspecified_Day()
        {
            var expectedNextTollFee = 20;
            var handledDayOfWeek = new DateTime(2018, 12, 20).DayOfWeek;
            var chekedDate = new DateTime(2018, 12, 19);

            var vehicle = new Mock<IVehicle>();
            var nextRule = new Mock<IRule>();
            nextRule.Setup(r => r.GetTollFee(vehicle.Object, chekedDate))
                .Returns(expectedNextTollFee);

            var rule = new WeekDaysRule(0, new[] { handledDayOfWeek });
            rule.SetNextRule(nextRule.Object);
            
            var actualTollFee = rule.GetTollFee(vehicle.Object, chekedDate);

            Assert.Equal(expectedNextTollFee, actualTollFee, 0);
        }
    }
}
