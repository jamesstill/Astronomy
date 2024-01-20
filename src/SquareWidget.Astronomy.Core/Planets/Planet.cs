using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements;
using SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates;

namespace SquareWidget.Astronomy.Core.Planets
{
    public class Planet
    {
        private Moment _moment;

        public Planet(Moment moment)
        {
            _moment = moment;
            this.OrbitalElements = new OrbitalElements();
            this.SphericalCoordinates = new SphericalCoordinates();
        }

        public Moment Moment
        {
            get
            {
                return _moment;
            }
            set
            {
                _moment = value;
                this.OrbitalElements = new OrbitalElements();
                this.SphericalCoordinates = new SphericalCoordinates();
            }
        }

        public OrbitalElements OrbitalElements { get; set; }

        public SphericalCoordinates SphericalCoordinates { get; set; }
    }
}
