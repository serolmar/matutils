namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa uma matriz de bits como um vector de listas de "bits".
    /// </summary>
    public class ArrayBitMatrix : IMatrix<int>
    {
        /// <summary>
        /// O valor por defeito.
        /// </summary>
        private int defaultValue;

        /// <summary>
        /// A lista com os elementos.
        /// </summary>
        private BitList[] elementsList;

        /// <summary>
        /// O número de colunas na matriz.
        /// </summary>
        private int columnsNumber;

        public ArrayBitMatrix(int lines, int columns)
            : this(lines, columns, 0)
        {
        }

        public ArrayBitMatrix(int lines, int columns, int defaultValue)
        {
            if (lines < 0)
            {
                throw new ArgumentException("Parameter lines must be non-negative.");
            }
            else if (columns < 0)
            {
                throw new ArgumentException("Parameter columns must be non-negative.");
            }
            else
            {
                this.columnsNumber = columns;
                this.elementsList = new BitList[lines];
                this.defaultValue = defaultValue == 0 ? 0 : 1;
                for (int i = 0; i < lines; ++i)
                {
                    this.elementsList[i] = new BitList();
                }
            }
        }

        public int this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.elementsList.Length)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else if (column < 0 || column >= this.columnsNumber)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else
                {
                    var bitListLine = this.elementsList[line];
                    if (column < bitListLine.Count)
                    {
                        return bitListLine[column];
                    }
                    else
                    {
                        return this.defaultValue;
                    }
                }
            }
            set
            {
                if (line < 0 || line >= this.elementsList.Length)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else if (column < 0 || column >= this.columnsNumber)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else
                {
                    var bitListLine = this.elementsList[line];
                    if (column >= bitListLine.Count)
                    {
                        for (int i = bitListLine.Count; i < column; ++i)
                        {
                            bitListLine.Add(this.defaultValue);
                        }

                        bitListLine.Add(value);
                    }
                    else
                    {
                        bitListLine[column] = value;
                    }
                }
            }
        }

        public bool IsSymmetric(IEqualityComparer<int> equalityComparer)
        {
            var innerEqualityComparer = equalityComparer;
            if (innerEqualityComparer == null)
            {
                innerEqualityComparer = EqualityComparer<int>.Default;
            }

            if (this.elementsList.Length != this.columnsNumber)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < this.elementsList.Length; ++i)
                {
                    for (int j = i + 1; j < this.columnsNumber; ++j)
                    {
                        var currentEntry = this.elementsList[i][j];
                        var symmetricEntry = this.elementsList[j][i];
                        if (!innerEqualityComparer.Equals(currentEntry, symmetricEntry))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.elementsList.Length;
            }
            else if (dimension == 1)
            {
                return this.columnsNumber;
            }
            else
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
        }

        public IMatrix<int> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<int>(this, lines, columns);
        }

        public IMatrix<int> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<int>(this, lines, columns);
        }

        public void SwapLines(int i, int j)
        {
            if (i < 0 || i > this.elementsList.Length)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.elementsList.Length)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                var swapLine = this.elementsList[i];
                this.elementsList[i] = this.elementsList[j];
                this.elementsList[j] = swapLine;
            }
        }

        public void SwapColumns(int i, int j)
        {
            if (i < 0 || i > this.columnsNumber)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.columnsNumber)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                for (int k = 0; k < this.elementsList.Length; ++k)
                {
                    var currentBitListLine = this.elementsList[k];

                    if (i >= currentBitListLine.Count)
                    {
                        if (j < currentBitListLine.Count)
                        {
                            var swapColumn = currentBitListLine[j];

                            // Troca quando o valor não é o que se encontra por defeito.
                            if (swapColumn != this.defaultValue)
                            {
                                for (int l = 0; l < i; ++l)
                                {
                                    currentBitListLine.Add(this.defaultValue);
                                }

                                currentBitListLine[i] = swapColumn;
                                currentBitListLine[j] = this.defaultValue;
                            }
                        }
                    }
                    else if (j >= currentBitListLine.Count)
                    {
                        if (i < currentBitListLine.Count)
                        {
                            var swapColumn = currentBitListLine[i];

                            // Troca quando o valor não é o que se encontra por defeito.
                            if (swapColumn != this.defaultValue)
                            {
                                for (int l = 0; l < j; ++l)
                                {
                                    currentBitListLine.Add(this.defaultValue);
                                }

                                currentBitListLine[j] = swapColumn;
                                currentBitListLine[i] = this.defaultValue;
                            }
                        }
                    }
                    else
                    {
                        var swapColumn = currentBitListLine[i];
                        this.elementsList[k][i] = currentBitListLine[j];
                        this.elementsList[k][j] = swapColumn;
                    }
                }
            }
        }

        /// <summary>
        /// Adiciona uma matriz à matriz actual de modo que a a soma dos compoentes seja realizada módulo dois.
        /// </summary>
        /// <param name="right">A matriz a ser adicionada.</param>
        /// <returns>O resultado da soma de ambas as matrizes.</returns>
        public ArrayBitMatrix AddModuloTwo(ArrayBitMatrix right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (this.elementsList.Length != right.elementsList.Length)
            {
                throw new ArgumentException("Can only sum matrices with the same number of lines.");
            }
            else if (this.columnsNumber != right.columnsNumber)
            {
                throw new ArgumentException("Can only sum matrices with the same number of columns.");
            }
            else
            {
                var result = new ArrayBitMatrix(this.elementsList.Length, this.columnsNumber, this.defaultValue);
                for (int i = 0; i < this.elementsList.Length; ++i)
                {
                    var currentThisMatrixLine = this.elementsList[i];
                    var currentRightMatrixLine = right.elementsList[i];
                    var j = 0;
                    for (;j < currentThisMatrixLine.Count && j < currentRightMatrixLine.Count;++j)
                    {
                        var sum = (currentThisMatrixLine[j] + currentRightMatrixLine[j]) % 2;
                        if (sum != this.defaultValue)
                        {
                            result[i, j] = sum;
                        }
                    }

                    for (; j < currentThisMatrixLine.Count; ++j)
                    {
                        var sum = (currentThisMatrixLine[j] + right.defaultValue) % 2;
                        if (sum != this.defaultValue)
                        {
                            result[i, j] = sum;
                        }
                    }

                    for (; j < currentRightMatrixLine.Count; ++j)
                    {
                        var sum = (this.defaultValue + currentRightMatrixLine[j]) % 2;
                        if (sum != this.defaultValue)
                        {
                            result[i, j] = sum;
                        }
                    }

                    if (this.defaultValue == right.defaultValue && this.defaultValue != 0)
                    {
                        for (; j < this.columnsNumber; ++j)
                        {
                            result.elementsList[i].Add(1);
                        }
                    }
                    else if(this.defaultValue != right.defaultValue && this.defaultValue != 1)
                    {
                        for (; j < currentThisMatrixLine.Count; ++j)
                        {
                            result.elementsList[i].Add(0);
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Multiplca uma matri pela matriz actual de modo que a a soma dos compoentes seja realizada módulo dois.
        /// </summary>
        /// <param name="right">A matriz a ser adicionada.</param>
        /// <returns>O resultado da soma de ambas as matrizes.</returns>
        public ArrayBitMatrix MultiplyModuloTwo(ArrayBitMatrix right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém um enumearador para todos os elementos da matriz.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < this.elementsList.Length; ++i)
            {
                var currentBitListLine = this.elementsList[i];
                var j = 0;
                for (; j < currentBitListLine.Count; ++j)
                {
                    yield return currentBitListLine[j];
                }

                for (; j < this.columnsNumber; ++j)
                {
                    yield return this.defaultValue;
                }
            }
        }

        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            var i = 0;
            if (0 < this.elementsList.Length)
            {
                resultBuilder.Append("[");
                if (0 < this.columnsNumber)
                {
                    var currentBitListLine = this.elementsList[0];
                    resultBuilder.Append(currentBitListLine[0]);
                    i = 1;
                    for (; i < currentBitListLine.Count; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", currentBitListLine[i]);
                    }

                    for (; i < this.columnsNumber; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", currentBitListLine[i]);
                    }
                }

                resultBuilder.Append("]");

                for (i = 1; i < this.elementsList.Length; ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.columnsNumber)
                    {
                        var currentBitListLine = this.elementsList[i];
                        resultBuilder.Append(currentBitListLine[0]);
                        var j = 1;
                        for (; j < currentBitListLine.Count; ++j)
                        {
                            resultBuilder.AppendFormat(", {0}", currentBitListLine[j]);
                        }

                        for (; i < this.columnsNumber; ++i)
                        {
                            resultBuilder.AppendFormat(", {0}", currentBitListLine[j]);
                        }
                    }

                    resultBuilder.Append("]");
                }

            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
