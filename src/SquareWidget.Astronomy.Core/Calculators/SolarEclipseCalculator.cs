using System;
using System.Collections.Generic;
using System.Linq;
using SquareWidget.Astronomy.Core.CelestialObjects.Moons;
using SquareWidget.Astronomy.Core.CelestialObjects.Stars;
using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Calculators
{
    /// <summary>
    /// Return all solar eclipses that occur within the date range. Returns a 
    /// list of SolarEclipse objects that can be compared to NASA's published 
    /// canon. See: https://eclipse.gsfc.nasa.gov/5MCSE/5MCSE-Maps-10.pdf
    /// </summary>
    public static class SolarEclipseCalculator
    {
        public static IEnumerable<SolarEclipse> Calculate(DateRange dateRange)
        {
            List<SolarEclipse> list = new();

            List<MoonPhase> moonPhases = MoonPhaseDatesCalculator.Calculate(dateRange);

            // select all new moon moments from the phases within the date range
            IEnumerable<Moment> moments = moonPhases
                .Where(m => m.PhaseName.Equals(PhaseName.NewMoon))
                .OrderBy(o => o.Moment.ToDateTime())
                .Select(m => m.Moment);

            // enumerate the new moon dates looking for solar eclipses
            foreach (Moment moment in moments)
            {
                DateTime datetime = moment.ToDateTime();

                Moon moon = new(moment);
                Earth earth = new(moment);
                Sun sun = new(moment);

                int k = moon.FirstNewMoonOfJ2000Epoch;
                double T = moment.T;
                double jde = ToJDE(k, T);
                double E = ToE(T);
                Radians Sm = sun.MeanAnomaly.ToRadians();
                Radians Mm = moon.MeanAnomaly.ToRadians();
                Degrees F = moon.ArgumentOfLatitude;
                Radians Ω = moon.LongitudeOfAscendingNode.ToRadians();
                Radians Fp = ToFPrime(F, Ω);
                Radians Ap = ToAPrime(k, T);
                Radians P = ToP(E, Sm, Mm, Fp);
                Radians Q = ToQ(E, Sm, Mm);
                Radians W = ToW(Fp);
                double g = ToG(P, Q, Fp, W); 
                double u = ToU(E, Sm, Mm);

                double magnitude = 0.0;

                if (!IsEclipse(g, u))
                {
                    // skip the date; variables fall outside of an eclipse event
                    continue;
                }

                string eclipseType = GetEclipseType(g, u);
                if (eclipseType == EclipseType.Partial)
                {
                    magnitude = ToGreatestMagnitude(u, g);
                }

                //  the instant when the axis of the Moon's shadow cone passes closest to Earth's center
                DateTime timeOfGreatestEclipseUTC = TimeOfGreatestEclipse(jde, E, Mm, Sm, Fp, Ap, Ω);
                DateOnly date = DateOnly.FromDateTime(datetime);
                TimeOnly time = TimeOnly.FromDateTime(timeOfGreatestEclipseUTC);

                yield return new SolarEclipse(date, time, eclipseType, magnitude, g);
            }
        }

        /// <summary>
        /// Time of mean conjuction or opposition (49.1)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public static double ToJDE(int k, double T)
        {
            return 2451550.09766 +
                (29.530588861 * k) +
                (0.00015437 * (T * T)) -
                (0.000000150 * (T * T * T)) +
                (0.00000000073 * (T * T * T * T));
        }

        /// <summary>
        /// Corrections for Earth's eccentricity in orbit (47.6)
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        private static double ToE(double T)
        {
            return 1 - 0.002516 * T - 0.0000074 * T * T;
        }

        /// <summary>
        /// Angle F prime to obtain time of maximum eclipse
        /// </summary>
        /// <param name="F"></param>
        /// <param name="Ω"></param>
        /// <returns></returns>
        private static Radians ToFPrime(Degrees F, Radians Ω)
        {
            Degrees d = new(F - (0.02665 * Math.Sin(Ω)));
            return d.ToReducedAngle().ToRadians();
        }

        /// <summary>
        /// Angle A prime to obtain time of maximum eclipse
        /// </summary>
        /// <param name="k"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        private static Radians ToAPrime(double k, double T)
        {
            Degrees d = new(299.77 + (0.107408 * k) - (0.009173 * (T * T)));
            return d.ToReducedAngle().ToRadians();
        }

        private static Radians ToP(double E, Radians Sm, Radians Mm, Radians Fp)
        {
            double r =
                (0.2070 * E * Math.Sin(Sm)) +
                (0.0024 * E * Math.Sin(Sm * 2)) -
                (0.0392 * Math.Sin(Mm)) +
                (0.0116 * Math.Sin(Mm * 2)) -
                (0.0073 * E * Math.Sin(Mm + Sm)) +
                (0.0067 * E * Math.Sin(Mm - Sm)) +
                (0.0118 * Math.Sin(Fp * 2));

            return new Radians(r);
        }

        private static Radians ToQ(double E, Radians Sm, Radians Mm)
        {
            double r =
                5.2207 -
                (0.0048 * E * Math.Cos(Sm)) +
                (0.0020 * E * Math.Cos(Sm * 2)) -
                (0.3299 * Math.Cos(Mm)) -
                (0.0060 * E * Math.Cos(Mm + Sm)) +
                (0.0041 * E * Math.Cos(Mm - Sm));

            return new Radians(r);
        }

        private static Radians ToW(Radians Fp)
        {
            double r = Math.Abs(Math.Cos(Fp));
            return new Radians(r);
        }

        /// <summary>
        /// Least distance from the axis of the Moon's shadow to the center of 
        /// the Earth in units of the equitorial radius of the Earth (6378 km)
        /// </summary>
        /// <param name="P"></param>
        /// <param name="Q"></param>
        /// <param name="Fp"></param>
        /// <param name="W"></param>
        /// <returns></returns>
        private static double ToG(Radians P, Radians Q, Radians Fp, Radians W)
        {
            return (P * Math.Cos(Fp) + Q * Math.Sin(Fp)) * (1 - 0.0048 * W);
        }

        /// <summary>
        ///  radius of Moon's umbral cone in the fundamental plane
        /// </summary>
        /// <param name="E"></param>
        /// <param name="Sm"></param>
        /// <param name="Mm"></param>
        /// <returns></returns>
        private static double ToU(double E, Radians Sm, Radians Mm)
        {
            return
                0.0059 +
                0.0046 * E * Math.Cos(Sm) -
                0.0182 * Math.Cos(Mm) +
                0.0004 * Math.Cos(Mm * 2) -
                0.0005 * Math.Cos(Sm + Mm);
        }

        /// <summary>
        /// Meeus (54.2)
        /// For a partial eclipse the magnitude at its greatest point
        /// </summary>
        /// <returns></returns>
        private static double ToGreatestMagnitude(double u, double g)
        {
            return (1.5433 + u - Math.Abs(g)) / (0.5461 + 2 * u);
        }

        /// <summary>
        /// Returns the time of greatest eclipse in UTC
        /// </summary>
        /// <param name="jde"></param>
        /// <param name="E"></param>
        /// <param name="Mm"></param>
        /// <param name="Sm"></param>
        /// <param name="Fp"></param>
        /// <param name="Ap"></param>
        /// <param name="Ω"></param>
        /// <returns></returns>
        private static DateTime TimeOfGreatestEclipse(double jde, double E, Radians Mm, Radians Sm, Radians Fp, Radians Ap, Radians Ω)
        {
            var jdeCorrection =
                (-0.4075 * Math.Sin(Mm)) +
                (0.1721 * E * Math.Sin(Sm)) +
                (0.0161 * Math.Sin(2 * Mm)) -
                (0.0097 * Math.Sin(2 * Fp)) +
                (0.0073 * E * Math.Sin(Mm - Sm)) -
                (0.0050 * E * Math.Sin(Mm + Sm)) -
                (0.0023 * Math.Sin(Mm - 2 * Fp)) +
                (0.0021 * E * Math.Sin(2 * Sm)) +
                (0.0012 * Math.Sin(Mm + 2 * Fp)) +
                (0.0006 * E * Math.Sin(2 * Mm + Sm)) -
                (0.0004 * Math.Sin(3 * Mm)) -
                (0.0003 * E * Math.Sin(Sm + 2 * Fp)) +
                (0.0003 * Math.Sin(Ap)) -
                (0.0002 * E * Math.Sin(Sm - 2 * Fp)) -
                (0.0002 * E * Math.Sin(2 * Mm - Sm)) -
                (0.0002 * Math.Sin(Ω));

            var correctedJDE = jde + jdeCorrection;
            Moment m = new(correctedJDE);
            DateTime d = m.ToDateTime();

            // convert TD to UTC by subtracting ΔT
            TimeSpan deltaT = DeltaT.GetValue(d.Year);
            return d.AddSeconds(-deltaT.Seconds);
        }

        /// <summary>
        /// Returns true if the variables for g and u fall within 
        /// the range indicating an eclipse of some type occurs.
        /// A call to GetEclipseType will return the type.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        private static bool IsEclipse(double g, double u)
        {
            double absG = Math.Abs(g);
            return (absG <= 1.5433 + u);
        }


        /// <summary>
        /// Per Meeus: in the case of a central eclipse, the type of the eclipse can be determined by 
        /// the following rules: if u &lt; 0 then it's a total eclipse; if u > 0.0047 then it is annular; 
        /// if u is >= 0 and &lt;= 0.0047 then it is either annular or annular-total ("hybrid"). 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        private static string GetEclipseType(double g, double u)
        {
            if (!IsEclipse(g, u))
            {
                return EclipseType.Unknown;
            }

            double absG = Math.Abs(g);

            if (absG >= 0.9972)
            {
                if (absG <= 1.5433 + u)
                {
                    return EclipseType.Partial;
                }
            }

            if (u < 0)
            {
                return EclipseType.Total;
            }

            if (u > 0.0047)
            {
                return EclipseType.Annular;
            }

            var w = 0.00464 * Math.Sqrt(1 - g * g);
            return (u < w) ? EclipseType.Hybrid : EclipseType.Annular;
        }
    }
}
