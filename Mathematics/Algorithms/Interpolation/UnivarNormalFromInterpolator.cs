namespace Mathematics
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Utilities;
    using Utilities.Collections;

    /// <summary>
    /// Permite construir o polinómio interpolador na forma normal.
    /// </summary>
    /// <typeparam name="SourceType">O tipo de objectos que constituem os coeficientes do polinómio.</typeparam>
    /// <typeparam name="TargetType">O tipo de objectos que interpolam.</typeparam>
    public class UnivarNormalFromInterpolator<SourceType, TargetType>
        : ABaseInterpolator<SourceType, TargetType>
    {


        /// <summary>
        /// O anel responsável pelas operações sobre os objectos da imagem.
        /// </summary>
        protected IRing<TargetType> targetRing;

        /// <summary>
        /// O objecto responsável pela multiplicação dos objectos de partida com os da imagem.
        /// </summary>
        protected IMultiplicationOperation<SourceType, TargetType, TargetType> multiplicationOperation;

        /// <summary>
        /// O polinómio interpolador actual.
        /// </summary>
        protected UnivariatePolynomialNormalForm<TargetType> interpolationgPolynomial;

        /// <summary>
        /// A matriz com os coeficientes simétricos nas coordenadas dos pontos.
        /// </summary>
        protected List<List<SourceType>> symmetricFuncMatrix;

        /// <summary>
        /// O vector que contém os inversos das diferenças entre as várias coordenadas.
        /// </summary>
        protected List<SourceType> inverseDifferencesVector;

        /// <summary>
        /// Permite converter um valor inteiro num coeficiente do polinómio a ser interpolado.
        /// </summary>
        protected IConversion<int, SourceType> integerConversion;

        /// <summary>
        /// O produto de todas as coordenadas dos pontos.
        /// </summary>
        protected SourceType productValue;

        /// <summary>
        /// O sinal associado à linha actual.
        /// </summary>
        protected SourceType signal;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UnivarNormalFromInterpolator{SourceType, TargetType}"/>.
        /// </summary>
        /// <param name="pointsContainer">O contentor de pontos que constitui o conjunto a ser interpolado.</param>
        /// <param name="variableName">A variável associada ao polinómio interpolador.</param>
        /// <param name="integerConversion">
        /// O objecto responsável pela conversão entre inteiros a o tipo de dados do conjunto
        /// dos objectos.
        /// </param>
        /// <param name="multiplicationOperation">
        /// O objecto responsável pelas operações de multiplicação entre objectos do conjunto de partida
        /// e objectos da imagem.
        /// </param>
        /// <param name="targetRing">O objecto responsável pelas operações sobre os objectos da imagem.</param>
        /// <param name="sourceField">O objecto responsável pelas operações sobre os objectos do conjunto de partida.</param>
        public UnivarNormalFromInterpolator(
            PointContainer2D<SourceType, TargetType> pointsContainer,
            string variableName,
            IConversion<int, SourceType> integerConversion,
            IMultiplicationOperation<SourceType, TargetType, TargetType> multiplicationOperation,
            IRing<TargetType> targetRing,
            IField<SourceType> sourceField)
            : base(pointsContainer, sourceField)
        {
            if (targetRing == null)
            {
                throw new ArgumentNullException("targetGroup");
            }
            else if (multiplicationOperation == null)
            {
                throw new ArgumentNullException("multiplicationOperation");
            }
            else
            {
                this.integerConversion = integerConversion;
                this.targetRing = targetRing;
                this.multiplicationOperation = multiplicationOperation;
                this.interpolationgPolynomial = new UnivariatePolynomialNormalForm<TargetType>(variableName);
                this.Initialize();
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pelas operações sobre os objectos do conjunto imagem.
        /// </summary>
        public IRing<TargetType> TargetRing
        {
            get
            {
                return this.targetRing;
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
        /// Obtém o polinómio interpolador na forma nornal.
        /// </summary>
        public override UnivariatePolynomialNormalForm<TargetType> InterpolatingPolynomial
        {
            get
            {
                this.UpdatePolynomialFromMatrices();
                return this.interpolationgPolynomial;
            }
        }

        /// <summary>
        /// Obtém a imagem de interpolação associada ao objecto.
        /// </summary>
        /// <param name="sourceValue">O objecto a ser interpolado.</param>
        /// <returns>A imagem que está associada ao objecto de acordo com a interpolação.</returns>
        public override TargetType Interpolate(SourceType sourceValue)
        {
            if (sourceValue == null)
            {
                throw new ArgumentNullException("sourceValue");
            }
            else
            {
                this.UpdatePolynomialFromMatrices();
                return this.interpolationgPolynomial.Replace(
                        sourceValue,
                        this.multiplicationOperation,
                        this.sourceField,
                        this.targetRing);

            }
        }

        /// <summary>
        /// Inicializa o interpolador com base nos pontos fornecidos.
        /// </summary>
        public void Initialize()
        {
            if (this.pointsContainer.Count == 0)
            {
                throw new MathematicsException("At least one point must be provided for interpolation.");
            }
            else
            {
                this.symmetricFuncMatrix = new List<List<SourceType>>();
                this.inverseDifferencesVector = new List<SourceType>() { this.sourceField.MultiplicativeUnity };
                var lastLine = new List<SourceType>() { this.sourceField.MultiplicativeUnity };
                this.symmetricFuncMatrix.Add(lastLine);
                this.productValue = this.sourceField.AdditiveInverse(this.pointsContainer[0].Item1);

                for (int i = 1; i < this.pointsContainer.Count; ++i)
                {
                    var currentPoint = this.pointsContainer[i].Item1;
                    this.UpdateStateFromPointAddition(currentPoint);
                }

                this.UpdatePolynomialFromMatrices();
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
            var firstCoord = eventArgs.AddedOrRemovedObject.Item1;
            this.UpdateStateFromPointAddition(firstCoord);
            this.UpdatePolynomialFromMatrices();
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
        /// Acutaliza o interpolador após a adição de um ponto.
        /// </summary>
        /// <param name="firstCoord">A primeira coordenada do ponto a ser adicionado.</param>
        private void UpdateStateFromPointAddition(SourceType firstCoord)
        {
            var count = this.symmetricFuncMatrix.Count;
            if (count == 1)
            {
                var firstLine = this.symmetricFuncMatrix[0];
                firstLine[0] = firstCoord;
                firstLine.Add(this.productValue);
                this.symmetricFuncMatrix.Add(new List<SourceType>(){
                    this.sourceField.AdditiveInverse(this.sourceField.MultiplicativeUnity),
                    this.sourceField.MultiplicativeUnity
                });

                var value = this.sourceField.Add(
                    firstCoord,
                    this.sourceField.AdditiveInverse(this.pointsContainer[0].Item1));
                value = this.sourceField.MultiplicativeInverse(value);
                this.inverseDifferencesVector[0] = value;
                this.inverseDifferencesVector.Add(value);

                // Actualiza os parâmetros auxiliares.
                this.productValue = this.sourceField.Multiply(this.productValue, firstCoord);
                this.signal = this.sourceField.AdditiveInverse(this.sourceField.MultiplicativeUnity);
            }
            else
            {
                //var tasks = new Task[]{
                //    new Task(()=>this.UpdateMatrixFromPointAddition(firstCoord)),
                //    new Task(()=>this.UpdateVectorFromPointAddition(firstCoord))
                //};

                //tasks[0].Start();
                //tasks[1].Start();
                //Task.WaitAll(tasks);
                this.UpdateMatrixFromPointAddition(firstCoord);
                this.UpdateVectorFromPointAddition(firstCoord);
            }
        }

        /// <summary>
        /// Actualiza o estado do interpolador com o ponto dado.
        /// </summary>
        /// <param name="firstCoord">A primeira coordenada do ponto.</param>
        private void UpdateMatrixFromPointAddition(SourceType firstCoord)
        {
            var count = this.symmetricFuncMatrix.Count;
            // Insere a última linha
            var vector = new SourceType[count];
            var last = count - 1;
            var lastLine = this.symmetricFuncMatrix[last];
            Parallel.For(0, count, i =>
            {
                vector[i] = this.sourceField.AdditiveInverse(lastLine[i]);
            });

            var insertionLine = new List<SourceType>(vector);
            insertionLine.Add(this.sourceField.MultiplicativeUnity);
            this.symmetricFuncMatrix.Add(insertionLine);

            // Soma os valores da linha anterior e adiciona à lista actual.
            vector[0] = this.productValue;
            Parallel.For(1, count, i =>
            {
                var accumulationLine = this.symmetricFuncMatrix[i - 1];
                var value = accumulationLine[0];
                var currentSignal = this.sourceField.AdditiveInverse(this.sourceField.MultiplicativeUnity);
                for (int j = 1; j < count; ++j)
                {
                    var accumulationValue = accumulationLine[j];
                    if (this.sourceField.IsMultiplicativeUnity(currentSignal))
                    {
                        value = this.sourceField.Add(value, accumulationValue);
                        currentSignal = this.sourceField.AdditiveInverse(this.sourceField.MultiplicativeUnity);
                    }
                    else
                    {
                        value = this.sourceField.Add(
                            value,
                            this.sourceField.AdditiveInverse(accumulationValue));
                        currentSignal = this.sourceField.MultiplicativeUnity;
                    }
                }

                var conversionValue = this.integerConversion.InverseConversion(i);
                vector[i] = this.sourceField.Multiply(
                    value,
                    this.sourceField.MultiplicativeInverse(conversionValue));
                if (!this.sourceField.IsMultiplicativeUnity(this.signal))
                {
                    vector[i] = this.sourceField.AdditiveInverse(vector[i]);
                }
            });

            // Adiciona os valores do vector às várias linhas da matriz.
            Parallel.For(0, count, i =>
            {
                var current = this.symmetricFuncMatrix[i];
                current.Add(vector[i]);
            });

            // A matriz pode ser actualizada de baixo para cima.
            var previousLine = this.symmetricFuncMatrix[last];
            --last;
            for (int i = last; i >= 0; --i)
            {
                var currentLine = this.symmetricFuncMatrix[i];

                // Actualiza todas as linhas com excepção da primeira.
                Parallel.For(0, count, j =>
                {
                    var temp = this.sourceField.Multiply(firstCoord, previousLine[j]);
                    previousLine[j] = this.sourceField.Add(
                        temp,
                        this.sourceField.AdditiveInverse(currentLine[j]));
                });

                previousLine = currentLine;
            }

            previousLine = this.symmetricFuncMatrix[0];
            Parallel.For(0, count, i =>
            {
                previousLine[i] = this.sourceField.Multiply(previousLine[i], firstCoord);
            });

            this.productValue = this.sourceField.Multiply(this.productValue, firstCoord);
            if (this.sourceField.IsMultiplicativeUnity(this.signal))
            {
                this.signal = this.sourceField.AdditiveInverse(this.sourceField.MultiplicativeUnity);
            }
            else
            {
                this.signal = this.sourceField.MultiplicativeUnity;
            }
        }

        /// <summary>
        /// Actualiza o vector independente a partir do ponto adicionado.
        /// </summary>
        /// <param name="firstCoord">A primeira coordenada do ponto adicionado.</param>
        private void UpdateVectorFromPointAddition(SourceType firstCoord)
        {
            var count = this.inverseDifferencesVector.Count;
            this.inverseDifferencesVector.Add(this.sourceField.AdditiveUnity);

            var blockinCollection = new BlockingCollection<SourceType>();
            var inserveProductConsumer = new InverseProductConsumer(
                this.inverseDifferencesVector,
                blockinCollection,
                this.sourceField);
            var consumerTask = new Task(() => inserveProductConsumer.Run());
            consumerTask.Start();
            Parallel.For(0, count, i =>
            {
                var value = this.sourceField.Add(
                    firstCoord,
                    this.sourceField.AdditiveInverse(this.pointsContainer[i].Item1));
                value = this.sourceField.MultiplicativeInverse(value);
                blockinCollection.Add(value);
                this.inverseDifferencesVector[i] = this.sourceField.Multiply(
                    this.inverseDifferencesVector[i],
                    value);

            });

            blockinCollection.CompleteAdding();
            consumerTask.Wait();
        }

        /// <summary>
        /// Actualiza o polinómio interpolador a partir das matrizes.
        /// </summary>
        private void UpdatePolynomialFromMatrices()
        {
            var count = this.symmetricFuncMatrix.Count;
            var polynomialCoeffs = new TargetType[count];
            Parallel.For(0, count, i =>
            {
                var currentLine = this.symmetricFuncMatrix[i];

                // Inicializa a primeira entrada do vector de coeficientes.
                var currentMatrixEntry = currentLine[0];
                var currentVectorItem = this.inverseDifferencesVector[0];
                var sourceResult = this.sourceField.Multiply(currentMatrixEntry, currentVectorItem);
                var targetResult = this.multiplicationOperation.Multiply(
                    sourceResult,
                    this.pointsContainer[0].Item2);
                polynomialCoeffs[i] = targetResult;

                // Processa as restante entradas.
                for (int j = 1; j < count; ++j)
                {
                    currentMatrixEntry = currentLine[j];
                    currentVectorItem = this.inverseDifferencesVector[j];
                    sourceResult = this.sourceField.Multiply(currentMatrixEntry, currentVectorItem);
                    sourceResult = this.sourceField.Multiply(currentMatrixEntry, currentVectorItem);
                    targetResult = this.multiplicationOperation.Multiply(
                    sourceResult,
                    this.pointsContainer[0].Item2);
                    targetResult = this.targetRing.Multiply(targetResult, pointsContainer[j].Item2);
                    polynomialCoeffs[i] = targetResult;
                }
            });

            // Actualiza o polinómio com os termos encontrados em paralelo uma vez que a verificação de unidade aditiva
            // pode constituir uma operação com custos.
            var polynomialTerms = this.interpolationgPolynomial.Terms;
            polynomialTerms.Clear();
            var lockObject = new object();
            Parallel.For(0, count, i =>
            {
                var coeff = polynomialCoeffs[i];
                if (!this.targetRing.IsAdditiveUnity(coeff))
                {
                    lock (lockObject)
                    {
                        polynomialTerms.Add(i, coeff);
                    }
                }
            });
        }

        /// <summary>
        /// Classe que representa um consumidor que permite actualizar a última entrada do vector.
        /// </summary>
        private class InverseProductConsumer
        {
            /// <summary>
            /// O vector a ser actualizado.
            /// </summary>
            private List<SourceType> vector;

            /// <summary>
            /// O último índice do vector.
            /// </summary>
            private int lastIndex;

            /// <summary>
            /// A colecção que bloqueia.
            /// </summary>
            private BlockingCollection<SourceType> blockingCollection;

            /// <summary>
            /// O corpo responsável pelas operações sobre os coeficientes do polinómio interpolador.
            /// </summary>
            private IField<SourceType> sourceField;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="InverseProductConsumer"/>.
            /// </summary>
            /// <param name="vector">O vector.</param>
            /// <param name="blockingCollection">A colecção que bloqueia.</param>
            /// <param name="sourceField">O corpo responsável pelas operações sobre os coeficientes.</param>
            public InverseProductConsumer(
                List<SourceType> vector,
                BlockingCollection<SourceType> blockingCollection,
                IField<SourceType> sourceField)
            {
                this.sourceField = sourceField;
                this.vector = vector;
                this.blockingCollection = blockingCollection;
                this.lastIndex = this.vector.Count - 1;
            }

            /// <summary>
            /// Executa o consumidor.
            /// </summary>
            public void Run()
            {
                while (!this.blockingCollection.IsCompleted)
                {
                    var value = default(SourceType);
                    if (this.blockingCollection.TryTake(out value))
                    {
                        this.vector[lastIndex] = this.sourceField.Multiply(
                                                vector[lastIndex],
                                                value);
                    }
                }
            }
        }
    }
}
