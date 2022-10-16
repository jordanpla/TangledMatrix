using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangled;

namespace Tutorial3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creando una matriz a partir de un array bidimensional
            Matrix m1 = new Matrix(new Q[,]
            {
                { 1, 1, 0 },
                { 2, 3, 1 }
            });
            // Creando una matriz cuadrada a partir de una secuencia de valores
            Matrix m2 = new Matrix(
                2, 3, 0,
                4, 1, (Q)1/2,
                1, 0, 0
                );
            // Multiplicando dos matrices
            Matrix m3 = Matrix.Mul(m1, m2);
            Console.WriteLine("mul(m1, m2)=m3");
            Console.WriteLine(m3);

            Matrix a = new Matrix (new Q[,]
            {
                { 1, 1, 0 },
                { 2, 3, 1 }
            });
            Matrix b = new Matrix(new Q[,]
            {
                { 2, -1, 2 },
                { 1, 0, 1 }
            });

            Console.WriteLine(a + b);
            Console.WriteLine(a - b);
            Console.WriteLine(a * b);
            Console.WriteLine(a.Transpose);
            Console.WriteLine(a.SliceCols(1, 2));
            Console.WriteLine(Matrix.Concat (a, b));
            Console.WriteLine(a.ReplaceCol(1, Vector.Expanded(2, new Q(3, 2))));
        }
    }
}
