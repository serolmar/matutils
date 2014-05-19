namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma célula.
    /// </summary>
    public interface ITabularCell
    {
        /// <summary>
        /// Obtém o número da linha.
        /// </summary>
        /// <value>O número da linha.</value>
        int RowNumber { get; }

        /// <summary>
        /// Obtém o número da coluna.
        /// </summary>
        /// <value>O número da coluna.</value>
        int ColumnNumber { get; }

        /// <summary>
        /// Obtém o valor que indica se o conteúdo da célula é nulo ou um valor textual vazio.
        /// </summary>
        /// <value>Verdadeiro caso a célula seja vazia e falso caso contrário.</value>
        bool NullOrEmpty { get; }

        /// <summary>
        /// Obtém o tipo de valor do conteúdo da célula.
        /// </summary>
        /// <value>O tipo do valor.</value>
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
