using SquareWidget.Astronomy.Core.CelestialObjects.Moons;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class MoonPositionTests
    {
        [Fact]
        public void GeocentricPosition_MatchesMeeusExample47A()
        {
            Moon moon = new(CreateUtcMomentForMeeusExample());
            EclipticalCoordinates ecliptic = moon.GeocentricEclipticalCoordinates;
            double longitude = ecliptic.λ.ToDegrees();
            double latitude = ecliptic.β.ToDegrees();

            Assert.InRange(longitude, 133.162645, 133.162665);
            Assert.InRange(latitude, -3.229136, -3.229116);
            Assert.Equal(368409.7, moon.GeocentricDistanceKilometers, 1);
        }

        [Fact]
        public void ApparentCoordinates_IncludeNutationCorrection()
        {
            Moon moon = new(CreateUtcMomentForMeeusExample());
            double meanLongitude = moon.GeocentricEclipticalCoordinates.λ.ToDegrees();
            double apparentLongitude = moon.ApparentEclipticalCoordinates.λ.ToDegrees();

            Assert.True(Math.Abs(apparentLongitude - meanLongitude - moon.Nutation.ΔΨ.ToDecimalDegrees()) < 1e-10);
            Assert.NotEqual(
                moon.GeocentricEquitorialCoordinates.α.ToDecimalDegrees(),
                moon.ApparentEquitorialCoordinates.α.ToDecimalDegrees());
        }

        [Fact]
        public void HorizontalParallax_IsDerivedFromGeocentricDistance()
        {
            Moon moon = new(CreateUtcMomentForMeeusExample());
            double expectedParallax = Math.Asin(6378.14 / moon.GeocentricDistanceKilometers) * (180.0 / Math.PI);

            Assert.Equal(expectedParallax, moon.HorizontalParallax, 12);
            Assert.InRange((double)moon.HorizontalParallax, 0.9, 1.1);
        }

        [Fact]
        public void GeocentricEquatorialPosition_ConvertsFromEclipticCoordinates()
        {
            Moon moon = new(CreateUtcMomentForMeeusExample());
            EquitorialCoordinates expected = new(moon.GeocentricEclipticalCoordinates);

            Assert.Equal(expected.α.ToDecimalDegrees(), moon.GeocentricEquitorialCoordinates.α.ToDecimalDegrees(), 12);
            Assert.Equal((double)expected.δ, (double)moon.GeocentricEquitorialCoordinates.δ, 12);
        }

        private static Moment CreateUtcMomentForMeeusExample()
        {
            Moment referenceTerrestrialTime = new(1992, 4, 12, 0, 0);
            double utcJulianDay = referenceTerrestrialTime.JDE - DeltaT.GetValue(1992).TotalDays;
            return new Moment(utcJulianDay);
        }
    }
}
