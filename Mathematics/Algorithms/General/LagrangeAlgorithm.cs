﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class LagrangeAlgorithm<T> : IBachetBezoutAlgorithm<T>
    {
        private IEuclidenDomain<T> domain;

        public LagrangeAlgorithm(IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                this.domain = domain;
            }
        }

        /// <summary>
        /// Obtém o domínio inerente ao algoritmo corrente.
        /// </summary>
        public IEuclidenDomain<T> Domain
        {
            get
            {
                return this.domain;
            }
        }

        public BacheBezoutResult<T> Run(T first, T second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else
            {
                var prevFirstCoeff = this.domain.MultiplicativeUnity;
                var prevSecondCoeff = this.domain.AdditiveUnity;
                var currentFirstCoeff = this.domain.AdditiveUnity;
                var currentSecondCoeff = this.domain.AdditiveInverse(this.domain.MultiplicativeUnity);
                var prevValue = first;
                var currentValue = second;
                var sign = -1;
                var domainResult = this.domain.GetQuotientAndRemainder(prevValue, currentValue);
                while (!this.domain.IsAdditiveUnity(domainResult.Remainder))
                {
                    var firstTempVar = currentFirstCoeff;
                    var secondTempVar = currentSecondCoeff;
                    currentFirstCoeff = this.domain.Multiply(currentFirstCoeff, domainResult.Quotient);
                    currentFirstCoeff = this.domain.Add(currentFirstCoeff, prevFirstCoeff);
                    currentSecondCoeff = this.domain.Multiply(currentSecondCoeff, domainResult.Quotient);
                    currentSecondCoeff = this.domain.Add(currentSecondCoeff, prevSecondCoeff);
                    prevFirstCoeff = firstTempVar;
                    prevSecondCoeff = secondTempVar;
                    prevValue = currentValue;
                    currentValue = domainResult.Remainder;
                    sign = -sign;
                    domainResult = this.domain.GetQuotientAndRemainder(prevValue, currentValue);
                }

                var firstTempValue = currentFirstCoeff;
                var secondTempValue = currentSecondCoeff;
                currentFirstCoeff = this.domain.Multiply(currentFirstCoeff, domainResult.Quotient);
                currentFirstCoeff = this.domain.Add(currentFirstCoeff, prevFirstCoeff);
                currentSecondCoeff = this.domain.Multiply(currentSecondCoeff, domainResult.Quotient);
                currentSecondCoeff = this.domain.Add(currentSecondCoeff, prevSecondCoeff);
                prevFirstCoeff = firstTempValue;
                prevSecondCoeff = secondTempValue;
                if (sign < 0)
                {
                    prevSecondCoeff = this.domain.Multiply(prevSecondCoeff, this.domain.AdditiveInverse(
                        this.domain.MultiplicativeUnity));
                }

                return new BacheBezoutResult<T>(
                    first,
                    second,
                    prevFirstCoeff,
                    prevSecondCoeff,
                    currentValue,
                    this.domain.AdditiveInverse(currentSecondCoeff),
                    currentFirstCoeff);
            }
        }
    }
}
