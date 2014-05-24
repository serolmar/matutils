using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Implementa as operações que são efectuadas sobre um polinómio a uma variável apenas.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos coeficientes do polinómio.</typeparam>
    public class UnivarPolynomRing<CoeffType> :
        IRing<UnivariatePolynomialNormalForm<CoeffType>>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </summary>
        protected IRing<CoeffType> ring;

        /// <summary>
        /// O nome da variável dos polinómios que podems ser submetidos às operações.
        /// </summary>
        protected string variableName;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="UnivarPolynomRing{CoeffType}"/>
        /// </summary>
        /// <param name="variableName">O nome da variável.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        public UnivarPolynomRing(string variableName, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentException("Variable must not be empty.");
            }
            else
            {
                this.variableName = variableName;
                this.ring = ring;
            }
        }

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public virtual UnivariatePolynomialNormalForm<CoeffType> AdditiveUnity
        {
            get
            {
                return new UnivariatePolynomialNormalForm<CoeffType>(this.variableName);
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public virtual UnivariatePolynomialNormalForm<CoeffType> MultiplicativeUnity
        {
            get
            {
                return new UnivariatePolynomialNormalForm<CoeffType>(
                    this.ring.MultiplicativeUnity, 
                    0, 
                    this.variableName, 
                    this.ring);
            }
        }

        /// <summary>
        /// Obtém o inverso aditivo de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O inverso aditivo do número.</returns>
        public virtual UnivariatePolynomialNormalForm<CoeffType> AdditiveInverse(
            UnivariatePolynomialNormalForm<CoeffType> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return number.GetSymmetric(this.ring);
            }
        }

        /// <summary>
        /// Determina se um polinómio é uma unidade aditiva.
        /// </summary>
        /// <param name="value">O polinómio.</param>
        /// <returns>Verdadeiro caso o polinómio seja uma unidade aditiva e falso caso contrário.</returns>
        public virtual bool IsAdditiveUnity(UnivariatePolynomialNormalForm<CoeffType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return value.IsZero;
            }
        }

        /// <summary>
        /// Calcula a soma de dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio a ser adicionado.</param>
        /// <param name="right">O segundo polinómio a ser adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        /// <exception cref="ArgumentNullException">Se algums dos polinómios for nulo.</exception>
        public virtual UnivariatePolynomialNormalForm<CoeffType> Add(
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
                return left.Add(right, this.ring);
            }
        }

        /// <summary>
        /// Determina se ambos os polinómios são iguais.
        /// </summary>
        /// <param name="x">O primeiro polinómio a ser comparado.</param>
        /// <param name="y">O segundo polinómio a ser comparado.</param>
        /// <returns>
        /// Verdadeiro se ambos os polinómios forem iguais e falso caso contrário.
        /// </returns>
        public virtual bool Equals(
            UnivariatePolynomialNormalForm<CoeffType> x, 
            UnivariatePolynomialNormalForm<CoeffType> y)
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
            else
            {
                return x.Equals(y);
            }
        }

        /// <summary>
        /// Obtém o código confuso de um polinómio.
        /// </summary>
        /// <param name="obj">O polinómio.</param>
        /// <returns>
        /// O código confuso do polinómio adequado à utilização em alguns algoritmos habituais.
        /// </returns>
        public virtual int GetHashCode(UnivariatePolynomialNormalForm<CoeffType> obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.GetHashCode();
            }
        }

        /// <summary>
        /// Calcula o produto de dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio a ser multiplicado.</param>
        /// <param name="right">O segundo polinómio a ser multiplicado.</param>
        /// <returns>O resultado da multiplicação.</returns>
        public virtual UnivariatePolynomialNormalForm<CoeffType> Multiply(
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
                return left.Multiply(right, this.ring);
            }
        }

        /// <summary>
        /// Determina se o polinómio é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O polinómio.</param>
        /// <returns>Verdadeiro caso o polinómio seja uma unidade aditiva e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o polinómio proporcionado for nulo.</exception>
        public virtual bool IsMultiplicativeUnity(UnivariatePolynomialNormalForm<CoeffType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return value.IsUnity(this.ring);
            }
        }

        /// <summary>
        /// Calcula a adição repetida de um polinómio.
        /// </summary>
        /// <remarks>
        /// A adição repetida de um polinómio é rapidamente determinada com base na operação de multiplicação.
        /// </remarks>
        /// <param name="element">O polinómio a ser adicionado.</param>
        /// <param name="times">O número de vezes que é realizada a adição.</param>
        /// <returns>O resultado da adição repetida.</returns>
        public virtual UnivariatePolynomialNormalForm<CoeffType> AddRepeated(
            UnivariatePolynomialNormalForm<CoeffType> element, 
            int times)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType>(this.variableName);
                foreach (var termsKvp in element)
                {
                    result = result.Add(
                        this.ring.AddRepeated(termsKvp.Value, times),
                        termsKvp.Key, this.ring);
                }

                return result;
            }
        }
    }
}
