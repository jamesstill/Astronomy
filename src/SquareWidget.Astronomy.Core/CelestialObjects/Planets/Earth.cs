using SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements;
using SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets
{
    public class Earth : Planet
    {
        public Earth(Moment moment) : base(moment)
        {
            OrbitalElements = OrbitalElementsBuilder.Create(this);
            SphericalCoordinates = SphericalCoordinatesBuilder.Create(this);
        }

        /// <summary>
        /// Return the eccentricity of the Earth's orbit following Jean Meeus, Astronomical 
        /// Algorithms (25.4). Eccentricity is a dimensionless parameter in the interval [0, 1)  
        /// </summary>
        public double OrbitalEccentricity
        {
            get
            {
                double T = Moment.T;
                double e = 0.016708634 - (0.000042037 * T) - (0.0000001267 * T * T);
                return e;
            }
        }

        /// <summary>
        /// Mean obliquity of the ecliptic per Meeus (22.2)
        /// </summary>
        public SexigesimalAngle MeanObliquity
        {
            get
            {
                double T = Moment.T;
                SexigesimalAngle a1 = new(23, 26, 21.448);
                SexigesimalAngle a2 = new(0, 0, 46.8150);
                SexigesimalAngle a3 = new(0, 0, 0.00059);
                SexigesimalAngle a4 = new(0, 0, 0.001813);

                double e0 = a1 - a2 * T - a3 * T * T + a4 * T * T * T;
                return new SexigesimalAngle(e0);
            }
        }
    }
}
