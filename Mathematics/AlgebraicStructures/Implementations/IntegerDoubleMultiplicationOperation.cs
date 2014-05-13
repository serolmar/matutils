namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define a multiplicação entre um número decimal de precisão dupla e um inteiro.
    /// </summary>
    public class IntegerDoubleMultiplicationOperation : IMultiplicationOperation<int, double, double>
    {
        /// <summary>
        /// Multiplica um número inteiro com um número decimal de precisão dupla.
        /// </summary>
        /// <param name="left">O número inteiro..</param>
        /// <param name="right">O número decimal de precisão dupla.</param>
        /// <returns>O resultado da multiplicação.</returns>
        public double Multiply(int left, double right)
        {
            return left * right;
        }
    }
}
