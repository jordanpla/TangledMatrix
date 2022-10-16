using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangled
{
    /// <summary>
    /// Representa un vector fila como un valor inmutable.
    /// </summary>
    public struct Vector
    {
        /// <summary>
        /// Representación en memoria de los elementos del vector.
        /// </summary>
        private Q[] elements;

        /// <summary>
        /// Permite construir un vector a partir de una secuencia de valores separadas por coma o un array.
        /// </summary>
        public Vector (params Q[] elements) : this(elements, true)
        {
        }

        /// <summary>
        /// Representa una creación que puede evitar la copia innecesaria del array de entrada.
        /// </summary>
        internal Vector(Q[] elements, bool copy)
        {
            // Los elementos se copian para garantizar la inmutabilidad del objeto después de creado.
            this.elements = copy ? elements.Clone() as Q[] : elements;
        }

        /// <summary>
        /// Obtiene el vector nulo para determinada dimensión.
        /// </summary>
        public static Vector O(int dimension)
        {
            return new Vector(new Q[dimension], false);
        }

        /// <summary>
        /// Obtiene un vector con el mismo valor en cada componente de determinada dimensión.
        /// </summary>
        public static Vector Expanded(int dimension, Q value)
        {
            Q[] elements = new Q[dimension];
            for (int i = 0; i < elements.Length; i++)
                elements[i] = value;
            return new Vector(elements, false);
        }

        /// <summary>
        /// Obtiene una copia de los elementos del vector en un array.
        /// </summary>
        public Q[] ToArray()
        {
            return elements.Clone() as Q[];
        }

        /// <summary>
        /// Permite acceder en forma de sólo-lectura al index-ésimo elemento del vector.
        /// </summary>
        public Q this[int index]
        {
            get
            {
                if (index < 0 || index >= Dimension)
                    throw new IndexOutOfRangeException();

                return elements[index];
            }
        }

        /// <summary>
        /// Obtiene la dimensión (aridad) del vector.
        /// </summary>
        public int Dimension { get { return elements == null ? 0 : elements.Length; } }

        /// <summary>
        /// Obtiene la suma de dos vectores de igual dimensión.
        /// </summary>
        public static Vector operator + (Vector v1, Vector v2)
        {
            if (v1.Dimension != v2.Dimension)
                throw new ArgumentException();

            Q[] result = new Q[v1.Dimension];
            for (int i = 0; i < v1.Dimension; i++)
                result[i] = v1[i] + v2[i];

            return new Vector(result, false);
        }

        /// <summary>
        /// Obtiene la diferencia de dos vectores de igual dimensión.
        /// </summary>
        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Dimension != v2.Dimension)
                throw new ArgumentException();

            Q[] result = new Q[v1.Dimension];
            for (int i = 0; i < v1.Dimension; i++)
                result[i] = v1[i] - v2[i];

            return new Vector(result, false);
        }

        /// <summary>
        /// Obtiene la multiplicación por componente de dos vectores de igual dimensión.
        /// </summary>
        public static Vector operator *(Vector v1, Vector v2)
        {
            if (v1.Dimension != v2.Dimension)
                throw new ArgumentException();

            Q[] result = new Q[v1.Dimension];
            for (int i = 0; i < v1.Dimension; i++)
                result[i] = v1[i] * v2[i];

            return new Vector(result, false);
        }

        /// <summary>
        /// Obtiene la división por componente de dos vectores de igual dimensión.
        /// </summary>
        public static Vector operator /(Vector v1, Vector v2)
        {
            if (v1.Dimension != v2.Dimension)
                throw new ArgumentException();

            Q[] result = new Q[v1.Dimension];
            for (int i = 0; i < v1.Dimension; i++)
                result[i] = v1[i] / v2[i];

            return new Vector(result, false);
        }

        /// <summary>
        /// Obtiene la multiplicación por un escalar.
        /// </summary>
        public static Vector operator *(Vector v1, Q alpha)
        {
            Q[] result = new Q[v1.Dimension];
            for (int i = 0; i < v1.Dimension; i++)
                result[i] = v1[i] * alpha;

            return new Vector(result, false);
        }

        /// <summary>
        /// Obtiene la multiplicación por un escalar.
        /// </summary>
        public static Vector operator *(Q alpha, Vector v1)
        {
            Q[] result = new Q[v1.Dimension];
            for (int i = 0; i < v1.Dimension; i++)
                result[i] = v1[i] * alpha;

            return new Vector(result, false);
        }

        /// <summary>
        /// Obtiene la división por un escalar.
        /// </summary>
        public static Vector operator /(Vector v1, Q alpha)
        {
            Q[] result = new Q[v1.Dimension];
            for (int i = 0; i < v1.Dimension; i++)
                result[i] = v1[i] / alpha;

            return new Vector(result, false);
        }

        /// <summary>
        /// Obtiene el producto escalar (dot) ente dos vectores.
        /// </summary>
        public static Q Dot (Vector v1, Vector v2)
        {
            if (v1.Dimension != v2.Dimension)
                throw new ArgumentException();

            Q accum = 0;
            for (int i = 0; i < v1.Dimension; i++)
                accum += v1[i] * v2[i];

            return accum;
        }

        /// <summary>
        /// Determina la norma 2 de un vector.
        /// </summary>
        public static double Length (Vector v)
        {
            Q accum = Dot(v, v);
            return Math.Sqrt(accum);
        }

        /// <summary>
        /// Determina la longitud al cuadrado del vector.
        /// </summary>
        public static Q SqrLength(Vector v)
        {
            return Dot(v, v);
        }

        /// <summary>
        /// Determina cuándo dos vectores son iguales comprobando componente a componente.
        /// </summary>
        public static bool operator == (Vector v1, Vector v2)
        {
            return v1.Equals(v2);
        }
        /// <summary>
        /// Determina cuándo dos vectores son distintos comprobando componente a componente.
        /// </summary>
        public static bool operator != (Vector v1, Vector v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        /// Determina cuándo el vector es igual a otro comprobando componente a componente.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector))
                return false;

            Vector v = (Vector)obj;
            if (v.Dimension != this.Dimension)
                return false;
            for (int i = 0; i < v.Dimension; i++)
                if (v[i] != this[i])
                    return false;
            return true;
        }

        /// <summary>
        /// Determina un valor de hash para poder utilizar el objeto vector como llave en un diccionario.
        /// </summary>
        public override int GetHashCode()
        {
            int hashing = Dimension.GetHashCode();
            for (int i = 0; i < Dimension; i++)
                hashing ^= this[i].GetHashCode();
            return hashing;
        }

        public override string ToString()
        {
            return "(" + string.Join(",", elements.Select(e => e.ToString())) + ")";
        }
    }
}
