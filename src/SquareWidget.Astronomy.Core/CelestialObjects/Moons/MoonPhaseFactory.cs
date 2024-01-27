using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;
using System.Linq;

namespace SquareWidget.Astronomy.Core.CelestialObjects.Moons
{
    public static class MoonPhaseFactory
    {
        public static MoonPhase Create(string phaseName, Moment moment)
        {
            string[] phases = { PhaseName.NewMoon, PhaseName.FirstQuarter, PhaseName.FullMoon, PhaseName.LastQuarter };
            if (!phases.Contains(phaseName))
            {
                throw new ArgumentOutOfRangeException(nameof(phaseName), $"Unexpected phase name '{phaseName}'");
            }

            return new MoonPhase(moment, phaseName);
        }
    }
}
