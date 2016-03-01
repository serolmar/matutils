namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um vector cujo contentor consiste num vector do sistema.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas do vector.</typeparam>
    public class ArrayMathVector<CoeffType>
        : ArrayVector<CoeffType>, IMathVector<CoeffType>
    {
        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMathVector{CoeffType}"/>.
        /// </summary>
        /// <param name="length">O comprimento do vector.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o comprimento for negativo.</exception>
        public ArrayMathVector(int length)
            : base(length) { }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMathVector{CoeffType}"/>.
        /// </summary>
        /// <param name="length">O comprimento do vector.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o comprimento for negativo.</exception>
        public ArrayMathVector(int length, CoeffType defaultValue)
            : base(length, defaultValue) { }

        /// <summary>
        /// Permite criar um vector com base num conjunto de valores pré-definidos.
        /// </summary>
        /// <param name="elements">O conjunto de valores pré-definidos.</param>
        public ArrayMathVector(CoeffType[] elements)
            : base(elements) { }

        private ArrayMathVector()
        {
        }

        /// <summary>
        /// Determina se o vector actual é zero relativamente a um monóide.
        /// </summary>
        /// <param name="monoid">O monóide.</param>
        /// <returns>Verdadeiro caso o vector seja nulo e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o monóide fornecido for nulo.</exception>
        public bool IsNull(IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                for (int i = 0; i < this.vectorEntries.Length; ++i)
                {
                    if (!monoid.IsAdditiveUnity(this.vectorEntries[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Constrói uma cópida do vector actual.
        /// </summary>
        /// <returns>A cópia do vector actual.</returns>
        public override IVector<CoeffType> Clone()
        {
            var result = new ArrayMathVector<CoeffType>();
            result.vectorEntries = new CoeffType[this.vectorEntries.Length];
            Array.Copy(this.vectorEntries, result.vectorEntries, this.vectorEntries.Length);
            return result;
        }

        /// <summary>
        /// Obtém uma representação textual do vector.
        /// </summary>
        /// <returns>A representação textual do vector.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            if (this.vectorEntries.Length > 0)
            {
                resultBuilder.Append(this.vectorEntries[0]);
                for (int i = 1; i < this.vectorEntries.Length; ++i)
                {
                    resultBuilder.Append(",");
                    resultBuilder.Append(this.vectorEntries[i]);
                }
            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Obtém o enumerador não genérico para os elementos do vector.
        /// </summary>
        /// <returns>O enumerador não genérico para os elementos do vector.</returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
