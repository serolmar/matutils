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
    /// <typeparam name="GroupCoeffType">O tipo de valores do grupo.</typeparam>
    public interface ILLLBasisReductionAlgorithm<VectorType, FieldCoeffType, GroupCoeffType>
        : IAlgorithm<VectorType[], FieldCoeffType, VectorType[]>
    {
        /// <summary>
        /// O espaço vectorial responsável pelas operações sobre o vector.
        /// </summary>
        IVectorSpace<FieldCoeffType, VectorType> FieldVectorSpace { get; }

        /// <summary>
        /// O objecto responsável pelas multiplicações dos valores aproximados pelo vector.
        /// </summary>
        IMultiplicationOperation<GroupCoeffType, VectorType, VectorType> GroupVectorMultOperation { get; }

        /// <summary>
        /// Obtém objecto reponsável pela multiplicação entre elementos do grupo e do corpo.
        /// </summary>
        IMultiplicationOperation<GroupCoeffType, FieldCoeffType, FieldCoeffType> GroupFieldMultOperation { get; }

        /// <summary>
        /// Obtém a melhor aproximação.
        /// </summary>
        INearest<FieldCoeffType, GroupCoeffType> Nearest { get; }
    }
}
