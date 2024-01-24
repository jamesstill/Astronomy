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
            Assert.Equal(3.84084252, n.ΔΨ.Seconds, tolerance);
            Assert.Equal(9.493672, n.Δε.Seconds, tolerance);
            Assert.Equal(23, n.ε.Degrees);
            Assert.Equal(26, n.ε.Minutes);
            Assert.Equal(36.87640, n.ε.Seconds, 3);
        }

        [Fact]
        public void Test_Nutation_2024_Jan_11()
        {
            double tolerance = 0.0001;
            DateTime datetime = new(2024, 1, 11);
            Nutation n = NutationCalculator.Calculate(datetime);

            Assert.True(n.ΔΨ.IsNegative);
            Assert.Equal(4.98006082, n.ΔΨ.Seconds, tolerance);
            Assert.Equal(8.00223341, n.Δε.Seconds, tolerance);
            Assert.Equal(23, n.ε.Degrees);
            Assert.Equal(26, n.ε.Minutes);
            Assert.Equal(18.27142908, n.ε.Seconds, 3);
        }
    }
}