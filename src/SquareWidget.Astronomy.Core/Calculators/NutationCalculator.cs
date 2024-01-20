using SquareWidget.Astronomy.Core.UnitsOfMeasure;
using System;
using System.Collections.Generic;

namespace SquareWidget.Astronomy.Core.Calculators
{
    /// <summary>
    /// Calculate nutations in longitude (ΔΨ), obliquity (Δε), and the obliquity of the ecliptic (ε)
    /// for the moment in time. Algorithm ported from IAU SOFA ANSI C at https://iausofa.org/
    /// </summary>
    public static class NutationCalculator
    {
        public static Nutation Calculate(DateTime datetime)
        {
            double dpsi;
            double deps;
            double el, elp, f, d, om, dp, de, arg, s, c;

            Moment moment = new(datetime);
            double t = moment.T;

            const double DAS2R = 4.848136811095359935899141e-6; // arcseconds to radians
            const double D2PI = Math.PI * 2;
            const double U2R = DAS2R / 1e4; // units of 0.1 milliarcseconds to radians

            // Mean anomaly of the Sun
            el = (1287099.804 + (1292581.224 + (-0.577 - 0.012 * t) * t) * t) * DAS2R +
                Math.IEEERemainder(99.0 * t, 1.0) * D2PI;

            // Mean anomaly of the Moon
            elp = (485866.733 + (715922.633 + (31.310 + 0.064 * t) * t) * t) * DAS2R +
                Math.IEEERemainder(1325.0 * t, 1.0) * D2PI;

            // Mean longitude of Moon minus mean longitude of Moon's node.
            f = (335778.877 + (295263.137 + (-13.257 + 0.011 * t) * t) * t) * DAS2R +
                Math.IEEERemainder(1342.0 * t, 1.0) * D2PI;

            // Mean elongation of Moon from Sun.
            d = (1072261.307 + (1105601.328 + (-6.891 + 0.019 * t) * t) * t) * DAS2R +
                Math.IEEERemainder(1236.0 * t, 1.0) * D2PI;

            // Longitude of the mean ascending node of the lunar orbit on the ecliptic,
            // measured from the mean equinox of date.
            om = (450160.280 + (-482890.539 + (7.455 + 0.008 * t) * t) * t) * DAS2R +
                Math.IEEERemainder(-5.0 * t, 1.0) * D2PI;

            // initialize nutation components
            dp = 0.0;
            de = 0.0;

            // nutation series
            foreach (var n in NutationPeriodicTerms.Values)
            {
                // argument for current term
                arg = n.nl * el + n.nlp * elp + n.nf * f + n.nd * d + n.nom * om;

                // accumulate current nutation term
                s = n.sp + n.spt * t;
                c = n.ce + n.cet * t;
                if (s != 0.0) dp += s * Math.Sin(arg);
                if (c != 0.0) de += c * Math.Cos(arg);
            }

            // convert results from units of 0.1 milliarcseconds to radians
            dpsi = dp * U2R;
            deps = de * U2R;

            // convert radians to decimal degrees
            double longitude = dpsi * (180 / Math.PI);
            double obliquity = deps * (180 / Math.PI);

            // Earth's mean obliquity of the ecliptic
            double U = t / 100;
            double e0 =
                new SexigesimalAngle(23, 26, 21.448)
                - new SexigesimalAngle(0, 0, 4680.93) * U
                - 1.55 * Math.Pow(U, 2)
                + 1999.25 * Math.Pow(U, 3)
                - 51.38 * Math.Pow(U, 4)
                - 249.67 * Math.Pow(U, 5)
                - 39.05 * Math.Pow(U, 6)
                + 7.12 * Math.Pow(U, 7)
                + 27.87 * Math.Pow(U, 8)
                + 5.79 * Math.Pow(U, 9)
                + 2.45 * Math.Pow(U, 10);

            SexigesimalAngle ΔΨ = new(longitude);
            SexigesimalAngle Δε = new(obliquity);

            double e = e0 + Δε.ToDecimalDegrees();
            SexigesimalAngle ε = new(e);

            return new Nutation
            {
                ΔΨ = ΔΨ,
                Δε = Δε,
                ε = ε
            };
        }
    }

    /// <summary>
    /// Port of ANSI C from IAU SOFA. See copyright and license at bottom.
    /// </summary>
    public readonly struct NutationPeriodicTerms
    {
        public static readonly IList<Terms> Values = new List<Terms>
        {
            /* 1- 10 */
            new Terms( 0,  0,  0,  0,  1, -171996.0, -174.2,  92025.0,    8.9),
            new Terms( 0,  0,  0,  0,  2,    2062.0,    0.2,   -895.0,    0.5),
            new Terms(-2,  0,  2,  0,  1,      46.0,    0.0,    -24.0,    0.0),
            new Terms( 2,  0, -2,  0,  0,      11.0,    0.0,      0.0,    0.0),
            new Terms(-2,  0,  2,  0,  2,      -3.0,    0.0,      1.0,    0.0),
            new Terms( 1, -1,  0, -1,  0,      -3.0,    0.0,      0.0,    0.0),
            new Terms( 0, -2,  2, -2,  1,      -2.0,    0.0,      1.0,    0.0),
            new Terms( 2,  0, -2,  0,  1,       1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  0,  2, -2,  2,  -13187.0,   -1.6,   5736.0,   -3.1),
            new Terms( 0,  1,  0,  0,  0,    1426.0,   -3.4,     54.0,   -0.1),

            /* 11-20 */
            new Terms( 0,  1,  2, -2,  2,    -517.0,    1.2,    224.0,   -0.6),
            new Terms( 0, -1,  2, -2,  2,     217.0,   -0.5,    -95.0,    0.3),
            new Terms( 0,  0,  2, -2,  1,     129.0,    0.1,    -70.0,    0.0),
            new Terms( 2,  0,  0, -2,  0,      48.0,    0.0,      1.0,    0.0),
            new Terms( 0,  0,  2, -2,  0,     -22.0,    0.0,      0.0,    0.0),
            new Terms( 0,  2,  0,  0,  0,      17.0,   -0.1,      0.0,    0.0),
            new Terms( 0,  1,  0,  0,  1,     -15.0,    0.0,      9.0,    0.0),
            new Terms( 0,  2,  2, -2,  2,     -16.0,    0.1,      7.0,    0.0),
            new Terms( 0, -1,  0,  0,  1,     -12.0,    0.0,      6.0,    0.0),
            new Terms(-2,  0,  0,  2,  1,      -6.0,    0.0,      3.0,    0.0),

            /* 21-30 */
            new Terms( 0, -1,  2, -2,  1,      -5.0,    0.0,      3.0,    0.0),
            new Terms( 2,  0,  0, -2,  1,       4.0,    0.0,     -2.0,    0.0),
            new Terms( 0,  1,  2, -2,  1,       4.0,    0.0,     -2.0,    0.0),
            new Terms( 1,  0,  0, -1,  0,      -4.0,    0.0,      0.0,    0.0),
            new Terms( 2,  1,  0, -2,  0,       1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  0, -2,  2,  1,       1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  1, -2,  2,  0,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  1,  0,  0,  2,       1.0,    0.0,      0.0,    0.0),
            new Terms(-1,  0,  0,  1,  1,       1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  1,  2, -2,  0,      -1.0,    0.0,      0.0,    0.0),

            /* 31-40 */
            new Terms( 0,  0,  2,  0,  2,   -2274.0,   -0.2,    977.0,   -0.5),
            new Terms( 1,  0,  0,  0,  0,     712.0,    0.1,     -7.0,    0.0),
            new Terms( 0,  0,  2,  0,  1,    -386.0,   -0.4,    200.0,    0.0),
            new Terms( 1,  0,  2,  0,  2,    -301.0,    0.0,    129.0,   -0.1),
            new Terms( 1,  0,  0, -2,  0,    -158.0,    0.0,     -1.0,    0.0),
            new Terms(-1,  0,  2,  0,  2,     123.0,    0.0,    -53.0,    0.0),
            new Terms( 0,  0,  0,  2,  0,      63.0,    0.0,     -2.0,    0.0),
            new Terms( 1,  0,  0,  0,  1,      63.0,    0.1,    -33.0,    0.0),
            new Terms(-1,  0,  0,  0,  1,     -58.0,   -0.1,     32.0,    0.0),
            new Terms(-1,  0,  2,  2,  2,     -59.0,    0.0,     26.0,    0.0),

            /* 41-50 */
            new Terms( 1,  0,  2,  0,  1,     -51.0,    0.0,     27.0,    0.0),
            new Terms( 0,  0,  2,  2,  2,     -38.0,    0.0,     16.0,    0.0),
            new Terms( 2,  0,  0,  0,  0,      29.0,    0.0,     -1.0,    0.0),
            new Terms( 1,  0,  2, -2,  2,      29.0,    0.0,    -12.0,    0.0),
            new Terms( 2,  0,  2,  0,  2,     -31.0,    0.0,     13.0,    0.0),
            new Terms( 0,  0,  2,  0,  0,      26.0,    0.0,     -1.0,    0.0),
            new Terms(-1,  0,  2,  0,  1,      21.0,    0.0,    -10.0,    0.0),
            new Terms(-1,  0,  0,  2,  1,      16.0,    0.0,     -8.0,    0.0),
            new Terms( 1,  0,  0, -2,  1,     -13.0,    0.0,      7.0,    0.0),
            new Terms(-1,  0,  2,  2,  1,     -10.0,    0.0,      5.0,    0.0),

            /* 51-60 */
            new Terms( 1,  1,  0, -2,  0,      -7.0,    0.0,      0.0,    0.0),
            new Terms( 0,  1,  2,  0,  2,       7.0,    0.0,     -3.0,    0.0),
            new Terms( 0, -1,  2,  0,  2,      -7.0,    0.0,      3.0,    0.0),
            new Terms( 1,  0,  2,  2,  2,      -8.0,    0.0,      3.0,    0.0),
            new Terms( 1,  0,  0,  2,  0,       6.0,    0.0,      0.0,    0.0),
            new Terms( 2,  0,  2, -2,  2,       6.0,    0.0,     -3.0,    0.0),
            new Terms( 0,  0,  0,  2,  1,      -6.0,    0.0,      3.0,    0.0),
            new Terms( 0,  0,  2,  2,  1,      -7.0,    0.0,      3.0,    0.0),
            new Terms( 1,  0,  2, -2,  1,       6.0,    0.0,     -3.0,    0.0),
            new Terms( 0,  0,  0, -2,  1,      -5.0,    0.0,      3.0,    0.0),
        
            /* 61-70 */
            new Terms( 1, -1,  0,  0,  0,       5.0,    0.0,      0.0,    0.0),
            new Terms( 2,  0,  2,  0,  1,      -5.0,    0.0,      3.0,    0.0),
            new Terms( 0,  1,  0, -2,  0,      -4.0,    0.0,      0.0,    0.0),
            new Terms( 1,  0, -2,  0,  0,       4.0,    0.0,      0.0,    0.0),
            new Terms( 0,  0,  0,  1,  0,      -4.0,    0.0,      0.0,    0.0),
            new Terms( 1,  1,  0,  0,  0,      -3.0,    0.0,      0.0,    0.0),
            new Terms( 1,  0,  2,  0,  0,       3.0,    0.0,      0.0,    0.0),
            new Terms( 1, -1,  2,  0,  2,      -3.0,    0.0,      1.0,    0.0),
            new Terms(-1, -1,  2,  2,  2,      -3.0,    0.0,      1.0,    0.0),
            new Terms(-2,  0,  0,  0,  1,      -2.0,    0.0,      1.0,    0.0),

            /* 71-80 */
            new Terms( 3,  0,  2,  0,  2,      -3.0,    0.0,      1.0,    0.0),
            new Terms( 0, -1,  2,  2,  2,      -3.0,    0.0,      1.0,    0.0),
            new Terms( 1,  1,  2,  0,  2,       2.0,    0.0,     -1.0,    0.0),
            new Terms(-1,  0,  2, -2,  1,      -2.0,    0.0,      1.0,    0.0),
            new Terms( 2,  0,  0,  0,  1,       2.0,    0.0,     -1.0,    0.0),
            new Terms( 1,  0,  0,  0,  2,      -2.0,    0.0,      1.0,    0.0),
            new Terms( 3,  0,  0,  0,  0,       2.0,    0.0,      0.0,    0.0),
            new Terms( 0,  0,  2,  1,  2,       2.0,    0.0,     -1.0,    0.0),
            new Terms(-1,  0,  0,  0,  2,       1.0,    0.0,     -1.0,    0.0),
            new Terms( 1,  0,  0, -4,  0,      -1.0,    0.0,      0.0,    0.0),


            /* 81-90 */
            new Terms(-2,  0,  2,  2,  2,       1.0,    0.0,     -1.0,    0.0),
            new Terms(-1,  0,  2,  4,  2,      -2.0,    0.0,      1.0,    0.0),
            new Terms( 2,  0,  0, -4,  0,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 1,  1,  2, -2,  2,       1.0,    0.0,     -1.0,    0.0),
            new Terms( 1,  0,  2,  2,  1,      -1.0,    0.0,      1.0,    0.0),
            new Terms(-2,  0,  2,  4,  2,      -1.0,    0.0,      1.0,    0.0),
            new Terms(-1,  0,  4,  0,  2,       1.0,    0.0,      0.0,    0.0),
            new Terms( 1, -1,  0, -2,  0,       1.0,    0.0,      0.0,    0.0),
            new Terms( 2,  0,  2, -2,  1,       1.0,    0.0,     -1.0,    0.0),
            new Terms( 2,  0,  2,  2,  2,      -1.0,    0.0,      0.0,    0.0),

            /* 91-100 */
            new Terms( 1,  0,  0,  2,  1,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  0,  4, -2,  2,       1.0,    0.0,      0.0,    0.0),
            new Terms( 3,  0,  2, -2,  2,       1.0,    0.0,      0.0,    0.0),
            new Terms( 1,  0,  2, -2,  0,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  1,  2,  0,  1,       1.0,    0.0,      0.0,    0.0),
            new Terms(-1, -1,  0,  2,  1,       1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  0, -2,  0,  1,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  0,  2, -1,  2,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  1,  0,  2,  0,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 1,  0, -2, -2,  0,      -1.0,    0.0,      0.0,    0.0),

            /* 101-106 */
            new Terms( 0, -1,  2,  0,  1,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 1,  1,  0, -2,  1,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 1,  0, -2,  2,  0,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 2,  0,  0,  2,  0,       1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  0,  2,  4,  2,      -1.0,    0.0,      0.0,    0.0),
            new Terms( 0,  1,  0,  1,  0,       1.0,    0.0,      0.0,    0.0)
        };

        public readonly struct Terms
        {
            public Terms(int nl, int nlp, int nf, int nd, int nom, double sp, double spt, double ce, double cet)
            {
                this.nl = nl;
                this.nlp = nlp;
                this.nf = nf;
                this.nd = nd;
                this.nom = nom;
                this.sp = sp;
                this.spt = spt;
                this.ce = ce;
                this.cet = cet;
            }

            public readonly int nl;
            public readonly int nlp;
            public readonly int nf;
            public readonly int nd;
            public readonly int nom;
            public readonly double sp;
            public readonly double spt;
            public readonly double ce;
            public readonly double cet;
        }
    }
}

/*----------------------------------------------------------------------
**
**  Copyright (C) 2023
**  Standards of Fundamental Astronomy Board
**  of the International Astronomical Union.
**
**  =====================
**  SOFA Software License
**  =====================
**
**  NOTICE TO USER:
**
**  BY USING THIS SOFTWARE YOU ACCEPT THE FOLLOWING SIX TERMS AND
**  CONDITIONS WHICH APPLY TO ITS USE.
**
**  1. The Software is owned by the IAU SOFA Board ("SOFA").
**
**  2. Permission is granted to anyone to use the SOFA software for any
**     purpose, including commercial applications, free of charge and
**     without payment of royalties, subject to the conditions and
**     restrictions listed below.
**
**  3. You (the user) may copy and distribute SOFA source code to others,
**     and use and adapt its code and algorithms in your own software,
**     on a world-wide, royalty-free basis.  That portion of your
**     distribution that does not consist of intact and unchanged copies
**     of SOFA source code files is a "derived work" that must comply
**     with the following requirements:
**
**     a) Your work shall be marked or carry a statement that it
**        (i) uses routines and computations derived by you from
**        software provided by SOFA under license to you; and
**        (ii) does not itself constitute software provided by and/or
**        endorsed by SOFA.
**
**     b) The source code of your derived work must contain descriptions
**        of how the derived work is based upon, contains and/or differs
**        from the original SOFA software.
**
**     c) The names of all routines in your derived work shall not
**        include the prefix "iau" or "sofa" or trivial modifications
**        thereof such as changes of case.
**
**     d) The origin of the SOFA components of your derived work must
**        not be misrepresented;  you must not claim that you wrote the
**        original software, nor file a patent application for SOFA
**        software or algorithms embedded in the SOFA software.
**
**     e) These requirements must be reproduced intact in any source
**        distribution and shall apply to anyone to whom you have
**        granted a further right to modify the source code of your
**        derived work.
**
**     Note that, as originally distributed, the SOFA software is
**     intended to be a definitive implementation of the IAU standards,
**     and consequently third-party modifications are discouraged.  All
**     variations, no matter how minor, must be explicitly marked as
**     such, as explained above.
**
**  4. You shall not cause the SOFA software to be brought into
**     disrepute, either by misuse, or use for inappropriate tasks, or
**     by inappropriate modification.
**
**  5. The SOFA software is provided "as is" and SOFA makes no warranty
**     as to its use or performance.   SOFA does not and cannot warrant
**     the performance or results which the user may obtain by using the
**     SOFA software.  SOFA makes no warranties, express or implied, as
**     to non-infringement of third party rights, merchantability, or
**     fitness for any particular purpose.  In no event will SOFA be
**     liable to the user for any consequential, incidental, or special
**     damages, including any lost profits or lost savings, even if a
**     SOFA representative has been advised of such damages, or for any
**     claim by any third party.
**
**  6. The provision of any version of the SOFA software under the terms
**     and conditions specified herein does not imply that future
**     versions will also be made available under the same terms and
**     conditions.
*
**  In any published work or commercial product which uses the SOFA
**  software directly, acknowledgement (see www.iausofa.org) is
**  appreciated.
**
**  Correspondence concerning SOFA software should be addressed as
**  follows:
**
**      By email:  sofa@ukho.gov.uk
**      By post:   IAU SOFA Center
**                 HM Nautical Almanac Office
**                 UK Hydrographic Office
**                 Admiralty Way, Taunton
**                 Somerset, TA1 2DN
**                 United Kingdom
**
**--------------------------------------------------------------------*/
