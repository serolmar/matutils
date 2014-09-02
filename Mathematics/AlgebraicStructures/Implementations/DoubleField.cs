namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Proporciona as funções de corpo sobre números decimais de precisão dupla.
    /// Ver também <see cref="double"/>.
    /// </summary>
    public class DoubleField : IField<double>
    {
        /// <summary>
        /// Mantém a precisão nas comparações.
        /// </summary>
        private double precision;

        /// <summary>
        /// Instancia objectos do tipo <see cref="DoubleField"/>.
        /// </summary>
        /// <param name="precisionUnits">A quantidade de unidades de precisão.</param>
        public DoubleField(double precisionUnits = 0.0)
        {
            if (this.precision < 0)
            {
                throw new ArgumentNullException("The precision must be a non-negative number.");
            }
            else
            {
                this.precision = precisionUnits;
            }
        }

        /// <summary>
        /// Obtém a quantidade precisão utilizada nas comparações de números de ponto flutuante.
        /// </summary>
        public double Precision
        {
            get
            {
                return this.precision;
            }
        }

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>A unidade aditiva.</value>
        public double AdditiveUnity
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public double MultiplicativeUnity
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Permite determinar a inversa multiplicativa de um número.
        /// </summary>
        /// <param name="number">O número do qual se pretende obter a inversa multiplicativa.</param>
        /// <returns>A inversa multiplicativa.</returns>
        /// <exception cref="DivideByZeroException">Se o número passao for zero.</exception>
        public double MultiplicativeInverse(double number)
        {
            if (number == 0)
            {
                throw new DivideByZeroException();
            }
            else
            {
                return 1 / number;
            }
        }

        /// <summary>
        /// Permite efectuar adições repetidas de um número decimal de precisão dupla.
        /// </summary>
        /// <param name="element">O número a ser adicionado.</param>
        /// <param name="times">O número de vezes que o número é adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        public double AddRepeated(double element, int times)
        {
            return element * times;
        }

        /// <summary>
        /// Permite determinar o simétrico de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O simétrico.</returns>
        public double AdditiveInverse(double number)
        {
            return -number;
        }

        /// <summary>
        /// Determina se um valor é uma unidade aditivia.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsAdditiveUnity(double value)
        {
            return this.Equals(value, 0);
        }

        /// <summary>
        /// Determina quando dois números de precisão dupla são iguais.
        /// </summary>
        /// <param name="x">O primeiro número de precisão dupla a ser comparado.</param>
        /// <param name="y">O segundo número de precisão dupla a ser comparado.</param>
        /// <returns>
        /// Verdadeiro caso ambos os objectos sejam iguais e falso no caso contrário.
        /// </returns>
        public bool Equals(double x, double y)
        {
            if (this.precision == 0.0)
            {
                return x.Equals(y);
            }
            else
            {
                var difference = Math.Abs(x - y);
                return difference <= this.precision;
            }
        }

        /// <summary>
        /// Retorna um código confuso para a instância.
        /// </summary>
        /// <param name="obj">A instância.</param>
        /// <returns>
        /// Um código confuso para a instância que pode ser usado em vários algoritmos habituais.
        /// </returns>
        public int GetHashCode(double obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Calcula a adição aritmética de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado da adição.</returns>
        public double Add(double left, double right)
        {
            return left + right;
        }

        /// <summary>
        /// Determina se o valor é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade multiplicativa e falso caso contrário.</returns>
        public bool IsMultiplicativeUnity(double value)
        {
            return this.Equals(value, 1);
        }

        /// <summary>
        /// Calcula a multiplicação de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado da multiplicação.</returns>
        public double Multiply(double left, double right)
        {
            return left * right;
        }
    }
}
