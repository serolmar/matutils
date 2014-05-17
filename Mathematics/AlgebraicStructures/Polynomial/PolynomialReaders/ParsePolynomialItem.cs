using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Um item de leitura polinomial.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem os coeficientes dos polinómios a serem lidos.</typeparam>
    public class ParsePolynomialItem<CoeffType>
    {
        /// <summary>
        /// O valor polinomial.
        /// </summary>
        private Polynomial<CoeffType> polynomial;

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
        /// Instancia um novo objecto do tipo <see cref="ParsePolynomialItem{CoeffType}"/>.
        /// </summary>
        public ParsePolynomialItem()
        {
        }

        /// <summary>
        /// Obtém o valor polinomial caso seja aplicável.
        /// </summary>
        /// <value>O valor polinomial caso seja aplicável.</value>
        /// <exception cref="MathematicsException">Se o valor a retornar não for um polinómio.</exception>
        public Polynomial<CoeffType> Polynomial
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
        /// <exception cref="MathematicsException">Se o valor a retornar não for um coeficiente.</exception>
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
        /// <value>O grau.</value>
        /// <exception cref="MathematicsException">Se o valor não for um inteiro.</exception>
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
        /// <value>O tipo do valor.</value>
        public EParsePolynomialValueType ValueType
        {
            get
            {
                return this.valueType;
            }
        }
    }
}
