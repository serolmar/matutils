namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumera todos os tipos de escopo.
    /// </summary>
    enum EScopeType
    {
        /// <summary>
        /// Princial.
        /// </summary>
        MAIN,

        /// <summary>
        /// Interno.
        /// </summary>
        INNER,

        /// <summary>
        /// Ciclo "enquanto".
        /// </summary>
        WHILE,

        /// <summary>
        /// Decisão.
        /// </summary>
        IF_ELSE,

        /// <summary>
        /// Ciclo "para".
        /// </summary>
        FOR
    }
}
