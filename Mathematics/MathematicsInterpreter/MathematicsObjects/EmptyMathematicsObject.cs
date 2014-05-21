namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um objecto vazio.
    /// </summary>
    class EmptyMathematicsObject : AMathematicsObject
    {
        /// <summary>
        /// Initializes a new instance of theInstancia um novo objecto do tiop <see cref="EmptyMathematicsObject"/>.
        /// </summary>
        public EmptyMathematicsObject()
            : base(EMathematicsType.EMPTY, false)
        {
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
            if (mathematicsType == this.mathematicsType)
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
        /// <exception cref="ExpressionInterpreterException">Se não for possível a conversão.</exception>
        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            if (mathematicsType == this.mathematicsType)
            {
                return this;
            }
            else
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
        }

        /// <summary>
        /// Constrói uma representação textual da instância.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return "Empty expression";
        }
    }
}
