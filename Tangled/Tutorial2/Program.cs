using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangled;

namespace Tutorial2
{
    class Program
    {
        static void Main(string[] args)
        {
            Vector v = new Vector(); // Vector de dimensión 0.
            Console.WriteLine(v.Dimension); // 0

            Vector v1 = new Vector(1, 2, 3); // creando a partir de valores separados por coma
            Vector v2 = new Vector(new Q[] { 2, 3, 4 }); // creando a partir de un array de Q

            Console.WriteLine("v1: " + v1);
            Console.WriteLine("v2: " + v2);
            Console.WriteLine("v1+v2: "+ (v1 + v2)); // Operando dos vectores.
            Console.WriteLine("v1*v2: " + (v1 * v2)); // Multiplicación componente a componente
            Console.WriteLine("dot(v1, v2): "+Vector.Dot(v1, v2)); // Producto escalar.
            Console.WriteLine();

            Console.WriteLine(Vector.O(4)); // (0, 0, 0, 0)
            Console.WriteLine(Vector.Expanded(4, 1)); // (1, 1, 1, 1)
            Console.WriteLine();

            Vector v3 = v2 + Vector.O(3);
            Console.WriteLine(v3 == v2);
            Console.WriteLine();

            Console.WriteLine(Vector.Length(new Vector(1, 1, 1))); // sqrt(3)
            Console.WriteLine(Vector.SqrLength(new Vector(1, 1, 1))); // 3
            Console.WriteLine();

            Vector v4 = new Vector(1, 2, 3);
            var elements = v4.ToArray(); // El ToArray() no permite modificar el vector.
            elements[0] = 4;
            Console.WriteLine(v4);
            Console.WriteLine(new Vector(elements));
        }
    }
}
