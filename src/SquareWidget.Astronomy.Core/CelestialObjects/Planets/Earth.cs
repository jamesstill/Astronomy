using System;
using SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements;
using SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets
{
    public class Earth : Planet
    {
        public Earth(Moment moment) : base(moment)
        {
            OrbitalElements = OrbitalElementsBuilder.Create(this);
            SphericalCoordinates = SphericalCoordinatesBuilder.Create(this);
        }

        /// <summary>
        /// Mean anomaly of the Earth
        /// </summary>
        public Degrees MeanAnomaly
        {
            get
            {
                double M = 357.529 + 0.9856003 * Moment.DayD;
                return new Degrees(M).ToReducedAngle();
            }
        }

        /// <summary>
        /// Equation of Center
        /// </summary>
        public Degrees EquationOfCenter
        {
            get
            {
                Radians M = MeanAnomaly.ToRadians();
                double C = 1.915 * Math.Sin(M) + 0.020 * Math.Sin(2 * M);
                return new Degrees(C);
            }
        }


        /// <summary>
        /// Return the eccentricity (e) of the Earth's orbit following Jean Meeus, Astronomical 
        /// Algorithms (25.4). Eccentricity is a dimensionless parameter in the interval [0, 1)  
        /// </summary>
        public double OrbitalEccentricity
        {
            get
            {
                double T = Moment.T;
                double e = 0.016708634 - (0.000042037 * T) - (0.0000001267 * T * T);
                return e;
            }
        }

        /// <summary>
        /// Radius Vector of the Earth
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
        /// Mean obliquity of the ecliptic (ε) per Meeus (22.2)
        /// </summary>
        public SexigesimalAngle MeanObliquity
        {
            get
            {
                double T = Moment.T;
                double U = T / 100;
                double e0 =
                    new SexigesimalAngle(23, 26, 21.448)
                    - new SexigesimalAngle(0, 0, 4680.93) * U
                    - 1.55 * Math.Pow(U, 2)
                    + 1999.25 * Math.Pow(U, 3)
                    - 51.38 * Math.Pow(U, 4)
                    - 249.67 * Math.Pow(U, 5)
                    - 39.05 * Math.Pow(U, 6)
                    + 7.12 * Math.Pow(U, 7)
                    + 27.87 * Math.Pow(U, 8)
                    + 5.79 * Math.Pow(U, 9)
                    + 2.45 * Math.Pow(U, 10);

                return new SexigesimalAngle(e0);
            }
        }
    }
}
