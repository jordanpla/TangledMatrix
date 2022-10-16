using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangled;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            //double x = Math.PI;
            //Q q1 = (Q)x;
            //Console.WriteLine(q1.ToString());
            //Console.WriteLine((double)q1);
            //Console.WriteLine(x);

            //Matrix m1 = new Matrix(
            //    1, 1, 1, 0,
            //    1, 1, 1, 0,
            //    0, 0, 1, 1,
            //    0, 0, 1, 1);

            //Vector b = new Vector(2, 1, 3, 4);
            //Matrix i = Matrix.Concat(m1, Matrix.TransposedVector(b));
            //Console.WriteLine(i);

            //#region Otros ejemplos. 

            ////Compatible Indeterminado

            ////Matrix m2 = new Matrix(
            ////    2, 0, 2, 0,
            ////    0, 1, 1, 0,
            ////    1, 0, 1, 0,
            ////    0, 2, 0, 1);

            ////Vector b = new Vector(0, 2, 0, 4);

            ////#endregion

            ////#region Incompatible

            ////Matrix m3 = new Matrix(
            ////    2, 0, 2, 0,
            ////    0, 1, 1, 0,
            ////    1, 0, 1, 0,
            ////    0, 2, 0, 1);

            ////Vector c = new Vector(0, 2, 3, 4);

            //#endregion
            ////COmpatible Determinado

            //Matrix m4 = new Matrix(
            //    2, 5, 2, 6,
            //    0, 1, 1, 0,
            //    1, 0, 1, 7,
            //    0, 2, 2, 1);

            //Vector c = new Vector(0, 2, 3, 4);

            //#region Rows < Cols

            //Q[,] q5 = { { 2, 0, 2, 0 }, { 1, 0, 2, 0 }, { 0, 1, 1, 1 } };
            //Matrix m5 = new Matrix(q5);

            //Vector e = new Vector(0, 2, 3, 4);

            //#endregion

            //#region Rows > Cols

            //Q[,] q6 = { { 2, 1, 0 }, { 0, 0, 1 }, { 2, 2, 1 }, { 0, 0, 1 } };
            //Matrix m6 = new Matrix(q6);

            //Vector f = new Vector(0, 2, 3, 0);

            //#endregion

            //Console.WriteLine(Solvers.Gauss(m4, c));
            //Console.WriteLine(Solvers.GaussJordan(m4, c));
            //Console.WriteLine(Solvers.Cramer(m4, c));
            //Console.WriteLine(Solvers.GetRank(m4));
            //Console.WriteLine(Solvers.InverseMethod(m4, c));
            //Console.WriteLine(m4.Inverse);
            //Console.WriteLine(m4.Adjoint);
            //Console.WriteLine(m4.Determinant);

            Matrix m10 = new Matrix(new Q[,]
            {
                 {1,1,1},
                 {1,2,3},
                 {1,3,6},
            });

            Matrix m11 = new Matrix(new Q[,]
            {
                 {1,2,3,4},
                 {2,3,4,1},
                 {3,4,1,2},
                 {4,1,2,3},
            });

            Matrix m12 = new Matrix(new Q[,]
            {
                 {2,1,1,1,1},
                 {1,3,1,1,1},
                 {1,1,4,1,1},
                 {1,1,1,5,1},
                 {1,1,1,1,6},
            });

            Console.WriteLine(m10.Determinant);
            Console.WriteLine(m11.Determinant);
            Console.WriteLine(m12.Determinant);

            Console.WriteLine(m10.Inverse);
            Console.WriteLine(Matrix.Mul(m10, m10.Inverse));

            Console.WriteLine(m11.Inverse);
            Console.WriteLine(Matrix.Mul(m11, m11.Inverse));


            Console.WriteLine(m12.Inverse);
            Console.WriteLine(Matrix.Mul(m12, m12.Inverse));


            Matrix m19 = new Matrix(new Q[,]
            {
                 {2,-1,-1},
                 {3,4,-2},
                 {3,-2,4},
            });

            Matrix m30 = new Matrix(new Q[,]
            {
                 {1,1,2},
                 {2,-1,2},
                 {4,1,4},

            });
            Vector b19 = new Vector(4, 11, 11);
            Vector b20 = new Vector(-1, -4, -2);
            Console.WriteLine(Solvers.Cramer(m19, b19));
            Console.WriteLine();
            Console.WriteLine(Solvers.Cramer(m30, b20));

            Matrix m21 = new Matrix(new Q[,]
            {
                 {0, 4, 10, 1},
                 {4, 8, 18, 7},
                 {10, 18, 40, 17},
                 {1, 7, 17, 3},
            });

            Matrix m22 = new Matrix(new Q[,]
            {
                 {2, 1, 11, 2},
                 {1, 0, 4, -1},
                 {11, 4, 56, 5},
                 {2, -1, 5, -6},
            });

            Matrix m23 = new Matrix(new Q[,]
            {
                 {0,0,1,0,0},
                 {0,1,0,0,0},
                 {0,0,0,1,0},
                 {1,1,1,1,1},
                 {1,3,4,5,1},
                 {1,2,3,4,5},
                 {2,3,4,5,6},

            });
            Matrix m24 = new Matrix(new Q[,]
            {
                 {1,1,1,1,1},
                 {0,0,1,1,-3},
                 {0,0,1,1,-3},
                 {0,0,0,0,-1},

            });

            Console.WriteLine(Solvers.GetRank(m21));
            Console.WriteLine(Solvers.GetRank(m22));
            Console.WriteLine(Solvers.GetRank(m23));
            Console.WriteLine(Solvers.GetRank(m24));


            Matrix m220 = new Matrix(new Q[,]
            {
                 {1,2,3,4},
                 {2,3,4,1},
                 {3,4,1,2},
                 {4,1,2,3},

            });
            Vector b220 = new Vector(11, 12, 13, 14);
            Console.WriteLine(Solvers.Gauss(m220, b220));
            Console.WriteLine(Solvers.GaussJordan(m220, b220));

            Matrix m20 = new Matrix(new Q[,]
            {
                 {1,1,1,1,1},
                 {3,2,1,1,-3},
                 {0,1,2,2,6},
                 {5,4,3,3,-1},
                 {5,4,3,3,-1},

            });
            Vector v20 = new Vector(7, -2, 23, 12, 12);
            Console.WriteLine(Solvers.GaussJordan(m20, v20));    
            //Console.WriteLine(Solvers.Gauss(m20, v20));
        }
    }
}
