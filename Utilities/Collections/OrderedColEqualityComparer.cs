namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um comparador de igualdade sobre colecções de elementos numa ordema arbitrária.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os elmentos das colecções.</typeparam>
    public class OrderedColEqualityComparer<CoeffType> : EqualityComparer<IEnumerable<CoeffType>>
    {
        private IEqualityComparer<CoeffType> coeffComparer;

        public OrderedColEqualityComparer()
        {
            this.coeffComparer = EqualityComparer<CoeffType>.Default;
        }

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
}
