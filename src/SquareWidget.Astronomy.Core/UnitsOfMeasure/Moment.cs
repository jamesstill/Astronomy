using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// A moment is a specific point in time down to the optional millisecond.
    /// Calculators assume that the passed in date is universal time.
    /// </summary>
    public readonly struct Moment : IEquatable<Moment>
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public int Hour { get; }
        public int Minute { get; }
        public int Second { get; }
        public int Millisecond { get; }

        public Moment(DateTime dt)
        {
            Year = dt.Year;
            Month = dt.Month;
            Day = dt.Day;
            Hour = dt.Hour;
            Minute = dt.Minute;
            Second = dt.Second;
            Millisecond = dt.Millisecond;
        }

        /// <summary>
        /// Creates a Moment with known values down to the millisecond.
        /// </summary>
        public Moment(int year, int month, int day, int hour, int minute, int second = 0, int millisecond = 0)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
        }

        /// <summary>
        /// Creates a Moment with known values using a fraction to represent the day
        /// of the month as well as the hour, minute, second, and millisecond component.
        /// For example, 4.812 is the 4th day at 19:29 (7:29 PM).
        /// </summary>
        public Moment(int year, int month, double day)
        {
            if (double.IsNaN(day))
            {
                throw new ArgumentException("day must be a valid value.", nameof(day));
            }

            Year = year;
            Month = month;

            // Round the TimeSpan directly so small milliseconds aren't inflated
            // and 59.999s rollovers cascade cleanly into Minute/Hour/Day.
            var ts = TimeSpan.FromSeconds(Math.Round(TimeSpan.FromDays(day).TotalSeconds));
            Day = ts.Days;
            Hour = ts.Hours;
            Minute = ts.Minutes;
            Second = ts.Seconds;
            Millisecond = 0;
        }

        /// <summary>
        /// Creates a Moment with a known Julian Day (JD) value.
        /// </summary>
        public Moment(double julianDay)
        {
            if (double.IsNaN(julianDay))
            {
                throw new ArgumentException("julianDay must be a valid value.", nameof(julianDay));
            }

            double jd = julianDay + 0.5;
            double Z = Math.Truncate(jd);
            double F = jd - Z;

            double A;
            if (Z < 2299161)
            {
                A = Z;
            }
            else
            {
                int a = (int)((Z - 1867216.25) / 36524.25);
                A = Z + 1 + a - a / 4;
            }

            double B = A + 1524;
            int C = (int)((B - 122.1) / 365.25);
            double D = (int)(365.25 * C);
            int E = (int)((B - D) / 30.6001);

            double day = B - D - (int)(30.6001 * E) + F;

            int M = E switch
            {
                14 or 15 => E - 13,
                _ => E - 1
            };

            int Y = M switch
            {
                1 or 2 => C - 4715,
                _ => C - 4716
            };

            Year = Y;
            Month = M;

            var ts = TimeSpan.FromDays(day);
            Day = ts.Days;
            Hour = ts.Hours;
            Minute = ts.Minutes;
            Second = ts.Seconds;
            Millisecond = ts.Milliseconds;
        }

        public static Moment GetDefaultUtcNowMoment() => new Moment(DateTime.UtcNow);

        /// <summary>
        /// Converts calendar date to decimal date.
        /// </summary>
        public double ToDecimalYear()
        {
            DateTime d = ToDateTime();
            double daysInYear = DateTime.IsLeapYear(Year) ? 366.0 : 365.0;
            double elapsedDays = (d.DayOfYear - 1) + (d.TimeOfDay.TotalSeconds / 86400.0);
            return Year + (elapsedDays / daysInYear);
        }

        public DateTime ToDateTime()
        {
            if (Year < 1 || Year > 9999)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(Year),
                    $"Year {Year} cannot be represented as a System.DateTime (valid range: 1–9999).");
            }

            return new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond, DateTimeKind.Utc);
        }

        /// <summary>
        /// Converts this UTC moment to an estimated Terrestrial Time (TT) moment.
        /// The conversion uses the library's year-based ΔT estimate (TT−UT1) and treats
        /// UTC as UT1, so it is approximate and should not be used for precision timing.
        /// </summary>
        public Moment ToTerrestrialTime()
        {
            double deltaTDays = DeltaT.GetValue(Year).TotalDays;
            return new Moment(JDE + deltaTDays);
        }

        public double DayOfMonth =>
            Day + Hour / 24.0 + Minute / 1440.0 + (Second + Millisecond / 1000.0) / 86400.0;

        /// <summary>
        /// A Julian Day (JD) is a continuous count of days and fractions thereof
        /// starting at 1 Jan -4712 at noon UTC to a given point in time thereafter.
        /// </summary>
        public double JDE
        {
            get
            {
                int Y = Year;
                int M = Month;
                double B = 0; // Julian calendar default

                if (M == 1 || M == 2)
                {
                    Y -= 1;
                    M += 12;
                }

                if (IsGregorianDate())
                {
                    double A = Math.Floor(Y / 100.0);
                    B = 2 - A + Math.Floor(A / 4.0);
                }

                return Math.Floor(365.25 * (Y + 4716))
                     + Math.Floor(30.6001 * (M + 1))
                     + DayOfMonth + B - 1524.5;
            }
        }

        /// <summary>
        /// Modified Julian Day which begins at Greenwich mean midnight.
        /// </summary>
        public double MJD => JDE - 2400000.5;

        /// <summary>
        /// Time T is measured in Julian centuries of 36525 ephemeris days from the epoch J2000.0.
        /// Note: the VSOP planetary theory uses Julian millennia (365250) instead of centuries.
        /// </summary>
        public double T => (JDE - 2451545.0) / 36525.0;

        /// <summary>
        /// Day D is the number of days (and decimals thereof) from the epoch J2000.0.
        /// </summary>
        public double DayD => JDE - 2451545.0;

        /// <summary>
        /// Pope Gregory introduced the Gregorian calendar in October 1582 when the
        /// calendar had drifted 10 days. Dates on or before 4 Oct 1582 are Julian dates
        /// and dates on or after 15 Oct 1582 are Gregorian dates. Any date in the gap
        /// (5–14 Oct 1582) returns false for both.
        /// </summary>
        public bool IsJulianDate()
        {
            if (Year > 1582) return false;
            if (Year < 1582) return true;
            if (Month > 10) return false;
            if (Month < 10) return true;
            return Day <= 4;
        }

        public bool IsGregorianDate()
        {
            if (Year > 1582) return true;
            if (Year < 1582) return false;
            if (Month > 10) return true;
            if (Month < 10) return false;
            return Day >= 15;
        }

        /// <summary>
        /// Given a TimeSpan with a Second value like 23 and a Millisecond value like 999
        /// return rounded value where 23.999 == 24 or 23.499 == 23.
        /// </summary>
        public static int ToRoundedSeconds(TimeSpan ts)
        {
            return (int)Math.Round(ts.Seconds + (ts.Milliseconds / 1000.0));
        }

        public bool Equals(Moment other) => JDE.Equals(other.JDE);

        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Moment other && Equals(other);

        public override string ToString()
        {
            string yearStr = Year < 0
                ? $"-{Math.Abs(Year):D4}"
                : $"{Year:D4}";

            return string.Create(
                CultureInfo.InvariantCulture,
                $"{yearStr}-{Month:D2}-{Day:D2} {Hour:D2}:{Minute:D2}:{Second:D2}.{Millisecond:D3}Z");
        }

        public static bool operator ==(Moment m1, Moment m2) => m1.Equals(m2);

        public static bool operator !=(Moment m1, Moment m2) => !m1.Equals(m2);

        public override int GetHashCode() => JDE.GetHashCode();
    }
}