namespace Mathematics
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

        /// <summary>
        /// Construtor responsável pelas instâncias internas à livraria.
        /// </summary>
        internal ComplexNumber()
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ComplexNumber{ObjectType}"/>.
        /// </summary>
        /// <param name="realPart">A parte real.</param>
        /// <param name="imaginaryPart">A parte imaginária.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
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
        /// <value>A parte real do número complexo.</value>
        public ObjectType RealPart
        {
            get
            {
                return this.realPart;
            }
            internal set
            {
                this.realPart = value;
            }
        }

        /// <summary>
        /// Obtém a parte imaginária do número complexo.
        /// </summary>
        /// <value>A parte imaginária do número complexo.</value>
        public ObjectType ImaginaryPart
        {
            get
            {
                return this.imaginaryPart;
            }
            internal set
            {
                this.imaginaryPart = value;
            }
        }

        /// <summary>
        /// Verifica se o número complexo actual é o número complexo nulo.
        /// </summary>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>Verdadeiro caso o número seja zero e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o monóide for nulo.</exception>
        public bool IsZero(IMonoid<ObjectType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                return monoid.IsAdditiveUnity(this.realPart) && 
                    monoid.IsAdditiveUnity(this.imaginaryPart);
            }
        }

        /// <summary>
        /// Verifica se o número complexo actual corresponde à unidade.
        /// </summary>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>Verdadeiro caso o número complexo seja unitário e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o anel for nulo.</exception>
        public bool IsOne(IRing<ObjectType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                return ring.IsMultiplicativeUnity(this.realPart) &&
                    ring.IsAdditiveUnity(this.imaginaryPart);
            }
        }

        /// <summary>
        /// Obtém a soma do número complexo actual com outro número complexo.
        /// </summary>
        /// <param name="right">O outro número complexo.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O resultado da soma.</returns>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
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
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
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
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
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

        /// <summary>
        /// Divide um número complexo corrente por outro.
        /// </summary>
        /// <param name="right">O número a ser dividido.</param>
        /// <param name="field">O corpo responsável pelas operações.</param>
        /// <returns>O resultado da divisão.</returns>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        /// <exception cref="DivideByZeroException">Se ocorrer uma divisão por zero.</exception>
        public ComplexNumber<ObjectType> Divide(ComplexNumber<ObjectType> right, IField<ObjectType> field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (right.IsZero(field))
            {
                throw new DivideByZeroException("Can't divide by the zero complex number.");
            }
            else
            {
                var quadReal = field.Multiply(right.realPart, right.realPart);
                var quadImaginary = field.Multiply(right.imaginaryPart, right.imaginaryPart);
                var denom = field.Add(quadReal, quadImaginary);

                var resReal = field.Multiply(this.realPart, right.realPart);
                resReal = field.Add(
                    resReal,
                    field.Multiply(this.imaginaryPart, right.imaginaryPart));
                resReal = field.Multiply(
                    resReal,
                    field.MultiplicativeInverse(denom));
                var resImg = field.Multiply(this.realPart, right.imaginaryPart);
                resImg = field.AdditiveInverse(resImg);
                resImg = field.Add(
                    resImg,
                    field.Multiply(this.imaginaryPart, right.realPart));
                resImg = field.Multiply(
                    resImg,
                    field.MultiplicativeInverse(denom));

                var result = new ComplexNumber<ObjectType>();
                result.realPart = resReal;
                result.imaginaryPart = resImg;
                return result;
            }
        }

        /// <summary>
        /// Obtém o complexo conjugado do número actual.
        /// </summary>
        /// <param name="group">O grupo responsável pelas operações.</param>
        /// <returns>O complexo conjugado do número actual.</returns>
        /// <exception cref="ArgumentNullException">Se o grupo for nulo.</exception>
        public ComplexNumber<ObjectType> Conjugate(IGroup<ObjectType> group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            else
            {
                var result = new ComplexNumber<ObjectType>();
                result.realPart = this.realPart;
                result.imaginaryPart = group.AdditiveInverse(this.imaginaryPart);
                return result;
            }
        }

        /// <summary>
        /// Obtém um representação textual do número complexo.
        /// </summary>
        /// <returns>A representação textual do número complexo.</returns>
        public override string ToString()
        {
            return string.Format("[{0}] + (i)[{1}]", this.realPart, this.imaginaryPart);
        }
    }
}
