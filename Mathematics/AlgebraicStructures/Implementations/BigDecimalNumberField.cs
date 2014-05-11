namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Proporciona as funções de corpo sobre números decimais de precisão arbitrária.
    /// Ver também <see cref="BigDecimalNumber"/>.
    /// </summary>
    public class BigDecimalNumberField : IField<BigDecimalNumber>
    {
        /// <summary>
        /// Permite determinar rapidamente o logaritmo na base dois de um número binário.
        /// </summary>
        private static FasterBigIntBinaryLogIntPartAlg integerPartLogAlg = new FasterBigIntBinaryLogIntPartAlg();

        /// <summary>
        /// Permite calcular de forma rápida o logaritmo binário de um inteiro de precisão arbitrária.
        /// </summary>
        private FasterBigIntBinaryLogIntPartAlg binaryLogAlg = new FasterBigIntBinaryLogIntPartAlg();

        /// <summary>
        /// A precisão em número de bits.
        /// </summary>
        private int bitsPrecision;

        /// <summary>
        /// A precisão decimal associada aos números.
        /// </summary>
        private int decimalPrecision;

        /// <summary>
        /// Cria uma instância capaz de aplicar as operações aritméticas sobre números decimais de precisão
        /// arbitrária.
        /// </summary>
        /// <param name="bitsPrecision">A precisão em número de bits utilizada durante as operações.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Caso o número de bits proporcionado seja um número
        /// negativo.</exception>
        public BigDecimalNumberField(int bitsPrecision)
        {
            if (bitsPrecision < 0)
            {
                throw new ArgumentOutOfRangeException("bitsPrecision");
            }
            else
            {
                this.bitsPrecision = bitsPrecision;
                this.decimalPrecision = (int)(bitsPrecision * Math.Log10(2));
            }
        }

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>A unidade aditiva.</value>
        public BigDecimalNumber AdditiveUnity
        {
            get
            {
                return BigDecimalNumber.Zero;
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public BigDecimalNumber MultiplicativeUnity
        {
            get
            {
                return BigDecimalNumber.One;
            }
        }

        /// <summary>
        /// Otbém a precisão em número de bits utilizada durante as operações sobre os números decimais
        /// de precisão arbitrária.
        /// </summary>
        public int BitsPrecision
        {
            get
            {
                return this.bitsPrecision;
            }
        }

        /// <summary>
        /// Obtém a precisão decimal.
        /// </summary>
        /// <remarks>
        /// A precisão associada às operações sobre os números decimais são definidas em termos de número de bits.
        /// Assim, o número de algarismos significativos correctos é dado pela sua precisão decimal. Esta calcula-se
        /// como D = d log10(2) onde D é a precisão decimal, d é a precisão em bits e log10(2) corresponde ao logaritmo
        /// de dois na base dez.
        /// </remarks>
        /// <value>
        /// A precisão decimal.
        /// </value>
        public int DecimalPrecision
        {
            get
            {
                return this.decimalPrecision;
            }
        }

        /// <summary>
        /// Permite determinar a inversa multiplicativa de um número.
        /// </summary>
        /// <param name="number">O número do qual se pretende obter a inversa multiplicativa.</param>
        /// <returns>A inversa multiplicativa.</returns>
        public BigDecimalNumber MultiplicativeInverse(BigDecimalNumber number)
        {
            return BigDecimalNumber.Divide(BigDecimalNumber.One, number, this.bitsPrecision);
        }

        /// <summary>
        /// Permite efectuar adições repetidas de um número decimal.
        /// </summary>
        /// <param name="element">O número a ser adicionado.</param>
        /// <param name="times">O número de vezes que o número é adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        public BigDecimalNumber AddRepeated(BigDecimalNumber element, int times)
        {
            var number = element.Number * times;
            var exponent = element.Exponent;
            var numberLog = (int)this.binaryLogAlg.Run(BigInteger.Abs(number));
            if (numberLog > this.bitsPrecision)
            {
                var difference = numberLog - this.bitsPrecision;
                number = number >> difference;
                exponent += difference;
            }

            var result = new BigDecimalNumber(number, element.Exponent);
            return result;
        }

        /// <summary>
        /// Permite determinar o simétrico de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O simétrico.</returns>
        public BigDecimalNumber AdditiveInverse(BigDecimalNumber number)
        {
            return BigDecimalNumber.Negate(number);
        }

        /// <summary>
        /// Determina se um valor é uma unidade aditivia.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsAdditiveUnity(BigDecimalNumber value)
        {
            return value.Number.IsZero;
        }

        /// <summary>
        /// Determina quando dois números decimais são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto do tipo <paramref name="T" /> a ser comparado.</param>
        /// <param name="y">O segundo objecto do tipo <paramref name="T" /> a ser comparado.</param>
        /// <returns>
        /// Verdadeiro caso ambos os objectos sejam iguais e falso no caso contrário.
        /// </returns>
        public bool Equals(BigDecimalNumber x, BigDecimalNumber y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Retorna um código confuso para a instância.
        /// </summary>
        /// <param name="obj">A instância.</param>
        /// <returns>
        /// Um código confuso para a instância que pode ser usado em vários algoritmos habituais.
        /// </returns>
        public int GetHashCode(BigDecimalNumber obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Calcula a adição aritmética entre dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns></returns>
        public BigDecimalNumber Add(BigDecimalNumber left, BigDecimalNumber right)
        {
            return BigDecimalNumber.Add(left, right);
        }

        /// <summary>
        /// Determina se o valor é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade multiplicativa e falso caso contrário.</returns>
        public bool IsMultiplicativeUnity(BigDecimalNumber value)
        {
            return value.Number.IsOne && value.Exponent == 1;
        }

        /// <summary>
        /// Calcula a multiplicação de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado da multiplicação.</returns>
        public BigDecimalNumber Multiply(BigDecimalNumber left, BigDecimalNumber right)
        {
            return BigDecimalNumber.Multiply(left, right, this.bitsPrecision);
        }

        /// <summary>
        /// Retorna um valor do tipo <see cref="System.String" /> que representa a instância.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>
        /// Um valor do tipo <see cref="System.String" /> que representa a instância.
        /// </returns>
        public string ToString(BigDecimalNumber number)
        {
            return number.ToString(this.bitsPrecision);
        }

        /// <summary>
        /// Tenta efectuar a leitura de um número decimal a partir de uma representação textual.
        /// </summary>
        /// <param name="numberText">A representação textual do número.</param>
        /// <param name="number">O número.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(string numberText, out BigDecimalNumber number)
        {
            return BigDecimalNumber.TryParse(numberText, this.bitsPrecision, out number);
        }
    }
}
