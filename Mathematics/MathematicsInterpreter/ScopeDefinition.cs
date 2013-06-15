using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics.MathematicsInterpreter
{
    class ScopeDefinition
    {
        private EScopeType scopeType;

        private IMemento scopeStartMemento;

        private bool hasBraces;

        private bool executeInScope;

        private AMathematicsFunctionObject initializationExpression;

        private AMathematicsFunctionObject updateExpression;

        private BooleanConditionMathematicsObject conditionExpression;

        public ScopeDefinition()
        {
        }

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
