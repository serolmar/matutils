namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um algoritmo que permite determinar o valor da função phi de Euler.
    /// </summary>
    /// <typeparam name="NumberType">O tipo de objectos que constituem os números.</typeparam>
    public class EulerTotFuncAlg<NumberType> : IAlgorithm<NumberType, NumberType>
    {
        /// <summary>
        /// Mantém o objecto responsável pelas operações sobre números inteiros.
        /// </summary>
        private IIntegerNumber<NumberType> integerNumber;

        /// <summary>
        /// A fábrica de enumeardores de números primos.
        /// </summary>
        IPrimeNumberIteratorFactory<NumberType> primeNumberFactory;

        /// <summary>
        /// Algoritmo que permite calcular a raiz quadrada.
        /// </summary>
        private IAlgorithm<NumberType, NumberType> squareRootAlgorithm;

        /// <summary>
        /// Obtém uma instância do algorimo orientado para o cálculo da função totient.
        /// </summary>
        /// <param name="squareRootAlgorithm">O algoritmo responsável pela extracção de raízes quadradas.</param>
        /// <param name="primeNumberFactory">
        /// A fábrica responsável pela instanciação de um iterador para números
        /// primos.
        /// </param>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre inteiros.</param>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public EulerTotFuncAlg(
            IAlgorithm<NumberType, NumberType> squareRootAlgorithm,
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
            else if (squareRootAlgorithm == null)
            {
                throw new ArgumentNullException("squareNumberAlgorithm");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.primeNumberFactory = primeNumberFactory;
                this.squareRootAlgorithm = squareRootAlgorithm;
            }
        }

        /// <summary>
        /// Determina a quantidade de números primos inferiores ou iguais ao módulo do valor especificado.
        /// </summary>
        /// <remarks>A função está obsoleta.</remarks>
        /// <param name="data">O valor a ser analisado.</param>
        /// <returns>A quantidade de números primos nestas condições.</returns>
        /// <exception cref="ArgumentNullException">Se o valor for nulo.</exception>
        [Obsolete("This function doesn't work as expected.", true)]
        public NumberType Run_Old(NumberType data)
        {
            // TODO: Acelerar o processo eliminando o iterador para números primos. O teste de
            // primalidade é apenas aplicado caso o número encontrado seja um factor.
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
                            return this.integerNumber.MultiplicativeUnity;
                        }
                    }

                    var result = this.integerNumber.MapFrom(1);
                    var sqrt = this.squareRootAlgorithm.Run(innerData);
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
                                computation = this.integerNumber.GetQuotientAndRemainder(innerData, prime);
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

        /// <summary>
        /// Determina a quantidade de números primos inferiores ou iguais ao módulo do valor especificado.
        /// </summary>
        /// <param name="data">O valor a ser analisado.</param>
        /// <returns>A quantidade de números primos nestas condições.</returns>
        /// <exception cref="ArgumentNullException">Se o valor for nulo.</exception>
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
                            return this.integerNumber.MultiplicativeUnity;
                        }
                    }

                    var result = this.integerNumber.MapFrom(1);
                    var currentPrime = this.integerNumber.MapFrom(2);
                    var computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(result, currentPrime);
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        }
                    }

                    foreach (var unit in this.integerNumber.Units)
                    {
                        if (data.Equals(unit))
                        {
                            return result;
                        }
                    }

                    var sqrt = this.squareRootAlgorithm.Run(innerData);
                    currentPrime = this.integerNumber.MapFrom(3);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            innerData = computation.Quotient;
                            result = this.integerNumber.Multiply(result, currentPrime);
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        }

                        sqrt = this.squareRootAlgorithm.Run(innerData);
                    }

                    currentPrime = this.integerNumber.MapFrom(5);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            innerData = computation.Quotient;
                            result = this.integerNumber.Multiply(result, currentPrime);
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        }

                        sqrt = this.squareRootAlgorithm.Run(innerData);
                    }

                    var n1 = this.integerNumber.MapFrom(2);
                    var n2 = this.integerNumber.MapFrom(4);
                    var n3 = this.integerNumber.MapFrom(6);
                    currentPrime = this.integerNumber.MapFrom(7);
                    while (this.integerNumber.Compare(currentPrime, sqrt) <= 0)
                    {
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                            {
                                innerData = computation.Quotient;
                                result = this.integerNumber.Multiply(result, currentPrime);
                                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            }

                            sqrt = this.squareRootAlgorithm.Run(innerData);
                        }

                        currentPrime = this.integerNumber.Add(currentPrime, n2);
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                            {
                                innerData = computation.Quotient;
                                result = this.integerNumber.Multiply(result, currentPrime);
                                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            }

                            sqrt = this.squareRootAlgorithm.Run(innerData);
                        }

                        currentPrime = this.integerNumber.Add(currentPrime, n1);
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                            {
                                innerData = computation.Quotient;
                                result = this.integerNumber.Multiply(result, currentPrime);
                                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            }

                            sqrt = this.squareRootAlgorithm.Run(innerData);
                        }

                        currentPrime = this.integerNumber.Add(currentPrime, n2);
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                            {
                                innerData = computation.Quotient;
                                result = this.integerNumber.Multiply(result, currentPrime);
                                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            }

                            sqrt = this.squareRootAlgorithm.Run(innerData);
                        }

                        currentPrime = this.integerNumber.Add(currentPrime, n1);
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                            {
                                innerData = computation.Quotient;
                                result = this.integerNumber.Multiply(result, currentPrime);
                                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            }

                            sqrt = this.squareRootAlgorithm.Run(innerData);
                        }

                        currentPrime = this.integerNumber.Add(currentPrime, n2);
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                            {
                                innerData = computation.Quotient;
                                result = this.integerNumber.Multiply(result, currentPrime);
                                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            }

                            sqrt = this.squareRootAlgorithm.Run(innerData);
                        }

                        currentPrime = this.integerNumber.Add(currentPrime, n3);
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                            {
                                innerData = computation.Quotient;
                                result = this.integerNumber.Multiply(result, currentPrime);
                                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            }

                            sqrt = this.squareRootAlgorithm.Run(innerData);
                        }

                        currentPrime = this.integerNumber.Add(currentPrime, n1);
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                        if (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                        {
                            result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(currentPrime));
                            innerData = computation.Quotient;
                            computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                            {
                                innerData = computation.Quotient;
                                result = this.integerNumber.Multiply(result, currentPrime);
                                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                            }

                            sqrt = this.squareRootAlgorithm.Run(innerData);
                        }

                        currentPrime = this.integerNumber.Add(currentPrime, n3);
                    }

                    var isunit = false;
                    foreach (var unit in this.integerNumber.Units)
                    {
                        if (innerData.Equals(unit))
                        {
                            isunit = true;
                            break;
                        }
                    }

                    if (!isunit)
                    {
                        result = this.integerNumber.Multiply(result, this.integerNumber.Predecessor(innerData));
                    }

                    return result;
                }
            }
        }
    }
}
