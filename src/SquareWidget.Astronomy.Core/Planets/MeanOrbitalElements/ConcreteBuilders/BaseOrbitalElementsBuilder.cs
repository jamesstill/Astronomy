using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders
{
    abstract class BaseOrbitalElementsBuilder : IOrbitalElementsBuilder
    {
        protected Moment Moment { get; set; }

        public BaseOrbitalElementsBuilder(Planet planet)
        {
            this.Moment = planet.Moment;
        }

        public virtual OrbitalElements Create()
        {
            throw new NotImplementedException("Implement a concrete builder and override this virtual method.");
        }

        protected abstract double GetOrbitalPeriod();
        
        protected abstract Degrees GetMeanLongitude();

        protected abstract AstronomicalUnits GetLengthOfSemiMajorAxis();

        protected abstract double GetOrbitalEccentricity();

        protected abstract Degrees GetInclinationOfPlaneOfEcliptic();

        protected abstract Degrees GetLongitudeOfAscendingNode();

        protected abstract Degrees GetLongitudeOfPerihelion();

        protected double CalculateOrbitalElementsPolynomials(double a0, double a1, double a2, double a3)
        {
            double t = Moment.T;
            double t2 = t * t;
            double t3 = t * t * t;

            return a0 + a1 * t + a2 * t2 + a3 * t3;
        }
    }
}
