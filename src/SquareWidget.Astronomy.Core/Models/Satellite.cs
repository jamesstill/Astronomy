using System.Globalization;

namespace SquareWidget.Astronomy.Core.Models
{
    /// <summary>
    /// A satellite is any body like a moon of Jupiter that has apparent rectangular coordinates 
    /// in a plane perpendicular to the line of sight from the observer to the planet. 
    /// </summary>
    public class Satellite
    {
        public string Name { get; set; } = string.Empty;
        public double X { get; set; } = 0.0;
        public double Y { get; set; } = 0.0;

        public override string ToString()
        {
            string name = (Name ?? string.Empty).PadLeft(8);
            string x = X.ToString("0.00", CultureInfo.InvariantCulture).PadLeft(6);
            string y = Y.ToString("0.00", CultureInfo.InvariantCulture).PadLeft(6);

            return $"{name} - X: {x}   Y: {y}";
        }
    }
}
