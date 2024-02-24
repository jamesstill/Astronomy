using System;
using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct to calculate and hold sidereal time
    /// </summary>
    public readonly struct SiderealTime
    {
        private readonly Moment _moment;

        /// <summary>
        /// Constructor for the sidereal time
        /// </summary>
        /// <param name="moment">Moment in time</param>
        public SiderealTime(Moment moment)
        {
            _moment = moment;
        }

        //public static implicit operator double(SiderealTime s) => s.value;

        /// <summary>
        /// Return the Greenwich Mean Sidereal Time (GMST).
        /// </summary>
        /// <returns>GMST</returns>
        public Degrees GreenwichMean
        {
            get
            {
                double JD = _moment.JDE;
                double T = _moment.T;

                double result = 280.46061837 + 360.98564736629 * (JD - 2451545.0) + (0.000387933 * T * T) - (T * T * T / 38710000.0);
                return new Degrees(result).ToReducedAngle();
            }
        }

        /// <summary>
        /// Return the Greenwich Apparent Sidereal Time (GAST)
        /// </summary>
        public Degrees GreenwichApparent
        {
            get
            {
                Nutation n = NutationCalculator.Calculate(_moment.ToDateTime());
                double correction = n.ΔΨ * Math.Cos(n.ε.ToRadians()) / 15;
                double result = GreenwichMean + correction;
                return new Degrees(result).ToReducedAngle();
            }
        }

        /// <summary>
        /// Given the observer's longitude (L), return the observer's Local Mean Sidereal Time (LMST).
        /// </summary>
        /// <param name="L">observer's longitude</param>
        /// <returns></returns>
        public RightAscension ToLocalMean(SexigesimalAngle L)
        {
            double result = GreenwichMean + L;
            return new RightAscension(result);
        }

        /// <summary>
        /// Given the observer's longitude (L), return the observer's Local Apparent Sidereal Time (LAST)
        /// </summary>
        /// <param name="L"></param>
        /// <returns></returns>
        public RightAscension ToLocalApparent(SexigesimalAngle L)
        {
            double result = GreenwichApparent + L;
            return new RightAscension(result);
        }

        /// <summary>
        /// Given the observer's longitude (L) and the object's apparent equatorial coordinate for 
        /// right ascension (α), return the local mean hour angle (H) of the observer's position.
        /// H = GMST - L - α or H = LST - α
        /// </summary>
        /// <param name="L"></param>
        /// <param name="α"></param>
        /// <returns></returns>
        public Degrees ToHourAngle(SexigesimalAngle L, RightAscension α)
        {
            double h = GreenwichMean - L.ToDecimalDegrees() - α.ToDecimalDegrees();
            if (h < 0)
            {
                h += 360.0;
            }

            return new(h);
        }
    }
}
