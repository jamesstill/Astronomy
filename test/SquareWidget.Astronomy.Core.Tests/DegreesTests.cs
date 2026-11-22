using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System.Globalization;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class DegreesTests
    {
        [Fact]
        public void ToString_UnderCommaDecimalCulture_InsertsDegreeSymbolBeforeDecimalPoint()
        {
            CultureInfo originalCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("de-DE");

                var deg = new Degrees(58.4168);

                Assert.Equal("58°.4168", deg.ToString());
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        [Fact]
        public void ToString_WholeNumber_AppendsDegreeSymbol()
        {
            var deg = new Degrees(90);

            Assert.Equal("90°", deg.ToString());
        }

        // -----------------------------------------------------------------
        // Fix: ToReducedAngle() handles ±360 without returning -0.0
        // -----------------------------------------------------------------
        [Theory]
        [InlineData(360.0, 0.0)]
        [InlineData(-360.0, 0.0)]
        [InlineData(-90.0, 270.0)]
        [InlineData(450.0, 90.0)]
        public void ToReducedAngle_BoundaryAndNegativeValues_ReturnsExpectedDegrees(
            double input, double expected)
        {
            double actual = new Degrees(input).ToReducedAngle();

            Assert.Equal(expected, actual, precision: 12);
            Assert.False(double.IsNegative(actual));
        }
    }
}
