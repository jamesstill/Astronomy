using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;
using System.Globalization;

namespace SquareWidget.Astronomy.Core.Models
{

	/// <summary>
	/// Sundial hour angle for a given latitude and hour.
	/// </summary>
	public readonly struct SundialAngle
	{
		public int Hour { get; }
		public Degrees Angle { get; }

		public SundialAngle(int hour, Degrees angle)
		{
			Hour = hour;
			Angle = angle;
		}

		public override string ToString()
		{
			int h = Hour == 0 ? 12 : (Hour > 12) ? (Hour - 12) : Hour;
			string period = GetHourPeriod(Hour);
			double truncatedAngle = Math.Truncate(Angle * 100) / 100;
			if (truncatedAngle == 0)
			{
				truncatedAngle = 0;
			}

			string angle = string.Format(CultureInfo.InvariantCulture, "{0:N2}", truncatedAngle);
			return $"{h} {period}: {angle}";
		}

        private string GetHourPeriod(int h)
        {
            return h switch
            {
                > 12 => "PM",
                < 12 => "AM",
                _ => "Solar Noon"
            };
        }
    }
}
