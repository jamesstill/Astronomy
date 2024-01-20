using SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements;
using SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;

namespace SquareWidget.Astronomy.Core.Planets
{
    public class Earth : Planet
    {
        public Earth(Moment moment) : base(moment)
        {
            OrbitalElements = OrbitalElementsBuilder.Create(this);
            SphericalCoordinates = SphericalCoordinatesBuilder.Create(this);
        }
    }
}
