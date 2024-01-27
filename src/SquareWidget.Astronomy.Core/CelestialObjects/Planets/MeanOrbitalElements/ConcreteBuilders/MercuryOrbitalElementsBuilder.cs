using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders
{
    internal class MercuryOrbitalElementsBuilder : BaseOrbitalElementsBuilder
    {
        public MercuryOrbitalElementsBuilder(Planet planet) : base(planet) { }

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

        protected override double GetOrbitalPeriod() => 0.240847;

        protected override Degrees GetMeanLongitude()
        {
            double a0 = 252.250906;
            double a1 = 149474.0722491;
            double a2 = 0.00030350;
            double a3 = 0.000000018;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }

        protected override AstronomicalUnits GetLengthOfSemiMajorAxis() => new AstronomicalUnits(0.387098310);

        protected override double GetOrbitalEccentricity()
        {
            double a0 = 0.20563175;
            double a1 = 0.000020407;
            double a2 = -0.0000000283;
            double a3 = -0.00000000018;

            return CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
        }

        protected override Degrees GetInclinationOfPlaneOfEcliptic()
        {
            double a0 = 7.004986;
            double a1 = 0.0018215;
            double a2 = -0.00001810;
            double a3 = 0.000000056;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }

        protected override Degrees GetLongitudeOfAscendingNode()
        {
            double a0 = 48.330893;
            double a1 = 1.1861883;
            double a2 = 0.00017542;
            double a3 = 0.000000215;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
        }

        protected override Degrees GetLongitudeOfPerihelion()
        {
            double a0 = 77.456119;
            double a1 = 1.5564776;
            double a2 = 0.00029544;
            double a3 = 0.000000009;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value); 
        }
    }
}
