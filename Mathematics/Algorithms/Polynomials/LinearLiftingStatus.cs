namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Contém o estado da aplicação do algoritmo do levantamento.
    /// </summary>
    /// <remarks>
    /// A notação é semelhante à que se encontra no livro "Algorithms for Computer Algebra".
    /// </remarks>
    /// <typeparam name="CoeffType">O tipo de dados dos coeficientes.</typeparam>
    public class LinearLiftingStatus<CoeffType>
    {
        /// <summary>
        /// O polinómio sobre o qual pretendemos executar o levantamento.
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> polynom;

        /// <summary>
        /// Contém o valor de e(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> ePol;

        /// <summary>
        /// Contém o valor de u(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> uFactor;

        /// <summary>
        /// Contém o valor de w(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> wFactor;

        /// <summary>
        /// Contém o valor de u1(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> u1Factor;

        /// <summary>
        /// Contém o valor de w1(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> w1Factor;

        /// <summary>
        /// Contém o valor de s(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> sPol;

        /// <summary>
        /// Contém o valor de t(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> tPol;

        /// <summary>
        /// Coném o módulo associado à solução actual.
        /// </summary>
        private CoeffType liftFactorizationModule;

        /// <summary>
        /// O módulo inicializado que permite controlar as iterações.
        /// </summary>
        private CoeffType initializedFactorizationModulus;

        /// <summary>
        /// Contém o valor que indica se o objecto já foi inicializado.
        /// </summary>
        private bool notInitialized;

        /// <summary>
        /// Valor que indica se os factores encontrados constituem uma solução inteira.
        /// </summary>
        private bool foundSolution;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LinearLiftingStatus{CoeffType}"/>.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <param name="firstFactor">O primeiro factor.</param>
        /// <param name="secondFactor">O segundo factor.</param>
        /// <param name="liftFactorizationModule">O módulo para a elevação.</param>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        public LinearLiftingStatus(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            UnivariatePolynomialNormalForm<CoeffType> firstFactor,
            UnivariatePolynomialNormalForm<CoeffType> secondFactor,
            CoeffType liftFactorizationModule)
        {
            if (polynom == null)
            {
                throw new ArgumentNullException("polynom");
            }
            else if (firstFactor == null)
            {
                throw new ArgumentNullException("firstFactor");
            }
            else if (secondFactor == null)
            {
                throw new ArgumentNullException("secondFactor");
            }
            else if (liftFactorizationModule == null)
            {
                throw new ArgumentNullException("liftFactorizationModule");
            }
            else
            {
                this.polynom = polynom;
                this.u1Factor = firstFactor;
                this.w1Factor = secondFactor;
                this.liftFactorizationModule = liftFactorizationModule;
                this.initializedFactorizationModulus = liftFactorizationModule;
                this.notInitialized = true;
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LinearLiftingStatus{CoeffType}"/>.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <param name="liftFactorizationModule">O módulo para a elevação.</param>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        internal LinearLiftingStatus(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            CoeffType liftFactorizationModule)
        {
            this.polynom = polynom;
            this.notInitialized = true;
            this.liftFactorizationModule = liftFactorizationModule;
            this.initializedFactorizationModulus = liftFactorizationModule;
        }

        /// <summary>
        /// Obtém o polinómio sobre o qual pretendemos executar o levantamento.
        /// </summary>
        /// <value>O polinómio.</value>
        public UnivariatePolynomialNormalForm<CoeffType> Polynom
        {
            get
            {
                return this.polynom;
            }
            internal set
            {
                this.polynom = value;
            }
        }

        /// <summary>
        /// Obtém o valor de u1(x).
        /// </summary>
        /// <value>O valor.</value>
        public UnivariatePolynomialNormalForm<CoeffType> U1Factor
        {
            get
            {
                return this.u1Factor;
            }
            internal set
            {
                this.u1Factor = value;
            }
        }

        /// <summary>
        /// Obtém o valor de w1(x).
        /// </summary>
        /// <value>O valor.</value>
        public UnivariatePolynomialNormalForm<CoeffType> W1Factor
        {
            get
            {
                return this.w1Factor;
            }
            internal set
            {
                this.w1Factor = value;
            }
        }

        /// <summary>
        /// Obtém o valor que indica se os factores correspondem a uma solução real.
        /// </summary>
        /// <value>Verdadeiro caso os factores correpondam a uma solução real e falso caso contrário.</value>
        public bool FoundSolution
        {
            get
            {
                return this.foundSolution;
            }
            internal set
            {
                this.foundSolution = value;
            }
        }

        /// <summary>
        /// Obtém e atribui o módulo associado à solução actual.
        /// </summary>
        /// <value>O módulo.</value>
        public CoeffType LiftedFactorizationModule
        {
            get
            {
                return this.liftFactorizationModule;
            }
            internal set
            {
                this.liftFactorizationModule = value;
            }
        }

        /// <summary>
        /// Obtém o valor de e(x).
        /// </summary>
        /// <value>O valor.</value>
        internal UnivariatePolynomialNormalForm<CoeffType> EPol
        {
            get
            {
                return this.ePol;
            }
            set
            {
                this.ePol = value;
            }
        }

        /// <summary>
        /// Obtém o valor de s(x).
        /// </summary>
        /// <value>O valor.</value>
        internal UnivariatePolynomialNormalForm<CoeffType> SPol
        {
            get
            {
                return this.sPol;
            }
            set
            {
                this.sPol = value;
            }
        }

        /// <summary>
        /// Obtém o valor de t(x).
        /// </summary>
        /// <value>O valor.</value>
        internal UnivariatePolynomialNormalForm<CoeffType> TPol
        {
            get
            {
                return this.tPol;
            }
            set
            {
                this.tPol = value;
            }
        }

        /// <summary>
        /// Obtém o valor de u(x).
        /// </summary>
        /// <value>O valor.</value>
        public UnivariatePolynomialNormalForm<CoeffType> UFactor
        {
            get
            {
                return this.uFactor;
            }
            internal set
            {
                this.uFactor = value;
            }
        }

        /// <summary>
        /// Obtém o valor de w(x).
        /// </summary>
        /// <value>O valor.</value>
        public UnivariatePolynomialNormalForm<CoeffType> WFactor
        {
            get
            {
                return this.wFactor;
            }
            internal set
            {
                this.wFactor = value;
            }
        }

        /// <summary>
        /// Obtém e atribui o valor que indica se o objecto já foi inicializado.
        /// </summary>
        /// <value>Verdadeiro caso o objecto já tenha sido inicializado e falso caso contrário.</value>
        internal bool NotInitialized
        {
            get
            {
                return this.notInitialized;
            }
            set
            {
                this.notInitialized = value;
            }
        }

        /// <summary>
        /// O módulo inicializado que permite controlar as iterações.
        /// </summary>
        /// <value>O módulo.</value>
        internal CoeffType InitializedFactorizationModulus
        {
            get
            {
                return this.initializedFactorizationModulus;
            }
            set
            {
                this.initializedFactorizationModulus = value;
            }
        }

        /// <summary>
        /// Constrói uma representação textual do estado.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return this.polynom.ToString();
        }
    }
}
