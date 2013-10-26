namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa o corpo responsável pelas operações sobre polinómios módulo x^r-1 de uma forma rápida e eficaz.
    /// </summary>
    class AuxAksModArithmRing<CoeffType> : UnivarPolynomRing<CoeffType>
    {
        /// <summary>
        /// O módulo segundo o qual são feitas as simplificações de potenciação.
        /// </summary>
        private int powerModule;

        public AuxAksModArithmRing(int powerModule, string variableName, IRing<CoeffType> ring)
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

        public override UnivariatePolynomialNormalForm<CoeffType> Multiply(
            UnivariatePolynomialNormalForm<CoeffType> left,
            UnivariatePolynomialNormalForm<CoeffType> right)
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
                var result = new UnivariatePolynomialNormalForm<CoeffType>(
                    this.variableName);
                var innerLeft = this.GetReduced(left);
                var innerRight = this.GetReduced(right);
                foreach (var leftTermsKvp in innerLeft)
                {
                    foreach (var rightTermsKvp in innerRight)
                    {
                        var coeff = this.ring.Multiply(leftTermsKvp.Value, rightTermsKvp.Value);
                        var degree = (leftTermsKvp.Key + rightTermsKvp.Key) % this.powerModule;
                        result = result.Add(coeff, degree, this.ring);
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
        public UnivariatePolynomialNormalForm<CoeffType> GetReduced(
            UnivariatePolynomialNormalForm<CoeffType> element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType>(
                    this.variableName);
                foreach (var termKvp in element)
                {
                    var modularDegree = termKvp.Key % this.powerModule;
                    result = result.Add(termKvp.Value, modularDegree, this.ring);
                }

                return result;
            }
        }
    }
}
