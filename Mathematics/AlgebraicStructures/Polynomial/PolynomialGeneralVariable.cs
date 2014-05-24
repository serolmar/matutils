namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma variável geral num polinómio.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem os coeficientes dos polinómios.</typeparam>
    class PolynomialGeneralVariable<T>
    {
        /// <summary>
        /// O polinómio interno.
        /// </summary>
        private Polynomial<T> internalPolynomial = null;

        /// <summary>
        /// A variável.
        /// </summary>
        private string variable = null;


        /// <summary>
        /// Inibe a criação da instância por defeito de objectos do tipo <see cref="PolynomialGeneralVariable{T}"/>.
        /// </summary>
        private PolynomialGeneralVariable()
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="PolynomialGeneralVariable{T}"/>.
        /// </summary>
        /// <param name="variable">A variável.</param>
        /// <exception cref="MathematicsException">Se a variável for nula ou vazia.</exception>
        public PolynomialGeneralVariable(string variable)
        {
            if (string.IsNullOrEmpty(variable))
            {
                throw new MathematicsException("Parameter variable can't be null.");
            }

            this.variable = variable;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="PolynomialGeneralVariable{T}"/>.
        /// </summary>
        /// <param name="polynomial">O polinómio.</param>
        /// <exception cref="MathematicsException">Se a o polinómio for nulo.</exception>
        public PolynomialGeneralVariable(Polynomial<T> polynomial)
        {
            if (polynomial == null)
            {
                throw new MathematicsException("Parameter polynomial can't be null.");
            }

            this.internalPolynomial = polynomial;
        }

        /// <summary>
        /// Obtém um valor que indica se a instância corrente representa uma variável.
        /// </summary>
        /// <value>
        /// Verdadeiro caso se trate de uma variável e falso caso contrário.
        /// </value>
        public bool IsVariable
        {
            get
            {
                return !string.IsNullOrEmpty(variable);
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a instância corrente representa um polinómio.
        /// </summary>
        /// <value>
        /// Verdadeiro caso se trate de um polinómio e falso caso contrário.
        /// </value>
        public bool IsPolynomial
        {
            get
            {
                return this.internalPolynomial != null;
            }
        }

        /// <summary>
        /// Obtém a variável.
        /// </summary>
        /// <returns>A variável.</returns>
        /// <exception cref="MathematicsException">Se a instância corrente não representar uma variável.</exception>
        public string GetVariable()
        {
            if (this.variable == null)
            {
                throw new MathematicsException("Polynomail general variable isn't simple.");
            }

            return this.variable;
        }

        /// <summary>
        /// Obtém o polinómio.
        /// </summary>
        /// <returns>O polinómio.</returns>
        /// <exception cref="MathematicsException">Se a instância corrente não representar um polinómio.</exception>
        public Polynomial<T> GetPolynomial()
        {
            if (this.internalPolynomial == null)
            {
                throw new MathematicsException("Polynomial general variable is simple.");
            }

            return this.internalPolynomial;
        }

        /// <summary>
        /// Cria um cópida da instância corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
        public PolynomialGeneralVariable<T> Clone()
        {
            var result = new PolynomialGeneralVariable<T>();
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

        /// <summary>
        /// Determina se o objecto especificado é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto a comparar.</param>
        /// <returns>
        /// Verdadeiro caso o objecto seja igual e falso caso contrário.
        /// </returns>
        public override bool Equals(object obj)
        {
            var innerObject = obj as PolynomialGeneralVariable<T>;
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

        /// <summary>
        /// Retorna um código confuso para a instância corrente.
        /// </summary>
        /// <returns>
        /// O código confuso da instância corrente que pode ser utilizado em alguns algoritmos.
        /// </returns>
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

        /// <summary>
        /// Obtém um representação textual da instância corrente.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            if (this.variable != null)
            {
                resultBuilder.Append(this.variable);
            }
            else if (this.internalPolynomial != null)
            {
                if (!this.internalPolynomial.IsValue)
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
