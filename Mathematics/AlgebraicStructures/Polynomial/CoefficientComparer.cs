using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Implementa um comparador de temos de polinómios.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficientes dos polinómios.</typeparam>
    internal class CoefficientComparer<CoeffType> : Comparer<CoefficientDegreePair<CoeffType>>
    {
        /// <summary>
        /// O comparador de graus.
        /// </summary>
        private IComparer<int> degreeComparer;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CoefficientComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="degreeComparer">O comparador de graus.</param>
        public CoefficientComparer(IComparer<int> degreeComparer)
        {
            this.degreeComparer = degreeComparer;
        }

        /// <summary>
        /// Obtém o comparador de graus.
        /// </summary>
        /// <value>
        /// O comparador de graus.
        /// </value>
        public IComparer<int> DegreeComparer
        {
            get
            {
                return this.degreeComparer;
            }
        }

        /// <summary>
        /// Compara dois termos polinomiais com base no grau.
        /// </summary>
        /// <param name="x">O primeiro termo a ser comparado.</param>
        /// <param name="y">O segundo termo a ser comparado.</param>
        /// <returns>
        /// O valor -1 caso o primeiro termo seja inferior ao segundo, 0 caso sejam iguais e 1 caso o primeiro
        /// termos seja superior ao segundo.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Se pelo menos um dos arugmentos for nulo.
        /// </exception>
        public override int Compare(
            CoefficientDegreePair<CoeffType> x, 
            CoefficientDegreePair<CoeffType> y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            else if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            else
            {
                return this.degreeComparer.Compare(x.Degree, y.Degree);
            }
        }
    }
}
