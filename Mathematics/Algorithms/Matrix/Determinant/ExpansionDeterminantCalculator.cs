using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Collections;

namespace Mathematics
{
    public class ExpansionDeterminantCalculator<ElementsType, RingType> : ADeterminant<ElementsType, RingType>
        where RingType : IRing<ElementsType>
    {
        public ExpansionDeterminantCalculator(RingType ring)
            : base(ring)
        {
        }

        protected override ElementsType ComputeDeterminant(IMatrix<ElementsType> data)
        {
            if (data.GetLength(0) == 0)
            {
                return this.ring.AdditiveUnity;
            }
            if (data.GetLength(0) == 1)
            {
                return data[0, 0];
            }
            else if (data.GetLength(1) == 2)
            {
                var firstDiagonal = this.ring.Multiply(
                    data[0, 0],
                    data[1, 1]);
                var secondDiagonal = this.ring.Multiply(
                    data[0, 1],
                    data[1, 0]);
                secondDiagonal = this.ring.AdditiveInverse(secondDiagonal);
                var result = this.ring.Add(firstDiagonal, secondDiagonal);
                return result;
            }
            else
            {
                var matrixOrder = data.GetLength(0);

                // Determina o número de itens a fixar
                var itemsNumber = matrixOrder / 2;

                var linesZeroesCount = this.GetLinesByZeroesCount(data, itemsNumber);
                var columnsZeroesCount = this.GetColumnsByZeroesCount(data, itemsNumber);

                if (linesZeroesCount == null || columnsZeroesCount == null)
                {
                    return this.ring.AdditiveUnity;
                }
                else if (this.FixLines(linesZeroesCount.Item2, columnsZeroesCount.Item2))
                {
                    var inverseValue = this.ring.AdditiveInverse(this.ring.MultiplicativeUnity);
                    var negativeSign = (linesZeroesCount.Item1.Sum() % 2 == 1);
                    var linesToBeFixed = linesZeroesCount.Item1;
                    var otherLinesToBeFixed = this.GetOtherCoords(linesToBeFixed, matrixOrder);
                    var columnsCombinator = new CombinationAffector(matrixOrder, itemsNumber);
                    var result = this.ring.AdditiveUnity;
                    foreach (var columnsCombination in columnsCombinator)
                    {
                        var otherColumns = this.GetOtherCoords(columnsCombination, matrixOrder);
                        var subMatrix = new SubMatrix<ElementsType>(data, linesToBeFixed, columnsCombination);
                        var firstDeterminant = this.ComputeDeterminant(subMatrix);
                        subMatrix = new SubMatrix<ElementsType>(data, otherLinesToBeFixed, otherColumns);
                        var secondDeterminant = this.ComputeDeterminant(subMatrix);
                        var multiplied = this.ring.Multiply(firstDeterminant, secondDeterminant);
                        if (negativeSign)
                        {
                            multiplied = this.ring.Multiply(multiplied, inverseValue);
                        }

                        result = this.ring.Add(result, multiplied);
                        negativeSign = !negativeSign;
                    }

                    return result;
                }
                else
                {
                    var inverseValue = this.ring.AdditiveInverse(this.ring.MultiplicativeUnity);
                    var negativeSign = (linesZeroesCount.Item1.Sum() % 2 == 1);
                    var columnsToBeFixed = columnsZeroesCount.Item1;
                    var otherColumnsToBeFixed = this.GetOtherCoords(columnsToBeFixed, matrixOrder);
                    var columnsCombinator = new CombinationAffector(matrixOrder, itemsNumber);
                    var result = this.ring.AdditiveUnity;
                    foreach (var linesCombination in columnsCombinator)
                    {
                        var otherLines = this.GetOtherCoords(linesCombination, matrixOrder);
                        var subMatrix = new SubMatrix<ElementsType>(data, linesCombination, columnsToBeFixed);
                        var firstDeterminant = this.ComputeDeterminant(subMatrix);
                        subMatrix = new SubMatrix<ElementsType>(data, otherLines, columnsToBeFixed);
                        var secondDeterminant = this.ComputeDeterminant(subMatrix);
                        var multiplied = this.ring.Multiply(firstDeterminant, secondDeterminant);
                        if (negativeSign)
                        {
                            multiplied = this.ring.Multiply(multiplied, inverseValue);
                        }

                        result = this.ring.Add(result, multiplied);
                        negativeSign = !negativeSign;
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Obtém a contagem dos zeros nas linhas.
        /// </summary>
        /// <remarks>
        /// Se o total de elementos na linha for igual a zero
        /// então o respectivo determinante é nulo.
        /// </remarks>
        /// <param name="matrix">A matriz.</param>
        /// <param name="itemsToFix">O número de linhas ou colunas a fixar.</param>
        /// <returns>A contagem dos zeros nas linhas com maior número de zeros e essas mesmas linhas.</returns>
        private Tuple<int[], int[]> GetColumnsByZeroesCount(IMatrix<ElementsType> matrix, int itemsToFix)
        {
            var matrixOrder = matrix.GetLength(0);
            var lineCoords = new int[itemsToFix];
            var zeroesCount = new int[itemsToFix];
            for (int i = 0; i < itemsToFix; ++i)
            {
                zeroesCount[i] = -1;
            }

            for (int i = 0; i < matrixOrder; ++i)
            {
                var count = 0;
                for (int j = 0; j < matrixOrder; ++j)
                {
                    if (this.ring.IsAdditiveUnity(matrix[i, j]))
                    {
                        ++count;
                    }
                }

                if (count == matrixOrder)
                {
                    return null;
                }
                else
                {
                    for (int j = 0; j < itemsToFix; ++j)
                    {
                        if (zeroesCount[j] < count)
                        {
                            for (int k = j + 1; k < itemsToFix; ++k)
                            {
                                zeroesCount[k] = zeroesCount[k - 1];
                                lineCoords[k] = lineCoords[k - 1];
                            }

                            zeroesCount[j] = count;
                            lineCoords[j] = i;
                            j = itemsToFix;
                        }
                    }
                }
            }

            Array.Sort(lineCoords);
            return Tuple.Create(lineCoords, zeroesCount);
        }

        /// <summary>
        /// Obtém a contagem dos zeros nas colunas.
        /// </summary>
        /// <remarks>
        /// Se o total de elementos na coluna for igual a zero
        /// então o respectivo determinante é nulo.
        /// </remarks>
        /// <param name="matrix">A matriz.</param>
        /// <param name="itemsToFix">O número de linhas ou colunas a fixar.</param>
        /// <returns>A contagem dos zeros nas colunas com maior número de zeros e essas mesmas colunas.</returns>
        private Tuple<int[], int[]> GetLinesByZeroesCount(IMatrix<ElementsType> matrix, int itemsToFix)
        {
            var matrixOrder = matrix.GetLength(0);
            var columnCoords = new int[itemsToFix];
            var zeroesCount = new int[itemsToFix];
            for (int i = 0; i < itemsToFix; ++i)
            {
                zeroesCount[i] = -1;
            }

            for (int i = 0; i < matrixOrder; ++i)
            {
                var count = 0;
                for (int j = 0; j < matrixOrder; ++j)
                {
                    if (this.ring.IsAdditiveUnity(matrix[j, i]))
                    {
                        ++count;
                    }
                }

                if (count == matrixOrder)
                {
                    return null;
                }
                else
                {
                    for (int j = 0; j < itemsToFix; ++j)
                    {
                        if (zeroesCount[j] < count)
                        {
                            for (int k = j + 1; k < itemsToFix; ++k)
                            {
                                zeroesCount[k] = zeroesCount[k - 1];
                                columnCoords[k] = columnCoords[k - 1];
                            }

                            zeroesCount[j] = count;
                            columnCoords[j] = i;
                            j = itemsToFix;
                        }
                    }
                }
            }

            Array.Sort(columnCoords);
            return Tuple.Create(columnCoords, zeroesCount);
        }

        /// <summary>
        /// Indica se é pertinente fixar as linhas ou as colunas.
        /// </summary>
        /// <param name="linesCount">A contagem de zeros por linha.</param>
        /// <param name="columnsCount">A contagem de zeros por coluna.</param>
        /// <returns>Verdadeiro caso seja para fixar linhas e falso caso seja para fixar as colunas.</returns>
        private bool FixLines(int[] linesCount, int[] columnsCount)
        {
            var result = true;
            for (int i = 0; i < linesCount.Length; ++i)
            {
                if (linesCount[i] < columnsCount[i])
                {
                    result = false;
                    i = linesCount.Length;
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém as linhas ou colunas que não estão contempladas na submatriz a fixar.
        /// </summary>
        /// <param name="itemCoords">As coordenadas fixadas.</param>
        /// <param name="matrixOrder">A ordem da matriz.</param>
        /// <returns>As restantes coordenadas.</returns>
        private int[] GetOtherCoords(int[] itemCoords, int matrixOrder)
        {
            var remaining = matrixOrder - itemCoords.Length;
            var result = new int[remaining];
            var resultPointer = 0;
            var itemsPointer = 0;
            for (int i = 0; i < matrixOrder; ++i)
            {
                if (itemsPointer < itemCoords.Length && itemCoords[itemsPointer] == i)
                {
                    ++itemsPointer;
                }
                else
                {
                    result[resultPointer] = i;
                    ++resultPointer;
                }
            }

            return result;
        }
    }
}
