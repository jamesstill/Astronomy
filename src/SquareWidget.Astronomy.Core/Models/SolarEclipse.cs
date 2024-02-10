using System;

namespace SquareWidget.Astronomy.Core.Models
{
    /// <summary>
    /// A Solar eclipse occurrence at a particular moment in time
    /// </summary>
    public class SolarEclipse
    {
        public SolarEclipse(DateOnly date, TimeOnly time, string eclipseType, double magnitude, double gamma)
        {
            Date = date;
            Time = time;
            EclipseType = eclipseType;
            Magnitude = magnitude;
            Gamma = gamma;
        }

        /// <summary>
        /// Eclipse Date
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// UTC Time of Greatest Eclipse when the axis of the moon's 
        /// shadow cone passes closest to Earth's center.
        /// </summary>
        public TimeOnly Time { get; set; }

        /// <summary>
        /// Eclipse type can be Partial, Total, Annular, or Hybrid
        /// </summary>
        public string EclipseType { get; set; } = "Unknown";
        
        /// <summary>
        /// Magnitude of the Eclipse
        /// </summary>
        public double Magnitude { get; set; }
        
        /// <summary>
        /// Least distance from the ais of the Moon's shadow to the 
        /// center of the Earth in units of the Earth's equatorial radius.
        /// </summary>
        public double Gamma { get; set; }

        public override string ToString()
        {
            string magnitudeDisplay = string.Empty;
            if (EclipseType == "Partial")
            {
                magnitudeDisplay = Magnitude.ToString("0.0000");
            }

            string gammaDisplay = Gamma.ToString("0.0000");

            return $"" +
                $"{Date.ToString("MM-dd-yyyy")} " +
                $"{Time.ToString("HH:mm")} UTC  " +
                $"magnitude: {string.Format("{0,6}", magnitudeDisplay)} " +
                $"g: {string.Format("{0,7}", gammaDisplay)}" +
                $"   {EclipseType}";
        }
    }
}
