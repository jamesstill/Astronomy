using System;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;


namespace SquareWidget.Astronomy.Core.CelestialObjects.Moons
{
    public class Moon
    {
        private Moment _moment;

        public Moon(Moment moment)
        {
            _moment = moment;
        }

        /// <summary>
        /// Baseline value k where k = 0 is 6 Jan 2000 (first new moon of J2000.0 epoch).
        /// </summary>
        /// <returns></returns>
        public int FirstNewMoonOfJ2000Epoch
        {
            get 
            {
                double decimalYear = (_moment.ToDecimalYear() - 2000) * 12.3685;
                return (int)Math.Floor(decimalYear);
            }
        }

        /// <summary>
        /// Moon's mean anomaly at time JDE
        /// </summary>
        /// <returns>M′</returns>
        public Degrees MeanAnomaly
        {
            get
            {
                int k = FirstNewMoonOfJ2000Epoch;
                double T = _moment.T;

                // Meeus (49.5)
                double M = 201.5643 +
                    (385.81693528 * k) +
                    (0.0107582 * (T * T)) +
                    (0.00001238 * (T * T * T)) -
                    (0.000000058 * (T * T * T * T));

                return new Degrees(M).ToReducedAngle();
            }
        }

        /// <summary>
        /// Moon's argument of latitude
        /// </summary>
        /// <returns>F</returns>
        public Degrees ArgumentOfLatitude
        {
            get {
                int k = FirstNewMoonOfJ2000Epoch;
                double T = _moment.T;

                double F = 160.7108 +
                    (390.67050284 * k) -
                    (0.0016118 * (T * T)) -
                    (0.00000227 * (T * T * T)) +
                    (0.000000011 * (T * T * T * T));

                return new Degrees(F).ToReducedAngle();
            }
        }

        /// <summary>
        /// Longitude of the ascending node of the lunar orbit
        /// </summary>
        /// <returns>Ω</returns>
        public Degrees LongitudeOfAscendingNode
        {
            get
            {
                int k = FirstNewMoonOfJ2000Epoch;
                double T = _moment.T;

                double L = 124.7746 -
                    (1.56375588 * k) +
                    (0.0020672 * (T * T)) +
                    (0.00000215 * (T * T * T));

                return new Degrees(L).ToReducedAngle();
            }
        }
    }
}
