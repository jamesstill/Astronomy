using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.CelestialObjects.Moons;
using SquareWidget.Astronomy.Core.Models;
using System.Diagnostics;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class MoonPhaseDatesCalculatorTests
    {
        [Fact]
        public void Test_First_Last_Quarter_For_Year_2044()
        {
            DateOnly startDate = new(2044, 1, 1);
            DateOnly endDate = new(2044, 2, 28);
            DateRange dateRange = new(startDate, endDate);

            var list = MoonPhaseDatesCalculator.Calculate(dateRange);

            MoonPhase janLQ = list.First(m => m.PhaseName == PhaseName.LastQuarter && m.Moment.Month == 1);

            Assert.Equal(2044, janLQ.Moment.Year);
            Assert.Equal(1, janLQ.Moment.Month);
            Assert.Equal(21, janLQ.Moment.Day);
            Assert.Equal(23, janLQ.Moment.Hour);
            Assert.Equal(46, janLQ.Moment.Minute);
        }

        [Fact]
        public void Test_New_Moon_Dates_For_Year_2001()
        {
            DateOnly startDate = new(2000, 12, 1);
            DateOnly endDate = new(2002, 1, 31);
            DateRange dateRange = new(startDate, endDate);

            var list = MoonPhaseDatesCalculator.Calculate(dateRange);

            MoonPhase janNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 1 && m.Moment.Year == 2001);
            MoonPhase febNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 2 && m.Moment.Year == 2001);
            MoonPhase marNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 3 && m.Moment.Year == 2001);
            MoonPhase aprNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 4 && m.Moment.Year == 2001);
            MoonPhase mayNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 5 && m.Moment.Year == 2001);
            MoonPhase junNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 6 && m.Moment.Year == 2001);
            MoonPhase julNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 7 && m.Moment.Year == 2001);
            MoonPhase augNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 8 && m.Moment.Year == 2001);
            MoonPhase sepNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 9 && m.Moment.Year == 2001);
            MoonPhase octNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 10 && m.Moment.Year == 2001);
            MoonPhase novNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 11 && m.Moment.Year == 2001);
            MoonPhase decNewMoon = list.First(m => m.PhaseName == PhaseName.NewMoon && m.Moment.Month == 12 && m.Moment.Year == 2001);

            Assert.Equal(janNewMoon.Moment.ToDateTime(), new DateTime(2001, 1, 24, 13, 6, 50, 611));
            Assert.Equal(febNewMoon.Moment.ToDateTime(), new DateTime(2001, 2, 23, 8, 21, 11, 307));
            Assert.Equal(marNewMoon.Moment.ToDateTime(), new DateTime(2001, 3, 25, 1, 21, 8, 899));
            Assert.Equal(aprNewMoon.Moment.ToDateTime(), new DateTime(2001, 4, 23, 15, 25, 38, 392));
            Assert.Equal(mayNewMoon.Moment.ToDateTime(), new DateTime(2001, 5, 23, 2, 46, 4, 969));
            Assert.Equal(junNewMoon.Moment.ToDateTime(), new DateTime(2001, 6, 21, 11, 57, 46, 832));
            Assert.Equal(julNewMoon.Moment.ToDateTime(), new DateTime(2001, 7, 20, 19, 44, 16, 744));
            Assert.Equal(augNewMoon.Moment.ToDateTime(), new DateTime(2001, 8, 19, 2, 55, 7, 715));
            Assert.Equal(sepNewMoon.Moment.ToDateTime(), new DateTime(2001, 9, 17, 10, 27, 17, 925));
            Assert.Equal(octNewMoon.Moment.ToDateTime(), new DateTime(2001, 10, 16, 19, 23, 17, 890));
            Assert.Equal(novNewMoon.Moment.ToDateTime(), new DateTime(2001, 11, 15, 6, 40, 3, 680));
            Assert.Equal(decNewMoon.Moment.ToDateTime(), new DateTime(2001, 12, 14, 20, 47, 33, 315));

            foreach (var item in list)
            {
                Debug.WriteLine(item.PhaseName + " on " + item.Moment.ToDateTime().ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            }
        }

        [Fact]
        public void Test_New_Moon_Dates_For_Beginning_Year_2024()
        {
            DateOnly startDate = new(2024, 1, 1);
            DateOnly endDate = new(2024, 3, 31);
            DateRange dateRange = new(startDate, endDate);

            List<MoonPhase> list = MoonPhaseDatesCalculator.Calculate(dateRange);

            foreach (var item in list)
            {
                Debug.WriteLine(item.PhaseName + " on " + item.Moment.ToDateTime().ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            }
        }
    }
}
