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
        private FiniteFieldPolynomialFactorizationResult<CoeffType> factorization;

        /// <summary>
        /// Contém o módulo associado à solução actual.
        /// </summary>
        private CoeffType liftFactorizationModule;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="MultiFactorLiftingStatus{CoeffType}"/>.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <param name="factorization">A lista de factores módulo um número primo.</param>
        /// <param name="liftFactorizationModule">O número primo que constitui o módulo.</param>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        public MultiFactorLiftingStatus(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            FiniteFieldPolynomialFactorizationResult<CoeffType> factorization,
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
        /// Mantém a lista de factores módulo um número primo.
        /// </summary>
        /// <value>Os factores.</value>
        public FiniteFieldPolynomialFactorizationResult<CoeffType> Factorization
        {
            get
            {
                return this.factorization;
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
    }
}
