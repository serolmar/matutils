namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma atribuição.
    /// </summary>
    class AssignMathematicsObject : AMathematicsFunctionObject
    {
        /// <summary>
        /// O objecto a ser atribuído.
        /// </summary>
        private AMathematicsObject leftObject;

        /// <summary>
        /// O objecto a atribuir.
        /// </summary>
        private AMathematicsObject rightObject;

        /// <summary>
        /// O mediador.
        /// </summary>
        private MathematicsInterpreterMediator mediator;

        /// <summary>
        /// Instancia um novo objecto do tipo<see cref="AssignMathematicsObject"/>.
        /// </summary>
        /// <param name="leftObject">O objecto a ser atribuído.</param>
        /// <param name="rightObject">O objecto a atribuir.</param>
        /// <param name="mediator">O mediador.</param>
        /// <exception cref="ExpressionInterpreterException">
        /// Se o objecto atribuído for nulo.
        /// </exception>
        public AssignMathematicsObject(
            AMathematicsObject leftObject, 
            AMathematicsObject rightObject, 
            MathematicsInterpreterMediator mediator)
            : base(EMathematicsType.ASSIGN)
        {
            if (leftObject == null)
            {
                throw new ExpressionInterpreterException("Left object must be non null within an assignement.");
            }
            else
            {
                this.leftObject = leftObject;
                this.rightObject = rightObject;
                this.mediator = mediator;
            }
        }

        /// <summary>
        /// Obtém o obecto a ser atribuído.
        /// </summary>
        /// <value>
        /// O objecto.
        /// </value>
        public AMathematicsObject LeftObject
        {
            get
            {
                return this.leftObject;
            }
        }

        /// <summary>
        /// Obtém o objecto a atribuir.
        /// </summary>
        /// <value>O objecto.</value>
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
            return this.mathematicsType.Equals(mathematicsType);
        }

        /// <summary>
        /// Obtém um objecto que resulta da conversão da instância corrente.
        /// </summary>
        /// <param name="mathematicsType">O tipo de objecto no qual se pretende converter.</param>
        /// <returns>
        /// O objecto convertido.
        /// </returns>
        /// <exception cref="ExpressionInterpreterException">Se a conversão não for possível.</exception>
        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            if (!mathematicsType.Equals(this.mathematicsType))
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Avalia todas as atribuições.
        /// </summary>
        public override void Evaulate()
        {
            this.mediator.Assign(this.leftObject as NameMathematicsObject, this.rightObject);
        }

        /// <summary>
        /// Constrói a representação textual da atribuição.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return string.Format("{0} = {1}", this.leftObject.ToString(), this.rightObject.ToString());
        }
    }
}
