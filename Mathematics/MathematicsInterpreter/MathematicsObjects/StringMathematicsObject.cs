namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;

    /// <summary>
    /// Representa um valor textual.
    /// </summary>
    class StringMathematicsObject : AMathematicsObject
    {
        /// <summary>
        /// O valor textual.
        /// </summary>
        private string value;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="StringMathematicsObject"/>.
        /// </summary>
        public StringMathematicsObject()
            : base(EMathematicsType.STRING_VALUE, false)
        {
        }

        /// <summary>
        /// Obtém ou atribui o valor.
        /// </summary>
        /// <value>O valor.</value>
        public string Value
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
            if (mathematicsType == EMathematicsType.STRING_VALUE)
            {
                return true;
            }
            else if (mathematicsType == EMathematicsType.INTEGER_VALUE)
            {
                var temp = 0;
                if (int.TryParse(this.value, out temp))
                {
                    return true;
                }
            }
            else if (mathematicsType == EMathematicsType.ASSIGN)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.BOOLEAN_VALUE)
            {
                var temp = true;
                if (bool.TryParse(this.value, out temp))
                {
                    return true;
                }
            }
            else if (mathematicsType == EMathematicsType.CONDITION)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.DOUBLE_VALUE)
            {
                var temp = 0.0;
                if (double.TryParse(this.value, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out temp))
                {
                    return true;
                }
            }
            else if (mathematicsType == EMathematicsType.EMPTY)
            {
                if (string.IsNullOrWhiteSpace(this.value))
                {
                    return true;
                }
            }
            else if (mathematicsType == EMathematicsType.LIST)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.NAME)
            {
            }
            else if (mathematicsType == EMathematicsType.POLYNOMIAL)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.RANGE)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.SET)
            {
                throw new NotImplementedException();
            }

            return false;
        }

        /// <summary>
        /// Obtém um objecto que resulta da conversão da instância corrente.
        /// </summary>
        /// <param name="mathematicsType">O tipo de objecto no qual se pretende converter.</param>
        /// <returns>
        /// O objecto convertido.
        /// </returns>
        /// <exception cref="ExpressionInterpreterException"></exception>
        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            if (mathematicsType == EMathematicsType.STRING_VALUE)
            {
                return this;
            }
            else if (mathematicsType == EMathematicsType.INTEGER_VALUE)
            {
                var temp = 0;
                if (int.TryParse(this.value, out temp))
                {
                    return new IntegerMathematicsObject() { Value = temp };
                }
            }
            else if (mathematicsType == EMathematicsType.ASSIGN)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.BOOLEAN_VALUE)
            {
                var temp = true;
                if (bool.TryParse(this.value, out temp))
                {
                    return new BooleanMathematicsObject() { Value = temp };
                }
            }
            else if (mathematicsType == EMathematicsType.CONDITION)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.DOUBLE_VALUE)
            {
                var temp = 0.0;
                if (double.TryParse(this.value, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out temp))
                {
                    return new DoubleMathematicsObject() { Value = temp };
                }
            }
            else if (mathematicsType == EMathematicsType.EMPTY)
            {
                if (string.IsNullOrWhiteSpace(this.value))
                {
                    return new EmptyMathematicsObject();
                }
            }
            else if (mathematicsType == EMathematicsType.LIST)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.NAME)
            {
            }
            else if (mathematicsType == EMathematicsType.POLYNOMIAL)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.RANGE)
            {
                throw new NotImplementedException();
            }
            else if (mathematicsType == EMathematicsType.SET)
            {
                throw new NotImplementedException();
            }

            throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
        }
    }
}
