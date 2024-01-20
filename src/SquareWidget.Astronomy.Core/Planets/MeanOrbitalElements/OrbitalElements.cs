using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements
{
    /// <summary>
    /// Orbital (Keplerian) elements of the planets referred to the ecliptic and the mean equinox of 
    /// the date using coefficients from Meeus 31.A. See: https://en.wikipedia.org/wiki/orbital_elements
    /// </summary>
    public class OrbitalElements
    {
        /// <summary>
        /// Orbital period in tropical years
        /// </summary>
        public double P { get; set; }

        /// <summary>
        /// Mean longitude
        /// </summary>
        public Degrees L { get; set; }

        /// <summary>
        /// Length of semimajor axis of the planet's orbit (AUs)
        /// </summary>
        public AstronomicalUnits a { get; set; }

        /// <summary>
        /// Orbital eccentricity is a dimensionless parameter in the interval [0, 1) 
        /// which is to say 0 <= e < 1 with 0 being a circle and 1 being a parabola.
        /// </summary>
        /// <returns>double in range [0, 1)</returns>
        public double e { get; set; }

        /// <summary>
        /// Inclination of the plane of the ecliptic
        /// </summary>
        /// <returns></returns>
        public Degrees ι { get; set; }

        /// <summary>
        /// Longitude of the ascending node
        /// </summary>
        /// <returns></returns>
        public Degrees Ω { get; set; }

        /// <summary>
        /// Longitude of the perihelion
        /// </summary>
        /// <returns></returns>
        public Degrees π { get; set; }

        /// <summary>
        /// Mean anomaly of the planet (M)
        /// </summary>
        public Degrees M
        {
            get
            {
                return new Degrees(L - π);
            }
        }

        /// <summary>
        /// Argument of the Perihilion (ω)
        /// </summary>
        public Degrees ω
        {
            get
            {
                return new Degrees(π - Ω);
            }
        }

        /// <summary>
        /// Given e and M solve Kepler's Equation iteratively to get eccentric anomaly E
        /// </summary>
        /// <returns></returns>
        public Radians E
        {
            get
            {
                double tolerance = 0.0001; // tolerance for equality

                double m = M.ToRadians();
                double E0 = new Radians(M);
                double E1 = 0;

                bool areEqual = false;

                while (!areEqual)
                {
                    E1 = E0 + (m + e * Math.Sin(E0) - E0) / (1 - e * Math.Cos(E0));

                    double difference = Math.Abs(E1 - E0);

                    E0 = E1;

                    areEqual = difference <= tolerance;
                }

                return new Radians(E1);
            }
        }

        /// <summary>
        /// True anomaly (ν) expressed as a series in terms of e and E via W.M. Smart, Textbook on 
        /// Spherical Astronomy, pp. 118-119 (formula 85). The wiki demonstrates a Fourier expansion at 
        /// https://en.wikipedia.org/wiki/True_anomaly which is also in Smart, p. 120 (Formula 87).
        /// </summary>
        /// <returns></returns>
        public Radians v
        {
            get
            {
                double e2 = e * e;
                double e3 = e * e * e;

                double d = E + (e + 0.25 * e3) * Math.Sin(E) + 0.25 * e2 * Math.Sin(2 * E) + 0.083333 * e3 * Math.Sin(3 * E);

                return new Radians(d);
            }
        }
    }
}
