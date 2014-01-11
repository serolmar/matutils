namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite calcular o produto de um coeficiente por um vector.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    /// <typeparam name="CoeffVectorType">O tipo de coeficientes no vector.</typeparam>
    public class CoeffVectorMultiplicationOperation<CoeffType, CoeffVectorType>
        : IMultiplicationOperation<CoeffType, IVector<CoeffVectorType>, IVector<CoeffVectorType>>
    {
        private IVectorFactory<CoeffVectorType> vectorFactory;

        private IMultiplicationOperation<CoeffType, CoeffVectorType, CoeffVectorType> coeffsMultOperation;

        /// <summary>
        /// Cria a instância de um objecto capaz de multiplicar um coeficiente por um vector.
        /// </summary>
        /// <param name="coeffsMultOperation">A operação de multiplicação entre os coeficientes.</param>
        /// <param name="vectorFactory">Uma fábrica que permita criar instâncias de vectores.</param>
        public CoeffVectorMultiplicationOperation(
            IMultiplicationOperation<CoeffType, CoeffVectorType, CoeffVectorType> coeffsMultOperation,
            IVectorFactory<CoeffVectorType> vectorFactory)
        {
            if (vectorFactory == null)
            {
                throw new ArgumentNullException("vectorFactory");
            }
            else if (coeffsMultOperation == null)
            {
                throw new ArgumentNullException("coeffsMultOperation");
            }
            else
            {
                this.vectorFactory = vectorFactory;
                this.coeffsMultOperation = coeffsMultOperation;
            }
        }

        public IVector<CoeffVectorType> Multiply(CoeffType left, IVector<CoeffVectorType> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var resultVector = this.vectorFactory.CreateVector(right.Length);
                for (int i = 0; i < right.Length; ++i)
                {
                    resultVector[i] = this.coeffsMultOperation.Multiply(left, right[i]);
                }

                return resultVector;
            }
        }
    }
}
