using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Um item relativo à leitura de um polinómio univariável.
    /// </summary>
    /// <typeparam name="CoeffType">O tiop de coeficientes do polinómio.</typeparam>
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ParseUnivarPolynomNormalFormItem{CoeffType}"/>.
        /// </summary>
        public ParseUnivarPolynomNormalFormItem()
        {
        }

        /// <summary>
        /// Obtém o valor polinomial caso seja aplicável.
        /// </summary>
        /// <value>O valor polinomial.</value>
        /// <exception cref="MathematicsException">Se o item retornado não representar um polinómio.</exception>
        public UnivariatePolynomialNormalForm<CoeffType> Polynomial
        {
            get
            {
                if (this.valueType != EParsePolynomialValueType.POLYNOMIAL)
                {
                    throw new MathematicsException("Value isn't a polynomial.");
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
        /// <value>O coeficiente.</value>
        /// <exception cref="MathematicsException">Se o item retornado não representar um coeficiente.</exception>
        public CoeffType Coeff
        {
            get
            {
                if (this.valueType != EParsePolynomialValueType.COEFFICIENT)
                {
                    throw new MathematicsException("Value isn't a coefficient.");
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
        /// <value>O valor inteiro.</value>
        /// <exception cref="MathematicsException">Se o item retornado não representar um inteiro.</exception>
        public int Degree
        {
            get
            {
                if (this.valueType != EParsePolynomialValueType.INTEGER)
                {
                    throw new MathematicsException("Value isn't an integer.");
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
        /// <value>O tipo de valor.</value>
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
