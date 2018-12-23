using Moq;
using System;
using TollFeeCalculator.Core;
using TollFeeCalculator.Core.Interfaces;
using TollFeeCalculator.Infrastructure.Configurations;
using TollFeeCalculator.Infrastructure.Vehicles;
using Xunit;

namespace TollFeeCalculator.Infrastructure.Tests.Rules
{
    public class SwedishCityConfigurationTests
    {
        [Theory]
        [InlineData("2018-12-22")]
        [InlineData("2018-12-23")]
        public void GetTollFee_Weekend_Is_Free(string dateStr)
        {
            var checkDate = DateTime.Parse(dateStr);
            var vehicle = new Mock<IVehicle>();
            var configuration = new SwedishCityConfiguration();

            var tollFee = configuration.Rule.GetTollFee(vehicle.Object, checkDate);

            Assert.Equal(0.0, tollFee, 0);
        }

        [Theory]
        [InlineData("2018-12-21 06:01", 9)]
        [InlineData("2018-12-21 06:31", 16)]
        [InlineData("2018-12-21 07:01", 22)]
        [InlineData("2018-12-21 08:01", 16)]
        [InlineData("2018-12-21 08:31", 9)]
        [InlineData("2018-12-21 15:01", 16)]
        [InlineData("2018-12-21 15:31", 22)]
        [InlineData("2018-12-21 17:01", 16)]
        [InlineData("2018-12-21 18:01", 9)]
        [InlineData("2018-12-21 18:31", 0)]
        public void GetTollFee_Day_Ranges_TollFee(string dateStr, double expectedTollFee)
        {
            var checkDate = DateTime.Parse(dateStr);
            var vehicle = new Mock<IVehicle>();            
            var configuration = new SwedishCityConfiguration();

            var tollFee = configuration.Rule.GetTollFee(vehicle.Object, checkDate);

            Assert.Equal(expectedTollFee, tollFee, 0);
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(1001)]
        [InlineData(1002)]
        [InlineData(1003)]
        [InlineData(1004)]
        [InlineData(1005)]
        public void GetTollFee_Toll_Free_Vehicles_TollFee_Should_Be_0(int vehicleCode)
        {
            double expectedTollFee = 0;
            var checkDate = new DateTime(2018, 12, 21, 18, 0, 0); // Friday
            var vehicle = new Vehicle(string.Empty, vehicleCode);
            var configuration = new SwedishCityConfiguration();

            var tollFee = configuration.Rule.GetTollFee(vehicle, checkDate);

            Assert.Equal(expectedTollFee, tollFee, 0);
        }

        [Theory]
        [InlineData("2013-1-1 15:00")]
        [InlineData("2013-3-28 15:00")]
        [InlineData("2013-3-29 15:00")]
        [InlineData("2013-4-1 15:00")]
        [InlineData("2013-4-30 15:00")]
        [InlineData("2013-5-1 15:00")]
        [InlineData("2013-5-8 15:00")]
        [InlineData("2013-5-9 15:00")]
        [InlineData("2013-6-5 15:00")]
        [InlineData("2013-6-6 15:00")]
        [InlineData("2013-6-21 15:00")]
        [InlineData("2013-11-1 15:00")]
        [InlineData("2013-12-24 15:00")]
        [InlineData("2013-12-25 15:00")]
        [InlineData("2013-12-26 15:00")]
        [InlineData("2013-12-31 15:00")]
        public void GetTollFee_Holliday_Days_TollFee_Should_Be_0(string dateStr)
        {
            var expectedTollFee = 0.0;
            var checkDate = DateTime.Parse(dateStr);
            var vehicle = new Mock<IVehicle>();
            var configuration = new SwedishCityConfiguration();

            var tollFee = configuration.Rule.GetTollFee(vehicle.Object, checkDate);

            Assert.Equal(expectedTollFee, tollFee, 0);
        }

        [Fact]
        public void GetTollFee_Holliday_Month_TollFee_Should_Be_0()
        {
            var expectedTollFee = 0.0;
            var monthNumber = 7;
            var checkDate = new DateTime(2013, monthNumber, 1, 15, 0, 0); // Monday
            var vehicle = new Mock<IVehicle>();
            var configuration = new SwedishCityConfiguration();

            var tollFee = configuration.Rule.GetTollFee(vehicle.Object, checkDate);

            Assert.Equal(expectedTollFee, tollFee, 0);
        }

        [Fact]
        public void DayTollFeeMaximum_Is_60()
        {
            var configuration = new SwedishCityConfiguration();

            Assert.Equal(60, configuration.DayTollFeeMaximum);
        }

        [Fact]
        public void TollFeeInterval_Is_60()
        {
            var configuration = new SwedishCityConfiguration();

            Assert.Equal(TimeSpan.FromHours(1), configuration.TollFeeInterval);
        }
    }
}