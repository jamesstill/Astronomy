using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;

namespace SquareWidget.Astronomy.Core.Calculators
{
    /// <summary>
    /// Calcuates the geocentric (apparent) position of the Sun.
    /// </summary>
    public static class SolarLongitudeCalculator
    {
        /// <summary>
        /// Calculate center-to-center geocentric position of the Sun referred to the mean equinox of the date. 
        /// Algorithm follows Meeus, Astronomical Algorithms, (25.1) through (25.4).
        /// </summary>
        /// <returns></returns>
        public static Degrees Calculate(DateTime datetime)
        {
            Moment moment = new(datetime);
            double T = moment.T;

            // geometric mean longitude of the Sun referred to the mean equinox of T (25.2)
            Degrees L0 = new(280.46646 + (36000.76983 * T) + (0.0003032 * T * T));

            // mean anomaly of the Sun (25.3)
            Degrees M = new(357.52911 + (35999.05029 * T) - (0.0001537 * T * T));
            Radians Mr = M.ToRadians();

            // Sun's equation of center
            Degrees C = new(
                + (1.914602 - (0.004817 * T) - (0.000014 * T * T)) * Math.Sin(Mr)
                + (0.019993 - (0.000101 * T)) * Math.Sin(Mr * 2)
                + (0.000289 * Math.Sin(Mr * 3))
            );

            // Sun's true geometric longitude
            return new Degrees(L0 + C).ToReducedAngle();
        }
    }
}
