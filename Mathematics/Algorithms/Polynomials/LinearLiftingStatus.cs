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
        /// Contém o valor que indica se o objecto já foi inicializado.
        /// </summary>
        private bool notInitialized;

        /// <summary>
        /// Valor que indica se os factores encontrados constituem uma solução inteira.
        /// </summary>
        private bool foundSolution;

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
                this.notInitialized = true;
            }
        }

        internal LinearLiftingStatus(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            CoeffType liftFactorizationModule)
        {
            this.polynom = polynom;
            this.notInitialized = true;
            this.liftFactorizationModule = liftFactorizationModule;
        }

        /// <summary>
        /// Obtém o polinómio sobre o qual pretendemos executar o levantamento.
        /// </summary>
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
        internal UnivariatePolynomialNormalForm<CoeffType> UFactor
        {
            get
            {
                return this.uFactor;
            }
            set
            {
                this.uFactor = value;
            }
        }

        /// <summary>
        /// Obtém o valor de w(x).
        /// </summary>
        internal UnivariatePolynomialNormalForm<CoeffType> WFactor
        {
            get
            {
                return this.wFactor;
            }
            set
            {
                this.wFactor = value;
            }
        }

        /// <summary>
        /// Obtém e atribui o valor que indica se o objecto já foi inicializado.
        /// </summary>
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

        public override string ToString()
        {
            return this.polynom.ToString();
        }
    }
}
