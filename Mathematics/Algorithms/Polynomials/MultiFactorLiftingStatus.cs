namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Mantém o estado para o algoritmo do levantamento para vários factores.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo do coeficiente.</typeparam>
    public class MultiFactorLiftingStatus<CoeffType>
    {
        /// <summary>
        /// O polinómio sobre o qual pretendemos executar o levantamento.
        /// </summary>
        private UnivariatePolynomialNormalForm<CoeffType> polynom;

        /// <summary>
        /// Mantém uma referência para o algoritmo do levantamento.
        /// </summary>
        private IAlgorithm<LinearLiftingStatus<int>, int, bool> linearLiftAlgorithm;

        /// <summary>
        /// Mantém a lista de factores módulo um número primo.
        /// </summary>
        private List<UnivariatePolynomialNormalForm<CoeffType>> factors;

        /// <summary>
        /// O corpo modular no qual é conhecida a factorização a levantar.
        /// </summary>
        private IModularField<CoeffType> modularField;

        /// <summary>
        /// O domínio responsável pela determinação de quocientes e restos sobre os coeficentes.
        /// </summary>
        private IEuclidenDomain<CoeffType> mainDomain;

        /// <summary>
        /// O domínio polinomial baseado no corpo modular.
        /// </summary>
        private UnivarPolynomEuclideanDomain<CoeffType> modularPolynomialDomain;

        /// <summary>
        /// O anel polinomial baseado no anel principal.
        /// </summary>
        private UnivarPolynomRing<CoeffType> mainPolynomialRing;

        /// <summary>
        /// Coném o módulo associado à solução actual.
        /// </summary>
        private CoeffType liftFactorizationModule;

        public MultiFactorLiftingStatus(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            List<UnivariatePolynomialNormalForm<CoeffType>> factors,
            IModularField<CoeffType> modularField,
            IEuclidenDomain<CoeffType> mainDomain)
        {
            if (polynom == null)
            {
                throw new ArgumentNullException("polynom");
            }
            else if (factors == null)
            {
                throw new ArgumentNullException("factors");
            }
            else if (modularField == null)
            {
                throw new ArgumentNullException("modularField");
            }
            else if (mainDomain == null)
            {
                throw new ArgumentNullException("mainDomain");
            }
            else
            {
                this.polynom = polynom;
                this.factors = factors;
                this.modularField = modularField;
                this.mainDomain = mainDomain;
                this.liftFactorizationModule = modularField.Module;
                this.modularPolynomialDomain = new UnivarPolynomEuclideanDomain<CoeffType>(
                    polynom.VariableName,
                    modularField);
                this.mainPolynomialRing = new UnivarPolynomRing<CoeffType>(
                    polynom.VariableName,
                    mainDomain);
                this.linearLiftAlgorithm = new LinearLiftAlgorithm();
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
        /// Mantém a lista de factores módulo um número primo.
        /// </summary>
        public List<UnivariatePolynomialNormalForm<CoeffType>> Factors
        {
            get
            {
                return this.factors;
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
        /// Obtém o domínio polinomial baseado no corpo modular.
        /// </summary>
        public UnivarPolynomEuclideanDomain<CoeffType> ModularPolynomialDomain
        {
            get
            {
                return this.modularPolynomialDomain;
            }
        }

        /// <summary>
        /// Obtém o anel polinomial baseado no anel principal.
        /// </summary>
        public UnivarPolynomRing<CoeffType> MainPolynomialRing
        {
            get
            {
                return this.mainPolynomialRing;
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
    }
}
