using SquareWidget.Astronomy.Core.Models;
using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;

namespace SquareWidget.Astronomy.Core.Calculators
{
    /// <summary>
    /// Calcuates the geocentric (apparent) position angle P of Saturn's rings
    /// following Meeus algorithm.
    /// </summary>
    public static class SaturnRingPositionAngleCalculator
    {
        /// <summary>
        /// Calculate position angle of Saturn's rings given a current geocentric position
        /// </summary>
        /// <returns></returns>
        public static Degrees Calculate(GeocentricPosition position, DateTime datetime)
        {
            Moment moment = new(datetime);
            double T = moment.T;
            
            Nutation n = NutationCalculator.Calculate(datetime);
            Degrees ΔΨ = n.ΔΨ.ToDegrees();

            Degrees λ = position.EclipticalCoordinates.λ.ToDegrees().ToReducedAngle();
            Degrees β = position.EclipticalCoordinates.β.ToDegrees();
            Degrees ε = position.EclipticalCoordinates.ε.ToDegrees();
       
            Degrees ι = new(28.075216 - 0.012998 * T + 0.000004 * T * T);
            Degrees Ω = new(169.508470 + 1.394681 * T + 0.000412 * T * T);

            Degrees λ0 = new(Ω - 90.0);
            Degrees β0 = new(90.0 - ι);

            Radians l0 = λ0.ToRadians();
            Radians l = λ.ToRadians();
            Radians b = β.ToRadians();

            λ += new Degrees(0.005693 * Math.Cos(l0 - l) / Math.Cos(b));
            β += new Degrees(0.005693 * Math.Sin(l0 - l) * Math.Sin(b));

            λ += ΔΨ;
            λ0 += ΔΨ;

            EclipticalCoordinates ec = new(λ, β, ε);
            EclipticalCoordinates ec0 = new(λ0, β0, ε);

            EquatorialCoordinates eqc = ec.ToΕquatorialCoordinates();
            EquatorialCoordinates eqc0 = ec0.ToΕquatorialCoordinates();

            Radians α = eqc.α.ToDegrees().ToRadians();
            Radians δ = eqc.δ.ToRadians();

            Radians α0 = eqc0.α.ToDegrees().ToRadians();
            Radians δ0 = eqc0.δ.ToRadians();

            // calculate position angle
            double value = Math.Atan2(Math.Cos(δ0) * Math.Sin(α0 - α), Math.Sin(δ0) * Math.Cos(δ) - Math.Cos(δ0) * Math.Sin(δ) * Math.Cos(α0 - α));
            Radians P = new(value);

            return new Degrees(P.ToDegrees());
        }
    }
}
