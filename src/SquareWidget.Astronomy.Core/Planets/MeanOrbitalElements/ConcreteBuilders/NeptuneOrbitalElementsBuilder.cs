using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders
{
    internal class NeptuneOrbitalElementsBuilder : BaseOrbitalElementsBuilder
    {
        const double ORBITAL_PERIOD = 164.79132;

        public NeptuneOrbitalElementsBuilder(Planet planet) : base(planet) { }

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

        protected override double GetOrbitalPeriod() => 164.79132;

        protected override Degrees GetMeanLongitude()
        {
            double a0 = 304.348665;
            double a1 = 219.8833092;
            double a2 = 0.00030882;
            double a3 = 0.000000018;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override AstronomicalUnits GetLengthOfSemiMajorAxis() 
        {
            double a0 = 30.110386869;
            double a1 = -0.0000001663;
            double a2 = 0.00000000069;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, 0.0);
            return new AstronomicalUnits(0);
        }

        protected override double GetOrbitalEccentricity()
        {
            double a0 = 0.00945575;
            double a1 = 0.000006033;
            double a2 = 0.0000000000;
            double a3 = -0.00000000005;

            return CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
         }

        protected override Degrees GetInclinationOfPlaneOfEcliptic()
        {
            double a0 = 1.769953;
            double a1 = -0.0093082;
            double a2 = -0.00000708;
            double a3 = 0.000000027;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfAscendingNode()
        {
            double a0 = 131.784057;
            double a1 = 1.1022039;
            double a2 = 0.00025952;
            double a3 = -0.000000637;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value);
         }

        protected override Degrees GetLongitudeOfPerihelion()
        {
            double a0 = 48.120276;
            double a1 = 1.4262957;
            double a2 = 0.00038434;
            double a3 = 0.000000020;

            var value = CalculateOrbitalElementsPolynomials(a0, a1, a2, a3);
            return new Degrees(value); 
        }
    }
}
