namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite definir o quociente e o resto de uma operação de divisão.
    /// </summary>
    /// <typeparam name="ObjectType">
    /// O tipo de objectos sobre os quais é efectuada a operação de divisão.
    /// </typeparam>
    public class DomainResult<ObjectType>
    {
        /// <summary>
        /// O quociente.
        /// </summary>
        private ObjectType quotient;

        /// <summary>
        /// O resto.
        /// </summary>
        private ObjectType remainder;

        /// <summary>
        /// Cria uma instância de um objecto do tipo <see cref="DomainResult{ObjectType}."/>
        /// </summary>
        /// <param name="quotient">O quociente.</param>
        /// <param name="remainder">O resto.</param>
        public DomainResult(ObjectType quotient, ObjectType remainder)
        {
            this.quotient = quotient;
            this.remainder = remainder;
        }

        /// <summary>
        /// Obtém o quociente.
        /// </summary>
        /// <value>
        /// O quociente.
        /// </value>
        public ObjectType Quotient
        {
            get
            {
                return this.quotient;
            }
        }

        /// <summary>
        /// Obtém o resto.
        /// </summary>
        /// <value>
        /// O resto.
        /// </value>
        public ObjectType Remainder
        {
            get
            {
                return this.remainder;
            }
        }
    }
}
