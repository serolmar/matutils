using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Implementa as operações de corpo sobre os números complexos.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coficiente associado ao número complexo.</typeparam>
    public class ComplexNumberField<CoeffType> : IField<ComplexNumber<CoeffType>>
    {
        /// <summary>
        /// Mantém a referência para o corpo responsável pelas operações sobre os coeficientes
        /// associados ao número complexo.
        /// </summary>
        private IField<CoeffType> coeffsField;

        /// <summary>
        /// Cria a instância de um objecto capaz de multiplicar dois números do tipo <see cref="ComplexNumber{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffsField">
        /// O corpo responsável pela multiplicação dos coeficientes que constituem cada uma das partes do número complexo.
        /// </param>
        public ComplexNumberField(IField<CoeffType> coeffsField)
        {
            if (coeffsField == null)
            {
                throw new ArgumentNullException("coeffsField");
            }
            else
            {
                this.coeffsField = coeffsField;
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public ComplexNumber<CoeffType> MultiplicativeUnity
        {
            get
            {
                return new ComplexNumber<CoeffType>(
                    this.coeffsField.MultiplicativeUnity,
                    this.coeffsField.AdditiveUnity);
            }
        }

        /// <summary>
        /// Otbém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public ComplexNumber<CoeffType> AdditiveUnity
        {
            get
            {
                return new ComplexNumber<CoeffType>(this.coeffsField.AdditiveUnity, this.coeffsField.AdditiveUnity);
            }
        }

        /// <summary>
        /// Obtém a inversa multiplicativa de um valor.
        /// </summary>
        /// <param name="number">O valor.</param>
        /// <returns>A inversa multiplicativa.</returns>
        /// <exception cref="ArgumentNullException">Caso o valor seja nulo.</exception>
        public ComplexNumber<CoeffType> MultiplicativeInverse(ComplexNumber<CoeffType> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                var den = this.coeffsField.Add(
                    this.coeffsField.Multiply(number.RealPart, number.RealPart),
                    this.coeffsField.Multiply(number.ImaginaryPart, number.ImaginaryPart));
                den = this.coeffsField.MultiplicativeInverse(den);
                var real = this.coeffsField.Multiply(number.RealPart, den);
                var imaginary = this.coeffsField.Multiply(
                    this.coeffsField.AdditiveInverse(number.ImaginaryPart),
                    den);
                return new ComplexNumber<CoeffType>(real, imaginary);
            }
        }

        /// <summary>
        /// Permite adicionar um número complexo tantas vezes quantas as especificadas.
        /// </summary>
        /// <remarks>
        /// Por vezes o processo de adição repetida pode ser realizado de uma forma mais eficaz do que
        /// efectuar o algoritmo habitual que envolve um conjunto de somas.
        /// </remarks>
        /// <param name="element">O número complexo a ser adicionado.</param>
        /// <param name="times">O número de vezes que se efectua a adição.</param>
        /// <returns>O resultado da adição repetida.</returns>
        /// <exception cref="ArgumentNullException">Caso o número complexo proporcionado seja nulo.</exception>
        public ComplexNumber<CoeffType> AddRepeated(ComplexNumber<CoeffType> element, int times)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else
            {
                if (times == 0)
                {
                    return new ComplexNumber<CoeffType>(
                        this.coeffsField.AdditiveUnity, 
                        this.coeffsField.AdditiveUnity);
                }
                else if (times == 1)
                {
                    return new ComplexNumber<CoeffType>(element.RealPart, element.ImaginaryPart);
                }
                else
                {
                    var real = this.coeffsField.AddRepeated(element.RealPart, times);
                    var imaginary = this.coeffsField.AddRepeated(element.ImaginaryPart, times);
                    return new ComplexNumber<CoeffType>(real, imaginary);
                }
            }
        }

        /// <summary>
        /// Obtém a inversa aditiva de um número complexo.
        /// </summary>
        /// <param name="number">O número complexo.</param>
        /// <returns>A inversa aditiva.</returns>
        /// <exception cref="ArgumentNullException">Caso o número complexo proporcionado seja nulo.</exception>
        public ComplexNumber<CoeffType> AdditiveInverse(ComplexNumber<CoeffType> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return new ComplexNumber<CoeffType>(
                    this.coeffsField.AdditiveInverse(number.RealPart),
                    this.coeffsField.AdditiveInverse(number.ImaginaryPart));
            }
        }

        /// <summary>
        /// Determina se um determinado valor constitui uma unidade aditiva.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade aditiva e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Caso o número complexo proporcionado seja nulo.</exception>
        public bool IsAdditiveUnity(ComplexNumber<CoeffType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return value.IsOne(this.coeffsField);
            }
        }

        /// <summary>
        /// Determina se os objectos especificados são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto do tipo <paramref name="T" /> a ser comparado.</param>
        /// <param name="y">O segundo objecto do tipo <paramref name="T" /> a ser comparado.</param>
        /// <returns>
        /// Verdadeiro caso os objectos sejam iguais e falso caso contrário.
        /// </returns>
        public bool Equals(ComplexNumber<CoeffType> x, ComplexNumber<CoeffType> y)
        {
            if (object.ReferenceEquals(x, y))
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
                return this.coeffsField.Equals(x.RealPart, y.RealPart) &&
                    this.coeffsField.Equals(x.ImaginaryPart, y.ImaginaryPart);
            }
        }

        /// <summary>
        /// Retorna um código confuso da instância.
        /// </summary>
        /// <param name="obj">A instância.</param>
        /// <returns>
        /// O código confuso da instância que pode ser utilizado em vários algoritmos habituais.
        /// </returns>
        /// <exception cref="ArgumentNullException">Se o obejcto passado for nulo.</exception>
        public int GetHashCode(ComplexNumber<CoeffType> obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else
            {
                var result = this.coeffsField.GetHashCode(obj.RealPart);
                result ^= 37 * this.coeffsField.GetHashCode(obj.ImaginaryPart);
                return result;
            }
        }

        /// <summary>
        /// Calcula a soma de dois números complexos.
        /// </summary>
        /// <param name="left">O primeiro número complexo.</param>
        /// <param name="right">O segundo número complexo.</param>
        /// <returns>O resultado da soma.</returns>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public ComplexNumber<CoeffType> Add(ComplexNumber<CoeffType> left, ComplexNumber<CoeffType> right)
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
                return left.Add(right, this.coeffsField);
            }
        }

        /// <summary>
        /// Determina se o valor especificado é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade multiplicativa e falso caso contrário.</returns>
        /// <exception cref="ArgumentException">Caso o valor seja nulo.</exception>
        public bool IsMultiplicativeUnity(ComplexNumber<CoeffType> value)
        {
            if (value == null)
            {
                throw new ArgumentException("value");
            }
            else
            {
                return value.IsOne(this.coeffsField);
            }
        }

        /// <summary>
        /// Calcula o produto de dois números complexos.
        /// </summary>
        /// <param name="left">O primeiro número complexo.</param>
        /// <param name="right">O segundo número complexo.</param>
        /// <returns>O resultado do produto.</returns>
        /// <exception cref="ArgumentNullException">Caso um dos argumentos seja nulo.</exception>
        public ComplexNumber<CoeffType> Multiply(ComplexNumber<CoeffType> left, ComplexNumber<CoeffType> right)
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
                return left.Multiply(right, this.coeffsField);
            }
        }
    }
}
