using SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates.ConcreteBuilders;
using System.Diagnostics;

namespace SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates
{
    internal class SphericalCoordinatesBuilder
    {
        public static SphericalCoordinates Create(Planet planet)
        {
            ISphericalCoordinatesBuilder builder;

            switch(planet.GetType().Name)
            {
                case PlanetName.Mercury:
                    builder = new MercurySphericalCoordinatesBuilder(planet);
                    break;

                case PlanetName.Venus:
                    builder = new VenusSphericalCoordinatesBuilder(planet);
                    break;

                case PlanetName.Earth:
                    builder = new EarthSphericalCoordinatesBuilder(planet);
                    break;

                case PlanetName.Mars:
                    builder = new MarsSphericalCoordinatesBuilder(planet);
                    break;

                case PlanetName.Jupiter:
                    builder = new JupiterSphericalCoordinatesBuilder(planet);
                    break;

                case PlanetName.Saturn:
                    builder = new SaturnSphericalCoordinatesBuilder(planet);
                    break;

                case PlanetName.Uranus:
                    builder = new UranusSphericalCoordinatesBuilder (planet);
                    break;

                case PlanetName.Neptune:
                    builder = new NeptuneSphericalCoordinatesBuilder(planet);
                    break;

                default:
                    Trace.TraceError("Unknown planet type '{0}'", planet);
                    builder = new BaseSphericalCoordinatesBuilder(planet);
                    break;
            }

            return builder.Create();
        }
    }
}
