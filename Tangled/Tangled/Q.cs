using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangled
{
    /// <summary>
    /// Representa un valor racional determinado por un numerador y el denominador.
    /// </summary>
    public struct Q : IComparable<Q>, IFormattable
    {
        #region Variables
        /// <summary>
        /// Devuelve el numerador.
        /// </summary>
        private long _p;
        /// <summary>
        /// Devuelve el denominador disminuido en uno. De esta forma el valor por defecto del struct es 0/1.
        /// </summary>
        private long _qMinusOne;
        #endregion

        #region Static Properties
        /// <summary>
        /// Valor 0 de tipo racional.
        /// </summary>
        public static readonly Q Zero = new Q();

        /// <summary>
        /// Valor 1 de tipo racional.
        /// </summary>
        public static readonly Q One = new Q(1, 1);
        #endregion

        #region Properties
        /// <summary>
        /// Devuelve el numerador del racional. Los racionales están siempre simplificados y el signo del racional depende únicamente del numerador.
        /// </summary>
        public long Numerator { get { return _p; } }

        /// <summary>
        /// Devuelve el denominador del racional. Garantizado siempre es un valor positivo.
        /// </summary>
        public long Denominator { get { return _qMinusOne + 1; } }

        /// <summary>
        /// Devuelve si el racional es el valor 0.
        /// </summary>
        public bool IsZero { get { return Numerator == 0; } }
        #endregion

        #region Constructors

        /// <summary>
        /// Métodos utilitarios para la simplificación de los racionales.
        /// </summary>
        static long MCD(long a, long b)
        {
            if (a < 0) a *= -1;
            if (b < 0) b *= -1;
            return a < b ? rec_MCD(b, a) : rec_MCD(a, b);
        }
        static long rec_MCD(long a, long b)
        {
            return b == 0 ? a : rec_MCD(b, a % b);
        }

        static long MCD(long a, long b, double error)
        {
            if (a < 0) a *= -1;
            if (b < 0) b *= -1;
            return a < b ? rec_MCD(b, a, error) : rec_MCD(a, b, error);
        }
        static long rec_MCD (long a, long b, double error)
        {
            if (b / (double)a < error)
                return a;
            return rec_MCD(b, a % b, error);
        }


        /// <summary>
        /// Inicializa un racional a partir de la división entre dos enteros p y q.
        /// </summary>
        /// <param name="q">Debe ser distinto de 0.</param>
        public Q(long p, long q)
        {
            if (q == 0)
                throw new ArgumentException("Division by 0.");

            // forcing equals values to have equal representation
            if (p == 0) q = 1; 
            if (q < 0) { p *= -1; q *= -1; }

            long mcd = MCD(p, q);


            this._p = p / mcd;
            this._qMinusOne = q/mcd - 1;
        }

        /// <summary>
        /// Inicializa un racional a partir de un entero.
        /// </summary>
        /// <param name="p"></param>
        public Q(long p) : this(p, 1) { }
        #endregion

        #region Conversions

        /// <summary>
        /// Conversión de un racional en un double.
        /// </summary>
        public static implicit operator double (Q q)
        {
            return q._p / (double)(q._qMinusOne + 1);
        }

        /// <summary>
        /// Conversión de un racional en un float.
        /// </summary>
        public static implicit operator float(Q q)
        {
            return q._p / (float)(q._qMinusOne + 1);
        }

        /// <summary>
        /// Conversión de un entero en un racional.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Q (long value)
        {
            return new Q(value, 1);
        }

        /// <summary>
        /// Conversión explícita de un racional a un entero. La parte decimal se pierde.
        /// </summary>
        /// <param name="x"></param>
        public static explicit operator long (Q x)
        {
            return (long)(x.Numerator / x.Denominator);
        }

        /// <summary>
        /// Conversión aproximada de un double en un racional.
        /// </summary>
        public static explicit operator Q(double d)
        {
            Q accum = (long)Math.Floor(d);
            d -= Math.Floor(d);
            double toRemoveFromD = 0.5;
            Q toAddToAccum = Q.One / 2;
            for (int i=0; i<56; i++)
            {
                if (d >= toRemoveFromD)
                {
                    d -= toRemoveFromD;
                    accum += toAddToAccum;
                }

                toAddToAccum /= 2;
                toRemoveFromD /= 2;
            }

            long simplifyingWithError = MCD(accum.Numerator, accum.Denominator, 0.0000001);

            return new Q(accum.Numerator / simplifyingWithError, accum.Denominator / simplifyingWithError);
        }

        #endregion

        #region Operators
        /// <summary>
        /// Obtiene el valor positivo de un racional.
        /// </summary>
        public static Q operator + (Q x)
        {
            return x;
        }
        /// <summary>
        /// Obtiene el valor negativo de un racional.
        /// </summary>
        public static Q operator -(Q x)
        {
            return x * -1;
        }
        /// <summary>
        /// Obtiene la suma entre dos racionales.
        /// </summary>
        public static Q operator + (Q x, Q y)
        {
            long mcd = MCD(x.Denominator, y.Denominator);
            long xd = x.Denominator / mcd;
            long yd = y.Denominator / mcd;
            return new Q(x.Numerator * yd + y.Numerator * xd, x.Denominator*yd);
        }
        /// <summary>
        /// Obtiene la diferencia entre dos racionales.
        /// </summary>
        public static Q operator -(Q x, Q y)
        {
            long mcd = MCD(x.Denominator, y.Denominator);
            long xd = x.Denominator / mcd;
            long yd = y.Denominator / mcd;
            return new Q(x.Numerator * yd - y.Numerator * xd, x.Denominator * yd);
        }
        /// <summary>
        /// Obtiene la multiplicación entre dos racionales.
        /// </summary>
        public static Q operator *(Q x, Q y)
        {
            return new Q(x.Numerator * y.Numerator, x.Denominator * y.Denominator);
        }
        /// <summary>
        /// Obtiene la división entre dos racionales.
        /// </summary>
        /// <param name="y">Debe ser diferente de cero.</param>
        public static Q operator /(Q x, Q y)
        {
            return new Q(x.Numerator * y.Denominator, x.Denominator * y.Numerator);
        }
        /// <summary>
        /// Determina cuándo el primer racional es menor estricto que el segundo.
        /// </summary>
        public static bool operator <(Q x, Q y)
        {
            return x.Numerator * y.Denominator < y.Numerator * x.Denominator;
        }
        /// <summary>
        /// Determina cuándo el primer racional es menor o igual que el segundo.
        /// </summary>
        public static bool operator <=(Q x, Q y)
        {
            return x < y || x == y;
        }
        /// <summary>
        /// Determina cuándo el primer racional es mayor estricto que el segundo.
        /// </summary>
        public static bool operator >(Q x, Q y)
        {
            return !(x <= y);
        }
        /// <summary>
        /// Determina cuándo el primer racional es mayor o igual que el segundo.
        /// </summary>
        public static bool operator >=(Q x, Q y)
        {
            return !(x < y);
        }
        /// ==, !=, Equals and GetHashcode can be reused from ValueType

        #endregion

        #region Comparable
        public int CompareTo(Q other)
        {
            return this < other ? -1 : this > other ? 1 : 0;
        }
        #endregion

        #region Object override
        public override string ToString()
        {
            if (Denominator != 1)
                return string.Format("{0}/{1}", Numerator, Denominator);
            else
                return Numerator.ToString();
        }
        #endregion

        #region Formattable
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                return ToString();

            return ((double)this).ToString(format, formatProvider);
        }
        public string ToString(string format)
        {
            return ToString(format, System.Globalization.NumberFormatInfo.CurrentInfo);
        }
        #endregion

        #region Static Functions
        /// <summary>
        /// Obtiene el valor máximo entre dos racionales.
        /// </summary>
        public static Q Max(Q x, Q y)
        {
            return x > y ? x : y;
        }
        /// <summary>
        /// Obtiene el valor mínimo entre dos racionales.
        /// </summary>
        public static Q Min(Q x, Q y)
        {
            return x < y ? x : y;
        }
        /// <summary>
        /// Obtiene el valor absoluto de un racional.
        /// </summary>
        public static Q Abs(Q x)
        {
            return x < 0 ? -x : x;
        }
        /// <summary>
        /// Obtiene el signo de un racional.
        /// </summary>
        public static Q Sign (Q x)
        {
            return Math.Sign(x.Numerator);
        }
        /// <summary>
        /// Obtiene el valor cuadrado de un racional.
        /// </summary>
        public static Q Sqr(Q x)
        {
            return x * x;
        }
        #endregion
    }
}
