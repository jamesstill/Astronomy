using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class RightAscensionTests
    {
        [Fact]
        public void Test_RA_To_DD_1()
        {
            RightAscension ra = new RightAscension(20, 21, 7.2);
            Degrees d = ra.ToDegrees();
            double dd = ra.ToDecimalDegrees();

            Assert.Equal(d, 305.2800, 4);
            Assert.Equal(dd, 305.2800, 4);
        }

        [Fact]
        public void Test_RA_To_DD_2()
        {
            RightAscension ra = new RightAscension(7, 45, 18.946);
            Degrees d = ra.ToDegrees();
            double dd = ra.ToDecimalDegrees();

            Assert.Equal(116.328942, d, 6);
            Assert.Equal(116.328942, dd, 6);
        }

        [Fact]
        public void Test_DD_To_RA_1()
        {
            Degrees d = new Degrees(305.2800);
            RightAscension ra = new RightAscension(d);

            Assert.Equal(20, ra.Hours);
            Assert.Equal(21, ra.Minutes);
            Assert.Equal(7.2, ra.Seconds, 6);
        }

    }
}
