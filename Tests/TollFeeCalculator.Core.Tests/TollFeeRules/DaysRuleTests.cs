using Moq;
using System;
using TollFeeCalculator.Core.Interfaces;
using TollFeeCalculator.Core.Rules;
using Xunit;

namespace TollFeeCalculator.Core.Tests.Rules
{
    public class DaysRuleTests
    {
        [Fact]
        public void GetTollFee_Handle_Specified_Date()
        {
            var expectedTollFee = 10;
            var vehicle = new Mock<IVehicle>();
            var date = new DateTime(2018, 12, 22, 1, 1, 1);
            var checkDate = date.AddHours(1);
            var rule = new DaysRule(expectedTollFee, new[] { date });

            var actualTollFee = rule.GetTollFee(vehicle.Object, checkDate);

            Assert.Equal(expectedTollFee, actualTollFee, 0);
        }

        [Fact]
        public void GetTollFee_Do_Not_Handle_Unspecified_Date()
        {
            var expectedNextTollFee = 20;
            var handledDate = new DateTime(2018, 12, 22);
            var checkedDate = new DateTime(2018, 12, 19);

            var vehicle = new Mock<IVehicle>();
            var nextRule = new Mock<IRule>();
            nextRule.Setup(r => r.GetTollFee(vehicle.Object, checkedDate))
                .Returns(expectedNextTollFee);

            var rule = new DaysRule(0, new[] { handledDate });
            rule.SetNextRule(nextRule.Object);

            var actualTollFee = rule.GetTollFee(vehicle.Object, checkedDate);

            Assert.Equal(expectedNextTollFee, actualTollFee, 0);
        }
    }
}
