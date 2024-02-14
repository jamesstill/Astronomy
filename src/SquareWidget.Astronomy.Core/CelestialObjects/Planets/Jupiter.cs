using System;
using SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements;
using SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets
{
    public class Jupiter : Planet
    {
        public Jupiter(Moment moment) : base(moment) 
        {
            OrbitalElements = OrbitalElementsBuilder.Create(this);
            SphericalCoordinates = SphericalCoordinatesBuilder.Create(this);
        }

        /// <summary>
        /// Long-period term in the motion of Jupiter
        /// </summary>
        public Degrees LongPeriodTerm
        {
            get
            {
                double V = 172.74 + 0.00111588 * Moment.DayD;
                return new Degrees(V);
            }
        }

        /// <summary>
        /// Mean anomaly of Jupiter
        /// </summary>
        public Degrees MeanAnomaly
        {
            get
            {
                Radians r = LongPeriodTerm.ToRadians();
                double N = 20.020 + 0.0830853 * Moment.DayD + 0.329 * Math.Sin(r);
                return new Degrees(N).ToReducedAngle();
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
                double C = 5.555 * Math.Sin(M) + 0.168 * Math.Sin(2 * M);
                return new Degrees(C);
            }
        }

        /// <summary>
        /// Radius Vector of Jupiter
        /// </summary>
        public AstronomicalUnits RadiusVector
        {
            get
            {
                Radians M = MeanAnomaly.ToRadians();
                double r = 5.20872 - 0.25208 * Math.Cos(M) - 0.00611 * Math.Cos(2 * M);
                return new AstronomicalUnits(r);
            }
        }
    }
}
