/*
    7-TopocentricParallax

    Code sample to show how to convert geocentric equatorial coordinates 
    to topocentric equatorial coordinates using the TopocentricParallax class. 
    The horizontal parallax specifies the object's distance, and the observer's 
    longitude is east-positive.

*/

using SquareWidget.Astronomy.Core.CelestialObjects.Moons;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

Moment moment = new(2024, 1, 1, 0, 0);      // UTC

// Observer location: U.S. Naval Observatory
SexigesimalAngle φ = new(38, 55, 17);       // latitude
SexigesimalAngle L = new(-77, 3, 56);       // east-positive longitude
double elevationMeters = 81.0;              // elevation in meters

Degrees latitude = φ.ToDegrees();
Degrees longitude = L.ToDegrees();

Moon moon = new(moment);

// Meeus-based approximation: Moon converts UTC to estimated TT using the year-based ΔT estimate.
// This example uses apparent equatorial coordinates with nutation applied.
EquitorialCoordinates topocentric = TopocentricParallax.ToTopocentric(
    moon.ApparentEquitorialCoordinates,
    moment,
    latitude,
    longitude,
    elevationMeters,
    moon.HorizontalParallax);

Console.WriteLine($"Apparent geocentric RA: {moon.ApparentEquitorialCoordinates.α}");
Console.WriteLine($"Topocentric RA: {topocentric.α}");
Console.WriteLine($"Apparent geocentric Dec: {moon.ApparentEquitorialCoordinates.δ}");
Console.WriteLine($"Topocentric Dec: {topocentric.δ}");
