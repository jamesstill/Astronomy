/*
    6-MoonPhases

    Code sample to show how to work with the Moon's phases and to calculate the 
    illuminated fraction of the Moon's disk. Formulas are taken from the book 
    Celestial Calculations, J.L. Lawrence, p 89 (4.7.1 and 4.7.2) as well as Jean Meeus, 
    Astronomical Algorithms, (13.5) and (13.6), and conversion help from a Go implementation 
    by Sonia Keys.

*/

using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.CelestialObjects.Moons;
using SquareWidget.Astronomy.Core.Models;

DateOnly startDate = new(2001, 1, 1);
DateOnly endDate = new(2001, 3, 31);
DateRange dateRange = new(startDate, endDate);

List<MoonPhase> list = MoonPhaseDatesCalculator.Calculate(dateRange);

Console.WriteLine("{0,-10} {1,-14} {2}", "Date", "Moon Phase", "Peak Time (UTC)");

foreach (var item in list)
{
    DateTime peakTime = item.Moment.ToDateTime();
    Console.WriteLine("{0,-10} {1,-14} {2:HH:mm}",
        peakTime.ToString("MM/dd/yyyy"), item.PhaseName, peakTime);
}

Console.ReadLine();
