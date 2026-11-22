using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class NutationTests
    {
        [Fact]
        public void Test_Nutation_1987_Apr_10()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(1987, 4, 10);
            Nutation n = NutationCalculator.Calculate(datetime);

            Assert.True(n.ΔΨ.IsNegative);
            Assert.Equal(3.7878243500000002, n.ΔΨ.Seconds, tolerance);
            Assert.Equal(9.44246538, n.Δε.Seconds, tolerance);
            Assert.Equal(23, n.ε.Degrees);
            Assert.Equal(26, n.ε.Minutes);
            Assert.Equal(36.84911283, n.ε.Seconds, 3);
        }

        [Fact]
        public void Test_Nutation_2024_Jan_11()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(2024, 1, 11);
            Nutation n = NutationCalculator.Calculate(datetime);

            Assert.True(n.ΔΨ.IsNegative);
            Assert.Equal(4.9011384600000003, n.ΔΨ.Seconds, tolerance);
            Assert.Equal(8.0097583199999995, n.Δε.Seconds, tolerance);
            Assert.Equal(23, n.ε.Degrees);
            Assert.Equal(26, n.ε.Minutes);
            Assert.Equal(18.2113702, n.ε.Seconds, 3);
        }
    }
}