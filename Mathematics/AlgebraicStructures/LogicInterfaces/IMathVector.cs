namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Define as propriedades e método essenciais a um vector.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas do vector.</typeparam>
    public interface IMathVector<CoeffType> : IVector<CoeffType>
    {
        /// <summary>
        /// Averigua se se trata de um vector nulo.
        /// </summary>
        /// <param name="monoid">O monóide responsável pela identificação do zero.</param>
        /// <returns>Veradeiro caso o vector seja nulo e falso caso contrário.</returns>
        bool IsNull(IMonoid<CoeffType> monoid);
    }

    /// <summary>
    /// Define as propriedades e métodos essenciais a um vector esparso.
    /// </summary>
    /// <typeparam name="CoeffType">
    /// O tipo de objectos que constituem as entradas dos vectores.
    /// </typeparam>
    public interface ISparseMathVector<CoeffType>
        : ISparseVector<CoeffType>, IMathVector<CoeffType>
    {
    }
}
