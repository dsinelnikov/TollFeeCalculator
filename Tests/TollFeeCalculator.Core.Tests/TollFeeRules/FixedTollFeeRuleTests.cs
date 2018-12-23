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
    public class FixedRuleTests
    {
        [Fact]
        public void TryGetTollFee_Returns_Specified_Toll_Fee()
        {
            var expectedTollFee = 10;
            var vehicle = new Mock<IVehicle>();
            var rule = new FixedRule(expectedTollFee, null);

            var actualTollFee = rule.GetTollFee(vehicle.Object, new DateTime(2018, 12, 22, 0, 30, 0));

            Assert.Equal(expectedTollFee, actualTollFee, 0);
        }

        [Fact]
        public void TryGetTollFee_Handle_Specified_Vehicles()
        {
            var expectedTollFee = 10;
            var vehicle = new Mock<IVehicle>();
            var rule = new FixedRule(expectedTollFee, new[] { vehicle.Object});

            var actualTollFee = rule.GetTollFee(vehicle.Object, new DateTime(2018, 12, 22, 0, 30, 0));

            Assert.Equal(expectedTollFee, actualTollFee, 0);
        }

        [Fact]
        public void TryGetTollFee_Do_Not_Handle_Unspecified_Vehicles()
        {
            var expectedTollFee = 0;
            var handledVehicle = new Mock<IVehicle>();
            var checkedVehicle = new Mock<IVehicle>();
            var rule = new FixedRule(10, new[] { handledVehicle.Object });

            var actualTollFee = rule.GetTollFee(checkedVehicle.Object, new DateTime(2018, 12, 22, 0, 30, 0));

            Assert.Equal(expectedTollFee, actualTollFee, 0);
        }
    }
}
