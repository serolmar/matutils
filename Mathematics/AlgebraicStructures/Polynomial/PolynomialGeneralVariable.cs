﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures.Polynomial
{
    class PolynomialGeneralVariable<T,R>
        where R : IRing<T>
    {
        private Polynomial<T,R> internalPolynomial = null;

        private string variable = null;

        private PolynomialGeneralVariable()
        {
        }

        public PolynomialGeneralVariable(string variable)
        {
            if (string.IsNullOrEmpty(variable))
            {
                throw new MathematicsException("Parameter variable can't be null.");
            }

            this.variable = variable;
        }

        public PolynomialGeneralVariable(Polynomial<T, R> polynomial)
        {
            if (polynomial == null)
            {
                throw new MathematicsException("Parameter polynomial can't be null.");
            }

            this.internalPolynomial = polynomial;
        }

        public bool IsVariable
        {
            get
            {
                return !string.IsNullOrEmpty(variable);
            }
        }

        public bool IsPolynomial
        {
            get
            {
                return this.internalPolynomial != null;
            }
        }

        public string GetVariable()
        {
            if (this.variable == null)
            {
                throw new MathematicsException("Polynomail general variable isn't simple.");
            }

            return this.variable;
        }

        public Polynomial<T, R> GetPolynomial()
        {
            if (this.internalPolynomial == null)
            {
                throw new MathematicsException("Polynomial general variable is simple.");
            }

            return this.internalPolynomial;
        }

        public PolynomialGeneralVariable<T, R> Clone()
        {
            var result = new PolynomialGeneralVariable<T, R>();
            if (this.internalPolynomial != null)
            {
                result.internalPolynomial = this.internalPolynomial.Clone();
            }
            else
            {
                result.variable = this.variable;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var innerObject = obj as PolynomialGeneralVariable<T, R>;
            if (innerObject == null)
            {
                return false;
            }

            if (this.variable != null && innerObject.variable != null)
            {
                return this.variable == innerObject.variable;
            }

            if (this.internalPolynomial != null && innerObject.internalPolynomial != null)
            {
                return this.internalPolynomial.Equals(innerObject.internalPolynomial);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.variable != null)
            {
                return this.variable.GetHashCode();
            }

            if (this.internalPolynomial != null)
            {
                return this.internalPolynomial.GetHashCode();
            }

            return 0;
        }

        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            if (this.variable != null)
            {
                resultBuilder.Append(this.variable);
            }
            else if (this.internalPolynomial != null)
            {
                if (!this.internalPolynomial.IsVariable && !this.internalPolynomial.IsValue)
                {
                    resultBuilder.Append("(");
                    resultBuilder.Append(this.internalPolynomial.ToString());
                    resultBuilder.Append(")");
                }
                else
                {
                    resultBuilder.Append(this.internalPolynomial.ToString());
                }
            }

            return resultBuilder.ToString();
        }
    }
}
