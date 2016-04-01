namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementa o algoritmo que permite obter a próxima referência.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de dados utilizado na matriz dos custos.</typeparam>
    public class DirectGreedyRefGetAlgorithm<ElementType>
        : IAlgorithm<IntegerSequence, ILongSparseMathMatrix<ElementType>, ElementType[], Tuple<int, ElementType>>
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

        public DirectGreedyRefGetAlgorithm(IComparer<ElementType> comparer, IRing<ElementType> ring)
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
        /// Escolhe a referência que maximiza o ganho.
        /// </summary>
        /// <param name="chosenReferences">As referências escolhidas anteriormente.</param>
        /// <param name="currentMatrix">A matriz dos custos.</param>
        /// <param name="currentLineBoard">A linha que contém a condensação dos custos das linhas escolhidas.</param>
        /// <returns>O índice da linha corresponde à próxima referência bem como o ganho respectivo.</returns>
        public Tuple<int, ElementType> Run(
            IntegerSequence chosenReferences,
            ILongSparseMathMatrix<ElementType> currentMatrix,
            ElementType[] currentLineBoard)
        {
            var result = default(KeyValuePair<int, ILongSparseMatrixLine<ElementType>>);
            var lines = currentMatrix.GetLines();
            var currentMaxGain = this.ring.AdditiveInverse(this.ring.MultiplicativeUnity);
            var foundRef = false;

            Parallel.ForEach(lines,
                line =>
                {
                    if (!chosenReferences.Contains(line.Key))
                    {
                        var sum = this.ring.AdditiveUnity;

                        foreach (var column in line.Value)
                        {
                            if (column.Key != line.Key)
                            {
                                var currentValue = column.Value;
                                if (this.comparer.Compare(currentValue, currentLineBoard[column.Key]) < 0)
                                {
                                    var difference = this.ring.Add(
                                        currentLineBoard[column.Key],
                                        this.ring.AdditiveInverse(currentValue));
                                    sum = this.ring.Add(sum, difference);
                                }
                            }
                            else
                            {
                                sum = this.ring.Add(sum, currentLineBoard[column.Key]);
                            }
                        }

                        lock (this.lockObject)
                        {
                            if (this.comparer.Compare(currentMaxGain, sum) < 0)
                            {
                                currentMaxGain = sum;
                                result = line;
                                foundRef = true;
                            }
                        }
                    }
                });

            if (foundRef)
            {
                foreach (var column in result.Value.GetColumns())
                {
                    if (column.Key != result.Key)
                    {
                        if (this.comparer.Compare(column.Value, currentLineBoard[column.Key]) < 0)
                        {
                            currentLineBoard[column.Key] = column.Value;
                        }
                    }
                    else
                    {
                        currentLineBoard[column.Key] = this.ring.AdditiveUnity;
                    }
                }

                return Tuple.Create(result.Key, currentMaxGain);
            }
            else
            {
                return Tuple.Create(-1, currentMaxGain);
            }
        }
    }
}
