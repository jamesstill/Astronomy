using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class CoordinateCorrectionsTests
    {
        [Fact]
        public void AtmosphericRefraction_IncreasesAltitudeMoreNearHorizon()
        {
            double horizonCorrection = AtmosphericRefraction.CalculateCorrectionDegrees(0.0);
            double highAltitudeCorrection = AtmosphericRefraction.CalculateCorrectionDegrees(45.0);

            Assert.True(horizonCorrection > highAltitudeCorrection);
            Assert.InRange(horizonCorrection, 0.4, 0.6);
            Assert.InRange(highAltitudeCorrection, 0.0, 0.03);
        }

        [Fact]
        public void AtmosphericRefraction_PreservesAzimuthAndEpsilon_AndIsMonotonic()
        {
            HorizontalCoordinates input = new(0.0, 137.5, 23.4);
            HorizontalCoordinates corrected = AtmosphericRefraction.Correct(input);
            double previousAltitude = -1.0;

            Assert.True(corrected.h > input.h);
            Assert.Equal((double)input.A, (double)corrected.A);
            Assert.Equal((double)input.ε, (double)corrected.ε);

            for (double altitude = -1.0; altitude <= 90.0; altitude += 1.0)
            {
                double apparentAltitude = altitude + AtmosphericRefraction.CalculateCorrectionDegrees(altitude);
                Assert.True(apparentAltitude >= previousAltitude);
                Assert.True(apparentAltitude <= 90.0);
                previousAltitude = apparentAltitude;
            }
        }

        [Fact]
        public void AtmosphericRefraction_HandlesHorizonAndZenithBoundaries()
        {
            Assert.True(AtmosphericRefraction.CalculateCorrectionDegrees(-1.0) > 0.0);
            Assert.True(AtmosphericRefraction.CalculateCorrectionDegrees(0.0) > 0.0);
            Assert.Equal(0.0, AtmosphericRefraction.CalculateCorrectionDegrees(90.0));
            Assert.Equal(90.0, AtmosphericRefraction.Correct(new HorizontalCoordinates(90.0, 0.0, 0.0)).h);
        }

        [Fact]
        public void AtmosphericRefraction_CorrectedCoordinatesRoundTripThroughEquatorialCoordinates()
        {
            Moment moment = new(2024, 3, 20, 12, 0);
            SexigesimalAngle latitude = new(35.0);
            SexigesimalAngle longitude = new(-105.0);
            HorizontalCoordinates refracted = AtmosphericRefraction.Correct(new HorizontalCoordinates(20.0, 140.0, 23.4));

            EquitorialCoordinates equatorial = refracted.ToEquitorialCoordinates(moment, latitude, longitude);
            HorizontalCoordinates roundTrip = new(moment, latitude, longitude, equatorial);

            Assert.Equal((double)refracted.h, (double)roundTrip.h, 10);
            Assert.Equal((double)refracted.A, (double)roundTrip.A, 10);
        }

        [Theory]
        [InlineData(double.NaN, 1010.0, 10.0)]
        [InlineData(-1.01, 1010.0, 10.0)]
        [InlineData(90.01, 1010.0, 10.0)]
        [InlineData(10.0, -1.0, 10.0)]
        [InlineData(10.0, 1010.0, -273.15)]
        public void AtmosphericRefraction_RejectsInvalidInputs(double altitude, double pressure, double temperature)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                AtmosphericRefraction.CalculateCorrectionDegrees(altitude, pressure, temperature));
        }

        [Fact]
        public void TopocentricParallax_ZeroParallaxIsAnIdentityTransform()
        {
            EquitorialCoordinates geocentric = new(new SexigesimalAngle(-12.5), new RightAscension(120.0), new Degrees(23.4));

            EquitorialCoordinates topocentric = TopocentricParallax.ToTopocentric(
                geocentric,
                new Moment(2024, 1, 1, 0, 0),
                new Degrees(45.0),
                new Degrees(-75.0),
                100.0,
                new Degrees(0.0));

            Assert.Equal(geocentric.α.ToDecimalDegrees(), topocentric.α.ToDecimalDegrees());
            Assert.Equal((double)geocentric.δ, (double)topocentric.δ);
            Assert.Equal((double)geocentric.ε, (double)topocentric.ε);
        }

        [Fact]
        public void TopocentricParallax_RejectsInvalidMoment()
        {
            EquitorialCoordinates position = new(new SexigesimalAngle(0.0), new RightAscension(0.0), new Degrees(0.0));

            Assert.Throws<ArgumentOutOfRangeException>(() => TopocentricParallax.ToTopocentric(
                position,
                default,
                new Degrees(0.0),
                new Degrees(0.0),
                0.0,
                new Degrees(0.1)));
        }

        [Fact]
        public void TopocentricParallax_ShiftsMoreForLargerParallax()
        {
            Moment moment = new(2024, 1, 1, 0, 0);
            double rightAscension = (double)new SiderealTime(moment).GreenwichMean + 90.0;
            EquitorialCoordinates geocentric = new(new SexigesimalAngle(0.0), new RightAscension(rightAscension), new Degrees(23.4));

            EquitorialCoordinates smallerParallax = TopocentricParallax.ToTopocentric(
                geocentric, moment, new Degrees(0.0), new Degrees(0.0), 0.0, new Degrees(0.5));
            EquitorialCoordinates largerParallax = TopocentricParallax.ToTopocentric(
                geocentric, moment, new Degrees(0.0), new Degrees(0.0), 0.0, new Degrees(1.0));
            double smallerShift = Math.Abs(SignedAngleDifference(smallerParallax.α.ToDecimalDegrees(), geocentric.α.ToDecimalDegrees()));
            double largerShift = Math.Abs(SignedAngleDifference(largerParallax.α.ToDecimalDegrees(), geocentric.α.ToDecimalDegrees()));

            Assert.True(smallerShift > 0.0);
            Assert.True(largerShift > smallerShift);
        }

        [Theory]
        [InlineData(-90.01, 0.0, 0.0, 0.1)]
        [InlineData(90.01, 0.0, 0.0, 0.1)]
        [InlineData(0.0, 180.01, 0.0, 0.1)]
        [InlineData(0.0, 0.0, double.NaN, 0.1)]
        [InlineData(0.0, 0.0, 0.0, 90.0)]
        public void TopocentricParallax_RejectsInvalidObserverOrParallax(
            double latitude,
            double longitude,
            double elevation,
            double parallax)
        {
            EquitorialCoordinates position = new(new SexigesimalAngle(0.0), new RightAscension(0.0), new Degrees(0.0));

            Assert.Throws<ArgumentOutOfRangeException>(() => TopocentricParallax.ToTopocentric(
                position,
                new Moment(2024, 1, 1, 0, 0),
                new Degrees(latitude),
                new Degrees(longitude),
                elevation,
                new Degrees(parallax)));
        }

        private static double SignedAngleDifference(double first, double second)
        {
            double difference = (first - second + 180.0) % 360.0;
            if (difference < 0.0)
            {
                difference += 360.0;
            }

            return difference - 180.0;
        }
    }
}
