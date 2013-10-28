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
        /// A representação interna do polinómio após a inicialização.
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> innerPolynom;

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
        /// Contém o valor de u1(x) após a inicialização.
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> innerU1Factor;

        /// <summary>
        /// Contém o valor de w1(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> w1Factor;

        /// <summary>
        /// Contém o valor de w1(x) após a inicialização.
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> innerW1Factor;

        /// <summary>
        /// Contém o valor de s(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> sPol;

        /// <summary>
        /// Contém o valor de t(x).
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> tPol;

        /// <summary>
        /// O corpo modular no qual é conhecida a factorização a levantar.
        /// </summary>
        private IModularField<CoeffType> modularField;

        /// <summary>
        /// O domínio responsável pela determinação de quocientes e restos sobre os coeficentes.
        /// </summary>
        private IEuclidenDomain<CoeffType> mainDomain;

        /// <summary>
        /// Coném o módulo associado à solução actual.
        /// </summary>
        private CoeffType liftFactorizationModule;

        /// <summary>
        /// Contém o valor de gama.
        /// </summary>
        private CoeffType gamma;

        /// <summary>
        /// Contém o valor que indica se o objecto já foi inicializado.
        /// </summary>
        private bool notInitialized;

        public LinearLiftingStatus(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            UnivariatePolynomialNormalForm<CoeffType> firstFactor,
            UnivariatePolynomialNormalForm<CoeffType> secondFactor,
            IModularField<CoeffType> modularField,
            IEuclidenDomain<CoeffType> mainDomain)
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
            else if (modularField == null)
            {
                throw new ArgumentNullException("modularField");
            }
            else if (mainDomain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                this.polynom = polynom;
                this.u1Factor = firstFactor;
                this.w1Factor = secondFactor;
                this.modularField = modularField;
                this.mainDomain = mainDomain;
                this.liftFactorizationModule = modularField.Module;
                this.notInitialized = true;
            }
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
        /// Obtém o valor de u(x).
        /// </summary>
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
        /// Obtém o valor de s(x).
        /// </summary>
        public UnivariatePolynomialNormalForm<CoeffType> SPol
        {
            get
            {
                return this.sPol;
            }
            internal set
            {
                this.sPol = value;
            }
        }

        /// <summary>
        /// Obtém o valor de t(x).
        /// </summary>
        public UnivariatePolynomialNormalForm<CoeffType> TPol
        {
            get
            {
                return this.tPol;
            }
            internal set
            {
                this.tPol = value;
            }
        }

        /// <summary>
        /// Obtém o corpo modular no qual é conhecida a factorização a levantar.
        /// </summary>
        public IModularField<CoeffType> ModularField
        {
            get
            {
                return this.modularField;
            }
        }

        /// <summary>
        /// Obtém o domínio responsável pela determinação de quocientes e restos sobre os coeficentes.
        /// </summary>
        public IEuclidenDomain<CoeffType> MainDomain
        {
            get
            {
                return this.mainDomain;
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
        /// Obtém e atribui o polinómio obtido após a inicialização.
        /// </summary>
        internal UnivariatePolynomialNormalForm<CoeffType> InnerPolynom
        {
            get
            {
                return this.innerPolynom;
            }
            set
            {
                this.innerPolynom = value;
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
        /// Obtém e atribui o polinómio u1(x) apóes a inicialização.
        /// </summary>
        internal UnivariatePolynomialNormalForm<CoeffType> InnerU1Factor
        {
            get
            {
                return this.innerU1Factor;
            }
            set
            {
                this.innerU1Factor = value;
            }
        }

        /// <summary>
        /// Obtém e atribui o polinómio w1(x) após a inicialização.
        /// </summary>
        internal UnivariatePolynomialNormalForm<CoeffType> InnerW1Factor
        {
            get
            {
                return this.innerW1Factor;
            }
            set
            {
                this.innerW1Factor = value;
            }
        }

        /// <summary>
        /// Obtém o valor de gama.
        /// </summary>
        internal CoeffType Gamma
        {
            get
            {
                return this.gamma;
            }
            set
            {
                this.gamma = value;
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

        /// <summary>
        /// Obtém a solução a partir do objecto de estado.
        /// </summary>
        /// <returns>O par ordenado com a respectiva factorização.</returns>
        public Tuple<UnivariatePolynomialNormalForm<CoeffType>, UnivariatePolynomialNormalForm<CoeffType>> GetSolution()
        {
            if (this.notInitialized)
            {
                return Tuple.Create(this.u1Factor, this.w1Factor);
            }
            else
            {
                var delta = this.uFactor.GetContent(this.mainDomain);
                var uResultFactor = this.uFactor.ApplyQuo(delta, this.mainDomain);
                var quo = this.mainDomain.Quo(this.gamma, delta);
                var wResultFactor = this.wFactor.ApplyQuo(quo, this.mainDomain);
                return Tuple.Create(uFactor, wFactor);
            }
        }
    }
}
