namespace Utilities.ExpressionBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Identifica o tipo de carácteres que constituem uma palavra.
    /// </summary>
    public enum EWordCharExpressionType
    {
        /// <summary>
        /// Um carácter arbitrário.
        /// </summary>
        ANY,

        /// <summary>
        /// Um espaço.
        /// </summary>
        SPACE,

        /// <summary>
        /// Um carácter de controlo do cabeço na
        /// mudança de linha.
        /// </summary>
        CARRIAGE_RETURN,

        /// <summary>
        /// Carácter de mudança de linha.
        /// </summary>
        NEW_LINE
    }
}
