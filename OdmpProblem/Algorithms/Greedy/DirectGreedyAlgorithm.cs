namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Mathematics;
    using Utilities;

    public class DirectGreedyAlgorithm<ElementType>
        : IAlgorithm<List<ILongSparseMathMatrix<ElementType>>, int, GreedyAlgSolution<ElementType>[]>,
        IAlgorithm<List<ILongSparseMathMatrix<ElementType>>, IEnumerable<GreedyAlgSolution<ElementType>[]>>
    {
        /// <summary>
        /// Mantém o algoritmo que permite a obtenção da próxima referência.
        /// </summary>
        private IAlgorithm<
                            IntegerSequence,
                            ILongSparseMathMatrix<ElementType>,
                            ElementType[],
                            Tuple<int, ElementType>> refGetAlgorithm;

        /// <summary>
        /// O anel responsável pelas operações sobre os elementos.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// O comparador responsável pela comparação dos elementos.
        /// </summary>
        private IComparer<ElementType> comparer;

        public DirectGreedyAlgorithm(
            IComparer<ElementType> comparer,
            IRing<ElementType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.refGetAlgorithm = new DirectGreedyRefGetAlgorithm<ElementType>(
                    comparer,
                    ring);
                this.ring = ring;
                this.comparer = comparer;
            }
        }

        /// <summary>
        /// Executa o algoritmo guloso.
        /// </summary>
        /// <param name="matrices">A lista das matrizes que representam as componentes conexas.</param>
        /// <param name="refsNumber">O número de referências a serem escolhidas.</param>
        /// <returns>O conjunto de soluções por matriz.</returns>
        public GreedyAlgSolution<ElementType>[] Run(
            List<ILongSparseMathMatrix<ElementType>> matrices,
            int refsNumber)
        {
            if (matrices == null)
            {
                throw new ArgumentNullException("matrices");
            }
            else if (refsNumber < matrices.Count)
            {
                throw new ArgumentException("At leas one reference per matriz must be chosen.");
            }
            else
            {
                var lineBoards = this.SetupBoards(matrices);
                var result = this.SetupResults(lineBoards);
                var references = this.GetFirstRefs(matrices, result, lineBoards);
                for (int i = 0; i < refsNumber; ++i)
                {
                    var found = references[0].Item1;
                    var foundIndex = 0;
                    var max = references[0].Item2;
                    for (int j = 1; j < references.Length; ++j)
                    {
                        if (references[j].Item1 != -1 &&
                            this.comparer.Compare(max, references[j].Item2) <= 0)
                        {
                            max = references[j].Item2;
                            found = references[j].Item1;
                            foundIndex = j;
                        }
                    }

                    if (found != -1)
                    {
                        result[foundIndex].Chosen.Add(found);
                        result[foundIndex].Cost = this.ring.Add(
                            result[foundIndex].Cost,
                            this.ring.AdditiveInverse(max));
                        references[foundIndex] = this.refGetAlgorithm.Run(
                            result[foundIndex].Chosen,
                            matrices[foundIndex],
                            lineBoards[foundIndex]);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um enumerador para todas as soluções
        /// </summary>
        /// <param name="matrices">As matrizes.</param>
        /// <returns>O enumerador.</returns>
        public IEnumerable<GreedyAlgSolution<ElementType>[]> Run(
            List<ILongSparseMathMatrix<ElementType>> matrices)
        {
            var lineBoards = this.SetupBoards(matrices);
            var result = this.SetupResults(lineBoards);
            var references = this.GetFirstRefs(matrices, result, lineBoards);

            yield return result;

            var state = true;
            while (state)
            {
                var found = references[0].Item1;
                var foundIndex = 0;
                var max = references[0].Item2;
                for (int j = 1; j < references.Length; ++j)
                {
                    if (references[j].Item1 != -1 &&
                        this.comparer.Compare(max, references[j].Item2) <= 0)
                    {
                        max = references[j].Item2;
                        found = references[j].Item1;
                        foundIndex = j;
                    }
                }

                if (found != -1)
                {
                    result[foundIndex].Chosen.Add(found);
                    result[foundIndex].Cost = this.ring.Add(
                        result[foundIndex].Cost,
                        this.ring.AdditiveInverse(max));
                    references[foundIndex] = this.refGetAlgorithm.Run(
                        result[foundIndex].Chosen,
                        matrices[foundIndex],
                        lineBoards[foundIndex]);
                    yield return result;
                }
                else
                {
                    state = false;
                }
            }
        }

        /// <summary>
        /// Inicializa as linhas condensadas por matriz.
        /// </summary>
        /// <param name="matrices">O conjunto de matrizes.</param>
        /// <returns>O conjunto de linhsa condensadas.</returns>
        private ElementType[][] SetupBoards(List<ILongSparseMathMatrix<ElementType>> matrices)
        {
            var result = new ElementType[matrices.Count][];
            for (int i = 0; i < matrices.Count; ++i)
            {
                if (matrices[i].GetLength(0) == 0)
                {
                    throw new ArgumentException("Empty matrices are not allowed.");
                }

                var firstLine = matrices[i][0];
                var firstBoard = new ElementType[matrices[i].GetLength(1)];
                var columns = firstLine.GetColumns();
                foreach (var column in columns)
                {
                    firstBoard[column.Key] = column.Value;
                }

                result[i] = firstBoard;
            }

            return result;
        }

        /// <summary>
        /// Inicializa a solução.
        /// </summary>
        /// <param name="boards">As linhas condensadas por matriz.</param>
        /// <returns>A solução inicializada.</returns>
        private GreedyAlgSolution<ElementType>[] SetupResults(ElementType[][] boards)
        {
            var results = new GreedyAlgSolution<ElementType>[boards.Length];
            for (int i = 0; i < boards.Length; ++i)
            {
                var innerSum = this.ring.AdditiveUnity;
                foreach (var column in boards[i])
                {
                    innerSum = this.ring.Add(innerSum, column);
                }

                results[i] = new GreedyAlgSolution<ElementType>() { Cost = innerSum };
                results[i].Chosen.Add(0);
            }

            return results;
        }

        /// <summary>
        /// Obtém as referências iniciais.
        /// </summary>
        /// <param name="matrices">A lista de matrizes.</param>
        /// <param name="currentSolutions">As soluções correntes.</param>
        /// <param name="boards">A lista de linhas condensadas por matriz.</param>
        /// <returns>As referências.</returns>
        private Tuple<int, ElementType>[] GetFirstRefs(
            List<ILongSparseMathMatrix<ElementType>> matrices,
            GreedyAlgSolution<ElementType>[] currentSolutions,
            ElementType[][] boards)
        {
            var result = new Tuple<int, ElementType>[matrices.Count];
            Parallel.For(0, matrices.Count, matr =>
            {
                result[matr] = this.refGetAlgorithm.Run(
                    currentSolutions[matr].Chosen,
                    matrices[matr],
                    boards[matr]);
            });

            return result;
        }
    }
}
