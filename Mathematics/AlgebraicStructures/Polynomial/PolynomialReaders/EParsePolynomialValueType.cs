using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Enumera os valores possíveis para uma leitura polinomial.
    /// </summary>
    public enum EParsePolynomialValueType
    {
        /// <summary>
        /// O valor é do tipo polinomial.
        /// </summary>
        POLYNOMIAL,

        /// <summary>
        /// O valor é do tipo inteiro.
        /// </summary>
        INTEGER,

        /// <summary>
        /// O coeficiente.
        /// </summary>
        COEFFICIENT
    }
}
