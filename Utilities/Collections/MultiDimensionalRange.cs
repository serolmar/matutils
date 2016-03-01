namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma matriz multidimensional.
    /// </summary>
    /// <typeparam name="T">O tipo de dados das entradas da matriz.</typeparam>
    public class MultiDimensionalRange<T> : IMultiDimensionalRange<T>
    {
        /// <summary>
        /// Contém todos os elementos.
        /// </summary>
        protected T[] elements;

        /// <summary>
        /// Contém a configuração das dimensões da matriz.
        /// </summary>
        protected int[] configuration;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="MultiDimensionalRange{T}"/>.
        /// </summary>
        public MultiDimensionalRange() { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="MultiDimensionalRange{T}"/>.
        /// </summary>
        /// <param name="dimensions">As dimensões.</param>
        public MultiDimensionalRange(IEnumerable<int> dimensions)
        {
            var p = 1;
            var config = new List<int>();
            foreach (var dimension in dimensions)
            {
                if (dimension < 0)
                {
                    throw new ArgumentException("Dimension value must be nonegative.");
                }

                p *= dimension;
                config.Add(dimension);
            }

            this.elements = new T[p];
            this.configuration = config.ToArray();
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice multidimensional.
        /// </summary>
        /// <value>
        /// O valor.
        /// </value>
        /// <param name="multiDimensionalIndex">O índice multidimensional.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Se o índice for nulo.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice se encontra fora dos limites da matriz multidimensional.
        /// </exception>
        public T this[int[] multiDimensionalIndex]
        {
            get
            {
                if (multiDimensionalIndex == null)
                {
                    throw new ArgumentNullException("muldiDimensionalIndex");
                }

                var p = this.ComputeLinearPosition(multiDimensionalIndex);
                if (p > this.elements.Length)
                {
                    throw new IndexOutOfRangeException("Parameter multiDimensionalIndex is out of range.");
                }

                return this.elements[p];
            }
            set
            {
                if (multiDimensionalIndex == null)
                {
                    throw new ArgumentNullException("muldiDimensionalIndex");
                }

                var p = this.ComputeLinearPosition(multiDimensionalIndex);
                if (p > this.elements.Length)
                {
                    throw new IndexOutOfRangeException("Parameter multiDimensionalIndex is out of range.");
                }

                this.elements[p] = value;
            }
        }

        /// <summary>
        /// Obtém o número de dimensões da matriz multidimensional.
        /// </summary>
        /// <value>O número de dimensões da matriz multidimensional.</value>
        public int Rank
        {
            get
            {
                return this.configuration.Length;
            }
        }

        /// <summary>
        /// Obtém a configuração.
        /// </summary>
        /// <value>
        /// A configuração.
        /// </value>
        public IEnumerable<int> Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        /// <summary>
        /// Obtém ou atribui os elementos internos.
        /// </summary>
        /// <value>
        /// Os elementos internos.
        /// </value>
        internal T[] InternalElements
        {
            get
            {
                return this.elements;
            }
            set
            {
                this.elements = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração interna.
        /// </summary>
        /// <value>
        /// A configuração interna.
        /// </value>
        internal int[] InnerConfiguration
        {
            get
            {
                return this.configuration;
            }
            set
            {
                this.configuration = value;
            }
        }

        /// <summary>
        /// Obtém o tamanho associado a uma dimensão.
        /// </summary>
        /// <param name="dimension">A dimensão.</param>
        /// <returns>O tamanho.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se a dimensão for negativa ou não for inferior ao número de dimensões.
        /// </exception>
        public int GetLength(int dimension)
        {
            if (dimension < 0 || dimension >= this.configuration.Length)
            {
                throw new IndexOutOfRangeException("Parameter dimension is out of bounds.");
            }

            return this.configuration[dimension];
        }

        /// <summary>
        /// Obtém uma sub-matriz multidimensional.
        /// </summary>
        /// <param name="subRangeConfiguration">A configuração da sub-matriz multidimensional.</param>
        /// <returns>A sub-matriz multidimensional.</returns>
        public IMultiDimensionalRange<T> GetSubMultiDimensionalRange(int[][] subRangeConfiguration)
        {
            return new SubMultiDimensionalRange<T>(this, subRangeConfiguration);
        }

        /// <summary>
        /// Obtém um enumerador genérico para a matriz multidimensional.
        /// </summary>
        /// <returns>O eumerador genérico.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.elements.Length; ++i)
            {
                yield return this.elements[i];
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para o alcance.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Obtém uma representação textual da matriz multidimensional.
        /// </summary>
        /// <returns>A representação textual da matriz multidimensional.</returns>
        public override string ToString()
        {
            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.Append("Range{");
            if (this.configuration.Length > 0)
            {
                for (int i = 0; i < this.elements.Length; ++i)
                {
                    resultBuilder.Append(" [");
                    var temp = i;
                    for (int j = 0; j < this.configuration.Length - 1; ++j)
                    {
                        resultBuilder.Append(temp % this.configuration[j]);
                        resultBuilder.Append(",");
                        temp = temp / this.configuration[j];
                    }

                    resultBuilder.Append(temp % this.configuration[this.configuration.Length - 1]);
                    resultBuilder.AppendFormat("]={0} ", this.elements[i]);
                }
            }

            resultBuilder.Append("}");
            return resultBuilder.ToString();
        }

        #region Funções auxiliares

        /// <summary>
        /// Incrementa um conjunto de coordenadas retornando o valor onde a coordenada é aumentada.
        /// </summary>
        /// <param name="coords">As coordenadas.</param>
        /// <param name="config">O vector de configuração.</param>
        /// <param name="advances">O vector que contém os apontadores asociados às coordenadas.</param>
        /// <returns>O valor do apontador caso exista aumento e -1 caso contrário.</returns>
        protected virtual int Increment(int[] coords, int[] config, int[] advances)
        {
            for (int i = 0; i < coords.Length; ++i)
            {
                ++coords[i];
                if (coords[i] < config[i])
                {
                    return advances[i];
                }
                else
                {
                    coords[i] = 0;
                }
            }

            return -1;
        }

        /// <summary>
        /// Permite determinar a posição linear correspondente a um vector de coordenadas.
        /// </summary>
        /// <param name="coords">O vector de coordenadas.</param>
        /// <returns>A posição linear.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se as coordenadas tiverem um excesso de dimensões ou 
        /// existir alguma coordenada negativa ou que não seja inferior ao respectivo tamanho da dimensão.</exception>
        protected virtual int ComputeLinearPosition(int[] coords)
        {
            if (coords.Length > this.configuration.Length)
            {
                throw new IndexOutOfRangeException("Parameter coords has too much dimensions.");
            }

            var result = 0;
            for (int i = coords.Length - 1; i >= 0; --i)
            {
                var currentCoord = coords[i];
                var currentConfiguration = this.configuration[i];
                if (currentCoord >= currentConfiguration || currentCoord < 0)
                {
                    throw new IndexOutOfRangeException("Every element in coords must be bound to the range configuration.");
                }

                result = result * this.configuration[i] + coords[i];
            }

            return result;
        }

        /// <summary>
        /// Determina o vector de coordenadas associado a uma posição especificada.
        /// </summary>
        /// <param name="pos">A posição.</param>
        /// <returns>O vector de coordenadas.</returns>
        protected virtual int[] ComputeCoordsFromPosition(int pos)
        {
            var result = new int[this.configuration.Length];
            var n = pos;
            for (int i = 0; i < this.configuration.Length; ++i)
            {
                result[i] = n % this.configuration[i];
                n = n / this.configuration[i];
            }

            return result;
        }

        /// <summary>
        /// Avalia se é possível contrair um alcance multidimensional.
        /// </summary>
        /// <param name="conversionIndices">Os índices de contracção.</param>
        /// <exception cref="ArgumentNullException">Se o vector de índices de conversão for nulo.</exception>
        /// <exception cref="UtilitiesException">
        /// Se o vector de índices não definir uma contracção válida.
        /// </exception>
        protected virtual void CheckConversion(int[] conversionIndices)
        {
            if (conversionIndices == null)
            {
                throw new ArgumentNullException("conversionIndices");
            }

            if (conversionIndices.Length < 2)
            {
                throw new UtilitiesException("At least two conversion indices needed.");
            }

            if (conversionIndices.Length > this.configuration.Length)
            {
                throw new UtilitiesException("Can't contract. Number of contract indices is greater than the number of dimensions.");
            }

            for (int i = 0; i < conversionIndices.Length; ++i)
            {
                if (conversionIndices[i] >= this.configuration.Length || conversionIndices[i] < 0)
                {
                    throw new UtilitiesException("Can't contract. Number of contract indices is greater than the dimension.");
                }
            }

            for (int i = 0; i < conversionIndices.Length; ++i)
            {
                var temp = conversionIndices[i];
                for (int j = i + 1; j < conversionIndices.Length; ++j)
                {
                    if (temp == conversionIndices[j])
                    {
                        throw new UtilitiesException("Repeated indices in contraction.");
                    }
                }
            }

            var comp = this.configuration[conversionIndices[0]];
            for (int i = 1; i < conversionIndices.Length; ++i)
            {
                if (this.configuration[conversionIndices[i]] != comp)
                {
                    throw new UtilitiesException("All indices must be the same.");
                }
            }
        }

        /// <summary>
        /// Verifica se o conjunto de dimensões é válido para um alcance.
        /// </summary>
        /// <param name="dimension">As dimensões a serem verificadas.</param>
        /// <returns>Verdadeiro caso as dimensões sejam válidas e falso caso contrário.</returns>
        protected virtual bool CheckDimension(int[] dimension)
        {
            if (dimension == null)
            {
                return false;
            }

            if (dimension.Length != this.configuration.Length)
            {
                return false;
            }

            for (int i = 0; i < this.configuration.Length; ++i)
            {
                if (this.configuration[i] != dimension[i])
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Funções auxiliares
    }

    /// <summary>
    /// Define um sub-alcance multidimensional-
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas dos alcances.</typeparam>
    internal class SubMultiDimensionalRange<ObjectType> : IMultiDimensionalRange<ObjectType>
    {
        /// <summary>
        /// O alcance multidimensional.
        /// </summary>
        private IMultiDimensionalRange<ObjectType> multiDimensionalRange;

        /// <summary>
        /// O conjunto de vectores que definem o sub-alcance multidimensional.
        /// </summary>
        private int[][] subMultiDimensionalRangeDefinition;

        /// <summary>
        /// Instancia um novo objecto do tiop <see cref="SubMultiDimensionalRange{ObjectType}"/>.
        /// </summary>
        /// <param name="multiDimensionalRange">O alcance multidimensional original.</param>
        /// <param name="subMultiDimensionalRangeDefinition">
        /// Os índices que definem o sub-alcance multidimensional.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Se o vector dos índices que definem o sub-alcance multidimensional for nulo.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Se o vector de índices for inválido.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// Se algum índice no vector for negativo ou não for inferior ao tamanho da respectiva dimensão.
        /// </exception>
        public SubMultiDimensionalRange(
            IMultiDimensionalRange<ObjectType> multiDimensionalRange,
            int[][] subMultiDimensionalRangeDefinition)
        {
            if (subMultiDimensionalRangeDefinition == null)
            {
                throw new ArgumentNullException("subMultiDimensionalRangeDefinition");
            }
            else if (subMultiDimensionalRangeDefinition.Length != multiDimensionalRange.Rank)
            {
                throw new ArgumentException("The number of columns in sub range configuration must match the range's rank.");
            }
            else
            {
                for (int i = 0; i < subMultiDimensionalRangeDefinition.Length; ++i)
                {
                    var currentDefinitionLine = subMultiDimensionalRangeDefinition[i];
                    var rangeLength = multiDimensionalRange.GetLength(i);
                    if (currentDefinitionLine == null)
                    {
                        throw new ArgumentException("Every dimension in sub range definition must be non null.");
                    }
                    else
                    {
                        for (int j = 0; j < currentDefinitionLine.Length; ++j)
                        {
                            var currentValue = currentDefinitionLine[j];
                            if (currentValue < 0 || currentValue > rangeLength)
                            {
                                throw new IndexOutOfRangeException("There are elements in range definition that are negative or greater than the range bounds.");
                            }
                        }
                    }
                }

                this.multiDimensionalRange = multiDimensionalRange;
                this.subMultiDimensionalRangeDefinition = new int[subMultiDimensionalRangeDefinition.Length][];
                for (int i = 0; i < this.subMultiDimensionalRangeDefinition.Length; ++i)
                {
                    var currentDefinitions = subMultiDimensionalRangeDefinition[i];
                    var columns = new int[currentDefinitions.Length];
                    Array.Copy(currentDefinitions, columns, currentDefinitions.Length);
                    this.subMultiDimensionalRangeDefinition[i] = columns;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui a entrada especificada pelas coordenadas.
        /// </summary>
        /// <value>
        /// O valor da entrada.
        /// </value>
        /// <param name="coords">As coordenadas.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se o vector de coordenadas for nulo.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Se as coordenadas não forem válidas para o alcance.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// Se algum índice no vector for negativo ou não for inferior ao tamanho da respectiva dimensão.
        /// </exception>
        public ObjectType this[int[] coords]
        {
            get
            {
                if (coords == null)
                {
                    throw new ArgumentNullException("coords");
                }
                else if (coords.Length != this.subMultiDimensionalRangeDefinition.Length)
                {
                    throw new ArgumentException("The provided coordinates don't match range configuration.");
                }
                else
                {
                    for (int i = 0; i < coords.Length; ++i)
                    {
                        var currentCoords = coords[i];
                        if (currentCoords < 0 || currentCoords >= this.subMultiDimensionalRangeDefinition[i].Length)
                        {
                            throw new IndexOutOfRangeException("There are coordinates that are negative or greater than the range bounds.");
                        }
                    }

                    var innerRangeCoords = new int[coords.Length];
                    for (int i = 0; i < coords.Length; ++i)
                    {
                        innerRangeCoords[i] = this.subMultiDimensionalRangeDefinition[i][coords[i]];
                    }

                    return this.multiDimensionalRange[innerRangeCoords];
                }
            }
            set
            {
                if (coords == null)
                {
                    throw new ArgumentNullException("coords");
                }
                else if (coords.Length != this.subMultiDimensionalRangeDefinition.Length)
                {
                    throw new ArgumentException("The provided coordinates don't match range configuration.");
                }
                else
                {
                    for (int i = 0; i < coords.Length; ++i)
                    {
                        var currentCoords = coords[i];
                        if (currentCoords < 0 || currentCoords >= this.subMultiDimensionalRangeDefinition[i].Length)
                        {
                            throw new IndexOutOfRangeException("There are coordinates that are negative or greater than the range bounds.");
                        }
                    }

                    var innerRangeCoords = new int[coords.Length];
                    for (int i = 0; i < coords.Length; ++i)
                    {
                        innerRangeCoords[i] = this.subMultiDimensionalRangeDefinition[i][coords[i]];
                    }

                    this.multiDimensionalRange[innerRangeCoords] = value;
                }
            }
        }

        /// <summary>
        /// Obtém o número de dimensões do alcance.
        /// </summary>
        /// <value>
        /// O número de dimensões do alcance.
        /// </value>
        public int Rank
        {
            get
            {
                return this.subMultiDimensionalRangeDefinition.Length;
            }
        }

        /// <summary>
        /// Obtém o tamanho associado a uma dimensão do alcance.
        /// </summary>
        /// <param name="dimension">A dimensão.</param>
        /// <returns>O tamanho associado à dimensão.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se a dimensão for negativa ou não for inferior ao número de dimensões do alcance.
        /// </exception>
        public int GetLength(int dimension)
        {
            if (dimension < 0 || dimension >= this.subMultiDimensionalRangeDefinition.Length)
            {
                throw new IndexOutOfRangeException("Parameter dimension is out of bounds.");
            }
            else
            {
                return this.subMultiDimensionalRangeDefinition[dimension].Length;
            }
        }

        /// <summary>
        /// Obtém um sub-alcance multidimensional.
        /// </summary>
        /// <param name="subRangeConfiguration">O vector de índices que define o sub-alcance multidimensional.</param>
        /// <returns>O sub-alcance multidimensional.</returns>
        public IMultiDimensionalRange<ObjectType> GetSubMultiDimensionalRange(int[][] subRangeConfiguration)
        {
            return new SubMultiDimensionalRange<ObjectType>(this, subRangeConfiguration);
        }

        /// <summary>
        /// Obtém um enumerador para todas as entradas do alcance multidimensional.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<ObjectType> GetEnumerator()
        {
            var multiDimLength = this.subMultiDimensionalRangeDefinition.Length;
            var currentCoords = new int[multiDimLength];
            var definitionPointer = new int[multiDimLength];
            for (int i = 0; i < definitionPointer.Length; ++i)
            {
                definitionPointer[i] = -1;
            }

            var generalPointer = 0;
            while (generalPointer != -1)
            {
                if (generalPointer >= multiDimLength)
                {
                    --generalPointer;
                    yield return this.multiDimensionalRange[currentCoords];
                }
                else
                {
                    ++definitionPointer[generalPointer];
                    if (definitionPointer[generalPointer] >= this.subMultiDimensionalRangeDefinition[generalPointer].Length)
                    {
                        definitionPointer[generalPointer] = -1;
                        --generalPointer;
                    }
                    else
                    {
                        currentCoords[generalPointer] = this.subMultiDimensionalRangeDefinition[generalPointer][definitionPointer[generalPointer]];
                        ++generalPointer;
                    }
                }
            }
        }

        /// <summary>
        /// Define um representação textual do alcance multidimensional.
        /// </summary>
        /// <returns>A representação textual do alcance multidimensional.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("Range{");

            var multiDimLength = this.subMultiDimensionalRangeDefinition.Length;
            var currentCoords = new int[multiDimLength];
            var definitionPointer = new int[multiDimLength];
            for (int i = 0; i < definitionPointer.Length; ++i)
            {
                definitionPointer[i] = -1;
            }

            var generalPointer = 0;
            while (generalPointer != -1)
            {
                if (generalPointer >= multiDimLength)
                {
                    resultBuilder.Append(" [");
                    if (definitionPointer.Length > 0)
                    {
                        resultBuilder.Append(definitionPointer[0]);
                        for (int i = 1; i < definitionPointer.Length; ++i)
                        {
                            resultBuilder.AppendFormat(", {0}", definitionPointer[i]);
                        }
                    }

                    resultBuilder.AppendFormat("]={0}", this.multiDimensionalRange[currentCoords]);
                    --generalPointer;
                }
                else
                {
                    ++definitionPointer[generalPointer];
                    if (definitionPointer[generalPointer] >= this.subMultiDimensionalRangeDefinition[generalPointer].Length)
                    {
                        definitionPointer[generalPointer] = -1;
                        --generalPointer;
                    }
                    else
                    {
                        currentCoords[generalPointer] = this.subMultiDimensionalRangeDefinition[generalPointer][definitionPointer[generalPointer]];
                        ++generalPointer;
                    }
                }
            }

            resultBuilder.Append("}");
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Obtém um enumerador não genérico para todas as entradas do alcance multidimensional.
        /// </summary>
        /// <returns>O enumerador não genérico para todas as entradas do alcance multidimensional.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
