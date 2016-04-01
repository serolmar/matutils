namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Representa uma matriz de bits como um vector de listas de "bits".
    /// </summary>
    public class ArrayBitMathMatrix : IMathMatrix<int>
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

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayBitMathMatrix"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        public ArrayBitMathMatrix(int lines, int columns)
            : this(lines, columns, 0)
        {
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayBitMathMatrix"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        /// <param name="defaultValue">O valor pode defeito.</param>
        /// <exception cref="ArgumentException">Se o número de linhas ou de colunas for negativo.</exception>
        public ArrayBitMathMatrix(int lines, int columns, int defaultValue)
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

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for inferior a zero não for inferior ao tamanho da dimensão.
        /// </exception>
        public int this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.elementsList.Length)
                {
                    throw new IndexOutOfRangeException(
                        "Parameter line must be non-negative and less than the size of matrix.");
                }
                else if (column < 0 || column >= this.columnsNumber)
                {
                    throw new IndexOutOfRangeException(
                        "Parameter line must be non-negative and less than the size of matrix.");
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
                    throw new IndexOutOfRangeException(
                        "Parameter line must be non-negative and less than the size of matrix.");
                }
                else if (column < 0 || column >= this.columnsNumber)
                {
                    throw new IndexOutOfRangeException(
                        "Parameter line must be non-negative and less than the size of matrix.");
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

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for inferior a zero não for inferior ao tamanho da dimensão.
        /// </exception>
        public int this[long line, long column]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se a dimensão for inferior a zero ou não for inferior ao número de dimensões.
        /// </exception>
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

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se a dimensão for inferior a zero ou não for inferior ao número de dimensões.
        /// </exception>
        public long GetLongLength(int dimension)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<int> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<int>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<int> GetSubMatrix(long[] lines, long[] columns)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<int> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<int>(this, lines, columns);
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice de cada linha for inferior a zero não for inferior ao tamanho da dimensão.
        /// </exception>
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

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice de cada linha for inferior a zero não for inferior ao tamanho da dimensão.
        /// </exception>
        public void SwapLines(long i, long j)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice de cada coluna for inferior a zero não for inferior ao tamanho da dimensão.
        /// </exception>
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
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice de cada coluna for inferior a zero não for inferior ao tamanho da dimensão.
        /// </exception>
        public void SwapColumns(long i, long j)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        public void ScalarLineMultiplication(int line, int scalar, IRing<int> ring)
        {
            if (line < 0 || line >= this.elementsList.Length)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                var currentLineValue = this.elementsList[line];

                // Se o escalar proporcionado for uma unidade aditiva, a linha irá conter todos os valores.
                if (ring.IsAdditiveUnity(scalar))
                {
                    var lineLength = currentLineValue.Count;
                    for (int i = 0; i < lineLength; ++i)
                    {
                        currentLineValue[i] = scalar;
                    }
                }
                else if (!ring.IsMultiplicativeUnity(scalar))
                {
                    var lineLength = currentLineValue.Count;
                    for (int i = 0; i < lineLength; ++i)
                    {
                        var columnValue = currentLineValue[i];
                        if (!ring.IsAdditiveUnity(columnValue))
                        {
                            columnValue = ring.Multiply(scalar, columnValue);
                            currentLineValue[i] = columnValue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Substitui a linha especificada por uma combinação linear desta com uma outra. Por exemplo, li = a * li + b * lj, isto é,
        /// a linha i é substituída pela soma do produto de a pela linha i com o produto de b peloa linha j.
        /// </summary>
        /// <param name="i">A linha a ser substituída.</param>
        /// <param name="j">A linha a ser combinada.</param>
        /// <param name="a">O escalar a ser multiplicado pela primeira linha.</param>
        /// <param name="b">O escalar a ser multiplicado pela segunda linha.</param>
        /// <param name="ring">O objecto responsável pelas operações sobre os coeficientes.</param>
        public void CombineLines(int i, int j, int a, int b, IRing<int> ring)
        {
            var lineslength = this.elementsList.Length;
            if (i < 0 || i >= lineslength)
            {
                throw new ArgumentNullException("i");
            }
            else if (j < 0 || j >= lineslength)
            {
                throw new ArgumentNullException("j");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                var replacementLine = this.elementsList[i];
                if (ring.IsAdditiveUnity(a))
                {
                    if (ring.IsAdditiveUnity(b))
                    {
                        var replacementLineLenght = replacementLine.Count;
                        for (int k = 0; k < replacementLineLenght; ++k)
                        {
                            replacementLine[k] = a;
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var combinationLine = this.elementsList[j];
                        var replacementLineLenght = replacementLine.Count;
                        for (int k = 0; k < replacementLineLenght; ++k)
                        {
                            replacementLine[k] = combinationLine[k];
                        }
                    }
                    else
                    {
                        var combinationLine = this.elementsList[j];
                        var replacementLineLenght = replacementLine.Count;
                        for (int k = 0; k < replacementLineLenght; ++k)
                        {
                            var value = combinationLine[k];
                            if (ring.IsAdditiveUnity(value))
                            {
                                replacementLine[k] = value;
                            }
                            else if (ring.IsMultiplicativeUnity(value))
                            {
                                replacementLine[k] = b;
                            }
                            else
                            {
                                replacementLine[k] = ring.Multiply(b, value);
                            }
                        }
                    }
                }
                else
                {
                    if (ring.IsAdditiveUnity(b))
                    {
                        if (!ring.IsMultiplicativeUnity(a))
                        {
                            var replacementLineLenght = replacementLine.Count;
                            for (int k = 0; k < replacementLineLenght; ++k)
                            {
                                var value = replacementLine[k];
                                replacementLine[k] = ring.Multiply(a, value);
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var combinationLine = this.elementsList[j];
                        var replacementLineLenght = replacementLine.Count;
                        if (ring.IsMultiplicativeUnity(a))
                        {
                            for (int k = 0; k < replacementLineLenght; ++k)
                            {
                                replacementLine[k] = ring.Add(replacementLine[k], combinationLine[k]);
                            }
                        }
                        else
                        {
                            for (int k = 0; k < replacementLineLenght; ++k)
                            {
                                var replacementValue = ring.Multiply(replacementLine[k], a);
                                replacementLine[k] = ring.Add(replacementValue, combinationLine[k]);
                            }
                        }
                    }
                    else
                    {
                        var combinationLine = this.elementsList[j];
                        var replacementLineLenght = replacementLine.Count;
                        for (int k = 0; k < replacementLineLenght; ++k)
                        {
                            var replacementValue = ring.Multiply(replacementLine[k], a);
                            var combinationValue = ring.Multiply(combinationLine[k], b);
                            replacementLine[k] = ring.Add(replacementValue, combinationValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adiciona uma matriz à matriz actual de modo que a a soma dos compoentes seja realizada módulo dois.
        /// </summary>
        /// <remarks>
        /// O resultado terá como valor por defeito o valor associado ao objecto corrente.
        /// </remarks>
        /// <param name="right">A matriz a ser adicionada.</param>
        /// <returns>O resultado da soma de ambas as matrizes.</returns>
        /// <exception cref="ArgumentNullException">Se a matriz a adicionar for nula.</exception>
        /// <exception cref="ArgumentException">
        /// Se as dimensões da matriz a adicionar não conicidirem com as dimensões da matriz corrente.
        /// </exception>
        public ArrayBitMathMatrix AddModuloTwo(ArrayBitMathMatrix right)
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
                var result = new ArrayBitMathMatrix(this.elementsList.Length, this.columnsNumber, this.defaultValue);
                for (int i = 0; i < this.elementsList.Length; ++i)
                {
                    var currentThisMatrixLine = this.elementsList[i];
                    var currentRightMatrixLine = right.elementsList[i];
                    var minLine = Math.Min(currentThisMatrixLine.Count, currentRightMatrixLine.Count);
                    var j = 0;
                    for (; j < minLine; ++j)
                    {
                        var leftValue = currentThisMatrixLine[j];
                        var rightValue = currentRightMatrixLine[j];
                        var sum = (leftValue & ~rightValue) | (~leftValue & rightValue);
                        if (sum != this.defaultValue)
                        {
                            result[i, j] = sum;
                        }
                    }

                    for (; j < currentThisMatrixLine.Count; ++j)
                    {
                        var leftValue = currentThisMatrixLine[j];
                        var rightValue = right.defaultValue;
                        var sum = (leftValue & ~rightValue) | (~leftValue & rightValue);
                        if (sum != this.defaultValue)
                        {
                            result[i, j] = sum;
                        }
                    }

                    for (; j < currentRightMatrixLine.Count; ++j)
                    {
                        var leftValue = this.defaultValue;
                        var rightValue = currentRightMatrixLine[j];
                        var sum = (leftValue & ~rightValue) | (~leftValue & rightValue);
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
                    else if (this.defaultValue != right.defaultValue && this.defaultValue != 1)
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
        /// Multiplca uma matriz pela matriz actual de modo que a a soma dos compoentes seja realizada módulo dois.
        /// </summary>
        /// <remarks>
        /// O resultado terá como valor por defeito o valor associado ao objecto corrente.
        /// </remarks>
        /// <param name="right">A matriz a ser adicionada.</param>
        /// <returns>O resultado da soma de ambas as matrizes.</returns>
        /// <exception cref="ArgumentNullException">Se a matriz a multiplicar for nula.</exception>
        /// <exception cref="MathematicsException">
        /// Se o número de linhas da matriz corrente não coincidir com o número de colunas da matriz a multplicar.
        /// </exception>
        public ArrayBitMathMatrix MultiplyModuloTwo(ArrayBitMathMatrix right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (this.columnsNumber != right.columnsNumber)
            {
                throw new MathematicsException(
                    "The number of columns in first matrix must match the number of lines in second matrix.");
            }
            else
            {
                var result = new ArrayBitMathMatrix(this.elementsList.Length, right.columnsNumber, 0);
                for (int i = 0; i < this.elementsList.Length; ++i)
                {
                    var currentLeftLine = this.elementsList[i];
                    for (int j = 0; j < currentLeftLine.Count; ++j)
                    {
                        var sum = 0;
                        var k = 0;
                        for (; k < currentLeftLine.Count; ++k)
                        {
                            var rightLine = right.elementsList[k];
                            var value = right.defaultValue;
                            if (j < rightLine.Count)
                            {
                                value = rightLine[j];
                            }

                            var prod = currentLeftLine[k] & value;
                            sum = (sum & ~prod) | (~sum & prod);
                        }

                        if (this.defaultValue == 1)
                        {
                            for (; k < this.columnsNumber; ++k)
                            {
                                var rightLine = right.elementsList[k];
                                var value = right.defaultValue;
                                if (j < rightLine.Count)
                                {
                                    value = rightLine[j];
                                }

                                sum = (sum & ~value) | (~sum & value);
                            }
                        }

                        if (sum != result.defaultValue)
                        {
                            var currentResultLine = result.elementsList[i];
                            k = currentResultLine.Count;
                            while (k < j)
                            {
                                currentResultLine.Add(result.defaultValue);
                            }

                            currentResultLine.Add(sum);
                        }
                    }
                }

                return result;
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

        /// <summary>
        /// Constrói uma representação textual da matriz.
        /// </summary>
        /// <returns>A representação textual da matriz.</returns>
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

        /// <summary>
        /// Obtém o enumerador não genércio para a matriz.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
