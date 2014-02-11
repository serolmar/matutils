namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um conjunto de objectos que permitem criar instâncias de domínios polinomiais.
    /// </summary>
    public interface IUnivarPolDomainFactory<CoeffType>
    {
        /// <summary>
        /// Obtém a instância do domínio polinomial.
        /// </summary>
        /// <param name="variableName">A variável associada ao domínio.</param>
        /// <param name="field">O corpo que define as operações sobre os coeficientes.</param>
        /// <returns>O domínio polinomial.</returns>
        UnivarPolynomEuclideanDomain<CoeffType> CreateInstance(
            string variableName, 
            IField<CoeffType> field);
    }
}
