using System;
using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

/*
 * The lunar periodic series follows Meeus, Astronomical Algorithms, 2nd edition, Chapter 47,
 * and the reference implementation by Sonia Keys at https://github.com/soniakeys/meeus.
 * Copyright (c) 2013 Sonia Keys
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace SquareWidget.Astronomy.Core.CelestialObjects.Moons
{
    public class Moon
    {
        private readonly Moment _moment;
        private readonly EclipticalCoordinates _geocentricEclipticalCoordinates;
        private readonly EquitorialCoordinates _geocentricEquitorialCoordinates;
        private readonly EclipticalCoordinates _apparentEclipticalCoordinates;
        private readonly EquitorialCoordinates _apparentEquitorialCoordinates;
        private readonly double _geocentricDistanceKilometers;
        private readonly Degrees _horizontalParallax;
        private readonly Nutation _nutation;
        private const double SynodicMonthsPerJulianCentury = 1236.85;

        public Moon(Moment moment)
        {
            _moment = moment;
            Moment terrestrialTime = moment.ToTerrestrialTime();

            // Fetch geocentric coordinates and distance at TT, as required by the lunar theory.
            (double longitudeDegrees, double latitudeDegrees, double distanceKilometers) = CalculateGeocentricPosition(terrestrialTime);

            Degrees obliquity = new Earth(terrestrialTime).MeanObliquity.ToDegrees();

            _geocentricEclipticalCoordinates = new EclipticalCoordinates(
                new Degrees(longitudeDegrees).ToRadians(),
                new Degrees(latitudeDegrees).ToRadians(),
                obliquity.ToRadians());

            _geocentricEquitorialCoordinates = new EquitorialCoordinates(_geocentricEclipticalCoordinates);

            _nutation = NutationCalculator.Calculate(terrestrialTime.ToDateTime());

            double apparentLongitudeDegrees = longitudeDegrees + _nutation.ΔΨ.ToDecimalDegrees();
            _apparentEclipticalCoordinates = new EclipticalCoordinates(
                new Degrees(apparentLongitudeDegrees).ToRadians(),
                new Degrees(latitudeDegrees).ToRadians(),
                _nutation.ε.ToDegrees().ToRadians());

            _apparentEquitorialCoordinates = new EquitorialCoordinates(_apparentEclipticalCoordinates);

            _geocentricDistanceKilometers = distanceKilometers;

            _horizontalParallax = new Degrees(Math.Asin(6378.14 / distanceKilometers) * (180.0 / Math.PI));
        }

        /// <summary>
        /// Geocentric ecliptic longitude and latitude, referred to the mean equinox of date.
        /// </summary>
        public EclipticalCoordinates GeocentricEclipticalCoordinates => _geocentricEclipticalCoordinates;

        /// <summary>
        /// Geocentric equatorial right ascension and declination, referred to the mean equinox of date.
        /// </summary>
        public EquitorialCoordinates GeocentricEquitorialCoordinates => _geocentricEquitorialCoordinates;

        /// <summary>
        /// Apparent geocentric ecliptic longitude and latitude, corrected for nutation.
        /// </summary>
        public EclipticalCoordinates ApparentEclipticalCoordinates => _apparentEclipticalCoordinates;

        /// <summary>
        /// Apparent geocentric equatorial right ascension and declination, corrected for nutation.
        /// </summary>
        public EquitorialCoordinates ApparentEquitorialCoordinates => _apparentEquitorialCoordinates;

        /// <summary>
        /// Nutation used to derive the apparent coordinates.
        /// </summary>
        public Nutation Nutation => _nutation;

        /// <summary>
        /// Geocentric distance between the centers of Earth and Moon, in kilometers.
        /// </summary>
        public double GeocentricDistanceKilometers => _geocentricDistanceKilometers;

        /// <summary>
        /// Equatorial horizontal parallax derived from the geocentric distance.
        /// </summary>
        public Degrees HorizontalParallax => _horizontalParallax;

        /// <summary>
        /// Baseline value k where k = 0 is 6 Jan 2000 (first new moon of J2000.0 epoch).
        /// </summary>
        /// <returns></returns>
        public int FirstNewMoonOfJ2000Epoch
        {
            get 
            {
                double decimalYear = (_moment.ToDecimalYear() - 2000) * 12.3685;
                return (int)Math.Round(decimalYear);
            }
        }

        /// <summary>
        /// Moon's mean anomaly at time JDE
        /// </summary>
        /// <returns>M′</returns>
        public Degrees MeanAnomaly
        {
            get
            {
                int k = FirstNewMoonOfJ2000Epoch;
                double T = k / SynodicMonthsPerJulianCentury;

                // Meeus (49.5)
                double M = 201.5643 +
                    (385.81693528 * k) +
                    (0.0107582 * (T * T)) +
                    (0.00001238 * (T * T * T)) -
                    (0.000000058 * (T * T * T * T));

                return new Degrees(M).ToReducedAngle();
            }
        }

        /// <summary>
        /// Moon's argument of latitude
        /// </summary>
        /// <returns>F</returns>
        public Degrees ArgumentOfLatitude
        {
            get {
                int k = FirstNewMoonOfJ2000Epoch;
                double T = k / SynodicMonthsPerJulianCentury;

                double F = 160.7108 +
                    (390.67050284 * k) -
                    (0.0016118 * (T * T)) -
                    (0.00000227 * (T * T * T)) +
                    (0.000000011 * (T * T * T * T));

                return new Degrees(F).ToReducedAngle();
            }
        }

        /// <summary>
        /// Longitude of the ascending node of the lunar orbit
        /// </summary>
        /// <returns>Ω</returns>
        public Degrees LongitudeOfAscendingNode
        {
            get
            {
                int k = FirstNewMoonOfJ2000Epoch;
                double T = k / SynodicMonthsPerJulianCentury;

                double L = 124.7746 -
                    (1.56375588 * k) +
                    (0.0020672 * (T * T)) +
                    (0.00000215 * (T * T * T));

                return new Degrees(L).ToReducedAngle();
            }
        }

        /// <summary>
        /// Calculates the Moon's geocentric ecliptic position and distance.
        /// </summary>
        /// <param name="moment">The time for which to calculate the position.</param>
        /// <returns>The ecliptic longitude and latitude in degrees and the distance in kilometers.</returns>
        private static (double LongitudeDegrees, double LatitudeDegrees, double DistanceKilometers) CalculateGeocentricPosition(Moment moment)
        {
            double t = moment.T;
            double lPrime = ToRadians(Polynomial(t, 218.3164477, 481267.88123421, -0.0015786, 1.0 / 538841, -1.0 / 65194000));
            double d = ToRadians(Polynomial(t, 297.8501921, 445267.1114034, -0.0018819, 1.0 / 545868, -1.0 / 113065000));
            double m = ToRadians(Polynomial(t, 357.5291092, 35999.0502909, -0.0001535, 1.0 / 24490000));
            double mPrime = ToRadians(Polynomial(t, 134.9633964, 477198.8675055, 0.0087414, 1.0 / 69699, -1.0 / 14712000));
            double f = ToRadians(Polynomial(t, 93.2720950, 483202.0175233, -0.0036539, -1.0 / 3526000, 1.0 / 863310000));
            double a1 = ToRadians(119.75 + 131.849 * t);
            double a2 = ToRadians(53.09 + 479264.29 * t);
            double a3 = ToRadians(313.45 + 481266.484 * t);
            double eccentricity = 1.0 - 0.002516 * t - 0.0000074 * t * t;

            double longitudeSum = 3958.0 * Math.Sin(a1)
                + 1962.0 * Math.Sin(lPrime - f)
                + 318.0 * Math.Sin(a2);
            double radiusSum = 0.0;
            double latitudeSum = -2235.0 * Math.Sin(lPrime)
                + 382.0 * Math.Sin(a3)
                + 175.0 * Math.Sin(a1 - f)
                + 175.0 * Math.Sin(a1 + f)
                + 127.0 * Math.Sin(lPrime - mPrime)
                - 115.0 * Math.Sin(lPrime + mPrime);

            foreach (LunarLongitudeRadiusTerm term in LongitudeRadiusTerms)
            {
                double argument = term.D * d + term.M * m + term.MPrime * mPrime + term.F * f;
                double eccentricityFactor = Math.Pow(eccentricity, Math.Abs(term.M));
                longitudeSum += term.LongitudeCoefficient * Math.Sin(argument) * eccentricityFactor;
                radiusSum += term.RadiusCoefficient * Math.Cos(argument) * eccentricityFactor;
            }

            foreach (LunarLatitudeTerm term in LatitudeTerms)
            {
                double argument = term.D * d + term.M * m + term.MPrime * mPrime + term.F * f;
                latitudeSum += term.Coefficient * Math.Sin(argument) * Math.Pow(eccentricity, Math.Abs(term.M));
            }

            double longitude = lPrime + ToRadians(longitudeSum / 1_000_000.0);
            longitude %= 2.0 * Math.PI;
            if (longitude < 0.0)
            {
                longitude += 2.0 * Math.PI;
            }

            return (
                longitude * (180.0 / Math.PI),
                latitudeSum / 1_000_000.0,
                385000.56 + radiusSum / 1000.0);
        }

        private static double Polynomial(double value, params double[] coefficients)
        {
            double result = coefficients[^1];
            for (int i = coefficients.Length - 2; i >= 0; i--)
            {
                result = result * value + coefficients[i];
            }

            return result;
        }

        private static double ToRadians(double degrees) => degrees * (Math.PI / 180.0);

        private static readonly LunarLongitudeRadiusTerm[] LongitudeRadiusTerms =
        {
            new(0, 0, 1, 0, 6288774, -20905355), new(2, 0, -1, 0, 1274027, -3699111),
            new(2, 0, 0, 0, 658314, -2955968), new(0, 0, 2, 0, 213618, -569925),
            new(0, 1, 0, 0, -185116, 48888), new(0, 0, 0, 2, -114332, -3149),
            new(2, 0, -2, 0, 58793, 246158), new(2, -1, -1, 0, 57066, -152138),
            new(2, 0, 1, 0, 53322, -170733), new(2, -1, 0, 0, 45758, -204586),
            new(0, 1, -1, 0, -40923, -129620), new(1, 0, 0, 0, -34720, 108743),
            new(0, 1, 1, 0, -30383, 104755), new(2, 0, 0, -2, 15327, 10321),
            new(0, 0, 1, 2, -12528, 0), new(0, 0, 1, -2, 10980, 79661),
            new(4, 0, -1, 0, 10675, -34782), new(0, 0, 3, 0, 10034, -23210),
            new(4, 0, -2, 0, 8548, -21636), new(2, 1, -1, 0, -7888, 24208),
            new(2, 1, 0, 0, -6766, 30824), new(1, 0, -1, 0, -5163, -8379),
            new(1, 1, 0, 0, 4987, -16675), new(2, -1, 1, 0, 4036, -12831),
            new(2, 0, 2, 0, 3994, -10445), new(4, 0, 0, 0, 3861, -11650),
            new(2, 0, -3, 0, 3665, 14403), new(0, 1, -2, 0, -2689, -7003),
            new(2, 0, -1, 2, -2602, 0), new(2, -1, -2, 0, 2390, 10056),
            new(1, 0, 1, 0, -2348, 6322), new(2, -2, 0, 0, 2236, -9884),
            new(0, 1, 2, 0, -2120, 5751), new(0, 2, 0, 0, -2069, 0),
            new(2, -2, -1, 0, 2048, -4950), new(2, 0, 1, -2, -1773, 4130),
            new(2, 0, 0, 2, -1595, 0), new(4, -1, -1, 0, 1215, -3958),
            new(0, 0, 2, 2, -1110, 0), new(3, 0, -1, 0, -892, 3258),
            new(2, 1, 1, 0, -810, 2616), new(4, -1, -2, 0, 759, -1897),
            new(0, 2, -1, 0, -713, -2117), new(2, 2, -1, 0, -700, 2354),
            new(2, 1, -2, 0, 691, 0), new(2, -1, 0, -2, 596, 0),
            new(4, 0, 1, 0, 549, -1423), new(0, 0, 4, 0, 537, -1117),
            new(4, -1, 0, 0, 520, -1571), new(1, 0, -2, 0, -487, -1739),
            new(2, 1, 0, -2, -399, 0), new(0, 0, 2, -2, -381, -4421),
            new(1, 1, 1, 0, 351, 0), new(3, 0, -2, 0, -340, 0),
            new(4, 0, -3, 0, 330, 0), new(2, -1, 2, 0, 327, 0),
            new(0, 2, 1, 0, -323, 1165), new(1, 1, -1, 0, 299, 0),
            new(2, 0, 3, 0, 294, 0), new(2, 0, -1, -2, 0, 8752)
        };

        private static readonly LunarLatitudeTerm[] LatitudeTerms =
        {
            new(0, 0, 0, 1, 5128122), new(0, 0, 1, 1, 280602), new(0, 0, 1, -1, 277693),
            new(2, 0, 0, -1, 173237), new(2, 0, -1, 1, 55413), new(2, 0, -1, -1, 46271),
            new(2, 0, 0, 1, 32573), new(0, 0, 2, 1, 17198), new(2, 0, 1, -1, 9266),
            new(0, 0, 2, -1, 8822), new(2, -1, 0, -1, 8216), new(2, 0, -2, -1, 4324),
            new(2, 0, 1, 1, 4200), new(2, 1, 0, -1, -3359), new(2, -1, -1, 1, 2463),
            new(2, -1, 0, 1, 2211), new(2, -1, -1, -1, 2065), new(0, 1, -1, -1, -1870),
            new(4, 0, -1, -1, 1828), new(0, 1, 0, 1, -1794), new(0, 0, 0, 3, -1749),
            new(0, 1, -1, 1, -1565), new(1, 0, 0, 1, -1491), new(0, 1, 1, 1, -1475),
            new(0, 1, 1, -1, -1410), new(0, 1, 0, -1, -1344), new(1, 0, 0, -1, -1335),
            new(0, 0, 3, 1, 1107), new(4, 0, 0, -1, 1021), new(4, 0, -1, 1, 833),
            new(0, 0, 1, -3, 777), new(4, 0, -2, 1, 671), new(2, 0, 0, -3, 607),
            new(2, 0, 2, -1, 596), new(2, -1, 1, -1, 491), new(2, 0, -2, 1, -451),
            new(0, 0, 3, -1, 439), new(2, 0, 2, 1, 422), new(2, 0, -3, -1, 421),
            new(2, 1, -1, 1, -366), new(2, 1, 0, 1, -351), new(4, 0, 0, 1, 331),
            new(2, -1, 1, 1, 315), new(2, -2, 0, -1, 302), new(0, 0, 1, 3, -283),
            new(2, 1, 1, -1, -229), new(1, 1, 0, -1, 223), new(1, 1, 0, 1, 223),
            new(0, 1, -2, -1, -220), new(2, 1, -1, -1, -220), new(1, 0, 1, 1, -185),
            new(2, -1, -2, -1, 181), new(0, 1, 2, 1, -177), new(4, 0, -2, -1, 176),
            new(4, -1, -1, -1, 166), new(1, 0, 1, -1, -164), new(4, 0, 1, -1, 132),
            new(1, 0, -1, -1, -119), new(4, -1, 0, -1, 115), new(2, -2, 0, 1, 107)
        };

        private readonly struct LunarLongitudeRadiusTerm
        {
            public LunarLongitudeRadiusTerm(int d, int m, int mPrime, int f, double longitudeCoefficient, double radiusCoefficient)
            {
                D = d;
                M = m;
                MPrime = mPrime;
                F = f;
                LongitudeCoefficient = longitudeCoefficient;
                RadiusCoefficient = radiusCoefficient;
            }

            public int D { get; }
            public int M { get; }
            public int MPrime { get; }
            public int F { get; }
            public double LongitudeCoefficient { get; }
            public double RadiusCoefficient { get; }
        }

        private readonly struct LunarLatitudeTerm
        {
            public LunarLatitudeTerm(int d, int m, int mPrime, int f, double coefficient)
            {
                D = d;
                M = m;
                MPrime = mPrime;
                F = f;
                Coefficient = coefficient;
            }

            public int D { get; }
            public int M { get; }
            public int MPrime { get; }
            public int F { get; }
            public double Coefficient { get; }
        }
    }
}
