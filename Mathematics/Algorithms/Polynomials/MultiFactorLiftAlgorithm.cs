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
    public class MultiFactorLiftAlgorithm : IAlgorithm<
        MultiFactorLiftingStatus<int>,
        List<UnivariatePolynomialNormalForm<int>>>
    {
        /// <summary>
        /// Aplica o algoritmo um número especificado de vezes ou até ser encontrada uma factorização.
        /// </summary>
        /// <param name="multiFactorLiftingStatus">O estado do levantamento multifactor actual.</param>
        /// <returns>A lista com os factores.</returns>
        public List<UnivariatePolynomialNormalForm<int>> Run(
            MultiFactorLiftingStatus<int> multiFactorLiftingStatus)
        {
            var constants = new List<UnivariatePolynomialNormalForm<int>>();
            var factorTree = this.MountFactorTree(
                multiFactorLiftingStatus.Factors,
                multiFactorLiftingStatus.ModularField,
                multiFactorLiftingStatus.MainDomain,
                constants);
            if (factorTree == null)
            {
                // Não existem factores suficientes para levantar
            }
            else
            {
                // A árvore contém factores para levantar
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Contrói a árvore de factores.
        /// </summary>
        /// <param name="polynomial">O polinómio factorizado.</param>
        /// <param name="factorsList">A lista de factores.</param>
        /// <param name="modularField">O corpo modular sobre o qual são efectuadas as operações.</param>
        /// <returns>A árvore.</returns>
        private Tree<LinearLiftingStatus<int>> MountFactorTree(
            List<UnivariatePolynomialNormalForm<int>> factorsList,
            IModularField<int> modularField,
            IEuclidenDomain<int> mainDomain,
            List<UnivariatePolynomialNormalForm<int>> coefficientFactors)
        {
            var tree = new Tree<LinearLiftingStatus<int>>();
            var currentNodes = new List<TreeNode<LinearLiftingStatus<int>>>();
            foreach (var factor in factorsList)
            {
                if (factor.Degree == 0)
                {
                    coefficientFactors.Add(factor);
                }
                else
                {
                    currentNodes.Add(new TreeNode<LinearLiftingStatus<int>>(
                        new LinearLiftingStatus<int>(factor, modularField, mainDomain),
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
                var temporaryNodes = new List<TreeNode<LinearLiftingStatus<int>>>();
                var count = 0;
                while (currentNodes.Count > 1)
                {
                    temporaryNodes.Clear();
                    var i = 0;
                    while (i < currentNodes.Count)
                    {
                        var parentNode = default(TreeNode<LinearLiftingStatus<int>>);
                        var first = currentNodes[i];
                        ++i;
                        if (i < currentNodes.Count)
                        {
                            var second = currentNodes[i];
                            var product = first.NodeObject.Polynom.Multiply(second.NodeObject.Polynom, modularField);
                            var liftingStatus = new LinearLiftingStatus<int>(
                                product,
                                first.NodeObject.Polynom,
                                second.NodeObject.Polynom,
                                modularField,
                                mainDomain);
                            parentNode = new TreeNode<LinearLiftingStatus<int>>(
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
    }
}
