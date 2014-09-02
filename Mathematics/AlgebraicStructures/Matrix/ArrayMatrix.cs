using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Collections;

namespace Mathematics
{
    /// <summary>
    /// Implementa uma matriz com base em vectore do sistema.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayMatrix<ObjectType> : IMatrix<ObjectType>
    {
        /// <summary>
        /// O número de linhas das matrizes.
        /// </summary>
        protected int numberOfLines;

        /// <summary>
        /// O número de colunas das matrizes.
        /// </summary>
        protected int numberOfColumns;

        /// <summary>
        /// O contentor para os coeficientes.
        /// </summary>
        protected ObjectType[][] elements;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="elements">O contentor com os elementos.</param>
        /// <param name="numberOfLines">O número de linhas.</param>
        /// <param name="numberOfColumns">O número de colunas.</param>
        internal ArrayMatrix(ObjectType[][] elements, int numberOfLines, int numberOfColumns)
        {
            this.elements = elements;
            this.numberOfLines = numberOfLines;
            this.numberOfColumns = numberOfColumns;
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o número de linhas ou colunas for negativo.</exception>
        public ArrayMatrix(int line, int column)
        {
            if (line < 0)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (column < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }
            else
            {
                this.InitializeMatrix(line, column);
            }
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o número de linhas ou colunas for negativo.</exception>
        public ArrayMatrix(int line, int column, ObjectType defaultValue)
        {
            if (line < 0)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (column < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }
            else
            {
                this.InitializeMatrix(line, column, defaultValue);
            }
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão.
        /// </exception>
        public virtual ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException(
                        "Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException(
                        "Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    return this.elements[line][column];
                }
            }
            set
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException(
                        "Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException(
                        "Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    this.elements[line][column] = value;
                }
            }
        }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        /// <exception cref="ArgumentException">Se o valor da dimensão diferir de zero ou um.</exception>
        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.numberOfLines;
            }
            else if (dimension == 1)
            {
                return this.numberOfColumns;
            }
            else
            {
                throw new ArgumentException("Parameter dimension can only take the values 0 or 1.");
            }
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ObjectType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<ObjectType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a matriz corrente como sendo um matriz quadrada.
        /// </summary>
        /// <returns>A matriz corrente como sendo uma matriz quadrada.</returns>
        /// <exception cref="MathematicsException">Se a matriz não for quadrada.</exception>
        public ISquareMatrix<ObjectType> AsSquare()
        {
            if (this.numberOfLines != this.numberOfColumns)
            {
                throw new MathematicsException("Current matrix isn't square.");
            }
            else
            {
                return new ArraySquareMatrix<ObjectType>(
                    this.elements,
                    this.numberOfLines);
            }
        }

        /// <summary>
        /// Obtém a soma da matriz corrente com outra matriz.
        /// </summary>
        /// <param name="right">A outra matriz.</param>
        /// <param name="semigroup">O semigrupo.</param>
        /// <returns>O resultado da soma.</returns>
        public ArrayMatrix<ObjectType> Add(ArrayMatrix<ObjectType> right, ISemigroup<ObjectType> semigroup)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (semigroup == null)
            {
                throw new ArgumentNullException("semigroup");
            }
            else
            {
                if (this.numberOfLines == right.numberOfLines &&
                    this.numberOfColumns == right.numberOfColumns)
                {
                    var result = new ArrayMatrix<ObjectType>(
                        this.numberOfLines,
                        this.numberOfColumns);
                    for (int i = 0; i < this.numberOfLines; ++i)
                    {
                        for (int j = 0; j < this.numberOfColumns; ++j)
                        {
                            result.elements[i][j] = semigroup.Add(
                                this.elements[i][j],
                                right.elements[i][j]);
                        }
                    }

                    return result;
                }
                else
                {
                    throw new ArgumentException("Matrices don't have the same dimensions.");
                }
            }
        }

        /// <summary>
        /// Obtém a diferença entre a matriz corrente e outra matriz.
        /// </summary>
        /// <param name="right">A outra matriz.</param>
        /// <param name="group">O grupo.</param>
        /// <returns>O resultado da diferença.</returns>
        public ArrayMatrix<ObjectType> Subtract(ArrayMatrix<ObjectType> right, IGroup<ObjectType> group)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (group == null)
            {
                throw new ArgumentNullException("semigroup");
            }
            else
            {
                if (this.numberOfLines == right.numberOfLines &&
                    this.numberOfColumns == right.numberOfColumns)
                {
                    var result = new ArrayMatrix<ObjectType>(
                        this.numberOfLines,
                        this.numberOfColumns);
                    for (int i = 0; i < this.numberOfLines; ++i)
                    {
                        for (int j = 0; j < this.numberOfColumns; ++j)
                        {
                            result.elements[i][j] = group.Add(
                                this.elements[i][j],
                                group.AdditiveInverse(right.elements[i][j]));
                        }
                    }

                    return result;
                }
                else
                {
                    throw new ArgumentException("Matrices don't have the same dimensions.");
                }
            }
        }

        /// <summary>
        /// Obtém o produto da matriz corrente com outra matriz.
        /// </summary>
        /// <param name="right">A outra matriz.</param>
        /// <param name="ring">O anel.</param>
        /// <returns>O resultado do produto.</returns>
        public ArrayMatrix<ObjectType> Multiply(
            ArrayMatrix<ObjectType> right,
            IRing<ObjectType> ring)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                var columnNumber = this.numberOfColumns;
                var lineNumber = right.numberOfColumns;
                if (columnNumber != lineNumber)
                {
                    throw new MathematicsException("To multiply two matrices, the number of columns of the first must match the number of lines of second.");
                }
                else
                {
                    var firstDimension = this.numberOfLines;
                    var secondDimension = right.numberOfColumns;
                    var result = new ArrayMatrix<ObjectType>(
                        firstDimension,
                        secondDimension);
                    for (int i = 0; i < firstDimension; ++i)
                    {
                        for (int j = 0; j < secondDimension; ++j)
                        {
                            var addResult = ring.AdditiveUnity;
                            for (int k = 0; k < columnNumber; ++k)
                            {
                                var multResult = ring.Multiply(
                                    this.elements[i][k],
                                    right.elements[k][j]);
                                addResult = ring.Add(addResult, multResult);
                            }

                            result.elements[i][j] = addResult;
                        }
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão respectiva.
        /// </exception>
        public virtual void SwapLines(int i, int j)
        {
            if (i < 0 || i > this.numberOfLines)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.numberOfLines)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                var swapLine = this.elements[i];
                this.elements[i] = this.elements[j];
                this.elements[j] = swapLine;
            }
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão respectiva.
        /// </exception>
        public virtual void SwapColumns(int i, int j)
        {
            if (i < 0 || i > this.numberOfColumns)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.numberOfColumns)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                for (int k = 0; k < this.numberOfLines; ++k)
                {
                    var swapColumn = this.elements[k][i];
                    this.elements[k][i] = this.elements[k][j];
                    this.elements[k][j] = swapColumn;
                }
            }
        }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        public virtual void ScalarLineMultiplication(
            int line,
            ObjectType scalar,
            IRing<ObjectType> ring)
        {
            if (line < 0 || line >= this.elements.Length)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (scalar == null)
            {
                throw new ArgumentNullException("scalar");
            }
            else
            {
                var currentLineValue = this.elements[line];

                // Se o escalar proporcionado for uma unidade aditiva, a linha irá conter todos os valores.
                if (ring.IsAdditiveUnity(scalar))
                {
                    var lineLength = currentLineValue.Length;
                    for (int i = 0; i < lineLength; ++i)
                    {
                        currentLineValue[i] = scalar;
                    }
                }
                else if (!ring.IsMultiplicativeUnity(scalar))
                {
                    var lineLength = currentLineValue.Length;
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
        public virtual void CombineLines(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var lineslength = this.elements.Length;
            if (i < 0 || i >= lineslength)
            {
                throw new ArgumentNullException("i");
            }
            else if (j < 0 || j >= lineslength)
            {
                throw new ArgumentNullException("j");
            }
            else if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            else if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                var replacementLine = this.elements[i];
                if (ring.IsAdditiveUnity(a))
                {
                    if (ring.IsAdditiveUnity(b))
                    {
                        var replacementLineLenght = replacementLine.Length;
                        for (int k = 0; k < replacementLineLenght; ++k)
                        {
                            replacementLine[k] = a;
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var combinationLine = this.elements[j];
                        var replacementLineLenght = replacementLine.Length;
                        for (int k = 0; k < replacementLineLenght; ++k)
                        {
                            replacementLine[k] = combinationLine[k];
                        }
                    }
                    else
                    {
                        var combinationLine = this.elements[j];
                        var replacementLineLenght = replacementLine.Length;
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
                            var replacementLineLenght = replacementLine.Length;
                            for (int k = 0; k < replacementLineLenght; ++k)
                            {
                                var value = replacementLine[k];
                                replacementLine[k] = ring.Multiply(a, value);
                            }
                        }
                    }
                    else if(ring.IsMultiplicativeUnity(b))
                    {
                        var combinationLine = this.elements[j];
                        var replacementLineLenght = replacementLine.Length;
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
                        var combinationLine = this.elements[j];
                        var replacementLineLenght = replacementLine.Length;
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
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.numberOfLines; ++i)
            {
                for (int j = 0; j < this.numberOfColumns; ++j)
                {
                    yield return this.elements[i][j];
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
            if (0 < this.numberOfLines)
            {
                resultBuilder.Append("[");
                if (0 < this.numberOfColumns)
                {
                    resultBuilder.Append(this.elements[0][0]);
                    for (int i = 1; i < this.numberOfColumns; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", this.elements[0][i]);
                    }
                }
                resultBuilder.Append("]");

                for (int i = 1; i < this.numberOfLines; ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.numberOfColumns)
                    {
                        resultBuilder.Append(this.elements[i][0]);
                        for (int j = 1; j < this.numberOfColumns; ++j)
                        {
                            resultBuilder.AppendFormat(", {0}", this.elements[i][j]);
                        }
                    }

                    resultBuilder.Append("]");
                }

            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Obtém uma matriz identidade.
        /// </summary>
        /// <typeparam name="RingType">O tipo do anel responsável pelas operações sobre os coeficientes.</typeparam>
        /// <param name="order">A dimensão da matriz.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>A matriz identidade.</returns>
        /// <exception cref="ArgumentNullException">Se o anel proporcionado for nulo.</exception>
        /// <exception cref="ArgumentOutOfRangeException">See a dimensão da matriz for inferior a um.</exception>
        public static ArrayMatrix<ObjectType> GetIdentity<RingType>(int order, RingType ring)
            where RingType : IRing<ObjectType>
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (order <= 0)
            {
                throw new ArgumentOutOfRangeException("Order of identity matrix must be greater than one.");
            }
            else
            {
                var result = new ArrayMatrix<ObjectType>(order, order);
                for (int i = 0; i < order; ++i)
                {
                    for (int j = 0; j < order; ++j)
                    {
                        if (i == j)
                        {
                            result.elements[i][j] = ring.MultiplicativeUnity;
                        }
                        else
                        {
                            result.elements[i][j] = ring.AdditiveUnity;
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Inicializa a matriz.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        protected virtual void InitializeMatrix(int line, int column)
        {
            this.elements = new ObjectType[line][];
            for (int i = 0; i < line; ++i)
            {
                this.elements[i] = new ObjectType[column];
            }

            this.numberOfLines = line;
            this.numberOfColumns = column;
        }

        /// <summary>
        /// Inicializa a matriz.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        protected virtual void InitializeMatrix(int line, int column, ObjectType defaultValue)
        {
            this.elements = new ObjectType[line][];
            if (EqualityComparer<object>.Default.Equals(defaultValue, default(ObjectType)))
            {
                for (int i = 0; i < line; ++i)
                {
                    this.elements[i] = new ObjectType[column];
                }
            }
            else
            {
                for (int i = 0; i < line; ++i)
                {
                    this.elements[i] = new ObjectType[column];
                    for (int j = 0; j < column; ++j)
                    {
                        this.elements[i][j] = defaultValue;
                    }
                }
            }

            this.numberOfLines = line;
            this.numberOfColumns = column;
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
