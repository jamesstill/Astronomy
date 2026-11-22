using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct for a sexagesimal angle in degrees, arcminutes, and arcseconds (DMS).
    /// </summary>
    public readonly struct SexigesimalAngle
    {
        private readonly double value;

        public bool IsNegative { get; }
        public int Degrees { get; }
        public int Minutes { get; }
        public double Seconds { get; }

        /// <summary>
        /// Constructor for decimal degrees (e.g., +58.255).
        /// </summary>
        public SexigesimalAngle(double dd)
        {
            const int precision = 8;
            value = dd;
            IsNegative = dd < 0;

            double degrees = Math.Abs(dd);
            int d = (int)Math.Floor(degrees);

            degrees = (degrees - d) * 60.0;
            int m = (int)Math.Floor(degrees);

            degrees = (degrees - m) * 60.0;
            double s = Math.Round(degrees, precision);

            // Carry over 60-second / 60-minute rollovers caused by rounding
            if (s >= 60.0)
            {
                s = 0.0;
                m += 1;

                if (m >= 60)
                {
                    m = 0;
                    d += 1;
                }
            }

            Degrees = d;
            Minutes = m;
            Seconds = s;
        }

        /// <summary>
        /// Constructor for degrees, minutes, seconds (DMS).
        /// </summary>
        public SexigesimalAngle(int degrees, int minutes, double seconds)
        {
            IsNegative = IsNegativeSign(degrees, minutes, seconds);
            Degrees = Math.Abs(degrees);
            Minutes = Math.Abs(minutes);
            Seconds = Math.Abs(seconds);

            double dd = Degrees + (Minutes / 60.0) + (Seconds / 3600.0);
            value = IsNegative ? -dd : dd;
        }

        public static implicit operator double(SexigesimalAngle a) => a.value;

        public double ToDecimalDegrees() => value;

        public Radians ToRadians() => new Radians(value * (Math.PI / 180.0));

        public Degrees ToDegrees() => new Degrees(value);

        public override string ToString()
        {
            string sign = IsNegative ? "-" : "+";

            if (Degrees == 0 && Minutes == 0)
            {
                return $"{sign}{Seconds}\"";
            }

            if (Degrees == 0 && Minutes > 0)
            {
                return $"{sign}{Minutes}' {Seconds}\"";
            }

            return $"{sign}{Degrees}° {Minutes}' {Seconds}\"";
        }

        /// <summary>
        /// Determines whether DMS is negative by evaluating the first non-zero term.
        /// </summary>
        private static bool IsNegativeSign(int degrees, int minutes, double seconds)
        {
            if (degrees != 0) return degrees < 0;
            if (minutes != 0) return minutes < 0;
            return seconds < 0;
        }
    }
}