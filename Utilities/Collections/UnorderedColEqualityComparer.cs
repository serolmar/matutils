namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UnorderedColEqualityComparer<CoeffType> : EqualityComparer<IEnumerable<CoeffType>>
    {
        private IEqualityComparer<CoeffType> coeffComparer;

        public UnorderedColEqualityComparer()
        {
            this.coeffComparer = EqualityComparer<CoeffType>.Default;
        }

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
