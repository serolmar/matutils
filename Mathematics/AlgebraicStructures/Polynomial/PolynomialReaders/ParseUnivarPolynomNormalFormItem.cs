using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class ParseUnivarPolynomNormalFormItem<CoeffType>
    {
        /// <summary>
        /// O valor polinomial.
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> polynomial;
        
        /// <summary>
        /// O coeficiente.
        /// </summary>
        private CoeffType coeff;

        /// <summary>
        /// O valor inteiro.
        /// </summary>
        private int degree;

        /// <summary>
        /// O tipo de valor existente.
        /// </summary>
        private EParsePolynomialValueType valueType = EParsePolynomialValueType.INTEGER;

        public ParseUnivarPolynomNormalFormItem()
        {
        }

        /// <summary>
        /// Obtém o valor polinomial caso seja aplicável.
        /// </summary>
        public UnivariatePolynomialNormalForm<CoeffType> Polynomial
        {
            get
            {
                if (this.valueType != EParsePolynomialValueType.POLYNOMIAL)
                {
                    throw new MathematicsException("Value is an integer.");
                }
                else
                {
                    return this.polynomial;
                }
            }
            set
            {
                this.polynomial = value;
                this.valueType = EParsePolynomialValueType.POLYNOMIAL;
            }
        }

        /// <summary>
        /// Obtém o coeficiente.
        /// </summary>
        public CoeffType Coeff
        {
            get
            {
                if (this.valueType != EParsePolynomialValueType.COEFFICIENT)
                {
                    throw new MathematicsException("Value is an integer.");
                }
                else
                {
                    return this.coeff;
                }
            }
            set
            {
                this.coeff = value;
                this.valueType = EParsePolynomialValueType.COEFFICIENT;
            }
        }

        /// <summary>
        /// Obtém o valor inteiro caso seja aplicável.
        /// </summary>
        public int Degree
        {
            get
            {
                if (this.valueType != EParsePolynomialValueType.INTEGER)
                {
                    throw new MathematicsException("Value is a polynomial.");
                }
                else
                {
                    return this.degree;
                }
            }
            set
            {
                this.degree = value;
                this.valueType = EParsePolynomialValueType.INTEGER;
            }
        }

        /// <summary>
        /// Obtém o tipo de valor.
        /// </summary>
        public EParsePolynomialValueType ValueType
        {
            get
            {
                return this.valueType;
            }
        }

        /// <summary>
        /// Obtém uma representação texutal do elemento corrente.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            if (this.valueType == EParsePolynomialValueType.COEFFICIENT)
            {
                return string.Format("{0}: {1}", this.valueType, this.coeff);
            }
            else if (this.valueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                return string.Format("{0}: {1}", this.valueType, this.polynomial);
            }
            else
            {
                return string.Format("{0}: {1}", this.valueType, this.degree);
            }
        }
    }
}
