using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders
{
    internal class VenusOrbitalElementsBuilder : BaseOrbitalElementsBuilder
    {
        public VenusOrbitalElementsBuilder(Planet planet) : base(planet) { }

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

        protected override double GetOrbitalPeriod() => 0.615197;

        protected override Degrees GetMeanLongitude()
        {
            double a0 = 181.979801;
            double a1 = 58519.2130302;
            double a2 = 0.00031014;
            double a3 = 0.000000015;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override AstronomicalUnits GetLengthOfSemiMajorAxis() => new AstronomicalUnits(0.723329820);

        protected override double GetOrbitalEccentricity()
        {
            double a0 = 0.00677192;
            double a1 = -0.000047765;
            double a2 = 0.0000000981;
            double a3 = 0.00000000046;

            return CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
         }

        protected override Degrees GetInclinationOfPlaneOfEcliptic()
        {
            double a0 = 3.394662;
            double a1 = 0.0010037;
            double a2 = -0.00000088;
            double a3 = -0.000000007;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }

        protected override Degrees GetLongitudeOfAscendingNode()
        {
            double a0 = 76.679920;
            double a1 = 0.9011206;
            double a2 = 0.00040618;
            double a3 = -0.000000093;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }

        protected override Degrees GetLongitudeOfPerihelion()
        {
            double a0 = 131.563703;
            double a1 = 1.4022288;
            double a2 = -0.00107618;
            double a3 = -0.000005678;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }
    }
}
