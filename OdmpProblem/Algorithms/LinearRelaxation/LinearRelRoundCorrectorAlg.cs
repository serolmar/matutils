namespace OdmpProblem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities;

    /// <summary>
    /// Permite obter uma correcção ao resultado proveniente da relaxação linear. 
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficientes que corresponde à saída da relaxação linear.</typeparam>
    public class LinearRelRoundCorrectorAlg<CoeffType>
        : IAlgorithm<CoeffType[], ILongSparseMathMatrix<CoeffType>, int, GreedyAlgSolution<CoeffType>>
    {
        /// <summary>
        /// O corpo responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IField<CoeffType> coeffsField;

        /// <summary>
        /// O object responsável pelos arredondamentos.
        /// </summary>
        private INearest<CoeffType, int> nearest;

        /// <summary>
        /// O objecto responsável pela conversão dos valores em inteiros.
        /// </summary>
        private IConversion<int, CoeffType> converter;

        private IComparer<CoeffType> comparer;

        /// <summary>
        /// Permite instanciar um objecto reponsável pela correcção da solução proveniente da relaxação linear.
        /// </summary>
        /// <param name="comparer">O comparador de custos.</param>
        /// <param name="converter">O conversor.</param>
        /// <param name="nearest">O objecto responsável pelos arredondamentos.</param>
        /// <param name="coeffsField">O corpo responsável pelas operações sobre os coeficientes.</param>
        public LinearRelRoundCorrectorAlg(
            IComparer<CoeffType> comparer,
            IConversion<int, CoeffType> converter,
            INearest<CoeffType, int> nearest,
            IField<CoeffType> coeffsField)
        {
            if (coeffsField == null)
            {
                throw new ArgumentNullException("coeffsField");
            }
            else if (nearest == null)
            {
                throw new ArgumentNullException("nearest");
            }
            else if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.coeffsField = coeffsField;
                this.converter = converter;
                this.nearest = nearest;
                this.comparer = comparer;
            }
        }

        /// <summary>
        /// Obtém uma solução a partir duma aproximação inicial.
        /// </summary>
        /// <param name="approximateMedians">As medianas.</param>
        /// <param name="costs">Os custos.</param>
        /// <param name="niter">O número máximo melhoramentos a serem aplicados à solução encontrada.</param>
        /// <returns>A solução construída a partir da aproximação.</returns>
        public GreedyAlgSolution<CoeffType> Run(
            CoeffType[] approximateMedians,
            ILongSparseMathMatrix<CoeffType> costs,
            int niter)
        {
            if (approximateMedians == null)
            {
                throw new ArgumentNullException("approximateMedians");
            }
            else if (costs == null)
            {
                throw new ArgumentNullException("costs");
            }
            else if (approximateMedians.Length != costs.GetLength(1))
            {
                throw new ArgumentException("The number of medians must match the number of columns in costs matrix.");
            }
            else
            {
                var settedSolutions = new IntegerSequence();
                var approximateSolutions = new List<int>();
                var sum = this.coeffsField.AdditiveUnity;
                for (int i = 0; i < approximateMedians.Length; ++i)
                {
                    var currentMedian = approximateMedians[i];
                    if (!this.coeffsField.IsAdditiveUnity(currentMedian))
                    {
                        sum = this.coeffsField.Add(sum, approximateMedians[i]);
                        if (this.converter.CanApplyDirectConversion(currentMedian))
                        {
                            var converted = this.converter.DirectConversion(currentMedian);
                            if (converted == 1)
                            {
                                settedSolutions.Add(i);
                            }
                            else
                            {
                                throw new OdmpProblemException(string.Format(
                                    "The median {0} at position {1} of medians array can't be converted to the unity.",
                                    currentMedian,
                                    i));
                            }
                        }
                        else
                        {
                            approximateSolutions.Add(i);
                        }
                    }
                }

                if (this.converter.CanApplyDirectConversion(sum))
                {
                    var convertedSum = this.converter.DirectConversion(sum);
                    if (convertedSum <= 0 || convertedSum > approximateMedians.Length)
                    {
                        throw new IndexOutOfRangeException(string.Format(
                            "The medians sum {0} is out of bounds. It must be between 1 and the number of elements in medians array.",
                            convertedSum));
                    }

                    var solutionBoard = new CoeffType[approximateMedians.Length];
                    var marked = new BitArray(approximateMedians.Length, false);
                    if (settedSolutions.Count == convertedSum)
                    {
                        var result = new GreedyAlgSolution<CoeffType>(settedSolutions);
                        result.Cost = this.ComputeCost(settedSolutions, costs, solutionBoard, marked);
                        return result;
                    }
                    else
                    {
                        // Partição das mediana em dois conjuntos: as que vão falzer parte da solução e as restantes
                        // entre as soluções aproximadas.
                        var recoveredMedians = new List<int>();
                        var unrecoveredMedians = new List<int>();
                        var innerComparer = new InnerComparer(approximateMedians, this.comparer);
                        approximateSolutions.Sort(innerComparer);

                        var count = convertedSum - settedSolutions.Count;
                        var i = 0;
                        for (; i < count; ++i)
                        {
                            recoveredMedians.Add(approximateSolutions[i]);
                            settedSolutions.Add(approximateSolutions[i]);
                        }

                        for (; i < approximateSolutions.Count; ++i)
                        {
                            unrecoveredMedians.Add(approximateSolutions[i]);
                        }

                        var currentCost = this.ComputeCost(settedSolutions, costs, solutionBoard, marked);

                        // Processa as melhorias de uma forma simples caso seja possível
                        if (unrecoveredMedians.Count > 0 && niter > 0)
                        {
                            var exchangeSolutionBoard = new CoeffType[solutionBoard.Length];
                            var currentBestBoard = new CoeffType[solutionBoard.Length];
                            for (i = 0; i < niter; ++i)
                            {
                                var itemToExchange = -1;
                                var itemToExchangeIndex = -1;
                                var itemToExchangeWith = -1;
                                var itemToExchangeWithIndex = -1;
                                var minimumCost = this.coeffsField.AdditiveUnity;
                                for (int j = 0; j < recoveredMedians.Count; ++j)
                                {
                                    for (int k = 0; k < unrecoveredMedians.Count; ++k)
                                    {
                                        var replacementCost = this.ComputeReplacementCost(
                                            unrecoveredMedians[k],
                                            recoveredMedians[j],
                                            settedSolutions,
                                            costs,
                                            solutionBoard,
                                            exchangeSolutionBoard);
                                        if (this.comparer.Compare(replacementCost, minimumCost) < 0)
                                        {
                                            // Aceita a troca
                                            itemToExchange = recoveredMedians[j];
                                            itemToExchangeIndex = j;
                                            itemToExchangeWith = unrecoveredMedians[k];
                                            itemToExchangeWithIndex = k;
                                            minimumCost = replacementCost;

                                            var swapBestBoard = currentBestBoard;
                                            currentBestBoard = exchangeSolutionBoard;
                                            exchangeSolutionBoard = swapBestBoard;
                                        }
                                    }
                                }

                                if (itemToExchange == -1 || itemToExchangeWith == -1)
                                {
                                    i = niter - 1;
                                }
                                else
                                {

                                    // Efectua a troca
                                    var swapSolutionBoard = solutionBoard;
                                    solutionBoard = currentBestBoard;
                                    currentBestBoard = swapSolutionBoard;

                                    currentCost = this.coeffsField.Add(currentCost, minimumCost);
                                    settedSolutions.Remove(itemToExchange);
                                    settedSolutions.Add(itemToExchangeWith);

                                    var swap = recoveredMedians[itemToExchangeIndex];
                                    recoveredMedians[itemToExchangeIndex] = unrecoveredMedians[itemToExchangeWithIndex];
                                    unrecoveredMedians[itemToExchangeWithIndex] = swap;
                                }
                            }
                        }

                        return new GreedyAlgSolution<CoeffType>(settedSolutions) { Cost = currentCost };
                    }
                }
                else
                {
                    throw new OdmpProblemException("The sum of medians can't be converted to an integer.");
                }
            }
        }

        /// <summary>
        /// Permite calcular o custo associado à escolha de um conjunto de medianas escolhidas.
        /// </summary>
        /// <param name="chosen">O conjunto de medianas escolhidas.</param>
        /// <param name="costs">A matriz dos custos.</param>
        /// <param name="solutionBoard">A linha que mantém os mínimos por mediana.</param>
        /// <returns>O valor do custo associado à escolha.</returns>
        private CoeffType ComputeCost(
            IntegerSequence chosen,
            ILongSparseMathMatrix<CoeffType> costs,
            CoeffType[] solutionBoard,
            BitArray marked)
        {
            var resultCost = this.coeffsField.AdditiveUnity;
            foreach (var item in chosen)
            {
                var currentCostLine = default(ILongSparseMatrixLine<CoeffType>);
                if (costs.TryGetLine(item, out currentCostLine))
                {
                    foreach (var column in currentCostLine.GetColumns())
                    {
                        if (column.Key == item)
                        {
                            if (marked[item])
                            {
                                resultCost = this.coeffsField.Add(
                                resultCost,
                                this.coeffsField.AdditiveInverse(solutionBoard[item]));
                            }
                            else
                            {
                                marked[item] = true;
                            }

                            solutionBoard[item] = this.coeffsField.AdditiveUnity;
                        }
                        else
                        {
                            if (marked[column.Key])
                            {
                                var boardCost = solutionBoard[column.Key];
                                if (this.comparer.Compare(column.Value, boardCost) < 0)
                                {
                                    solutionBoard[column.Key] = column.Value;
                                    var difference = this.coeffsField.Add(
                                        column.Value,
                                        this.coeffsField.AdditiveInverse(boardCost));
                                    resultCost = this.coeffsField.Add(
                                        resultCost,
                                        difference);
                                }
                            }
                            else
                            {
                                resultCost = this.coeffsField.Add(
                                    resultCost,
                                    column.Value);
                                solutionBoard[column.Key] = column.Value;
                                marked[column.Key] = true;
                            }
                        }
                    }
                }
                else
                {
                    marked[item] = true;
                    solutionBoard[item] = this.coeffsField.AdditiveUnity;
                }
            }

            for (int i = 0; i < marked.Length; ++i)
            {
                if (!marked[i] && !chosen.Contains(i))
                {
                    throw new OdmpProblemException("Not all nodes are covered by some median.");
                }
            }

            return resultCost;
        }

        /// <summary>
        /// Calcula o custo da substituição de uma mediana por outra. 
        /// </summary>
        /// <param name="replacementMedian">A mediana que substitui.</param>
        /// <param name="medianToBeReplaced">A mediana que é substituída.</param>
        /// <param name="existingMedians">As medianas que forma escolhidas.</param>
        /// <param name="costs">A matriz dos custos.</param>
        /// <param name="currentSolutionBoard">O quadro de solução actual.</param>
        /// <param name="replacementSolutionBoard">O quadro da solução substituída.</param>
        /// <returns>O valor do mínimo.</returns>
        private CoeffType ComputeReplacementCost(
            int replacementMedian,
            int medianToBeReplaced,
            IntegerSequence existingMedians,
            ILongSparseMathMatrix<CoeffType> costs,
            CoeffType[] currentSolutionBoard,
            CoeffType[] replacementSolutionBoard)
        {
            Array.Copy(currentSolutionBoard, replacementSolutionBoard, currentSolutionBoard.Length);
            var result = this.coeffsField.AdditiveUnity;

            var minimum = this.GetMinimumFromColumn(
                existingMedians,
                medianToBeReplaced,
                medianToBeReplaced,
                costs);
            replacementSolutionBoard[medianToBeReplaced] = minimum;
            result = this.coeffsField.Add(result, minimum);

            var boardValue = currentSolutionBoard[replacementMedian];
            replacementSolutionBoard[replacementMedian] = this.coeffsField.AdditiveUnity;
            result = this.coeffsField.Add(
                result,
                this.coeffsField.AdditiveInverse(boardValue));

            // Remove a linha a ser substituída.
            var currentRow = default(ILongSparseMatrixLine<CoeffType>);
            if (costs.TryGetLine(medianToBeReplaced, out currentRow))
            {
                foreach (var column in currentRow.GetColumns())
                {
                    if (column.Key != medianToBeReplaced && column.Key != replacementMedian)
                    {
                        if (this.coeffsField.Equals(column.Value, replacementSolutionBoard[column.Key]))
                        {
                            var current = replacementSolutionBoard[column.Key];
                            minimum = this.GetMinimumFromColumn(
                                existingMedians,
                                medianToBeReplaced,
                                column.Key,
                                costs);
                            replacementSolutionBoard[column.Key] = minimum;
                            var temp = this.coeffsField.Add(
                                minimum,
                                this.coeffsField.AdditiveInverse(current));
                            result = this.coeffsField.Add(result, temp);
                        }
                    }
                }
            }

            // Insere a linha a substituir.
            if (costs.TryGetLine(replacementMedian, out currentRow))
            {
                foreach (var column in currentRow.GetColumns())
                {
                    if (column.Key != replacementMedian)
                    {
                        var current = replacementSolutionBoard[column.Key];
                        if (this.comparer.Compare(column.Value, current) < 0)
                        {
                            replacementSolutionBoard[column.Key] = column.Value;
                            var temp = this.coeffsField.Add(
                                column.Value,
                                this.coeffsField.AdditiveInverse(current));
                            result = this.coeffsField.Add(result, temp);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Permite obter o valor mínimo entre as colunas da matriz para as linhas escolhidas com excepção
        /// de uma delas.
        /// </summary>
        /// <param name="chosen">As linhas escolhidas.</param>
        /// <param name="exceptLine">
        /// A linha que não vai ser considerada.</param>
        /// <param name="column">A coluna sobre a qual é calculado o mínimo.</param>
        /// <param name="costs">A matriz dos custos.</param>
        /// <returns>O valor do custo mínimo.</returns>
        private CoeffType GetMinimumFromColumn(
            IntegerSequence chosen,
            int exceptLine,
            int column,
            ILongSparseMathMatrix<CoeffType> costs)
        {
            var result = default(CoeffType);
            var chosenEnumerator = chosen.GetEnumerator();
            var state = chosenEnumerator.MoveNext();
            var keep = true;
            while (state && keep)
            {
                // Pesquisa pelo primeiro elemento.
                var currentChosen = chosenEnumerator.Current;
                if (currentChosen != exceptLine)
                {
                    var currentLine = default(ILongSparseMatrixLine<CoeffType>);
                    if (costs.TryGetLine(currentChosen, out currentLine))
                    {
                        var columnValue = default(CoeffType);
                        if (currentLine.TryGetColumnValue(column, out columnValue))
                        {
                            if (this.coeffsField.IsAdditiveUnity(columnValue))
                            {
                                return columnValue;
                            }
                            else
                            {
                                result = columnValue;
                                keep = false;
                            }
                        }
                    }
                }

                state = chosenEnumerator.MoveNext();
            }

            if (keep)
            {
                throw new OdmpProblemException("Something went very wrong. Check if costs matrix is valid.");
            }
            else
            {
                while (state)
                {
                    var currentChosen = chosenEnumerator.Current;
                    if (currentChosen != exceptLine)
                    {
                        var currentLine = default(ILongSparseMatrixLine<CoeffType>);
                        if (costs.TryGetLine(currentChosen, out currentLine))
                        {
                            var columnValue = default(CoeffType);
                            if (currentLine.TryGetColumnValue(column, out columnValue))
                            {
                                if (this.coeffsField.IsAdditiveUnity(columnValue))
                                {
                                    return columnValue;
                                }
                                else
                                {
                                    if (this.comparer.Compare(columnValue, result) < 0)
                                    {
                                        result = columnValue;
                                    }
                                }
                            }
                        }
                    }

                    state = chosenEnumerator.MoveNext();
                }
            }

            return result;
        }

        /// <summary>
        /// Utilizada para determinar uma ordenação da lista de posições que contêm valores aproximados
        /// de acordo com o valor correspondente.
        /// </summary>
        private class InnerComparer : Comparer<int>
        {
            private IComparer<CoeffType> coeffsComparer;

            private CoeffType[] coeffs;

            public InnerComparer(CoeffType[] coeffs, IComparer<CoeffType> coeffsComparer)
            {
                this.coeffsComparer = coeffsComparer;
                this.coeffs = coeffs;
            }

            /// <summary>
            /// Permite comparar dois interios conforme os coeficientes.
            /// </summary>
            /// <param name="x">O primeiro valor.</param>
            /// <param name="y">O segundo valor.</param>
            /// <returns>
            /// O valor 1 caso o primeiro seja maior do que o segundo, -1 caso o segundo seja maior do que
            /// o primeiro e 0 caso contrário.</returns>
            public override int Compare(int x, int y)
            {
                var firstValue = this.coeffs[x];
                var secondValur = this.coeffs[y];

                // A comparação é invertidade de modo a proporcionar uma ordenação do maior para o menor.
                return this.coeffsComparer.Compare(secondValur, firstValue);
            }
        }
    }
}
