using System;
using SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements.ConcreteBuilders;

namespace SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements
{
    internal class OrbitalElementsBuilder
    {
        public static OrbitalElements Create(Planet planet)
        {
            IOrbitalElementsBuilder builder;

            switch (planet.GetType().Name)
            {
                case PlanetName.Mercury:
                    builder = new MercuryOrbitalElementsBuilder(planet);
                    break;

                case PlanetName.Venus:
                    builder = new VenusOrbitalElementsBuilder(planet);
                    break;

                case PlanetName.Earth:
                    builder = new EarthOrbitalElementsBuilder(planet);
                    break;

                case PlanetName.Mars:
                    builder = new MarsOrbitalElementsBuilder(planet);
                    break;

                case PlanetName.Jupiter:
                    builder = new JupiterOrbitalElementsBuilder(planet);
                    break;

                case PlanetName.Saturn:
                    builder = new SaturnOrbitalElementsBuilder(planet);
                    break;

                case PlanetName.Uranus:
                    builder = new SaturnOrbitalElementsBuilder(planet);
                    break;

                case PlanetName.Neptune:
                    builder = new NeptuneOrbitalElementsBuilder(planet);
                    break;

                default:
                    throw new NotImplementedException("Unknown planet type passed into OrbitalElementsBuilder");
            }

            return builder.Create();
        }
    }
}
