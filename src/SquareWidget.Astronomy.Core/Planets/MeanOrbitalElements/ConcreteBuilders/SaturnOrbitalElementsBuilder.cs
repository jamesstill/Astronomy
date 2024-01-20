using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders
{
    internal class SaturnOrbitalElementsBuilder : BaseOrbitalElementsBuilder
    {
        public SaturnOrbitalElementsBuilder(Planet planet) : base(planet) { }

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

        protected override double GetOrbitalPeriod() => 29.447498;

        protected override Degrees GetMeanLongitude()
        {
            double a0 = 50.077444;
            double a1 = 1223.5110686;
            double a2 = 0.00051908;
            double a3 = -0.000000030;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override AstronomicalUnits GetLengthOfSemiMajorAxis() 
        {
            double a0 = 9.554909192;
            double a1 = -0.0000021390;
            double a2 = 0.000000004;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, 0.0);
            return new AstronomicalUnits(0);
        }

        protected override double GetOrbitalEccentricity()
        {
            double a0 = 0.05554814;
            double a1 = -0.000346641;
            double a2 = -0.0000006436;
            double a3 = 0.00000000340;

            return CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
         }

        protected override Degrees GetInclinationOfPlaneOfEcliptic()
        {
            double a0 = 2.488879;
            double a1 = -0.0037362;
            double a2 = -0.00001519;
            double a3 = 0.000000087;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfAscendingNode()
        {
            double a0 = 113.665503;
            double a1 = 0.8770880;
            double a2 = -0.00012176;
            double a3 = -0.000002249;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfPerihelion()
        {
            double a0 = 93.057237;
            double a1 = 1.9637613;
            double a2 = 0.00083753;
            double a3 = 0.000004928;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value); 
        }
    }
}
