using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangled
{
    /// <summary>
    /// Permite resolver un sistema de ecuaciones lineales mediante varios algoritmos.
    /// </summary>
    public static class Solvers
    {
        #region Variables

        static int count;
        static Q det = 1;
        public static int signo = 1;
        static int pivpos;
        static int rows = 0;
        static int cols = 0;
        static bool[,] wasSelctedAsPivot;
        static Matrix independents = new Matrix();
        static int rank = 0;
        #endregion

        /// <summary>
        /// Obtiene la resolución de un sistema de ecuaciones mediante el método de Gauss.
        /// </summary>
        /// <param name="A">Matriz con los coeficientes en el sistema.</param>
        /// <param name="B">Vector con los términos independientes.</param>
        /// <returns>Un vector con la solución del sistema.</returns>
        public static Vector Gauss (Matrix A, Vector B)
        {
            Vector result = new Vector(A.Cols);
            independents = Matrix.TransposedVector(B);
            Matrix extended = Matrix.Concat(A, independents);
            Matrix afterGauss = MyGauss(extended);
            rank = GetRank(extended);
            if (GetRank(A) != rank)
                throw new Exception("Sistema Incompatible");
            if (rank < extended.Rows)
                throw new Exception("Sistema Compatible Indeterminado");
            if (!IsStaggered(afterGauss))
                afterGauss = SwapRows(afterGauss);
            result = GetSolutionsByGauss(afterGauss);
            return result;
        }

        /// <summary>
        /// Obtiene la resolución de un sistema de ecuaciones mediante el método de Gauss Jordan.
        /// </summary>
        /// <param name="A">Matriz con los coeficientes en el sistema.</param>
        /// <param name="B">Vector con los términos independientes.</param>
        /// <returns>Un vector con la solución del sistema.</returns>
        public static Vector GaussJordan(Matrix A, Vector B)
        {
            independents = Matrix.TransposedVector(B);
            Matrix extended = Matrix.Concat(A, independents);
            rank = GetRank(extended);
            if (GetRank(A) != rank)
                throw new Exception("Sistema Incompatible");
            if (rank < extended.Rows)
                throw new Exception("Sistema Compatible Indeterminado");
            A = MyGauss(extended);
            A = SwapRows(A);
            A = MyGaussInverse(A);
            for (int i = 0; i < A.Rows; i++)
            {
                Q[] temp = A.GetRow(i).ToArray();
                for (int j = 0; j < A.Cols; j++)
                {
                    temp[j] /= A[i, i];
                }
                Vector newrow = new Vector(temp);
                A = A.ReplaceRow(i, newrow);
            }
            return A.GetCol(A.Cols - 1);
        }

        /// <summary>
        /// Obtiene la resolución de un sistema de ecuaciones mediante el método de Cramer.
        /// </summary>
        /// <param name="A">Matriz con los coeficientes en el sistema.</param>
        /// <param name="B">Vector con los términos independientes.</param>
        /// <returns>Un vector con la solución del sistema.</returns>
        public static Vector Cramer (Matrix A, Vector B)
        {
            Q[] result = new Q[A.Rows];
            det = A.Determinant;
            if (det == 0)
                throw new ArgumentException("Determinante es 0");

            for (int i = 0; i < result.Length; i++)
            {
                Vector temp = A.GetCol(i);
                A = A.ReplaceCol(i, B);
                Q delta = A.Determinant;
                result[i] = delta / det;
                A = A.ReplaceCol(i, temp);
            }
            return new Vector(result);
        }

        /// <summary>
        /// Obtiene el rango de una matriz.
        /// </summary>
        public static int GetRank (Matrix A)
        {
            Tuple<int, int> Zeros = RowsAndColsZero(MyGauss(A));
            return Math.Min(A.Rows - Zeros.Item1, A.Cols - Zeros.Item2);
        }

        /// <summary>
        /// Obtiene la resolución de un sistema de ecuaciones mediante el método de Kramer.
        /// </summary>
        /// <param name="A">Matriz con los coeficientes en el sistema.</param>
        /// <param name="B">Vector con los términos independientes.</param>
        /// <returns>Un vector con la solución del sistema.</returns>
        public static Vector InverseMethod (Matrix A, Vector B)
        {
            Q[] result = new Q[A.Rows];
            if (HasInverse(A))
            {
                independents = Matrix.Mul(A.Inverse, Matrix.TransposedVector(B));
                for (int i = 0; i < independents.Rows; i++)
                {
                    result[i] = independents[i, 0];
                }
            }
            else
                throw new ArgumentException("No tiene inversa");
            return new Vector(result);
        }

        #region Método Auxiliar

        public static Matrix MyGauss(Matrix matrix)
        {
            count = 0;
            signo = 1;
            wasSelctedAsPivot = new bool[matrix.Rows, 1];
            Vector rowtochange = new Vector();

            while (count < Math.Min(matrix.Rows, matrix.Cols))
            {
                Vector pivot = SelectPivot(matrix);

                for (int i = 0; i < matrix.Rows; i++)
                {
                    if (wasSelctedAsPivot[i, 0]) continue;

                    rowtochange = matrix.GetRow(i);

                    if (rowtochange[count] == 0) continue;

                    Q zero = rowtochange[count] / -pivot[count];
                    Q[] newrow = new Q[matrix.Cols];

                    for (int cols = 0; cols < matrix.Cols; cols++)
                    {
                        newrow[cols] = (zero * pivot[cols] + rowtochange[cols]);
                    }
                    matrix = matrix.ReplaceRow(i, new Vector(newrow));
                }
                pivpos++;
                count++;
            }
            return matrix;
        }

        static Matrix MyGaussInverse(Matrix matrix)
        {
            count = matrix.Rows - 1;
            wasSelctedAsPivot = new bool[matrix.Rows, 1];
            Vector rowtochange = new Vector();
            while (count > 0)
            {
                Vector pivot = SelectPivotInverse(matrix);
                for (int i = matrix.Rows - 1; i >= 0; i--)
                {
                    if (wasSelctedAsPivot[i, 0]) continue;

                    rowtochange = matrix.GetRow(i);

                    if (rowtochange[count] == 0) continue;

                    Q zero = rowtochange[count] / -pivot[count];
                    Q[] newrow = new Q[matrix.Cols];

                    for (int cols = 0; cols < matrix.Cols; cols++)
                    {
                        newrow[cols] = (zero * pivot[cols] + rowtochange[cols]);
                    }
                    matrix = matrix.ReplaceRow(i, new Vector(newrow));
                }
                pivpos++;
                count--;
            }
            return matrix;
        }

        static Vector SelectPivot(Matrix matrix)
        {
            Vector pivot = new Vector(0);
            for (int i = 0; i < matrix.Rows; i++)
            {
                if (!wasSelctedAsPivot[i, 0] && matrix[i, count] == 0)
                {
                    signo *= -1;
                }
                if (!wasSelctedAsPivot[i, 0] && matrix[i, count] != 0)
                {
                    pivot = matrix.GetRow(i);
                    pivpos = i;
                    wasSelctedAsPivot[i, 0] = true;
                    break;
                }
            }
            return pivot;
        }
        static Vector SelectPivotInverse(Matrix matrix)
        {
            Vector pivot = new Vector(0);
            for (int i = matrix.Rows - 1; i > 0; i--)
            {
                if (!wasSelctedAsPivot[i, 0] && matrix[i, count] != 0)
                {
                    pivot = matrix.GetRow(i);
                    pivpos = i;
                    wasSelctedAsPivot[i, 0] = true;
                    break;
                }
            }
            return pivot;
        }
        static Tuple<int, int> RowsAndColsZero(Matrix matrix)
        {
            rows = cols = 0;
            for (int i = 0; i < matrix.Rows; i++)
            {
                int colindex = 0;
                for (int j = 0; j < matrix.Cols && matrix[i, j] == 0; j++)
                {
                    colindex++;
                    int rowindex = 0;
                    for (int k = 0; k < matrix.Rows && matrix[k, j] == 0; k++)
                    {
                        rowindex++;
                    }
                    if (rowindex == matrix.Rows)
                        cols++;
                }
                if (colindex == matrix.Cols)
                    rows++;
            }
            return new Tuple<int, int>(rows, cols);
        }

        static bool HasInverse(Matrix matrix)
        {
            if (matrix.Rows != matrix.Cols)
                return false;
            det = matrix.Determinant;
            if (det == 0)
                return false;
            return true;
        }
        static Matrix SwapRows(Matrix matrix)
        {
            for (int i = 0; i < matrix.Cols - 1; i++)
            {
                if (matrix[i, i] == 0)
                {
                    for (int j = i + 1; j < matrix.Rows; j++)
                    {
                        if (matrix[j, i] != 0)
                        {
                            Vector tempA = matrix.GetRow(i);
                            Vector tempB = matrix.GetRow(j);
                            matrix = matrix.ReplaceRow(i, tempB);
                            matrix = matrix.ReplaceRow(j, tempA);
                            break;
                        }
                    }
                }
            }
            return matrix;
        }
        static bool IsStaggered(Matrix matrix)
        {
            for (int i = 0; i < matrix.Cols - 1; i++)
            {
                if (matrix[i, i] == 0)
                    return false;
            }
            return true;
        }
        static Vector GetSolutionsByGauss(Matrix matrix)
        {
            Q[] solutions = new Q[matrix.Cols - 1];
            for (int i = matrix.Rows - 1; i >= 0; i--)
            {
                solutions[i] = matrix[i, matrix.Cols - 1];
                for (int j = matrix.Cols - 2; j > i; j--)
                {
                    solutions[i] -= matrix[i, j] * solutions[j];
                }
                solutions[i] /= matrix[i, i];
            }
            return new Vector(solutions);
        }

        #endregion
    }
}
