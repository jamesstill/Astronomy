using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class JupiterSatellitePositionsCalculatorTests
    {
        [Fact]
        public void Test_Satellite_Positions_1992_12_16()
        {
            double tolerance = 0.01;

            DateTime datetime = new(1992, 12, 16);
            SatellitePositions positions = JupiterSatellitePositionCalculator.Calculate(datetime);
            var I = positions.Satellites.First(s => s.Name.Equals("Io"));
            var II = positions.Satellites.First(s => s.Name.Equals("Europa"));
            var III = positions.Satellites.First(s => s.Name.Equals("Ganymede"));
            var IV = positions.Satellites.First(s => s.Name.Equals("Callisto"));

            Assert.NotNull(positions);
            Assert.NotNull(I);
            Assert.NotNull(II);
            Assert.NotNull(III);
            Assert.NotNull(IV);
            Assert.Equal(DateOnly.FromDateTime(datetime), DateOnly.FromDateTime(positions.Date));
            Assert.Equal(-3.44, I.X, tolerance);
            Assert.Equal(0.21, I.Y, tolerance);
            Assert.Equal(7.44, II.X, tolerance);
            Assert.Equal(0.25, II.Y, tolerance);
            Assert.Equal(1.24, III.X, tolerance);
            Assert.Equal(0.65, III.Y, tolerance);
            Assert.Equal(7.08, IV.X, tolerance);
            Assert.Equal(1.10, IV.Y, tolerance);
        }
    }
}
