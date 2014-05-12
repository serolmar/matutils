namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar instâncias de domínios sobre polinómios.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos coeficientes dos polinómios.</typeparam>
    public class UnivarPolEuclideanDomainFactory<CoeffType>
        : IUnivarPolDomainFactory<CoeffType>
    {
        /// <summary>
        /// Permite criar uma instância do domínio polinomial.
        /// </summary>
        /// <remarks>
        /// Apesar de ser possível criar instâncias dos vários tipos de objectos que permitem realizar
        /// aritmética modular, alguns algoritmos poderão necessitar criá-los internamente.
        /// </remarks>
        /// <param name="variableName">O nome da variável associada ao domínio.</param>
        /// <param name="field">O corpo responsável pelas operações sobre os coeificientes.</param>
        /// <returns>O domínio polinomial.</returns>
        public UnivarPolynomEuclideanDomain<CoeffType> CreateInstance(
            string variableName, 
            IField<CoeffType> field)
        {
            return new UnivarPolynomEuclideanDomain<CoeffType>(variableName, field);
        }
    }
}
