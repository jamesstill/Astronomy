using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    /// <summary>
    /// See: https://theskylive.com/planets as well as JPL Horizons
    /// </summary>
    public class GeocentricPositionTests
    {
        double tolerance = 0.001;

        [Fact]
        public void Test_Venus_Geocentric_Position_Calculator_1992_Dec_20()
        {
            DateTime datetime = new(1992, 12, 20);

            GeocentricPosition g = GeocentricPositionCalculator.Calculate(datetime, PlanetName.Venus);

            EquitorialCoordinates eqc = g.EquitorialCoordinates;

            RightAscension α = eqc.α;
            SexigesimalAngle δ = eqc.δ;

            Assert.Equal(0.9108459564347563, g.Δt, 6);
            Assert.Equal(0.9109477126948372, g.Δa, 6);

            Assert.Equal(21, α.Hours);
            Assert.Equal(4, α.Minutes);
            Assert.Equal(42.463759, α.Seconds, tolerance);

            Assert.True(δ.IsNegative);
            Assert.Equal(18, δ.Degrees);
            Assert.Equal(53, δ.Minutes);
            Assert.Equal(12.071851, δ.Seconds, tolerance);
        }

        [Fact]
        public void Test_Saturn_Rings_Position_Angle_1992_Dec_16()
        {
            DateTime datetime = new(1992, 12, 16);
            GeocentricPosition g = GeocentricPositionCalculator.Calculate(datetime, PlanetName.Saturn);
            Degrees P = SaturnRingPositionAngleCalculator.Calculate(g, datetime);

            Assert.Equal(6.7402, P, tolerance);
        }

        [Fact]
        public void Test_Saturn_Rings_Position_Angle_2024_Jan_15()
        {
            DateTime datetime = new(2024, 1, 15);
            GeocentricPosition g = GeocentricPositionCalculator.Calculate(datetime, PlanetName.Saturn);
            Degrees P = SaturnRingPositionAngleCalculator.Calculate(g, datetime);

            Assert.Equal(5.834080, P, tolerance);
        }  
    }
}