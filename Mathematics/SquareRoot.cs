namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma raiz quadrada na forma simplificada.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de elemento na raiz.</typeparam>
    public class SquareRoot<ObjectType>
    {
        /// <summary>
        /// A factorização no interior da raiz.
        /// </summary>
        private Dictionary<ObjectType, int> rootNumberFactorization;

        /// <summary>
        /// O valor no interior da raiz.
        /// </summary>
        private ObjectType rootNumber;

        /// <summary>
        /// A parte inteira da raíz.
        /// </summary>
        private ObjectType integerPart;

        /// <summary>
        /// O construtor responsável pela criaçõa de objectos no interior da classe.
        /// </summary>
        private SquareRoot()
        {
        }

        public SquareRoot(
            ObjectType integerPart,
            Dictionary<ObjectType, int> rootNumberFactorization,
            IMultipliable<ObjectType> multipliable)
        {
            this.SetupParameters(integerPart, rootNumberFactorization, multipliable);
            this.ComputeRootNumber(multipliable);
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
                this.ComputeRootNumber(multipliable);
            }
        }

        public SquareRoot(
            ObjectType integerPart,
            ObjectType rootNumber,
            IAlgorithm<ObjectType, Tuple<ObjectType, ObjectType>> factorizationAlg,
            IMultipliable<ObjectType> multipliable,
            IEqualityComparer<ObjectType> comparer)
        {
            this.SetupParameters(
                integerPart,
                rootNumber,
                factorizationAlg,
                multipliable,
                comparer);
            this.ComputeRootNumber(multipliable);
        }

        /// <summary>
        /// Obtém o valor correspondente à parte inteira.
        /// </summary>
        public ObjectType IntegerPart
        {
            get
            {
                return this.integerPart;
            }
        }

        /// <summary>
        /// Obtém o valor que se encontra no interior da raiz.
        /// </summary>
        public ObjectType RootNumber
        {
            get
            {
                return this.rootNumber;
            }
        }

        /// <summary>
        /// Obtém o produto da raiz actual com outra raiz.
        /// </summary>
        /// <param name="right">A outra raiz.</param>
        /// <param name="multipliable">O objecto responsável pelas multiplicações.</param>
        /// <returns>O resultado do produto.</returns>
        public SquareRoot<ObjectType> Multiply(
            SquareRoot<ObjectType> right,
            IMultipliable<ObjectType> multipliable)
        {
            var result = new SquareRoot<ObjectType>();
            result.integerPart = multipliable.Multiply(this.integerPart, right.integerPart);
            result.rootNumberFactorization = new Dictionary<ObjectType, int>(
                this.rootNumberFactorization.Comparer);
            var temporaryFactorization = new Dictionary<ObjectType, int>(this.rootNumberFactorization.Comparer);
            foreach (var kvp in right.rootNumberFactorization)
            {
                temporaryFactorization.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in this.rootNumberFactorization)
            {
                if (temporaryFactorization.ContainsKey(kvp.Key))
                {
                    result.integerPart = multipliable.Multiply(result.integerPart, kvp.Key);
                    temporaryFactorization.Remove(kvp.Key);
                }
                else
                {
                    result.rootNumberFactorization.Add(kvp.Key, 1);
                }
            }

            foreach (var kvp in temporaryFactorization)
            {
                result.rootNumberFactorization.Add(kvp.Key, kvp.Value);
            }

            return result;
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

        /// <summary>
        /// Establece os parâmetros internos com base nos argumentos externos.
        /// </summary>
        /// <param name="integerPart">A parte inteira da raiz.</param>
        /// <param name="rootNumber">O valor do radicando na fase anterior ao processamento.</param>
        /// <param name="factorizationAlg">O algoritmo responsável pela factorização do radicando.</param>
        /// <param name="multipliable"></param>
        private void SetupParameters(
            ObjectType integerPart,
            ObjectType rootNumber,
            IAlgorithm<ObjectType, Tuple<ObjectType, ObjectType>> factorizationAlg,
            IMultipliable<ObjectType> multipliable,
            IEqualityComparer<ObjectType> comparer)
        {
            if (integerPart == null)
            {
                throw new ArgumentNullException("integerPart");
            }
            else if (rootNumber == null)
            {
                throw new ArgumentNullException("rootNumber");
            }
            else if (factorizationAlg == null)
            {
                throw new ArgumentNullException("factorizationAlg");
            }
            else if (multipliable == null)
            {
                throw new ArgumentNullException("multipliable");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.integerPart = integerPart;
                this.rootNumber = rootNumber;
                this.rootNumberFactorization = new Dictionary<ObjectType, int>(
                    comparer);

                var alreadyComputed = new Dictionary<ObjectType, Tuple<ObjectType, ObjectType>>(
                    comparer
                    );
                var factorsStack = new Stack<ObjectType>();
                factorsStack.Push(rootNumber);
                while (factorsStack.Count > 0)
                {
                    var top = factorsStack.Pop();
                    var topFactor = default(Tuple<ObjectType, ObjectType>);
                    if (!alreadyComputed.TryGetValue(top, out topFactor))
                    {
                        topFactor = factorizationAlg.Run(top);
                        alreadyComputed.Add(top, topFactor);
                    }

                    if (multipliable.IsMultiplicativeUnity(topFactor.Item1))
                    {
                        var deg = 0;
                        if (this.rootNumberFactorization.TryGetValue(topFactor.Item2, out deg))
                        {
                            this.rootNumberFactorization.Remove(topFactor.Item2);
                            this.integerPart = multipliable.Multiply(
                                this.integerPart,
                                topFactor.Item2);
                        }
                        else
                        {
                            this.rootNumberFactorization.Add(topFactor.Item2, 1);
                        }
                    }
                    else if (multipliable.IsMultiplicativeUnity(topFactor.Item2))
                    {
                        var deg = 0;
                        if (this.rootNumberFactorization.TryGetValue(topFactor.Item1, out deg))
                        {
                            this.rootNumberFactorization.Remove(topFactor.Item1);
                            this.integerPart = multipliable.Multiply(
                                this.integerPart,
                                topFactor.Item1);
                        }
                        else
                        {
                            this.rootNumberFactorization.Add(topFactor.Item1, 1);
                        }
                    }
                    else
                    {
                        factorsStack.Push(topFactor.Item1);
                        factorsStack.Push(topFactor.Item2);
                    }
                }
            }
        }

        /// <summary>
        /// Determina o valor do radicando após o processamento.
        /// </summary>
        /// <param name="multipliable">O objecto responsável pelas multiplicações.</param>
        private void ComputeRootNumber(IMultipliable<ObjectType> multipliable)
        {
            this.rootNumber = multipliable.MultiplicativeUnity;
            foreach (var factKvp in rootNumberFactorization)
            {
                this.rootNumber = multipliable.Multiply(
                    this.rootNumber,
                    factKvp.Key);
            }
        }
    }
}
