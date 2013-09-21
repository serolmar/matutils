namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa o corpo responsável pelas operações sobre polinómios módulo x^r-1 de uma forma rápida e eficaz.
    /// </summary>
    class AuxAksModArithmRing<CoeffType, RingType> : UnivarPolynomRing<CoeffType, RingType>
        where RingType : IRing<CoeffType>
    {
        /// <summary>
        /// O módulo segundo o qual são feitas as simplificações de potenciação.
        /// </summary>
        private int powerModule;

        public AuxAksModArithmRing(int powerModule, string variableName, RingType ring)
            : base(variableName, ring)
        {
            if (powerModule < 1)
            {
                throw new ArgumentException("The power module can't be less than one.");
            }
            else
            {
                this.powerModule = powerModule;
            }
        }

        /// <summary>
        /// Obtém e atribui o módulo segundo o qual são feitas as simplificações de potenciação.
        /// </summary>
        public int PowerModule
        {
            get
            {
                return this.powerModule;
            }
            set
            {
                if (value < 1)
                {
                    throw new MathematicsException("The power module can't be less than one.");
                }
                else
                {
                    this.powerModule = value;
                }
            }
        }

        public override UnivariatePolynomialNormalForm<CoeffType, RingType> Multiply(
            UnivariatePolynomialNormalForm<CoeffType, RingType> left,
            UnivariatePolynomialNormalForm<CoeffType, RingType> right)
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
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(
                    this.variableName,
                    this.ring);
                var innerLeft = this.GetReduced(left);
                var innerRight = this.GetReduced(right);
                foreach (var leftTermsKvp in innerLeft)
                {
                    foreach (var rightTermsKvp in innerRight)
                    {
                        var coeff = this.ring.Multiply(leftTermsKvp.Value, rightTermsKvp.Value);
                        var degree = (leftTermsKvp.Key + rightTermsKvp.Key) % this.powerModule;
                        result = result.Add(coeff, degree);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o polinómio reduzido mediante o módulo actual.
        /// </summary>
        /// <param name="element">O polinómio a ser reduzido.</param>
        /// <returns>O polinómio reduzido correspondente.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> GetReduced(
            UnivariatePolynomialNormalForm<CoeffType, RingType> element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(
                    this.variableName,
                    this.ring);
                foreach (var termKvp in element)
                {
                    var modularDegree = termKvp.Key % this.powerModule;
                    result = result.Add(termKvp.Value, modularDegree);
                }

                return result;
            }
        }
    }
}
