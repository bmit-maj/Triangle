using System;

namespace Triangle
{
    class Program
    {
        static void Main(string[] args)
        {
            String var;
            float a, b;
            Dreieck dreieck;
            Console.WriteLine("Geben Sie zwei Katheten eines rechtwinkligen Dreiecks an.");
            do {
                Console.Write("Kathete a = ");
                var = Console.ReadLine();
            } while (!float.TryParse(var, out a));
            do {
                Console.Write("Kathete b = ");
                var = Console.ReadLine();
            } while (!float.TryParse(var, out b));
            dreieck = new Dreieck(a, b);
            Console.WriteLine("Gegebene Kathetenlängen waren {0} und {1}, errechnete Hypotenuse ist:  {2}.", dreieck.A(), dreieck.B(), dreieck.C());
        }
    }
}
