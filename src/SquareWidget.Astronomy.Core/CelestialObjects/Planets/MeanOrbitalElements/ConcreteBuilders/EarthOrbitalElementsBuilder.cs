using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders
{
    internal class EarthOrbitalElementsBuilder : BaseOrbitalElementsBuilder
    {
        public EarthOrbitalElementsBuilder(Planet planet) : base(planet) { }

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
            double a0 = 100.466457;
            double a1 = 36000.7698278;
            double a2 = 0.00030322;
            double a3 = 0.000000020;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }

        protected override AstronomicalUnits GetLengthOfSemiMajorAxis() => new AstronomicalUnits(1.000001018);

        protected override double GetOrbitalEccentricity()
        {
            double a0 = 0.01670863;
            double a1 = -0.000042037;
            double a2 = -0.0000001267;
            double a3 = 0.00000000014;

            return CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
        }

        protected override Degrees GetInclinationOfPlaneOfEcliptic()
        {
            double a0 = 0;
            double a1 = 0;
            double a2 = 0;
            double a3 = 0;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }

        protected override Degrees GetLongitudeOfAscendingNode()
        {
            double a0 = 0;
            double a1 = 0;
            double a2 = 0;
            double a3 = 0;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }

        protected override Degrees GetLongitudeOfPerihelion()
        {
            double a0 = 102.937348;
            double a1 = 1.7195366;
            double a2 = 0.00045688;
            double a3 = -0.000000018;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }
    }
}
