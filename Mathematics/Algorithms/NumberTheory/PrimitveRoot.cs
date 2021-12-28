using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Algoritmo para determinação da raíz primitiva de um número primo.
    /// </summary>
    /// <remarks>
    /// Não se tratando de um número primo, os resultados serão imprevisíveis.
    /// </remarks>
    /// <typeparam name="NumberType">O tipo de objectos que constituem o número inteiro.</typeparam>
    public class NaivePrimitveRoot<NumberType>
        : IAlgorithm<NumberType, NumberType>
    {
        /// <summary>
        /// A expressão que permite construir o corpo responsável pelas operações.
        /// </summary>
        private Func<NumberType, IModularField<NumberType>> moduleFieldFactory;

        /// <summary>
        /// O enumerador de números primos.
        /// </summary>
        IEnumerable<NumberType> primesEnumerable;

        /// <summary>
        /// Algoritmo responsável pelo cálculo da parte inteira da raiz quadrada.
        /// </summary>
        private IAlgorithm<NumberType, NumberType> integerSquareRootAlg;

        /// <summary>
        /// Objecto responsável pelas operações sobre inteiros.
        /// </summary>
        IIntegerNumber<NumberType> integerNumber;

        /// <summary>
        /// Instancia uma nova instância de objcetos do tipo <see cref="NaivePrimitveRoot{NumberType}"/>.
        /// </summary>
        /// <param name="moduleFieldFactory"></param>
        /// <param name="integerNumber"></param>
        /// <param name="primesEnumerable"></param>
        public NaivePrimitveRoot(
            Func<NumberType, IModularField<NumberType>> moduleFieldFactory,
            IIntegerNumber<NumberType> integerNumber,
            IAlgorithm<NumberType, NumberType> integerSquareRootAlg,
            IEnumerable<NumberType> primesEnumerable)
        {
            if (moduleFieldFactory == null)
            {
                throw new ArgumentNullException("moduleField");
            }
            else if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (integerSquareRootAlg == null)
            {
                throw new ArgumentNullException("integerSquareRootAlg");
            }
            else if (primesEnumerable == null)
            {
                throw new ArgumentNullException("primesEnumerable");
            }
            else
            {
                this.moduleFieldFactory = moduleFieldFactory;
                this.integerNumber = integerNumber;
                this.integerSquareRootAlg = integerSquareRootAlg;
                this.primesEnumerable = primesEnumerable;
            }
        }

        /// <summary>
        /// Determina a raiz primitiva de um número.
        /// </summary>
        /// <param name="module">O módulo do qual se pretende determinar a raiz primitiva.</param>
        /// <returns></returns>
        public NumberType Run(
            NumberType module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }
            else
            {
                var moduleField = this.moduleFieldFactory(module);
                var factorNumber = moduleField.Add(
                    module,
                    moduleField.AdditiveInverse(moduleField.MultiplicativeUnity));

                var primeEnumerator = this.primesEnumerable.GetEnumerator();
                var primitiveRoot = moduleField.MultiplicativeUnity;
                var sqrt = this.integerSquareRootAlg.Run(factorNumber);
                var sq = this.integerNumber.Multiply(sqrt, sqrt);
                if (this.integerNumber.Compare(sq, factorNumber) < 0)
                {
                    sqrt = this.integerNumber.Successor(sqrt);
                }

                var testFactors = new List<NumberType>();
                var keep = true;
                while (keep)
                {
                    if (primeEnumerator.MoveNext())
                    {
                        var currentPrime = primeEnumerator.Current;
                        if (this.integerNumber.Compare(currentPrime, sqrt) > 0)
                        {
                            keep = false;
                        }
                        else
                        {
                            var quoAndRem = this.integerNumber.GetQuotientAndRemainder(
                                factorNumber,
                                currentPrime);
                            if (this.integerNumber.IsAdditiveUnity(quoAndRem.Remainder))
                            {
                                testFactors.Add(quoAndRem.Quotient);
                            }
                        }

                    }
                    else
                    {
                        throw new MathematicsException(
                            "Prime number iterator has reached the end before primitive root was computed.");
                    }
                }

                // Neste ponto existem os factores de teste
                keep = true;
                var count = testFactors.Count;
                var currentTest = this.integerNumber.MultiplicativeUnity;
                while (keep)
                {
                    currentTest = this.integerNumber.Successor(currentTest);
                    var isPrimitiveRoot = true;
                    for(var i = 0;i < count; ++i)
                    {
                        var pow = testFactors[i];
                        var testPower = MathFunctions.Power(
                            currentTest,
                            pow,
                            moduleField,
                            this.integerNumber);
                        if(this.integerNumber.Equals(testPower, this.integerNumber.MultiplicativeUnity))
                        {
                            i = count;
                            isPrimitiveRoot = false;
                        }
                    }

                    if (isPrimitiveRoot)
                    {
                        primitiveRoot = currentTest;
                        keep = false;
                    }
                }

                //var keep = true;
                //while (keep)
                //{
                //    if (primeEnumerator.MoveNext())
                //    {
                //        var prime = primeEnumerator.Current;
                //        if (this.integerNumber.Compare(prime, sqrt) <= 0)
                //        {
                //            var quoAndRem = this.integerNumber.GetQuotientAndRemainder(
                //                factorNumber,
                //                prime);
                //            var rem = quoAndRem.Remainder;
                //            var quo = quoAndRem.Quotient;
                //            if (this.integerNumber.IsAdditiveUnity(rem))
                //            {
                //                quoAndRem = this.integerNumber.GetQuotientAndRemainder(
                //                    quo,
                //                    prime);
                //                rem = quoAndRem.Remainder;
                //                while (this.integerNumber.IsAdditiveUnity(rem))
                //                {
                //                    quo = quoAndRem.Quotient;
                //                    quoAndRem = integerNumber.GetQuotientAndRemainder(
                //                        quo,
                //                        prime);
                //                    rem = quoAndRem.Remainder;
                //                }

                //                var candidate = this.integerNumber.Successor(
                //                    this.integerNumber.MultiplicativeUnity);
                //                var power = MathFunctions.Power(
                //                    candidate,
                //                    quo,
                //                    moduleField,
                //                    this.integerNumber);
                //                while (this.integerNumber.IsMultiplicativeUnity(power))
                //                {
                //                    candidate = this.integerNumber.Successor(
                //                        candidate);
                //                    power = MathFunctions.Power(
                //                        candidate,
                //                        quoAndRem.Quotient,
                //                        moduleField,
                //                        this.integerNumber);
                //                }

                //                primitiveRoot = moduleField.Multiply(
                //                    primitiveRoot,
                //                    power);
                //            }
                //        }
                //        else
                //        {
                //            // Termina aqui
                //            keep = false;
                //        }
                //    }
                //    else
                //    {
                //        throw new MathematicsException(
                //            "Prime number iterator has reached the end before primitive root was computed.");
                //    }
                //}

                return primitiveRoot;
            }
        }
    }
}
