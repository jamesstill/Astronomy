/*
    1-PlanetaryDistance

    Code sample to show how to obtain the true and apparent planetary distance between 
    the Earth and another planet using the spherical coordinates of each planet. 

    First Example:

    Per J. L. Lawrence,Celestial Calcuations, p. 245, the formula to calculate
    the true distance between the Earth and another planet is:

        Distance in AU = Sqrt(Re^2 + Rp^2 - 2ReRp * Cos(Lp - Le))

    where Re (Earth) and Rp (Planet) are radius vector lengths in astronomical units (AU),
    and Le (Earth) and Lp (Planet) are the heliocentric ecliptic longitudes of the two bodies.

    Second Example:

    Using the code library's GeocentricPositionCalculator obtain the true and apparent
    distance between the Earth and another planet.

*/

using SquareWidget.Astronomy.Core;
using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.Planets;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

// Per J. L. Lawrence,Celestial Calcuations, p. 245, calculating the true planetary distance 
// between the Earth and another planet is:
//
// Distance d = Sqrt(Re^2 + Rp^2 - 2ReRp * Cos(Lp - Le))
//
// where Re (Earth) and Rp (Planet) are radius vector lengths in astronomical units,
// and Le (Earth) and Lp (Planet) are the heliocentric ecliptic longitudes of the two bodies.

Moment moment = new(1992, 12, 20);

// -------------------------------------------------------------------------------------------------------
// Using Lawrence's formula calculate the true distance between the Earth and Venus on the date
// -------------------------------------------------------------------------------------------------------

Planet earth = PlanetFactory.Create(PlanetName.Earth, moment);
Planet planet = PlanetFactory.Create(PlanetName.Venus, moment);

AstronomicalUnits Re = earth.SphericalCoordinates.R;
AstronomicalUnits Rp = planet.SphericalCoordinates.R;
Radians Le = earth.SphericalCoordinates.L;
Radians Lp = planet.SphericalCoordinates.L;

double d = Math.Sqrt(Re * Re + Rp * Rp - 2 * Re * Rp * Math.Cos(Lp - Le));
AstronomicalUnits distance = new(d);

Console.WriteLine($"On {moment.ToDateTime().ToString("d")} the true distance between the Earth and the planet is {distance} AU or {distance.ToKilometers()} km.");

// -------------------------------------------------------------------------------------------------------
// Using the GeocentricPositionCalculator in the code library calculate the true and apparent distances
// between the Earth and the planet on the date.
// -------------------------------------------------------------------------------------------------------
Console.WriteLine("");
Console.WriteLine("Obtain using the GeocentricPositionCalculator");
Console.WriteLine("");

GeocentricPosition gp = GeocentricPositionCalculator.Calculate(moment.ToDateTime(), PlanetName.Venus);

Console.WriteLine($"On {moment.ToDateTime().ToString("d")} the true distance between the Earth and the planet is {gp.Δt} AU and the apparent distance is {gp.Δa} AU.");

