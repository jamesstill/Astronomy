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
            EquatorialCoordinates eqc = new(δ, α, ε.ToDegrees());

            SiderealTime st = new(moment);
            RightAscension gmst = new(st.GreenwichMean);
            RightAscension gast = new(st.GreenwichApparent);

            Degrees H = st.ToHourAngle(L, α);

            // construct the horizontal coordinates
            HorizontalCoordinates hc = new(moment, φ, L, eqc);

            // convert the horizontal coordinates back into equatorial coordinates
            EquatorialCoordinates eqc0 = hc.ToΕquatorialCoordinates(moment, φ, L);

            Assert.Equal(8, gmst.Hours);
            Assert.Equal(34, gmst.Minutes);
            Assert.Equal(57.0896, gmst.Seconds, tolerance);

            Assert.Equal(8, gast.Hours);
            Assert.Equal(34, gast.Minutes);
            Assert.Equal(57.073455, gast.Seconds, tolerance);
            Assert.Equal(64.352980, H, tolerance);

            Assert.Equal(15.1242627, hc.h, tolerance);
            Assert.Equal(68.0342926, hc.A, tolerance);

            Assert.Equal(23, eqc0.α.Hours);
            Assert.Equal(9, eqc0.α.Minutes);
            Assert.Equal(16.641, eqc0.α.Seconds, tolerance);
            Assert.Equal(6, eqc0.δ.Degrees);
            Assert.True(eqc0.δ.IsNegative);
            Assert.Equal(43, eqc0.δ.Minutes);
            Assert.Equal(11.61, eqc0.δ.Seconds, tolerance);
        }       
    }
}