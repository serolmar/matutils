namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UnivarPolEuclideanDomainFactory<CoeffType>
        : IUnivarPolDomainFactory<CoeffType>
    {
        /// <summary>
        /// Permite criar uma instância do domínio polinomial.
        /// </summary>
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
