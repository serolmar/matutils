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
        NUMERIC,

        /// <summary>
        /// Valor alfabético.
        /// </summary>
        ALPHA,

        /// <summary>
        /// Espaço.
        /// </summary>
        SPACE,

        /// <summary>
        /// Parêntesis de abertura.
        /// </summary>
        OPEN_PARENTHESIS,

        /// <summary>
        /// Parêntesis de fecho.
        /// </summary>
        CLOSE_PARENTHESIS,

        /// <summary>
        /// Delimitador de abertura.
        /// </summary>
        OPEN_DELIMITER,

        /// <summary>
        /// Delimitador de fecho.
        /// </summary>
        CLOSE_DELIMITER,

        /// <summary>
        /// A vírgula.
        /// </summary>
        COMMA,

        /// <summary>
        /// Outro tipo de elementos.
        /// </summary>
        OTHER,

        /// <summary>
        /// O final de ficheiro.
        /// </summary>
        EOF
    }
}
