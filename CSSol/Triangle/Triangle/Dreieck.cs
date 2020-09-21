using System;
using System.Collections.Generic;
using System.Text;

namespace Triangle
{
    public class Dreieck
    {
        private double a;
        private double b;
        private double c;

        private double surface;

        private bool simple;

        public Dreieck(double a, double b)
            /*
             * Kreiert ein Dreiecksobjekt aus zwei gegebenen Kathetenlängen eines Rechtwinkligen Dreiecks.
             */
        {
            this.a = a;
            this.b = b;
            this.c = Math.Sqrt(a * a + b * b);
            this.surface = a * b / 2;
            simple = true;
        }

        public double A()
        {
            return this.a;
        }
        public double B()
        {
            return this.b;
        }
        public double C()
        {
            return this.c;
        }
        public double Umfang()
        {
            return this.a + this.b + this.c;
        }

        public double Fläche()
        {
            return this.surface;
        }

    }
}
