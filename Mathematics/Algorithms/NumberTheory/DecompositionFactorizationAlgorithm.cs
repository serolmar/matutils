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
    public class DecompositionFactorizationAlgorithm<NumberType, DegreeType> : IAlgorithm<NumberType, Dictionary<NumberType, DegreeType>>
    {
        private IAlgorithm<NumberType, Tuple<NumberType, NumberType>> productDecompAlg;

        /// <summary>
        /// O anel responsável pelas operações sobre os números.
        /// </summary>
        private IRing<NumberType> numberRing;

        /// <summary>
        /// O monóide responsável pelas operações sobre os graus.
        /// </summary>
        private IMonoid<DegreeType> degreeMonoid;

        /// <summary>
        /// Mantém o grau unitário.
        /// </summary>
        private DegreeType unitaryDegree;

        public DecompositionFactorizationAlgorithm(
            IAlgorithm<NumberType, Tuple<NumberType, NumberType>> productDecompAlg,
            DegreeType unitaryDegree,
            IMonoid<DegreeType> degreeMonoid,
            IRing<NumberType> numberRing)
        {
            if (numberRing == null)
            {
                throw new ArgumentNullException("numberRing");
            }
            else if (degreeMonoid == null)
            {
                throw new ArgumentNullException("degreeMonoid");
            }
            else if (unitaryDegree == null)
            {
                throw new ArgumentNullException("uniartyDegree");
            }
            else if (productDecompAlg == null)
            {
                throw new ArgumentNullException("productDecompAlg");
            }
            else
            {
                this.numberRing = numberRing;
                this.degreeMonoid = degreeMonoid;
                this.unitaryDegree = unitaryDegree;
                this.productDecompAlg = productDecompAlg;
            }
        }

        /// <summary>
        /// Obtém a factorização do módulo do número.
        /// </summary>
        /// <param name="data">O número a ser factorizado.</param>
        /// <returns>A factorização.</returns>
        public Dictionary<NumberType, DegreeType> Run(NumberType data)
        {
            if(data == null){
                throw new ArgumentNullException("data");
            }
            else if (this.numberRing.IsAdditiveUnity(data))
            {
                throw new ArgumentException("Zero has no factor.");
            }
            else if (this.numberRing.IsMultiplicativeUnity(data) || this.numberRing.IsMultiplicativeUnity(this.numberRing.AdditiveInverse(data)))
            {
                // Caso seja uma unidade
                var result = new Dictionary<NumberType, DegreeType>(this.numberRing);
                result.Add(data, this.unitaryDegree);
                return result;
            }
            else
            {
                var result = new Dictionary<NumberType, DegreeType>(this.numberRing);
                var alreadyComputed = new Dictionary<NumberType, Tuple<NumberType, NumberType>>();
                var factorsStack = new Stack<NumberType>();
                factorsStack.Push(data);
                while (factorsStack.Count > 0)
                {
                    var top = factorsStack.Pop();
                    var topFactor = default(Tuple<NumberType, NumberType>);
                    if (!alreadyComputed.TryGetValue(top, out topFactor))
                    {
                        topFactor = this.productDecompAlg.Run(top);
                        alreadyComputed.Add(top, topFactor);
                    }

                    if (this.numberRing.IsAdditiveUnity(topFactor.Item1))
                    {
                        var deg = this.degreeMonoid.AdditiveUnity;
                        if (result.TryGetValue(topFactor.Item2, out deg))
                        {
                            deg = this.degreeMonoid.Add(deg, this.unitaryDegree);
                            result[topFactor.Item2] = deg;
                        }
                        else
                        {
                            result.Add(topFactor.Item2, this.unitaryDegree);
                        }
                    }
                    else if (this.numberRing.IsMultiplicativeUnity(topFactor.Item2))
                    {
                        var deg = this.degreeMonoid.AdditiveUnity;
                        if (result.TryGetValue(topFactor.Item1, out deg))
                        {
                            deg = this.degreeMonoid.Add(deg, this.unitaryDegree);
                            result[topFactor.Item1] = deg;
                        }
                        else
                        {
                            result.Add(topFactor.Item1, this.unitaryDegree);
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
