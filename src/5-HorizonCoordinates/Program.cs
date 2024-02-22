/*
    5-HorizonCoordinates

    Code sample to show how to work with horizontal coordinates (Alt-Az) and to
    convert between horizon and equitorial coordinates. Formulas are taken from 
    the book Celestial Calculations, J.L. Lawrence, p 89 (4.7.1 and 4.7.2) as 
    well as Jean Meeus, Astronomical Algorithms, (13.5) and (13.6).

    For calculations of GMST and LST see:
    https://squarewidget.com/astronomical-calculations-sidereal-time/

    To convert from horizontal to equitorial coordinates we need the object's 
    altitude (h), azimuth (A), and the latitude (φ) of the observer.

*/

using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

DateTime datetime = new(1987, 4, 10, 19, 21, 0);
Moment moment = new Moment(datetime);

// Earth's obliquity of the ecliptic
Earth earth = new(moment);
SexigesimalAngle ε = earth.MeanObliquity;

// U.S. Naval Observatory
SexigesimalAngle L = new(77, 3, 56);   // longitude
SexigesimalAngle φ = new(38, 55, 17); // latitude

// Venus apparent equatorial coordinates
RightAscension α = new(23, 9, 16.641);
SexigesimalAngle δ = new(-6, 43, 11.61);
EquatorialCoordinates eqc = new(δ, α, ε.ToDegrees());

SiderealTime st = new(moment);
RightAscension gmst = new(st.GreenwichMean);
RightAscension gast = new(st.GreenwichApparent);

Degrees H = st.ToHourAngle(L, α);
HorizontalCoordinates hc = new(moment, φ, L, eqc);

Console.WriteLine($"Azimuth A and altitude h of Venus on {datetime.ToString("g")} UTC from the U.S. Naval Observatory");
Console.WriteLine(string.Empty);
Console.WriteLine("Hour angle H: " + H.ToString());
Console.WriteLine("        Az A: " + hc.A.ToString());
Console.WriteLine("       Alt h: " + hc.h.ToString());



