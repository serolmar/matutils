namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um provedor de leitores.
    /// </summary>
    /// <typeparam name="ReaderType">O tipo de objectos que constituem os leitores providenciados.</typeparam>
    public interface IDataReaderProvider<ReaderType>
    {
        /// <summary>
        /// Tenta obter o leitor de dados para a célula especificada.
        /// </summary>
        /// <param name="rowNumber">O número da célula.</param>
        /// <param name="columnNumber">O número da coluna.</param>
        /// <param name="reader">O leitor.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        bool TryGetDataReader(
            int rowNumber, 
            int columnNumber, 
            out ReaderType reader);
    }
}
