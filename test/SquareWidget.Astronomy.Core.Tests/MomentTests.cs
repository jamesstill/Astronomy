using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class MomentTests
    {
        [Fact]
        public void Test_JulianDay_Date1()
        {
            var moment = new Moment(2000, 1, 1.5);
            Assert.Equal(2451545.0, moment.JDE);
        }

        [Fact]
        public void Test_JulianDay_Date2()
        {
            var moment = new Moment(1999, 1, 1.0);
            Assert.Equal(2451179.5, moment.JDE);
        }

        [Fact]
        public void Test_JulianDay_Date3()
        {
            var moment = new Moment(1987, 1, 27.0);
            Assert.Equal(2446822.5, moment.JDE);
        }

        [Fact]
        public void Test_JulianDay_Date4()
        {
            var moment = new Moment(1600, 1, 1.0);
            Assert.Equal(2305447.5, moment.JDE);
        }

        [Fact]
        public void Test_JulianDay_Date5()
        {
            var moment = new Moment(-123, 12, 31.0);
            Assert.Equal(1676496.5, moment.JDE);
        }

        [Fact]
        public void Test_JulianDay_Date6()
        {
            var moment = new Moment(837, 4, 10.3);
            Assert.Equal(2026871.8, moment.JDE);
        }

        [Fact]
        public void Test_JulianDay_Date7()
        {
            DateTime datetime = new(2065, 6, 24);
            Moment moment = new(datetime);
            Assert.Equal(2475460.5, moment.JDE);
        }

        [Fact]
        public void Test_JulianDay_Date8()
        {
            var moment = new Moment(-4712, 1, 1.5);
            Assert.Equal(0.0, moment.JDE);
        }

        [Fact]
        public void Test_T_2065_Jun_24()
        {
            DateTime datetime = new(2065, 6, 24);
            Moment moment = new(datetime);

            Assert.Equal(0.654770704997, moment.T, 9);
        }

        // -----------------------------------------------------------------
        // Bug 1a: ToRoundedSeconds drops leading zeros on milliseconds
        // 23s + 5ms (23.005s) gets concatenated as "23.5" and rounded to 24.
        // -----------------------------------------------------------------
        [Fact]
        public void FractionalDay_SmallMilliseconds_DoesNotInflateSeconds()
        {
            double day = 1.0 + (23.005 / 86400.0); // Day 1, 00:00:23.005
            var m = new Moment(2024, 1, day);

            Assert.Equal(0, m.Hour);
            Assert.Equal(0, m.Minute);
            Assert.Equal(23, m.Second);
        }

        // -----------------------------------------------------------------
        // Bug 1b: ToRoundedSeconds rolls over to 60 without carrying
        // 19:26:59.999 produces Second = 60 and crashes ToDateTime().
        // -----------------------------------------------------------------
        [Fact]
        public void FractionalDay_NearMinuteBoundary_CarriesRolloverCorrectly()
        {
            double totalSeconds = (19 * 3600) + (26 * 60) + 59.999;
            double day = 1.0 + (totalSeconds / 86400.0);

            var m = new Moment(2024, 6, day);

            Assert.Equal(19, m.Hour);
            Assert.Equal(27, m.Minute);
            Assert.Equal(0, m.Second);

            // Should not throw ArgumentOutOfRangeException
            var dt = m.ToDateTime();
            Assert.Equal(new DateTime(2024, 6, 1, 19, 27, 0), dt);
        }

        // -----------------------------------------------------------------
        // Bug 2: 1582 Gregorian reform gap (Oct 5–14)
        // Oct 5–14, 1582 did not exist in either calendar.
        // -----------------------------------------------------------------
        [Theory]
        [InlineData(1582, 10, 4, true, false)]   // Last Julian date
        [InlineData(1582, 10, 5, false, false)]  // Gap start
        [InlineData(1582, 10, 10, false, false)] // Mid-gap (currently true for BOTH)
        [InlineData(1582, 10, 14, false, false)] // Gap end
        [InlineData(1582, 10, 15, false, true)]  // First Gregorian date
        public void CalendarClassification_1582ReformBoundary_HandlesGapCorrectly(
            int year, int month, double day, bool expectedJulian, bool expectedGregorian)
        {
            var m = new Moment(year, month, day);

            Assert.Equal(expectedJulian, m.IsJulianDate());
            Assert.Equal(expectedGregorian, m.IsGregorianDate());
        }

        // -----------------------------------------------------------------
        // Bug 3: GetHashCode() violates Equals() contract
        // Equal JDE values must produce identical hash codes for HashSet/Dictionary.
        // -----------------------------------------------------------------
        [Fact]
        public void EqualMoments_HaveSameHashCode_AndWorkInHashSet()
        {
            var fromJde = new Moment(2451545.0);      // J2000.0 (2000-01-01 12:00:00)
            var fromYmd = new Moment(2000, 1, 1.5);   // Same instant, different ctor path

            Assert.Equal(fromJde, fromYmd);
            Assert.Equal(fromJde.GetHashCode(), fromYmd.GetHashCode());

            var set = new HashSet<Moment> { fromJde };
            Assert.Contains(fromYmd, set);
        }

        // -----------------------------------------------------------------
        // Bug 4: ToDecimalYear() is off by one day and uses 365.2425
        // -----------------------------------------------------------------
        [Fact]
        public void ToDecimalYear_Jan1_ReturnsExactIntegerYear()
        {
            var m = new Moment(2023, 1, 1.0);

            Assert.Equal(2023.0, m.ToDecimalYear(), precision: 6);
        }

        [Fact]
        public void ToDecimalYear_LeapYearDec31_DoesNotRollIntoNextYear()
        {
            var m = new Moment(2024, 12, 31.0);
            double expected = 2024.0 + (365.0 / 366.0);

            Assert.Equal(expected, m.ToDecimalYear(), precision: 6);
        }

        [Fact]
        public void ToDecimalYear_NonLeapYearMidYear_Uses365DayDenominator()
        {
            var m = new Moment(2023, 7, 2.0); // DayOfYear = 183 -> 182 full days elapsed
            double expected = 2023.0 + (182.0 / 365.0);

            Assert.Equal(expected, m.ToDecimalYear(), precision: 6);
        }

        // -----------------------------------------------------------------
        // Bug 5: ToString() throws on BCE / Year 0 dates
        // -----------------------------------------------------------------
        [Theory]
        [InlineData(0, 1, 1.0)]     // 1 BCE (astronomical year 0)
        [InlineData(-4712, 1, 1.5)] // Julian Day 0
        public void ToString_BceDates_DoesNotThrow(int year, int month, double day)
        {
            var m = new Moment(year, month, day);

            var ex = Record.Exception(() => m.ToString());
            Assert.Null(ex);
        }
    }
}