namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IDataReaderProvider<ConversionType>
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
            out IDataReader<ConversionType> reader);
    }
}
