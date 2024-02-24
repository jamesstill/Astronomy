using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct to hold ecliptic longitude and latitude. Latitude λ should be corrected  
    /// to required degree range [0, 360] and longitude β should be +/- 90.
    /// </summary>
    public readonly struct EclipticalCoordinates
    {
        public readonly Radians λ;
        public readonly Radians β;
        public readonly Radians ε;

        public EclipticalCoordinates(Radians λ, Radians β, Radians ε)
        {
            this.λ = λ;
            this.λ = λ;
            this.β = β;
            this.ε = ε;
        }

        public EclipticalCoordinates(Degrees λ, Degrees β, Degrees ε)
        {
            this.λ = λ.ToRadians();
            this.β = β.ToRadians();
            this.ε = ε.ToRadians();
        }

        public EclipticalCoordinates(EquatorialCoordinates ec)
        {
            Radians ε = ec.ε;
            Radians δ = ec.δ.ToRadians();
            Radians α = ec.α.ToDegrees().ToRadians();

            this.ε = ε;
            this.λ = new Radians(Math.Atan2(Math.Sin(α) * Math.Cos(ε) + Math.Tan(δ) * Math.Sin(ε), Math.Cos(α)));
            this.β = new Radians(Math.Asin(Math.Sin(δ) * Math.Cos(ε) - Math.Cos(δ) * Math.Sin(ε) * Math.Sin(α)));
        }

        public static implicit operator double(EclipticalCoordinates ec) => ec;

        public EquatorialCoordinates ToΕquatorialCoordinates() 
        {
            Radians l = new(Math.Atan2(Math.Sin(λ) * Math.Cos(ε) - Math.Tan(β) * Math.Sin(ε), Math.Cos(λ)));
            Radians b = new(Math.Asin(Math.Sin(β) * Math.Cos(ε) + Math.Cos(β) * Math.Sin(ε) * Math.Sin(λ)));

            RightAscension α = new(l.ToDegrees().ToReducedAngle());
            SexigesimalAngle δ = new(b.ToDegrees());

            return new EquatorialCoordinates(δ, α, ε.ToDegrees());

        }
    }
}
