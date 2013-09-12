namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Implementa o algoritmo de inicialização.
    /// </summary>
    public class DualHeuristicInitAlgorithm<ElementType>
        : IAlgorithm<SparseDictionaryMatrix<ElementType>, 
                     GreedyAlgSolution<ElementType>, 
                     DualHeuristicAlgInput<ElementType>, bool>
    {
        /// <summary>
        /// O anel responsável pelas operações.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// O comparador de elementos.
        /// </summary>
        private IComparer<ElementType> comparer;

        public DualHeuristicInitAlgorithm(IComparer<ElementType> comparer, IRing<ElementType> ring)
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
                this.comparer = comparer;
                this.ring = ring;
            }
        }

        /// <summary>
        /// Obtém os valores iniciais das variáveis lambda.
        /// </summary>
        /// <param name="matrix">A matriz dos custos.</param>
        /// <param name="greedySolution">A solução do algoritmo guloso.</param>
        /// <param name="dualInput">A entrada para o algoritmo dual propriamente dito.</param>
        /// <returns>Verdadeiro caso o algoritmo seja bem sucedido e falso caso contrário.</returns>
        public bool Run(
            SparseDictionaryMatrix<ElementType> matrix, 
            GreedyAlgSolution<ElementType> greedySolution, 
            DualHeuristicAlgInput<ElementType> dualInput)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else if (greedySolution == null)
            {
                throw new ArgumentNullException("greedySolution");
            }
            else if (dualInput == null)
            {
                throw new ArgumentNullException("dualInput");
            }
            else
            {
                dualInput.Lambdas.Clear();
                var componentEnum = matrix.GetLines().GetEnumerator();
                if (componentEnum.MoveNext())
                {
                    dualInput.Lambdas.Add(componentEnum.Current.Key, this.ring.AdditiveUnity);
                    foreach (var column in componentEnum.Current.Value.GetColumns())
                    {
                        if (column.Key != componentEnum.Current.Key)
                        {
                            if (greedySolution.Chosen.Contains(column.Key))
                            {
                                dualInput.Lambdas.Add(column.Key, this.ring.AdditiveUnity);
                            }
                            else
                            {
                                dualInput.Lambdas.Add(column.Key, column.Value);
                            }
                        }
                    }

                    while (componentEnum.MoveNext())
                    {
                        if (greedySolution.Chosen.Contains(componentEnum.Current.Key))
                        {
                            foreach (var column in componentEnum.Current.Value.GetColumns())
                            {
                                if (column.Key != componentEnum.Current.Key)
                                {
                                    var forComponentValue = this.ring.AdditiveUnity;
                                    if (dualInput.Lambdas.TryGetValue(column.Key, out forComponentValue))
                                    {
                                        if (this.comparer.Compare(column.Value, forComponentValue) < 0)
                                        {
                                            dualInput.Lambdas[column.Key] = column.Value;
                                        }
                                    }
                                    else
                                    {
                                        dualInput.Lambdas.Add(column.Key, this.ring.AdditiveUnity);
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
        }
    }
}
