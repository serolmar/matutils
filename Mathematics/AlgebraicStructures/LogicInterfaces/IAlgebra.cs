using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Repesenta uma álgebra.
    /// </summary>
    /// <remarks>
    /// Uma álgebra resume-se a um espaço vectorial no qual se encontra definida
    /// uma operação de multiplicação entre dois dos seus vectores. Entre os vectores
    /// existe um que não influencia o produto.
    /// </remarks>
    /// <typeparam name="CoeffType">O tipo do coeficiente.</typeparam>
    /// <typeparam name="VectorSpaceType">O tipo do espaço vectorial.</typeparam>
    public interface IAlgebra<CoeffType, VectorSpaceType> : 
        IVectorSpace<CoeffType, VectorSpaceType>,
        IMultiplication<VectorSpaceType>
    {
    }
}
