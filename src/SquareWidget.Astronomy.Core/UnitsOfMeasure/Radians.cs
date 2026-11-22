using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct to hold radians (e.g., +/-1.019565498).
    /// </summary>
    public readonly struct Radians
    {
        private readonly double value;

        /// <summary>
        /// Initialize with a value in radians.
        /// </summary>
        /// <param name="value">Value in radians, not decimal degrees.</param>
        public Radians(double value)
        {
            this.value = value;
        }

        public Radians(Degrees degrees)
        {
            this.value = degrees.ToRadians();
        }

        public static implicit operator double(Radians r) => r.value;
        public static explicit operator Radians(double value) => new Radians(value);
        public override string ToString() => $"{value}";

        public Degrees ToDegrees()
        {
            return new Degrees(value * 180.0 / Math.PI);
        }

        public Radians ToReducedAngle()
        {
            const double twoPi = 2.0 * Math.PI;
            double r = value % twoPi;
            if (r < 0)
            {
                r += twoPi;
            }

            // Avoid negative zero (-0.0)
            if (r == 0)
            {
                r = 0;
            }

            return new Radians(r);
        }
    }
}