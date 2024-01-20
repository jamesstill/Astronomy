using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct to hold radians (e.g., +/-1.019565498)
    /// </summary>
    public readonly struct Radians
    {
        private readonly double value;

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
            double r = this.value;
            if (value < 0)
            {
                r = 2 * Math.PI - (Math.Abs(r) % (2 * Math.PI));
            }
            else if (r > 2 * Math.PI)
            {
                r = r % (2 * Math.PI);
            }

            return new Radians(r);
        }
    }
}
