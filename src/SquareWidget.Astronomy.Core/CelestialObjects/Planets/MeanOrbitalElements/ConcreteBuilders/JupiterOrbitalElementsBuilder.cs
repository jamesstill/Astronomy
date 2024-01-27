using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders
{
    internal class JupiterOrbitalElementsBuilder : BaseOrbitalElementsBuilder
    {
        public JupiterOrbitalElementsBuilder(Planet planet) : base(planet) { }

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

        protected override double GetOrbitalPeriod() => 11.862615;

        protected override Degrees GetMeanLongitude()
        {
            double a0 = 34.351519;
            double a1 = 3036.3027748;
            double a2 = 0.00022330;
            double a3 = 0.000000037;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override AstronomicalUnits GetLengthOfSemiMajorAxis() 
        {
            double a0 = 5.202603209;
            double a1 = 0.0000001913;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, 0.0, 0.0);
            return new AstronomicalUnits(0);
        }

        protected override double GetOrbitalEccentricity()
        {
            double a0 = 0.04849793;
            double a1 = 0.000163225;
            double a2 = -0.0000004714;
            double a3 = -0.00000000201;

            return CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
         }

        protected override Degrees GetInclinationOfPlaneOfEcliptic()
        {
            double a0 = 1.303267;
            double a1 = -0.0054965;
            double a2 = 0.00000466;
            double a3 = -0.000000002;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfAscendingNode()
        {
            double a0 = 100.464407;
            double a1 = 1.0209774;
            double a2 = 0.00040315;
            double a3 = 0.000000404;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfPerihelion()
        {
            double a0 = 14.331207;
            double a1 = 1.6126352;
            double a2 = 0.00103042;
            double a3 = 00.000004464;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value); 
        }
    }
}
