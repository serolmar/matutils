namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    public class NullableDoubleField : IField<Nullable<double>>
    {
        /// <summary>
        /// O objecto responsável pelas operações de corpo sobre números de vírgula flutuante.
        /// </summary>
        private DoubleField field = new DoubleField();

        /// <summary>
        /// Verifica se se trata de uma unidade aditiva.
        /// </summary>
        public Nullable<double> AdditiveUnity
        {
            get
            {
                return this.field.AdditiveUnity;
            }
        }

        /// <summary>
        /// Verifica se se trata de uma unidade multiplicativa.
        /// </summary>
        public Nullable<double> MultiplicativeUnity
        {
            get
            {
                return this.field.MultiplicativeUnity;
            }
        }

        /// <summary>
        /// Determina a inversa multiplicativa de um número de vírgula flutuante.
        /// </summary>
        /// <param name="number">O número do qual se pretende obter a inversa.</param>
        /// <returns>A inversa do número especificado.</returns>
        public Nullable<double> MultiplicativeInverse(Nullable<double> number)
        {
            if (number.HasValue)
            {
                return this.field.MultiplicativeInverse(number.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        /// <summary>
        /// Adiciona uma quantidade um número especificado de vezes.
        /// </summary>
        /// <param name="element">A quantidade a ser adicionada.</param>
        /// <param name="times">O número de vezes que a quantidade é adicionada.</param>
        /// <returns>O resultado da soma.</returns>
        public Nullable<double> AddRepeated(Nullable<double> element, int times)
        {
            if (element.HasValue)
            {
                return this.field.AddRepeated(element.Value, times);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        /// <summary>
        /// Obtém a inversa aditiva de um número de vírgula flutuante.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>A inversa aditiva.</returns>
        public Nullable<double> AdditiveInverse(Nullable<double> number)
        {
            if (number.HasValue)
            {
                return this.field.AdditiveInverse(number.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        /// <summary>
        /// Determina se um número é uma unidade aditiva.
        /// </summary>
        /// <param name="value">O número a ser verificado.</param>
        /// <returns>Verdadeiro caso o número seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsAdditiveUnity(Nullable<double> value)
        {
            if (value.HasValue)
            {
                return this.field.IsAdditiveUnity(value.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        /// <summary>
        /// Verifica se dois números de vírgula flutuante são iguais.
        /// </summary>
        /// <param name="x">O primeiro número a comparar.</param>
        /// <param name="y">O segundo número a comparar.</param>
        /// <returns>Verdadeiro caso os números sejam iguais e falso caso contrário.</returns>
        public bool Equals(Nullable<double> x, Nullable<double> y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x.HasValue && y.HasValue)
            {
                return this.field.Equals(x.Value, y.Value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém o código confuso de um número.
        /// </summary>
        /// <param name="obj">O número do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso.</returns>
        public int GetHashCode(Nullable<double> obj)
        {
            if (obj.HasValue)
            {
                return this.field.GetHashCode(obj.Value);
            }
            else
            {
                return typeof(Nullable<double>).GetHashCode();
            }
        }

        /// <summary>
        /// Determina a soma de dois números de vírgula flutuante.
        /// </summary>
        /// <param name="left">O primeiro número a ser adicionado.</param>
        /// <param name="right">O segundo número a ser adicionado.</param>
        /// <returns>O resultado da soma.</returns>
        public Nullable<double> Add(Nullable<double> left, Nullable<double> right)
        {
            if (left.HasValue && right.HasValue)
            {
                return this.field.Add(left.Value, right.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        /// <summary>
        /// Averigua se um número constitui uma unidade multipllicativa.
        /// </summary>
        /// <param name="value">O número.</param>
        /// <returns>Verdadeiro caso o número seja uma unidade multiplicativa e falso caso contrário.</returns>
        public bool IsMultiplicativeUnity(Nullable<double> value)
        {
            if (value.HasValue)
            {
                return this.field.IsMultiplicativeUnity(value.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        /// <summary>
        /// Determina o produto de dois números de vírgula flutuante.
        /// </summary>
        /// <param name="left">O primeiro número a ser multiplciado.</param>
        /// <param name="right">O segundo número a ser multiplicado.</param>
        /// <returns>O resultado do produto.</returns>
        public Nullable<double> Multiply(Nullable<double> left, Nullable<double> right)
        {
            if (left.HasValue && right.HasValue)
            {
                return this.field.Multiply(left.Value, right.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }
    }
}
