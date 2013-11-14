namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um espaço onde está definido um produto escalar.
    /// </summary>
    public interface IScalarProductSpace<CoeffType, VectorType>
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
