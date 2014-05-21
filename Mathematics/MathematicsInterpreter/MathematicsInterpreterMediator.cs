namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um mediador para o interpretador.
    /// </summary>
    class MathematicsInterpreterMediator
    {
        /// <summary>
        /// As atribuições de variáveis.
        /// </summary>
        private Dictionary<NameMathematicsObject, AMathematicsObject> assignements = 
            new Dictionary<NameMathematicsObject, AMathematicsObject>();

        /// <summary>
        /// Determina se um nome está atribuído.
        /// </summary>
        /// <param name="name">O nome.</param>
        /// <returns>Verdadeiro caso o nome esteja atribuído e falso caso contrário.</returns>
        public bool IsAssigned(NameMathematicsObject name)
        {
            return this.assignements.ContainsKey(name);
        }

        /// <summary>
        /// Atribui o nome.
        /// </summary>
        /// <param name="name">O nome.</param>
        /// <param name="value">O valor.</param>
        /// <exception cref="ExpressionInterpreterException">Se a atribuição for recursiva.</exception>
        public void Assign(NameMathematicsObject name, AMathematicsObject value)
        {
            if (this.IsAssignementRecursive(name, value))
            {
                throw new ExpressionInterpreterException("Recursive assignment.");
            }
            else
            {
                if (this.assignements.ContainsKey(name))
                {
                    this.assignements[name] = value;
                }
                else
                {
                    this.assignements.Add(name, value);
                }
            }
        }

        /// <summary>
        /// Liberta o nome de alguma atribuição.
        /// </summary>
        /// <param name="name">O nome.</param>
        public void Unassign(NameMathematicsObject name)
        {
            this.assignements.Remove(name);
        }

        /// <summary>
        /// Tenta obter o valor atribuído ao nome.
        /// </summary>
        /// <param name="name">O nome.</param>
        /// <param name="value">A referência que recebe o valor.</param>
        /// <returns>Veradeiro caso o nome esteja atribuído e falso caso contrário.</returns>
        public bool TryGetValue(NameMathematicsObject name, out AMathematicsObject value)
        {
            return this.assignements.TryGetValue(name, out value);
        }

        /// <summary>
        /// Averigua se uma atribuição é recursiva.
        /// </summary>
        /// <param name="name">O nome da atribuição.</param>
        /// <param name="value">O valor da atribuição.</param>
        /// <returns>Verdadeiro se a atribuição é recursiva e falso caso contrário.</returns>
        private bool IsAssignementRecursive(NameMathematicsObject name, AMathematicsObject value)
        {
            if (value.MathematicsType == EMathematicsType.INTEGER_VALUE ||
                value.MathematicsType == EMathematicsType.DOUBLE_VALUE ||
                value.MathematicsType == EMathematicsType.BOOLEAN_VALUE)
            {
                return false;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
