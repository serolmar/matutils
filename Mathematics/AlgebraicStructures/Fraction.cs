using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mathematics.Algorithms;

namespace Mathematics
{
    public class Fraction<T,D> where D : IEuclidenDomain<T>
    {
        /// <summary>
        /// O domínio euclideano onde se encontra definida a fracção.
        /// </summary>
        private D euclideanDomain;

        /// <summary>
        /// O numerador.
        /// </summary>
        private T numerator;

        /// <summary>
        /// O denominador.
        /// </summary>
        private T denominator;

        public Fraction(D euclideanDomain)
        {
            if (euclideanDomain == null)
            {
                throw new ArgumentNullException("euclideanDomain");
            }

            this.euclideanDomain = euclideanDomain;
            this.numerator = this.euclideanDomain.AdditiveUnity;
            this.denominator = this.euclideanDomain.MultiplicativeUnity;
        }

        public Fraction(T numerator, T denominator, D euclideanDomain)
        {
            if (numerator == null)
            {
                throw new ArgumentNullException("numerator");
            }

            if (denominator == null)
            {
                throw new ArgumentNullException("denominator");
            }

            if (euclideanDomain == null)
            {
                throw new ArgumentNullException("euclideanDomain");
            }

            this.euclideanDomain = euclideanDomain;
            if (euclideanDomain.IsAdditiveUnity(denominator))
            {
                throw new ArgumentException("Denominator can't be zero.");
            }

            if (euclideanDomain.IsAdditiveUnity(numerator))
            {
                this.numerator = numerator;
                this.denominator = euclideanDomain.MultiplicativeUnity;
            }
            else
            {
                this.numerator = numerator;
                this.denominator = denominator;
                this.Reduce();
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
        public T IntegralPart
        {
            get
            {
                return this.euclideanDomain.Quo(this.numerator, this.denominator);
            }
        }

        /// <summary>
        /// Obtém o que resta da fracção quando lhe é extraída a sua parte inteira.
        /// </summary>
        public Fraction<T,D> FractionalPart
        {
            get
            {
                var remainder = this.euclideanDomain.Rem(this.numerator, this.denominator);
                return new Fraction<T, D>(remainder, this.denominator, this.euclideanDomain);
            }
        }

        /// <summary>
        /// Obtém a decomposição da fracção em parte inteira e fracionária na mesma função.
        /// </summary>
        public FractionDecompositionResult<T, D> FractionDecomposition
        {
            get
            {
                var domainResult = this.euclideanDomain.GetQuotientAndRemainder(this.numerator, this.denominator);
                return new FractionDecompositionResult<T,D>(
                    domainResult.Quotient,
                    new Fraction<T,D>(domainResult.Remainder, this.denominator, this.euclideanDomain));
                    
            }
        }

        /// <summary>
        /// Obtém o resultado da adição da fracção corrente com a fracção especificada.
        /// </summary>
        /// <param name="right">A fracção especificada.</param>
        /// <returns>O resultado da soma.</returns>
        public Fraction<T, D> Add(Fraction<T, D> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            var gcd = MathFunctions.GreatCommonDivisor<T, D>(this.denominator, right.denominator, this.euclideanDomain);
            var currentReducedDenominator = this.euclideanDomain.Quo(this.denominator, gcd);
            var rightReducedDenominator = this.euclideanDomain.Quo(right.denominator, gcd);
            var resultDenominator = this.euclideanDomain.Multiply(currentReducedDenominator, right.denominator);
            var currentAddNumerator = this.euclideanDomain.Multiply(this.numerator, rightReducedDenominator);
            var rightAddNumerator = this.euclideanDomain.Multiply(right.numerator, currentReducedDenominator);
            var resultNumerator = this.euclideanDomain.Add(currentAddNumerator, rightAddNumerator);
            return new Fraction<T, D>(resultNumerator, resultDenominator, this.euclideanDomain);
        }

        /// <summary>
        /// Obtém o resultado da diferença entre a fracção corrente e a fracção especificada.
        /// </summary>
        /// <param name="right">A fracção especificada.</param>
        /// <returns>O resultado da subtracção.</returns>
        public Fraction<T, D> Subtract(Fraction<T, D> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            var gcd = MathFunctions.GreatCommonDivisor<T, D>(this.denominator, right.denominator, this.euclideanDomain);
            var currentReducedDenominator = this.euclideanDomain.Quo(this.denominator, gcd);
            var rightReducedDenominator = this.euclideanDomain.Quo(right.denominator, gcd);
            var resultDenominator = this.euclideanDomain.Multiply(currentReducedDenominator, right.denominator);
            var currentAddNumerator = this.euclideanDomain.Multiply(this.numerator, rightReducedDenominator);
            var rightAddNumerator = this.euclideanDomain.Multiply(right.numerator, currentReducedDenominator);
            rightAddNumerator = this.euclideanDomain.Multiply(
                this.euclideanDomain.AdditiveInverse(this.euclideanDomain.MultiplicativeUnity),
                rightAddNumerator);
            var resultNumerator = this.euclideanDomain.Add(currentAddNumerator, rightAddNumerator);
            return new Fraction<T, D>(resultNumerator, resultDenominator, this.euclideanDomain);
        }

        /// <summary>
        /// Obtém o resultado do produto da fracção corrente com a fracção especificada.
        /// </summary>
        /// <param name="right">A fracção especificada.</param>
        /// <returns>O resultado do produto.</returns>
        public Fraction<T, D> Multiply(Fraction<T, D> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            var crossGcd = MathFunctions.GreatCommonDivisor(this.numerator, right.denominator, this.euclideanDomain);
            var firstNumerator = this.euclideanDomain.Quo(this.numerator, crossGcd);
            var secondDenominator = this.euclideanDomain.Quo(right.denominator, crossGcd);
            crossGcd = MathFunctions.GreatCommonDivisor(this.denominator, right.numerator, this.euclideanDomain);
            var secondNumerator = this.euclideanDomain.Quo(right.numerator, crossGcd);
            var firstDenominator = this.euclideanDomain.Quo(this.denominator, crossGcd);

            var resultNumerator = this.euclideanDomain.Multiply(firstNumerator, secondNumerator);
            var resultDenominator = this.euclideanDomain.Multiply(firstDenominator, secondDenominator);

            var result = new Fraction<T, D>(this.euclideanDomain);
            result.numerator = resultNumerator;
            result.denominator = resultDenominator;
            return result;
        }

        /// <summary>
        /// Obtém o resultado do quociente da fracção corrente com a fracção especificada.
        /// </summary>
        /// <param name="right">A fracção especificada.</param>
        /// <returns>O resultado do quociente.</returns>
        public Fraction<T, D> Divide(Fraction<T, D> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (this.euclideanDomain.IsAdditiveUnity(right.numerator))
            {
                throw new DivideByZeroException();
            }
            else
            {
                var resultNumerator = this.euclideanDomain.Multiply(this.numerator, right.denominator);
                var resultDenominator = this.euclideanDomain.Multiply(this.denominator, right.numerator);
                return new Fraction<T, D>(resultNumerator, resultDenominator, this.euclideanDomain);
            }
        }

        /// <summary>
        /// Obtém a respectiva inversa.
        /// </summary>
        /// <returns>A inversa.</returns>
        public Fraction<T, D> GetInverse()
        {
            if (this.euclideanDomain.IsAdditiveUnity(this.numerator))
            {
                throw new DivideByZeroException();
            }
            else
            {
                return new Fraction<T, D>(this.denominator, this.numerator, this.euclideanDomain);
            }
        }

        /// <summary>
        /// Obtém a fracção simétrica.
        /// </summary>
        /// <returns>A fracção simétrica.</returns>
        public Fraction<T, D> GetSymmetric()
        {
            var symmetricNumerator = this.euclideanDomain.AdditiveInverse(this.numerator);
            return new Fraction<T, D>(symmetricNumerator, this.denominator, this.euclideanDomain);
        }

        public override string ToString()
        {
            return string.Format("({0})/({1})", this.numerator, this.denominator);
        }

        /// <summary>
        /// Reduz a fracção à forma canónica.
        /// </summary>
        private void Reduce()
        {
            T gcd = MathFunctions.GreatCommonDivisor<T, D>(this.numerator, this.denominator, this.euclideanDomain);
            this.numerator = this.euclideanDomain.Quo(this.numerator, gcd);
            this.denominator = this.euclideanDomain.Quo(this.denominator, gcd);
        }
    }
}
