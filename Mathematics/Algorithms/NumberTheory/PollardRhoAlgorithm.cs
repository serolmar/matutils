namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo rho de Pollard para factorizar um número.
    /// </summary>
    public class PollardRhoAlgorithm : 
        IAlgorithm<int, Tuple<int, int>>
    {
        private List<UnivariatePolynomialNormalForm<int, ModularIntegerField>> polynomialsList;

        public PollardRhoAlgorithm()
        {
            this.polynomialsList = new List<UnivariatePolynomialNormalForm<int, ModularIntegerField>>();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém a decomposição do número especificado num produto de factores.
        /// </summary>
        /// <remarks>
        /// A decomposição poderá consistir no produto do número especificado pela unidade. Neste caso,
        /// não é garantida a primalidade do número. Normalmente, é escolhido outro polinómio como gerador da
        /// sequência pseudo-aleatória.
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        public Tuple<int, int> Run(int data)
        {
            throw new NotImplementedException();
        }
    }
}
