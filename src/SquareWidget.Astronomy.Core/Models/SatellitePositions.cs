using System;
using System.Collections.Generic;
using System.Text;

namespace SquareWidget.Astronomy.Core.Models
{
    public class SatellitePositions
    {
        public DateTime Date { get; set; }
        public List<Satellite> Satellites { get; set; } = new();

        public override string ToString()
        {
            if (Satellites.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new();
            sb.AppendLine(Date.ToString("g"));

            foreach(var satellite in Satellites)
            {
                sb.AppendLine(satellite.ToString());
            }

            return sb.ToString();
        }
    }
}
