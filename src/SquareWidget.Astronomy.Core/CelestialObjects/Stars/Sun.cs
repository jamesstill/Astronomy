using System;
using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.CelestialObjects.Stars
{
    public class Sun
    {
        private Moment _moment;

        public Sun(Moment moment)
        {
            _moment = moment;
        }

        /// <summary>
        /// Geometric mean longitude of the Sun
        /// </summary>
        /// <returns></returns>
        public Degrees MeanLongitude
        {
            get
            {
                double T = _moment.T;
                double L = 280.46646 + (36000.76983 * T) + (0.0003032 * Math.Pow(T, 2));
                return new Degrees(L).ToReducedAngle();
            }
        }

        /// <summary>
        /// Mean anomaly angle of the Sun 
        /// </summary>
        /// <returns></returns>
        public Degrees MeanAnomaly
        {
            get
            {
                double T = _moment.T;
                double M = 357.52911 + (35999.05029 * T) - (0.0001537 * Math.Pow(T, 2));
                return new Degrees(M).ToReducedAngle();
            }
        }

        /// <summary>
        /// Equation of Center of the Sun
        /// </summary>
        /// <returns></returns>
        public Degrees EquationOfCenter
        {
            get
            {
                double T = _moment.T;
                Radians M = MeanAnomaly.ToRadians();

                // Sun's equation of center
                double C =
                    +(1.914602 - (0.004817 * T)
                    - (0.000014 * T * T)) * Math.Sin(M)
                    + (0.019993 - (0.000101 * T)) * Math.Sin(M * 2)
                    + (0.000289 * Math.Sin(M * 3));

                return new Degrees(C);
            }
        }

        /// <summary>
        /// True geometric longitude of the Sun
        /// </summary>
        public Degrees TrueLongitude
        {
            get
            {
                double L = (MeanLongitude + EquationOfCenter);
                return new Degrees(L).ToReducedAngle();
            }
        }

        /// <summary>
        /// True anomaly of the Sun
        /// </summary>
        public Degrees TrueAnomaly
        {
            get
            {
                double v = (MeanAnomaly + EquationOfCenter);
                return new Degrees(v).ToReducedAngle();
            }
        }

        /// <summary>
        /// Radius vector of the sun per algorithm from U.S. Naval Observatory
        /// </summary>
        public AstronomicalUnits RadiusVector
        {
            get
            {
                Radians M = MeanAnomaly.ToRadians();
                double R = 1.00014 - 0.01671 * Math.Cos(M) - 0.00014 * Math.Cos(2 * M);
                return new AstronomicalUnits(R);
            }
        }

        /// <summary>
        /// Return the geocentric position of the Sun referred to the mean equinox 
        /// of the date per Jean Meeus, Astronomical Algorithms, 2nd Ed, Chap 25.
        /// </summary>
        /// <returns></returns>
        public EquitorialCoordinates GetGeocentricPosition()
        {
            double T = _moment.T;

            // correction for nutation and aberration
            Degrees O = new(125.04 - (1934.136 * T));
            Radians Ω = new(O);

            // apparent longitude (λ) of the Sun
            Degrees L = new(TrueLongitude - 0.00569 - (0.00478 * Math.Sin(Ω)));
            Radians λ = new(L);

            Earth earth = new(_moment);
            double e0 = earth.MeanObliquity.ToDecimalDegrees();

            // correction for parallax (25.8)
            Degrees e = new(e0 + 0.00256 * Math.Cos(Ω));
            Radians ε = new(e);

            // Sun's right ascension a and declination d
            Radians a = new(Math.Atan2(Math.Cos(ε) * Math.Sin(λ), Math.Cos(λ)));
            Radians d = new(Math.Asin(Math.Sin(ε) * Math.Sin(λ)));

            // final solar coordinates
            RightAscension ra = new RightAscension(a.ToDegrees().ToReducedAngle());
            SexigesimalAngle dec = new(d.ToDegrees());

            return new EquitorialCoordinates(dec, ra, e);
        }
    }
}
