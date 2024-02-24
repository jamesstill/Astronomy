/*
    5-HorizonCoordinates

    Code sample to show how to work with horizontal coordinates (Alt-Az) and to convert between 
    horizon and equitorial coordinates. Formulas are taken from the book Celestial Calculations, 
    J.L. Lawrence, p 89 (4.7.1 and 4.7.2) as well as Jean Meeus, Astronomical Algorithms, (13.5) 
    and (13.6), and conversion help from a Go implementation by Sonia Keys. See the file coord.go 
    at https://github.com/soniakeys/meeus

    For calculations of GMST and LST see my blog entry:
    https://squarewidget.com/astronomical-calculations-sidereal-time/

    To convert from horizontal to equitorial coordinates for a given moment in time we need the 
    object's altitude (h), azimuth (A), and the latitude (φ) of the observer. In this code sample 
    the observer is at the U.S. Naval Observatory on 1987 Apr 10 19:21 UTC where Venus is at known 
    equatorial coordinates. This sample is taken from Meeus, Example 13.b.

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
SexigesimalAngle φ = new(38, 55, 17);  // latitude

// Venus apparent equatorial coordinates
RightAscension α = new(23, 9, 16.641);
SexigesimalAngle δ = new(-6, 43, 11.61);
EquatorialCoordinates eqc = new(δ, α, ε.ToDegrees());

// construct the horizontal coordinates
HorizontalCoordinates hc = new(moment, φ, L, eqc);

// convert the horizontal coordinates back into equatorial coordinates
EquatorialCoordinates ec = hc.ToΕquatorialCoordinates(moment, φ, L);

Console.WriteLine($"Azimuth A and altitude h of Venus on {datetime.ToString("g")} UTC from the U.S. Naval Observatory");
Console.WriteLine(string.Empty);
Console.WriteLine("Venus α: " + α.ToString());
Console.WriteLine("Venus δ: " + δ.ToString());
Console.WriteLine(string.Empty);

Console.WriteLine("Calculate the horizontal coordinates for Venus...");
Console.WriteLine(string.Empty);
Console.WriteLine("Venus Az A : " + hc.A.ToString());
Console.WriteLine("Venus Alt h: " + hc.h.ToString());
Console.WriteLine(string.Empty);

Console.WriteLine("Venus horizontal coordinates converted back into equatorial coordinates...");
Console.WriteLine(string.Empty);
Console.WriteLine("Venus α: " + ec.α.ToString());
Console.WriteLine("Venus δ: " + ec.δ.ToString());

