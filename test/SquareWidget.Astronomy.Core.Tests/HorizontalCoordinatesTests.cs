using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class HorizontalCoordinatesTests
    {
        [Fact]
        public void Test_Alt_Az_Venus_From_USNO_On_1987_Apr_10()
        {
            double tolerance = 0.0001;

            DateTime datetime = new(1987, 4, 10, 19, 21, 0);
            Moment moment = new Moment(datetime);

            // Earth's obliquity of the ecliptic
            Earth earth = new(moment);
            SexigesimalAngle ε = earth.MeanObliquity;

            // U.S. Naval Observatory
            SexigesimalAngle L = new(77, 3, 56);   // longitude
            SexigesimalAngle φ = new (38, 55, 17); // latitude

            // Venus apparent equatorial coordinates
            RightAscension α = new(23, 9, 16.641);
            SexigesimalAngle δ = new(-6, 43, 11.61);
            EquatorialCoordinates ec = new(δ, α, ε.ToDegrees());

            SiderealTime st = new(moment);
            RightAscension gmst = new(st.GreenwichMean);
            RightAscension gast = new(st.GreenwichApparent);

            RightAscension H = new(st.ToHourAngle(L, α));

            Assert.Equal(8, gmst.Hours);
            Assert.Equal(34, gmst.Minutes);
            Assert.Equal(57.0896, gmst.Seconds, tolerance);

            Assert.Equal(8, gast.Hours);
            Assert.Equal(34, gast.Minutes);
            Assert.Equal(57.073455, gast.Seconds, tolerance);
            Assert.Equal(64.352133, H.ToDegrees(), tolerance);
            

            //Degrees H = st.ToHourAngle(L, α);

            // HorizontalCoordinates hc = new(moment, φ, L, ec);

            //Assert.Equal(8, gast.Hours);
            //Assert.Equal(34, gast.Minutes);
            //Assert.Equal(56.853, gast.Seconds);
            //Assert.Equal(64.352133, H, tolerance);
            //Assert.Equal(68.0337, )



            //Assert.Equal(2446895.5, moment.JDE, tolerance);
            //Assert.Equal(-0.127296, moment.T, tolerance);
            //Assert.Equal(197.693195, st.GreenwichMean, tolerance);
            //Assert.Equal(197.693129, st.GreenwichApparent, tolerance);
            //Assert.Equal(13, gmst.Hours);
            //Assert.Equal(10, gmst.Minutes);
            //Assert.Equal(46.3668, gmst.Seconds, tolerance);
        }

       
    }
}