namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um número inteiro.
    /// </summary>
    class IntegerMathematicsObject : AMathematicsObject
    {
        /// <summary>
        /// O valor.
        /// </summary>
        private int value;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="IntegerMathematicsObject"/>.
        /// </summary>
        public IntegerMathematicsObject()
            : base(EMathematicsType.INTEGER_VALUE, false)
        {
        }

        /// <summary>
        /// Obtém ou atribui o valor.
        /// </summary>
        /// <value>O valor.</value>
        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Determina se é possível converter a instância corrente no tipo especificado.
        /// </summary>
        /// <param name="mathematicsType">O tipo.</param>
        /// <returns>
        /// Verdadeiro caso seja possível converter a instância e falso caso contrário.
        /// </returns>
        public override bool CanConvertTo(EMathematicsType mathematicsType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um objecto que resulta da conversão da instância corrente.
        /// </summary>
        /// <param name="mathematicsType">O tipo de objecto no qual se pretende converter.</param>
        /// <returns>
        /// O objecto convertido.
        /// </returns>
        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constrói uma representação textual da instância corrente.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
