namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Proporciona as funções de corpo sobre números decimais de precisão dupla.
    /// Ver também <see cref="System.double"/>.
    /// </summary>
    public class DoubleField : IField<double>
    {
        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>A unidade aditiva.</value>
        public double AdditiveUnity
        {
            get {
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
            get {
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
        /// Permite efectuar adições repetidas de um número decimal.
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
            return value == 0;
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
            return x.Equals(y);
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
        /// Calcula a adição aritmética entre dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns></returns>
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
            return value == 1;
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
