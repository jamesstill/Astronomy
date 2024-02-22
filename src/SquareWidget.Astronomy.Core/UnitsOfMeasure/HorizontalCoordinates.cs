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

        /// <summary>
        /// Construct with known altitude h and azimuth A in decimal degrees
        /// </summary>
        /// <param name="h">Altitude h in decimal degrees</param>
        /// <param name="A">Azimuth A in decimal degrees</param>
        public HorizontalCoordinates(double h, double A)
        {
            this.h = new Degrees(h);
            this.A = new Degrees(A);
        }
        
        /// <summary>
        /// Construct with known altitude h and azimuth A in degrees
        /// </summary>
        /// <param name="h"></param>
        /// <param name="A"></param>
        public HorizontalCoordinates(Degrees h, Degrees A)
        {
            this.h = h;
            this.A = A;
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

            double h = Math.Asin(Math.Sin(f) * Math.Sin(d) + Math.Cos(f) * Math.Cos(d) * Math.Cos(H));
            double A = Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(f) - Math.Tan(d) * Math.Cos(f));

            this.h = new Radians(h).ToDegrees(); 
            this.A = new Radians(A).ToDegrees();
        }

        public static implicit operator double(HorizontalCoordinates hc) => hc;

        public EquatorialCoordinates ToΕquitorialCoordinates() 
        {
            // TODO: p. 94





            throw new NotImplementedException("TODO");
        }
    }
}
