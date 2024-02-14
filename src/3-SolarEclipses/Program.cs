/*
    3-SolarEclipses

    Code sample to show how to calculate solar eclipses. Calculations are from Jean
    Meeus, Astronomical Algorithms, 2nd Ed., Chapter 54. Compare results to NASA's
    "Five Millennium Canon of Solar Eclipses -1999 to +3000 published online at:
    https://eclipse.gsfc.nasa.gov/5MCSE/5MCSE-Maps-10.pdf

*/

using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;

DateOnly startDate = new(1785, 1, 1);
DateOnly endDate = new(1785, 12, 31);
DateRange dateRange = new(startDate, endDate);

IEnumerable<SolarEclipse> eclipses = SolarEclipseCalculator.Calculate(dateRange);

foreach (var eclipse in eclipses)
{
    Console.WriteLine(eclipse.ToString());
}
