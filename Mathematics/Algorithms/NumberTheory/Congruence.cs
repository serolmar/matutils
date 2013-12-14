namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma congruência onde 
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objecto que representa a congruência.</typeparam>
    public class Congruence<ObjectType>
    {
        /// <summary>
        /// O módulo da congruência.
        /// </summary>
        private ObjectType modulus;

        /// <summary>
        /// O valor associado à congruência.
        /// </summary>
        private ObjectType value;

        public Congruence(ObjectType modulus, ObjectType congruence)
        {
            if (modulus == null)
            {
                throw new ArgumentNullException("modulus");
            }
            else if (congruence == null)
            {
                throw new ArgumentNullException("congruence");
            }
            else
            {
                this.modulus = modulus;
                this.value = congruence;
            }
        }

        public ObjectType Modulus
        {
            get
            {
                return this.modulus;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The modulus value can't be null.");
                }
                else
                {
                    this.modulus = value;
                }
            }
        }

        public ObjectType Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The value can't be null.");
                }
                else
                {
                    this.value = value;
                }
            }
        }

        /// <summary>
        /// Obtém a congruência reduzida dado o domínio.
        /// </summary>
        /// <param name="domain">O domínio.</param>
        /// <returns>A congruência reduzida.</returns>
        public Congruence<ObjectType> GetReduced(IEuclidenDomain<ObjectType> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                var reduced = domain.Rem(this.value, this.modulus);
                return new Congruence<ObjectType>(this.modulus, reduced);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} mod {1}", this.value, this.modulus);
        }
    }
}
