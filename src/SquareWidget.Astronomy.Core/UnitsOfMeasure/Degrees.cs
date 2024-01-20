using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct to hold decimal degrees (+/-58°.4168)
    /// </summary>
    public readonly struct Degrees
    {
        private readonly double value;

        public Degrees(double value)
        {
            this.value = value;   
        }

        public Degrees(Radians radians)
        {
            this.value = radians.ToDegrees();
        }

        public Degrees(SexigesimalAngle angle)
        {
            value = angle;
        }

        public static implicit operator double(Degrees d) => d.value;
        public static explicit operator Degrees(double value) => new Degrees(value);

        public Radians ToRadians()
        {
            return new Radians(value * Math.PI / 180.0);
        }

        public Degrees ToReducedAngle()
        {
            double d = this.value;
            d %= 360;
            if (d < 0)
            {
                d += 360;
            }

            return new Degrees(d);
        }

        /// <summary>
        /// Given a decimal degree like 58.4168 return as 58°.4168
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FormatDegree(value);
        }

        /// <summary>
        /// Given a decimal degree like 58.4168 return as 58°.4168
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string FormatDegree(double d)
        {
            string s = d.ToString();
            int pos = s.IndexOf('.');
            return s.Insert(pos, "°");
        }
    }
}
