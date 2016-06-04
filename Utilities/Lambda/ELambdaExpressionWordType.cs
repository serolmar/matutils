namespace Utilities.Lambda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Identifica os tiops de palavras utilizadas pelo construtor de expressões.
    /// </summary>
    public enum ELambdaExpressionWordType
    {
        /// <summary>
        /// Valor numérico.
        /// </summary>
        NUMERIC = 0,

        /// <summary>
        /// Valor alfabético.
        /// </summary>
        ALPHA = 1,

        /// <summary>
        /// Espaço.
        /// </summary>
        SPACE = 2,

        /// <summary>
        /// Parêntesis de abertura.
        /// </summary>
        OPEN_PARENTHESIS = 3,

        /// <summary>
        /// Parêntesis de fecho.
        /// </summary>
        CLOSE_PARENTHESIS = 4,

        /// <summary>
        /// Delimitador de abertura.
        /// </summary>
        OPEN_DELIMITER = 5,

        /// <summary>
        /// Delimitador de fecho.
        /// </summary>
        CLOSE_DELIMITER = 6,

        /// <summary>
        /// A vírgula.
        /// </summary>
        COMMA = 7,

        /// <summary>
        /// Outro tipo de elementos.
        /// </summary>
        OTHER = 8,

        /// <summary>
        /// O final de ficheiro.
        /// </summary>
        EOF = 9
    }
}
