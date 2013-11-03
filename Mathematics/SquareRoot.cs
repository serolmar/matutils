namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma raiz quadrada.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de elemento na raiz.</typeparam>
    public class SquareRoot<ObjectType>
    {
        /// <summary>
        /// A factorização no interior da raiz.
        /// </summary>
        private Dictionary<ObjectType, int> rootNumberFactorization;

        /// <summary>
        /// A parte inteira da raíz.
        /// </summary>
        private ObjectType integerPart;

        public SquareRoot(
            ObjectType integerPart,
            Dictionary<ObjectType, int> rootNumberFactorization,
            IMultipliable<ObjectType> multipliable)
        {
            this.SetupParameters(integerPart, rootNumberFactorization, multipliable);
        }

        public SquareRoot(
            ObjectType integerPart,
            ObjectType rootNumber,
            IAlgorithm<ObjectType, Dictionary<ObjectType, int>> factorizationAlg,
            IMultipliable<ObjectType> multipliable)
        {
            if (factorizationAlg == null)
            {
                throw new ArgumentNullException("factorizationAlg");
            }
            else
            {
                var factorization = factorizationAlg.Run(rootNumber);
                this.SetupParameters(integerPart, factorization, multipliable);
            }
        }

        /// <summary>
        /// Establece os parâmetros internos com base nos argumentos externos.
        /// </summary>
        /// <param name="integerPart">A parte inteira da raiz.</param>
        /// <param name="rootNumberFactorization">A factorização do radicando.</param>
        private void SetupParameters(
            ObjectType integerPart,
            Dictionary<ObjectType, int> rootNumberFactorization,
            IMultipliable<ObjectType> multipliable)
        {
            if (integerPart == null)
            {
                throw new ArgumentNullException("integerPart");
            }
            else if (rootNumberFactorization == null)
            {
                throw new ArgumentNullException("rootNumberFactorization");
            }
            else if (multipliable == null)
            {
                throw new ArgumentNullException("multipliable");
            }
            else
            {
                this.integerPart = integerPart;
                this.rootNumberFactorization = new Dictionary<ObjectType, int>(rootNumberFactorization.Comparer);
                foreach (var fact in rootNumberFactorization)
                {
                    var factDegree = fact.Value;
                    var factSquaredValue = factDegree / 2;
                    var factRem = factDegree % 2;
                    this.integerPart = multipliable.Multiply(
                        this.integerPart,
                        MathFunctions.Power(fact.Key, factSquaredValue, multipliable));
                    if (factRem == 1)
                    {
                        rootNumberFactorization.Add(fact.Key, 1);
                    }
                }
            }
        }
    }
}
