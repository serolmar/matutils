namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa as operações de anel sobre o polinómio de grau generalizado.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente do polinómio.</typeparam>
    /// <typeparam name="DegreeType">O tipo de grau do polinómio.</typeparam>
    public class GeneralDegUnivarPolynomRing<CoeffType, DegreeType>
        : IRing<GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>>
    {
        protected IIntegerNumber<DegreeType> integerNumber;

        protected IRing<CoeffType> ring;

        protected string variableName;

        public GeneralDegUnivarPolynomRing(
            string variableName, 
            IRing<CoeffType> ring,
            IIntegerNumber<DegreeType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (ring == null)
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

        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> AdditiveUnity
        {
            get
            {
                return new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.variableName,
                    this.integerNumber);
            }
        }

        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> MultiplicativeUnity
        {
            get
            {
                return new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.ring.MultiplicativeUnity, 
                    this.integerNumber.AdditiveUnity, 
                    this.variableName,
                    this.ring,
                    this.integerNumber);
            }
        }

        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> AdditiveInverse(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> number)
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

        public virtual bool IsAdditiveUnity(GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> value)
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

        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Add(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> left, 
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> right)
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

        public virtual bool Equals(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> x, 
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> y)
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

        public virtual int GetHashCode(GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> obj)
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

        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Multiply(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> left, 
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> right)
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

        public virtual bool IsMultiplicativeUnity(GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> value)
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

        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> AddRepeated(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> element, 
            int times)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.variableName,
                    this.integerNumber);
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
