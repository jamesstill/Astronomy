using System;

namespace SquareWidget.Astronomy.Core.UnitsOfMeasure
{
    /// <summary>
    /// Struct to hold ecliptic longitude and latitude
    /// </summary>
    public readonly struct EquitorialCoordinates
    {
        public readonly SexigesimalAngle δ;
        public readonly RightAscension α;
        public readonly Radians ε;

        public EquitorialCoordinates(SexigesimalAngle δ, RightAscension α, Degrees ε)
        {
            this.δ = δ;
            this.α = α;
            this.ε = ε.ToRadians();
        }

        public EquitorialCoordinates(SexigesimalAngle δ, RightAscension α, Radians ε)
        {
            this.δ = δ;
            this.α = α;
            this.ε = ε;
        }

        public EquitorialCoordinates(EclipticalCoordinates ec)
        {
            Radians λ = ec.λ;
            Radians β = ec.β;
            Radians ε = ec.ε;

            this.ε = ε;

            Radians l = new(Math.Atan2(Math.Sin(λ) * Math.Cos(ε) - Math.Tan(β) * Math.Sin(ε), Math.Cos(λ)));
            Radians b = new(Math.Asin(Math.Sin(β) * Math.Cos(ε) + Math.Cos(β) * Math.Sin(ε) * Math.Sin(λ)));
            
            this.α = new RightAscension(l.ToDegrees().ToReducedAngle());
            this.δ = new SexigesimalAngle(b.ToDegrees());
        }

        public static implicit operator double(EquitorialCoordinates ec) => ec;

        public EclipticalCoordinates ToΕclipticCoordinates() 
        {
            Radians d = δ.ToRadians();
            Radians a = α.ToDegrees().ToRadians();

            Radians λ = new(Math.Atan2(Math.Sin(a) * Math.Cos(ε) + Math.Tan(d) * Math.Sin(ε), Math.Cos(a)));
            Radians β = new(Math.Asin(Math.Sin(d) * Math.Cos(ε) - Math.Cos(d) * Math.Sin(ε) * Math.Sin(a)));

            return new EclipticalCoordinates(λ, β, ε);
        }
    }
}
