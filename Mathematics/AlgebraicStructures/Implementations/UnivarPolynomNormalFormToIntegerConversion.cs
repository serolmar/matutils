namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
            }
        }

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

        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return this.coeffsConversion.CanApplyInverseConversion(objectToConvert);
        }

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
