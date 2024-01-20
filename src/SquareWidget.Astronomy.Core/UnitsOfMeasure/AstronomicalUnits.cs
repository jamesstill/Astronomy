namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct to hold astronomical units (AU)
    /// </summary>
    public readonly struct AstronomicalUnits
    {
        private readonly double value;

        public AstronomicalUnits(double value)
        {
            this.value = value;
        }

        public static implicit operator double(AstronomicalUnits au) => au.value;
        public static explicit operator AstronomicalUnits(double value) => new AstronomicalUnits(value);
        public override string ToString() => $"{value}";

        public readonly double ToKilometers()
        {
            return value * 149597870.700;
        }
    }
}
