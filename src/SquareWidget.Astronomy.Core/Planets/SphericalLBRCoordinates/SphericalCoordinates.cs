using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates
{
    public class SphericalCoordinates
    {
        /// <summary>
        /// Ecliptical longitude
        /// </summary>
        public Radians L { get; private set; }

        /// <summary>
        /// Ecliptical latitude
        /// </summary>
        public Radians B { get; private set; }

        /// <summary>
        /// Radius Vector (distance to Sun)
        /// </summary>
        public AstronomicalUnits R { get; private set; }

        public SphericalCoordinates() { }

        public SphericalCoordinates(Radians l, Radians b, AstronomicalUnits r)
        {
            L = l;
            B = b;
            R = r;
        }
    }
}

