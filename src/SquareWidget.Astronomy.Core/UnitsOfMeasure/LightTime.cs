using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    public readonly struct LightTime
    {
        private readonly TimeSpan value;

        public double TotalDays
        {
            get
            {
                return value.TotalDays;
            }
        }

        public static implicit operator TimeSpan(LightTime lt) => lt.value;

        /// <summary>
        /// Given an existing distance d in AU return the time it takes for 
        /// light to reach the Earth from the body at the distance Δ.
        /// </summary>
        /// <param name="distance"></param>
        public LightTime(AstronomicalUnits distance)
        {
            value = TimeSpan.FromDays(0.0057755183 * distance);
        }

        /// <summary>
        /// Two light times are considered equal if their TotalDays value
        /// is within a tolerance of 0.0000001.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(LightTime other)
        {
            double tolerance = Math.Abs(TotalDays * 0.0000001);
            return Math.Abs(TotalDays - other.TotalDays) <= tolerance;
        }
    }
}
