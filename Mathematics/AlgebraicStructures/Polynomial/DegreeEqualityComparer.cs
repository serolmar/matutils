using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Implementa um comparador de graus representados como uma lista de inteiros.
    /// </summary>
    class DegreeEqualityComparer : EqualityComparer<IEnumerable<int>>
    {
        /// <summary>
        /// O número primo usado na obtenção do código confuso.
        /// </summary>
        private const int primeMultiplier = 31;

        /// <summary>
        /// O código confuso de uma colecção de inteiros.
        /// </summary>
        private const int emptyEnumerableHash = 0x2D2816FE;

        /// <summary>
        /// Determina se duas listas de graus são iguais.
        /// </summary>
        /// <param name="x">A primeira lista a ser comparada.</param>
        /// <param name="y">A segunda lista a ser comparada..</param>
        /// <returns>
        /// Verdadeiro se as listas forem iguais e falso caso contrário.
        /// </returns>
        public override bool Equals(IEnumerable<int> x, IEnumerable<int> y)
        {
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var firstEnumerator = x.GetEnumerator();
            var secondEnumerator = y.GetEnumerator();
            var firstState = firstEnumerator.MoveNext();
            var secondState = secondEnumerator.MoveNext();
            while (firstState && secondState)
            {
                if (firstEnumerator.Current != secondEnumerator.Current)
                {
                    return false;
                }

                firstState = firstEnumerator.MoveNext();
                secondState = secondEnumerator.MoveNext();
            }

            // The initial part of degree list was the same and the rest are zero
            if (firstState)
            {
                if (firstEnumerator.Current != 0)
                {
                    return false;
                }

                while (firstEnumerator.MoveNext())
                {
                    if (firstEnumerator.Current != 0)
                    {
                        return false;
                    }
                }
            }

            if (secondState)
            {
                if (secondEnumerator.Current != 0)
                {
                    return false;
                }

                while (secondEnumerator.MoveNext())
                {
                    if (secondEnumerator.Current != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Obtém um código confuso para a instância especificada.
        /// </summary>
        /// <param name="obj">A instância do objecto.</param>
        /// <returns>
        /// Um código confuso para a instância.
        /// </returns>
        public override int GetHashCode(IEnumerable<int> obj)
        {
            if (obj == null)
            {
                return 0;
            }

            var numberOfZeroesFound = 0;

            // The default empty list hash code
            var res = emptyEnumerableHash;
            foreach (var item in obj)
            {
                if (item != 0)
                {
                    for (int i = 0; i < numberOfZeroesFound; ++i)
                    {
                        res *= 31;
                    }

                    res = res * 31 + item;
                    numberOfZeroesFound = 0;
                }
                else
                {
                    ++numberOfZeroesFound;
                }
            }

            return res;
        }
    }
}
