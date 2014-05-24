namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Aplica o algoritmo húngaro de modo a verificar se é possível formar uma correspondência
    /// entre dois conjuntos. Se não for possível, indica os maiores subconjunto tanto do lado do domínio
    /// como da imagem sobre os quais é possível estabelecer uma correspondência.
    /// </summary>
    public class HungarianMatchingAlgorithm : IAlgorithm<int[][], HungarianMatchingOutput>
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="HungarianMatchingAlgorithm"/>.
        /// </summary>
        public HungarianMatchingAlgorithm()
        {
        }

        /// <summary>
        /// Corre o algoritmo.
        /// </summary>
        /// <param name="affectations">A lista de correspondências possíveis.</param>
        public HungarianMatchingOutput Run(int[][] affectations)
        {
            var results = default(int[]);
            var innerAffectations = this.Init(affectations, results);
            var maximumDomainAffected = default(int[]);
            var maximumTargetAffected = default(int[]);
            var hasSolution = false;
            InsertionSortedCollection<int> domainSubset = new InsertionSortedCollection<int>(
                Comparer<int>.Default, 
                true);
            InsertionSortedCollection<int> targetSubset = new InsertionSortedCollection<int>(
                Comparer<int>.Default, 
                true);
            InsertionSortedCollection<int> neighboursSubset = new InsertionSortedCollection<int>(
                Comparer<int>.Default, 
                true);
            List<Tuple<int, int>> alternatingPath = new List<Tuple<int, int>>();
            int domainFree = -1;
            int found = -1;
            int targetUncommon = -1;
            int state = 0;
            while (state != -1)
            {
                switch (state)
                {
                    case 0:
                        for (int i = 0; i < innerAffectations.Length; ++i)
                        {
                            if (innerAffectations[i].Count != 0)
                            {
                                int temp = innerAffectations[i].First;
                                results[i] = temp;
                                domainSubset.InsertSortElement(i);
                                i = innerAffectations.Length;  // Termina o ciclo de forma elegante.
                            }
                        }

                        state = 1;
                        break;
                    case 1:
                        targetSubset.Clear();
                        domainSubset.Clear();
                        neighboursSubset.Clear();
                        alternatingPath.Clear();
                        domainFree = -1;
                        for (int i = 0; i < results.Length; ++i)
                        {
                            if (results[i] == -1)
                            {
                                domainFree = i;
                                domainSubset.InsertSortElement(domainFree);
                                i = results.Length;
                            }
                        }

                        // Uma solução existe.
                        if (domainFree == -1)
                        {
                            domainSubset.Clear();
                            neighboursSubset.Clear();
                            for (int i = 0; i < innerAffectations.Length; ++i)
                            {
                                domainSubset.InsertSortElement(i);
                            }

                            foreach (var item in domainSubset)
                            {
                                foreach (var possibleAffectations in innerAffectations[item])
                                {
                                    neighboursSubset.InsertSortElement(possibleAffectations);
                                }
                            }

                            maximumDomainAffected = domainSubset.ToArray();
                            maximumTargetAffected = neighboursSubset.ToArray();
                            hasSolution = true;
                            state = -1;
                        }
                        else
                        {
                            state = 2;
                        }

                        break;
                    case 2:
                        foreach (var item in domainSubset)
                        {
                            foreach (var possibleAffectations in innerAffectations[item])
                            {
                                neighboursSubset.InsertSortElement(possibleAffectations);
                            }
                        }

                        targetUncommon = -1;
                        if (!neighboursSubset.TryFindValueNotIn(targetSubset, out targetUncommon))
                        {
                            maximumDomainAffected = domainSubset.ToArray();
                            maximumTargetAffected = neighboursSubset.ToArray();
                            state = -1;
                        }
                        else
                        {
                            found = -1;
                            for (int i = 0; i < results.Length; ++i)
                            {
                                if (results[i] == targetUncommon)
                                {
                                    found = i;
                                    domainSubset.InsertSortElement(found);
                                    targetSubset.InsertSortElement(targetUncommon);
                                    i = results.Length;
                                }
                            }

                            if (found == -1)
                            {
                                results[domainFree] = targetUncommon;
                                state = 1;
                            }
                            else
                            {
                                alternatingPath.Add(new Tuple<int, int>(domainFree, targetUncommon));
                                state = 3;
                            }
                        }

                        break;
                    case 3:
                        foreach (var item in domainSubset)
                        {
                            foreach (var possibleAffectations in innerAffectations[item])
                            {
                                neighboursSubset.InsertSortElement(possibleAffectations);
                            }
                        }

                        targetUncommon = -1;
                        if (!neighboursSubset.TryFindValueNotIn(targetSubset, out targetUncommon))
                        {
                            maximumDomainAffected = domainSubset.ToArray();
                            maximumTargetAffected = neighboursSubset.ToArray();
                            state = -1;
                        }
                        else
                        {
                            alternatingPath.Add(new Tuple<int, int>(found, targetUncommon));

                            found = -1;
                            for (int i = 0; i < results.Length; ++i)
                            {
                                if (results[i] == targetUncommon)
                                {
                                    found = i;
                                    domainSubset.InsertSortElement(found);
                                    targetSubset.InsertSortElement(targetUncommon);
                                    i = results.Length;
                                }
                            }

                            if (found == -1)
                            {
                                foreach (var pair in alternatingPath)
                                {
                                    results[pair.Item1] = pair.Item2;
                                }

                                state = 1;
                            }
                        }

                        break;
                }
            }

            return new HungarianMatchingOutput(
                results, 
                maximumDomainAffected, 
                maximumTargetAffected,
                hasSolution);
        }

        /// <summary>
        /// Inicializa todas as variáveis.
        /// </summary>
        /// <param name="affectations">A matriz das afectações.</param>
        /// <param name="results">O vector que contém os resultados.</param>
        /// <exception cref="ArgumentNullException">Se as afectações forem nulas.</exception>
        /// <exception cref="MathematicsException">Se não existirem elementos para afectar.</exception>
        public InsertionSortedCollection<int>[] Init(
            int[][] affectations,
            int[] results)
        {
            if (affectations == null)
            {
                throw new ArgumentNullException("affectations");
            }

            if (affectations.Length == 0)
            {
                throw new MathematicsException("There's no elements to affect.");
            }

            results = new int[affectations.Length];
            this.ClearResults(results);
            var innerAffectations = new InsertionSortedCollection<int>[affectations.Length];
            for (int i = 0; i < affectations.Length; ++i)
            {
                if (affectations[i] == null)
                {
                    throw new MathematicsException(string.Format(
                        "Element {0} has no positions to be affected.", i));
                }

                if (affectations[i].Length == 0)
                {
                    throw new MathematicsException(string.Format(
                        "Element {0} has no positions to be affected.", i));
                }

                innerAffectations[i] = new InsertionSortedCollection<int>(Comparer<int>.Default, true);
                foreach (var item in affectations[i])
                {
                    innerAffectations[i].InsertSortElement(item);
                }
            }

            return innerAffectations;
        }

        /// <summary>
        /// Limpa os resultados.
        /// </summary>
        /// <param name="results">Os resultados a serem limpos.</param>
        private void ClearResults(int[] results)
        {
            for (int i = 0; i < results.Length; ++i)
            {
                results[i] = -1;
            }
        }
    }
}
