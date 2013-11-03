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
    public class ComplexNumber<ObjectType>
    {

        /// <summary>
        /// A parte real.
        /// </summary>
        protected ObjectType realPart;

        /// <summary>
        /// A parte imaginária.
        /// </summary>
        protected ObjectType imaginaryPart;

        public ComplexNumber()
        {
        }

        public ComplexNumber(ObjectType realPart, ObjectType imaginaryPart)
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
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O resultado da soma.</returns>
        public ComplexNumber<ObjectType> Add(ComplexNumber<ObjectType> right, IRing<ObjectType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var result = new ComplexNumber<ObjectType>();
                result.realPart = ring.Add(this.realPart, right.realPart);
                result.imaginaryPart = ring.Add(this.imaginaryPart, right.imaginaryPart);
                return result;
            }
        }

        /// <summary>
        /// Obtém a diferença entre o número complexo actual e outro número complexo.
        /// </summary>
        /// <param name="right">O outro número complexo.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O resultado da diferença.</returns>
        public ComplexNumber<ObjectType> Subtract(ComplexNumber<ObjectType> right, IRing<ObjectType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var result = new ComplexNumber<ObjectType>();
                result.realPart = ring.Add(this.realPart, ring.AdditiveInverse(right.realPart));
                result.imaginaryPart = ring.Add(this.imaginaryPart, ring.AdditiveInverse(right.imaginaryPart));
                return result;
            }
        }

        /// <summary>
        /// Obtém o produto do número complexo actual com outro número complexo.
        /// </summary>
        /// <param name="right">O outro número complexo.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O resultado do produto.</returns>
        public ComplexNumber<ObjectType> Multiply(ComplexNumber<ObjectType> right, IRing<ObjectType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var result = new ComplexNumber<ObjectType>();
                result.realPart = ring.Add(
                    ring.Multiply(this.realPart, right.realPart), 
                    ring.AdditiveInverse(ring.Multiply(this.imaginaryPart, right.imaginaryPart)));
                result.imaginaryPart = ring.Add(ring.Multiply(
                    this.realPart, right.imaginaryPart),
                    ring.Multiply(this.imaginaryPart, right.realPart));
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}] + (i)[{1}]", this.realPart, this.imaginaryPart);
        }
    }
}
