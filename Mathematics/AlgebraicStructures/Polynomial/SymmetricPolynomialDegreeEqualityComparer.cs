using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures.Polynomial
{
    /// <summary>
    /// Permite comparar o grau do polinómio simétrico assumindo que se trata de uma lista
    /// previamente ordenada de forma decrescente.
    /// </summary>
    class SymmetricPolynomialDegreeEqualityComparer : EqualityComparer<List<int>>
    {

        public override bool Equals(List<int> x, List<int> y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null)
            {
                return false;
            }
            else if (y == null)
            {
                return false;
            }
            else
            {
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
                    else
                    {
                        firstState = firstEnumerator.MoveNext();
                        secondState = secondEnumerator.MoveNext();
                    }
                }

                if (firstEnumerator.MoveNext())
                {
                    if (firstEnumerator.Current != 0)
                    {
                        return false;
                    }
                    else
                    {
                        while (firstEnumerator.MoveNext())
                        {
                            if (firstEnumerator.Current != 0)
                            {
                                return false;
                            }
                        }
                    }
                }

                if (secondEnumerator.MoveNext())
                {
                    if (secondEnumerator.Current != 0)
                    {
                        return false;
                    }
                    else
                    {
                        while (secondEnumerator.MoveNext())
                        {
                            if (secondEnumerator.Current != 0)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
        }

        public override int GetHashCode(List<int> obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var result = 19;
                foreach (var degree in obj)
                {
                    result = unchecked(result + degree.GetHashCode());
                }

                return result;
            }
        }
    }
}
