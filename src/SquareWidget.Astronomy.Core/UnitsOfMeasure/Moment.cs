using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// A moment is a specific point in time down to the optional millisecond.
    /// 
    /// TODO: The caller must tell us if the moment is UT or TD because:
    ///       1977 APR 26.4 UT ==  JD 2443 259.9
    ///       1977 APR 26.4 TD == JDE 2443 259.9
    ///       
    /// See Meeus p. 59
    /// </summary>
    public readonly struct Moment
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
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <param name="millisecond"></param>
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
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public Moment(int year, int month, double day)
        {
            if (double.IsNaN(day))
            {
                throw new ArgumentException("day must be a valid value.");
            }

            Year = year;
            Month = month;

            // leverage the C# TimeSpan struct
            var ts = TimeSpan.FromDays(day);
            Day = ts.Days;
            Hour = ts.Hours;
            Minute = ts.Minutes;
            Second = ToRoundedSeconds(ts);
            Millisecond = 0;
        }

        /// <summary>
        /// Creates a Moment with a known Julian Day (JD) value.
        /// </summary>
        /// <param name="julianDay"></param>
        public Moment(double julianDay)
        {
            if (double.IsNaN(julianDay))
            {
                throw new ArgumentException("julianDay must be a valid value.");
            }

            double A;
            double B;
            int C;
            double D;
            int E;
            double jd = julianDay + 0.5;
            double Z = Math.Truncate(jd);
            double F = jd - Math.Truncate(jd);

            if (Z < 2299161)
            {
                A = Z;
            }
            else
            {
                var a = (int)((Z - 1867216.25) / 36524.25);
                A = Z + 1 + a - a / 4;
            }

            B = A + 1524;
            C = (int)((B - 122.1) / 365.25);
            D = (int)(365.25 * C);
            E = (int)((B - D) / 30.6001);

            int Y;
            int M;
            double day = B - D - (int)(30.6001 * E) + F;
            switch (E)
            {
                case 14:
                case 15:
                    M = E - 13;
                    break;

                default:
                    M = E - 1;
                    break;
            }

            switch (M)
            {
                case 1:
                case 2:
                    Y = C - 4715;
                    break;

                default:
                    Y = C - 4716;
                    break;
            }

            Year = Y;
            Month = M;

            // leverage the C# TimeSpan struct
            var ts = TimeSpan.FromDays(day);
            Day = ts.Days;
            Hour = ts.Hours;
            Minute = ts.Minutes;
            Second = ts.Seconds;
            Millisecond = ts.Milliseconds;
        }

        // factory constructor
        public static Moment GetDefaultUtcNowMoment()
        {
            return new Moment(DateTime.UtcNow);
        }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
        }

        public double DayOfMonth
        {
            get
            {
                return Day + Hour / 24.0 + Minute / 1440.0 + (Second + Millisecond / 1000.0) / 86400.0;
            }
        }

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

                // if the date is Jan or Feb then it is considered to be in the 
                // 13th or 14th month of the preceding year.
                switch (M)
                {
                    case 1:
                    case 2:
                        Y -= 1;
                        M += 12;
                        break;

                    default:
                        break;
                }

                if (!IsJulianDate()) // convert to Gregorian calendar
                {
                    double A = Math.Floor(Y / 100.0);
                    B = 2 - A + Math.Floor(A / 4);
                }

                return Math.Floor(365.25 * (Y + 4716)) + Math.Floor(30.6001 * (M + 1)) + DayOfMonth + B - 1524.5;
            }
        }

        /// <summary>
        /// Modified Julian Day which begins at Greenwich mean midnight
        /// </summary>
        public double MDJ
        {
            get
            {
                return JDE - 2400000.5;
            }
        }

        /// <summary>
        /// Time T is measured in Julian centuries of 36525 ephemeris days from the epoch J2000.0.
        /// Note: the VSOP planetary theory uses Julian millennia (365250) instead of centuries.
        /// </summary>
        public double T
        {
            get
            {
                return (JDE - 2451545.0) / 36525.0;
            }
        }

        /// <summary>
        /// Day D is the number of days (and decimals thereof) from the epoch J2000.0
        /// </summary>
        public double DayD
        {
            get
            {
                return JDE - 2451545.0;
            }
        }

        /// <summary>
        /// Pope Gregory introduced the Gregorian calendar in October 1582 when the 
        /// calendar had drifted 10 days. Dates prior to 4 Oct 1582 are Julian dates
        /// and dates after 15 Oct 1582 are Gregorian dates. Any date in the gap is
        /// invalid on the Gregorian calendar.
        /// </summary>
        /// <returns></returns>
        public bool IsJulianDate()
        {
            if (Year > 1582)
                return false;

            if (Year < 1582)
                return true;

            // year is 1582 so check month
            if (Month > 10)
                return false;

            if (Month < 10)
                return true;

            // month is 10 so check days
            if (Day > 14)
                return false;

            return true;
        }

        public bool IsGregorianDate()
        {
            if (Year > 1582)
                return true;

            if (Year < 1582)
                return false;

            // year is 1582 so check month
            if (Month > 10)
                return true;

            if (Month < 10)
                return false;

            // month is 10 so check days
            if (Day > 5)
                return true;

            return false;
        }

        /// <summary>
        /// Given a TimeSpan with a Second value like 23 and a Millisecond value like 999
        /// return rounded value where 23.999 == 24 or 23.499 == 23.
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static int ToRoundedSeconds(TimeSpan ts)
        {
            // time might be 19:26:23.999 so round up seconds
            string value = ts.Seconds + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSepar‌​ator + ts.Milliseconds;
            double seconds = double.Parse(value);
            return (int)Math.Round(seconds);
        }
    
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is Moment)) 
                return false;

            Moment moment = (Moment)obj;
            return moment.JDE == JDE;
        }

        public override string ToString()
        {
            DateTime dt = new DateTime(Year, Month, Day, Hour, Minute, Second, DateTimeKind.Utc);
            return dt.ToString("u");
        }

        public static bool operator ==(Moment m1, Moment m2)
        {
            return m1.JDE == m2.JDE;
        }

        public static bool operator !=(Moment m1, Moment m2)
        {
            return m1.JDE != m2.JDE;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
