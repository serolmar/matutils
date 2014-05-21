namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um nome.
    /// </summary>
    class NameMathematicsObject : AMathematicsObject
    {
        /// <summary>
        /// O mediador.
        /// </summary>
        private MathematicsInterpreterMediator mediator;

        /// <summary>
        /// O nome.
        /// </summary>
        private string name;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="NameMathematicsObject"/>.
        /// </summary>
        /// <param name="name">O nome.</param>
        /// <param name="mediator">O mediador.</param>
        /// <exception cref="ExpressionInterpreterException">Se o nome for nulo ou vazio.</exception>
        public NameMathematicsObject(string name, MathematicsInterpreterMediator mediator) 
            : base(EMathematicsType.NAME, false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ExpressionInterpreterException("Empty names aren't allowed.");
            }

            this.name = name;
            this.mediator = mediator;
        }

        /// <summary>
        /// Obtém o tipo do objecto.
        /// </summary>
        /// <value>
        /// O tipo do objecto.
        /// </value>
        public override EMathematicsType MathematicsType
        {
            get
            {
                return this.mathematicsType;
            }
        }

        /// <summary>
        /// Obtém o nome.
        /// </summary>
        /// <value>
        /// O nome.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Obt+em o valor associado ao nome.
        /// </summary>
        /// <value>
        /// O valor.
        /// </value>
        public AMathematicsObject Value
        {
            get
            {
                AMathematicsObject value = null;
                if (this.mediator.TryGetValue(this, out value))
                {
                    return value;
                }
                else
                {
                    return this;
                }
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
            if (mathematicsType == EMathematicsType.NAME || mathematicsType == EMathematicsType.POLYNOMIAL)
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
        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="ExpressionInterpreterException"></exception>
        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            if (mathematicsType == EMathematicsType.NAME)
            {
                return this;
            }
            else if (mathematicsType == EMathematicsType.POLYNOMIAL)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
        }

        /// <summary>
        /// Constrói uma representação textual da instância corrente.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return this.name;
        }
    }
}
