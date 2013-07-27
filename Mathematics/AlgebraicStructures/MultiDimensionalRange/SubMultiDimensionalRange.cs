using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Collections;

namespace Mathematics
{
    internal class SubMultiDimensionalRange<ObjectType> : IMultiDimensionalRange<ObjectType>
    {
        private IMultiDimensionalRange<ObjectType> multiDimensionalRange;

        private int[][] subMultiDimensionalRangeDefinition;

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

        public int Rank
        {
            get
            {
                return this.subMultiDimensionalRangeDefinition.Length;
            }
        }

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

        public IMultiDimensionalRange<ObjectType> GetSubMultiDimensionalRange(int[][] subRangeConfiguration)
        {
            return new SubMultiDimensionalRange<ObjectType>(this, subRangeConfiguration);
        }

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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
