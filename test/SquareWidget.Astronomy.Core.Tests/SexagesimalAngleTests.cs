using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using Xunit;

namespace Astronomy.Tests
{
    public class SexagesimalAngleTests
    {
        // -----------------------------------------------------------------
        // Bug 1: SignOf() used GetHashCode() >> 63, flipping signs on ~half
        // of fractional values.
        // -----------------------------------------------------------------
        [Theory]
        [InlineData(-12, 30, 0.0, true)]
        [InlineData(12, 30, 0.0, false)]
        [InlineData(0, -15, 30.5, true)]
        [InlineData(0, 15, 30.5, false)]
        [InlineData(0, 0, -0.5, true)]
        [InlineData(0, 0, 0.5, false)]
        [InlineData(0, 0, -45.25, true)]
        [InlineData(0, 0, 45.25, false)]
        [InlineData(0, 0, 0.0, false)]
        public void DmsConstructor_DeterminesSignFromFirstNonZeroTerm(
            int degrees, int minutes, double seconds, bool expectedNegative)
        {
            var angle = new SexigesimalAngle(degrees, minutes, seconds);

            Assert.Equal(expectedNegative, angle.IsNegative);
            if (expectedNegative)
            {
                Assert.True(angle.ToDecimalDegrees() < 0);
            }
            else
            {
                Assert.True(angle.ToDecimalDegrees() >= 0);
            }
        }

        // -----------------------------------------------------------------
        // Bug 2: Rounding seconds to 8 decimal places can produce 60.0"
        // without carrying into minutes or degrees.
        // -----------------------------------------------------------------
        [Fact]
        public void DecimalDegreesConstructor_RoundsNearMinuteBoundary_CarriesToMinutes()
        {
            // 10° 14' 59.999999999" -> should carry to 10° 15' 0"
            double dd = 10.0 + (15.0 / 60.0) - 1e-12;
            var angle = new SexigesimalAngle(dd);

            Assert.Equal(10, angle.Degrees);
            Assert.Equal(15, angle.Minutes);
            Assert.Equal(0.0, angle.Seconds);
        }

        [Fact]
        public void DecimalDegreesConstructor_RoundsNearDegreeBoundary_CarriesToDegrees()
        {
            // 0° 59' 59.999999999" -> should carry to 1° 0' 0"
            double dd = 1.0 - 1e-12;
            var angle = new SexigesimalAngle(dd);

            Assert.Equal(1, angle.Degrees);
            Assert.Equal(0, angle.Minutes);
            Assert.Equal(0.0, angle.Seconds);
        }

        [Fact]
        public void DecimalDegreesConstructor_NegativeNearDegreeBoundary_PreservesSignAndCarries()
        {
            double dd = -(1.0 - 1e-12);
            var angle = new SexigesimalAngle(dd);

            Assert.True(angle.IsNegative);
            Assert.Equal(1, angle.Degrees);
            Assert.Equal(0, angle.Minutes);
            Assert.Equal(0.0, angle.Seconds);
        }

        // -----------------------------------------------------------------
        // ToString formatting across all three branches
        // -----------------------------------------------------------------
        [Theory]
        [InlineData(0, 0, -12.5, "-12.5\"")]
        [InlineData(0, 15, 30, "+15' 30\"")]
        [InlineData(-58, 25, 10, "-58° 25' 10\"")]
        public void ToString_FormatsExpectedDmsString(
            int degrees, int minutes, double seconds, string expected)
        {
            var angle = new SexigesimalAngle(degrees, minutes, seconds);

            Assert.Equal(expected, angle.ToString());
        }
    }
}