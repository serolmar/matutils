// -----------------------------------------------------------------------
// <copyright file="CollectionEqualityComparers.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um comparador de igualdade sobre colecções de elementos tendo em conta a ordem.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os elmentos das colecções.</typeparam>
    public class OrderedColEqualityComparer<CoeffType> : EqualityComparer<IEnumerable<CoeffType>>
    {
        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IEqualityComparer<CoeffType> coeffComparer;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="OrderedColEqualityComparer{CoeffType}"/>.
        /// </summary>
        public OrderedColEqualityComparer()
        {
            this.coeffComparer = EqualityComparer<CoeffType>.Default;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="OrderedColEqualityComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffComparer">O comparador de coeficientes.</param>
        public OrderedColEqualityComparer(IEqualityComparer<CoeffType> coeffComparer)
        {
            if (coeffComparer == null)
            {
                this.coeffComparer = EqualityComparer<CoeffType>.Default;
            }
            else
            {
                this.coeffComparer = coeffComparer;
            }
        }

        /// <summary>
        /// Determina se dois objectos são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>
        /// Verdadeiro se os objetos forem iguais e falso caso contrário.
        /// </returns>
        public override bool Equals(IEnumerable<CoeffType> x, IEnumerable<CoeffType> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else
            {
                var xEnumerator = x.GetEnumerator();
                var yEnumerator = y.GetEnumerator();
                var xState = xEnumerator.MoveNext();
                var yState = yEnumerator.MoveNext();
                while (xState && yState)
                {
                    if (!this.coeffComparer.Equals(xEnumerator.Current, yEnumerator.Current))
                    {
                        return false;
                    }
                    else
                    {
                        xState = xEnumerator.MoveNext();
                        yState = yEnumerator.MoveNext();
                    }
                }

                if (xState || yState)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Retorna um código confuso para o objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// Um código confuso para o objecto utilizado em alguns algoritmos.
        /// </returns>
        public override int GetHashCode(IEnumerable<CoeffType> obj)
        {
            if (obj == null)
            {
                return typeof(IEnumerable<CoeffType>).GetHashCode();
            }
            else
            {
                var res = 19;
                foreach (var item in obj)
                {
                    if (item == null)
                    {
                        res = res * 31;
                    }
                    else
                    {
                        res = res * 31 + item.GetHashCode();
                    }
                }

                return res;
            }
        }
    }

    /// <summary>
    /// Implementa um comparador de igualdade sobre colecções de elementos numa ordema arbitrária.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os elmentos das colecções.</typeparam>
    public class UnorderedColEqualityComparer<CoeffType> : EqualityComparer<IEnumerable<CoeffType>>
    {
        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IEqualityComparer<CoeffType> coeffComparer;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="UnorderedColEqualityComparer{CoeffType}"/>.
        /// </summary>
        public UnorderedColEqualityComparer()
        {
            this.coeffComparer = EqualityComparer<CoeffType>.Default;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="UnorderedColEqualityComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffComparer">O comparador de coeficientes.</param>
        public UnorderedColEqualityComparer(IEqualityComparer<CoeffType> coeffComparer)
        {
            if (coeffComparer == null)
            {
                this.coeffComparer = EqualityComparer<CoeffType>.Default;
            }
            else
            {
                this.coeffComparer = coeffComparer;
            }
        }

        /// <summary>
        /// Determina se dois objectos são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>
        /// Verdadeiro se os objetos forem iguais e falso caso contrário.
        /// </returns>
        public override bool Equals(IEnumerable<CoeffType> x, IEnumerable<CoeffType> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else
            {
                var xCounts = this.CountItems(x);
                var yCounts = this.CountItems(y);
                if (xCounts.Count == yCounts.Count)
                {
                    foreach (var xCount in xCounts)
                    {
                        var existing = default(int);
                        if (yCounts.TryGetValue(xCount.Key, out existing))
                        {
                            if (existing != xCount.Value)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Retorna um código confuso para o objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// Um código confuso para o objecto utilizado em alguns algoritmos.
        /// </returns>
        public override int GetHashCode(IEnumerable<CoeffType> obj)
        {
            if (obj == null)
            {
                return typeof(IEnumerable<CoeffType>).GetHashCode();
            }
            else
            {
                var result = 17;
                var countItems = this.CountItems(obj);
                foreach (var countKvp in countItems)
                {
                    result ^= (countKvp.Value * countKvp.Key.GetHashCode());
                }

                return result;
            }
        }

        /// <summary>
        /// Conta os itens existentes na colecção.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <returns>A contagem dos itens.</returns>
        private Dictionary<CoeffType, int> CountItems(IEnumerable<CoeffType> collection)
        {
            var result = new Dictionary<CoeffType, int>(this.coeffComparer);
            foreach (var item in collection)
            {
                var existing = default(int);
                if (result.TryGetValue(item, out existing))
                {
                    ++existing;
                    result[item] = existing;
                }
                else
                {
                    result.Add(item, 1);
                }
            }

            return result;
        }
    }
}
