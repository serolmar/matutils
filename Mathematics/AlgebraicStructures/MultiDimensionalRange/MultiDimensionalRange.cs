namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;
    using Utilities.Parsers;

    /// <summary>
    /// Representa uma matriz multidimensional.
    /// </summary>
    /// <typeparam name="T">O tipo de dados das entradas da matriz.</typeparam>
    public class MultiDimensionalRange<T> : IMultiDimensionalRange<T>
    {
        /// <summary>
        /// Contém todos os elementos.
        /// </summary>
        private T[] elements;

        /// <summary>
        /// Contém a configuração das dimensões da matriz.
        /// </summary>
        private int[] configuration;

        internal MultiDimensionalRange()
        {
        }

        /// <summary>
        /// Instancia uma classe do tipo <see cref="MultiDimensionalRange"/>, provendo a
        /// respectiva configuração.
        /// </summary>
        /// <param name="dimensions">A configuração da matriz.</param>
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
        public int Rank
        {
            get
            {
                return this.configuration.Length;
            }
        }

        public IEnumerable<int> Configuration
        {
            get
            {
                return this.configuration;
            }
        }

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

        public int GetLength(int dimension)
        {
            if (dimension < 0 || dimension >= this.configuration.Length)
            {
                throw new IndexOutOfRangeException("Parameter dimension is out of bounds.");
            }

            return this.configuration[dimension];
        }

        public MultiDimensionalRange<T> Sum(MultiDimensionalRange<T> right, ISemigroup<T> semiGroup)
        {
            if (semiGroup == null)
            {
                throw new ArgumentNullException("Semi group structure is needed.");
            }

            if (!this.CheckDimension(right.configuration))
            {
                throw new MathematicsException("Can only sum ranges with the same dimensions.");
            }

            var result = new MultiDimensionalRange<T>();
            Array.Copy(this.configuration, result.configuration, this.configuration.Length);
            var p = 1;
            for (int i = 0; i < this.configuration.Length; ++i)
            {
                p *= this.configuration[i];
            }

            result.elements = new T[p];
            for (int i = 0; i < this.elements.Length; ++i)
            {
                result.elements[i] = semiGroup.Add(this.elements[i], right.elements[i]);
            }

            return result;
        }

        public MultiDimensionalRange<T> Multiply(MultiDimensionalRange<T> right, IMultipliable<T> multiplyable)
        {
            if (multiplyable == null)
            {
                throw new ArgumentNullException("A multiplyable structure is needed.");
            }

            var result = new MultiDimensionalRange<T>();
            if (this.configuration.Length == 0 || right.configuration.Length == 0)
            {
                result.configuration = new int[0];
                result.elements = new T[0];
            }
            else
            {
                var p = 1;
                var q = 1;
                for (int i = 0; i < this.configuration.Length; ++i)
                {
                    p *= this.configuration[i];
                }

                for (int i = 0; i < right.configuration[i]; ++i)
                {
                    q *= right.configuration[i];
                }

                result.elements = new T[p * q];
                result.configuration = new int[this.configuration.Length + right.configuration.Length];
                for (int i = 0; i < this.configuration.Length; ++i)
                {
                    result.configuration[i] = this.configuration[i];
                }

                for (int i = 0; i < right.configuration.Length; ++i)
                {
                    result.configuration[this.configuration.Length + i] = right.configuration[i];
                }

                for (int i = 0; i < p; ++i)
                {
                    for (int j = 0; j < q; ++j)
                    {
                        result.elements[j * p + i] = multiplyable.Multiply(this.elements[i], right.elements[j]);
                    }
                }
            }

            return result;
        }

        public MultiDimensionalRange<T> Contract(int[] contractionIndices, ISemigroup<T> semiGroup)
        {
            if (semiGroup == null)
            {
                throw new MathematicsException("Parameter semiGroup can't be null.");
            }

            var result = new MultiDimensionalRange<T>();
            this.CheckConversion(contractionIndices);
            if (contractionIndices.Length == this.configuration.Length)
            {
                result.elements = new T[1];
                var advance = 0;
                for (int i = this.configuration.Length - 1; i > 0; --i)
                {
                    advance *= this.configuration[i];
                    advance += 1;
                }

                var pos = advance;
                for (int i = 1; i < this.configuration.Length; ++i)
                {
                    result.elements[0] = semiGroup.Add(result.elements[0], this.elements[pos]);
                    pos += advance;
                }

                result.configuration = new[] { 0 };
            }
            else
            {
                var contractionMask = new int[this.configuration.Length];
                var newConfigurationCount = 0;
                for (int i = 0; i < this.configuration.Length; ++i)
                {
                    if (!contractionIndices.Contains(i))
                    {
                        contractionMask[i] = 1;
                    }
                    else
                    {
                        ++newConfigurationCount;
                    }
                }

                var newSize = 1;
                result.configuration = new int[newConfigurationCount];
                var pointer = 0;
                for (int i = 0; i < this.configuration.Length; ++i)
                {
                    if (contractionMask[i] == 1)
                    {
                        newSize *= this.configuration[i];
                        result.configuration[pointer++] = this.configuration[i];
                    }
                }

                var oldAdvances = new int[this.configuration.Length - newConfigurationCount];
                var oldConfig = new int[oldAdvances.Length];
                pointer = oldAdvances.Length;
                var advance = 1;
                for (int i = 0; i < this.configuration.Length; ++i)
                {
                    if (contractionMask[i] == 0)
                    {
                        oldConfig[pointer] = this.configuration[i];
                        oldAdvances[pointer++] = advance;
                    }

                    advance *= this.configuration[i];
                }

                advance = 0;
                for (int i = this.configuration.Length - 1; i > 0; --i)
                {
                    advance *= this.configuration[i];
                    advance += contractionMask[i];
                }

                result.elements = new T[newSize];
                var oldCoords = new int[oldConfig.Length];
                pointer = 0;
                for (int i = 0; i < newSize; ++i)
                {
                    result.elements[i] = this.elements[pointer];
                    while (pointer < this.elements.Length)
                    {
                        result.elements[i] = semiGroup.Add(result.elements[i], result.elements[pointer]);
                        pointer += advance;
                    }

                    pointer = this.Increment(oldCoords, oldConfig, oldAdvances);
                }
            }

            return result;
        }

        public IMultiDimensionalRange<T> GetSubMultiDimensionalRange(int[][] subRangeConfiguration)
        {
            return new SubMultiDimensionalRange<T>(this, subRangeConfiguration);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.elements.Length; ++i)
            {
                yield return this.elements[i];
            }
        }

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

        #region Private Methods
        private int Increment(int[] coords, int[] config, int[] advances)
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

        private int ComputeLinearPosition(int[] coords)
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

        private int[] ComputeCoordsFromPosition(int pos)
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

        private void CheckConversion(int[] conversionIndices)
        {
            if (conversionIndices == null)
            {
                throw new ArgumentNullException();
            }

            if (conversionIndices.Length < 2)
            {
                throw new MathematicsException("At least two conversion indices needed.");
            }

            if (conversionIndices.Length > this.configuration.Length)
            {
                throw new MathematicsException("Can't contract. Number of contract indices is greater than the number of dimensions.");
            }

            for (int i = 0; i < conversionIndices.Length; ++i)
            {
                if (conversionIndices[i] >= this.configuration.Length || conversionIndices[i] < 0)
                {
                    throw new MathematicsException("Can't contract. Number of contract indices is greater than the dimension.");
                }
            }

            for (int i = 0; i < conversionIndices.Length; ++i)
            {
                var temp = conversionIndices[i];
                for (int j = i + 1; j < conversionIndices.Length; ++j)
                {
                    if (temp == conversionIndices[j])
                    {
                        throw new MathematicsException("Repeated indices in contraction.");
                    }
                }
            }

            var comp = this.configuration[conversionIndices[0]];
            for (int i = 1; i < conversionIndices.Length; ++i)
            {
                if (this.configuration[conversionIndices[i]] != comp)
                {
                    throw new MathematicsException("All indices must be the same.");
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private bool CheckDimension(int[] dimension)
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
        #endregion
    }
}
