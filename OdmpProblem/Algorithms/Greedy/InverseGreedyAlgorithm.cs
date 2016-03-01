namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Mathematics;
    using Utilities;

    public class InverseGreedyAlgorithm<ElementType>
        : IAlgorithm<List<SparseDictionaryMathMatrix<ElementType>>, int, GreedyAlgSolution<ElementType>[]>,
        IAlgorithm<List<SparseDictionaryMathMatrix<ElementType>>, IEnumerable<GreedyAlgSolution<ElementType>[]>>
    {
        /// <summary>
        /// Mantém o algoritmo que permite a obtenção da próxima referência.
        /// </summary>
        private IAlgorithm<
                            IntegerSequence,
                            SparseDictionaryMathMatrix<ElementType>,
                            ElementType[],
                            Tuple<int, ElementType>> refGetAlgorithm;

        /// <summary>
        /// O anel responsável pelas operações sobre os elementos.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// O comparador responsável pela comparação dos elementos.
        /// </summary>
        private Comparer<ElementType> comparer;

        public InverseGreedyAlgorithm(
            Comparer<ElementType> comparer,
            IRing<ElementType> ring,
            IAlgorithm<
                            IntegerSequence,
                            SparseDictionaryMathMatrix<ElementType>,
                            ElementType[],
                            Tuple<int, ElementType>> refGetAlgorithm)
        {
            if (refGetAlgorithm == null)
            {
                throw new ArgumentNullException("refGetAlgorithm");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.refGetAlgorithm = new InverseGreedyRefGetAlgorithm<ElementType>(
                    comparer,
                    ring);
                this.ring = ring;
                this.comparer = comparer;
            }
        }

        /// <summary>
        /// Executa o algoritmo guloso invertido.
        /// </summary>
        /// <param name="matrices">A lista das matrizes que representam as componentes conexas.</param>
        /// <param name="refsNumber">O número de referências a serem escolhidas.</param>
        /// <returns>O conjunto de soluções por matriz.</returns>
        public GreedyAlgSolution<ElementType>[] Run(
            List<SparseDictionaryMathMatrix<ElementType>> matrices, 
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
                for (int i = this.CountVertices(matrices); i > refsNumber; --i)
                {
                    var found = references[0].Item1;
                    var foundIndex = 0;
                    var minLost = references[0].Item2;
                    for (int j = 1; j < references.Length; ++j)
                    {
                        if (references[j].Item1 != -1 && 
                            this.comparer.Compare(minLost, references[j].Item2) < 0)
                        {
                            minLost = references[j].Item2;
                            found = references[j].Item1;
                            foundIndex = j;
                        }
                    }

                    if (found != -1)
                    {
                        var chosen = result[foundIndex].Chosen;
                        result[foundIndex].Chosen.Remove(chosen[found]);
                        result[foundIndex].Cost = this.ring.Add(result[foundIndex].Cost, minLost);
                        references[foundIndex] = this.refGetAlgorithm.Run(
                            chosen,
                            matrices[foundIndex],
                            lineBoards[foundIndex]);
                    }
                }

                return result; ;
            }
        }

        /// <summary>
        /// Obtém um enumerador para todas as soluções
        /// </summary>
        /// <param name="matrices">As matrizes.</param>
        /// <returns>O enumerador.</returns>
        public IEnumerable<GreedyAlgSolution<ElementType>[]> Run(List<SparseDictionaryMathMatrix<ElementType>> matrices)
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
        private ElementType[][] SetupBoards(List<SparseDictionaryMathMatrix<ElementType>> matrices)
        {
            var result = new ElementType[matrices.Count][];
            for (int i = 0; i < matrices.Count; ++i)
            {
                if (matrices[i].GetLength(0) == 0)
                {
                    throw new ArgumentException("Empty matrices are not allowed.");
                }

                var firstLine = matrices[i][0];
                var firstBoard = new ElementType[firstLine.NumberOfColumns + 1];
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
                var linesCount = boards[i].Length;
                results[i] = new GreedyAlgSolution<ElementType>() { Cost = this.ring.AdditiveUnity };
                results[i].Chosen.Add(0, linesCount - 1);
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
            List<SparseDictionaryMathMatrix<ElementType>> matrices,
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

        /// <summary>
        /// Obtém o custo inicial da matriz especificada.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <returns>O custo.</returns>
        private ElementType GetMatrixInitialCost(SparseDictionaryMathMatrix<ElementType> matrix)
        {
            var result = this.ring.AdditiveUnity;
            if (matrix.GetLength(0) > 0)
            {
                var firstLine = matrix[0];
                var coverColumns = firstLine.GetColumns();
                foreach (var column in coverColumns)
                {
                    result = this.ring.Add(result, column.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o número de vértices do grafo.
        /// </summary>
        /// <param name="matrices">O número de matrizes.</param>
        /// <returns>O número de vértices.</returns>
        private int CountVertices(List<SparseDictionaryMathMatrix<ElementType>> matrices)
        {
            var result = 0;
            for (int i = 0; i < matrices.Count; ++i)
            {
                var componentMatrix = matrices[i];
                var dimension = componentMatrix.GetLength(1);
                if (dimension > 0)
                {
                    result += dimension;
                }
            }

            return result;
        }
    }
}
