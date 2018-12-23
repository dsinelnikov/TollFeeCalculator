using Moq;
using System;
using TollFeeCalculator.Core.Interfaces;
using TollFeeCalculator.Core.Rules;
using Xunit;

namespace TollFeeCalculator.Core.Tests.Rules
{
    public class MonthRuleTests
    {
        [Fact]
        public void GetTollFee_Handle_Date_With_Same_Month()
        {
            var expectedTollFee = 10;
            var monthNumber = 5;
            var vehicle = new Mock<IVehicle>();
            var date = new DateTime(2018, monthNumber, 22, 1, 1, 1);
            var checkDate = date.AddHours(1);
            var rule = new MonthRule(expectedTollFee, monthNumber);

            var actualTollFee = rule.GetTollFee(vehicle.Object, checkDate);

            Assert.Equal(expectedTollFee, actualTollFee, 0);
        }

        [Fact]
        public void GetTollFee_Do_Not_Handle_Date_With_Other_Month()
        {
            var expectedNextTollFee = 20;
            var monthNumber = 5;
            var handledDate = new DateTime(2018, monthNumber, 22);
            var checkedDate = handledDate.AddMonths(1);

            var vehicle = new Mock<IVehicle>();
            var nextRule = new Mock<IRule>();
            nextRule.Setup(r => r.GetTollFee(vehicle.Object, checkedDate))
                .Returns(expectedNextTollFee);

            var rule = new MonthRule(0, monthNumber);
            rule.SetNextRule(nextRule.Object);

            var actualTollFee = rule.GetTollFee(vehicle.Object, checkedDate);

            Assert.Equal(expectedNextTollFee, actualTollFee, 0);
        }
    }
}
