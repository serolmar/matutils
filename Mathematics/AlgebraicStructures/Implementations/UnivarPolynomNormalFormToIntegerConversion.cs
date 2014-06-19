namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite converter um polinómio para um inteiro.
    /// </summary>
    /// <typeparam name="CoeffsType">O tipo dos objectos que constituem os coeficientes.</typeparam>
    public class UnivarPolynomNormalFormToIntegerConversion<CoeffsType> 
        : IConversion<int, UnivariatePolynomialNormalForm<CoeffsType>>
    {
        /// <summary>
        /// O anel responsável pela determinaçao do zero e da unidade dos coeficientes.
        /// </summary>
        private IRing<CoeffsType> coeffsRing;

        /// <summary>
        /// O conversor responsável por tentar converter um coeficiente para um valor inteiro.
        /// </summary>
        private IConversion<int, CoeffsType> coeffsConversion;

        /// <summary>
        /// A variável associada ao polinómio convertido.
        /// </summary>
        private string variableName;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="UnivarPolynomNormalFormToIntegerConversion{CoeffsType}"/>.
        /// </summary>
        /// <param name="variableName">O nome da variável dos polinómios sobre os quais se efectuam as operações.</param>
        /// <param name="coeffsConversion">O conversor que permite converter inteiros em coeficientes.</param>
        /// <param name="coeffsRing">O anel responsável pelas operações sobre os coeficientes.</param>
        public UnivarPolynomNormalFormToIntegerConversion(
            string variableName,
            IConversion<int, CoeffsType> coeffsConversion,
            IRing<CoeffsType> coeffsRing)
        {
            if (coeffsRing == null)
            {
                throw new ArgumentNullException("coeffsRing");
            }
            if (coeffsConversion == null)
            {
                throw new ArgumentNullException("coeffsConversion");
            }
            else if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new MathematicsException("Variable name can't be empty.");
            }
            else
            {
                this.coeffsConversion = coeffsConversion;
                this.coeffsRing = coeffsRing;
                this.variableName = variableName;
            }
        }

        /// <summary>
        /// Indica se é possível converter um polinómio para inteiro.
        /// </summary>
        /// <param name="objectToConvert">O polinómio em análise.</param>
        /// <returns>Verdadeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyDirectConversion(UnivariatePolynomialNormalForm<CoeffsType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else if (objectToConvert.IsValue)
            {
                var value = objectToConvert.GetAsValue(this.coeffsRing);
                return this.coeffsConversion.CanApplyDirectConversion(value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indica se é possível converter um número inteiro para um polinómio.
        /// </summary>
        /// <remarks>
        /// Esta conversão é sempre possível se for possível converter o inteiro num coeficiente.
        /// </remarks>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>Verdadeiro caso a conversão seja possível e falso caso contrário.</returns>
        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return this.coeffsConversion.CanApplyInverseConversion(objectToConvert);
        }

        /// <summary>
        /// Obtém o resultado da conversão de um polinómio num inteiro.
        /// </summary>
        /// <param name="objectToConvert">O polinómio.</param>
        /// <returns>O inteiro convertido.</returns>
        /// <exception cref="ArgumentNullException">Se o polinómio proporcionado for nulo.</exception>
        /// <exception cref="MathematicsException">Caso o polinómio não represente um valor inteiro.</exception>
        public int DirectConversion(UnivariatePolynomialNormalForm<CoeffsType> objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else if (objectToConvert.IsValue)
            {
                var value = objectToConvert.GetAsValue(this.coeffsRing);
                return this.coeffsConversion.DirectConversion(value);
            }
            else
            {
                throw new MathematicsException(string.Format(
                    "Can't convert {0} to integer.",
                    objectToConvert));
            }
        }

        /// <summary>
        /// Converte um número inteiro num polinómio.
        /// </summary>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>O polinómio.</returns>
        public UnivariatePolynomialNormalForm<CoeffsType> InverseConversion(int objectToConvert)
        {
            var value = this.coeffsConversion.InverseConversion(objectToConvert);
            return new UnivariatePolynomialNormalForm<CoeffsType>(
                value,
                0,
                variableName,
                this.coeffsRing);
        }
    }
}
