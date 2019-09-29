// -----------------------------------------------------------------------
// <copyright file="ArrayMathMatrix.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa uma matriz com base em vectore do sistema.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayMathMatrix<ObjectType>
        : ArrayMatrix<ObjectType>, ILongMathMatrix<ObjectType>
    {
        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMathMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="elements">O contentor com os elementos.</param>
        /// <param name="numberOfLines">O número de linhas.</param>
        /// <param name="numberOfColumns">O número de colunas.</param>
        internal ArrayMathMatrix(ObjectType[][] elements, long numberOfLines, long numberOfColumns)
            : base(numberOfLines, numberOfColumns)
        {
            this.elements = elements;
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMathMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o número de linhas ou colunas for negativo.</exception>
        public ArrayMathMatrix(long line, long column)
            : base(line, column) { }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMathMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o número de linhas ou colunas for negativo.</exception>
        public ArrayMathMatrix(long line, long column, ObjectType defaultValue)
            : base(line, column, defaultValue) { }

        /// <summary>
        /// Obtém a matriz corrente como sendo um matriz quadrada.
        /// </summary>
        /// <returns>A matriz corrente como sendo uma matriz quadrada.</returns>
        /// <exception cref="UtilitiesException">Se a matriz não for quadrada.</exception>
        public ISquareMathMatrix<ObjectType> AsSquare()
        {
            if (this.numberOfLines != this.numberOfColumns)
            {
                throw new UtilitiesException("Current matrix isn't square.");
            }
            else
            {
                return new ArraySquareMathMatrix<ObjectType>(
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
        public ArrayMathMatrix<ObjectType> Add(ArrayMathMatrix<ObjectType> right, ISemigroup<ObjectType> semigroup)
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
                    var result = new ArrayMathMatrix<ObjectType>(
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
        public ArrayMathMatrix<ObjectType> Subtract(ArrayMathMatrix<ObjectType> right, IGroup<ObjectType> group)
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
                    var result = new ArrayMathMatrix<ObjectType>(
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
        public ArrayMathMatrix<ObjectType> Multiply(
            ArrayMathMatrix<ObjectType> right,
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
                var lineNumber = right.numberOfLines;
                if (columnNumber != lineNumber)
                {
                    throw new MathematicsException("To multiply two matrices, the number of columns of the first must match the number of lines of second.");
                }
                else
                {
                    var firstDimension = this.numberOfLines;
                    var secondDimension = right.numberOfColumns;
                    var result = new ArrayMathMatrix<ObjectType>(
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
        /// Obtém o produto da matriz corrente com outra matriz.
        /// </summary>
        /// <param name="right">A outra matriz.</param>
        /// <param name="ring">O anel.</param>
        /// <returns>O resultado do produto.</returns>
        public ArrayMathMatrix<ObjectType> ParallelMultiply(
            ArrayMathMatrix<ObjectType> right,
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
                    var result = new ArrayMathMatrix<ObjectType>(
                        firstDimension,
                        secondDimension);
                    Parallel.For(0, firstDimension, i =>
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
                    });

                    return result;
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
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        public void ScalarLineMultiplication(long line, ObjectType scalar, IRing<ObjectType> ring)
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
                    var lineLength = currentLineValue.LongLength;
                    for (var i = 0L; i < lineLength; ++i)
                    {
                        currentLineValue[i] = scalar;
                    }
                }
                else if (!ring.IsMultiplicativeUnity(scalar))
                {
                    var lineLength = currentLineValue.LongLength;
                    for (var i = 0L; i < lineLength; ++i)
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
                    else if (ring.IsMultiplicativeUnity(b))
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
        /// Substitui a linha especificada por uma combinação linear desta com uma outra. Por exemplo, li = a * li + b * lj, isto é,
        /// a linha i é substituída pela soma do produto de a pela linha i com o produto de b peloa linha j.
        /// </summary>
        /// <param name="i">A linha a ser substituída.</param>
        /// <param name="j">A linha a ser combinada.</param>
        /// <param name="a">O escalar a ser multiplicado pela primeira linha.</param>
        /// <param name="b">O escalar a ser multiplicado pela segunda linha.</param>
        /// <param name="ring">O objecto responsável pelas operações sobre os coeficientes.</param>
        public void CombineLines(long i, long j, ObjectType a, ObjectType b, IRing<ObjectType> ring)
        {
            var lineslength = this.elements.LongLength;
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
                        var replacementLineLenght = replacementLine.LongLength;
                        for (var k = 0L; k < replacementLineLenght; ++k)
                        {
                            replacementLine[k] = a;
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var combinationLine = this.elements[j];
                        var replacementLineLenght = replacementLine.LongLength;
                        for (int k = 0; k < replacementLineLenght; ++k)
                        {
                            replacementLine[k] = combinationLine[k];
                        }
                    }
                    else
                    {
                        var combinationLine = this.elements[j];
                        var replacementLineLenght = replacementLine.LongLength;
                        for (var k = 0L; k < replacementLineLenght; ++k)
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
                            var replacementLineLenght = replacementLine.LongLength;
                            for (var k = 0L; k < replacementLineLenght; ++k)
                            {
                                var value = replacementLine[k];
                                replacementLine[k] = ring.Multiply(a, value);
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var combinationLine = this.elements[j];
                        var replacementLineLenght = replacementLine.LongLength;
                        if (ring.IsMultiplicativeUnity(a))
                        {
                            for (var k = 0L; k < replacementLineLenght; ++k)
                            {
                                replacementLine[k] = ring.Add(replacementLine[k], combinationLine[k]);
                            }
                        }
                        else
                        {
                            for (var k = 0L; k < replacementLineLenght; ++k)
                            {
                                var replacementValue = ring.Multiply(replacementLine[k], a);
                                replacementLine[k] = ring.Add(replacementValue, combinationLine[k]);
                            }
                        }
                    }
                    else
                    {
                        var combinationLine = this.elements[j];
                        var replacementLineLenght = replacementLine.LongLength;
                        for (var k = 0L; k < replacementLineLenght; ++k)
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
        public static ArrayMathMatrix<ObjectType> GetIdentity<RingType>(int order, RingType ring)
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
                var result = new ArrayMathMatrix<ObjectType>(order, order);
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
        /// Obtém o enumerador não genércio para a matriz.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
