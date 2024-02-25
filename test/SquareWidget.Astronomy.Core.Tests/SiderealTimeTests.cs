using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class SiderealTimeTests
    {
        [Fact]
        public void Test_Greenwich_Mean_Sidereal_Time_1987_Apr_10()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(1987, 4, 10);
            Moment moment = new Moment(datetime);
            SiderealTime st = new(moment);
            Degrees d = st.GreenwichMean;
            RightAscension gmst = new(d);

            Assert.Equal(2446895.5, moment.JDE, tolerance);
            Assert.Equal(-0.127296, moment.T, tolerance);
            Assert.Equal(197.693195, st.GreenwichMean, tolerance);
            Assert.Equal(197.693129, st.GreenwichApparent, tolerance);
            Assert.Equal(13, gmst.Hours);
            Assert.Equal(10, gmst.Minutes);
            Assert.Equal(46.3668, gmst.Seconds, tolerance);
        }

        [Fact]
        public void Test_Greenwich_Mean_Sidereal_Time_1987_Apr_10_19_21_00()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(1987, 4, 10, 19, 21, 0);
            Moment moment = new Moment(datetime);
            SiderealTime st = new(moment);
            Degrees d = st.GreenwichMean;
            RightAscension gmst = new(d);

            Assert.Equal(2446896.306249, moment.JDE, tolerance);
            Assert.Equal(-0.127296, moment.T, tolerance);
            Assert.Equal(128.737873, st.GreenwichMean, tolerance);
            Assert.Equal(128.737806, st.GreenwichApparent, tolerance);
            Assert.Equal(8, gmst.Hours);
            Assert.Equal(34, gmst.Minutes);
            Assert.Equal(57.089578, gmst.Seconds, tolerance);
        }

        [Fact]
        public void Test_Local_Sidereal_Time_2024_Feb_19()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(2024, 2, 19, 2, 0, 0); // UTC
            Moment moment = new Moment(datetime);
            SiderealTime st = new(moment);

            RightAscension gmst = new(st.GreenwichMean);

            SexigesimalAngle L = new(-123, 15, 43.34); // longitude in Corvallis, OR
            RightAscension lst = st.ToLocalMean(L);   // local mean sidereal time on the date (PST -8 offset)

            Assert.Equal(2460359.583333, moment.JDE, tolerance);
            Assert.Equal(178.5315, gmst.ToDegrees(), tolerance);
            Assert.Equal(11, gmst.Hours);
            Assert.Equal(54, gmst.Minutes);
            Assert.Equal(7.557231, gmst.Seconds, tolerance);
            Assert.Equal(55.269441, lst.ToDecimalDegrees(), tolerance);
            Assert.Equal(3, lst.Hours);
            Assert.Equal(41, lst.Minutes);
            Assert.Equal(4.667898, lst.Seconds, tolerance);
        }

        [Fact]
        public void Test_Local_Sidereal_Time_2024_Jul_4()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(2024, 7, 4, 18, 0, 0); // UTC
            Moment moment = new Moment(datetime);
            SiderealTime st = new(moment);
            SexigesimalAngle L = new(-118, 14, 38); // longitude in Los Angeles, USA
            
            RightAscension gmst = new(st.GreenwichMean);
            RightAscension lst = st.ToLocalMean(L);

            Assert.Equal(12, gmst.Hours);
            Assert.Equal(52, gmst.Minutes);
            Assert.Equal(56.791001, gmst.Seconds, tolerance);

            Assert.Equal(4, lst.Hours);
            Assert.Equal(59, lst.Minutes);
            Assert.Equal(58.257668, lst.Seconds, tolerance);
        }

        [Fact]
        public void Test_Local_Mean_Sidereal_Time_2024_Feb_25()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(2024, 2, 25, 18, 0, 0); // UTC
            Moment moment = new Moment(datetime);
            SiderealTime st = new(moment);

            SexigesimalAngle L = new(-123, 15, 43.34);
            RightAscension lst = st.ToLocalMean(L);

            Assert.Equal(20, lst.Hours);
            Assert.Equal(7, lst.Minutes);
            Assert.Equal(21.703678, lst.Seconds, tolerance);
        }

        [Fact]
        public void Test_Local_Apparent_Sidereal_Time_2024_Feb_25()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(2024, 2, 25, 18, 0, 0); // UTC
            Moment moment = new Moment(datetime);
            SiderealTime st = new(moment);

            SexigesimalAngle L = new(-123, 15, 43.34);
            RightAscension lst = st.ToLocalApparent(L);

            Assert.Equal(20, lst.Hours);
            Assert.Equal(7, lst.Minutes);
            Assert.Equal(21.687064, lst.Seconds, tolerance);
        }

        [Fact]
        public void Test_Local_Hour_Angle_Moon_2024_Jul_4()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(2024, 7, 4, 17, 0, 0); // UTC
            Moment moment = new Moment(datetime);
            SiderealTime st = new(moment);

            SexigesimalAngle L = new(-118, 14, 38); // longitude in Los Angeles, USA
            RightAscension α = new(5.838); // Moon

            Degrees H = st.ToHourAngle(L, α);

            Assert.Equal(290.601449, H, tolerance);
        }
    }
}