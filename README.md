
# Repository

This repository is home to the `SquareWidget.Astronomy.Core` code library project. This project is licensed under the [GNU GPLv3](LICENSE.txt) and maintained by James Still who hosts the [SquareWidget](https://squarewidget.com) blog. 

## SquareWidget.Astronomy.Core

SquareWidget.Astronomy.Core is a code library for .NET that supports common astronomical calculations and algorithms. 

# Basic Usage
## Moment

A `Moment` is any specific point in time down to the optional millisecond. Instantiate a `Moment` with a `DateTime`.
```
DateTime d = new(2024, 1, 31);
Moment m = new(d);
Console.WriteLine(m.ToString()); // 2024-01-31 00:00:00Z
``` 
You can pass in a decimal value for the day and its time.
```
Moment m = new(1957, 10, 4.812);
Console.WriteLine(m.ToString()); // 1957-10-04 19:29:17Z
```
Or the Julian Day Ephemeris (JD/JDE).
```
Moment m = new(2436116.3120023147);
Console.WriteLine(m.ToString()); // 1957-10-04 19:29:16Z
```
A `Moment` will also return a JDE, a modified JDE, Day D, and the Time T from the J2000.0 epoch.
```
Moment m = new(1957, 10, 4, 7, 29, 17);
double jde = m.JDE;   //  2436115.8120023147
double mdj = m.MDJ;   //  36115.31200231472
double dayd = m.DayD; // -15429.18799768528
double t = m.T;       // -0.42242814504271814
```

## Units of Measure
It is necessary to have units of measure for celestial calculations and to be able to convert easily between them. We looked at the `Moment` struct above. Others include:

 - Degrees
 - Radians
 - Astronomical Units
 - Delta T
 - Light Time
 - Nutation
 - Right Ascension
 - Sexagesimal Angle
 - Ecliptical Coordinates
 - Equitorial Coordinates
 - Geocentric Position

A `Degree` struct can be instantiated in several ways.
```
Degrees d = new(23.4328); // decimal degrees
Degrees d = new(r);       // Radian struct
Degrees d = new(a);       // Sexagesimal Angle struct (DMS)
```
In most cases `ToString()` is overridden for display purposes. Units of measure can also be converted to other units.
```
Degrees d = new(23.4328);
Console.WriteLine(d.ToString()); // 23°.4328

Radians r = d.ToRadians(); // calling method to get radians
Radians r = new(d);        // or pass in the object itself

Radians r = new(0.40897951296);
Degrees d = new(r);
Console.WriteLine(d.ToString()); // 23°.43279
```
Very large angles can be reduced to the [0, 360] range. Functions can be chained to produce the desired conversion and output.
```
Degrees d = new(25487.1873);
Console.WriteLine(d.ToReducedAngle().ToString()); // 287°.187300

Radians r = new(d);
Console.WriteLine("r0: " +  r.ToString());                    // 444.8353354
Console.WriteLine(r.ToReducedAngle().ToString());             //   5.0123639
Console.WriteLine(r.ToReducedAngle().ToDegrees().ToString()); // 287°.187300
```

A `RightAscension` handles hours, minutes and seconds (HMS).
```
// passing in HMS
RightAscension ra = new(12, 37, 27);
Console.WriteLine(ra.ToString());   // 12h 37m 27s

// passing decimal degrees
RightAscension ra = new(189.3625);
Console.WriteLine(ra.ToString()); // 12h 37m 27s
```
A `SexagesimalAngle` represents degrees, arcminutes, and arcseconds (DMS).
```
SexigesimalAngle a = new(34, 10, 49);
Console.WriteLine(a.ToString());      // +34° 10' 49"
```
If the angle is negative pass in it with any value > 0. Do not pass in `-0` as this will fail.
```
SexigesimalAngle a = new(-0, 13, 49);   // NO
SexigesimalAngle a = new(-0, -13, 49);  // NO

// GOOD
SexigesimalAngle a = new(0, -13, 49);   // YES!
SexigesimalAngle a = new(0, 0, -49);    // YES!

// Example Display Output
SexigesimalAngle a = new(0, -13, 49);
Console.WriteLine(a.ToString());        // -13' 49"
```
Two coordinate systems have been implemented: *equitorial* and *ecliptical*. Suppose we want to model Pollux (β Gem) using an `EquitorialCoordinate`:
```
// Pollux (β Gem)
RightAscension α = new(7, 45, 18.946);
SexigesimalAngle δ = new(28, 01, 34.26);
Degrees ε = new(23.4392911);

EquitorialCoordinates eqc = new(δ, α, ε);

Console.WriteLine(eqc.α.ToString()); // 7h 45m 18.946s
Console.WriteLine(eqc.δ.ToString()); // +28° 1' 34.26"
```
You can convert between the two coordinate systems:
```
EquitorialCoordinates eqc = new(δ, α, ε);
EclipticalCoordinates ec = eqc.ToΕclipticCoordinates();

Console.WriteLine(ec.λ.ToDegrees().ToReducedAngle().ToString()); // 113°.21
Console.WriteLine(ec.β.ToDegrees().ToString());                  //   6°.68
```

## Planets

You can instantiate a planet with the `new` operator or use the factory method. You must pass in a `Moment` struct either way.
```
DateTime datetime = new(2028, 7, 1);
Moment moment = new(datetime);

// using new operator
Earth earth = new(moment);

// using factory method for polymorphism
Planet earth = PlanetFactory.Create(PlanetName.Earth, moment);
```
Once you have a planet you can get that planet's orbital (Keplerian) elements. Suppose I want to know the mean, eccentric and true anomalies of Mars on 1 Jul 2028:
```
DateTime datetime = new(2028, 7, 1, 0, 0, 0);
Moment moment = new(datetime);

Planet planet = PlanetFactory.Create(PlanetName.Mars, moment);
OrbitalElements oe = planet.OrbitalElements;

string planetName = planet.GetType().Name;
string d = datetime.ToShortDateString();
string M = oe.M.ToReducedAngle().ToString();
string E = oe.E.ToDegrees().ToReducedAngle().ToString();
string v = oe.v.ToDegrees().ToReducedAngle().ToString();

Console.WriteLine($"{planetName} on {d}");
Console.WriteLine($"Mean anomaly (MA) of {M}");
Console.WriteLine($"Eccentric anomaly (EC) of {E}");
Console.WriteLine($"True anomaly (TA) of {v}");

/* Displays:
 
Mars on 7/1/2028
Mean anomaly (MA) of 73°.64204662016618
Eccentric anomaly (EC) of 78°.89475302840492
True anomaly (TA) of 84°.20292912028344
*/
```
A planet has its heliocentric spherical coordinates LBR in a `SphericalCoordinates` object:
```
DateTime datetime = new(2028, 7, 1, 0, 0, 0);
Moment moment = new(datetime);

Planet planet = PlanetFactory.Create(PlanetName.Mars, moment);
SphericalCoordinates sc = planet.SphericalCoordinates;

Console.WriteLine($"{planetName} on {d}");
Console.WriteLine("L: " + sc.L.ToDegrees().ToReducedAngle().ToString());
Console.WriteLine("B: " + sc.B.ToDegrees().ToReducedAngle().ToString()); 
Console.WriteLine("R: " + sc.R.ToString());

/* Displays:
 
Mars on 7/1/2028
L: 60°.810047542251596
B: 0°.354134695931737
R: 1.4962191891309637
*/
```

## Calculators
Calculators are standalone static classes used to compute an output. All calculators have a method `Calculate` that expect one or more arguments and when called return the calculated results. Currently, there are four calculators.

### Nutation Calculator
The Nutation Calculator returns the Earth's nutation components for a given `DateTime`. The algorithm uses periodic terms taken from the [IAU SOFA](https://iausofa.org) system.
```
DateTime datetime = new(1987, 4, 10);
Nutation n = NutationCalculator.Calculate(datetime);

string d = datetime.ToShortDateString(); // culture en-US
string ΔΨ = n.ΔΨ.ToString();
string Δε = n.Δε.ToString();
string ε = n.ε.ToString();

Console.WriteLine($"Nutation on {d}");
Console.WriteLine($"Nutation in longitude: {ΔΨ}");
Console.WriteLine($"Nutation in latitude: {Δε}");
Console.WriteLine($"True obliquity of the ecliptic: {ε}");

/* Displays:
 
Nutation on 4/10/1987
Nutation in longitude: -3.84084252"
Nutation in latitude: +9.4936372"
True obliquity of the ecliptic: +23° 26' 36.87640248"
*/
```
### Geocentric Position Calculator
The `GeocentricPositionCalculator` expects a `DateTime` and a planet name (target body). It returns a `GeocentricPosition` object with ecliptical and equitorial coordinates, true distance, and apparent distance (in AU).
```
DateTime datetime = new(1992, 12, 20);
string planetName = PlanetName.Venus;
GeocentricPosition gp = GeocentricPositionCalculator.Calculate(datetime, planetName);

EclipticalCoordinates ec = gp.EclipticalCoordinates;
Degrees longitude = ec.λ.ToDegrees().ToReducedAngle();
Degrees latitude = ec.β.ToDegrees();
EquitorialCoordinates eqc = gp.EquitorialCoordinates;
AstronomicalUnits Δt = gp.Δt;
AstronomicalUnits Δa = gp.Δa;

string d = datetime.ToShortDateString(); // culture en-US

Console.WriteLine($"Geocentric position of {planetName} on {d}");        
Console.WriteLine($"Ecliptical coordinates longitude {longitude} and latitude {latitude}");
Console.WriteLine($"True distance to Earth is {Δt} (AU) and apparent distance is {Δa} (AU)");
Console.WriteLine($"Equitorial coordinates RA {eqc.α} and declination {eqc.δ}");

/* Displays:
 
Geocentric position of Venus on 12/20/1992
Ecliptical coordinates longitude 313°.0855 and latitude -2°.0847
True distance to Earth is 0.91085 (AU) and apparent distance is 0.91095 (AU)
Equitorial coordinates RA 21h 4m 42.46s and declination -18° 53' 12.07"
*/
```
### Solar Longitude Calculator
The `SolarLongitudeCalculator` calculates the center-to-center geocentric longitude of the Sun on a given `DateTime`. 
```
DateTime datetime = new(1992, 10, 13);
Degrees p = SolarLongitudeCalculator.Calculate(datetime);

string d = datetime.ToShortDateString(); // culture en-US

Console.WriteLine($"Sun's geometric longitude on {d} is {p}");

/* Displays:
 
Sun's geometric longitude on 10/13/1992 is 199°.909873
*/
```

### Saturn's Ring Position Angle Calculator
Use this calculator to find the geocentric position angle of Saturn's rings, as measured from the northern celestial pole towards the East, on a given `DateTime`. The calculator expects a `GeocentricPosition` of Saturn as an argument along with a `DateTime`.
```
DateTime datetime = new(1992, 12, 16);
string planetName = PlanetName.Saturn;

GeocentricPosition gp = GeocentricPositionCalculator.Calculate(datetime, planetName);
Degrees p = SaturnRingPositionAngleCalculator.Calculate(gp, datetime);

string d = datetime.ToShortDateString(); // culture en-US

Console.WriteLine($"The position angle P of Saturn's rings on {d} is {p}");

/* Displays:
 
The position angle P of Saturn's rings on 12/16/1992 is 6°.7404
*/
```
### Sources
Algorithms are implemented from many sources including formulas published on Wikipedia, NASA (DeltaT), the U.S. Naval Observatory, and from Jean Meeus *Astronomical Algorithms*, 2nd Edition. Additionally, I benefited from consulting W. M. Smart (*Textbook on Spherical Astronomy*, 6th Edition) and J. L. Lawrence (*Celestial Calculations*). Heliocentric spherical coordinates for all planets are from the [VSOP87 Theory Series D](https://phpsciencelabs.com/vsop87-source-code-generator-tool/) data. Last, the library uses nutations in longitude, obliquity, and the obliquity of the ecliptic from the [IAU SOFA](https://iausofa.org/) ANSI C code base.

###  Contributing
The code library will be stronger if there are many hands contributing to the code base. I welcome community pull requests for bug fixes, enhancements, and documentation. 