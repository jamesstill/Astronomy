using System;
using System.Collections.Generic;
using SquareWidget.Astronomy.Core.CelestialObjects.Moons;
using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Calculators
{
    /// <summary>
    /// Calcuates the instance of the new moon phases within a given date range 
    /// following Jean Meeus, Astronomical Algorithms, Chapter 49.
    /// </summary>
    public static class MoonPhaseDatesCalculator
    {
        const double LUNAR_CYCLE = 29.53058770576;

        public static List<MoonPhase> Calculate(DateRange dateRange)
        {
            List<MoonPhase> result = new();

            DateTime start = dateRange.StartDate.ToDateTime(default);
            DateTime end = dateRange.EndDate.ToDateTime(default);

            for (var d = start; d <= end; d = d.AddDays(LUNAR_CYCLE))
            {
                Moment m = new Moment(d);
                double k = ToK(m); // New Moon
                MoonPhase phase = GetMoonPhase(k);
                result.Add(phase);

                // k + 0.25 = First Quarter; k + 0.50 = Full Moon; k + 0.75 = Last Quarter
                for (double i = 0.25; i <= 0.75; i += 0.25)
                {
                    k += i;
                    phase = GetMoonPhase(k);
                    result.Add(phase);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the moon phase along with its date and time.
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private static MoonPhase GetMoonPhase(double k)
        {
            double T = ToT(k);
            double JDE = ToJDE(k, T);
            double E = ToE(T);
            double Sm = ToM(k, T);
            double Mm = ToMPrime(k, T);
            double F = ToF(k, T);
            double O = ToO(k, T);
            double A = ToA(k, T);

            double NM;
            string phaseName = GetPhaseName(k);
            switch (phaseName)
            {
                case PhaseName.NewMoon:
                    NM = ToNM(E, Sm, Mm, F, O);
                    break;

                case PhaseName.FullMoon:
                    NM = ToFM(E, Sm, Mm, F, O);
                    break;

                case PhaseName.FirstQuarter:
                    NM = ToFLQ(E, Sm, Mm, F, O);
                    double W = ToFLQCorrection(E, Sm, Mm, F);
                    NM += W;
                    break;

                case PhaseName.LastQuarter:
                    NM = ToFLQ(E, Sm, Mm, F, O);
                    W = ToFLQCorrection(E, Sm, Mm, F);
                    NM -= W;
                    break;

                default:
                    throw new Exception("Only values for k that are 0.0, 0.25, 0.50, or 0.75 are valid.");
            }

            double correctedJDE = JDE + NM + A;
            DateTime correctedDateTime = ToDateTimeUTC(correctedJDE);
            return MoonPhaseFactory.Create(phaseName, new Moment(correctedDateTime));
        }

        /// <summary>
        /// Meeus (49.2)
        /// Baseline value k where k = 0 is 6 Jan 2000 (first new moon of J2000.0 epoch).
        /// </summary>
        /// <param name="m">Moment</param>
        /// <returns></returns>
        private static double ToK(Moment m)
        {
            double decimalYear = (m.ToDecimalYear() - 2000) * 12.3685;
            return Math.Floor(decimalYear);
        }

        /// <summary>
        /// Meeus (49.3)
        /// Time T is the time in Julian centuries since the J2000.0 epoch
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private static double ToT(double k)
        {
            return k / 1236.85;
        }

        /// <summary>
        /// Meeus (49.1)
        /// Time of mean conjuction or opposition
        /// </summary>
        /// <param name="k"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        private static double ToJDE(double k, double T)
        {
            return 2451550.09766 +
                (29.530588861 * k) +
                (0.00015437 * T * T) -
                (0.000000150 * T * T * T) +
                (0.00000000073 * T * T * T * T);
        }

        /// <summary>
        /// Meeus (47.6)
        /// Earth's eccentricity in orbit
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        private static double ToE(double T)
        {
            return 1 - 0.002516 * T - 0.0000074 * T * T;
        }

        /// <summary>
        /// Meeus (49.4)
        /// Sun's mean anomaly at time JDE
        /// </summary>
        /// <param name="k"></param>
        /// <param name="T"></param>
        /// <returns>M</returns>
        private static double ToM(double k, double T)
        {
            double i = 2.5534 +
                (29.10535670 * k) -
                (0.0000014 * T * T) -
                (0.00000011 * T * T * T);

            return new Degrees(i).ToReducedAngle();
        }

        /// <summary>
        /// Meeus (49.5)
        /// Moon's mean anomaly at time JDE
        /// </summary>
        /// <param name="k"></param>
        /// <param name="T"></param>
        /// <returns>M′</returns>
        private static double ToMPrime(double k, double T)
        {
            double i = 201.5643 +
                (385.81693528 * k) +
                (0.0107582 * T * T) +
                (0.00001238 * T * T * T) -
                (0.000000058 * T * T * T * T);

            return new Degrees(i).ToReducedAngle();
        }

        /// <summary>
        /// Meeus (49.6)
        /// Moon's argument of latitude
        /// </summary>
        /// <param name="k"></param>
        /// <param name="T"></param>
        /// <returns>F</returns>
        private static double ToF(double k, double T)
        {
            double i = 160.7108 +
                (390.67050284 * k) -
                (0.0016118 * (T * T)) -
                (0.00000227 * (T * T * T)) +
                (0.000000011 * (T * T * T * T));

            return new Degrees(i).ToReducedAngle();
        }

        /// <summary>
        /// Meeus (49.7)
        /// Longitude of the ascending node of the lunar orbit
        /// </summary>
        /// <param name="k"></param>
        /// <param name="T"></param>
        /// <returns>Ω</returns>
        private static double ToO(double k, double T)
        {
            double i = 124.7746 -
                (1.56375588 * k) +
                (0.0020672 * (T * T)) +
                (0.00000215 * (T * T * T));

            return new Degrees(i).ToReducedAngle();
        }

        /// <summary>
        /// Planetary coefficients A1 thru A14 in degrees as 
        /// periodic terms for correction to JDE (p. 351).
        /// </summary>
        /// <param name="k"></param>
        /// <param name="T"></param> 
        /// <returns></returns>
        private static double ToA(double k, double T)
        {
            Degrees a1 = new(299.77 + 0.107408 * k - 0.009173 * T * T);
            Degrees a2 = new(251.88 + 0.016321 * k);
            Degrees a3 = new(251.83 + 26.651886 * k);
            Degrees a4 = new(349.42 + 36.412478 * k);
            Degrees a5 = new(84.66 + 18.206239 * k);
            Degrees a6 = new(141.74 + 53.303771 * k);
            Degrees a7 = new(207.14 + 2.453732 * k);
            Degrees a8 = new(154.84 + 7.306860 * k);
            Degrees a9 = new(34.52 + 27.261239 * k);
            Degrees a10 = new(207.19 + 0.121824 * k);
            Degrees a11 = new(291.34 + 1.844379 * k);
            Degrees a12 = new(161.72 + 24.198154 * k);
            Degrees a13 = new(239.56 + 25.513099 * k);
            Degrees a14 = new(331.55 + 3.592518 * k);

            // additional corrections for all phases
            return
                0.000325 * Math.Sin(a1.ToReducedAngle().ToRadians()) +
                0.000165 * Math.Sin(a2.ToReducedAngle().ToRadians()) +
                0.000164 * Math.Sin(a3.ToReducedAngle().ToRadians()) +
                0.000126 * Math.Sin(a4.ToReducedAngle().ToRadians()) +
                0.000110 * Math.Sin(a5.ToReducedAngle().ToRadians()) +
                0.000062 * Math.Sin(a6.ToReducedAngle().ToRadians()) +
                0.000060 * Math.Sin(a7.ToReducedAngle().ToRadians()) +
                0.000056 * Math.Sin(a8.ToReducedAngle().ToRadians()) +
                0.000047 * Math.Sin(a9.ToReducedAngle().ToRadians()) +
                0.000042 * Math.Sin(a10.ToReducedAngle().ToRadians()) +
                0.000040 * Math.Sin(a11.ToReducedAngle().ToRadians()) +
                0.000037 * Math.Sin(a12.ToReducedAngle().ToRadians()) +
                0.000035 * Math.Sin(a13.ToReducedAngle().ToRadians()) +
                0.000023 * Math.Sin(a14.ToReducedAngle().ToRadians());
        }

        /// <summary>
        /// Additional corrections to JDE in order to obtain 
        /// the true (apparent) phase of the new moon.
        /// </summary>
        /// <param name="E">Earth's eccentricity</param>
        /// <param name="Sm">Sun's mean anomaly (radians)</param>
        /// <param name="Mm">Moon's mean anomaly (radians)</param>
        /// <param name="F">Moon's argument of latitude (radians)</param>
        /// <param name="O">Longitude of the ascending node of the lunar orbit</param>
        /// <returns>Corrections for the new moon</returns>
        private static double ToNM(double E, double Sm, double Mm, double F, double O)
        {
            Sm = new Degrees(Sm).ToRadians();
            Mm = new Degrees(Mm).ToRadians();
            F = new Degrees(F).ToRadians();
            O = new Degrees(O).ToRadians();

            return
                (-0.40720 * Math.Sin(Mm)) +
                (0.17241 * E * Math.Sin(Sm)) +
                (0.01608 * Math.Sin(Mm * 2)) +
                (0.01039 * Math.Sin(F * 2)) +
                (0.00739 * E * Math.Sin(Mm - Sm)) -
                (0.00514 * E * Math.Sin(Mm + Sm)) +
                (0.00208 * E * E * Math.Sin(Sm * 2)) -
                (0.00111 * Math.Sin(Mm - 2 * F)) -
                (0.00057 * Math.Sin(Mm + 2 * F)) +
                (0.00056 * E * Math.Sin(2 * Mm + Sm)) -
                (0.00042 * Math.Sin(3 * Mm)) +
                (0.00042 * E * Math.Sin(Sm + 2 * F)) +
                (0.00038 * E * Math.Sin(Sm - 2 * F)) -
                (0.00024 * E * Math.Sin(2 * Mm - Sm)) -
                (0.00017 * Math.Sin(O)) -
                (0.00007 * Math.Sin(Mm + 2 * Sm)) +
                (0.00004 * Math.Sin(2 * Mm - 2 * F)) +
                (0.00004 * Math.Sin(3 * Sm)) +
                (0.00003 * Math.Sin(Mm + Sm - 2 * F)) +
                (0.00003 * Math.Sin(2 * Mm + 2 * F)) -
                (0.00003 * Math.Sin(Mm + Sm + 2 * F)) +
                (0.00003 * Math.Sin(Mm - Sm + 2 * F)) -
                (0.00002 * Math.Sin(Mm - Sm - 2 * F)) -
                (0.00002 * Math.Sin(3 * Mm + Sm)) +
                (0.00002 * Math.Sin(4 * Mm));
        }

        /// <summary>
        /// Additional corrections to JDE in order to obtain 
        /// the true (apparent) phase of the full moon.
        /// </summary>
        /// <param name="E">Earth's eccentricity</param>
        /// <param name="Sm">Sun's mean anomaly (radians)</param>
        /// <param name="Mm">Moon's mean anomaly (radians)</param>
        /// <param name="F">Moon's argument of latitude (radians)</param>
        /// <param name="O">Longitude of the ascending node of the lunar orbit</param>
        /// <returns>Corrections for the full moon</returns>
        private static double ToFM(double E, double Sm, double Mm, double F, double O)
        {
            Sm = new Degrees(Sm).ToRadians();
            Mm = new Degrees(Mm).ToRadians();
            F = new Degrees(F).ToRadians();
            O = new Degrees(O).ToRadians();

            return
                (-0.40614 * Math.Sin(Mm)) +
                (0.17302 * E * Math.Sin(Sm)) +
                (0.01614 * Math.Sin(Mm * 2)) +
                (0.01043 * Math.Sin(F * 2)) +
                (0.00734 * E * Math.Sin(Mm - Sm)) -
                (0.00515 * E * Math.Sin(Mm + Sm)) +
                (0.00209 * E * E * Math.Sin(Sm * 2)) -
                (0.00111 * Math.Sin(Mm - 2 * F)) -
                (0.00057 * Math.Sin(Mm + 2 * F)) +
                (0.00056 * E * Math.Sin(2 * Mm + Sm)) -
                (0.00042 * Math.Sin(3 * Mm)) +
                (0.00042 * E * Math.Sin(Sm + 2 * F)) +
                (0.00038 * E * Math.Sin(Sm - 2 * F)) -
                (0.00024 * E * Math.Sin(2 * Mm - Sm)) -
                (0.00017 * Math.Sin(O)) -
                (0.00007 * Math.Sin(Mm + 2 * Sm)) +
                (0.00004 * Math.Sin(2 * Mm - 2 * F)) +
                (0.00004 * Math.Sin(3 * Sm)) +
                (0.00003 * Math.Sin(Mm + Sm - 2 * F)) +
                (0.00003 * Math.Sin(2 * Mm + 2 * F)) -
                (0.00003 * Math.Sin(Mm + Sm + 2 * F)) +
                (0.00003 * Math.Sin(Mm - Sm + 2 * F)) -
                (0.00002 * Math.Sin(Mm - Sm - 2 * F)) -
                (0.00002 * Math.Sin(3 * Mm + Sm)) +
                (0.00002 * Math.Sin(4 * Mm));
        }

        /// <summary>
        /// Additional corrections to JDE in order to obtain 
        /// the true (apparent) phase of the first and last quarters.
        /// </summary>
        /// <param name="E">Earth's eccentricity</param>
        /// <param name="Sm">Sun's mean anomaly (radians)</param>
        /// <param name="Mm">Moon's mean anomaly (radians)</param>
        /// <param name="F">Moon's argument of latitude (radians)</param>
        /// <param name="O">Longitude of the ascending node of the lunar orbit</param>
        /// <returns>Corrections for the First and Last Quarters</returns>
        private static double ToFLQ(double E, double Sm, double Mm, double F, double O)
        {
            Sm = new Degrees(Sm).ToRadians();
            Mm = new Degrees(Mm).ToRadians();
            F = new Degrees(F).ToRadians();
            O = new Degrees(O).ToRadians();

            return
                (-0.62801 * Math.Sin(Mm)) +
                (0.17172 * E * Math.Sin(Sm)) -
                (0.01183 * E * Math.Sin(Mm + Sm)) +
                (0.00862 * Math.Sin(2 * Mm)) +
                (0.00804 * Math.Sin(2 * F)) +
                (0.00454 * E * Math.Sin(Mm - Sm)) +
                (0.00204 * E * E * Math.Sin(Sm * 2)) -
                (0.00180 * Math.Sin(Mm - 2 * F)) -
                (0.00070 * Math.Sin(Mm + 2 * F)) -
                (0.00040 * Math.Sin(3 * Mm)) -
                (0.00040 * Math.Sin(3 * Mm)) -
                (0.00034 * E * Math.Sin(2 * Mm - Sm)) +
                (0.00032 * E * Math.Sin(Sm + 2 * F)) +
                (0.00032 * E * Math.Sin(Sm - 2 * F)) -
                (0.00028 * E * E * Math.Sin(Mm + 2 * Sm)) +
                (0.00027 * E * Math.Sin(2 * Mm + Sm)) -
                (0.00017 * Math.Sin(O)) -
                (0.00005 * Math.Sin(Mm - Sm - 2 * F)) +
                (0.00004 * Math.Sin(2 * Mm + 2 * F)) -
                (0.00004 * Math.Sin(Mm + Sm + 2 * F)) +
                (0.00004 * Math.Sin(Mm - 2 * Sm)) +
                (0.00003 * Math.Sin(Mm + Sm - 2 * F)) +
                (0.00003 * Math.Sin(3 * Sm)) +
                (0.00002 * Math.Sin(2 * Mm - 2 * F)) +
                (0.00002 * Math.Sin(Mm - Sm + 2 * F)) -
                (0.00002 * Math.Sin(3 * Mm + Sm));
        }

        /// <summary>
        /// Additional correction W only for First and Last Quarters.
        /// </summary>
        /// <param name="E"></param>
        /// <param name="Sm"></param>
        /// <param name="Mm"></param>
        /// <param name="F"></param>
        /// <returns>correction W</returns>
        private static double ToFLQCorrection(double E, double Sm, double Mm, double F)
        {
            double W = 0.00306 -
                0.00038 * E * Math.Cos(Sm) +
                0.00026 * Math.Cos(Mm) -
                0.00002 * Math.Cos(Mm - Sm) +
                0.00002 * Math.Cos(Mm + Sm) +
                0.00002 * Math.Cos(F * 2);

            return W;
        }

        /// <summary>
        /// Given the corrected JDE for the new moon event adjust 
        /// for Delta T and return UTC
        /// </summary>
        /// <param name="correctedJDE">Corrected JDE</param>
        /// <returns></returns>
        private static DateTime ToDateTimeUTC(double correctedJDE)
        {
            // create a date from the JDE
            Moment moment = new Moment(correctedJDE);
            DateTime d = moment.ToDateTime();

            // convert JDE TD to UTC by subtracting ΔT
            TimeSpan ΔT = DeltaT.GetValue(d.Year);
            return d.AddSeconds(-ΔT.TotalSeconds);
        }

        /// <summary>
        /// Given a value of k in which the integral is in the range 0.0, 0.25, 0.50, or 0.75
        /// return the matching moon phase name.
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static string GetPhaseName(double k)
        {
            double trunc = k - Math.Truncate(k);
            switch (trunc)
            {
                case 0.0:
                    return PhaseName.NewMoon;

                case 0.25:
                    return PhaseName.FirstQuarter;

                case 0.50:
                    return PhaseName.FullMoon;

                case 0.75:
                    return PhaseName.LastQuarter;

                default:
                    throw new ArgumentOutOfRangeException("Variable k must be a valid phase of the moon.");
            }
        }
    }
}

