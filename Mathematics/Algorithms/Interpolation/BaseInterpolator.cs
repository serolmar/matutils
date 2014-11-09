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
    public interface IInterpolator<SourceType, TargetType>
    {
        /// <summary>
        /// Determina o polinómio interpolador na forma nornal.
        /// </summary>
        /// <returns>O polinóomio interpolador na forma normal.</returns>
        UnivariatePolynomialNormalForm<TargetType> InterpolatingPolynomial { get; }

        /// <summary>
        /// Obtém a imagem de interpolação associada ao objecto.
        /// </summary>
        /// <param name="sourceValue">O objecto a ser interpolado.</param>
        /// <returns>A imagem que está associada ao objecto de acordo com a interpolação.</returns>
        TargetType Interpolate(SourceType sourceValue);
    }

    /// <summary>
    /// Define a base para instâncias de polinómios interpoladores.
    /// </summary>
    /// <typeparam name="SourceType">O tipo de objectos do conjunto de partida.</typeparam>
    /// <typeparam name="TargetType">O tipo de objectos do conjunto imagem.</typeparam>
    public abstract class ABaseInterpolator<SourceType, TargetType> : IInterpolator<SourceType, TargetType>, IDisposable
    {
        /// <summary>
        /// O contentor de pontos a serem interpolados.
        /// </summary>
        protected PointContainer2D<SourceType, TargetType> pointsContainer;

        /// <summary>
        /// O corpo responsável pelas operações sobre os objectos do conjunto de partida.
        /// </summary>
        protected IField<SourceType> sourceField;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ABaseInterpolator{SourceType, TargetType}"/>.
        /// </summary>
        /// <param name="pointsContainer">O contentor de pontos que constitui o conjunto a ser interpolado.</param>
        /// <param name="sourceField">O objecto responsável pelas operações sobre os objectos do conjunto de partida.</param>
        public ABaseInterpolator(
            PointContainer2D<SourceType, TargetType> pointsContainer,
            IField<SourceType> sourceField)
        {
            if (sourceField == null)
            {
                throw new ArgumentNullException("sourceField");
            }
            else if (pointsContainer == null)
            {
                throw new ArgumentNullException("pointsConatiner");
            }
            else
            {
                // Delega a inicialização para mais tarde.
                this.pointsContainer = pointsContainer;
                this.sourceField = sourceField;

                // Inicializa os eventos.
                this.pointsContainer.BeforeAddEvent += this.BeforeAddEventHandler;
                this.pointsContainer.AfterAddEvent += this.AfterAddEventHandler;
                this.pointsContainer.BeforeDeleteEvent += this.BeforeRemoveEventHandler;
                this.pointsContainer.AfterDeleteEvent += this.AfterRemoveEventHandler;
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
        /// Obtém o polinómio interpolador na forma nornal.
        /// </summary>
        public abstract UnivariatePolynomialNormalForm<TargetType> InterpolatingPolynomial { get; }

        /// <summary>
        /// Obtém a imagem de interpolação associada ao objecto.
        /// </summary>
        /// <param name="sourceValue">O objecto a ser interpolado.</param>
        /// <returns>A imagem que está associada ao objecto de acordo com a interpolação.</returns>
        public abstract TargetType Interpolate(SourceType sourceValue);

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
