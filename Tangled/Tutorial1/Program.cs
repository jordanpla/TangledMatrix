using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangled;

namespace Tutorial1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Creando valores racionales
            {
                Q q1 = new Q(3, 5); // Obteniendo el valor 3/5
                Q q2 = new Q(5); // Obteniendo el valor 5.
                                 //Q q2 = 5;  // Equivalente a la linea anterior.
                Q q3 = (Q)(3 / 5.0); // Obteniendo el valor 3/5 a partir de un double.
                Console.WriteLine(q3.ToString()); // Imprimiendo el racional recuperado del double.
                Console.WriteLine();
            }
            #endregion

            #region Chequeando simplificación en Q.
            Q q = new Q(12, -4);
            Console.WriteLine(q.Numerator + "/" + q.Denominator);
            Console.WriteLine();
            #endregion

            #region Aproximando valores irracionales con Q
            {
                Q q4 = (Q)Math.PI;
                Console.WriteLine((q4).ToString()); // Imprimiendo la aproximacion de Pi en racional
                Console.WriteLine((Math.PI).ToString()); // Imprimiendo el valor double para Pi
                Console.WriteLine(((double)q4).ToString()); // Imprimiendo el valor double obtenido del racional.

                Q q5 = (Q)Math.E;
                Console.WriteLine((q5).ToString()); // Imprimiendo la aproximacion de E en racional
                Console.WriteLine((Math.E).ToString()); // Imprimiendo el valor double para E
                Console.WriteLine(((double)q5).ToString()); // Imprimiendo el valor double obtenido del racional.
                Console.WriteLine();
            }
            #endregion

            #region Operando valores racionales

            Console.WriteLine((new Q(1, 2) + new Q(3, 5)).ToString());
            Console.WriteLine((new Q(1, 2) < new Q(3, 5)).ToString());
            Console.WriteLine((new Q(1, 2) == new Q(7, 14)).ToString());
            Console.WriteLine();

            #endregion

            #region Funciones en Q.
            Console.WriteLine(Q.Abs(new Q(-4)).ToString());
            Console.WriteLine(Q.Sqr(new Q(4)).ToString());
            Console.WriteLine(Q.One.ToString());
            Console.WriteLine(Q.Zero.ToString());
            Console.WriteLine();
            #endregion
        }
    }
}
