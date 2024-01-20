﻿using System;
using System.Collections.Generic;
using SquareWidget.Astronomy.Core.Planets.MeanOrbitalElements;
using SquareWidget.Astronomy.Core.Planets.SphericalLBRCoordinates;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;

namespace SquareWidget.Astronomy.Core.Planets
{
    public class Venus : Planet
    {
        public Venus(Moment moment) : base(moment) 
        {
            OrbitalElements = OrbitalElementsBuilder.Create(this);
            SphericalCoordinates = SphericalCoordinatesBuilder.Create(this);
        }

    }
}
