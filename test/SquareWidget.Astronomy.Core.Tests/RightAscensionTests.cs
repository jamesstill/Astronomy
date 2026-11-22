using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class RightAscensionTests
    {
        [Fact]
        public void Test_RA_To_DD_1()
        {
            RightAscension ra = new RightAscension(20, 21, 7.2);
            Degrees d = ra.ToDegrees();
            double dd = ra.ToDecimalDegrees();

            Assert.Equal(d, 305.2800, 4);
            Assert.Equal(dd, 305.2800, 4);
        }

        [Fact]
        public void Test_RA_To_DD_2()
        {
            RightAscension ra = new RightAscension(7, 45, 18.946);
            Degrees d = ra.ToDegrees();
            double dd = ra.ToDecimalDegrees();

            Assert.Equal(116.328942, d, 6);
            Assert.Equal(116.328942, dd, 6);
        }

        [Fact]
        public void Test_DD_To_RA_1()
        {
            Degrees d = new Degrees(305.2800);
            RightAscension ra = new RightAscension(d);

            Assert.Equal(20, ra.Hours);
            Assert.Equal(21, ra.Minutes);
            Assert.Equal(7.2, ra.Seconds, 6);
        }

        [Theory]
        [InlineData(-15.0, 23, 0, 0.0)]
        [InlineData(-375.0, 23, 0, 0.0)]
        [InlineData(360.0, 0, 0, 0.0)]
        [InlineData(375.0, 1, 0, 0.0)]
        [InlineData(735.0, 1, 0, 0.0)]
        public void DecimalDegreesConstructor_OutOfRangeInputs_NormalizesTo24HourRange(
            double decimalDegrees, double expectedHours, double expectedMinutes, double expectedSeconds)
        {
            var ra = new RightAscension(decimalDegrees);

            Assert.Equal(expectedHours, ra.Hours);
            Assert.Equal(expectedMinutes, ra.Minutes);
            Assert.Equal(expectedSeconds, ra.Seconds, precision: 6);
        }

        [Fact]
        public void ToString_SecondsRoundTo60_CarriesToMinutes()
        {
            var ra = new RightAscension(5, 14, 59.9999999);

            Assert.Equal("5h 15m 0s", ra.ToString());
        }

        [Fact]
        public void ToString_MinutesAndSecondsRoundTo60_CarriesToHours()
        {
            var ra = new RightAscension(5, 59, 59.9999999);

            Assert.Equal("6h 0m 0s", ra.ToString());
        }

        [Fact]
        public void ToString_Near24HourBoundary_WrapsToZeroHours()
        {
            var ra = new RightAscension(23, 59, 59.9999999);

            Assert.Equal("0h 0m 0s", ra.ToString());
        }
    }
}
