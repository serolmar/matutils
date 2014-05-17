namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Permite determinar se um número corresponde a uma potência perfeita.
    /// </summary>
    public class BigIntPerfectPowerTestAlg : IAlgorithm<BigInteger, bool>
    {
        /// <summary>
        /// A fábrica que permite criar instâncias de iteradores para os números primos.
        /// </summary>
        private IPrimeNumberIteratorFactory<BigInteger> primeNumbersIteratorFactory;

        /// <summary>
        /// O algoritmo responsável pela determinação da raiz de índice n de um número.
        /// </summary>
        IAlgorithm<BigInteger, BigInteger, BigInteger> nthRootAlgorithm;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BigIntPerfectPowerTestAlg"/>.
        /// </summary>
        /// <param name="nthRootAlgorithm">
        /// O algoritmo que permite calcular a parte inteira de uma raiz de ordem n.
        /// </param>
        /// <param name="primeNumbersIteratorFactory">
        /// A fábrica que permite criar enumeradores para números primos.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        public BigIntPerfectPowerTestAlg(
            IAlgorithm<BigInteger, BigInteger, BigInteger> nthRootAlgorithm,
            IPrimeNumberIteratorFactory<BigInteger> primeNumbersIteratorFactory)
        {
            if (primeNumbersIteratorFactory == null)
            {
                throw new ArgumentNullException("primeNumbersIteratorFactory");
            }
            else if (nthRootAlgorithm == null)
            {
                throw new ArgumentNullException("nthRootAlgorithm");
            }
            else
            {
                this.primeNumbersIteratorFactory = primeNumbersIteratorFactory;
                this.nthRootAlgorithm = nthRootAlgorithm;
            }
        }

        /// <summary>
        /// Determina se o número corresponde a uma potência perfeita.
        /// </summary>
        /// <param name="data">O número a ser testado.</param>
        /// <returns>Verdadeiro caso o número seja uma potência perfeita e falso caso contrário.</returns>
        public bool Run(BigInteger data)
        {
            var innerData = BigInteger.Abs(data);

            if (innerData == 0)
            {
                return true;
            }
            else if (innerData == 1)
            {
                return true;
            }
            else
            {
                var limitValue = (int)Math.Floor(BigInteger.Log(innerData) / BigInteger.Log(2));
                var primeNumbersIterator = this.primeNumbersIteratorFactory.CreatePrimeNumberIterator(
                    limitValue + 1);
                foreach (var prime in primeNumbersIterator)
                {
                    var root = this.nthRootAlgorithm.Run(prime, innerData);
                    var power = BigInteger.Pow(root, (int)prime);
                    if (power == innerData)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
