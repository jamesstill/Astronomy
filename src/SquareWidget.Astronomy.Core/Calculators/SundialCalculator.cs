using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SquareWidget.Astronomy.Core.Calculators
{
    /// <summary>
    /// Calcuates the hour angles in a given range for a latitude (φ).
    /// See: https://squarewidget.com/lets-make-a-sundial
    /// </summary>
    public static class SundialCalculator
    {
        public static List<SundialAngle> Calculate(Degrees φ)
        {
            List<SundialAngle> list = new();
            
            double L = Math.Sin(φ * 2 * Math.PI / 360);

            list.Add(new SundialAngle(12, new Degrees(0))); // solar noon

            int n = 1;
            for (int t = 13; t <= 18; t++)
            {
                double h = (t - 12) * 15.0;
                double H = Math.Atan(L * Math.Tan(h * 2 * Math.PI / 360)) * 360 / (2 * Math.PI);
                int morningHour = t - 2 * n;

                SundialAngle afternoon = new(t, new Degrees(H));
                SundialAngle morning = new(morningHour, new Degrees(-H));

                list.Add(afternoon);
                list.Add(morning);

                n++;
            }

            return list.OrderBy(o => o.Hour).ToList();
        }
    }
}
