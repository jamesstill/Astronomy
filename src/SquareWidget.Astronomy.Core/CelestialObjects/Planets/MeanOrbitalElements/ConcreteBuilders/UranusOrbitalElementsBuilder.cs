using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders
{
    internal class UranusOrbitalElementsBuilder : BaseOrbitalElementsBuilder
    {
        public UranusOrbitalElementsBuilder(Planet planet) : base(planet) { }

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

        protected override double GetOrbitalPeriod() => 84.016846;

        protected override Degrees GetMeanLongitude()
        {
            double a0 = 314.055005;
            double a1 = 429.8640561;
            double a2 = 0.00030390;
            double a3 = 0.000000026;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override AstronomicalUnits GetLengthOfSemiMajorAxis() 
        {
            double a0 = 19.218446062;
            double a1 = -0.0000000372;
            double a2 = 0.00000000098;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, 0.0);
            return new AstronomicalUnits(0);
        }

        protected override double GetOrbitalEccentricity()
        {
            double a0 = 0.04638122;
            double a1 = -0.000027293;
            double a2 = 0.0000000789;
            double a3 = 0.00000000024;

            return CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
         }

        protected override Degrees GetInclinationOfPlaneOfEcliptic()
        {
            double a0 = 0.773197;
            double a1 = 0.0007744;
            double a2 = 0.00003749;
            double a3 = -0.000000092;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfAscendingNode()
        {
            double a0 = 74.005957;
            double a1 = 0.5211278;
            double a2 = 0.00133947;
            double a3 = 0.000018484;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfPerihelion()
        {
            double a0 = 173.005291;
            double a1 = 1.4863790;
            double a2 = 0.00021406;
            double a3 = 0.000000434;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value); 
        }
    }
}
