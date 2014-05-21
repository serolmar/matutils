namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa uma definição de escopo.
    /// </summary>
    class ScopeDefinition
    {
        /// <summary>
        /// O tipo de escopo.
        /// </summary>
        private EScopeType scopeType;

        /// <summary>
        /// O memorizador para o escopo.
        /// </summary>
        private IMemento scopeStartMemento;

        /// <summary>
        /// Valor que indica se o escopo contém chavetas.
        /// </summary>
        private bool hasBraces;

        /// <summary>
        /// Valor que indica se o interpretador estará em modo de execução no escopo.
        /// </summary>
        private bool executeInScope;

        /// <summary>
        /// A expressão de inicialização.
        /// </summary>
        private AMathematicsFunctionObject initializationExpression;

        /// <summary>
        /// A expressão de actualização.
        /// </summary>
        private AMathematicsFunctionObject updateExpression;

        /// <summary>
        /// A expressão de condição.
        /// </summary>
        private BooleanConditionMathematicsObject conditionExpression;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ScopeDefinition"/>.
        /// </summary>
        public ScopeDefinition()
        {
        }

        /// <summary>
        /// Otbém ou atribui o tipo do escopo.
        /// </summary>
        /// <value>O tipo do escopo.</value>
        public EScopeType ScopeType
        {
            get
            {
                return this.scopeType;
            }
            set
            {
                this.scopeType = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o memorizador de início.
        /// </summary>
        /// <value>O memorizador de início.</value>
        public IMemento ScopeStartMemento
        {
            get
            {
                return this.scopeStartMemento;
            }
            set
            {
                this.scopeStartMemento = value;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o escopo é delimitado por chavetas.
        /// </summary>
        /// <value>Verdadeiro caso o escopo seja delimitado por chavetas e falso caso contrário.</value>
        public bool HasBraces
        {
            get
            {
                return this.hasBraces;
            }
            set
            {
                this.hasBraces = value;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o interpretador está em modo de execução no escopo.
        /// </summary>
        /// <value>
        /// Verdadeiro caso o interpretador esteja em modo de execução no escopor e falso caso contrário.
        /// </value>
        public bool ExecuteInScope
        {
            get
            {
                return this.executeInScope;
            }
            set
            {
                this.executeInScope = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a expressão de inicialização.
        /// </summary>
        /// <remarks>No caso de ciclos "para".</remarks>
        /// <value>A expressão de inicialização.</value>
        public AMathematicsFunctionObject InitializationExpression
        {
            get
            {
                return this.initializationExpression;
            }
            set
            {
                this.initializationExpression = value;
            }
        }

        /// <summary>
        /// Obtém e atribui a expressão de actualização.
        /// </summary>
        /// <remarks>No caso de ciclos "para".</remarks>
        /// <value>A expressão de actualização.</value>
        public AMathematicsFunctionObject UpdateExpression
        {
            get
            {
                return this.updateExpression;
            }
            set
            {
                this.updateExpression = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a expressão de condição.
        /// </summary>
        /// <value>A expressão de condição.</value>
        public BooleanConditionMathematicsObject ConditionExpression
        {
            get
            {
                return this.conditionExpression;
            }
            set
            {
                this.conditionExpression = value;
            }
        }
    }
}
