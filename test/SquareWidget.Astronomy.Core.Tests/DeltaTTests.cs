using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace Astronomy.Tests
{
    public class DeltaTBugTests
    {
        [Theory]
        // Before -500: -20 + 32 * ((y - 1820) / 100.0)^2
        [InlineData(-1000, 25427.68)]
        [InlineData(-501, 17218.53)]

        // -500 to 499: 6th-degree polynomial (u = y / 100.0)
        [InlineData(-500, 17203.66)]
        [InlineData(0, 10583.60)]
        [InlineData(100, 9596.87)]

        // 500 to 1599: 6th-degree polynomial (u = (y - 1000) / 100.0)
        [InlineData(1000, 1574.20)]
        [InlineData(1100, 1088.90)]

        // 1600 to 1619: cubic polynomial (t = y - 1600)
        [InlineData(1600, 120.00)]
        [InlineData(1610, 108.80)]

        // 1620 to 2009: observed Values dictionary
        [InlineData(1620, 124.00)]
        [InlineData(1900, -2.72)]
        [InlineData(2000, 63.83)]
        [InlineData(2009, 65.78)]

        // 2010 to 2050: quadratic polynomial (t = y - 2000)
        [InlineData(2010, 66.70)]
        [InlineData(2050, 93.00)]

        // 2051 to 2150: catches integer division bug
        [InlineData(2051, 95.04)]
        [InlineData(2119, 248.64)]
        [InlineData(2150, 328.48)]

        // After 2150: catches integer division bug
        [InlineData(2151, 330.60)]
        [InlineData(2200, 442.08)]
        public void GetValue_AllYearRanges_ReturnsExpectedSeconds(int year, double expectedSeconds)
        {
            TimeSpan actual = DeltaT.GetValue(year);

            Assert.Equal(expectedSeconds, actual.TotalSeconds, precision: 2);
        }

        // -----------------------------------------------------------------
        // 2051–2150 branch: -20 + 32 * ((y - 1820) / 100.0)^2 - 0.5628 * (2150 - y)
        // Integer division truncates (y - 1820) / 100, dropping up to 158s.
        // -----------------------------------------------------------------
        [Theory]
        [InlineData(2051, 95.038)]   // Bug returns ~52.283s
        [InlineData(2119, 248.636)]  // Bug returns ~90.553s (worst-case before 2120 jump)
        [InlineData(2150, 328.480)]  // Bug returns 268.000s
        public void DeltaT_2051To2150_UsesFloatingPointDivision(int year, double expectedSeconds)
        {
            TimeSpan actual = DeltaT.GetValue(year);

            Assert.Equal(expectedSeconds, actual.TotalSeconds, precision: 2);
        }

        // -----------------------------------------------------------------
        // Post-2150 branch: -20 + 32 * ((y - 1820) / 100.0)^2
        // -----------------------------------------------------------------
        [Theory]
        [InlineData(2151, 330.595)]  // Bug returns 268.000s
        [InlineData(2200, 442.080)]  // Bug returns 268.000s
        public void DeltaT_After2150_UsesFloatingPointDivision(int year, double expectedSeconds)
        {
            TimeSpan actual = DeltaT.GetValue(year);

            Assert.Equal(expectedSeconds, actual.TotalSeconds, precision: 2);
        }

        // -----------------------------------------------------------------
        // Regression guard: verifies no artificial step-jump at century boundaries
        // With integer division, 2119 -> 2120 jumps by ~160 seconds in a single year.
        // -----------------------------------------------------------------
        [Fact]
        public void DeltaT_2119To2120_IncreasesSmoothlyWithoutStepJump()
        {
            double delta2119 = DeltaT.GetValue(2119).TotalSeconds;
            double delta2120 = DeltaT.GetValue(2120).TotalSeconds;
            double yearlyIncrease = delta2120 - delta2119;

            Assert.InRange(yearlyIncrease, 2.0, 3.0);
        }
    }
}