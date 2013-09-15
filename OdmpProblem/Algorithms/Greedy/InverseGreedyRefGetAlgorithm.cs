namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities.Collections;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementa o algoritmo que permite obter a próxima referência a ser removida.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de dados utilizado na matriz dos custos.</typeparam>
    public class InverseGreedyRefGetAlgorithm<ElementType>
        : IAlgorithm<IntegerSequence, SparseDictionaryMatrix<ElementType>, ElementType[], Tuple<int, ElementType>>
    {
        /// <summary>
        /// O objecto utilizado para bloquear os processos.
        /// </summary>
        private object lockObject = new object();

        /// <summary>
        /// O corpo responsável pelas operações sobre os elementos.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// O comparador de elementos.
        /// </summary>
        private IComparer<ElementType> comparer;

        public InverseGreedyRefGetAlgorithm(IComparer<ElementType> comparer, IRing<ElementType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                if (comparer == null)
                {
                    this.comparer = Comparer<ElementType>.Default;
                }

                this.ring = ring;
            }
        }

        /// <summary>
        /// O corpo responsável pelas operações sobre os elementos.
        /// </summary>
        public IRing<ElementType> Ring
        {
            get
            {
                return this.ring;
            }
        }

        /// <summary>
        /// O comparador de elementos.
        /// </summary>
        public IComparer<ElementType> Comparer
        {
            get
            {
                return this.comparer;
            }
        }

        /// <summary>
        /// Escolhe a referência que minimiza a perda.
        /// </summary>
        /// <param name="chosenReferences">As referências escolhidas anteriormente.</param>
        /// <param name="currentMatrix">A matriz dos custos.</param>
        /// <param name="currentLineBoard">A linha que contém a condensação dos custos das linhas escolhidas.</param>
        /// <returns>O índice da linha correspondente à próxima referência bem como a perda respectiva.</returns>
        public Tuple<int, ElementType> Run(
            IntegerSequence chosenReferences,
            SparseDictionaryMatrix<ElementType> currentMatrix,
            ElementType[] currentLineBoard)
        {
            var result = -1;
            var currentMinimumLost = default(ElementType);
            var lines = currentMatrix.GetLines();

            if (chosenReferences.Count > 2)
            {
                currentMinimumLost = this.GetMinimumSum(
                        chosenReferences,
                        currentMatrix,
                        currentLineBoard,
                        lines,
                        2);
                result = 2;
            }

            Parallel.For(2, chosenReferences.Count,
                chosenSolution =>
                {
                    var sum = this.GetMinimumSum(
                        chosenReferences, 
                        currentMatrix, 
                        currentLineBoard, 
                        lines, 
                        chosenSolution);

                    lock (this.lockObject)
                    {
                        if (this.comparer.Compare(sum, currentMinimumLost) < 0)
                        {
                            currentMinimumLost = sum;
                            result = chosenSolution;
                        }
                    }
                });

            if (result != -1)
            {
                var solutionValue = chosenReferences[result];
                var minimumCover = this.GetMinimumCover(
                    chosenReferences,
                    lines,
                    solutionValue);

                currentLineBoard[solutionValue] = minimumCover;

                var columns = currentMatrix.GetColumns(solutionValue);
                foreach (var column in columns)
                {
                    if (column.Key != solutionValue)
                    {
                        var currentValue = currentMatrix[solutionValue, column.Key];
                        if (this.ring.Equals(currentValue, currentLineBoard[column.Key]))
                        {
                            minimumCover = this.GetMinimumCover(
                                chosenReferences,
                                lines,
                                solutionValue);

                            currentLineBoard[column.Key] = minimumCover;
                        }
                    }
                }
            }
            
            return Tuple.Create(result, currentMinimumLost);
        }

        private ElementType GetMinimumSum(
            IntegerSequence chosenReferences, 
            SparseDictionaryMatrix<ElementType> currentMatrix, 
            ElementType[] currentLineBoard, 
            IEnumerable<KeyValuePair<int, ISparseMatrixLine<ElementType>>> lines, 
            int chosenSolution)
        {
            var sum = this.ring.AdditiveUnity;
            var solutionValue = chosenReferences[chosenSolution];
            var minimumCover = GetMinimumCover(chosenReferences, lines, solutionValue);

            sum = this.ring.Add(sum, minimumCover);

            var columns = currentMatrix.GetColumns(solutionValue);
            foreach (var column in columns)
            {
                if (column.Key != solutionValue)
                {
                    var currentValue = currentMatrix[solutionValue, column.Key];
                    if (this.ring.Equals(currentValue, currentLineBoard[column.Key]))
                    {
                        minimumCover = this.GetMinimumCover(
                            chosenReferences,
                            lines,
                            solutionValue);

                        sum = this.ring.Add(sum, minimumCover);
                    }
                }
            }

            return sum;
        }

        private ElementType GetMinimumCover(
            IntegerSequence chosenReferences, 
            IEnumerable<KeyValuePair<int, ISparseMatrixLine<ElementType>>> lines, 
            int solutionValue)
        {
            var minimumCover = default(ElementType);
            var lineEnumerator = lines.GetEnumerator();
            var status = lineEnumerator.MoveNext();
            if (status)
            {
                var line = lineEnumerator.Current;
                if (line.Key != solutionValue &&
                    chosenReferences.Contains(line.Key) &&
                    line.Value.ContainsColumn(solutionValue))
                {
                    minimumCover = line.Value[solutionValue];
                }
                else
                {
                    status = lineEnumerator.MoveNext();
                    while (status &&
                        (line.Key == solutionValue ||
                        !chosenReferences.Contains(line.Key) ||
                        !line.Value.ContainsColumn(solutionValue)))
                    {
                        status = lineEnumerator.MoveNext();
                    }
                }

                while (status)
                {
                    line = lineEnumerator.Current;
                    if (line.Key != solutionValue &&
                    chosenReferences.Contains(line.Key) &&
                    line.Value.ContainsColumn(solutionValue))
                    {
                        var compareValue = line.Value[solutionValue];
                        if (this.comparer.Compare(compareValue, minimumCover) < 0)
                        {
                            minimumCover = compareValue;
                        }
                    }
                }
            }

            return minimumCover;
        }
    }
}
