namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Define uma linha geral.
    /// </summary>
    /// <typeparam name="L">O tipo de objectos que constituem as células.</typeparam>
    public interface IGeneralTabularRow<L>
        : IIndexed<int, L>, IEnumerable<L>
        where L : IGeneralTabularCell
    {
        /// <summary>
        /// Obtém o número da linha.
        /// </summary>
        int RowNumber { get; }
    }

    /// <summary>
    /// Define a linha de uma tabela só de leitura.
    /// </summary>
    public interface IReadonlyTabularRow
        : IGeneralTabularRow<IReadonlyTabularCell>
    {
    }

    /// <summary>
    /// Define uma linha de tabela.
    /// </summary>
    public interface ITabularRow
        : IGeneralTabularRow<ITabularCell>
    {
    }
}
