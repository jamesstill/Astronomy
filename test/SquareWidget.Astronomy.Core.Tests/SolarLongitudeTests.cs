using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class SolarLongitudeTests
    {
        [Fact]
        public void Test_Sun_Apparent_Position_1992_Oct_13()
        {
            DateTime datetime = new(1992, 10, 13);
            Degrees l = SolarLongitudeCalculator.Calculate(datetime);
            
            Assert.Equal(199.909873, l, 6);      
        }
    }
}