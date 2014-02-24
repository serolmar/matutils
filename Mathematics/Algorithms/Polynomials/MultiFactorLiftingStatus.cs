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
        private FiniteFieldFactorizationResult<CoeffType> factorization;

        /// <summary>
        /// Contém o módulo associado à solução actual.
        /// </summary>
        private CoeffType liftFactorizationModule;

        public MultiFactorLiftingStatus(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            FiniteFieldFactorizationResult<CoeffType> factorization,
            CoeffType liftFactorizationModule)
        {
            if (polynom == null)
            {
                throw new ArgumentNullException("polynom");
            }
            else if (factorization == null)
            {
                throw new ArgumentNullException("factorization");
            }
            else if (liftFactorizationModule == null)
            {
                throw new ArgumentNullException("liftFactorizationModule");
            }
            else
            {
                this.polynom = polynom;
                this.factorization = factorization;
                this.liftFactorizationModule = liftFactorizationModule;
                this.linearLiftAlgorithm = new LinearLiftAlgorithm<int>(
                    new ModularIntegerFieldFactory(),
                    new UnivarPolEuclideanDomainFactory<int>(),
                    new IntegerDomain());
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
        public FiniteFieldFactorizationResult<CoeffType> Factorization
        {
            get
            {
                return this.factorization;
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
