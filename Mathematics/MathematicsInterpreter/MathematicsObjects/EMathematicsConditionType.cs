namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumeara os tipos de condição possíveis.
    /// </summary>
    public enum EMathematicsConditionType
    {
        /// <summary>
        /// Igual a...
        /// </summary>
        EQUAL,

        /// <summary>
        /// Maior do que...
        /// </summary>
        GREAT_THAN,

        /// <summary>
        /// Maior ou igual a...
        /// </summary>
        GREAT_OR_EQUAL_THAN,

        /// <summary>
        /// Menor do que...
        /// </summary>
        LESS_THAN,

        /// <summary>
        /// Menor ou igual a...
        /// </summary>
        LESS_OR_EQUAL_THAN,

        /// <summary>
        /// Valor lógico.
        /// </summary>
        BOOLEAN_VALUE
    }
}
