namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa qualquer objecto matemático.
    /// </summary>
    public abstract class AMathematicsObject
    {
        /// <summary>
        /// O tipo associado ao objecto.
        /// </summary>
        protected EMathematicsType mathematicsType;

        /// <summary>
        /// Valor que determina se se trata de uma função.
        /// </summary>
        protected bool isFunctionObject;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="AMathematicsObject"/>.
        /// </summary>
        /// <param name="mathematicsType">O tipo.</param>
        /// <param name="isFunctionObject">Um valor que indica se se trata de uma função.</param>
        public AMathematicsObject(EMathematicsType mathematicsType, bool isFunctionObject)
        {
            this.mathematicsType = mathematicsType;
            this.isFunctionObject = isFunctionObject;
        }

        /// <summary>
        /// Obtém o tipo do objecto.
        /// </summary>
        /// <value>
        /// O tipo do objecto.
        /// </value>
        public virtual EMathematicsType MathematicsType
        {
            get
            {
                return this.mathematicsType;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a instância corrente representa uma função.
        /// </summary>
        /// <value>
        /// Verdadeiro caso a instância corrente seja uma função e falso caso contrário.
        /// </value>
        public virtual bool IsFunctionObject
        {
            get
            {
                return this.isFunctionObject;
            }
        }

        /// <summary>
        /// Determina se é possível converter a instância corrente no tipo especificado.
        /// </summary>
        /// <param name="mathematicsType">O tipo.</param>
        /// <returns>Verdadeiro caso seja possível converter a instância e falso caso contrário.</returns>
        public abstract bool CanConvertTo(EMathematicsType mathematicsType);

        /// <summary>
        /// Obtém um objecto que resulta da conversão da instância corrente.
        /// </summary>
        /// <param name="mathematicsType">O tipo de objecto no qual se pretende converter.</param>
        /// <returns>O objecto convertido.</returns>
        public abstract AMathematicsObject ConvertTo(EMathematicsType mathematicsType);
    }
}
