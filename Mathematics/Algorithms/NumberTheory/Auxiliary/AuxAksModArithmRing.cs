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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="AuxAksModArithmRing{CoeffType}"/>.
        /// </summary>
        /// <param name="powerModule">O módulo segundo o qual são realizadas as simplificações de potência.</param>
        /// <param name="variableName">O nome da variável.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentException">Se o módulo potência for inferior a um.</exception>
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
        /// <value>O módulo.</value>
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

        /// <summary>
        /// Multiplica dois polinómio.
        /// </summary>
        /// <param name="left">O primeiro polinómio a ser multiplicado.</param>
        /// <param name="right">O segundo polinómio a ser multiplicado.</param>
        /// <returns>O resultado da multiplicação.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se pelo menos um dos argumentos for nulo.
        /// </exception>
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
        /// <exception cref="ArgumentNullException">Se o polinómio for nulo.</exception>
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
