namespace Mathematics.AlgebraicStructures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um número complexo sobre um corpo.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objecto.</typeparam>
    /// <typeparam name="RingType">O anel que actua sobre o objecto.</typeparam>
    public class ComplexNumber<ObjectType, RingType>
        where RingType : IRing<ObjectType>
    {
        /// <summary>
        /// O anel.
        /// </summary>
        protected RingType ring;

        /// <summary>
        /// A parte real.
        /// </summary>
        protected ObjectType realPart;

        /// <summary>
        /// A parte imaginária.
        /// </summary>
        protected ObjectType imaginaryPart;

        public ComplexNumber(RingType ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("A ring must be provided.");
            }
            else
            {
                this.ring = ring;
            }
        }

        public ComplexNumber(ObjectType realPart, ObjectType imaginaryPart, RingType ring)
            : this(ring)
        {
            if (realPart == null)
            {
                throw new ArgumentNullException("realPart");
            }
            else if (imaginaryPart == null)
            {
                throw new ArgumentNullException("imaginaryPart");
            }
            else
            {
                this.realPart = realPart;
                this.imaginaryPart = imaginaryPart;
            }
        }

        /// <summary>
        /// Obtém a parte real do número complexo.
        /// </summary>
        public ObjectType RealPart
        {
            get
            {
                return this.realPart;
            }
        }

        /// <summary>
        /// Obtém a parte imaginária do número complexo.
        /// </summary>
        public ObjectType ImaginaryPart
        {
            get
            {
                return this.imaginaryPart;
            }
        }

        /// <summary>
        /// Obtém a soma do número complexo actual com outro número complexo.
        /// </summary>
        /// <param name="right">O outro número complexo.</param>
        /// <returns>O resultado da soma.</returns>
        public ComplexNumber<ObjectType, RingType> Add(ComplexNumber<ObjectType, RingType> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var result = new ComplexNumber<ObjectType, RingType>(this.ring);
                result.realPart = this.ring.Add(this.realPart, right.realPart);
                result.imaginaryPart = this.ring.Add(this.imaginaryPart, right.imaginaryPart);
                return result;
            }
        }

        /// <summary>
        /// Obtém a diferença entre o número complexo actual e outro número complexo.
        /// </summary>
        /// <param name="right">O outro número complexo.</param>
        /// <returns>O resultado da diferença.</returns>
        public ComplexNumber<ObjectType, RingType> Subtract(ComplexNumber<ObjectType, RingType> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var result = new ComplexNumber<ObjectType, RingType>(this.ring);
                result.realPart = this.ring.Add(this.realPart, this.ring.AdditiveInverse(right.realPart));
                result.imaginaryPart = this.ring.Add(this.imaginaryPart, this.ring.AdditiveInverse(right.imaginaryPart));
                return result;
            }
        }

        /// <summary>
        /// Obtém o produto do número complexo actual com outro número complexo.
        /// </summary>
        /// <param name="right">O outro número complexo.</param>
        /// <returns>O resultado do produto.</returns>
        public ComplexNumber<ObjectType, RingType> Multiply(ComplexNumber<ObjectType, RingType> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var result = new ComplexNumber<ObjectType, RingType>(this.ring);
                result.realPart = this.ring.Add(
                    this.ring.Multiply(this.realPart, right.realPart), 
                    this.ring.AdditiveInverse(this.ring.Multiply(this.imaginaryPart, right.imaginaryPart)));
                result.imaginaryPart = this.ring.Add(this.ring.Multiply(
                    this.realPart, right.imaginaryPart),
                    this.ring.Multiply(this.imaginaryPart, right.realPart));
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}] + (i)[{1}]", this.realPart, this.imaginaryPart);
        }
    }
}
