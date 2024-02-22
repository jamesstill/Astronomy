/*
    1-SolarCoordinates

    Code sample to show how to obtain the geocentric position of the Sun
    referred to the mean equinox of the date. Calculations are from Jean
    Meeus, Astronomical Algorithms, 2nd Ed., Chapter 25.

*/

using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using SquareWidget.Astronomy.Core.CelestialObjects.Stars;

DateTime d = new(1992, 10, 13);
Moment moment = new(d);
Sun sun = new(moment);

EquatorialCoordinates ec = sun.GetGeocentricPosition();

Console.WriteLine($"On {d.ToShortDateString()} the apparent position of the Sun was at RA {ec.α.ToString()} and Dec {ec.δ.ToString()}.");

SexigesimalAngle meanLongitude = new(sun.MeanLongitude);
SexigesimalAngle trueLongitude = new(sun.TrueLongitude);

Console.WriteLine("");
Console.WriteLine("Other elements...");
Console.WriteLine("");
Console.WriteLine($"Mean anomaly: {sun.MeanAnomaly.ToString()}");
Console.WriteLine($"True anomaly: {sun.TrueAnomaly.ToString()}");
Console.WriteLine($"Mean longitude: {meanLongitude.ToString()}");
Console.WriteLine($"True longitude: {trueLongitude.ToString()}");
Console.WriteLine($"Radius vector: {sun.RadiusVector.ToString()} AU");
Console.WriteLine($"Equation of Center: {sun.EquationOfCenter.ToString()}");


