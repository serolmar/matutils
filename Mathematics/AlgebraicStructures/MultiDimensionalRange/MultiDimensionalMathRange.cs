namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Representa uma matriz multidimensional.
    /// </summary>
    /// <typeparam name="T">O tipo de dados das entradas da matriz.</typeparam>
    public class MultiDimensionalMathRange<T> : MultiDimensionalRange<T>
    {
        /// <summary>
        /// Instancia uma classe do tipo <see cref="MultiDimensionalMathRange{T}"/>, provendo a
        /// respectiva configuração.
        /// </summary>
        public MultiDimensionalMathRange()
            : base() { }

        /// <summary>
        /// Instancia uma classe do tipo <see cref="MultiDimensionalMathRange{T}"/>, provendo a
        /// respectiva configuração.
        /// </summary>
        /// <param name="dimensions">A configuração da matriz.</param>
        /// <exception cref="ArgumentException">Se a colecção das dimensões for nula.</exception>
        public MultiDimensionalMathRange(IEnumerable<int> dimensions)
            : base(dimensions) { }

        /// <summary>
        /// Determina a soma de duas matrizes multidimensionais.
        /// </summary>
        /// <param name="right">A matriz multidimensional a ser somada.</param>
        /// <param name="semiGroup">O semi-grupo responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado da soma.</returns>
        /// <exception cref="ArgumentNullException">Se um dos argumentos for nulo.</exception>
        public MultiDimensionalMathRange<T> Sum(MultiDimensionalMathRange<T> right, ISemigroup<T> semiGroup)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            if (semiGroup == null)
            {
                throw new ArgumentNullException("Semi group structure is needed.");
            }

            if (!this.CheckDimension(right.configuration))
            {
                throw new MathematicsException("Can only sum ranges with the same dimensions.");
            }

            var result = new MultiDimensionalMathRange<T>();
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

        /// <summary>
        /// Determina o produto de duas matrizes multidimensionais.
        /// </summary>
        /// <param name="right">A matriz multidimensional a ser somada.</param>
        /// <param name="multiplyable">O objeto responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado do produto.</returns>
        /// <exception cref="ArgumentNullException">Se um dos argumentos for nulo.</exception>
        public MultiDimensionalMathRange<T> Multiply(MultiDimensionalMathRange<T> right, IMultiplication<T> multiplyable)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            if (multiplyable == null)
            {
                throw new ArgumentNullException("A multiplyable structure is needed.");
            }

            var result = new MultiDimensionalMathRange<T>();
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

        /// <summary>
        /// Determina a contracção da matriz corrente segundo os índices espcificados.
        /// </summary>
        /// <param name="contractionIndices">A matriz multidimensional a ser somada.</param>
        /// <param name="semiGroup">O objeto responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado do produto.</returns>
        /// <exception cref="ArgumentNullException">Se um dos argumentos for nulo.</exception>
        public MultiDimensionalMathRange<T> Contract(int[] contractionIndices, ISemigroup<T> semiGroup)
        {
            if (contractionIndices == null)
            {
                throw new ArgumentNullException("contractionIndices");
            }

            if (semiGroup == null)
            {
                throw new MathematicsException("Parameter semiGroup can't be null.");
            }

            var result = new MultiDimensionalMathRange<T>();
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
    }
}
