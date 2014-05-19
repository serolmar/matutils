namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define os vários tipos de operadores.
    /// </summary>
    enum EOperatorType
    {
        /// <summary>
        /// A binary operator.
        /// </summary>
        BINARY,

        /// <summary>
        /// An unary operator.
        /// </summary>
        UNARY,

        /// <summary>
        /// An internal delimiter operator.
        /// </summary>
        INTERNAL_DELIMITER,

        /// <summary>
        /// An external delimiter operator.
        /// </summary>
        EXTERNAL_DELIMITER,

        /// <summary>
        /// A sequence delimiter operator.
        /// </summary>
        SEQUENCE_DELIMITER
    }
}
