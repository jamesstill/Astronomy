using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class OrbitalElementsTests
    {
        double tolerance = 0.0001;

        [Fact]
        public void Test_Mercury_Orbital_Elements()
        {
            DateTime datetime = new(2065, 6, 24);
            Moment moment = new(datetime);
            Mercury mercury = new(moment);

            Assert.Equal(98123.494701, mercury.OrbitalElements.L, 6);
            Assert.Equal(203.49470, mercury.OrbitalElements.L.ToReducedAngle(), 5);
            Assert.Equal(0.387098310, mercury.OrbitalElements.a, 6);
            Assert.Equal(0.20564510, mercury.OrbitalElements.e, 6);
            Assert.Equal(7.006171, mercury.OrbitalElements.ι, 6);
            Assert.Equal(49.107650, mercury.OrbitalElements.Ω, 6);
            Assert.Equal(78.475382, mercury.OrbitalElements.π, 6);
            Assert.Equal(98045.019319775, mercury.OrbitalElements.M, 9);
            Assert.Equal(125.01932, mercury.OrbitalElements.M.ToReducedAngle(), 5);
            Assert.Equal(29.367732, mercury.OrbitalElements.ω, 6);
        }

        [Fact]
        public void Test_Mercury_Orbital_Elements_Builder()
        {
            DateTime datetime = DateTime.Now;
            Moment moment = new(datetime);

            Mercury mercury = new(moment);

            OrbitalElements oe = mercury.OrbitalElements;

            Assert.NotEqual(0.0, oe.L);
        }

        [Fact]
        public void Test_Jupiter_Orbital_Elements()
        {
            DateTime datetime = new(2024, 1, 15);
            Moment moment = new(datetime);
            Jupiter jupiter = new(moment);

            Assert.NotNull(jupiter.OrbitalElements);
            
            Assert.Equal(44.186445, jupiter.OrbitalElements.L.ToReducedAngle(), tolerance);
            Assert.Equal(0, jupiter.OrbitalElements.a, tolerance);
            Assert.Equal(0.048537, jupiter.OrbitalElements.e, tolerance);
            Assert.Equal(1.301946, jupiter.OrbitalElements.ι, tolerance);
            Assert.Equal(100.709842, jupiter.OrbitalElements.Ω, tolerance);
            Assert.Equal(14.718895, jupiter.OrbitalElements.π, tolerance);
            Assert.Equal(749.467550, jupiter.OrbitalElements.M, tolerance);
            Assert.Equal(29.46755019, jupiter.OrbitalElements.M.ToReducedAngle(), tolerance);
            Assert.Equal(-85.990947, jupiter.OrbitalElements.ω, tolerance);
        }
    }
}