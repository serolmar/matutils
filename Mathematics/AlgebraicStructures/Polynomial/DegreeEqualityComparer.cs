using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class DegreeEqualityComparer : EqualityComparer<IEnumerable<int>>
    {
        private const int primeMultiplier = 31;

        private const int emptyEnumerableHash = 0x2D2816FE;

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
                if (firstEnumerator.Current != 0)
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
