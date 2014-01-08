﻿using System;
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

        public ComplexNumber<CoeffType> MultiplicativeUnity
        {
            get
            {
                return new ComplexNumber<CoeffType>(
                    this.coeffsField.MultiplicativeUnity,
                    this.coeffsField.AdditiveUnity);
            }
        }

        public ComplexNumber<CoeffType> AdditiveUnity
        {
            get
            {
                return new ComplexNumber<CoeffType>(this.coeffsField.AdditiveUnity, this.coeffsField.AdditiveUnity);
            }
        }

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

        public ComplexNumber<CoeffType> AddRepeated(ComplexNumber<CoeffType> element, int times)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else if (times < 0)
            {
                throw new ArgumentException("The number of times in repeated add must be non-negative.");
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
