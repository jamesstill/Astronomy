using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class SundialCalculatorTests
    {
        [Fact]
        public void Test_Sundial_London_UK()
        {
            SexigesimalAngle latitude = new(51, 30, 26); // London UK
            Degrees d = latitude.ToDegrees();

            List<HourAngle> hourAngles = SundialCalculator.Calculate(d);
            Assert.NotNull(hourAngles);

            int precision = 2;

            Assert.Equal(-90.00, hourAngles.First(a => a.Hour == 6).Angle, precision);
            Assert.Equal(-71.10, hourAngles.First(a => a.Hour == 7).Angle, precision);
            Assert.Equal(-53.59, hourAngles.First(a => a.Hour == 8).Angle, precision);
            Assert.Equal(-38.05, hourAngles.First(a => a.Hour == 9).Angle, precision);
            Assert.Equal(-24.32, hourAngles.First(a => a.Hour == 10).Angle, precision);
            Assert.Equal(-11.84, hourAngles.First(a => a.Hour == 11).Angle, precision);
            Assert.Equal(0, hourAngles.First(a => a.Hour == 12).Angle, precision);
            Assert.Equal(11.84, hourAngles.First(a => a.Hour == 13).Angle, precision);
            Assert.Equal(24.32, hourAngles.First(a => a.Hour == 14).Angle, precision);
            Assert.Equal(38.05, hourAngles.First(a => a.Hour == 15).Angle, precision);
            Assert.Equal(53.59, hourAngles.First(a => a.Hour == 16).Angle, precision);
            Assert.Equal(71.10, hourAngles.First(a => a.Hour == 17).Angle, precision);
            Assert.Equal(90.00, hourAngles.First(a => a.Hour == 18).Angle, precision);
        }

        [Fact]
        public void Test_Sundial_Corvallis_OR_USA()
        {
            SexigesimalAngle latitude = new(44, 34, 11); // Corvallis OR USA
            Degrees d = latitude.ToDegrees();

            List<HourAngle> hourAngles = SundialCalculator.Calculate(d);
            Assert.NotNull(hourAngles);

            int precision = 2;

            Assert.Equal(-90.00, hourAngles.First(a => a.Hour == 6).Angle, precision);
            Assert.Equal(-69.10, hourAngles.First(a => a.Hour == 7).Angle, precision);
            Assert.Equal(-50.56, hourAngles.First(a => a.Hour == 8).Angle, precision);
            Assert.Equal(-35.06, hourAngles.First(a => a.Hour == 9).Angle, precision);
            Assert.Equal(-22.06, hourAngles.First(a => a.Hour == 10).Angle, precision);
            Assert.Equal(-10.65, hourAngles.First(a => a.Hour == 11).Angle, precision);
            Assert.Equal(0, hourAngles.First(a => a.Hour == 12).Angle, precision);
            Assert.Equal(10.65, hourAngles.First(a => a.Hour == 13).Angle, precision);
            Assert.Equal(22.06, hourAngles.First(a => a.Hour == 14).Angle, precision);
            Assert.Equal(35.06, hourAngles.First(a => a.Hour == 15).Angle, precision);
            Assert.Equal(50.56, hourAngles.First(a => a.Hour == 16).Angle, precision);
            Assert.Equal(69.10, hourAngles.First(a => a.Hour == 17).Angle, precision);
            Assert.Equal(90.00, hourAngles.First(a => a.Hour == 18).Angle, precision);
        }
    }
}
