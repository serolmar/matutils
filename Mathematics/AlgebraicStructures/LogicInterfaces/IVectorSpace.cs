using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IVectorSpace<CoefficientType, VectorSpaceType> : IGroup<VectorSpaceType>
    {
        /// <summary>
        /// Define a multiplicação do anel ou campo com o elemento do espaço vectorial.
        /// </summary>
        /// <param name="coefficientElement">O coeficiente.</param>
        /// <param name="vectorSpaceElement">O vector.</param>
        /// <returns>O vector resultante.</returns>
        VectorSpaceType MultiplyScalar(CoefficientType coefficientElement, VectorSpaceType vectorSpaceElement);
    }
}
