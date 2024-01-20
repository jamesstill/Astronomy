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
    }
}