using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct for right ascension (α) in hours, minutes, and seconds (HMS).
    /// </summary>
    public readonly struct RightAscension
    {
        public RightAscension(int hours, int minutes, double seconds)
        {
            if (hours < 0 || hours > 23)
            {
                throw new ArgumentException("Hours must be between 0 and 23", nameof(hours));
            }

            if (minutes < 0 || minutes > 59)
            {
                throw new ArgumentException("Minutes must be between 0 and 59", nameof(minutes));
            }

            if (seconds < 0 || seconds >= 60)
            {
                throw new ArgumentException("Seconds must be between 0 and less than 60", nameof(seconds));
            }

            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public RightAscension(double decimalDegrees)
        {
            double normalized = new Degrees(decimalDegrees).ToReducedAngle();
            Hours = ToHours(normalized);
            Minutes = ToMinutes(normalized);
            Seconds = ToSeconds(normalized);
        }

        public double Hours { get; }
        public double Minutes { get; }
        public double Seconds { get; }

        public double ToDecimalDegrees() => (Hours + (Minutes / 60.0) + (Seconds / 3600.0)) * 15.0;

        public Degrees ToDegrees() => new Degrees(ToDecimalDegrees());

        public override string ToString()
        {
            int h = (int)Hours;
            int m = (int)Minutes;
            double ss = Math.Round(Seconds, 6);

            if (ss >= 60.0)
            {
                ss = 0.0;
                m += 1;

                if (m >= 60)
                {
                    m = 0;
                    h = (h + 1) % 24;
                }
            }

            return $"{h}h {m}m {ss}s";
        }

        private static int ToHours(double d) => (int)Math.Floor(d / 15.0);

        private static int ToMinutes(double d)
        {
            int h = ToHours(d);
            return (int)Math.Floor(((d / 15.0) - h) * 60.0);
        }

        private static double ToSeconds(double d)
        {
            int h = ToHours(d);
            int m = ToMinutes(d);
            return ((((d / 15.0) - h) * 60.0) - m) * 60.0;
        }
    }
}