using System;
using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct to calculate and hold sidereal time.
    /// </summary>
    public readonly struct SiderealTime
    {
        private readonly Moment _moment;

        /// <summary>
        /// Constructor for the sidereal time.
        /// </summary>
        /// <param name="moment">Moment in time</param>
        public SiderealTime(Moment moment)
        {
            _moment = moment;
        }

        /// <summary>
        /// Return the Greenwich Mean Sidereal Time (GMST) in degrees [0, 360).
        /// </summary>
        public Degrees GreenwichMean
        {
            get
            {
                double JD = _moment.JDE;
                double T = _moment.T;

                double result = 280.46061837
                    + 360.98564736629 * (JD - 2451545.0)
                    + (0.000387933 * T * T)
                    - (T * T * T / 38710000.0);

                return new Degrees(result).ToReducedAngle();
            }
        }

        /// <summary>
        /// Return the Greenwich Apparent Sidereal Time (GAST) in degrees [0, 360).
        /// </summary>
        public Degrees GreenwichApparent
        {
            get
            {
                Nutation n = NutationCalculator.Calculate(_moment.ToDateTime());

                // ΔΨ is stored in decimal degrees, so the equation of the equinoxes is in degrees.
                double correction = n.ΔΨ * Math.Cos(n.ε.ToRadians());
                double result = GreenwichMean + correction;
                return new Degrees(result).ToReducedAngle();
            }
        }

        /// <summary>
        /// Given the observer's longitude (L), return the observer's Local Mean Sidereal Time (LMST).
        /// </summary>
        public RightAscension ToLocalMean(SexigesimalAngle L)
        {
            Degrees result = new Degrees(GreenwichMean + L).ToReducedAngle();
            return new RightAscension(result);
        }

        /// <summary>
        /// Given the observer's longitude (L), return the observer's Local Apparent Sidereal Time (LAST).
        /// </summary>
        public RightAscension ToLocalApparent(SexigesimalAngle L)
        {
            Degrees result = new Degrees(GreenwichApparent + L).ToReducedAngle();
            return new RightAscension(result);
        }

        /// <summary>
        /// Given the observer's longitude (L) and the object's apparent equatorial coordinate for
        /// right ascension (α), return the local mean hour angle (H) in degrees [0, 360).
        /// H = GMST - L - α
        /// </summary>
        public Degrees ToHourAngle(SexigesimalAngle L, RightAscension α)
        {
            double h = GreenwichMean - L.ToDecimalDegrees() - α.ToDecimalDegrees();
            return new Degrees(h).ToReducedAngle();
        }
    }
}