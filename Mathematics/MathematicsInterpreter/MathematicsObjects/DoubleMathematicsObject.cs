namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um número de precisão dupla.
    /// </summary>
    class DoubleMathematicsObject : AMathematicsObject
    {
        /// <summary>
        /// O valor.
        /// </summary>
        private double value;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="DoubleMathematicsObject"/>.
        /// </summary>
        public DoubleMathematicsObject()
            : base(EMathematicsType.DOUBLE_VALUE, false)
        {
        }

        /// <summary>
        /// Obtém e atribui o valor.
        /// </summary>
        /// <value>O valor.</value>
        public double Value
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
        /// Constrói uma representação textual do número.
        /// </summary>
        /// <returns>A representação extual.</returns>
        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
