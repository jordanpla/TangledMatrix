using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangled
{
    /// <summary>
    /// Representa una matriz como un tipo por valor e inmutable.
    /// </summary>
    public struct Matrix
    {
        #region Variables
        /// <summary>
        /// Representación en memoria de la matriz a partir de un array bidimensional.
        /// La dimensión 0 del array son los vectores filas y la dimensión 1 del array son los vectores columna.
        /// </summary>
        Q[,] mat;
        #endregion

        #region Constructors

        /// <summary>
        /// Permite crear una matriz especificando si es necesario realizar una copia de los elementos.
        /// </summary>
        private Matrix(Q[,] values, bool copy)
        {
            this.mat = copy ? (Q[,])values.Clone() : values;
        }
        /// <summary>
        /// Inicializa una matriz a partir de un array bidimensional con los elementos de la matriz.
        /// </summary>
        public Matrix(Q[,] values):this(values, true)
        {
        }
       
        /// <summary>
        /// Método utilitario para construir una matriz cuadrada a partir de sus elementos dados en forma secuencial.
        /// </summary>
        static Q[,] SqrVersion (Q[] values)
        {
            int dimension = (int)Math.Sqrt(values.Length);
            if (values.Length != dimension * dimension)
                throw new ArgumentException("Not square matrix");
            Q[,] matrix = new Q[dimension, dimension];
            int count = 0;
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                    matrix[i, j] = values[count++];
            return matrix;
        }
        /// <summary>
        /// Inicializa una matriz cuadrada a partir de sus elementos dados en forma secuencial.
        /// </summary>
        public Matrix(params Q[] values) : this(SqrVersion(values), false)
        {
        }
        #endregion

        #region Properties and Indexers

        /// <summary>
        /// Obtiene la cantidad de filas de esta matriz.
        /// </summary>
        public int Rows { get { return mat == null ? 0 : mat.GetLength(0); } }

        /// <summary>
        /// Obtiene la cantidad de columnas de esta matriz.
        /// </summary>
        public int Cols { get { return mat == null  ? 0 : mat.GetLength(1); } }

        /// <summary>
        /// Accede de forma sólo-lectura al elemento de la matriz en determinada fila (r) y columna (c).
        /// </summary>
        public Q this[int r, int c]
        {
            get { return mat[r,c]; }
        }

        /// <summary>
        /// Obtiene el vector de la fila r de esta matriz.
        /// </summary>
        public Vector this[int r] { get { return GetRow(r); } }



        /// <summary>
        /// Obtiene la transpuesta de esta matriz.
        /// </summary>
        public Matrix Transpose
        {
            get
            {
                Q[,] tElements = new Q[Cols, Rows];
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        tElements[j, i] = this[i, j];
                return new Matrix(tElements, false);
            }
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Crea una matriz cuadrada a partir de los elementos en su diagonal.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Matrix Diagonal (Vector values)
        {
            Q[,] mat = new Q[values.Dimension, values.Dimension];
            for (int i = 0; i < values.Dimension; i++)
                mat[i, i] = values[i];
            return new Matrix(mat, false);
        }

        /// <summary>
        /// Crea una matriz identidad de determinada dimensión.
        /// </summary>
        public static Matrix Identity (int dimension)
        {
            return Diagonal(Vector.Expanded(dimension, 1));
        }

        /// <summary>
        /// Crea una matriz nula con determinadas dimensiones.
        /// </summary>
        public static Matrix Zero (int rows, int cols)
        {
            return new Matrix(new Q[rows, cols], false);
        }
        
        /// <summary>
        /// Crea una matriz nula cuadrada con determinada dimensión.
        /// </summary>
        public static Matrix Zero(int dimension)
        {
            return Zero(dimension, dimension);
        }

        /// <summary>
        /// Obtiene la matriz resultado de transponer un vector fila.
        /// </summary>
        /// <returns>Una matriz de una columna y tantas filas como dimensión tenga el vector.</returns>
        public static Matrix TransposedVector (Vector v)
        {
            Q[,] mat = new Q[v.Dimension, 1];
            for (int i = 0; i < v.Dimension; i++)
                mat[i, 0] = v[i];
            return new Matrix(mat, false);
        }

        /// <summary>
        /// Obtiene una matriz como extensión de la matriz left con la matriz right.
        /// </summary>
        public static Matrix Concat(Matrix left, Matrix right)
        {
            if (left.Rows != right.Rows)
                throw new ArgumentException();

            Q[,] elements = new Q[left.Rows, left.Cols + right.Cols];

            for (int i = 0; i < left.Rows; i++)
                for (int j = 0; j < left.Cols; j++)
                    elements[i, j] = left[i, j];

            for (int i = 0; i < right.Rows; i++)
                for (int j = 0; j < right.Cols; j++)
                    elements[i, j + left.Cols] = right[i, j];

            return new Matrix(elements, false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Obtiene el vector de la fila index de esta matriz
        /// </summary>
        public Vector GetRow(int index)
        {
            if (index < 0 || index >= Rows)
                throw new ArgumentOutOfRangeException();

            Q[] elements = new Q[Cols];
            for (int i = 0; i < Cols; i++)
                elements[i] = this[index, i];
            return new Vector(elements);
        }

        /// <summary>
        /// Obtiene el vector de la columna index de esta matriz en forma de vector fila.
        /// </summary>
        public Vector GetCol(int index)
        {
            if (index < 0 || index >= Cols)
                throw new ArgumentOutOfRangeException();

            Q[] elements = new Q[Rows];
            for (int i = 0; i < Rows; i++)
                elements[i] = this[i, index];
            return new Vector(elements);
        }

        /// <summary>
        /// Reemplaza determinada fila de la matriz por un vector y devuelve la matriz resultante.
        /// </summary>
        public Matrix ReplaceRow (int row, Vector vector)
        {
            if (row < 0 || row >= Rows)
                throw new ArgumentOutOfRangeException();

            if (vector.Dimension != Cols)
                throw new ArgumentException();

            Q[,] elements = (Q[,])this.mat.Clone();
            for (int i = 0; i < vector.Dimension; i++)
                elements[row, i] = vector[i];
            return new Matrix(elements, false);
        }

        /// <summary>
        /// Reemplaza determinada columna de la matriz por un vector y devuelve la matriz resultante.
        /// </summary>
        public Matrix ReplaceCol(int col, Vector vector)
        {
            if (col < 0 || col >= Cols)
                throw new ArgumentOutOfRangeException();

            if (vector.Dimension != Rows)
                throw new ArgumentException();

            Q[,] elements = (Q[,])this.mat.Clone();
            for (int i = 0; i < vector.Dimension; i++)
                elements[i, col] = vector[i];
            return new Matrix(elements, false);
        }

        /// <summary>
        /// Obtiene la submatriz formada por las columnas desde start tomando count columnas.
        /// </summary>
        public Matrix SliceCols(int start, int count)
        {
            if (start < 0 || start >= Cols || count <= 0 || start + count > Cols)
                throw new ArgumentOutOfRangeException();

            Q[,] values = new Q[Rows, count];
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < count; j++)
                    values[i, j] = this[i, j + start];
            return new Matrix(values, false);
        }

        /// <summary>
        /// Obtiene la submatriz formada por las filas desde start tomando count filas.
        /// </summary>
        public Matrix SliceRows(int start, int count)
        {
            if (start < 0 || start >= Rows || count <= 0 || start + count > Rows)
                throw new ArgumentOutOfRangeException();

            Q[,] values = new Q[count, Cols];
            for (int i = 0; i < count; i++)
                for (int j = 0; j < Cols; j++)
                    values[i, j] = this[i + start, j];
            return new Matrix(values, false);
        }

        #endregion

        #region Operators

        public static Matrix operator + (Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
                throw new ArgumentException("Different dimensions.");

            Q[,] elements = new Q[m1.Rows, m1.Cols];
            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Cols; j++)
                    elements[i, j] = m1[i, j] + m2[i, j];
            return new Matrix(elements, false);
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
                throw new ArgumentException("Different dimensions.");

            Q[,] elements = new Q[m1.Rows, m1.Cols];
            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Cols; j++)
                    elements[i, j] = m1[i, j] - m2[i, j];
            return new Matrix(elements, false);
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
                throw new ArgumentException("Different dimensions.");

            Q[,] elements = new Q[m1.Rows, m1.Cols];
            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Cols; j++)
                    elements[i, j] = m1[i, j] * m2[i, j];
            return new Matrix(elements, false);
        }

        public static Matrix operator *(Matrix m, Q alpha)
        {
            Q[,] result = new Q[m.Rows, m.Cols];
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    result[i, j] = m[i, j] * alpha;
            return new Matrix(result, false);
        }


        public static Matrix operator *(Q alpha, Matrix m)
        {
            return m * alpha;
        }

        public static Matrix operator /(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
                throw new ArgumentException("Different dimensions.");

            Q[,] elements = new Q[m1.Rows, m1.Cols];
            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Cols; j++)
                    elements[i, j] = m1[i, j] / m2[i, j];
            return new Matrix(elements, false);
        }

        public static Matrix Mul (Matrix m1, Matrix m2)
        {
            if (m1.Cols != m2.Rows)
                throw new ArgumentException("m1 columns must be equals to m2 rows");

            Q[,] result = new Q[m1.Rows, m2.Cols];
            for (int i = 0; i < result.GetLength(0); i++)
                for (int j = 0; j < result.GetLength(1); j++)
                    result[i, j] = Vector.Dot(m1.GetRow(i), m2.GetCol(j));

            return new Matrix(result, false);
        }

        public static Vector Mul(Vector v, Matrix m)
        {
            if (v.Dimension != m.Rows)
                throw new ArgumentException();

            Q[] result = new Q[m.Cols];
            for (int i = 0; i < m.Cols; i++)
                result[i] = Vector.Dot(v, m.GetCol(i));
            return new Vector(result); 
        }

        public static bool operator ==(Matrix v1, Matrix v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Matrix v1, Matrix v2)
        {
            return !(v1 == v2);
        }


        #endregion

        #region Object overrides

        public override string ToString()
        {
            string value = "";
            for (int i=0; i<Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                    value += ((double)this[i, j]) + "\t";
                value += "\n";
            }
            return value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix))
                return false;

            Matrix v = (Matrix)obj;
            if (v.Rows != this.Rows || v.Cols != this.Cols)
                return false;
            for (int i = 0; i < v.Rows; i++)
                for (int j = 0; j < v.Cols; j++)
                    if (v[i, j] != this[i, j])
                        return false;
            return true;
        }

        public override int GetHashCode()
        {
            int hashing = Rows.GetHashCode() ^ (Cols.GetHashCode() * 1020 + 14);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    hashing ^= this[i, j].GetHashCode();
            return hashing;
        }

        #endregion

        static int count = 0;
        static Q result = 1;
        static Q det = 1;

        #region TO DO

        /// <summary>
        /// Cuando este implementada, deberá devolver la matriz inversa de una matriz cuadrada.
        /// </summary>
        public Matrix Inverse
        {
            get
            {
                if (Rows != Cols)
                    throw new ArgumentException("La cantidad de filas no corresponde con la cantidad de columnas.");
                det = Determinant;

                if (det == 0)
                    throw new ArgumentException("Determinante es 0. La Matriz no tiene inversa.");
                Q[,] inverse = new Q[mat.GetLength(0), mat.GetLength(1)];

                for (int i = 0; i < inverse.GetLength(0); i++)
                {
                    for (int j = 0; j < inverse.GetLength(1); j++)
                    {
                        inverse[i, j] = Adjoint[i, j] * (1 / det);
                    }
                }
                return new Matrix(inverse);
            }
        }

        /// <summary>
        /// Cuando este implementada, deberá devolver la matriz adjunta de una matriz cuadrada.
        /// </summary>
        public Matrix Adjoint
        {
            get
            {
                Q[,] matrixWithCofactor = new Q[mat.GetLength(0), mat.GetLength(1)];

                for (int i = 0; i < matrixWithCofactor.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixWithCofactor.GetLength(1); j++)
                    {
                        matrixWithCofactor[i, j] = (((i + j) % 2 == 0) ? 1 : -1) * MatrixCut(i, j).Determinant;
                    }
                }
                return new Matrix(matrixWithCofactor).Transpose;
            }
        }

        /// <summary>
        /// Cuando este implementada, deberá devolver el determinante de la matriz.
        /// </summary>
        public Q Determinant
        {
            get
            {
                if (Rows != Cols)
                    throw new ArgumentException("La matriz no es cuadrada.");

                count = 0;
                Solvers.signo = 1;
                Matrix aux = Clone(new Matrix(mat));
                aux = Solvers.MyGauss(aux);
                result = 1;
                count = 0;

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        if (aux[i, j] != 0)
                        {
                            result *= aux[i, j];
                            count++;
                            break;
                        }
                    }
                }
                if (count != Rows)
                    result *= 0;
                return result * Solvers.signo;
            }
        }

        #endregion

        #region Método Auxiliar
        Matrix Clone(Matrix A)
        {
            Q[,] result = new Q[A.Cols, A.Rows];
            for (int i = 0; i < A.Cols; i++)
            {
                for (int j = 0; j < A.Rows; j++)
                {
                    result[i, j] = A[i, j];
                }
            }
            return new Matrix(result);
        }

        Matrix MatrixCut(int row, int col)
        {
            Q[,] cut = new Q[mat.GetLength(0) - 1, mat.GetLength(1) - 1];
            int indexK = 0;
            for (int k = 0; k < mat.GetLength(0); k++)
            {
                if (row != k)
                {
                    int indexH = 0;
                    for (int h = 0; h < mat.GetLength(1); h++)
                    {
                        if (col != h)
                            cut[indexK, indexH++] = mat[k, h];
                    }
                    indexK++;
                }
            }
            return new Matrix(cut);
        }

        #endregion
    }
}
