﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mathematics.Algorithms;

namespace Mathematics
{
    public class Fraction<T>
    {
        /// <summary>
        /// O numerador.
        /// </summary>
        private T numerator;

        /// <summary>
        /// O denominador.
        /// </summary>
        private T denominator;

        public Fraction(IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("euclideanDomain");
            }
            else
            {
                this.numerator = domain.AdditiveUnity;
                this.denominator = domain.MultiplicativeUnity;
            }
        }

        public Fraction(T numerator, T denominator, IEuclidenDomain<T> domain)
        {
            if (numerator == null)
            {
                throw new ArgumentNullException("numerator");
            }
            else if (denominator == null)
            {
                throw new ArgumentNullException("denominator");
            }
            else if (domain == null)
            {
                throw new ArgumentNullException("euclideanDomain");
            }
            else if (domain.IsAdditiveUnity(denominator))
            {
                throw new ArgumentException("Denominator can't be zero.");
            }
            else if (domain.IsAdditiveUnity(numerator))
            {
                this.numerator = numerator;
                this.denominator = domain.MultiplicativeUnity;
            }
            else
            {
                this.numerator = numerator;
                this.denominator = denominator;
                this.Reduce(domain);
            }
        }

        /// <summary>
        /// Obtém o numerador.
        /// </summary>
        public T Numerator
        {
            get
            {
                return this.numerator;
            }
        }

        /// <summary>
        /// Obtém o denominador.
        /// </summary>
        public T Denominator
        {
            get
            {
                return this.denominator;
            }
        }

        /// <summary>
        /// Obtém a parte inteira da fracção.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        public T IntegralPart(IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                return domain.Quo(this.numerator, this.denominator);
            }
        }

        /// <summary>
        /// Obtém o que resta da fracção quando lhe é extraída a sua parte inteira.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        public Fraction<T> FractionalPart(IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                var remainder = domain.Rem(this.numerator, this.denominator);
                return new Fraction<T>(remainder, this.denominator, domain);
            }
        }

        /// <summary>
        /// Obtém a decomposição da fracção em parte inteira e fracionária na mesma função.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        public FractionDecompositionResult<T> FractionDecomposition(IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                var domainResult = domain.GetQuotientAndRemainder(this.numerator, this.denominator);
                return new FractionDecompositionResult<T>(
                    domainResult.Quotient,
                    new Fraction<T>(domainResult.Remainder, this.denominator, domain));

            }
        }

        /// <summary>
        /// Obtém o resultado da adição da fracção corrente com a fracção especificada.
        /// </summary>
        /// <param name="right">A fracção especificada.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado da soma.</returns>
        public Fraction<T> Add(Fraction<T> right, IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var gcd = MathFunctions.GreatCommonDivisor(this.denominator, right.denominator, domain);
                var currentReducedDenominator = domain.Quo(this.denominator, gcd);
                var rightReducedDenominator = domain.Quo(right.denominator, gcd);
                var resultDenominator = domain.Multiply(currentReducedDenominator, right.denominator);
                var currentAddNumerator = domain.Multiply(this.numerator, rightReducedDenominator);
                var rightAddNumerator = domain.Multiply(right.numerator, currentReducedDenominator);
                var resultNumerator = domain.Add(currentAddNumerator, rightAddNumerator);
                return new Fraction<T>(resultNumerator, resultDenominator, domain);
            }
        }

        /// <summary>
        /// Obtém o resultado da diferença entre a fracção corrente e a fracção especificada.
        /// </summary>
        /// <param name="right">A fracção especificada.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado da subtracção.</returns>
        public Fraction<T> Subtract(Fraction<T> right, IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var gcd = MathFunctions.GreatCommonDivisor(this.denominator, right.denominator, domain);
                var currentReducedDenominator = domain.Quo(this.denominator, gcd);
                var rightReducedDenominator = domain.Quo(right.denominator, gcd);
                var resultDenominator = domain.Multiply(currentReducedDenominator, right.denominator);
                var currentAddNumerator = domain.Multiply(this.numerator, rightReducedDenominator);
                var rightAddNumerator = domain.Multiply(right.numerator, currentReducedDenominator);
                rightAddNumerator = domain.Multiply(
                    domain.AdditiveInverse(domain.MultiplicativeUnity),
                    rightAddNumerator);
                var resultNumerator = domain.Add(currentAddNumerator, rightAddNumerator);
                return new Fraction<T>(resultNumerator, resultDenominator, domain);
            }
        }

        /// <summary>
        /// Obtém o resultado do produto da fracção corrente com a fracção especificada.
        /// </summary>
        /// <param name="right">A fracção especificada.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado do produto.</returns>
        public Fraction<T> Multiply(Fraction<T> right, IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (domain.IsAdditiveUnity(this.numerator))
            {
                var result = new Fraction<T>(domain);
                result.numerator = domain.AdditiveUnity;
                result.denominator = domain.MultiplicativeUnity;
                return result;
            }
            else if (domain.IsAdditiveUnity(right.numerator))
            {
                var result = new Fraction<T>(domain);
                result.numerator = domain.AdditiveUnity;
                result.denominator = domain.MultiplicativeUnity;
                return result;
            }
            else
            {
                var crossGcd = MathFunctions.GreatCommonDivisor(this.numerator, right.denominator, domain);
                var firstNumerator = domain.Quo(this.numerator, crossGcd);
                var secondDenominator = domain.Quo(right.denominator, crossGcd);
                crossGcd = MathFunctions.GreatCommonDivisor(this.denominator, right.numerator, domain);
                var secondNumerator = domain.Quo(right.numerator, crossGcd);
                var firstDenominator = domain.Quo(this.denominator, crossGcd);

                var resultNumerator = domain.Multiply(firstNumerator, secondNumerator);
                var resultDenominator = domain.Multiply(firstDenominator, secondDenominator);

                var result = new Fraction<T>(domain);
                result.numerator = resultNumerator;
                result.denominator = resultDenominator;
                return result;
            }
        }

        /// <summary>
        /// Obtém o resultado do quociente da fracção corrente com a fracção especificada.
        /// </summary>
        /// <param name="right">A fracção especificada.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado do quociente.</returns>
        public Fraction<T> Divide(Fraction<T> right, IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (domain.IsAdditiveUnity(right.numerator))
            {
                throw new DivideByZeroException();
            }
            else
            {
                var resultNumerator = domain.Multiply(this.numerator, right.denominator);
                var resultDenominator = domain.Multiply(this.denominator, right.numerator);
                return new Fraction<T>(resultNumerator, resultDenominator, domain);
            }
        }

        /// <summary>
        /// Obtém a respectiva inversa.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>A inversa.</returns>
        public Fraction<T> GetInverse(IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (domain.IsAdditiveUnity(this.numerator))
            {
                throw new DivideByZeroException();
            }
            else
            {
                return new Fraction<T>(this.denominator, this.numerator, domain);
            }
        }

        /// <summary>
        /// Obtém a fracção simétrica.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>A fracção simétrica.</returns>
        public Fraction<T> GetSymmetric(IEuclidenDomain<T> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                var symmetricNumerator = domain.AdditiveInverse(this.numerator);
                return new Fraction<T>(symmetricNumerator, this.denominator, domain);
            }
        }

        public override string ToString()
        {
            return string.Format("({0})/({1})", this.numerator, this.denominator);
        }

        /// <summary>
        /// Reduz a fracção à forma canónica.
        /// </summary>
        /// <param name="domain">O domínio sobre o qual se processa a redução.</param>
        private void Reduce(IEuclidenDomain<T> domain)
        {
            T gcd = MathFunctions.GreatCommonDivisor(this.numerator, this.denominator, domain);
            this.numerator = domain.Quo(this.numerator, gcd);
            this.denominator = domain.Quo(this.denominator, gcd);
        }

        /// <summary>
        /// Verifica a igualdade entre fracções.
        /// </summary>
        /// <param name="obj">A fracção a comprar.</param>
        /// <returns>Verdadeiro caso as fracções sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                var innerObj = obj as Fraction<T>;
                if (innerObj == null)
                {
                    return false;
                }
                else
                {
                    return this.numerator.Equals(innerObj.numerator) &&
                           this.denominator.Equals(innerObj.denominator);
                }
            }
        }

        /// <summary>
        /// Obtém o valor representativo da fracção actual.
        /// </summary>
        /// <returns>O valor da fracção actual.</returns>
        public override int GetHashCode()
        {
            var result = 17 * this.numerator.GetHashCode();
            result *= 19 * this.denominator.GetHashCode();
            return result.GetHashCode();
        }
    }
}
