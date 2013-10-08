using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Obtém a factorização de um número aplicando sucessivamente um outro algoritmo
    /// que permita obter os respectivos factores.
    /// </summary>
    public class DecompositionFactorizationAlgorithm : IAlgorithm<int, Dictionary<int, int>>
    {
        private IAlgorithm<int, Tuple<int, int>> productDecompAlg;

        public DecompositionFactorizationAlgorithm(
            IAlgorithm<int, Tuple<int, int>> productDecompAlg)
        {
            if (productDecompAlg == null)
            {
                throw new ArgumentNullException("productDecompAlg");
            }
            else
            {
                this.productDecompAlg = productDecompAlg;
            }
        }

        /// <summary>
        /// Obtém a factorização do módulo do número.
        /// </summary>
        /// <param name="data">O número a ser factorizado.</param>
        /// <returns>A factorização.</returns>
        public Dictionary<int, int> Run(int data)
        {
            if (data == 0)
            {
                throw new ArgumentException("Zero has no factor.");
            }
            else if (data == 1)
            {
                throw new ArgumentException("Unity is a trivial factor.");
            }
            else if (data == -1)
            {
                throw new ArgumentException("Negative unity is a trivial factor.");
            }
            else
            {
                var result = new Dictionary<int, int>();
                var innerData = Math.Abs(data);
                var alreadyComputed = new Dictionary<int, Tuple<int, int>>();
                var factorsStack = new Stack<int>();
                factorsStack.Push(innerData);
                while (factorsStack.Count > 0)
                {
                    var top = factorsStack.Pop();
                    var topFactor = default(Tuple<int, int>);
                    if (!alreadyComputed.TryGetValue(top, out topFactor))
                    {
                        topFactor = this.productDecompAlg.Run(top);
                        alreadyComputed.Add(top, topFactor);
                    }

                    if (topFactor.Item1 == 1)
                    {
                        var deg = 0;
                        if (result.TryGetValue(topFactor.Item2, out deg))
                        {
                            result[topFactor.Item2] = ++deg;
                        }
                        else
                        {
                            result.Add(topFactor.Item2, 1);
                        }
                    }
                    else if (topFactor.Item2 == 1)
                    {
                        var deg = 0;
                        if (result.TryGetValue(topFactor.Item1, out deg))
                        {
                            result[topFactor.Item1] = ++deg;
                        }
                        else
                        {
                            result.Add(topFactor.Item1, 1);
                        }
                    }
                    else
                    {
                        factorsStack.Push(topFactor.Item1);
                        factorsStack.Push(topFactor.Item2);
                    }
                }

                return result;
            }
        }
    }
}
