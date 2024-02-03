using SquareWidget.Astronomy.Core.CelestialObjects.Stars;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    /// <summary>
    /// Calculator used to build test cases: 
    /// https://ned.ipac.caltech.edu/coordinate_calculator
    /// </summary>
    public class CoordinateSystemsTests
    {
        double tolerance = 0.0001;

        [Fact]
        public void Test_Equatorial_Conversion_To_Ecliptial_Pollux()
        {
            // Pollux (β Gem)
            RightAscension α = new RightAscension(7, 45, 18.946);
            SexigesimalAngle δ = new SexigesimalAngle(28, 01, 34.26);
            Degrees ε = new Degrees(23.4392911);

            EquitorialCoordinates eqc = new EquitorialCoordinates(δ, α, ε);
            EclipticalCoordinates ec = eqc.ToΕclipticCoordinates();

            Assert.Equal(113.215630, ec.λ.ToDegrees(), tolerance);
            Assert.Equal(6.684170, ec.β.ToDegrees(), tolerance);
            Assert.Equal(23.4392911, ec.ε.ToDegrees(), tolerance);
        }

        [Fact]
        public void Test_Ecliptical_Conversion_To_Equatorial_Pollux()
        {
            // Pollux (β Gem)
            Degrees λ = new Degrees(113.215630);
            Degrees β = new Degrees(6.684170);
            Degrees ε = new Degrees(23.4392911);

            EclipticalCoordinates ec = new EclipticalCoordinates(λ, β, ε);
            EquitorialCoordinates eqc = ec.ToΕquitorialCoordinates();

            RightAscension α = eqc.α;
            SexigesimalAngle δ = eqc.δ;

            Assert.Equal(7, α.Hours);
            Assert.Equal(45, α.Minutes);
            Assert.Equal(18.946201, α.Seconds, tolerance);

            Assert.False(δ.IsNegative);
            Assert.Equal(28, δ.Degrees);
            Assert.Equal(01, δ.Minutes);
            Assert.Equal(34.259254, δ.Seconds, tolerance);
        }

        [Fact]
        public void Test_Ecliptical_Conversion_To_Equitorial_Jupiter()
        {
            Degrees l = new Degrees(184.6);
            Degrees b = new Degrees(1.2);
            Degrees e = new Degrees(23.439292);

            Radians λ = l.ToRadians();
            Radians β = b.ToRadians();
            Radians ε = e.ToRadians();

            EclipticalCoordinates ec = new EclipticalCoordinates(λ, β, ε);
            EquitorialCoordinates eqc = ec.ToΕquitorialCoordinates();

            RightAscension α = eqc.α;
            SexigesimalAngle δ = eqc.δ;

            // right ascension
            Assert.Equal(12, α.Hours);
            Assert.Equal(18, α.Minutes);
            Assert.Equal(47.4954, α.Seconds, tolerance);

            // declination
            Assert.True(δ.IsNegative);
            Assert.Equal(0, δ.Degrees);
            Assert.Equal(43, δ.Minutes);
            Assert.Equal(35.509946, δ.Seconds, tolerance);
        }

        [Fact]
        public void Test_Sun_Position_1992_Oct_13()
        {
            double tolerance = 0.0001;

            DateTime datetime = new(1992, 10, 13);
            Moment moment = new(datetime);
            Sun sun = new(moment);

            EquitorialCoordinates g = sun.GetGeocentricPosition();

            Assert.Equal(13, g.α.Hours);
            Assert.Equal(13, g.α.Minutes);
            Assert.Equal(31.4565, g.α.Seconds, tolerance);
            Assert.Equal(-7.78447, g.δ.ToDecimalDegrees(), tolerance);
            Assert.True(g.δ.IsNegative);
            Assert.Equal(7, g.δ.Degrees);
            Assert.Equal(47, g.δ.Minutes);
            Assert.Equal(4.12007, g.δ.Seconds, tolerance);
        }
    }
}
