using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Tests
{
    public class EccentricAnomalyTests
    {
        [Fact]
        public void Test_Eccentric_And_True_Anomaly_Mars_1()
        {
            DateTime datetime = new(2024, 4, 7, 0, 0, 0);
            Moment moment = new(datetime);
            Planet mars = PlanetFactory.Create(PlanetName.Mars, moment);

            Degrees M = mars.OrbitalElements.M.ToReducedAngle();
            Degrees E = mars.OrbitalElements.E.ToDegrees().ToReducedAngle();
            Degrees v = mars.OrbitalElements.v.ToDegrees().ToReducedAngle();

            Assert.Equal(343.5061, M, 4);
            Assert.Equal(341.8376, E, 4);
            Assert.Equal(340.0882, v, 4);
        }

        [Fact]
        public void Test_Eccentric_And_True_Anomaly_Mars_2()
        {
            DateTime datetime = new(2028, 7, 1, 0, 0, 0);
            Moment moment = new(datetime);
            Planet mars = PlanetFactory.Create(PlanetName.Mars, moment);

            Degrees M = mars.OrbitalElements.M.ToReducedAngle();
            Degrees E = mars.OrbitalElements.E.ToDegrees().ToReducedAngle();
            Degrees v = mars.OrbitalElements.v.ToDegrees().ToReducedAngle();

            Assert.Equal(73.6420, M, 4);
            Assert.Equal(78.8948, E, 4);
            Assert.Equal(84.2029, v, 4); 
        }

        [Fact]
        public void Test_Eccentric_And_True_Anomaly_Earth_1()
        {
            DateTime datetime = new(2019, 4, 7, 21, 0, 0);
            Moment moment = new(datetime);
            Planet earth = PlanetFactory.Create(PlanetName.Earth, moment);

            Degrees M = earth.OrbitalElements.M.ToReducedAngle();
            Degrees E = earth.OrbitalElements.E.ToDegrees().ToReducedAngle();
            Degrees v = earth.OrbitalElements.v.ToDegrees().ToReducedAngle();

            Assert.Equal(92.5823, M, 4);
            Assert.Equal(93.5373, E, 4);
            Assert.Equal(94.4919, v, 4);
        }

        [Fact]
        public void Test_Eccentric_And_True_Anomaly_Earth_2()
        {
            DateTime datetime = new(2023, 12, 31, 0, 0, 0);
            Moment moment = new(datetime);
            Earth earth = new(moment);

            Degrees M = earth.OrbitalElements.M.ToReducedAngle();
            Degrees E = earth.OrbitalElements.E.ToDegrees().ToReducedAngle();
            Degrees ν = earth.OrbitalElements.v.ToDegrees().ToReducedAngle();

            Assert.Equal(355.8228, M, 4);
            Assert.Equal(355.7519, E, 4);
            Assert.Equal(355.6804, ν, 4);
        }

        [Fact]
        public void Test_Eccentric_And_True_Anomaly_Earth_3()
        {
            DateTime datetime = new(2024, 1, 5, 0, 0, 0);
            Moment moment = new(datetime);
            Earth earth = new(moment);

            Degrees M = earth.OrbitalElements.M.ToReducedAngle();
            Degrees E = earth.OrbitalElements.E.ToDegrees().ToReducedAngle();
            Degrees ν = earth.OrbitalElements.v.ToDegrees().ToReducedAngle();

            Assert.Equal(0.7508, M, 4);
            Assert.Equal(0.7635, E, 4);
            Assert.Equal(0.7764, ν, 4);
        }
    }
}