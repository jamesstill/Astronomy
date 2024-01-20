using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;

namespace SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates.ConcreteBuilders
{
    internal class BaseSphericalCoordinatesBuilder : ISphericalCoordinatesBuilder
    {
        protected Moment Moment { get; set; }

        protected double T {  get
            {
                // VSOP planetary theory uses Julian millennia
                return (Moment.JDE - 2451545) / 365250;
            } 
        }

        public BaseSphericalCoordinatesBuilder(Planet planet)
        {
            this.Moment = planet.Moment;
        }

        public virtual SphericalCoordinates Create()
        {
            throw new NotImplementedException("Implement a concrete builder and override this virtual method.");
        }
    }
}
