using System;
using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Calculators
{
    /// <summary>
    /// Calcuates the positions of the four Galilean moons of Jupiter on a given date
    /// per Jean Meeus, Astronomical Algorithms, Chapters 43 and 44. 
    /// See: https://squarewidget.com/astronomical-calculations-moons-of-jupiter/
    /// </summary>
    public static class JupiterSatellitePositionCalculator
    {
        public static SatellitePositions Calculate(DateTime datetime)
        {
            TimeSpan Δt = DeltaT.GetValue(datetime.Year);
            datetime = datetime.AddSeconds(Δt.TotalSeconds);
            Moment moment = new Moment(datetime);

            double d = moment.DayD;

            Earth earth = new Earth(moment);
            Jupiter jupiter = new Jupiter(moment);

            // argument for the long-period term in the motion of Jupiter
            Degrees V = jupiter.LongPeriodTerm.ToReducedAngle();
          
            // difference in heliocentric longitude between Earth and Jupiter
            double j = 66.115 + 0.9025179 * moment.DayD - 0.329 * Math.Sin(V.ToRadians());
            Degrees J = new Degrees(j).ToReducedAngle();

            // equations of the center for Earth and Jupiter
            Degrees A = earth.EquationOfCenter;
            Degrees B = jupiter.EquationOfCenter;
            Degrees K = new(J + A - B);

            // radius vectors of the Earth and Jupiter
            AstronomicalUnits R = earth.RadiusVector;
            AstronomicalUnits r = jupiter.RadiusVector;

            // distance between Earth and Jupiter
            AstronomicalUnits Δ = new(Math.Sqrt((r * r) + (R * R) - 2 * r * R * Math.Cos(K.ToRadians())));

            // phase angle of Jupiter; psi (ψ) always lies between -12 and +12 degrees
            Radians psi = new(Math.Asin(R / Δ * Math.Sin(K.ToRadians())));
            Degrees ψ = new Degrees(psi);

            // Jupiter's heliocentric longitude referred to the equinox of 2000.0
            double lambda = 34.35 + 0.083091 * d + 0.329 * Math.Sin(V.ToRadians()) + B;
            Degrees λ = new(lambda);

            // Jupiter's inclination of the equator on the orbital plane is 3.12 degrees
            Degrees Ds = new(3.12 * Math.Sin(λ.ToRadians() + 0.74700091985));

            // Jupiter's inclination of the equator on the ecliptic is 2.22 degrees
            Degrees Dx = new(2.22 * Math.Sin(ψ.ToRadians()) * Math.Cos(λ.ToRadians() + 0.38397243544));

            // Jupiter's inclination of the orbital plane on the ecliptic is 1.30 degrees
            Degrees Dy = new(1.30 * ((r - Δ) / Δ) * Math.Sin(λ.ToRadians() - 1.7540558983));

            // planetocentric declination De of the Earth
            Degrees De = new(Ds - Dx - Dy);

            // correction for light time in days; Meeus p. 298
            double t = d - Δ / 173.0;

            double du1 = 163.8069 + 203.4058646 * t + ψ - B;
            double du2 = 358.4140 + 101.2916335 * t + ψ - B;
            double du3 = 5.7176 + 50.2345180 * t + ψ - B;
            double du4 = 224.8092 + 21.4879800 * t + ψ - B;

            Degrees u1 = new Degrees(du1).ToReducedAngle();
            Degrees u2 = new Degrees(du2).ToReducedAngle();
            Degrees u3 = new Degrees(du3).ToReducedAngle();
            Degrees u4 = new Degrees(du4).ToReducedAngle();

            // corrections for perterbations between the satellites themselves
            Degrees G = new(331.18 + 50.310482 * t);
            Degrees H = new(87.45 + 21.569231 * t);

            Radians u1r = u1.ToRadians();
            Radians u2r = u2.ToRadians();
            Radians u3r = u3.ToRadians();

            u1 = new(u1 + 0.473 * Math.Sin(2 * (u1r - u2r)));
            u2 = new(u2 + 1.065 * Math.Sin(2 * (u2r - u3r)));
            u3 = new(u3 + 0.165 * Math.Sin(G.ToRadians()));
            u4 = new(u4 + 0.843 * Math.Sin(H.ToRadians()));

            Radians ru1 = u1.ToRadians();
            Radians ru2 = u2.ToRadians();
            Radians ru3 = u3.ToRadians();
            Radians ru4 = u4.ToRadians();

            Radians r1 = new(5.9057 - 0.0244 * Math.Cos(2 * (ru1 - ru2)));
            Radians r2 = new(9.3966 - 0.0882 * Math.Cos(2 * (ru2 - ru3)));
            Radians r3 = new(14.9883 - 0.0216 * Math.Cos(G.ToRadians()));
            Radians r4 = new(26.3627 - 0.1939 * Math.Cos(H.ToRadians()));

            Radians Der = De.ToRadians();

            // apparent rectangular coordinates of each satellite

            // Satellite I (Io)
            double x1 = r1 * Math.Sin(ru1);
            double y1 = -r1 * Math.Cos(ru1) * Math.Sin(Der);

            // Satellite II (Europa)
            double x2 = r2 * Math.Sin(ru2);
            double y2 = -r2 * Math.Cos(ru2) * Math.Sin(Der);

            // Satellite III (Ganymede)
            double x3 = r3 * Math.Sin(ru3);
            double y3 = -r3 * Math.Cos(ru3) * Math.Sin(Der);

            // Satellite IV (Callisto)
            double x4 = r4 * Math.Sin(ru4);
            double y4 = -r4 * Math.Cos(ru4) * Math.Sin(Der);

            return new SatellitePositions
            {
                Date = datetime,
                Satellites =
                {
                    new Satellite { Name = "Io", X = x1, Y = y1 },
                    new Satellite { Name = "Europa", X = x2, Y = y2 },
                    new Satellite { Name = "Ganymede", X = x3, Y = y3 },
                    new Satellite { Name = "Callisto", X = x4, Y = y4 }
                }
            };
        }
    }
}
