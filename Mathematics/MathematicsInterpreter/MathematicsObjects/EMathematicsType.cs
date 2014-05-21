namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumera todos os tipos nativos ao interpretador.
    /// </summary>
    public enum EMathematicsType
    {
        /// <summary>
        /// Atribuição.
        /// </summary>
        ASSIGN,

        /// <summary>
        /// Condição.
        /// </summary>
        CONDITION,

        /// <summary>
        /// Nome.
        /// </summary>
        NAME,

        /// <summary>
        /// Valor inteiro.
        /// </summary>
        INTEGER_VALUE,

        /// <summary>
        /// Valor de precisão dupla.
        /// </summary>
        DOUBLE_VALUE,

        /// <summary>
        /// Valor lógico.
        /// </summary>
        BOOLEAN_VALUE,

        /// <summary>
        /// Valor extual.
        /// </summary>
        STRING_VALUE,

        /// <summary>
        /// Polinómio.
        /// </summary>
        POLYNOMIAL,

        /// <summary>
        /// Alcance.
        /// </summary>
        RANGE,

        /// <summary>
        /// Lista.
        /// </summary>
        LIST,

        /// <summary>
        /// Conjunto.
        /// </summary>
        SET,

        /// <summary>
        /// Vazio.
        /// </summary>
        EMPTY
    }
}
