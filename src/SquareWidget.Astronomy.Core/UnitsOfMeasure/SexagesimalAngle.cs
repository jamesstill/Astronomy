using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct for a sexigesimal angle in degrees, arcminutes, and arcseconds (DMS)
    /// </summary>
    public struct SexigesimalAngle
    {
        private readonly double value;
        private bool negative;
        private int d;
        private int m;
        private double s;

        /// <summary>
        /// Constructor for decimal degree (e.g., +58.255) build angle
        /// </summary>
        /// <param name="dd"></param>
        public SexigesimalAngle(double dd) : this()
        {
            int precision = 8;
            value = dd;

            negative = dd < 0;
            double degrees = Math.Abs(dd);
            d = (int)Math.Floor(degrees);

            degrees -= d;
            degrees *= 60.0;

            m = (int)Math.Floor(degrees);

            degrees -= m;
            degrees *= 60.0;

            s = Math.Round(degrees, precision);
        }

        /// <summary>
        /// Constructor for degrees, minutes, seconds (DMS)
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        public SexigesimalAngle(int degrees, int minutes, double seconds) : this()
        {
            int sign = SignOf(degrees, minutes, seconds);
            negative = sign == 0;

            d = Math.Abs(degrees);
            m = Math.Abs(minutes);
            s = Math.Abs(seconds);
            value = ToDegrees(); // implicit conversion to decimal degrees
        }

        public static implicit operator double(SexigesimalAngle a) => a.value;

        public double ToDecimalDegrees() => value;

        public readonly bool IsNegative
        {
            get { return negative; }
        }

        public readonly int Degrees
        {
            get { return d; }
        }

        public readonly int Minutes
        {
            get { return m; }
        }

        public readonly double Seconds
        {
            get { return s; }
        }

        public readonly Radians ToRadians()
        {
            double r = value * (Math.PI / 180.0);
            return new Radians(r);
        }

        public readonly Degrees ToDegrees()
        {
            double dd = d + (m / 60.0) + (s / 3600.0);
            if (IsNegative)
            {
                dd = -dd;
            }

            return new Degrees(dd);
        }

        public override readonly string ToString()
        {
            string sign = negative ? "-" : "+";

            if (d == 0 && m == 0)
            {
                return $"{sign}{s}\"";
            }

            if (d == 0 && m > 0)
            {
                return $"{sign}{m}' {s}\"";
            }

            if (d == 0 && m != 0 && s != 0)
            {
                return $"{sign}{s}\"";
            }

            return $"{sign}{d}° {m}' {s}\"";
        }

        /// <summary>
        /// Determine whether DMS is negative or positive by evaluating the first non-zero term.
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private int SignOf(double degrees, double minutes, double seconds)
        {
            double term;

            if (degrees == 0.0 && minutes == 0.0 && seconds != 0.0)
            {
                term = seconds;
            }
            else if (degrees == 0.0 && minutes != 0.0)
            {
                term = minutes;
            }
            else
            {
                term = degrees;
            }

            return (term.GetHashCode() >> 63) + 1;
        }

    }
}
