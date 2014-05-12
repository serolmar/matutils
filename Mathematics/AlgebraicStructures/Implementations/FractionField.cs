using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Permite aplicar as operações de corpo sobre objectos do tipo <see cref="FractionField[T}" />.
    /// </summary>
    /// <typeparam name="T">O tipo dos coeficientes das fracções.</typeparam>
    public class FractionField<T> : ElementFractionConversion<T>, IField<Fraction<T>>, IConversion<T, Fraction<T>>
    {
        /// <summary>
        /// O comparador de coeficientes das fracções.
        /// </summary>
        private IEqualityComparer<T> elementsComparer;

        /// <summary>
        /// Cria uma nova instância de objectos do tipo <see cref="FractionField{T}"/>.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        public FractionField(IEuclidenDomain<T> domain)
            : base(domain)
        {
            this.elementsComparer = null;
        }

        /// <summary>
        /// Cria uma nova instância de objectos do tipo <see cref="FractionField{T}"/>.
        /// </summary>
        /// <param name="elementsComparer">O comparador de coeficientes das fracções.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        public FractionField(IEqualityComparer<T> elementsComparer, IEuclidenDomain<T> domain)
            : base(domain)
        {
            this.elementsComparer = elementsComparer;
        }

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public Fraction<T> AdditiveUnity
        {
            get
            {
                return new Fraction<T>(this.domain);
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public Fraction<T> MultiplicativeUnity
        {
            get
            {
                return new Fraction<T>(
                    this.domain.MultiplicativeUnity,
                    this.domain.MultiplicativeUnity,
                    this.domain);
            }
        }

        /// <summary>
        /// Obtém a inversa multiplicativa de uma fracção.
        /// </summary>
        /// <param name="number">A fracção.</param>
        /// <returns>A inversa multiplicativa da fracção.</returns>
        /// <exception cref="System.ArgumentNullException">Caso a fracção proporcionada seja nula.</exception>
        public Fraction<T> MultiplicativeInverse(Fraction<T> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return number.GetInverse(this.domain);
            }
        }

        /// <summary>
        /// Obtém a inversa aditiva de uma fracção.
        /// </summary>
        /// <param name="number">A fracção.</param>
        /// <returns>A inversa multiplicativa.</returns>
        /// <exception cref="System.ArgumentNullException">Caso a fracção proporcionada seja nula.</exception>
        public Fraction<T> AdditiveInverse(Fraction<T> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return number.GetSymmetric(this.domain);
            }
        }

        /// <summary>
        /// Determina se uma fracção é uma unidade aditiva.
        /// </summary>
        /// <param name="value">A fracção a ser analisada.</param>
        /// <returns>Verdadeiro caso a fracção seja uma unidade aditiva e falso caso contrário.</returns>
        /// <exception cref="System.ArgumentNullException">Caso o valor passado seja nulo.</exception>
        public bool IsAdditiveUnity(Fraction<T> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return this.domain.IsAdditiveUnity(value.Numerator);
            }
        }

        /// <summary>
        /// Calcula a soma aritmética de duas fracções.
        /// </summary>
        /// <param name="left">A primeira fracção a ser adicionada.</param>
        /// <param name="right">A segunda fracção a ser adicionada.</param>
        /// <returns>O resultado da adição.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Caso ambos os argumentos sejam nulos.
        /// </exception>
        public Fraction<T> Add(Fraction<T> left, Fraction<T> right)
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
                return left.Add(right, this.domain);
            }
        }

        /// <summary>
        /// Calcula o produto aritmético de dois números.
        /// </summary>
        /// <param name="left">O primeiro número a ser multiplicado.</param>
        /// <param name="right">O segundo número a ser multiplicado.</param>
        /// <returns>O resultado do produto.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Caso um dos argumentos seja nulo.
        /// </exception>
        public Fraction<T> Multiply(Fraction<T> left, Fraction<T> right)
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
                return left.Multiply(right, this.domain);
            }
        }

        /// <summary>
        /// Determina se a fracção é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">A fracção.</param>
        /// <returns>Verdadeiro caso a fracção seja uma unidade multiplicativa e falso caso contrário.</returns>
        /// <exception cref="System.ArgumentNullException">Caso o valor seja nulo.</exception>
        public bool IsMultiplicativeUnity(Fraction<T> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return this.domain.IsMultiplicativeUnity(value.Numerator) &&
                    this.domain.IsMultiplicativeUnity(value.Denominator);
            }
        }

        /// <summary>
        /// Calcula o resultado da adição repetida de uma fracção.
        /// </summary>
        /// <param name="element">A fracção.</param>
        /// <param name="times">O número de vezes a adicionar.</param>
        /// <returns>O resultado da adição repetida.</returns>
        public Fraction<T> AddRepeated(Fraction<T> element, int times)
        {
            if (this.domain.IsAdditiveUnity(element.Numerator))
            {
                return new Fraction<T>(element.Numerator, element.Denominator, this.domain);
            }
            else
            {
                var added = this.domain.AddRepeated(element.Numerator, times);
                return new Fraction<T>(added, element.Denominator, this.domain);
            }
        }

        /// <summary>
        /// Determina quando dois números decimais são iguais.
        /// </summary>
        /// <param name="x">A primeira fracção a ser comparada.</param>
        /// <param name="y">A segunda fracção a ser comparada.</param>
        /// <returns>
        /// Verdadeiro caso ambos os objectos sejam iguais e falso no caso contrário.
        /// </returns>
        public bool Equals(Fraction<T> x, Fraction<T> y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null)
            {
                return false;
            }
            else if (y == null)
            {
                return false;
            }
            else if (this.elementsComparer == null)
            {
                return x.Numerator.Equals(y.Numerator) && x.Denominator.Equals(y.Denominator);
            }
            else
            {
                return this.elementsComparer.Equals(x.Numerator, y.Numerator) &&
                    this.elementsComparer.Equals(x.Denominator, y.Denominator);
            }
        }

        /// <summary>
        /// Retorna um código confuso para a instância.
        /// </summary>
        /// <param name="obj">A instância.</param>
        /// <returns>
        /// Um código confuso para a instância que pode ser usado em vários algoritmos habituais.
        /// </returns>
        public int GetHashCode(Fraction<T> obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else if (this.elementsComparer == null)
            {
                return 19 * obj.Numerator.GetHashCode() + 17 * obj.Denominator.GetHashCode();
            }
            else
            {
                return 19 * this.elementsComparer.GetHashCode(obj.Numerator) +
                    17 * this.elementsComparer.GetHashCode(obj.Denominator);
            }
        }
    }
}
