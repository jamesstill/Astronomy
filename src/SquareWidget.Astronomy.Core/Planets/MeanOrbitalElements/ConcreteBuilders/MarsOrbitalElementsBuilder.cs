using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;
using System.Threading;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders
{
    internal class MarsOrbitalElementsBuilder : BaseOrbitalElementsBuilder
    {
        public MarsOrbitalElementsBuilder(Planet planet) : base(planet) { }

        public override OrbitalElements Create()
        {
            return new OrbitalElements()
            {
                P = GetOrbitalPeriod(),
                L = GetMeanLongitude(),
                a = GetLengthOfSemiMajorAxis(),
                e = GetOrbitalEccentricity(),
                ι = GetInclinationOfPlaneOfEcliptic(),
                Ω = GetLongitudeOfAscendingNode(),
                π = GetLongitudeOfPerihelion()
            };
        }

        protected override double GetOrbitalPeriod() => 1.880848;

        protected override Degrees GetMeanLongitude()
        {
            double a0 = 355.433000;
            double a1 = 19141.6964471;
            double a2 = 0.00031052;
            double a3 = 0.000000016;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }

        protected override AstronomicalUnits GetLengthOfSemiMajorAxis() => new AstronomicalUnits(1.523679342);

        protected override double GetOrbitalEccentricity()
        {
            double a0 = 0.09340065;
            double a1 = 0.000090484;
            double a2 = -0.0000000806;
            double a3 = -0.00000000025;

            return CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
        }

        protected override Degrees GetInclinationOfPlaneOfEcliptic()
        {
            double a0 = 1.849726;
            double a1 = -0.0006011;
            double a2 = 0.00001276;
            double a3 = -0.000000007;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfAscendingNode()
        {
            double a0 = 49.558093;
            double a1 = 0.7720959;
            double a2 = 0.00001557;
            double a3 = 0.000002267;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfPerihelion()
        {
            double a0 = 336.060234;
            double a1 = 1.8410449;
            double a2 = 0.00013477;
            double a3 = 0.000000536;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value); 
        }
    }
}
