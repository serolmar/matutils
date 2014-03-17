﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo mais simples para factorizar um número.
    /// </summary>
    public class NaiveIntegerFactorizationAlgorithm<NumberType> : IAlgorithm<NumberType, Dictionary<NumberType, int>>
    {
        /// <summary>
        /// Permite realizar todas as operações sobre números inteiros.
        /// </summary>
        private IIntegerNumber<NumberType> integerNumber;

        IAlgorithm<NumberType, NumberType> integerSquareRootAlgorithm;

        public NaiveIntegerFactorizationAlgorithm(
            IAlgorithm<NumberType, NumberType> integerSquareRootAlgorithm, 
            IIntegerNumber<NumberType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (integerSquareRootAlgorithm == null)
            {
                throw new ArgumentNullException("integerSquareRootAlgorithm");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.integerSquareRootAlgorithm = integerSquareRootAlgorithm;
            }
        }

        /// <summary>
        /// Obtém a factorização do número proporcionado.
        /// </summary>
        /// <param name="data">O número a ser factorizado.</param>
        /// <returns>Os factores primos afectos do respectivo grau.</returns>
        public Dictionary<NumberType, int> Run(NumberType data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else if (data.Equals(this.integerNumber.AdditiveUnity))
            {
                throw new ArgumentException("Zero has no factor.");
            }
            else
            {
                var result = new Dictionary<NumberType, int>(this.integerNumber);
                foreach (var unit in this.integerNumber.Units)
                {
                    if (data.Equals(unit))
                    {
                        result.Add(data, 1);
                        return result;
                    }
                }

                var innerData = this.integerNumber.GetNorm(data);
                var power = 0;
                var currentPrime = this.integerNumber.MapFrom(2);
                var computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                {
                    ++power;
                    innerData = computation.Quotient;
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                }

                if (power > 0)
                {
                    result.Add(currentPrime, power);
                }

                foreach (var unit in this.integerNumber.Units)
                {
                    if (data.Equals(unit))
                    {
                        return result;
                    }
                }

                var sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                power = 0;
                currentPrime = this.integerNumber.MapFrom(3);
                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                {
                    ++power;
                    innerData = computation.Quotient;
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                }

                if (power > 0)
                {
                    result.Add(currentPrime, power);
                    sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                }

                power = 0;
                currentPrime = this.integerNumber.MapFrom(5);
                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                {
                    ++power;
                    innerData = computation.Quotient;
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                }

                if (power > 0)
                {
                    result.Add(currentPrime, power);
                    sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                }

                var n1 = this.integerNumber.MapFrom(2);
                var n2 = this.integerNumber.MapFrom(4);
                var n3 = this.integerNumber.MapFrom(6);
                currentPrime = this.integerNumber.MapFrom(7);
                while (this.integerNumber.Compare(currentPrime, sqrt) <= 0)
                {
                    power = 0;
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        ++power;
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    }

                    if (power > 0)
                    {
                        result.Add(currentPrime, power);
                        sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                    }

                    power = 0;
                    currentPrime = this.integerNumber.Add(currentPrime, n2);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        ++power;
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    }

                    if (power > 0)
                    {
                        result.Add(currentPrime, power);
                        sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                    }

                    power = 0;
                    currentPrime = this.integerNumber.Add(currentPrime, n1);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        ++power;
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    }

                    if (power > 0)
                    {
                        result.Add(currentPrime, power);
                        sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                    }

                    power = 0;
                    currentPrime = this.integerNumber.Add(currentPrime, n2);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        ++power;
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    }

                    if (power > 0)
                    {
                        result.Add(currentPrime, power);
                        sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                    }

                    power = 0;
                    currentPrime = this.integerNumber.Add(currentPrime, n1);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        ++power;
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    }

                    if (power > 0)
                    {
                        result.Add(currentPrime, power);
                        sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                    }

                    power = 0;
                    currentPrime = this.integerNumber.Add(currentPrime, n2);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        ++power;
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    }

                    if (power > 0)
                    {
                        result.Add(currentPrime, power);
                        sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                    }

                    power = 0;
                    currentPrime = this.integerNumber.Add(currentPrime, n3);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        ++power;
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    }

                    if (power > 0)
                    {
                        result.Add(currentPrime, power);
                        sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                    }

                    power = 0;
                    currentPrime = this.integerNumber.Add(currentPrime, n1);
                    computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    while (this.integerNumber.IsAdditiveUnity(computation.Remainder))
                    {
                        ++power;
                        innerData = computation.Quotient;
                        computation = this.integerNumber.GetQuotientAndRemainder(innerData, currentPrime);
                    }

                    if (power > 0)
                    {
                        result.Add(currentPrime, power);
                        sqrt = this.integerSquareRootAlgorithm.Run(innerData);
                    }

                    currentPrime = this.integerNumber.Add(currentPrime, n3);
                }

                var isunit = false;
                foreach (var unit in this.integerNumber.Units)
                {
                    if (data.Equals(unit))
                    {
                        isunit = true;
                        break;
                    }
                }

                if (!isunit)
                {
                    result.Add(innerData, 1);
                }

                return result;
            }
        }
    }
}
