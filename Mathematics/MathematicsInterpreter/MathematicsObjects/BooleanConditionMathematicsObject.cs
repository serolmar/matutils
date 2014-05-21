namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Respresenta uma condição lógica binária.
    /// </summary>
    class BooleanConditionMathematicsObject : AMathematicsConditionObject
    {
        /// <summary>
        /// The left condition object.
        /// </summary>
        private AMathematicsObject leftObject;

        /// <summary>
        /// The right condition object.
        /// </summary>
        private AMathematicsObject rightObject;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BooleanConditionMathematicsObject"/>.
        /// </summary>
        /// <param name="leftObject">O primeiro membro da condição.</param>
        /// <param name="rightObject">O segundo membro da condição.</param>
        /// <param name="conditionType">O tipo da condição.</param>
        /// <exception cref="ExpressionInterpreterException">Se a condição não for binária.</exception>
        public BooleanConditionMathematicsObject(
            AMathematicsObject leftObject, 
            AMathematicsObject rightObject, 
            EMathematicsConditionType conditionType)
            : base(conditionType, EMathematicsType.CONDITION, false)
        {
            if (conditionType == EMathematicsConditionType.BOOLEAN_VALUE)
            {
                throw new ExpressionInterpreterException("Condition must be binary.");
            }

            this.leftObject = leftObject;
            this.rightObject = rightObject;
        }

        /// <summary>
        /// Obtém o primeiro membro.
        /// </summary>
        /// <value>
        /// O primeiro membro.
        /// </value>
        public AMathematicsObject LeftObject
        {
            get
            {
                return this.leftObject;
            }
        }

        /// <summary>
        /// Obtém o segundo membro.
        /// </summary>
        /// <value>
        /// O segundo membro.
        /// </value>
        public AMathematicsObject RightObject
        {
            get
            {
                return this.rightObject;
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
            if (this.mathematicsType.Equals(mathematicsType))
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
            if (!this.mathematicsType.Equals(mathematicsType))
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
            else
            {
                return this;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constrói uma representação textual da condição.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            var conditionSymbol = string.Empty;
            if (this.mathematicsConditionType == EMathematicsConditionType.EQUAL)
            {
                conditionSymbol = "==";
            }
            else if (this.mathematicsConditionType == EMathematicsConditionType.GREAT_OR_EQUAL_THAN)
            {
                conditionSymbol = ">=";
            }
            else if (this.mathematicsConditionType == EMathematicsConditionType.GREAT_THAN)
            {
                conditionSymbol = ">";
            }
            else if (this.mathematicsConditionType == EMathematicsConditionType.LESS_OR_EQUAL_THAN)
            {
                conditionSymbol = "<=";
            }
            else if (this.mathematicsConditionType == EMathematicsConditionType.LESS_THAN)
            {
                conditionSymbol = "<";
            }
            else
            {
                return "Invalid condition";
            }

            return string.Format("{0} {1} {2}", this.leftObject.ToString(), conditionSymbol, this.rightObject.ToString());
        }
    }
}
