namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UnivarPolynomRingFactory<CoeffType> 
        : IUnivarPolRingFactory<CoeffType>
    {
        /// <summary>
        /// Cria uma instância de um anel polinomial.
        /// </summary>
        /// <param name="variableName">O nome da variável associada ao anel.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O anel polinomial.</returns>
        public UnivarPolynomRing<CoeffType> CreateInstance(
            string variableName, 
            IRing<CoeffType> ring)
        {
            return new UnivarPolynomRing<CoeffType>(variableName, ring);
        }
    }
}
