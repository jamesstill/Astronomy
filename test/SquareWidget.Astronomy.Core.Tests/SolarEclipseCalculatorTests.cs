using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class SolarEclipseCalculatorTests
    {
        [Fact]
        public void Test_Total_Solar_Eclipse_In_1785()
        {
            double tolerance = 0.0001;
            DateOnly startDate = new(1785, 1, 1);
            DateOnly endDate = new(1785, 12, 31);
            DateRange dateRange = new(startDate, endDate);

            IEnumerable<SolarEclipse> list = SolarEclipseCalculator.Calculate(dateRange);

            Assert.NotNull(list);
            Assert.Equal(2, list.Count());
            Assert.Equal(1, list.Count(i => i.EclipseType == EclipseType.Total));
            Assert.Equal(1, list.Count(i => i.EclipseType == EclipseType.Annular));
            Assert.Equal(0.0086, list.First(e => e.Date.Equals(new DateOnly(1785, 2, 9))).Gamma, tolerance);
            Assert.Equal(-0.0825, list.First(e => e.Date.Equals(new DateOnly(1785, 8, 5))).Gamma, tolerance);
        }

        [Fact]
        public void Test_Total_Solar_Eclipse_1993_May_21()
        {
            double tolerance = 0.0001;
            DateOnly startDate = new(1993, 5, 21);
            DateOnly endDate = new(1993, 5, 21);
            DateRange dateRange = new(startDate, endDate);

            IEnumerable<SolarEclipse> list = SolarEclipseCalculator.Calculate(dateRange);
            SolarEclipse eclipse = list.First();

            Assert.NotNull(list);
            Assert.NotNull(eclipse);
            Assert.Equal(startDate, eclipse.Date);
            Assert.Equal(EclipseType.Partial, eclipse.EclipseType);
            Assert.Equal(0.7372, eclipse.Magnitude, tolerance);
            Assert.Equal(1.1361, eclipse.Gamma, tolerance);
        }

        [Fact]
        public void Test_Total_Solar_Eclipse_2009_Jul_22()
        {
            double tolerance = 0.0001;
            DateOnly startDate = new(2009, 7, 22);
            DateOnly endDate = new(2009, 7, 22);
            DateRange dateRange = new(startDate, endDate);

            IEnumerable<SolarEclipse> list = SolarEclipseCalculator.Calculate(dateRange);
            SolarEclipse eclipse = list.First();

            Assert.NotNull(list);
            Assert.NotNull(eclipse);
            Assert.Equal(startDate, eclipse.Date);
            Assert.Equal(EclipseType.Total, eclipse.EclipseType);
            Assert.Equal(0, eclipse.Magnitude);
            Assert.Equal(0.0692137, eclipse.Gamma, tolerance);
        }

        [Fact]
        public void Test_Total_Solar_Eclipse_From_2024_To_2028()
        {
            DateOnly startDate = new(2024, 1, 1);
            DateOnly endDate = new(2028, 12, 31);
            DateRange dateRange = new(startDate, endDate);

            IEnumerable<SolarEclipse> list = SolarEclipseCalculator.Calculate(dateRange);

            Assert.NotNull(list);
            Assert.Equal(10, list.Count());
            Assert.Equal(4, list.Count(i => i.EclipseType == EclipseType.Total));
            Assert.Equal(2, list.Count(i => i.Date.Year == 2024));
            Assert.Equal(2, list.Count(i => i.Date.Year == 2025));
            Assert.Equal(2, list.Count(i => i.Date.Year == 2026));
            Assert.Equal(2, list.Count(i => i.Date.Year == 2027));
            Assert.Equal(2, list.Count(i => i.Date.Year == 2028));
        }

        [Fact]
        public void Test_Total_Solar_Eclipse_From_2022_To_2035()
        {
            double tolerance = 0.0001;
            DateOnly startDate = new(2022, 1, 1);
            DateOnly endDate = new(2035, 12, 31);
            DateRange dateRange = new(startDate, endDate);

            IEnumerable<SolarEclipse> list = SolarEclipseCalculator.Calculate(dateRange);

            Assert.NotNull(list);
            Assert.Equal(30, list.Count());
            Assert.Equal(8, list.Count(i => i.EclipseType == EclipseType.Total));
            Assert.Equal(0.37308, list.First(e => e.Date.Equals(new DateOnly(2035, 9, 2))).Gamma, tolerance);
        }
    }
}
