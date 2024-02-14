/*
    4-JupiterSatellitePositions

    Code sample to show how to obtain the apparent rectangular coordinates of 
    Jupiter's four Galilean moons on a given date (or date range). Calculations 
    are from Jean Meeus, Astronomical Algorithms, 2nd Ed., Chapters 43 and 44.
    See: https://squarewidget.com/astronomical-calculations-moons-of-jupiter/

*/

using SquareWidget.Astronomy.Core.Calculators;
using SquareWidget.Astronomy.Core.Models;

DateTime startDate = new(2024, 1, 1);
DateTime endDate = new(2024, 12, 31);

for (var d = startDate; d <= endDate; d = d.AddDays(1))
{
    SatellitePositions positions = JupiterSatellitePositionCalculator.Calculate(d);
    Console.WriteLine(positions.ToString());
}