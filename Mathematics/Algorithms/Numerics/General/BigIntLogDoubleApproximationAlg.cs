namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Permite calcular o logaritmo natural de um número inteiro com precisão dupla.
    /// </summary>
    public class BigIntLogDoubleApproximationAlg : IAlgorithm<BigInteger, double>
    {
        /// <summary>
        /// Permite calcular o logaritmo natural do número especificado.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <returns>O logaritmo.</returns>
        public double Run(BigInteger data)
        {
            return BigInteger.Log(data);
        }
    }
}
