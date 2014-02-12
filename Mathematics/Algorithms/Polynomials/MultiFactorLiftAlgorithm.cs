namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite realizar um levantamento multifactor.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de dados do polinómio.</typeparam>
    public class MultiFactorLiftAlgorithm<CoeffType> : IAlgorithm<
        MultiFactorLiftingStatus<CoeffType>,
        int,
        MultiFactorLiftingResult<CoeffType>>
    {
        private IAlgorithm<LinearLiftingStatus<CoeffType>, int, bool> linearLiftAlg;

        public MultiFactorLiftAlgorithm(IAlgorithm<LinearLiftingStatus<CoeffType>, int, bool> linearLiftAlg)
        {
            if (linearLiftAlg == null)
            {
                throw new ArgumentNullException("linearLiftAlg");
            }
            else
            {
                this.linearLiftAlg = linearLiftAlg;
            }
        }

        /// <summary>
        /// Aplica o algoritmo um número especificado de vezes ou até ser encontrada uma factorização.
        /// </summary>
        /// <param name="multiFactorLiftingStatus">O estado do levantamento multifactor actual.</param>
        /// <param name="numberOfIterations">O número de iterações a ser efectuado.</param>
        /// <returns>A lista com os factores.</returns>
        public MultiFactorLiftingResult<CoeffType> Run(
            MultiFactorLiftingStatus<CoeffType> multiFactorLiftingStatus,
            int numberOfIterations)
        {
            var constants = new List<UnivariatePolynomialNormalForm<CoeffType>>();
            var factorTree = this.MountFactorTree(
                multiFactorLiftingStatus.Factors,
                multiFactorLiftingStatus.ModularField,
                multiFactorLiftingStatus.MainDomain,
                constants);
            factorTree.InternalRootNode.NodeObject.Polynom = multiFactorLiftingStatus.Polynom;
            if (factorTree == null)
            {
                // Não existem factores suficientes para elevar
                var result = new List<UnivariatePolynomialNormalForm<CoeffType>>();
                result.AddRange(multiFactorLiftingStatus.Factors);
                return new MultiFactorLiftingResult<CoeffType>(
                    multiFactorLiftingStatus.Polynom,
                    result, 
                    multiFactorLiftingStatus.LiftedFactorizationModule);
            }
            else
            {
                // A árvore contém factores para elevar
                var factorQueue = new Queue<TreeNode<LinearLiftingStatus<CoeffType>>>();
                var factorPrime = multiFactorLiftingStatus.LiftedFactorizationModule;
                for (int i = 0; i < numberOfIterations; ++i)
                {
                    factorQueue.Enqueue(factorTree.InternalRootNode);
                    while (factorQueue.Count > 0)
                    {
                        var dequeued = factorQueue.Dequeue();
                        if (dequeued.Count == 2)
                        {
                            if (this.linearLiftAlg.Run(dequeued.NodeObject, 1))
                            {
                                factorPrime = dequeued.NodeObject.LiftedFactorizationModule;
                            }

                            var solution = dequeued.NodeObject.GetSolution();
                            var firstChild = dequeued.ChildsList[0];
                            var secondChild = dequeued.ChildsList[1];
                            firstChild.NodeObject.Polynom = solution.Item1;
                            secondChild.NodeObject.Polynom = solution.Item2;
                            factorQueue.Enqueue(firstChild);
                            factorQueue.Enqueue(secondChild);
                        }
                    }
                }

                var mainDomain = multiFactorLiftingStatus.MainDomain;
                var treeSolution = this.GetSolutionFromTree(factorTree, mainDomain);
                if (treeSolution.Count > 0)
                {
                    var firstFactor = treeSolution[0];
                    if (firstFactor.IsValue)
                    {
                        for (int i = 0; i < constants.Count; ++i)
                        {
                            firstFactor = firstFactor.Multiply(constants[i], mainDomain);
                        }

                        if (firstFactor.IsUnity(mainDomain))
                        {
                            treeSolution.RemoveAt(0);
                        }
                        else
                        {
                            treeSolution[0] = firstFactor;
                        }
                    }
                }

                return new MultiFactorLiftingResult<CoeffType>(
                    multiFactorLiftingStatus.Polynom,
                    treeSolution, 
                    factorPrime);
            } 
        }

        /// <summary>
        /// Contrói a árvore de factores.
        /// </summary>
        /// <param name="polynomial">O polinómio factorizado.</param>
        /// <param name="factorsList">A lista de factores.</param>
        /// <param name="modularField">O corpo modular sobre o qual são efectuadas as operações.</param>
        /// <returns>A árvore.</returns>
        private Tree<LinearLiftingStatus<CoeffType>> MountFactorTree(
            List<UnivariatePolynomialNormalForm<CoeffType>> factorsList,
            IModularField<CoeffType> modularField,
            IEuclidenDomain<CoeffType> mainDomain,
            List<UnivariatePolynomialNormalForm<CoeffType>> coefficientFactors)
        {
            var tree = new Tree<LinearLiftingStatus<CoeffType>>();
            var currentNodes = new List<TreeNode<LinearLiftingStatus<CoeffType>>>();
            foreach (var factor in factorsList)
            {
                if (factor.Degree == 0)
                {
                    coefficientFactors.Add(factor);
                }
                else
                {
                    currentNodes.Add(new TreeNode<LinearLiftingStatus<CoeffType>>(
                        new LinearLiftingStatus<CoeffType>(factor, modularField, mainDomain),
                        tree,
                        null));
                }
            }

            if (currentNodes.Count < 2)
            {
                return null;
            }
            else
            {
                var temporaryNodes = new List<TreeNode<LinearLiftingStatus<CoeffType>>>();
                var count = 0;
                while (currentNodes.Count > 1)
                {
                    temporaryNodes.Clear();
                    var i = 0;
                    while (i < currentNodes.Count)
                    {
                        var parentNode = default(TreeNode<LinearLiftingStatus<CoeffType>>);
                        var first = currentNodes[i];
                        ++i;
                        if (i < currentNodes.Count)
                        {
                            var second = currentNodes[i];
                            var product = first.NodeObject.Polynom.Multiply(second.NodeObject.Polynom, modularField);
                            var liftingStatus = new LinearLiftingStatus<CoeffType>(
                                product,
                                first.NodeObject.Polynom,
                                second.NodeObject.Polynom,
                                modularField,
                                mainDomain);
                            parentNode = new TreeNode<LinearLiftingStatus<CoeffType>>(
                                liftingStatus,
                                tree,
                                null);

                            first.InternalParent = parentNode;
                            second.InternalParent = parentNode;
                            parentNode.Add(first);
                            parentNode.Add(second);
                            ++i;
                        }
                        else
                        {
                            parentNode = first;
                        }

                        temporaryNodes.Add(parentNode);
                    }

                    var swap = currentNodes;
                    currentNodes = temporaryNodes;
                    temporaryNodes = swap;
                    ++count;
                }

                var lastNode = currentNodes[0];
                tree.InternalRootNode = lastNode;
                return tree;
            }
        }

        /// <summary>
        /// Obtém a solução a partir da árvore de factores.
        /// </summary>
        /// <param name="tree">A árvore de factores.</param>
        /// <returns>A solução.</returns>
        private List<UnivariatePolynomialNormalForm<CoeffType>> GetSolutionFromTree(
            Tree<LinearLiftingStatus<CoeffType>> tree,
            IEuclidenDomain<CoeffType> mainDomain)
        {
            var result = new List<UnivariatePolynomialNormalForm<CoeffType>>();

            var factorConstant = mainDomain.MultiplicativeUnity;
            var factorQueue = new Queue<TreeNode<LinearLiftingStatus<CoeffType>>>();
            factorQueue.Enqueue(tree.InternalRootNode);
            while (factorQueue.Count > 0)
            {
                var dequeued = factorQueue.Dequeue();
                if (dequeued.Count == 0)
                {
                    var current = dequeued.NodeObject.Polynom;
                    if (current.IsValue)
                    {
                        factorConstant = mainDomain.Multiply(factorConstant, current.GetAsValue(mainDomain));
                    }
                    else
                    {
                        result.Add(current);
                    }
                }
                else
                {
                    factorQueue.Enqueue(dequeued.ChildsList[0]);
                    factorQueue.Enqueue(dequeued.ChildsList[1]);
                }
            }

            if (!mainDomain.IsMultiplicativeUnity(factorConstant))
            {
                result.Insert(0, new UnivariatePolynomialNormalForm<CoeffType>(factorConstant, 0, "x", mainDomain));
            }

            return result;
        }
    }
}
