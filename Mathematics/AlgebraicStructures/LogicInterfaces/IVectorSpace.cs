using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Implementa a multiplicação entre um escalar e um elemento de um espaço vectorial.
    /// </summary>
    /// <remarks>
    /// <typeparam name="CoefficientType">O tipo de coeficientes.</typeparam>
    /// <typeparam name="VectorSpaceType">O tipo do vector.</typeparam>
    public interface IVectorSpace<CoefficientType, VectorSpaceType> : IGroup<VectorSpaceType>
    {
        /// <summary>
        /// Obtém o corpo sobre o qual funciona o espaço vectorial.
        /// </summary>
        /// <value>
        /// O corpo sobre o qual funciona o espaço vectorial.
        /// </value>
        IField<CoefficientType> Field { get; }

        /// <summary>
        /// Define a multiplicação do anel ou campo com o elemento do espaço vectorial.
        /// </summary>
        /// <param name="coefficientElement">O coeficiente.</param>
        /// <param name="vectorSpaceElement">O vector.</param>
        /// <returns>O vector resultante.</returns>
        VectorSpaceType MultiplyScalar(CoefficientType coefficientElement, VectorSpaceType vectorSpaceElement);
    }
}
