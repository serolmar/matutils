namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um algoritmo de redução LLL.
    /// </summary>
    /// <typeparam name="VectorType">O tipo de vectores.</typeparam>
    /// <typeparam name="FieldCoeffType">O tipo de valores do corpo.</typeparam>
    public interface ILLLBasisReductionAlgorithm<VectorType, FieldCoeffType>
        : IAlgorithm<VectorType[], FieldCoeffType, VectorType[]>
    {
        /// <summary>
        /// O espaço vectorial responsável pelas operações sobre o vector.
        /// </summary>
        IVectorSpace<FieldCoeffType, VectorType> FieldVectorSpace { get; }

        /// <summary>
        /// Obtém a melhor aproximação.
        /// </summary>
        INearest<FieldCoeffType, FieldCoeffType> Nearest { get; }

        /// <summary>
        /// Obtém o objecto responsável pela determinação do produto escalar entre dois vectores.
        /// </summary>
        IScalarProductSpace<VectorType, FieldCoeffType> ScalarProduct { get; }
    }
}
