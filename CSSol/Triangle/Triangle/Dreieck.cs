using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Triangle
{
    public class RWDreieck
    {
        private Dreieck dreieck;
        public readonly double height;
        public readonly double theta;
        public Dreieck Dreieck()
        {
            return dreieck.clone();
        }

        private RWDreieck (Dreieck dreieck)
        {
            this.dreieck = dreieck;
            this.height = this.dreieck.Höhe()[2];
            this.theta = Math.Acos(this.dreieck.seiten[0] / this.dreieck.seiten[2]);
            Debug.Assert(invariant());
        }

        private static Dreieck[] Trigo(Nullable<double> a, Nullable<double> b, Nullable<double> c, Nullable<double> h, Nullable<double> theta)
        {
            int valuecount = 0;
            int index = -1;

            double[] sides = new double[3];

            if (a is double aval)
            {
                index = 0;
                valuecount++;
                sides[0] = aval;
                if (b is double bval)
                {
                    valuecount++;
                    sides[1] = bval;
                    return new Dreieck[] { Dreieck.Rechtwinklig(new double?[] { aval, bval, null }) };
                }
                else
                {
                    if (c is double cval)
                    {
                        sides[2] = cval;
                        valuecount++;
                        return new Dreieck[] { Dreieck.Rechtwinklig(new double?[] { aval, null, cval }) };
                    }
                }
            }
            else
            {
                if (b is double bval)
                {
                    index = 1;
                    valuecount++;
                    sides[1] = bval;
                    if (c is double cval)
                    {
                        valuecount++;
                        sides[2] = cval;
                        return new Dreieck[] { Dreieck.Rechtwinklig(new double?[] { null, bval, cval }) };
                    }
                }
                else
                {
                    if (c is double cval)
                    {
                        valuecount++;
                        sides[2] = cval;
                        index = 2;
                    }
                }
            }

            Debug.Assert(valuecount < 2);
            Debug.Assert(valuecount == 0 || index >= 0);

            // less trivial cases

            if (h is double hval)
            {
                if (valuecount == 1)
                {
                    if (index == 2)
                    {

                        double var = Math.Pow(sides[2], 2) - 4 * Math.Pow(hval, 4);

                        if (var > 0)
                        {
                            Dreieck[] sol = new Dreieck[2];
                            double q = (sides[2] + var) / 2;
                            double p = (sides[2] - var) / 2;

                            sol[0] = Dreieck.Rechtwinklig(new double?[] { q, hval, null }).strecken(2, sides[2]);
                            sol[1] = Dreieck.Rechtwinklig(new double?[] { hval, p, null }).strecken(2, sides[2]);
                            return sol;
                        }
                        else if (var == 0)
                        {
                            Dreieck[] sol = new Dreieck[1];
                            double q = sides[2] / 2;
                            sol[0] = Dreieck.Rechtwinklig(new double?[] { q, hval, null }).strecken(2, sides[2]);
                            return sol;
                        }
                        else
                        {
                            return new Dreieck[0];
                        }
                    }
                    else if (index == 1)
                    {
                        return new Dreieck[] { Dreieck.Rechtwinklig(new double?[] { hval, null, sides[1] }).strecken(1, sides[1]) };
                    }
                    else
                    {
                        Debug.Assert(index == 0);
                        return new Dreieck[] { Dreieck.Rechtwinklig(new double?[] { null, hval, sides[0] }).strecken(0, sides[0]) };
                    }
                }
                else
                {
                    if (theta is double thetaval)
                    {
                        Dreieck basis = new Dreieck(Math.Cos(thetaval), Math.Sin(thetaval), 1);
                        basis = basis.strecken(2, hval);
                        sides[0] = basis.seiten[2];
                        return new Dreieck[] { basis.strecken(0, sides[0]) };
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            else
            {
                if (theta is double thetaval)
                {
                    Debug.Assert(index >= 0 && index < 3);
                    return new Dreieck[] { Dreieck.ThetaEinheit(thetaval).strecken(index, sides[index]) };
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private bool invariant()
            /*
             * Die Invariante muss immer wahr sein.
             * Konkret wird geprüft ob das RWDreieck rechtwinklig ist.
             */
        {
            return Math.Abs(
                    this.dreieck.seiten[0] * this.dreieck.seiten[0]
                    + this.dreieck.seiten[1] * this.dreieck.seiten[1]
                    - this.dreieck.seiten[2] * this.dreieck.seiten[2]
                ) <= double.Epsilon;
        }
    }

    public class Dreieck
    {
        public double[] seiten = new double[3];
        public Dreieck (double a, double b, double c)
            /*
             * Kreiert ein Dreiecksobjekt aus drei gegebenen Seitenlängen.
             */
        {
            this.seiten[0] = a;
            this.seiten[1] = b;
            this.seiten[2] = c;
        }
        public Dreieck (double[] seiten)
        {
            if (seiten.Length < 3)
            {
                throw new ArgumentException("`seiten' enthält night genügend Elemente");
            }
            else
            {
                for (int c = 0; c < 3; c++)
                {
                    this.seiten[c] = seiten[c];
                } 
            }
        }

        public Dreieck clone()
        {
            return new Dreieck(this.seiten);
        }

        public static Dreieck Rechtwinklig(double? a, double? b, double? c)
            /*
             * Konstruiert ein neues rechtwinkliges Dreieckselement aus mindestens zwei gegebenen Seiten.
             * Argument `a' und `b' sind Katheten (seitenindex 0 bzw. 1).
             * Argument `c' ist die Hypothenuse (seitenindex 2).
             * höchstens eines der Argumente darf null sein. Das fehlende Argument wird berechnet.
             */
        {
            return Rechtwinklig(new double?[] { a, b, c });
        }
        public static Dreieck Rechtwinklig (double?[] seiten)
            /*
             * Konstruiert ein neues rechtwinkliges Dreieckselement aus mindestens zwei gegebenen Seiten.
             * `seiten.Length()' muss mindestens 3 betragen.
             * `seiten[0]' und `seiten[1]' sind die Katheten.
             * `seiten[2]' ist die Hypothenuse.
             * höchstens eines der der ersten drei Einträge von `seiten' darf null sein. Die fehlende Seite wird berechnet.
             */
        {
            double[] newval = new double[3];
            int nullcounter = 0;
            int nullindex = -1;

            if (seiten.Length < 3)
            {
                throw new ArgumentException("Argument `seiten' ist zu kurz; ihre länge muss (mindestens) 3 betragen");
            }

            for (int c = 0; c < 3; c++) 
            {
                if (seiten[c] is double curval)
                {
                    newval[c] = curval;
                }
                else
                {
                    nullcounter++;
                    nullindex = c;
                }
            }

            if (nullcounter > 1)
            {
                throw new ArgumentNullException();
            }

            Debug.Assert(nullindex >= 0 && nullindex < 3);

            if (nullcounter == 1)
            {
                double a = newval[(nullindex + 1) % 3];
                double b = newval[(nullindex + 2) % 3];
                if (nullindex == 2)
                {
                    newval[nullindex] = Math.Sqrt((a * a) + (b * b));
                }
                else
                {
                    newval[nullindex] = Math.Sqrt(Math.Abs((a * a) - (b * b)));
                }
            }

            return new Dreieck(newval[0], newval[1], newval[2]);
        }
        public static Dreieck ThetaEinheit(double theta)
            /*
             * Konstruiert das rechtwinklige, im Einheitskreis eingepasste, durch `theta' definierte Dreieckselement.
             * `theta' ist in radian einzugeben.
             * `theta' ist der Winkel zwischen der ersten Kathete (index 0) und der Hypothenuse (index 2).
             */
        {
            return new Dreieck(Math.Cos(theta), Math.Sin(theta), 1);
        }

        /* non-statische Funktionen */

        public Dreieck strecken(int index, double wert)
            /*
             * Berechnet ein neues formgleiches Dreieckselement dessen durch `index' definierte Seite auf die Grösse von `wert' skaliert wird.
             * Übrige Seiten werden proportionsgerecht mitskaliert.
             */
        {
            double[] neu = new double[3];
            neu[index] = wert;
            for (int c = 0; c < 3; c++)
            {
                if (c != index)
                {
                    neu[c] = this.seiten[c] * wert / this.seiten[index];
                }
            }
            return new Dreieck(neu[0], neu[1], neu[2]);
        }
        public double Umfang()
            /*
             * Berechnung des Umfangs (Summe aller Seiten).
             */
        {
            int sum = 0;
            foreach (int val in seiten)
            {
                sum += val;
            }
            return sum;
        }
        public double Fläche()
            /* 
             * Berechnung der Fläche nach dem Satz des Heron (Allgemeingültig für beliebige Dreiecke).
             */
        {
            double s = Umfang() / 2;
            double akku = s;
            foreach (double seite in this.seiten)
            {
                akku *= (s - seite);
            }
            return Math.Sqrt(akku);

        }
        public double[] Höhe()
            /*
             * Berechnung der Höhen über alle Grundseiten.
             * Der Index des Resultatvektors markiert die zugehörige Grundseite.
             */
        {
            double fläche = Fläche();
            double[] resultat = new double[3];
            for (int c = 0; c < 3; c++)
            {
                resultat[c] = 2 * fläche / seiten[c];
            }
            return resultat;
        }

    }
}
