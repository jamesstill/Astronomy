using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;

namespace SquareWidget.Astronomy.Core.Calculators
{
    /// <summary>
    /// Calcuates the geocentric (apparent) position of a planet relative to the mean equinox of the date.
    /// </summary>
    public static class GeocentricPositionCalculator
    {
        /// <summary>
        /// Calculate geocentric position of the planet in its elliptical orbit
        /// </summary>
        /// <returns></returns>
        public static GeocentricPosition Calculate(DateTime datetime, string planetName)
        {
            // get true distance from Earth to the planet
            Moment moment = new(datetime);
            Planet planet = PlanetFactory.Create(planetName, moment);

            Radians L = planet.SphericalCoordinates.L;
            Radians B = planet.SphericalCoordinates.B;
            AstronomicalUnits R = planet.SphericalCoordinates.R;

            Earth earth = new(moment);
            Radians L0 = earth.SphericalCoordinates.L;
            Radians B0 = earth.SphericalCoordinates.B;
            AstronomicalUnits R0 = earth.SphericalCoordinates.R;

            AstronomicalUnits x = new(R * Math.Cos(B) * Math.Cos(L) - R0 * Math.Cos(B0) * Math.Cos(L0));
            AstronomicalUnits y = new(R * Math.Cos(B) * Math.Sin(L) - R0 * Math.Cos(B0) * Math.Sin(L0));
            AstronomicalUnits z = new(R * Math.Sin(B) - R0 * Math.Sin(B0));

            // true distance
            AstronomicalUnits Δt = new(Math.Sqrt(x * x + y * y + z * z));

            LightTime previousLightTime = new(Δt);

            // t - τ
            double jde = moment.JDE;
            moment = new Moment(jde - previousLightTime.TotalDays);

            LightTime nextLightTime = previousLightTime;
            AstronomicalUnits Δa = new();

            bool areEqual = false;

            while (!areEqual) // solve iteratively for light time distance Δ
            {
                DateTime lightTimeAdjusted = datetime.Subtract(nextLightTime);
                moment = new Moment(lightTimeAdjusted);

                planet = PlanetFactory.Create(planetName, moment);
                L = planet.SphericalCoordinates.L;
                B = planet.SphericalCoordinates.B;
                R = planet.SphericalCoordinates.R;

                x = new AstronomicalUnits(R * Math.Cos(B) * Math.Cos(L) - R0 * Math.Cos(B0) * Math.Cos(L0));
                y = new AstronomicalUnits(R * Math.Cos(B) * Math.Sin(L) - R0 * Math.Cos(B0) * Math.Sin(L0));
                z = new AstronomicalUnits(R * Math.Sin(B) - R0 * Math.Sin(B0));
                
                // apparent distance
                Δa = new AstronomicalUnits(Math.Sqrt(x * x + y * y + z * z));

                previousLightTime = nextLightTime;
                nextLightTime = new LightTime(Δa);

                areEqual = nextLightTime.Equals(previousLightTime);
            }

            Radians l = new(Math.Atan2(y, x));
            Radians b = new(Math.Atan2(z, Math.Sqrt(x * x + y * y)));

            // correct longitude λ for nutation
            Nutation n = NutationCalculator.Calculate(datetime);
            l += n.ΔΨ.ToRadians();

            Degrees λ = l.ToDegrees().ToReducedAngle();
            Degrees β = b.ToDegrees();
            Degrees ε = n.ε.ToDegrees();

            // convert from ecliptical to equitorial coordinates
            EclipticalCoordinates ec = new EclipticalCoordinates(λ, β, ε);
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
