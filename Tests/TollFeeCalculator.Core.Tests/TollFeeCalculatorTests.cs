using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator.Core.Interfaces;
using Xunit;

namespace TollFeeCalculator.Core.Tests
{
    public class TollFeeCalculatorTests
    {
        [Fact]
        public void CalculateTollFee_Empty_Dates()
        {
            var rule = new Mock<IRule>();
            var vehicle = new Mock<IVehicle>();
            var tollFeeInterval = TimeSpan.FromHours(1);
            var configuration = CreateConfigurationMock(rule.Object, tollFeeInterval, int.MaxValue);           

            var tollFeeCalculator = new Calculator(configuration.Object);
            var tollFee = tollFeeCalculator.CalculateTollFee(vehicle.Object, new DateTime[] { });

            Assert.Equal(0.0, tollFee, 0);
        }

        [Fact]
        public void CalculateTollFee_One_Date()
        {
            var expectedTollFee = 10;
            var vehicle = new Mock<IVehicle>();
            var tollFeeInterval = TimeSpan.FromHours(1);
            var checkDate = new DateTime(2018, 12, 22, 0, 30, 0);
            var rule = new Mock<IRule>();
            rule.Setup(r => r.GetTollFee(vehicle.Object, It.IsAny<DateTime>()))
                .Returns(expectedTollFee);
            var configuration = CreateConfigurationMock(rule.Object, tollFeeInterval, int.MaxValue);
            var tollFeeCalculator = new Calculator(configuration.Object);            

            var tollFee = tollFeeCalculator.CalculateTollFee(vehicle.Object, new [] { checkDate });

            Assert.Equal(expectedTollFee, tollFee, 0);
        }

        [Fact]
        public void CalculateTollFee_Two_Trips_In_One_TollFeeInterval()
        {
            var expectedTollFee = 10;
            var vehicle = new Mock<IVehicle>();
            var tollFeeInterval = TimeSpan.FromHours(1);
            var checkDate = new DateTime(2018, 12, 22, 0, 30, 0);
            var checkDates = new[]
            {
                checkDate,
                checkDate + TimeSpan.FromMinutes(10)
            };

            var rule = new Mock<IRule>();            
            rule.Setup(r => r.GetTollFee(vehicle.Object, It.IsAny<DateTime>()))
                .Returns(expectedTollFee);
            var configuration = CreateConfigurationMock(rule.Object, tollFeeInterval, int.MaxValue);
            var tollFeeCalculator = new Calculator(configuration.Object);

            var tollFee = tollFeeCalculator.CalculateTollFee(vehicle.Object, checkDates);

            Assert.Equal(expectedTollFee, tollFee, 0);
        }

        [Fact]
        public void CalculateTollFee_Three_Trips_In_Two_TollFeeInterval()
        {
            var ruleTollFee = 10;
            var expectedTollFee = ruleTollFee * 2;
            var vehicle = new Mock<IVehicle>();
            var tollFeeInterval = TimeSpan.FromHours(1);
            var checkDate = new DateTime(2018, 12, 22, 0, 30, 0);
            var checkDates = new[]
            {
                checkDate,               
                checkDate + TimeSpan.FromMinutes(10),
                checkDate + tollFeeInterval
            };

            var rule = new Mock<IRule>();
            rule.Setup(r => r.GetTollFee(vehicle.Object, It.IsAny<DateTime>()))
                .Returns(ruleTollFee);
            var configuration = CreateConfigurationMock(rule.Object, tollFeeInterval, int.MaxValue);
            var tollFeeCalculator = new Calculator(configuration.Object);

            var tollFee = tollFeeCalculator.CalculateTollFee(vehicle.Object, checkDates);

            Assert.Equal(expectedTollFee, tollFee, 0);
        }

        [Fact]
        public void CalculateTollFee_TotalTollFee_Greate_Then_MaximumDayTollFee_Should_Return_MaximumDayTollFee()
        {
            var ruleTollFee = 10;
            var maximumDayTollFee = 15;            
            var vehicle = new Mock<IVehicle>();
            var tollFeeInterval = TimeSpan.FromHours(1);
            var checkDate = new DateTime(2018, 12, 22, 0, 30, 0);
            var checkDates = new[]
            {
                checkDate,
                checkDate + tollFeeInterval
            };

            var rule = new Mock<IRule>();
            rule.Setup(r => r.GetTollFee(vehicle.Object, It.IsAny<DateTime>()))
                .Returns(ruleTollFee);
            var configuration = CreateConfigurationMock(rule.Object, tollFeeInterval, maximumDayTollFee);
            var tollFeeCalculator = new Calculator(configuration.Object);

            var tollFee = tollFeeCalculator.CalculateTollFee(vehicle.Object, checkDates);

            Assert.Equal(maximumDayTollFee, tollFee, 0);
        }

        [Fact]
        public void CalculateTollFee_FirstDay_And_SecondDay_TotalTollFee_Greate_Then_MaximumDayTollFee_Should_Use_MaximumDayTollFee_For_Each_Day()
        {
            var ruleTollFee = 10;
            var maximumDayTollFee = 15;
            var expectedTollFee = maximumDayTollFee * 2;
            var vehicle = new Mock<IVehicle>();
            var tollFeeInterval = TimeSpan.FromHours(1);
            var checkDate = new DateTime(2018, 12, 22, 0, 30, 0);
            var checkDates = new[]
            {
                checkDate,
                checkDate + tollFeeInterval,
                checkDate + TimeSpan.FromDays(1),
                checkDate + TimeSpan.FromDays(1) + tollFeeInterval
            };

            var rule = new Mock<IRule>();
            rule.Setup(r => r.GetTollFee(vehicle.Object, It.IsAny<DateTime>()))
                .Returns(ruleTollFee);
            var configuration = CreateConfigurationMock(rule.Object, tollFeeInterval, maximumDayTollFee);
            var tollFeeCalculator = new Calculator(configuration.Object);

            var tollFee = tollFeeCalculator.CalculateTollFee(vehicle.Object, checkDates);

            Assert.Equal(expectedTollFee, tollFee, 0);
        }

        private static IMock<ITollFeeConfiguration> CreateConfigurationMock(IRule rule, TimeSpan tollFeeInterval, double dayTollFeeMaximum)
        {
            var configuration = new Mock<ITollFeeConfiguration>();
            configuration.SetupGet(c => c.Rule)
                .Returns(rule);
            configuration.SetupGet(c => c.TollFeeInterval)
                .Returns(tollFeeInterval);
            configuration.SetupGet(c => c.DayTollFeeMaximum)
                .Returns(dayTollFeeMaximum);

            return configuration;
        }
    }
}
