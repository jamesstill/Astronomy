namespace SquareWidget.Astronomy.Core.Models
{
    public class Satellite
    {
        public string Name { get; set; } = string.Empty;
        public double X { get; set; } = 0.0;
        public double Y { get; set; } = 0.0;

        public override string ToString()
        {
            string name = Name.PadLeft(8);
            string x = X.ToString("0.00").PadLeft(6);
            string y = Y.ToString("0.00").PadLeft(6);

            return $"{name} - X: {x}   Y: {y}";
        }
    }
}
