using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Transforms geocentric equatorial coordinates to topocentric coordinates using horizontal parallax.
    /// </summary>
    public static class TopocentricParallax
    {
        private const double EarthEquatorialRadiusMeters = 6378137.0;
        private const double EarthFlatteningFactor = 0.99664719;

        /// <summary>
        /// Convert geocentric equatorial coordinates to topocentric equatorial coordinates.
        /// Horizontal parallax specifies the object's distance; observer longitude is east-positive.
        /// </summary>
        public static EquitorialCoordinates ToTopocentric(
            EquitorialCoordinates geocentric,
            Moment moment,
            Degrees latitude,
            Degrees longitude,
            double elevationMeters,
            Degrees horizontalParallax)
        {
            double latitudeDegrees = latitude;
            double longitudeDegrees = longitude;
            double parallaxDegrees = horizontalParallax;
            double declinationDegrees = geocentric.δ;
            double rightAscensionDegrees = geocentric.α.ToDecimalDegrees();

            ValidateFinite(latitudeDegrees, nameof(latitude));
            ValidateFinite(longitudeDegrees, nameof(longitude));
            ValidateFinite(elevationMeters, nameof(elevationMeters));
            ValidateFinite(parallaxDegrees, nameof(horizontalParallax));
            ValidateFinite(declinationDegrees, nameof(geocentric));
            ValidateFinite(rightAscensionDegrees, nameof(geocentric));
            ValidateMoment(moment);

            if (latitudeDegrees < -90.0 || latitudeDegrees > 90.0)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees.");
            }

            if (longitudeDegrees < -180.0 || longitudeDegrees > 180.0)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees.");
            }

            if (elevationMeters <= -EarthEquatorialRadiusMeters)
            {
                throw new ArgumentOutOfRangeException(nameof(elevationMeters), "Elevation must be greater than the negative Earth equatorial radius.");
            }

            if (parallaxDegrees < 0.0 || parallaxDegrees >= 90.0)
            {
                throw new ArgumentOutOfRangeException(nameof(horizontalParallax), "Horizontal parallax must be between 0 inclusive and 90 exclusive degrees.");
            }

            if (declinationDegrees < -90.0 || declinationDegrees > 90.0)
            {
                throw new ArgumentOutOfRangeException(nameof(geocentric), "Declination must be between -90 and 90 degrees.");
            }

            if (parallaxDegrees == 0.0)
            {
                return geocentric;
            }

            double phi = latitudeDegrees * (Math.PI / 180.0);

            double delta = declinationDegrees * (Math.PI / 180.0);

            double alpha = rightAscensionDegrees * (Math.PI / 180.0);

            double parallax = parallaxDegrees * (Math.PI / 180.0);

            double u = Math.Atan(EarthFlatteningFactor * Math.Tan(phi));

            double heightRatio = elevationMeters / EarthEquatorialRadiusMeters;

            double rhoSinPhi = EarthFlatteningFactor * Math.Sin(u) + heightRatio * Math.Sin(phi);
            double rhoCosPhi = Math.Cos(u) + heightRatio * Math.Cos(phi);

            double localSiderealTime = (double)new SiderealTime(moment).GreenwichMean
                * (Math.PI / 180.0) + longitudeDegrees * (Math.PI / 180.0);

            double hourAngle = localSiderealTime - alpha;

            double deltaAlpha = Math.Atan2(
                -rhoCosPhi * Math.Sin(parallax) * Math.Sin(hourAngle),
                Math.Cos(delta) - rhoCosPhi * Math.Sin(parallax) * Math.Cos(hourAngle));

            double topocentricRightAscension = alpha + deltaAlpha;

            double topocentricDeclination = Math.Atan2(
                (Math.Sin(delta) - rhoSinPhi * Math.Sin(parallax)) * Math.Cos(deltaAlpha),
                Math.Cos(delta) - rhoCosPhi * Math.Sin(parallax) * Math.Cos(hourAngle));

            RightAscension rightAscension = new(new Radians(topocentricRightAscension).ToDegrees().ToReducedAngle());
            SexigesimalAngle declination = new(new Radians(topocentricDeclination).ToDegrees());
            return new EquitorialCoordinates(declination, rightAscension, geocentric.ε.ToDegrees());
        }

        private static void ValidateMoment(Moment moment)
        {
            if (moment.Month < 1 || moment.Month > 12 || moment.Day < 1
                || moment.Hour < 0 || moment.Hour > 23
                || moment.Minute < 0 || moment.Minute > 59
                || moment.Second < 0 || moment.Second > 59
                || moment.Millisecond < 0 || moment.Millisecond > 999)
            {
                throw new ArgumentOutOfRangeException(nameof(moment), "Moment must contain valid calendar and time components.");
            }

            if (moment.Year == 1582 && moment.Month == 10 && moment.Day is > 4 and < 15)
            {
                throw new ArgumentOutOfRangeException(nameof(moment), "Moment cannot fall in the Gregorian calendar transition gap.");
            }

            bool isLeapYear = moment.IsGregorianDate()
                ? moment.Year % 4 == 0 && (moment.Year % 100 != 0 || moment.Year % 400 == 0)
                : moment.Year % 4 == 0;
            int daysInMonth = moment.Month switch
            {
                2 => isLeapYear ? 29 : 28,
                4 or 6 or 9 or 11 => 30,
                _ => 31
            };

            if (moment.Day > daysInMonth)
            {
                throw new ArgumentOutOfRangeException(nameof(moment), "Moment must contain a valid day for its month.");
            }
        }

        private static void ValidateFinite(double value, string parameterName)
        {
            if (!double.IsFinite(value))
            {
                throw new ArgumentOutOfRangeException(parameterName, "Value must be finite.");
            }
        }
    }
}
