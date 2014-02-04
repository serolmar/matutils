namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um algoritmo que permite determinar o valor da função phi de Euler.
    /// </summary>
    public class EulerTotFuncAlg<NumberType> : IAlgorithm<NumberType, NumberType>
    {
        private IIntegerNumber<NumberType> integerNumber;

        IPrimeNumberIteratorFactory<NumberType> primeNumberFactory;

        private IAlgorithm<NumberType, NumberType> squareNumberAlgorithm;

        public EulerTotFuncAlg(
            IAlgorithm<NumberType, NumberType> squareNumberAlgorithm,
            IPrimeNumberIteratorFactory<NumberType> primeNumberFactory,
            IIntegerNumber<NumberType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (primeNumberFactory == null)
            {
                throw new ArgumentNullException("primeNumberFactory");
            }
            else if (squareNumberAlgorithm == null)
            {
                throw new ArgumentNullException("squareNumberAlgorithm");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.primeNumberFactory = primeNumberFactory;
                this.squareNumberAlgorithm = squareNumberAlgorithm;
            }
        }

        /// <summary>
        /// Determina a quantidade de números primos inferiores ou iguais ao módulo do valor especificado.
        /// </summary>
        /// <param name="data">O valor a ser analisado.</param>
        /// <returns>A quantidade de números primos nestas condições.</returns>
        public NumberType Run(NumberType data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var innerData = this.integerNumber.GetNorm(data);
                if (this.integerNumber.IsAdditiveUnity(innerData))
                {
                    return innerData;
                }
                else
                {
                    foreach (var unit in this.integerNumber.Units)
                    {
                        if (data.Equals(unit))
                        {
                            return this.integerNumber.AdditiveUnity;
                        }
                    }

                    var result = this.integerNumber.MapFrom(1);
                    var sqrt = this.squareNumberAlgorithm.Run(innerData);
                    var primeNumbersIterator = this.primeNumberFactory.CreatePrimeNumberIterator(sqrt);
                    foreach (var prime in primeNumbersIterator)
                    {
                        var computation = this.integerNumber.GetQuotientAndRemainder(innerData, prime);
                        if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(
                                result,
                                this.integerNumber.Predecessor(prime));
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, prime);
                            while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                            {
                                result = this.integerNumber.Multiply(result, prime);
                                innerData = computation.Quotient;
                            }

                            foreach (var unit in this.integerNumber.Units)
                            {
                                if (innerData.Equals(unit))
                                {
                                    return result;
                                }
                            }
                        }
                    }

                    // Neste ponto verifica-se que "innerDta" é um número primo.
                    result = this.integerNumber.Multiply(
                        result,
                        this.integerNumber.Predecessor(innerData));
                    return result;
                }
            }
        }
    }
}
