using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;

namespace SquareWidget.Astronomy.Core.Calculators
{
    /// <summary>
    /// Calculates the geocentric (apparent) position of a planet relative to the true equinox of the date.
    /// </summary>
    public static class GeocentricPositionCalculator
    {
        private const int MaxIterations = 20;

        /// <summary>
        /// Calculate the apparent geocentric position of a planet in its elliptical orbit.
        /// </summary>
        public static GeocentricPosition Calculate(DateTime datetime, string planetName)
        {
            Moment moment = new(datetime);

            // Earth's heliocentric rectangular coordinates at time t (constant across light-time iterations)
            Earth earth = new(moment);
            Radians L0 = earth.SphericalCoordinates.L;
            Radians B0 = earth.SphericalCoordinates.B;
            AstronomicalUnits R0 = earth.SphericalCoordinates.R;

            double x0 = R0 * Math.Cos(B0) * Math.Cos(L0);
            double y0 = R0 * Math.Cos(B0) * Math.Sin(L0);
            double z0 = R0 * Math.Sin(B0);

            // Planet's true position and distance at time t
            Planet planet = PlanetFactory.Create(planetName, moment);
            Radians L = planet.SphericalCoordinates.L;
            Radians B = planet.SphericalCoordinates.B;
            AstronomicalUnits R = planet.SphericalCoordinates.R;

            double x = R * Math.Cos(B) * Math.Cos(L) - x0;
            double y = R * Math.Cos(B) * Math.Sin(L) - y0;
            double z = R * Math.Sin(B) - z0;

            AstronomicalUnits Δt = new(Math.Sqrt(x * x + y * y + z * z));

            LightTime previousLightTime = new(Δt);
            LightTime nextLightTime = previousLightTime;
            AstronomicalUnits Δa = Δt;

            bool areEqual = false;
            int iterations = 0;

            // Solve iteratively for light-time adjusted distance Δ (t - τ)
            while (!areEqual && iterations < MaxIterations)
            {
                iterations++;

                DateTime lightTimeAdjusted = datetime.Subtract(nextLightTime);
                moment = new Moment(lightTimeAdjusted);

                planet = PlanetFactory.Create(planetName, moment);
                L = planet.SphericalCoordinates.L;
                B = planet.SphericalCoordinates.B;
                R = planet.SphericalCoordinates.R;

                x = R * Math.Cos(B) * Math.Cos(L) - x0;
                y = R * Math.Cos(B) * Math.Sin(L) - y0;
                z = R * Math.Sin(B) - z0;

                // Apparent distance
                Δa = new AstronomicalUnits(Math.Sqrt(x * x + y * y + z * z));

                previousLightTime = nextLightTime;
                nextLightTime = new LightTime(Δa);

                areEqual = nextLightTime.Equals(previousLightTime);
            }

            Radians l = new(Math.Atan2(y, x));
            Radians b = new(Math.Atan2(z, Math.Sqrt(x * x + y * y)));

            // Correct longitude λ for nutation
            Nutation n = NutationCalculator.Calculate(datetime);
            l = new Radians(l + n.ΔΨ.ToRadians());

            Degrees λ = l.ToDegrees().ToReducedAngle();
            Degrees β = b.ToDegrees();
            Degrees ε = n.ε.ToDegrees();

            // Convert from ecliptical to equatorial coordinates
            EclipticalCoordinates ec = new(λ, β, ε);
            EquitorialCoordinates eqc = ec.ToΕquitorialCoordinates();

            return new GeocentricPosition
            {
                EclipticalCoordinates = ec,
                EquitorialCoordinates = eqc,
                Δt = Δt,
                Δa = Δa
            };
        }
    }
}