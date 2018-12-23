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
    public class DayRuleTests
    {
        [Fact]
        public void AddTime_Time_Greater_Then_One_Day_Should_Throw_ArgumentOutOfRangeException()
        {            
            var rule = new DayRule();

            Assert.Throws<ArgumentOutOfRangeException>(() => rule.AddTime(new TimeSpan(1, 0, 0, 1), 0));
        }

        [Fact]
        public void AddTime_Time_Less_Then_Previous_Time_Should_Throw_ArgumentException()
        {
            var rule = new DayRule()
                .AddTime(new TimeSpan(0, 2, 0, 0), 0);

            Assert.Throws<ArgumentException>(() => rule.AddTime(new TimeSpan(0, 1, 0, 0), 0));
        }

        [Fact]
        public void AddTime_Configuration_Is_Ended_Should_Throw_InvalidOperationException()
        {
            var rule = new DayRule()
                .AddTime(new TimeSpan(0, 1, 0, 0), 0)
                .EndConfiguration();

            Assert.Throws<InvalidOperationException>(() => rule.AddTime(new TimeSpan(0, 1, 0, 0), 0));
        }

        [Fact]
        public void AddTime_Add_Times_In_Ascending_Order_Should_Not_Throw_Exception()
        {
            var rule = new DayRule()
                .AddTime(new TimeSpan(1, 0, 0), 0)
                .AddTime(new TimeSpan(2, 0, 0), 0)
                .AddTime(new TimeSpan(3, 0, 0), 0)
                .AddTime(new TimeSpan(4, 0, 0), 0);
        }

        [Fact]
        public void AddTime_TollFee_Less_Then_0_Should_Throw_ArgumentException()
        {
            var rule = new DayRule();

            Assert.Throws<ArgumentException>(() => rule.AddTime(new TimeSpan(1, 0, 0), -1));
        }

        [Fact]
        public void AddTime_TollFee_Equals_0_Should_Not_Throw_Exception()
        {
            var rule = new DayRule()
                .AddTime(new TimeSpan(1, 0, 0), 0);
        }

        [Fact]
        public void EndConfiguration_Ended_Empty_Rule_Should_Throw_InvalidOperationException()
        {
            var rule = new DayRule();

            Assert.Throws<InvalidOperationException>(() => rule.EndConfiguration());
        }

        [Fact]
        public void GetTollFee_Call_Before_Calling_EndConfiguration_Throws_InvalidOperationException()
        {
            var vehicle = new Mock<IVehicle>();

            var rule = new DayRule()
                .AddTime(new TimeSpan(1, 0, 0), 0);
            
            Assert.Throws<InvalidOperationException>(() => rule.GetTollFee(vehicle.Object, new DateTime(2018, 12, 22)));
        }

        [Fact]
        public void GetTollFee_Time_Less_Then_First_Added_Time()
        {
            var expectedTollFee = 10;
            var vehicle = new Mock<IVehicle>();

            var rule = new DayRule()
                .AddTime(new TimeSpan(1, 0, 0), 5)
                .AddTime(new TimeSpan(3, 0, 0), expectedTollFee)
                .EndConfiguration();

            var tollFee = rule.GetTollFee(vehicle.Object, new DateTime(2018, 12, 22, 0, 30, 0));

            Assert.Equal(expectedTollFee, tollFee);
        }

        [Fact]
        public void GetTollFee_Time_Greater_Then_Last_Added_Time()
        {
            var expectedTollFee = 10;
            var vehicle = new Mock<IVehicle>();

            var rule = new DayRule()
                .AddTime(new TimeSpan(1, 0, 0), 5)
                .AddTime(new TimeSpan(3, 0, 0), expectedTollFee)
                .EndConfiguration();

            
            var tollFee = rule.GetTollFee(vehicle.Object, new DateTime(2018, 12, 22, 5, 30, 0));

            Assert.Equal(expectedTollFee, tollFee);
        }

        [Fact]
        public void GetTollFee_Time_Less_Then_Last_Added_Time()
        {
            var expectedTollFee = 5;
            var vehicle = new Mock<IVehicle>();

            var rule = new DayRule()
                .AddTime(new TimeSpan(1, 0, 0), expectedTollFee)
                .AddTime(new TimeSpan(3, 0, 0), 10)
                .EndConfiguration();

            var tollFee = rule.GetTollFee(vehicle.Object, new DateTime(2018, 12, 22, 2, 30, 0));

            Assert.Equal(expectedTollFee, tollFee);
        }
    }
}
