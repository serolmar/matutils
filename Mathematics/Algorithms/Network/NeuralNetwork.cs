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
    public class FeedForwardNeuralNetwork<CoeffType> : IAlgorithm<CoeffType[], CoeffType[]>
    {
        /// <summary>
        /// A função de propagação.
        /// </summary>
        /// <remarks>
        /// A função recebe, como argumentos, o vector de valores de entrada,
        /// o vector dos pesos e respectivos tamanhos. Retorna o valor propagado.
        /// </remarks>
        private Func<CoeffType[], CoeffType[], long, CoeffType> propagationFunc;

        /// <summary>
        /// A função geral de activação.
        /// </summary>
        /// <remarks>
        /// A função de activação recebe o valor limiar, o valor propagado e retorna
        /// o valor de activação.
        /// </remarks>
        private Func<CoeffType, CoeffType, CoeffType> activationFunc;

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
        /// <param name="propagationFunc">A função de propagação.</param>
        /// <param name="activationFunc">A função de activação.</param>
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
            long[] schema,
            Func<CoeffType[], CoeffType[], long, CoeffType> propagationFunc,
            Func<CoeffType, CoeffType, CoeffType> activationFunc)
        {
            if (schema == null)
            {
                throw new ArgumentNullException("schema");
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
                    this.propagationFunc = propagationFunc;
                    this.activationFunc = activationFunc;
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
        /// <returns>Os valores de saída.</returns>
        public CoeffType[] Run(CoeffType[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
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
                        return this.InnerRun(data);
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
        /// <returns>Os valores de saída.</returns>
        private CoeffType[] InnerRun(CoeffType[] data)
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
                var value = this.propagationFunc.Invoke(
                    data,
                    currWeights,
                    prevSchema);
                value = this.activationFunc.Invoke(
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
                        var value = this.propagationFunc.Invoke(
                            firstInterOut,
                            currWeights,
                            prevSchema);
                        value = this.activationFunc.Invoke(
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

            throw new NotImplementedException();
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
    }
}
