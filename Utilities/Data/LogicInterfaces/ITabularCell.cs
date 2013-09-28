namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ITabularCell
    {
        /// <summary>
        /// Obtém o número da linha.
        /// </summary>
        int RowNumber { get; }

        /// <summary>
        /// Obtém o número da coluna.
        /// </summary>
        int ColumnNumber { get; }

        /// <summary>
        /// Obtém o valor que indica se o conteúdo da célula é nulo ou um valor textual vazio.
        /// </summary>
        bool NullOrEmpty { get; }

        /// <summary>
        /// Obtém o tipo de valor do conteúdo da célula.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Atribui um valor à célula.
        /// </summary>
        /// <typeparam name="T">O tipo do valor a ser atribuído.</typeparam>
        /// <param name="value">O valor a ser atribuído.</param>
        void SetCellValue<T>(T value);

        /// <summary>
        /// Obtém o valor da célula.
        /// </summary>
        /// <typeparam name="T">O valor da célula a ser atribuído.</typeparam>
        /// <returns>O valor obtido.</returns>
        T GetCellValue<T>();

        /// <summary>
        /// Obtém o valor da célula como texto.
        /// </summary>
        /// <returns>A representação textual da célula.</returns>
        string GetAsText();
    }
}
