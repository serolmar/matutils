using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Collections;

namespace Mathematics
{
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
        /// <exception cref="System.ArgumentNullException">
        /// Se o vector dos índices que definem o sub-alcance multidimensional for nulo.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Se o vector de índices for inválido.
        /// </exception>
        /// <exception cref="System.IndexOutOfRangeException">
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
        /// <exception cref="System.ArgumentNullException">
        /// Se o vector de coordenadas for nulo.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Se as coordenadas não forem válidas para o alcance.
        /// </exception>
        /// <exception cref="System.IndexOutOfRangeException">
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
        /// <exception cref="System.IndexOutOfRangeException">
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
