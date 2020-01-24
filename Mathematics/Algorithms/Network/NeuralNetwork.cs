// -----------------------------------------------------------------------
// <copyright file="NeuralNetwork.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Define um padrão de treino.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem os coeficientes.</typeparam>
    /// <typeparam name="InputVectorType">O tipo dos objectos que constituem o vector de entrada.</typeparam>
    /// <typeparam name="OutputVectorType">O tipo dos objectos que constituem o vector de saída.</typeparam>
    public class NeuralNetworkTrainingPattern<CoeffType, InputVectorType, OutputVectorType>
        where InputVectorType : IVector<CoeffType>
        where OutputVectorType : IVector<CoeffType>
    {
        /// <summary>
        /// O vector de entrada.
        /// </summary>
        private InputVectorType input;

        /// <summary>
        /// O vector de saída.
        /// </summary>
        private OutputVectorType output;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="NeuralNetworkTrainingPattern{CoeffType, InputVectorType, OutputVectorType}"/>.
        /// </summary>
        /// <param name="input">O vector com os dados de entrada.</param>
        /// <param name="output">O vector com os dados de saída.</param>
        public NeuralNetworkTrainingPattern(
            InputVectorType input,
            OutputVectorType output)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            else if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            else
            {
                this.input = input;
                this.output = output;
            }
        }

        /// <summary>
        /// Obtém o vector de entrada.
        /// </summary>
        public InputVectorType Input
        {
            get
            {
                return this.input;
            }
        }

        /// <summary>
        /// Obtém o vector de saída.
        /// </summary>
        public OutputVectorType Output
        {
            get
            {
                return this.output;
            }
        }
    }

    /// <summary>
    /// Modelo que pode ser carregado nas redes neurais.
    /// </summary>
    public class NeuralNetworkModel<CoeffType, MatrixType, VectorType>
        where MatrixType : IMatrix<CoeffType>
        where VectorType : IVector<CoeffType>
    {
        /// <summary>
        /// A matriz dos pesos.
        /// </summary>
        private MatrixType weightsMatrix;

        /// <summary>
        /// A matriz dos valores limiar.
        /// </summary>
        private VectorType tresholds;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="NeuralNetworkModel{CoeffType,  MatrixType, VectorType}"/>.
        /// </summary>
        internal NeuralNetworkModel() { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="NeuralNetworkModel{CoeffType,  MatrixType, VectorType}"/>.
        /// </summary>
        /// <param name="weightsMatrix">A matriz dos pesos.</param>
        /// <param name="tresholds">O vector com os valores limiar.</param>
        public NeuralNetworkModel(
            MatrixType weightsMatrix,
            VectorType tresholds)
        {
            if (weightsMatrix == null)
            {
                throw new ArgumentNullException("weightsMatrix");
            }
            else if (tresholds == null)
            {
                throw new ArgumentNullException("tresholds");
            }
            else
            {
                this.weightsMatrix = weightsMatrix;
                this.tresholds = tresholds;
            }
        }

        /// <summary>
        /// Obtém os pesos.
        /// </summary>
        public MatrixType WeightsMatrix
        {
            get
            {
                return this.weightsMatrix;
            }
            internal set
            {
                this.weightsMatrix = value;
            }
        }

        /// <summary>
        /// Obtém os valores limiar.
        /// </summary>
        public VectorType Tresholds
        {
            get
            {
                return this.tresholds;
            }
            internal set
            {
                this.tresholds = value;
            }
        }
    }

    /// <summary>
    /// Define uma rede neuronal por camadas.
    /// </summary>
    /// <typeparam name="CoeffType">
    /// O tipo dos objectos que constituem os coeficientes.
    /// </typeparam>
    public class FeedForwardNeuralNetwork<CoeffType> :
        IAlgorithm<
            CoeffType[],
            Func<CoeffType[], CoeffType[], long, CoeffType>,
            Func<CoeffType, CoeffType, CoeffType>,
            CoeffType[]>
    {
        /// <summary>
        /// A função de propagação.
        /// </summary>
        /// <remarks>
        /// A função recebe, como argumentos, o vector de valores de entrada,
        /// o vector dos pesos e respectivos tamanhos. Retorna o valor propagado.
        /// </remarks>
        //private Func<CoeffType[], CoeffType[], long, CoeffType> propagationFunc;

        /// <summary>
        /// A função geral de activação.
        /// </summary>
        /// <remarks>
        /// A função de activação recebe o valor limiar, o valor propagado e retorna
        /// o valor de activação.
        /// </remarks>
        //private Func<CoeffType, CoeffType, CoeffType> activationFunc;

        /// <summary>
        /// O esquema de camadas.
        /// </summary>
        private long[] schema;

        /// <summary>
        /// O vector dos pesos.
        /// </summary>
        private CoeffType[][] weights;

        /// <summary>
        /// Os valores limiar.
        /// </summary>
        private CoeffType[] tresholds;

        /// <summary>
        /// O número de nós da maior camada.
        /// </summary>
        private long max;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="FeedForwardNeuralNetwork{CoeffType}"/>.
        /// </summary>
        /// <param name="schema">O esquema de camadas de rede neuronal.</param>
        /// <remarks>
        /// <list type="table">
        /// <listheader>
        /// <term>Descrições</term>
        /// </listheader>
        /// <item>
        /// <term>Função de propagação:</term>
        /// <description>
        /// (vector entrada, pesos, número de elementos a considerar) => valor propagado
        /// </description>
        /// </item>
        /// <item>
        /// <term>Função de activação:</term>
        /// <description>(limiar, valor propagado) => valor activado</description>
        /// </item>
        /// <item>
        /// <term>Esquema:</term>
        /// <description>
        /// O primeiro item contém o número de neurónios de entrada e o último item
        /// contém o número de neurónios de saída. Os items intermédios correspondem
        /// ao número de neurónios nas camadas intermédias escondidas. Tem de existir,
        /// no mínimo, duas camadas, nomeadamente, a camada de entrada e a de saída.
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        public FeedForwardNeuralNetwork(
            long[] schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException("schema");
            }
            else
            {
                var length = schema.LongLength;
                if (length < 1)
                {
                    throw new ArgumentException("Neural network must have at least a processing layer.");
                }
                else
                {
                    for (var i = 0L; i < length; ++i)
                    {
                        var curr = schema[i];
                        if (curr <= 0)
                        {
                            throw new ArgumentNullException("All schema values must be greater than zero.");
                        }
                    }

                    this.ReserveWeights(
                        schema);
                    this.schema = schema;
                }
            }
        }

        /// <summary>
        /// Obtém o esquema da rede.
        /// </summary>
        /// <remarks>
        /// O primeiro item contém o número de neurónios de entrada e o último item
        /// contém o número de neurónios de saída. Os items intermédios correspondem
        /// ao número de neurónios nas camadas intermédias escondidas. Tem de existir,
        /// no mínimo, duas camadas, nomeadamente, a camada de entrada e a de saída.
        /// </remarks>
        public ReadOnlyCollection<long> Schema
        {
            get
            {
                return Array.AsReadOnly(this.schema);
            }
        }

        /// <summary>
        /// Obtém os valores limiar para efeitos de diagnóstico.
        /// </summary>
        internal CoeffType[] InternalTresholds
        {
            get
            {
                return this.tresholds;
            }
        }

        /// <summary>
        /// Obtém a matriz de pesos para efeito de diagnóstico.
        /// </summary>
        internal CoeffType[][] InternalWeights
        {
            get
            {
                return this.weights;
            }
        }

        /// <summary>
        /// Obtém os dados de saída corresopndentes ao processamento
        /// dos dados de entrada pela rede neural.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <param name="propagationFunc">A função de propagação.</param>
        /// <param name="activationFunc">A função de activação.</param>
        /// <returns>Os valores de saída.</returns>
        public CoeffType[] Run(
            CoeffType[] data,
            Func<CoeffType[], CoeffType[], long, CoeffType> propagationFunc,
            Func<CoeffType, CoeffType, CoeffType> activationFunc)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else if (propagationFunc == null)
            {
                throw new ArgumentNullException("propagationFunc");
            }
            else if (activationFunc == null)
            {
                throw new ArgumentNullException("activationFunc");
            }
            else
            {
                var length = data.LongLength;
                if (length == 0)
                {
                    throw new ArgumentException("Data vector has no values.");
                }
                else
                {
                    var firstLayerSize = this.schema[0];
                    if (length > firstLayerSize)
                    {
                        throw new ArgumentException(string.Format(
                            "Data length {0} is greater than the number {1} of first layer.",
                            length,
                            firstLayerSize));
                    }
                    else
                    {
                        return this.InnerRun(
                            data,
                            propagationFunc,
                            activationFunc);
                    }
                }
            }
        }

        /// <summary>
        /// Carrega um modelo.
        /// </summary>
        /// <param name="model">O modelo.</param>
        public void LoadModelSparse<MatrixType, LineType, VectorType>(
            NeuralNetworkModel<CoeffType, MatrixType, VectorType> model)
            where MatrixType : ISparseMatrix<CoeffType, LineType>
            where LineType : ISparseMatrixLine<CoeffType>
            where VectorType : IVector<CoeffType>
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            else
            {
                var networkLength = this.tresholds.LongLength;

                var modelWeights = model.WeightsMatrix;
                var weightsLength = modelWeights.GetLength(0);
                if (weightsLength == networkLength)
                {
                    var modelTresholds = model.Tresholds;
                    var tresholdLength = modelTresholds.LongLength;
                    if (tresholdLength == networkLength)
                    {
                        this.InnerLoadModelSparse<MatrixType, LineType>(
                                modelWeights,
                                modelTresholds);
                    }
                    else
                    {
                        throw new ArgumentException(
                            "Number of nodes in model tresholds doesn't match the number of nodes in current schema.");
                    }
                }
                else
                {
                    throw new ArgumentException(
                        "Number of nodes in model weights doesn't match the number of nodes in current schema.");
                }
            }
        }

        /// <summary>
        /// Carrega um modelo.
        /// </summary>
        /// <param name="model">O modelo.</param>
        public void LoadModel<MatrixType, VectorType>(
            NeuralNetworkModel<CoeffType, MatrixType, VectorType> model)
            where MatrixType : ILongMatrix<CoeffType>
            where VectorType : IVector<CoeffType>
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            else
            {
                var networkLength = this.tresholds.LongLength;

                var modelWeights = model.WeightsMatrix;
                var weightsLength = modelWeights.GetLength(0);
                if (weightsLength == networkLength)
                {
                    var modelTresholds = model.Tresholds;
                    var tresholdLength = modelTresholds.LongLength;
                    if (tresholdLength == networkLength)
                    {
                        var tresholLength = this.tresholds.LongLength;
                        modelTresholds.CopyTo(this.tresholds, 0);

                        var modelMatrix = model.WeightsMatrix;
                        var matrixDim = modelMatrix.GetLength(0);
                        var innerMatrix = this.weights;
                        var schema = this.schema;

                        var pointer = 0;
                        var prevCol = 0L;
                        var currCol = schema[pointer++];
                        var currLine = schema[pointer];
                        for (var i = 0; i < matrixDim; ++i)
                        {
                            if (i == currLine)
                            {
                                prevCol = currCol;
                                currCol += schema[pointer++];
                                currLine += schema[pointer];
                            }

                            var actualLine = innerMatrix[i];
                            for (var j = prevCol; j < currCol; ++j)
                            {
                                actualLine[j - prevCol] = modelMatrix[i, j];
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException(
                            "Number of nodes in model tresholds doesn't match the number of nodes in current schema.");
                    }
                }
                else
                {
                    throw new ArgumentException(
                        "Number of nodes in model weights doesn't match the number of nodes in current schema.");
                }
            }
        }

        /// <summary>
        /// Permite treinar a rede neuronal com base no método do gradiente descendente.
        /// </summary>
        /// <typeparam name="InputVectorType">
        /// Os tipos de objectos que constituem os vectores de entrada no padrão.
        /// </typeparam>
        /// <typeparam name="OutputVectorType">
        /// Os tipos dos objectos que constituem os vectores de saída no padrão.
        /// </typeparam>
        /// <param name="pattern">O padrão.</param>
        /// <param name="epocs">O número de épocas a aplicar no treino.</param>
        /// <param name="ring">O anel responsável pelas operações de adição e multiplicação.</param>
        /// <param name="trainingActivationFunction">
        /// A função de activação a ser usada durante o treino.
        /// (valor limiar, valor saída propagação) => valor
        /// </param>
        /// <param name="trainingPropagationFunction">
        /// A função de propagação a ser usada durante o treino.
        /// (vector peso nó actual, vector nós camada anterior, tamanho) => valor
        /// </param>
        /// <param name="trainingDiffActivFunc">
        /// Derivada relativa à função de activação em função do valor do nó.
        /// (valor nó actual) => valor.
        /// </param>
        /// <param name="trainingDiffPropFunc">
        /// Derivada relativa à função de propagação.
        /// (vector peso nó actual, vector nós camada anterior, índice) => valor
        /// </param>
        /// <param name="initializationAct">
        /// A acção que permite estabelecer os valores iniciais dos parâmetros.
        /// </param>
        public void Train<InputVectorType, OutputVectorType>(
            NeuralNetworkTrainingPattern<CoeffType, InputVectorType, OutputVectorType>[] pattern,
            long epocs,
            IRing<CoeffType> ring,
            Func<CoeffType, CoeffType, CoeffType> trainingActivationFunction,
            Func<CoeffType[], CoeffType[], long, CoeffType> trainingPropagationFunction,
            Func<CoeffType, CoeffType> trainingDiffActivFunc,
            Func<CoeffType[], CoeffType[], long, CoeffType> trainingDiffPropFunc,
            Action<CoeffType[], CoeffType[][]> initializationAct)
            where InputVectorType : IVector<CoeffType>
            where OutputVectorType : IVector<CoeffType>
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            else if (epocs < 1)
            {
                throw new ArgumentException("At least one epoc must be considered.");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (trainingActivationFunction == null)
            {
                throw new ArgumentNullException("trainingActivationFunction");
            }
            else if (trainingPropagationFunction == null)
            {
                throw new ArgumentNullException("trainingPropagationFunction");
            }
            else if (initializationAct == null)
            {
                throw new ArgumentException("initializationAct");
            }
            else
            {
                var patternLength = pattern.LongLength;
                if (patternLength == 0L)
                {
                    throw new ArgumentException("No item found in pattern.");
                }
                else
                {
                    // Verifica a integridade do padrão.
                    var inputLength = this.schema[0];
                    var outputLength = this.schema[this.schema.LongLength - 1];

                    for (var i = 0L; i < patternLength; ++i)
                    {
                        var currentPattern = pattern[i];
                        if (currentPattern.Input.LongLength != inputLength)
                        {
                            throw new ArgumentException(string.Format(
                                "Error in pattern {0}: {1}",
                                i,
                                "Input vector length must match the input size of the neural network."));
                        }
                        else if (currentPattern.Output.LongLength != outputLength)
                        {
                            throw new ArgumentException(string.Format(
                                "Error in pattern {0}: {1}",
                                i,
                                "Output vector length must match the output size of the neural network."));
                        }
                    }

                    this.InnerTrain(
                        pattern,
                        epocs,
                        ring,
                        trainingActivationFunction,
                        trainingPropagationFunction,
                        trainingDiffActivFunc,
                        trainingDiffPropFunc,
                        initializationAct);
                }
            }
        }

        /// <summary>
        /// Função que permite testar a determinação dos valores de saída associados
        /// a cada nó em particular.
        /// </summary>
        /// <typeparam name="InputVectorType">
        /// O tipo de objectos que constituem os valores de entrada.
        /// </typeparam>
        /// <param name="input">O vector com os valores de entrada.</param>
        /// <param name="outputMatrix">A matriz que irá conter os resultados.</param>
        /// <param name="trainingActivationFunction">
        /// A função de activação a ser usada durante o treino.
        /// </param>
        /// <param name="trainingPropagationFunction">
        /// A função de propagação a ser usada durante o treino.
        /// </param>
        internal void InternalComputeLayerOutputs<InputVectorType>(
            InputVectorType input,
            CoeffType[][] outputMatrix,
            Func<CoeffType, CoeffType, CoeffType> trainingActivationFunction,
            Func<CoeffType[], CoeffType[], long, CoeffType> trainingPropagationFunction)
            where InputVectorType : IVector<CoeffType>
        {
            this.ComputeLayerOutputs(
                input,
                outputMatrix,
                trainingActivationFunction,
                trainingPropagationFunction);
        }

        /// <summary>
        /// Reserva o espaço necessário para os resultados intermédios.
        /// </summary>
        /// <remarks>Função cujo propósito é a aplicação de testes.</remarks>
        /// <returns>O espaço reservado.</returns>
        internal CoeffType[][] InternalReserveOutput()
        {
            return this.ReserveOutputs(
                this.schema);
        }

        /// <summary>
        /// Reserva o espaço necessário para o armazenamento dos pesos.
        /// </summary>
        /// <param name="schema">O esquema.</param>
        private void ReserveWeights(
            long[] schema)
        {
            var length = schema.LongLength;
            var sum = 0L;
            var max = 0L;
            for (var i = 1L; i < length; ++i)
            {
                var curr = schema[i];
                sum += curr;
                if (max < curr)
                {
                    max = curr;
                }
            }

            this.max = max;
            this.tresholds = new CoeffType[sum];
            var innerWeights = new CoeffType[sum][];

            var pointer = 0L;
            var prevSchema = schema[0];
            for (var i = 1L; i < length; ++i)
            {
                var currSchema = schema[i];
                for (var j = 0L; j < currSchema; ++j)
                {
                    innerWeights[pointer++] = new CoeffType[prevSchema];
                }

                prevSchema = currSchema;
            }

            this.weights = innerWeights;
        }

        /// <summary>
        /// Processa a função de execução.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <param name="propagationFunc">A função de propagação.</param>
        /// <param name="activationFunc">A função de activação.</param>
        /// <returns>Os valores de saída.</returns>
        private CoeffType[] InnerRun(
            CoeffType[] data,
            Func<CoeffType[], CoeffType[], long, CoeffType> propagationFunc,
            Func<CoeffType, CoeffType, CoeffType> activationFunc
            )
        {
            var schemaLength = this.schema.LongLength;
            var dataLength = data.LongLength;
            var firstInterOut = new CoeffType[this.max];
            var innerWeights = this.weights;
            var innerTresholds = this.tresholds;
            var pointer = 0L;

            var prevSchema = this.schema[0];
            var currSchema = this.schema[1];
            for (var i = 0L; i < currSchema; ++i)
            {
                var currWeights = innerWeights[pointer];
                var currTreshold = innerTresholds[pointer++];
                var value = propagationFunc.Invoke(
                    data,
                    currWeights,
                    prevSchema);
                value = activationFunc.Invoke(
                    currTreshold, value);
                firstInterOut[i] = value;
            }

            if (schemaLength == 2)
            {
                if (currSchema == this.max)
                {
                    return firstInterOut;
                }
                else
                {
                    var result = new CoeffType[currSchema];
                    Array.Copy(firstInterOut, result, currSchema);
                    return result;
                }
            }
            else
            {
                var secondInterOut = new CoeffType[this.max];
                for (var i = 2L; i < schemaLength; ++i)
                {
                    prevSchema = currSchema;
                    currSchema = this.schema[i];
                    for (var j = 0L; j < currSchema; ++j)
                    {
                        var currWeights = innerWeights[pointer];
                        var currTreshold = innerTresholds[pointer++];
                        var value = propagationFunc.Invoke(
                            firstInterOut,
                            currWeights,
                            prevSchema);
                        value = activationFunc.Invoke(
                            currTreshold, value);
                        secondInterOut[j] = value;
                    }

                    var aux = secondInterOut;
                    secondInterOut = firstInterOut;
                    firstInterOut = aux;
                }

                if (currSchema == this.max)
                {
                    return firstInterOut;
                }
                else
                {
                    var result = new CoeffType[currSchema];
                    Array.Copy(firstInterOut, result, currSchema);
                    return result;
                }
            }
        }

        /// <summary>
        /// Carrega um modelo.
        /// </summary>
        /// <param name="weightsMatrix">A matriz dos pesos.</param>
        /// <param name="tresholdsVector">O vector de valores limiar.</param>
        private void InnerLoadModelSparse<MatrixType, LineType>(
            MatrixType weightsMatrix,
            IVector<CoeffType> tresholdsVector)
            where MatrixType : ISparseMatrix<CoeffType, LineType>
            where LineType : ISparseMatrixLine<CoeffType>
        {
            var tresholLength = this.tresholds.LongLength;
            tresholdsVector.CopyTo(this.tresholds, 0);

            var pointer = 0L;
            var prevColSchema = 0L;
            var colSchema = this.schema[pointer++];
            var lineSchema = this.schema[pointer];
            var lines = weightsMatrix.GetLines();
            var linesEnumerator = lines.GetEnumerator();
            var linePointer = 0L;
            var lineState = linesEnumerator.MoveNext();
            while (lineState)
            {
                var currentLine = linesEnumerator.Current;
                var currentLineNumber = currentLine.Key;
                if (currentLineNumber < lineSchema)
                {
                    while (linePointer < currentLineNumber)
                    {
                        var innerWeights = this.weights[linePointer++];
                        var innerLength = innerWeights.LongLength;
                        for (var i = 0L; i < innerLength; ++i)
                        {
                            innerWeights[i] = weightsMatrix.DefaultValue;
                        }
                    }

                    var thisWeights = this.weights[linePointer++];
                    var thisWeightsLength = thisWeights.LongLength;
                    var weightsPointer = 0L;
                    var currentLineValue = currentLine.Value;
                    var columns = currentLineValue.GetColumns();
                    var columnEnumerator = columns.GetEnumerator();
                    var columnState = columnEnumerator.MoveNext();
                    while (columnState)
                    {
                        var currentColumn = columnEnumerator.Current;
                        var currentColumnNumber = currentColumn.Key;
                        var currentColumnValue = currentColumn.Value;
                        if (currentColumnNumber < prevColSchema)
                        {
                            // Conexões rectroactivas são ignoradas.
                            columnState = columnEnumerator.MoveNext();
                        }
                        else if (currentColumnNumber < colSchema)
                        {
                            var columnDisplacement = currentColumnNumber - prevColSchema;
                            while (weightsPointer < columnDisplacement
                                && weightsPointer < thisWeightsLength)
                            {
                                thisWeights[weightsPointer++] = weightsMatrix.DefaultValue;
                            }

                            if (weightsPointer < thisWeightsLength)
                            {
                                thisWeights[weightsPointer++] = currentColumnValue;
                                columnState = columnEnumerator.MoveNext();
                            }
                            else
                            {
                                columnState = false;
                            }
                        }
                        else
                        {
                            // Conexões para camadas não contíguas são ignoradas.
                            columnState = false;
                        }
                    }

                    lineState = linesEnumerator.MoveNext();
                }
                else
                {
                    var schemaLength = this.schema.LongLength;
                    while (lineSchema <= currentLineNumber)
                    {
                        while (linePointer < lineSchema)
                        {
                            var thisWeights = this.weights[linePointer++];
                            var length = thisWeights.LongLength;
                            for (var i = 0L; i < length; ++i)
                            {
                                thisWeights[i] = weightsMatrix.DefaultValue;
                            }
                        }

                        if (pointer < schemaLength)
                        {
                            prevColSchema = colSchema;
                            colSchema += this.schema[pointer++];
                            lineSchema += this.schema[pointer];
                        }
                        else
                        {
                            // Termina o processo.
                            currentLineNumber = -1;
                            lineState = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Permite treinar a rede neuronal com base no método do gradiente descendente.
        /// </summary>
        /// <typeparam name="InputVectorType">
        /// Os tipos de objectos que constituem os vectores de entrada no padrão.
        /// </typeparam>
        /// <typeparam name="OutputVectorType">
        /// Os tipos dos objectos que constituem os vectores de saída no padrão.
        /// </typeparam>
        /// <param name="pattern">O padrão.</param>
        /// <param name="epocs">O número de épocas a aplicar no treino.</param>
        /// <param name="ring">O anel responsável pelas operações de adição e multiplicação.</param>
        /// <param name="trainingActivationFunction">
        /// A função de activação a ser usada durante o treino.
        /// (valor limiar, valor saída propagação) => valor
        /// </param>
        /// <param name="trainingPropagationFunction">
        /// A função de propagação a ser usada durante o treino.
        /// (vector peso nó actual, vector nós camada anterior, tamanho) => valor
        /// </param>
        /// <param name="trainingDiffActivFunc">
        /// Derivada relativa à função de activação em função do valor do nó.
        /// (valor nó actual) => valor.
        /// </param>
        /// <param name="trainingDiffPropFunc">
        /// Derivada relativa à função de propagação.
        /// (vector peso nó actual, vector nós camada anterior, índice) => valor
        /// </param>
        /// <param name="initializationAct">
        /// A acção responsável pela inicialização dos parâmetros.
        /// </param>
        private void InnerTrain<InputVectorType, OutputVectorType>(
            NeuralNetworkTrainingPattern<CoeffType, InputVectorType, OutputVectorType>[] pattern,
            long epocs,
            IRing<CoeffType> ring,
            Func<CoeffType, CoeffType, CoeffType> trainingActivationFunction,
            Func<CoeffType[], CoeffType[], long, CoeffType> trainingPropagationFunction,
            Func<CoeffType, CoeffType> trainingDiffActivFunc,
            Func<CoeffType[], CoeffType[], long, CoeffType> trainingDiffPropFunc,
            Action<CoeffType[], CoeffType[][]> initializationAct)
            where InputVectorType : IVector<CoeffType>
            where OutputVectorType : IVector<CoeffType>
        {
            var innerTresholds = this.tresholds;
            var innerWeights = this.weights;
            var nodesLength = innerTresholds.LongLength;
            var deltaTreshold = new CoeffType[nodesLength];
            var deltaWeights = this.ReserveSpace(
                nodesLength,
                this.schema);
            var outputs = this.ReserveOutputs(
                this.schema);

            initializationAct.Invoke(
                innerTresholds,
                innerWeights);

            var patternLength = pattern.LongLength;
            for (var i = 0L; i < epocs; ++i)
            {
                this.SetVectorToZero(deltaTreshold, ring);
                this.SetMatrixToZero(deltaWeights, ring);
                for (var j = 0L; j < patternLength; ++j)
                {
                    var currPattern = pattern[j];
                    var currPatternInput = currPattern.Input;
                    var currPatternOutput = currPattern.Output;

                    this.ComputeLayerOutputs(
                        currPatternInput,
                        outputs,
                        trainingActivationFunction,
                        trainingPropagationFunction);

                    this.ComputeDeltas(
                        deltaTreshold,
                        deltaWeights,
                        currPatternInput,
                        currPatternOutput,
                        outputs,
                        ring,
                        trainingDiffActivFunc,
                        trainingDiffPropFunc);

                    // Adiciona os valores calculados
                    this.AddComputedValues(
                        deltaTreshold,
                        deltaWeights,
                        innerTresholds,
                        innerWeights,
                        ring);
                }
            }
        }

        /// <summary>
        /// Determina os deltas dos valores limiares e dos pesos.
        /// </summary>
        /// <param name="tresholdDeltas">
        /// O vector que irá conter os deltas dos valores limiar.
        /// </param>
        /// <param name="weightsDeltas">
        /// Matriz que irá conter os deltas dos pesos.
        /// </param>
        /// <param name="inputVector">O vector de entrada.</param>
        /// <param name="outVector">O vector de saída do padrão.</param>
        /// <param name="outputs">A matriz das saídas de cada nó.</param>
        /// <param name="ring">
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </param>
        /// <param name="trainingDiffActivFunc">
        /// Derivada relativa à função de activação em função do valor do nó.
        /// (valor nó actual) => valor.
        /// </param>
        /// <param name="trainingDiffPropFunc">
        /// Derivada relativa à função de propagação.
        /// (vector peso nó actual, vector nós camada anterior, índice) => valor
        /// </param>
        private void ComputeDeltas<InputVectorType, OutputVectorType>(
            CoeffType[] tresholdDeltas,
            CoeffType[][] weightsDeltas,
            InputVectorType inputVector,
            OutputVectorType outVector,
            CoeffType[][] outputs,
            IRing<CoeffType> ring,
            Func<CoeffType, CoeffType> trainingDiffActivFunc,
            Func<CoeffType[], CoeffType[], long, CoeffType> trainingDiffPropFunc)
            where InputVectorType : IVector<CoeffType>
            where OutputVectorType : IVector<CoeffType>
        {
            var innerTresholds = this.tresholds;
            var innerWeights = this.weights;
            var innerSchema = this.schema;

            var betas = new CoeffType[this.max];
            var nodesPointer = innerTresholds.LongLength - 1L;
            var schemaPointer = innerSchema.LongLength - 1;

            // Cálculo dos primeiros betas
            var currentSchema = innerSchema[schemaPointer--];
            var prevOutVector = outputs[schemaPointer];
            for (var i = 0L; i < currentSchema; ++i)
            {
                var currPat = outVector[i];
                var currOut = prevOutVector[i];
                currOut = ring.AdditiveInverse(currOut);
                betas[i] = ring.Add(currOut, currPat);

                prevOutVector[i] = trainingDiffActivFunc(
                    prevOutVector[i]);
            }

            var computedBetas = new CoeffType[this.max];
            while (schemaPointer > 0)
            {
                var prevSchema = currentSchema;
                currentSchema = innerSchema[schemaPointer--];
                var currOutVector = outputs[schemaPointer];

                for (var i = prevSchema - 1; i >= 0L; --i)
                {
                    var currentBeta = betas[i];
                    var currentPrevOut = prevOutVector[i];

                    // TODO: Actualização do valor limiar - considerar parâmetro de aprendizagem
                    var value = currentPrevOut;
                    value = ring.Multiply(
                        value,
                        currentBeta);
                    value = ring.AdditiveInverse(value);
                    tresholdDeltas[nodesPointer] = ring.Add(
                        value,
                        tresholdDeltas[nodesPointer]);

                    var currWeigthDeltas = weightsDeltas[nodesPointer];
                    var currweights = innerWeights[nodesPointer];

                    // Início do ciclo
                    value = currentPrevOut;
                    value = ring.Multiply(
                        value,
                        currOutVector[0]);
                    value = ring.Multiply(
                        value,
                        currentBeta);

                    // TODO: Actualização do peso - considerar parâmetro de aprendizagem
                    currWeigthDeltas[0] = ring.Add(
                        currWeigthDeltas[0],
                        value);

                    this.InitializeComputedBetas(
                        currentBeta,
                        currentPrevOut,
                        currweights,
                        currentSchema,
                        computedBetas,
                        ring);

                    for (var j = 1L; j < currentSchema; ++j)
                    {
                        value = currentPrevOut;
                        value = ring.Multiply(
                            value,
                            currOutVector[j]);
                        value = ring.Multiply(
                            value,
                            currentBeta);

                        // TODO: Actualização do peso - considerar parâmetros de aprendizagem
                        currWeigthDeltas[j] = ring.Add(
                            currWeigthDeltas[j],
                            value);

                        this.UpdateComputedBetas(
                            currentBeta,
                            currentPrevOut,
                            currweights,
                            currentSchema,
                            computedBetas,
                            ring);
                    }

                    --nodesPointer;
                }

                betas = computedBetas;
                prevOutVector = currOutVector;
                for (var i = 0L; i < currentSchema; ++i)
                {
                    prevOutVector[i] = trainingDiffActivFunc.Invoke(prevOutVector[i]);
                }
            }

            // Resta a determinação final
            var outPrevSchema = currentSchema;
            currentSchema = innerSchema[schemaPointer];

            for (var i = outPrevSchema - 1; i >= 0L; --i)
            {
                var currentPrevOut = prevOutVector[i];
                var currentBeta = betas[i];
                var value = currentPrevOut;
                value = ring.Multiply(
                        value,
                        currentBeta);
                value = ring.AdditiveInverse(value);
                tresholdDeltas[nodesPointer] = ring.Add(
                        tresholdDeltas[nodesPointer],
                        value);

                var currWeigthDeltas = weightsDeltas[nodesPointer];
                for (var j = 0L; j < currentSchema; ++j)
                {
                    value = currentPrevOut;
                    value = ring.Multiply(
                        value,
                        inputVector[j]);
                    value = ring.Multiply(
                        value,
                        currentBeta);

                    // TODO: Actualização do peso - considerar parâmetros de aprendizagem
                    currWeigthDeltas[j] = ring.Add(
                        currWeigthDeltas[j],
                        value);
                }

                --nodesPointer;
            }
        }

        /// <summary>
        /// Adiciona os valores dos deltas aos vectores dos valores.
        /// </summary>
        /// <param name="tresholdDeltas">Os deltas dos valores limiar.</param>
        /// <param name="weightDeltas">Os deltas dos pesos.</param>
        /// <param name="tresholds">O vector dos valores limiar.</param>
        /// <param name="weights">O vector dos pesos.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        private void AddComputedValues(
            CoeffType[] tresholdDeltas,
            CoeffType[][] weightDeltas,
            CoeffType[] tresholds,
            CoeffType[][] weights,
            IRing<CoeffType> ring)
        {
            var length = tresholds.LongLength;
            for (var i = 0L; i < length; ++i)
            {
                tresholds[i] = ring.Add(
                    tresholds[i],
                    tresholdDeltas[i]);
                var currWeightDeltas = weightDeltas[i];
                var currWeight = weights[i];
                var weightLength = currWeight.LongLength;
                for (var j = 0L; j < weightLength; ++j)
                {
                    currWeight[j] = ring.Add(
                        currWeight[j],
                        currWeightDeltas[j]);
                }
            }
        }

        /// <summary>
        /// Inicializa o cálculo dos novos betas.
        /// </summary>
        /// <param name="currentBeta">O valor beta actual.</param>
        /// <param name="prevVectorValue">
        /// O valor previamente calculado com a derivada da função de activação.
        /// </param>
        /// <param name="weights">O vector dos pesos do nó actual.</param>
        /// <param name="schema">O número de nós na camada actual.</param>
        /// <param name="computedBetas">
        /// O vector que irá receber a computação dos novos betas.
        /// </param>
        /// <param name="ring">
        /// O anel responsável pelas operações sober os coeficientes.
        /// </param>
        private void InitializeComputedBetas(
            CoeffType currentBeta,
            CoeffType prevVectorValue,
            CoeffType[] weights,
            long schema,
            CoeffType[] computedBetas,
            IRing<CoeffType> ring)
        {
            for (var i = 0L; i < schema; ++i)
            {
                var value = ring.Multiply(
                    currentBeta,
                    prevVectorValue);
                value = ring.Multiply(
                    value,
                    weights[i]);
                computedBetas[i] = value;
            }
        }

        /// <summary>
        /// Actualiza o cálculo dos novos betas.
        /// </summary>
        /// <param name="currentBeta">O valor beta actual.</param>
        /// <param name="prevVectorValue">
        /// O valor previamente calculado com a derivada da função de activação.
        /// </param>
        /// <param name="weights">O vector dos pesos do nó actual.</param>
        /// <param name="schema">O número de nós na camada actual.</param>
        /// <param name="computedBetas">
        /// O vector que irá receber a computação dos novos betas.
        /// </param>
        /// <param name="ring">
        /// O anel responsável pelas operações sober os coeficientes.
        /// </param>
        private void UpdateComputedBetas(
            CoeffType currentBeta,
            CoeffType prevVectorValue,
            CoeffType[] weights,
            long schema,
            CoeffType[] computedBetas,
            IRing<CoeffType> ring)
        {
            for (var i = 0L; i < schema; ++i)
            {
                var value = ring.Multiply(
                    currentBeta,
                    prevVectorValue);
                value = ring.Multiply(
                    value,
                    weights[i]);
                computedBetas[i] = ring.Add(
                    computedBetas[i],
                    value);
            }
        }

        /// <summary>
        /// Calcula os resultados intermédios.
        /// </summary>
        /// <typeparam name="InputVectorType">
        /// O tipo dos objectos que constituem o conjunto de valores de entrada.
        /// </typeparam>
        /// <param name="input">O vector com os valores de entrada.</param>
        /// <param name="outputMatrix">
        /// A matriz que irá conter o resultados intermédios.
        /// </param>
        /// <param name="trainingActivationFunction">
        /// A função de activação a ser usada durante o treino.
        /// </param>
        /// <param name="trainingPropagationFunction">
        /// A função de propagação a ser usada durante o treino.
        /// </param>
        private void ComputeLayerOutputs<InputVectorType>(
            InputVectorType input,
            CoeffType[][] outputMatrix,
            Func<CoeffType, CoeffType, CoeffType> trainingActivationFunction,
            Func<CoeffType[], CoeffType[], long, CoeffType> trainingPropagationFunction)
            where InputVectorType : IVector<CoeffType>
        {
            var innerSchema = this.schema;
            var innerTreshold = this.tresholds;
            var innerWeight = this.weights;
            var schemaLength = innerSchema.LongLength;
            var nodePointer = 0L;

            var currSchema = innerSchema[0L];
            var inVector = new CoeffType[input.LongLength];
            input.CopyTo(inVector, 0L);
            var prevSchema = input.LongLength;
            for (var i = 1L; i < schemaLength; ++i)
            {
                currSchema = innerSchema[i];
                var outVector = outputMatrix[i - 1];
                for (var j = 0L; j < currSchema; ++j)
                {
                    var currWeight = innerWeight[nodePointer];
                    var currValue = trainingPropagationFunction(
                        currWeight,
                        inVector,
                        prevSchema);
                    var currTreshold = innerTreshold[nodePointer];
                    currValue = trainingActivationFunction(
                        currTreshold,
                        currValue);
                    outVector[j] = currValue;
                    ++nodePointer;
                }

                inVector = outVector;
                prevSchema = currSchema;
            }
        }

        /// <summary>
        /// Inicializa o vector das variações do valor limiar.
        /// </summary>
        /// <param name="tresholds">
        /// O vector dos valores limiar a ser inicializado.
        /// </param>
        /// <param name="ring">
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </param>
        private void InitializeTreshold(
            CoeffType[] tresholds,
            IRing<CoeffType> ring)
        {
            this.SetVectorToZero(tresholds, ring);
        }

        /// <summary>
        /// Inicializa a matriz de pesos.
        /// </summary>
        /// <param name="weights">
        /// A matriz de pesos a ser inicializada.
        /// </param>
        /// <param name="ring">
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </param>
        private void InitializeWeights(
            CoeffType[][] weights,
            IRing<CoeffType> ring)
        {
            this.SetMatrixToZero(weights, ring);
        }

        /// <summary>
        /// Coloca o vector a zero.
        /// </summary>
        /// <param name="vector">O vector.</param>
        /// <param name="ring">
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </param>
        private void SetVectorToZero(
            CoeffType[] vector,
            IRing<CoeffType> ring)
        {
            Utils.FillArray(
                vector,
                ring.AdditiveUnity);
        }

        /// <summary>
        /// Coloca as colunas da matriz a zero.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <param name="ring">
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </param>
        private void SetMatrixToZero(
            CoeffType[][] matrix,
            IRing<CoeffType> ring)
        {
            var length = matrix.LongLength;
            for (var i = 0L; i < length; ++i)
            {
                var curr = matrix[i];
                Utils.FillArray(
                    curr,
                    ring.AdditiveUnity);
            }
        }

        /// <summary>
        /// Reserva espaço para os resultados intermédios.
        /// </summary>
        /// <param name="schema">O esquema.</param>
        /// <returns>O espaço reservado.</returns>
        private CoeffType[][] ReserveOutputs(
            long[] schema)
        {
            var schemaLength = schema.LongLength - 1;
            var result = new CoeffType[schemaLength][];
            for (var i = 0L; i < schemaLength; ++i)
            {
                var currSchema = schema[i + 1];
                result[i] = new CoeffType[currSchema];
            }

            return result;
        }

        /// <summary>
        /// Reserva espaço necessário para o armazenamento de dados internos.
        /// </summary>
        /// <param name="sum">O número de nós na rede.</param>
        /// <param name="schema">O esquema que define a topologia de camadas.</param>
        /// <returns>O espaço reservado.</returns>
        private CoeffType[][] ReserveSpace(
            long sum,
            long[] schema)
        {
            var length = schema.LongLength;
            var innerWeights = new CoeffType[sum][];

            var pointer = 0L;
            var prevSchema = schema[0];
            for (var i = 1L; i < length; ++i)
            {
                var currSchema = schema[i];
                for (var j = 0L; j < currSchema; ++j)
                {
                    innerWeights[pointer++] = new CoeffType[prevSchema];
                }

                prevSchema = currSchema;
            }

            return innerWeights;
        }
    }
}
