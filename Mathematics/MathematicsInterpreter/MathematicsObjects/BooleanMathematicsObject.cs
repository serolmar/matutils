namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Reprsenta um valor lógico.
    /// </summary>
    class BooleanMathematicsObject : AMathematicsConditionObject
    {
        /// <summary>
        /// O valor lógico.
        /// </summary>
        private bool value;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BooleanMathematicsObject"/>.
        /// </summary>
        public BooleanMathematicsObject()
            : base(EMathematicsConditionType.BOOLEAN_VALUE, EMathematicsType.BOOLEAN_VALUE, false)
        {
        }

        /// <summary>
        /// Obtém o valor lógico.
        /// </summary>
        /// <value>
        ///  O valor lógico.
        /// </value>
        public bool Value
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
            if (mathematicsType == EMathematicsType.BOOLEAN_VALUE || mathematicsType == EMathematicsType.CONDITION)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém um objecto que resulta da conversão da instância corrente.
        /// </summary>
        /// <param name="mathematicsType">O tipo de objecto no qual se pretende converter.</param>
        /// <returns>
        /// O objecto convertido.
        /// </returns>
        /// <exception cref="ExpressionInterpreterException">Se não for possível converter.</exception>
        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            if (mathematicsType == EMathematicsType.BOOLEAN_VALUE || mathematicsType == EMathematicsType.CONDITION)
            {
                return this;
            }
            else
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
        }

        /// <summary>
        /// Avalia a condição.
        /// </summary>
        /// <returns>
        /// O resultado da avaliação.
        /// </returns>
        public override bool AssertCondition()
        {
            return this.value;
        }

        /// <summary>
        /// Constrói uma representação textual do valor lógico.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
