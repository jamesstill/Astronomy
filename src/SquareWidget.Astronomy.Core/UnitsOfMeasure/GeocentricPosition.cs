namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    public class GeocentricPosition
    {
        /// <summary>
        /// Geocentric ecliptical coordinates λ and β
        /// </summary>
        public EclipticalCoordinates EclipticalCoordinates { get; set; }

        /// <summary>
        /// Geocentric equitorial coordinates α and δ
        /// </summary>
        public EquitorialCoordinates EquitorialCoordinates { get; set; }

        /// <summary>
        /// True distance from Earth in AU
        /// </summary>
        public AstronomicalUnits Δt { get; set; }

        /// <summary>
        /// Apparent distance from Earth in AU after adjusting for planetary aberration
        /// </summary>
        public AstronomicalUnits Δa { get; set; }
    }
}
