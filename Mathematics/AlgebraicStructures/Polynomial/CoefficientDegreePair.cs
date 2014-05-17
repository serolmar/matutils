using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Repreesenta um par de coeficiente e grau.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os coeficientes.</typeparam>
    public class CoefficientDegreePair<CoeffType>
    {
        /// <summary>
        /// O grau.
        /// </summary>
        private int degree;

        /// <summary>
        /// O coeficiente.
        /// </summary>
        private CoeffType coeff;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CoefficientDegreePair{CoeffType}"/>.
        /// </summary>
        /// <param name="degree">O grau.</param>
        /// <param name="coeff">O coeficiente.</param>
        public CoefficientDegreePair(int degree, CoeffType coeff)
        {
            this.degree = degree;
            this.coeff = coeff;
        }

        /// <summary>
        /// Obtém o grau.
        /// </summary>
        /// <value>
        /// O grau.
        /// </value>
        public int Degree
        {
            get
            {
                return this.degree;
            }
        }

        /// <summary>
        /// Obtém o coeficiente.
        /// </summary>
        /// <value>
        /// O coeficiente.
        /// </value>
        public CoeffType Coeff
        {
            get
            {
                return this.coeff;
            }
        }
    }
}
