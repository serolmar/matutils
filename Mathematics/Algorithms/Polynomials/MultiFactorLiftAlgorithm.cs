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
        private ILinearLiftAlgorithm<CoeffType> linearLiftAlg;

        public MultiFactorLiftAlgorithm(ILinearLiftAlgorithm<CoeffType> linearLiftAlg)
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
            if (multiFactorLiftingStatus.Factorization.Factors.Count > 1)
            {
                var modularField = this.linearLiftAlg.ModularFieldFactory.CreateInstance(
                    multiFactorLiftingStatus.LiftedFactorizationModule);
                var factorTree = this.MountFactorTree(
                    multiFactorLiftingStatus,
                    modularField);
                factorTree.InternalRootNode.NodeObject.Polynom = multiFactorLiftingStatus.Polynom;
                if (factorTree == null)
                {
                    // Não existem factores suficientes para elevar
                    var result = new List<UnivariatePolynomialNormalForm<CoeffType>>();
                    result.AddRange(multiFactorLiftingStatus.Factorization.Factors);
                    return new MultiFactorLiftingResult<CoeffType>(
                        multiFactorLiftingStatus.Polynom,
                        result,
                        multiFactorLiftingStatus.LiftedFactorizationModule);
                }
                else
                {
                    // A árvore contém factores para elevar
                    var factorQueue = new Queue<TreeNode<LinearLiftingStatus<CoeffType>>>();
                    var factorTreeRootNode = factorTree.RootNode.NodeObject.Polynom;

                    factorQueue.Enqueue(factorTree.InternalRootNode);
                    while (factorQueue.Count > 0)
                    {
                        var dequeued = factorQueue.Dequeue();
                        if (dequeued.Count == 2)
                        {
                            this.linearLiftAlg.Run(dequeued.NodeObject, numberOfIterations);
                            if (this.linearLiftAlg.IntegerNumber.Compare(
                                multiFactorLiftingStatus.LiftedFactorizationModule,
                                dequeued.NodeObject.LiftedFactorizationModule) < 0)
                            {
                                multiFactorLiftingStatus.LiftedFactorizationModule = dequeued.NodeObject.LiftedFactorizationModule;
                            }

                            var firstChild = dequeued.ChildsList[0];
                            var secondChild = dequeued.ChildsList[1];
                            firstChild.NodeObject.Polynom = dequeued.NodeObject.UFactor;
                            secondChild.NodeObject.Polynom = dequeued.NodeObject.WFactor;
                            factorQueue.Enqueue(firstChild);
                            factorQueue.Enqueue(secondChild);
                        }
                    }

                    var treeSolution = this.GetSolutionFromTree(factorTree, this.linearLiftAlg.IntegerNumber);

                    return new MultiFactorLiftingResult<CoeffType>(
                        factorTreeRootNode,
                        treeSolution,
                        multiFactorLiftingStatus.LiftedFactorizationModule);
                }
            }
            else
            {
                return new MultiFactorLiftingResult<CoeffType>(
                    multiFactorLiftingStatus.Polynom,
                    multiFactorLiftingStatus.Factorization.Factors,
                    multiFactorLiftingStatus.LiftedFactorizationModule);
            }
        }

        /// <summary>
        /// Contrói a árvore de factores.
        /// </summary>
        /// <param name="multiFactorLiftingStatus">Contém a factorização que se pretende elevar.</param>
        /// <param name="modularField">O corpo modular sobre o qual são efectuadas as operações.</param>
        /// <returns>A árvore.</returns>
        private Tree<LinearLiftingStatus<CoeffType>> MountFactorTree(
            MultiFactorLiftingStatus<CoeffType> multiFactorLiftingStatus,
            IModularField<CoeffType> modularField)
        {
            var tree = new Tree<LinearLiftingStatus<CoeffType>>();
            var currentNodes = new List<TreeNode<LinearLiftingStatus<CoeffType>>>();
            var factorEnumerator = multiFactorLiftingStatus.Factorization.Factors.GetEnumerator();
            if (factorEnumerator.MoveNext())
            {
                var factor = factorEnumerator.Current;
                if (!modularField.IsMultiplicativeUnity(multiFactorLiftingStatus.Factorization.IndependentCoeff))
                {
                    factor = factor.Multiply(multiFactorLiftingStatus.Factorization.IndependentCoeff, modularField);
                }

                currentNodes.Add(new TreeNode<LinearLiftingStatus<CoeffType>>(
                        new LinearLiftingStatus<CoeffType>(factor, modularField.Module),
                        tree,
                        null));

                while (factorEnumerator.MoveNext())
                {
                    factor = factorEnumerator.Current;
                    currentNodes.Add(new TreeNode<LinearLiftingStatus<CoeffType>>(
                        new LinearLiftingStatus<CoeffType>(factor, modularField.Module),
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
                            var product = first.NodeObject.Polynom.Multiply(
                                second.NodeObject.Polynom,
                                modularField);

                            var liftingStatus = new LinearLiftingStatus<CoeffType>(
                                product,
                                first.NodeObject.Polynom,
                                second.NodeObject.Polynom,
                                modularField.Module);

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
