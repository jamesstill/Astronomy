using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.CelestialObjects.Moons
{
    public class MoonPhase
    {
        private Moment _moment;
        private string _phaseName;

        public MoonPhase(Moment moment, string phaseName)
        {
            _moment = moment;
            _phaseName = phaseName;
        }

        public Moment Moment { get { return _moment; } }
        public string PhaseName { get { return _phaseName; } }
    }
}