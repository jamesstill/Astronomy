using SquareWidget.Astronomy.Core.Planets;
using System;
using System.Diagnostics;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct to hold horizontal coordinates (Alt-Az) and to 
    /// convert between horizontal and equitorial coordinates.
    /// </summary>
    public readonly struct HorizontalCoordinates
    {
        public readonly Degrees h;
        public readonly Degrees A;
        public readonly Degrees ε;

        /// <summary>
        /// Construct with known altitude h and azimuth A in decimal degrees
        /// </summary>
        /// <param name="h">Altitude h in decimal degrees</param>
        /// <param name="A">Azimuth A in decimal degrees</param>
        /// <param name="ε">Mean obliquity of the ecliptic (ε) in decimal degrees</param>
        public HorizontalCoordinates(double h, double A, double ε)
        {
            this.h = new Degrees(h);
            this.A = new Degrees(A);
            this.ε = new Degrees(ε);
        }

        /// <summary>
        /// Construct with known altitude h and azimuth A in degrees
        /// </summary>
        /// <param name="h">Altitude h in degrees</param>
        /// <param name="A">Azimuth A in degrees</param>
        /// <param name="ε">Mean obliquity of the ecliptic (ε) in degrees</param>
        public HorizontalCoordinates(Degrees h, Degrees A, Degrees ε)
        {
            this.h = h;
            this.A = A;
            this.ε = ε;
        }

        /// <summary>
        /// Construct given the observer's latitude (φ), longitude (L), 
        /// and the object's apparent equitorial coordinates following
        /// Jean Meeus (13.5) and (13.6). See also J. L. Lawrence (4.7.3)
        /// and (4.7.4).
        /// </summary>
        /// <param name="moment">Moment in time</param>
        /// <param name="φ">Observer's Latitude</param>
        /// <param name="L">Observer's Longitude</param>
        /// <param name="eqc">EquitorialCoordinates</param>
        public HorizontalCoordinates(Moment moment, SexigesimalAngle φ, SexigesimalAngle L, EquatorialCoordinates eqc)
        {
            SiderealTime siderealTime = new SiderealTime(moment);
            Radians H = siderealTime.ToHourAngle(L, eqc.α).ToRadians();
            Radians f = φ.ToRadians();
            double d = eqc.δ.ToDegrees().ToRadians();

            Radians h = new(Math.Asin(Math.Sin(f) * Math.Sin(d) + Math.Cos(f) * Math.Cos(d) * Math.Cos(H)));
            Radians A = new(Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(f) - Math.Tan(d) * Math.Cos(f)));

            this.h = new Degrees(h); 
            this.A = new Degrees(A);
            ε = eqc.ε.ToDegrees();
        }

        public static implicit operator double(HorizontalCoordinates hc) => hc;

        /// <summary>
        /// Convert the horizontal coordinates to equatorial coordinates. A part of this 
        /// implementation was ported to C# from a Go library by Sonia Keys. See coord.go
        /// at https://github.com/soniakeys/meeus
        /// </summary>
        /// <param name="moment"></param>
        /// <param name="φ">Observer's Latitude</param>
        /// <param name="L">Observer's Longitude</param>
        /// <returns>EquatorialCoordinates</returns>
        /// <exception cref="NotImplementedException"></exception>
        public EquatorialCoordinates ToΕquatorialCoordinates(Moment moment, SexigesimalAngle φ, SexigesimalAngle L) 
        {
            Radians f = φ.ToRadians();
            Radians hd = h.ToRadians();
            Radians Ad = A.ToRadians();

            // declination
            Radians d = new(Math.Asin(Math.Sin(f) * Math.Sin(hd) - Math.Cos(f) * Math.Cos(hd) * Math.Cos(Ad)));
            
            // hour angle
            Radians H = new(Math.Atan2(Math.Sin(Ad), Math.Cos(Ad) * Math.Sin(f) + Math.Tan(hd) * Math.Cos(f)));

            SiderealTime siderealTime = new SiderealTime(moment);
            Degrees gmst = siderealTime.GreenwichMean;
            
            Radians r = new(gmst.ToRadians() - L.ToRadians() - H);
            double rad = r.ToDegrees();
            if (rad < 0)
            {
                rad += 360.0;
            }

            Degrees ra = new(rad);
            RightAscension α = new(ra);
            SexigesimalAngle δ = new(d.ToDegrees()); 

            return new EquatorialCoordinates(δ, α, ε);
        }
    }
}
