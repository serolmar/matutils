namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Utilities;
    using Utilities.Collections;

    public class UnivarNormalFromInterpolator<SourceType, TargetType>
        : ABaseInterpolator<SourceType, TargetType>
    {
        /// <summary>
        /// O polinómio interpolador actual.
        /// </summary>
        protected UnivariatePolynomialNormalForm<SourceType> interpolationgPolynomial;

        /// <summary>
        /// A matriz com os coeficientes simétricos nas coordenadas dos pontos.
        /// </summary>
        protected List<List<SourceType>> symmetricFuncMatrix;

        /// <summary>
        /// O vector que contém os inversos das diferenças entre as várias coordenadas.
        /// </summary>
        protected List<SourceType> inverseDifferencesVector;

        /// <summary>
        /// O produto de todas as coordenadas dos pontos.
        /// </summary>
        protected SourceType productValue;

        /// <summary>
        /// O sinal actual.
        /// </summary>
        protected SourceType signal;

        /// <summary>
        /// A unidade aditiva inversa.
        /// </summary>
        protected SourceType additiveUnityInverse;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UnivarNormalFromInterpolator{SourceType, TargetType}"/>.
        /// </summary>
        /// <param name="pointsContainer">O contentor de pontos que constitui o conjunto a ser interpolado.</param>
        /// <param name="multiplicationOperation">
        /// O objecto responsável pelas operações de multiplicação entre objectos do conjunto de partida
        /// e objectos da imagem.
        /// </param>
        /// <param name="targetGroup">O objecto responsável pelas operações sobre os objectos da imagem.</param>
        /// <param name="sourceField">O objecto responsável pelas operações sobre os objectos do conjunto de partida.</param>
        public UnivarNormalFromInterpolator(
            PointContainer2D<SourceType, TargetType> pointsContainer,
            IMultiplicationOperation<SourceType, TargetType, TargetType> multiplicationOperation,
            IGroup<TargetType> targetGroup,
            IField<SourceType> sourceField)
            : base(pointsContainer, multiplicationOperation, targetGroup, sourceField) { }

        /// <summary>
        /// Obtém o polinómio interpolador na forma nornal.
        /// </summary>
        public override UnivariatePolynomialNormalForm<SourceType> InterpolatingPolynomial
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a imagem de interpolação associada ao objecto.
        /// </summary>
        /// <param name="sourceValue">O objecto a ser interpolado.</param>
        /// <returns>A imagem que está associada ao objecto de acordo com a interpolação.</returns>
        public override TargetType Interpolate(SourceType sourceValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inicializa o interpolador com base nos pontos fornecidos.
        /// </summary>
        public override void Initialize()
        {
            if (this.pointsContainer.Count == 0)
            {
                throw new MathematicsException("At least one point must be provided for interpolation.");
            }
            else
            {
                this.symmetricFuncMatrix = new List<List<SourceType>>();
                this.inverseDifferencesVector = new List<SourceType>() { this.sourceField.AdditiveUnity };
                var lastLine = new List<SourceType>() { this.sourceField.AdditiveUnity };
                this.symmetricFuncMatrix.Add(lastLine);
                this.productValue = this.pointsContainer[0].Item1;
                this.signal = this.sourceField.AdditiveUnity;

                for (int i = 1; i < this.pointsContainer.Count; ++i)
                {
                    var currentPoint = this.pointsContainer[i].Item1;
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Executado quando o evento de aviso de adição de um ponto é excutado.
        /// </summary>
        /// <param name="sender">O obejcto que enviou o evento.</param>
        /// <param name="eventArgs">Os argumentos passados para o evento.</param>
        public override void BeforeAddEventHandler(
            object sender,
            AddDeleteEventArgs<Tuple<SourceType, TargetType>> eventArgs)
        {
            var firstCoord = eventArgs.AddedOrRemovedObject.Item1;
            if (this.pointsContainer.AsParallel().Any(pt => this.sourceField.Equals(pt.Item1, firstCoord)))
            {
                throw new MathematicsException("A point with the same first coordinate in point set already exists.");
            }
        }

        /// <summary>
        /// Exectuado quando o evento de adição de ponto concluída é despoletado.
        /// </summary>
        /// <param name="sender">O objecto que despoleta o evento.</param>
        /// <param name="eventArgs">Os argumentos do evento.</param>
        public override void AfterAddEventHandler(
            object sender,
            AddDeleteEventArgs<Tuple<SourceType, TargetType>> eventArgs)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executado quando o evento de aviso de remoção de ponto é executado.
        /// </summary>
        /// <param name="sender">O objecto que despoleta o evento.</param>
        /// <param name="eventArgs">Os argumentos do evento.</param>
        public override void BeforeRemoveEventHandler(
            object sender,
            AddDeleteEventArgs<int> eventArgs)
        {
            throw new NotSupportedException("Points removal isn´t supported yet.");
        }

        /// <summary>
        /// Exectuado quando o evento de remoção do ponto concluída é despoletado.
        /// </summary>
        /// <param name="sender">O objecto que depoleta o evento.</param>
        /// <param name="eventArgs">Os argumentos do evento.</param>
        public override void AfterRemoveEventHandler(
            object sender,
            AddDeleteEventArgs<int> eventArgs)
        {
            throw new NotSupportedException("Points removal isn´t supported yet.");
        }

        /// <summary>
        /// Actualiza o estado do interpolador com o ponto dado.
        /// </summary>
        /// <param name="firstCoord">A primeira coordenada do ponto.</param>
        private void UpdateStateFromPoint(SourceType firstCoord)
        {
            // A matriz pode ser actualizada de baixo para cima.
            var count = this.symmetricFuncMatrix.Count;
            for (int i = count - 1; i > 0; --i)
            {
                var currentLine = this.symmetricFuncMatrix[count];

                // Actualiza todas as linhas com excepção da primeira.
                Parallel.For(0, count, j =>
                {

                });
            }
            throw new NotImplementedException();
        }
    }
}
