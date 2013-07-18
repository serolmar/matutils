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
    class SymmetricPolynomialDegreeEqualityComparer : EqualityComparer<Dictionary<int, int>>
    {
        /// <summary>
        /// Verifica se dois graus são iguais.
        /// </summary>
        /// <param name="x">O primeiro grau a ser verificado.</param>
        /// <param name="y">O segundo grau a ser verificado.</param>
        /// <returns>Verdadeiro se ambos os graus são iguais e falso caso contrário.</returns>
        public override bool Equals(Dictionary<int, int> x, Dictionary<int, int> y)
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
                if (x.Keys.Count == y.Keys.Count)
                {
                    foreach (var kvp in x)
                    {
                        var otherCount = 0;
                        if (y.TryGetValue(kvp.Key, out otherCount))
                        {
                            return kvp.Value == otherCount;
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
        /// Obtém o código descritivo do objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código do objecto.</returns>
        public override int GetHashCode(Dictionary<int, int> obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var result = 19;
                foreach (var degreeCount in obj)
                {
                    result = unchecked(result + degreeCount.Key.GetHashCode() + degreeCount.Value.GetHashCode());
                }

                return result;
            }
        }
    }
}
