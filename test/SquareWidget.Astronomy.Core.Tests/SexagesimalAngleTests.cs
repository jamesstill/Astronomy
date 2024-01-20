using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System.Diagnostics;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class SexigesimalAngleTests
    {
        [Fact]
        public void Test_DD_To_DMS_1()
        {
            Degrees d = new Degrees(47.31);
            SexigesimalAngle l = new SexigesimalAngle(d);

            Assert.Equal(l.Degrees, 47);
            Assert.Equal(l.Minutes, 18);
            Assert.Equal(l.Seconds, 36);
        }

        [Fact]
        public void Test_DD_To_DMS_2()
        {
            Degrees d = new Degrees(92.7890);
            SexigesimalAngle l = new SexigesimalAngle(d);

            Assert.Equal(l.Degrees, 92);
            Assert.Equal(l.Minutes, 47);
            Assert.Equal(l.Seconds, 20.4, 4);
        }

        [Fact]
        public void Test_DMS_Sign_1()
        {
            SexigesimalAngle a = new(-23, 13, 49);
            Assert.True(a.IsNegative);
        }

        [Fact]
        public void Test_DMS_Sign_2()
        {
            SexigesimalAngle a = new(0, -13, 49);
            Assert.True(a.IsNegative);
        }

        [Fact]
        public void Test_DMS_Sign_3()
        {
            SexigesimalAngle a = new(0, 0, -49);
            Assert.True(a.IsNegative);
        }

        [Fact]
        public void Test_DMS_Sign_4()
        {
            SexigesimalAngle a = new(0, 0, 49);
            Assert.False(a.IsNegative);
        }

        [Fact]
        public void Test_DMS_Sign_5()
        {
            SexigesimalAngle a = new(0, 13, 49);
            Assert.False(a.IsNegative);
        }

        [Fact]
        public void Test_DMS_Sign_6()
        {
            SexigesimalAngle a = new(23, 13, 49);
            Assert.False(a.IsNegative);
        }

        [Fact]
        public void Test_DMS_To_Display()
        {
            SexigesimalAngle a = new(0, -13, 49);
            Debug.WriteLine(a.ToString());
            Assert.True(a.IsNegative);
        }

        [Fact]
        public void Test_DMS_To_DD_1()
        {
            SexigesimalAngle l = new SexigesimalAngle(47, 18, 36);
            Degrees d = new Degrees(l);

            Assert.Equal(d, 47.31, 4);
        }

        [Fact]
        public void Test_DMS_To_DD_2()
        {
            SexigesimalAngle l = new SexigesimalAngle(92, 47, 20.4);
            Degrees d = new Degrees(l);

            Assert.Equal(d, 92.789);
        }

        [Fact]
        public void Test_DMS_To_Radians_1()
        {
            Degrees d = new Degrees(47.31);
            SexigesimalAngle l = new SexigesimalAngle(d);
            Radians r = l.ToRadians();

            Assert.Equal(0.8257152, r, 6);
        }

        [Fact]
        public void Test_DMS_To_Radians_2()
        {
            double tolerance = 0.0001;
            SexigesimalAngle l = new SexigesimalAngle(184, 36, 0);
            Radians r = l.ToRadians();

            Assert.Equal(3.221878, r, tolerance);
        }

        [Fact]
        public void Test_Radians_To_DMS_1()
        {
            Radians r = new Radians(0.825715);
            Degrees d = new Degrees(r);
            SexigesimalAngle l = new SexigesimalAngle(d);

            Assert.Equal(47.31, d, 2);
            Assert.Equal(47, l.Degrees);
            Assert.Equal(18, l.Minutes);
            Assert.Equal(35.94, l.Seconds, 2);
        }
    }
}
