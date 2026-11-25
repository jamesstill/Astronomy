using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Applies a deterministic Bennett atmospheric-refraction correction to horizontal coordinates.
    /// </summary>
    public static class AtmosphericRefraction
    {
        /// <summary>
        /// Return horizontal coordinates with apparent altitude increased by atmospheric refraction.
        /// Altitudes below -1 degree or above 90 degrees are outside the supported model range.
        /// </summary>
        public static HorizontalCoordinates Correct(
            HorizontalCoordinates coordinates,
            double pressureHectopascals = 1010.0,
            double temperatureCelsius = 10.0)
        {
            double altitude = coordinates.h;
            double correctedAltitude = altitude + CalculateCorrectionDegrees(
                altitude,
                pressureHectopascals,
                temperatureCelsius);

            return new HorizontalCoordinates(correctedAltitude, coordinates.A, coordinates.ε);
        }

        /// <summary>
        /// Calculate Bennett refraction in degrees for an observed altitude.
        /// Pressure must be from 0 to 1200 hPa and temperature must be above absolute zero.
        /// </summary>
        public static double CalculateCorrectionDegrees(
            double altitudeDegrees,
            double pressureHectopascals = 1010.0,
            double temperatureCelsius = 10.0)
        {
            if (!double.IsFinite(altitudeDegrees) || altitudeDegrees < -1.0 || altitudeDegrees > 90.0)
            {
                throw new ArgumentOutOfRangeException(nameof(altitudeDegrees), "Altitude must be finite and between -1 and 90 degrees.");
            }

            if (!double.IsFinite(pressureHectopascals) || pressureHectopascals < 0.0 || pressureHectopascals > 1200.0)
            {
                throw new ArgumentOutOfRangeException(nameof(pressureHectopascals), "Pressure must be finite and between 0 and 1200 hPa.");
            }

            if (!double.IsFinite(temperatureCelsius) || temperatureCelsius <= -273.15)
            {
                throw new ArgumentOutOfRangeException(nameof(temperatureCelsius), "Temperature must be finite and above absolute zero.");
            }

            if (pressureHectopascals == 0.0 || altitudeDegrees == 90.0)
            {
                return 0.0;
            }

            // Bennett's approximation becomes singular near the zenith; taper its value at
            // 85 degrees linearly to zero at the zenith instead of extrapolating through it.
            double modelAltitude = Math.Min(altitudeDegrees, 85.0);

            double argument = (modelAltitude + 10.3 / (modelAltitude + 5.11)) * (Math.PI / 180.0);

            double correctionArcMinutes = 1.02 / Math.Tan(argument);

            double correctionDegrees = correctionArcMinutes / 60.0
                * pressureHectopascals / 1010.0
                * 283.0 / (273.0 + temperatureCelsius);

            if (altitudeDegrees > 85.0)
            {
                correctionDegrees *= (90.0 - altitudeDegrees) / 5.0;
            }

            return Math.Max(0.0, correctionDegrees);
        }
    }
}
