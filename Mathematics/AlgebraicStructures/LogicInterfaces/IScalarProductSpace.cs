namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um espaço onde está definido um produto escalar.
    /// </summary>
    /// <typeparam name="VectorType">O tipo de objecto que representa um vector.</typeparam>
    /// <typeparam name="CoeffType">O tipo de objecto que representa um coeficiente.</typeparam>
    public interface IScalarProductSpace<in VectorType, CoeffType> : IComparer<CoeffType>
    {
        /// <summary>
        /// Multiplica escalarmente dois vectores.
        /// </summary>
        /// <param name="left">O primeiro vector.</param>
        /// <param name="right">O segundo vector.</param>
        /// <returns>O valor do produto escalar.</returns>
        CoeffType Multiply(VectorType left, VectorType right);
    }
}
