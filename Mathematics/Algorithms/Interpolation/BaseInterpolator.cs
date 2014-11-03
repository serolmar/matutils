namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;
    using Utilities.Collections;

    /// <summary>
    /// Define um interpolador básico.
    /// </summary>
    /// <typeparam name="SourceType">
    /// O tiop de dados que constituem os coeficientes do conjunto 
    /// de partida.</typeparam>
    /// <typeparam name="TargetType">
    /// O tipo dos objectos que constituem o conjunto imagem.
    /// </typeparam>
    public interface IInterpolator<SourceType, out TargetType>
    {
        /// <summary>
        /// Obtém a imagem de interpolação associada ao objecto.
        /// </summary>
        /// <param name="sourceValue">O objecto a ser interpolado.</param>
        /// <returns>A imagem que está associada ao objecto de acordo com a interpolação.</returns>
        TargetType Interpolate(SourceType sourceValue);

        /// <summary>
        /// Determina o polinómio interpolador na forma nornal.
        /// </summary>
        /// <returns>O polinóomio interpolador na forma normal.</returns>
        UnivariatePolynomialNormalForm<SourceType> InterpolatingPolynomial();
    }

    public abstract class ABaseInterpolator<SourceType, TargetType> : IInterpolator<SourceType, TargetType>, IDisposable
    {
        /// <summary>
        /// O corpo responsável pelas operações sobre os objectos do conjunto de partida.
        /// </summary>
        private IField<SourceType> sourceField;

        /// <summary>
        /// O grupo responsável pelas operações sobre os objectos da imagem.
        /// </summary>
        private IGroup<TargetType> targetGroup;

        /// <summary>
        /// O objecto responsável pela multiplicação dos objectos de partida com os da imagem.
        /// </summary>
        private IMultiplicationOperation<SourceType, TargetType, TargetType> multiplicationOperation;

        /// <summary>
        /// O contentor de pontos a serem interpolados.
        /// </summary>
        private PointContainer2D<SourceType, TargetType> pointsContainer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ABaseInterpolator{SourceType, TargetType}"/>.
        /// </summary>
        /// <param name="pointsContainer">O contentor de pontos que constitui o conjunto a ser interpolado.</param>
        /// <param name="multiplicationOperation">
        /// O objecto responsável pelas operações de multiplicação entre objectos do conjunto de partida
        /// e objectos da imagem.
        /// </param>
        /// <param name="targetGroup">O objecto responsável pelas operações sobre os objectos da imagem.</param>
        /// <param name="sourceField">O objecto responsável pelas operações sobre os objectos do conjunto de partida.</param>
        public ABaseInterpolator(
            PointContainer2D<SourceType, TargetType> pointsContainer,
            IMultiplicationOperation<SourceType, TargetType, TargetType> multiplicationOperation,
            IGroup<TargetType> targetGroup,
            IField<SourceType> sourceField)
        {
            if (sourceField == null)
            {
                throw new ArgumentNullException("sourceField");
            }
            else if (targetGroup == null)
            {
                throw new ArgumentNullException("targetGroup");
            }
            else if (multiplicationOperation == null)
            {
                throw new ArgumentNullException("multiplicationOperation");
            }
            else if (pointsContainer == null)
            {
                throw new ArgumentNullException("pointsConatiner");
            }
            else
            {
                // Delega a inicialização para mais tarde.
                this.Initialize(pointsContainer);

                // Inicializa os eventos.
                this.pointsContainer.BeforeAddEvent -= this.BeforeAddEventHandler;
                this.pointsContainer.AfterAddEvent -= this.AfterAddEventHandler;
                this.pointsContainer.BeforeDeleteEvent -= this.BeforeRemoveEventHandler;
                this.pointsContainer.AfterDeleteEvent -= this.AfterRemoveEventHandler;

                // Atribui as variáveis privadas.
                this.pointsContainer = pointsContainer;
                this.multiplicationOperation = multiplicationOperation;
                this.targetGroup = targetGroup;
                this.sourceField = sourceField;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pelas operações sobre os objectos do conjunto de partida.
        /// </summary>
        public IField<SourceType> SourceField
        {
            get
            {
                return this.sourceField;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pelas operações sobre os objectos do conjunto imagem.
        /// </summary>
        public IGroup<TargetType> TargetGroup
        {
            get
            {
                return this.targetGroup;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pelas operações de multiplicação entre objectos do conjunto de partida
        /// e os objectos do conjunto imagem.
        /// </summary>
        public IMultiplicationOperation<SourceType, TargetType, TargetType> MultiplicationOperation
        {
            get
            {
                return this.multiplicationOperation;
            }
        }

        /// <summary>
        /// Obtém o conjunto de pontos que constituem o interpolador.
        /// </summary>
        public PointContainer2D<SourceType, TargetType> PointsContainer
        {
            get
            {
                return this.pointsContainer;
            }
        }

        /// <summary>
        /// Obtém a imagem de interpolação associada ao objecto.
        /// </summary>
        /// <param name="sourceValue">O objecto a ser interpolado.</param>
        /// <returns>A imagem que está associada ao objecto de acordo com a interpolação.</returns>
        public abstract TargetType Interpolate(SourceType sourceValue);

        /// <summary>
        /// Determina o polinómio interpolador na forma nornal.
        /// </summary>
        /// <returns>O polinóomio interpolador na forma normal.</returns>
        public abstract UnivariatePolynomialNormalForm<SourceType> InterpolatingPolynomial();

        /// <summary>
        /// Permite dispôr os rescursos alocados pelo objecto em questão.
        /// </summary>
        public virtual void Dispose()
        {
            this.pointsContainer.BeforeAddEvent -= this.BeforeAddEventHandler;
            this.pointsContainer.AfterAddEvent -= this.AfterAddEventHandler;
            this.pointsContainer.BeforeDeleteEvent -= this.BeforeRemoveEventHandler;
            this.pointsContainer.AfterDeleteEvent -= this.AfterRemoveEventHandler;
        }

        #region Eventos

        /// <summary>
        /// Permite inicializar o interpolador com o conjunto de pontos proporcionado.
        /// </summary>
        /// <param name="pointsEnumerator">O enumerador de pontos.</param>
        public abstract void Initialize(IIndexed<int, Tuple<SourceType, TargetType>> pointsEnumerator);

        /// <summary>
        /// Manuseador do evento que é despoletado antes do objecto ser adicionado ao contentor de pontos.
        /// </summary>
        /// <param name="sender">O objecto que despoleta o evento.</param>
        /// <param name="eventArgs">O argumento do evento.</param>
        public abstract void BeforeAddEventHandler(object sender, AddDeleteEventArgs<Tuple<SourceType, TargetType>> eventArgs);

        /// <summary>
        /// Manuseador do evento que é despoletado após o objecto ser adicionado ao contentor de pontos.
        /// </summary>
        /// <param name="sender">O objecto que despoleta o evento.</param>
        /// <param name="eventArgs">O argumento do evento.</param>
        public abstract void AfterAddEventHandler(object sender, AddDeleteEventArgs<Tuple<SourceType, TargetType>> eventArgs);

        /// <summary>
        /// Manuseador do evento que é despoletado antes do objecto ser removido do contentor de pontos.
        /// </summary>
        /// <param name="sender">O objecto que despoleta o evento.</param>
        /// <param name="eventArgs">O argumento do evento.</param>
        public abstract void BeforeRemoveEventHandler(object sender, AddDeleteEventArgs<int> eventArgs);

        /// <summary>
        /// Manuseador do evento que é despoletado após o objecto ser removido do contentor de pontos.
        /// </summary>
        /// <param name="sender">O objecto que despoleta o evento.</param>
        /// <param name="eventArgs">O argumento do evento.</param>
        public abstract void AfterRemoveEventHandler(object sender, AddDeleteEventArgs<int> eventArgs);

        #endregion Eventos
    }
}
