namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Obtém uma estimativa para a variável gama.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de dados dos custos.</typeparam>
    public class DualMatrixGammaEstimAlgorithm<ElementType>
        : IAlgorithm<SparseDictionaryMatrix<ElementType>, DualHeuristicAlgInput<ElementType>, ElementType>
    {
        /// <summary>
        /// O anel responsável pelas operações.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// O comparador de elementos.
        /// </summary>
        private IComparer<ElementType> comparer;

        public DualMatrixGammaEstimAlgorithm(IComparer<ElementType> comparer, IRing<ElementType> ring)
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
                this.ring = ring;
                this.comparer = comparer;
            }
        }

        public ElementType Run(
            SparseDictionaryMatrix<ElementType> matrix, 
            DualHeuristicAlgInput<ElementType> input)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            else
            {
                input.Taus.Clear();
                var result = new Dictionary<int, ElementType>();
                input.Gamma = this.ring.AdditiveUnity;
                var currentGamma = this.ring.AdditiveUnity;
                foreach (var line in matrix.GetLines())
                {
                    // Setup the tau and cbar values
                    var tempTau = input.Lambdas[line.Key];
                    foreach (var coveredLine in line.Value.GetColumns())
                    {
                        if (coveredLine.Key != line.Key)
                        {
                            var difference = this.ring.Add(
                                input.Lambdas[coveredLine.Key],
                                this.ring.AdditiveInverse(coveredLine.Value));
                            if (this.comparer.Compare(difference, this.ring.AdditiveUnity) < 0)
                            {
                                tempTau = this.ring.Add(tempTau, difference);
                            }
                        }
                    }

                    if (this.comparer.Compare(input.Gamma, tempTau) < 0)
                    {
                        input.Gamma = tempTau;
                    }

                    if (this.comparer.Compare(currentGamma, tempTau) < 0)
                    {
                        currentGamma = tempTau;
                    }

                    input.Taus.Add(line.Key, tempTau);
                }

                foreach (var line in matrix.GetLines())
                {
                    foreach (var coveredLine in line.Value.GetColumns())
                    {
                        if (coveredLine.Key != line.Key)
                        {
                            var tempCbarValue = this.ring.Add(
                                coveredLine.Value,
                                this.ring.AdditiveInverse(input.Lambdas[coveredLine.Key]));
                            if (this.comparer.Compare(this.ring.AdditiveUnity, tempCbarValue) < 0)
                            {
                                var currentCbarInCoveredVertice = this.ring.AdditiveUnity;
                                if (input.Cbar.TryGetValue(coveredLine.Key, out currentCbarInCoveredVertice))
                                {
                                    if (this.comparer.Compare(tempCbarValue, currentCbarInCoveredVertice) < 0)
                                    {
                                        input.Cbar[coveredLine.Key] = coveredLine.Value;
                                    }
                                }
                                else
                                {
                                    input.Cbar.Add(coveredLine.Key, coveredLine.Value);
                                }
                            }
                        }
                    }
                }

                return currentGamma;
            }
        }
    }
}
