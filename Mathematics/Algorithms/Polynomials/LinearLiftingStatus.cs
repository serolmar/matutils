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
        /// O corpo modular no qual é conhecida a factorização a levantar.
        /// </summary>
        private IModularField<CoeffType> modularField;

        /// <summary>
        /// O anel responsável pelas operações não modulares.
        /// </summary>
        private IRing<CoeffType> mainRing;

        /// <summary>
        /// O domínio responsável pela determinação de quocientes e restos sobre os coeficentes.
        /// </summary>
        private IEuclidenDomain<CoeffType> mainDomain;

        /// <summary>
        /// Coném o número de iterações efectuado.
        /// </summary>
        private int iterationsNumber;

        /// <summary>
        /// Contém o valor de gama.
        /// </summary>
        private CoeffType gamma;

        /// <summary>
        /// Contém o valor que indica se o objecto já foi inicializado.
        /// </summary>
        private bool initialized;

        public LinearLiftingStatus(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            UnivariatePolynomialNormalForm<CoeffType> firstFactor,
            UnivariatePolynomialNormalForm<CoeffType> secondFactor,
            IModularField<CoeffType> modularField,
            IRing<CoeffType> mainRing,
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
            else if (mainRing == null)
            {
                throw new ArgumentNullException("mainRing");
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
                this.mainRing = mainRing;
                this.mainDomain = mainDomain;
                this.iterationsNumber = 0;
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
        /// Obtém o valor de e(x).
        /// </summary>
        public UnivariatePolynomialNormalForm<CoeffType> EPol
        {
            get
            {
                return this.ePol;
            }
            internal set
            {
                this.ePol = value;
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
        /// Obtém o anel responsável pelas operações não modulares.
        /// </summary>
        public IRing<CoeffType> MainRing
        {
            get
            {
                return this.mainRing;
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
        /// Obtém o número de iterações efectuado.
        /// </summary>
        public int IterationsNumber
        {
            get
            {
                return this.iterationsNumber;
            }
            internal set
            {
                this.iterationsNumber = value;
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
        internal bool Initialized
        {
            get
            {
                return this.initialized;
            }
            set
            {
                this.initialized = value;
            }
        }
    }
}
