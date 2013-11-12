namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Numerics;
    using System.Text;
    using Mathematics;
    using Mathematics.AlgebraicStructures.Polynomial;
    using Mathematics.MathematicsInterpreter;
    using Utilities.Collections;
    using Utilities.Parsers;
    using System.Linq.Expressions;
    using Utilities;
    using OdmpProblem;

    class Program
    {
        delegate void Temp();

        static void Main(string[] args)
        {
            Test10();
            //using (var streamWriter = new StreamWriter("temp.txt"))
            //{
            //    streamWriter.Write(2);
            //    var primes = new[] { 3, 5, 7, 11, 13, 17, 19 };
            //    var product = 1;
            //    for (int i = 0; i < primes.Length; ++i)
            //    {
            //        product *= primes[i];
            //    }

            //    var previous = 23;
            //    streamWriter.Write(",{0}", previous - 1);
            //    var current = previous + 2;
            //    while (current < product)
            //    {
            //        var divide = false;
            //        for (int i = 0; i < primes.Length; ++i)
            //        {
            //            if (current % primes[i] == 0)
            //            {
            //                divide = true;
            //                i = primes.Length;
            //            }
            //        }

            //        if (!divide)
            //        {
            //            streamWriter.Write(",{0}", current - previous);
            //            previous = current;
            //        }

            //        current += 2;
            //    }

            //    // 3*5*7*...+-2 está na mesma situação
            //    streamWriter.Write(",{0}", 4);
            //}

            var n = 500000000;
            var stopWatch = new Stopwatch();
            //var count = 0;
            //stopWatch.Start();
            //var primeNumbersIterator = new BigIntPrimeNumbsIterator(n);
            //foreach (var prime in primeNumbersIterator)
            //{
            //    ++count;
            //}

            //stopWatch.Stop();
            //Console.WriteLine(stopWatch.Elapsed);

            //count = 0;
            //stopWatch.Reset();
            //stopWatch.Start();
            //primeNumbersIterator = new BigIntPrimeNumbsIterator(n);
            //foreach (var prime in primeNumbersIterator)
            //{
            //    ++count;
            //}

            //stopWatch.Stop();
            //Console.WriteLine(stopWatch.Elapsed);

            var count1 = 0;
            stopWatch.Reset();
            stopWatch.Start();
            var primeNumbsIter = new PrimeNumbersIterator(n);
            foreach (var prime in primeNumbsIter)
            {
                ++count1;
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed);

            count1 = 0;
            stopWatch.Reset();
            stopWatch.Start();
            primeNumbsIter = new PrimeNumbersIterator(n);
            foreach (var prime in primeNumbsIter)
            {
                ++count1;
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed);
            Console.ReadLine();
        }

        static void Example()
        {
            //return 0;
        }

        static void RunObjectTester()
        {
            var tester = new ObjectTester();
            tester.Run(Console.In, Console.Out);
        }

        /// <summary>
        /// Testes ao método da condensação.
        /// </summary>
        public static void Test18()
        {
            var doubleArrayMatrixReader = new DoubleArrayMatrixReader();
            var independentMatrix = doubleArrayMatrixReader.ReadArray(3, 4, "[[1,0,0],[0,0,0],[0,3,3],[2,0,1]]");
            var dependentMatrix = doubleArrayMatrixReader.ReadArray(3, 1, "[[1,3,3]]");

            var linearSystemAlg = new DenseCondensationLinSysAlgorithm<double>(
                new DoubleField());
            var result = linearSystemAlg.Run(independentMatrix, dependentMatrix);
        }

        /// <summary>
        /// Testes ao algoritmo de decomposição.
        /// </summary>
        public static void Test17()
        {
            var costs = new List<List<int>>();
            costs.Add(new List<int>() { 12, 9, 7, 6, 4 });
            costs.Add(new List<int>() { 12, 11, 8, 7, 5 });
            costs.Add(new List<int>() { 13, 10, 9, 6, 3 });
            costs.Add(new List<int>() { 12, 8, 6, 4, 2 });

            var integerDomain = new IntegerDomain();
            var numberTdecomposition = new IntegerMinWeightTdecomposition<int>(
                Comparer<int>.Default,
                integerDomain);

            var result = numberTdecomposition.Run(18, costs);
            Console.WriteLine("Cost: {0}", result.Cost);
            Console.WriteLine(PrintVector(result.Medians));;

            var upperCosts = new List<List<int>>();
            upperCosts.Add(new List<int>() { 12, 9, 7, 6, 4 });
            upperCosts.Add(new List<int>() { 12, 11, 8, 7, 5 });
            upperCosts.Add(new List<int>() { 13, 10, 9, 6, 3 });
            upperCosts.Add(new List<int>() { 12, 8, 6, 4, 2 });

            var boundsAlg = new ComponentBoundsAlgorithm<int>(
                numberTdecomposition,
                Comparer<int>.Default,
                integerDomain);
            var boundsResult = boundsAlg.Run(13, costs, upperCosts);
        }

        /// <summary>
        /// Teste ao método do simplex habitual (implementação paralela).
        /// </summary>
        public static void Test16()
        {
            var inputConstraintsMatrix = "[[1,0,3],[0,2,2]]";
            var inputConstraintsVector = "[[4,12,18]]";
            var inputObjectiveFunc = "[[-3],[-2]]";
            var cost = 0.0;
            var nonBasicVariables = new[] { 0, 1 };
            var basicVariables = new[] { 2, 3, 4 };

            // Responsável pela leitura de apenas um elemento, incluindo o sinal
            var doubleSimpleParser = new DoubleParser<string>();

            // O sinal negativo é lido automaticamente uma vez que não é necessária a leitura de uma expressão
            var stringSymbolReader = new StringSymbolReader(new StringReader(inputConstraintsMatrix), true);

            var matrixFactory = new ArrayMatrixFactory<double>();
            var arrayMatrixReader = new ConfigMatrixReader<double, string, string, CharSymbolReader<string>>(
                3,
                2,
                matrixFactory);
            arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayMatrixReader.AddBlanckSymbolType("blancks");
            arrayMatrixReader.SeparatorSymbType = "comma";

            var constraintsMatrix = default(IMatrix<double>);
            if (arrayMatrixReader.TryParseMatrix(stringSymbolReader, doubleSimpleParser, out constraintsMatrix))
            {
                stringSymbolReader = new StringSymbolReader(new StringReader(inputConstraintsVector), true);

                arrayMatrixReader = new ConfigMatrixReader<double, string, string, CharSymbolReader<string>>(
                3,
                1,
                matrixFactory);
                arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
                arrayMatrixReader.AddBlanckSymbolType("blancks");
                arrayMatrixReader.SeparatorSymbType = "comma";

                var constraintsVector = default(IMatrix<double>);
                if (arrayMatrixReader.TryParseMatrix(stringSymbolReader, doubleSimpleParser, out constraintsVector))
                {
                    stringSymbolReader = new StringSymbolReader(new StringReader(inputObjectiveFunc), true);

                    arrayMatrixReader = new ConfigMatrixReader<double, string, string, CharSymbolReader<string>>(
                    1,
                    2,
                    matrixFactory);
                    arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
                    arrayMatrixReader.AddBlanckSymbolType("blancks");
                    arrayMatrixReader.SeparatorSymbType = "comma";

                    var objectiveFunction = default(IMatrix<double>);
                    if (arrayMatrixReader.TryParseMatrix(stringSymbolReader, doubleSimpleParser, out objectiveFunction))
                    {
                        var doubleField = new DoubleField();
                        var simplexInput = new SimplexInput<double, double>(
                            basicVariables,
                            nonBasicVariables,
                            objectiveFunction,
                            cost,
                            constraintsMatrix,
                            constraintsVector);

                        var simplexAlg = new SimplexAlgorithm<double>(
                            Comparer<double>.Default,
                            doubleField);

                        var simplexOut = simplexAlg.Run(simplexInput);
                        Console.WriteLine("Cost: {0}", simplexOut.Cost);
                    }
                    else
                    {
                        Console.WriteLine("Can't parse objective function.");
                    }
                }
                else
                {
                    Console.WriteLine("Can't parse constraints vector.");
                }
            }
            else
            {
                Console.WriteLine("Can't parse constraints matrix.");
            }
        }

        /// <summary>
        /// Teste aos méotodos relacionados com a resolução de sistemas de equações.
        /// </summary>
        public static void Test15()
        {
            // Note-se que a leitura é realizada coluna a coluna.
            var inputMatrix = "[[1,2,1],[2,-1,-1],[1,-1,3]]";

            // Vector coluna.
            var inputVector = "[[1,2,3]]";

            var reader = new StringReader(inputMatrix);
            var stringsymbolReader = new StringSymbolReader(reader, false);
            var integerDomain = new BigIntegerDomain();
            var integerParser = new BigIntegerParser<string>();
            var fractionField = new FractionField<BigInteger, BigIntegerDomain>(integerDomain);
            var fractionParser = new FractionExpressionParser<BigInteger, BigIntegerDomain>(
                integerParser,
                fractionField);

            var arrayMatrixFactory = new ArrayMatrixFactory<Fraction<BigInteger, BigIntegerDomain>>();
            var arrayMatrixReader = new ConfigMatrixReader<Fraction<BigInteger, BigIntegerDomain>, string, string, CharSymbolReader<string>>(
                3,
                3,
                arrayMatrixFactory);
            arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayMatrixReader.AddBlanckSymbolType("blancks");
            arrayMatrixReader.SeparatorSymbType = "comma";

            var matrix = default(IMatrix<Fraction<BigInteger, BigIntegerDomain>>);
            if (arrayMatrixReader.TryParseMatrix(stringsymbolReader, fractionParser, out matrix))
            {
                arrayMatrixReader = new ConfigMatrixReader<Fraction<BigInteger, BigIntegerDomain>, string, string, CharSymbolReader<string>>(
                3,
                1,
                arrayMatrixFactory);
                arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
                arrayMatrixReader.AddBlanckSymbolType("blancks");
                arrayMatrixReader.SeparatorSymbType = "comma";

                var independentVector = default(IMatrix<Fraction<BigInteger, BigIntegerDomain>>);
                reader = new StringReader(inputVector);
                stringsymbolReader = new StringSymbolReader(reader, false);
                if (arrayMatrixReader.TryParseMatrix(stringsymbolReader, fractionParser, out independentVector))
                {
                    var systemSolver = new SequentialLanczosAlgorithm<Fraction<BigInteger, BigIntegerDomain>, FractionField<BigInteger, BigIntegerDomain>>(
                        arrayMatrixFactory,
                        fractionField);
                    var result = systemSolver.Run(matrix, independentVector);
                }
            }
        }

        /// <summary>
        /// Teste à leitura de expressões que envolvem conjuntos.
        /// </summary>
        public static void Test14()
        {
            var input = "  {1 , 5,  2,3}  intersection ({3,2} union {1,5})";
            var reader = new StringReader(input);
            var symbolReader = new SetSymbolReader(reader);
            var hashSetExpressionParser = new SetExpressionParser<int>(new IntegerParser<ESymbolSetType>());
            var parsed = default(HashSet<int>);
            if (hashSetExpressionParser.TryParse(symbolReader, out parsed))
            {
                Console.WriteLine("Elements in set are:");
                foreach (var element in parsed)
                {
                    Console.WriteLine(element);
                }
            }
            else
            {
                Console.WriteLine("Can't parse expression.");
            }
        }

        /// <summary>
        /// Teste aos algoritmos relacionados com números primos.
        /// </summary>
        public static void Test13()
        {
            var quadraticSieve = new QuadraticFieldSieve();
            var temp = quadraticSieve.Run(13459, 100, 50);

            var aksPrimalityTest = new AksPrimalityTest();
            var n = 99991;
            if (aksPrimalityTest.Run(n))
            {
                Console.WriteLine("{0} is prime", n);
            }
            else
            {
                Console.WriteLine("{0} isn't prime", n);
            }

            var eulerFunction = new EulerTotFuncAlg();
            Console.WriteLine(eulerFunction.Run(1937));

            var pollardRhoAlg = new PollardRhoAlgorithm();
            //n = 38;
            var pollardResult = pollardRhoAlg.Run(n);
            var pollardBlockedResult = pollardRhoAlg.Run(n, 10);
            Console.WriteLine("[{0}, {1}]", pollardResult.Item1, pollardResult.Item2);
            Console.WriteLine("[{0}, {1}]", pollardBlockedResult.Item1, pollardBlockedResult.Item2);

            Console.WriteLine(MathFunctions.Power(2, 6, new IntegerDomain()));
            var legendreJacobiAlg = new LegendreJacobiSymbolAlgorithm();
            Console.WriteLine(legendreJacobiAlg.Run(12345, 331));
            Console.WriteLine(legendreJacobiAlg.Run(13, 44));

            var resSol = new ResSolAlgorithm();
            Console.WriteLine(PrintVector(resSol.Run(10, 13)));
            Console.WriteLine(PrintVector(resSol.Run(17, 47)));

            var perfectPowerAlgotithm = new PerfectPowerTestAlgorithm();
            for (int i = 0; i <= 10; ++i)
            {
                if (perfectPowerAlgotithm.Run(i))
                {
                    Console.WriteLine(i);
                }
            }

            var primeNumberEnumerator = new PrimeNumbersIterator(100);
            foreach (var primeNumber in primeNumberEnumerator)
            {
                Console.WriteLine(primeNumber);
            }

            var integerDomain = new IntegerDomain();
            var lagrangeAlg = new LagrangeAlgorithm<int>(integerDomain);
            var firstValue = 51;
            var secondValue = 192;
            var result = lagrangeAlg.Run(firstValue, secondValue);
            Console.WriteLine("First: {0}", result.FirstItem);
            Console.WriteLine("Second: {0}", result.SecondItem);
            Console.WriteLine("First Bezout factor: {0}", result.FirstFactor);
            Console.WriteLine("Second Bezout factor: {0}", result.SecondFactor);
            Console.WriteLine("GCD: {0}", result.GreatestCommonDivisor);
            Console.WriteLine("First cofactor: {0}", result.FirstCofactor);
            Console.WriteLine("Second cofactor: {0}", result.SecondCofactor);

            var naiveFactorizationAlgorithm = new NaiveIntegerFactorizationAlgorithm();
            n = 35349384;
            Console.WriteLine("The factors of {0} are:", n);
            var factorsResult = naiveFactorizationAlgorithm.Run(n);
            foreach (var factor in factorsResult)
            {
                Console.WriteLine("{0}^{1}", factor.Key, factor.Value);
            }
        }

        /// <summary>
        /// Factorização livre de quadrados e domínios polinomiais.
        /// </summary>
        public static void Test12()
        {
            var firstInput = "    x^3-1/3*x^2+ - -x/5-1/2";
            var secondInput = "x^2-x/2+1";
            var thirdInput = "(x^2+3*x+2)*(x^2-4*x+3)^3";

            var reader = new StringReader(firstInput);
            var stringSymbolReader = new StringSymbolReader(reader, false);
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int, IntegerDomain>(integerDomain);
            var integerParser = new IntegerParser<string>();
            var fractionParser = new ElementFractionParser<int, IntegerDomain>(integerParser, integerDomain);
            var polynomialParser = new UnivariatePolynomialReader<Fraction<int, IntegerDomain>,
                CharSymbolReader<string>>(
                "x",
                fractionParser,
                fractionField);

            var fractionConversion = new ElementFractionConversion<int, IntegerDomain>(integerDomain);
            var firstPol = default(UnivariatePolynomialNormalForm<Fraction<int, IntegerDomain>>);
            if (polynomialParser.TryParsePolynomial(stringSymbolReader, fractionConversion, out firstPol))
            {
                reader = new StringReader(secondInput);
                stringSymbolReader = new StringSymbolReader(reader, false);
                var secondPol = default(UnivariatePolynomialNormalForm<Fraction<int, IntegerDomain>>);
                if (polynomialParser.TryParsePolynomial(stringSymbolReader, fractionConversion, out secondPol))
                {
                    var polynomialEuclideanDomain = new UnivarPolynomEuclideanDomain<Fraction<int, IntegerDomain>>(
                        "x",
                        fractionField);

                    var result = polynomialEuclideanDomain.GetQuotientAndRemainder(firstPol, secondPol);
                    Console.WriteLine("Quotient: {0}", result.Quotient);
                    Console.WriteLine("Remainder: {0}", result.Remainder);
                }
                else
                {
                    Console.WriteLine("Can't parse the second polynomial.");
                }
            }
            else
            {
                Console.WriteLine("Can't parse the first polynomial.");
            }

            reader = new StringReader(thirdInput);
            stringSymbolReader = new StringSymbolReader(reader, false);
            var bigIntegerDomain = new BigIntegerDomain();
            var bigIntegerParser = new BigIntegerParser<string>();
            var otherFractionParser = new ElementFractionParser<BigInteger, BigIntegerDomain>(bigIntegerParser, bigIntegerDomain);
            var otherFractionField = new FractionField<BigInteger, BigIntegerDomain>(bigIntegerDomain);
            var otherPolParser = new UnivariatePolynomialReader<
                Fraction<BigInteger, BigIntegerDomain>,
                CharSymbolReader<string>>(
                "x",
                otherFractionParser,
                otherFractionField);
            var otherFractionConversion = new BigIntegerFractionToIntConversion();
            var thirdPol = default(UnivariatePolynomialNormalForm<Fraction<BigInteger, BigIntegerDomain>>);
            if (otherPolParser.TryParsePolynomial(stringSymbolReader, otherFractionConversion, out thirdPol))
            {
                var univarSquareFreeAlg = new UnivarSquareFreeDecomposition<Fraction<BigInteger, BigIntegerDomain>>();
                var result = univarSquareFreeAlg.Run(thirdPol, otherFractionField);
                Console.WriteLine("The squarefree factors are:");
                foreach (var factor in result)
                {
                    Console.WriteLine("Factor: {0}; Degree: {1}", factor.Value, factor.Key);
                }
            }
            else
            {
                Console.WriteLine("Can't parse the third polynomial.");
            }
        }

        /// <summary>
        /// Testes os vários métodos de cálculo do polinómio característico.
        /// </summary>
        public static void Test11()
        {
            var input = "[[1,-1,2], [3,4,5], [2,1,1]]";

            var reader = new StringReader(input);
            var stringsymbolReader = new StringSymbolReader(reader, true);
            var integerParser = new IntegerParser<string>();

            var arrayMatrixFactory = new ArrayMatrixFactory<int>();
            var arrayMatrixReader = new ConfigMatrixReader<int, string, string, CharSymbolReader<string>>(3, 3, arrayMatrixFactory);
            arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayMatrixReader.AddBlanckSymbolType("blancks");
            arrayMatrixReader.SeparatorSymbType = "comma";

            var matrix = default(IMatrix<int>);
            var errors = new List<string>();
            if (arrayMatrixReader.TryParseMatrix(stringsymbolReader, integerParser, errors, out matrix))
            {
                var integerDomain = new IntegerDomain();
                var divFreeCharPol = new FastDivisionFreeCharPolynomCalculator<int>(
                    "lambda", 
                    integerDomain);
                var computedCharPol = divFreeCharPol.Run(matrix);
                Console.WriteLine("O determinante usando permutações vale: {0}.", computedCharPol);
            }
        }

        /// <summary>
        /// Testes à factorização de polinómios.
        /// </summary>
        public static void Test10()
        {

            var integerDomain = new IntegerDomain();
            var integerModularField = new ModularIntegerField(5);
            var integerParser = new IntegerParser<string>();
            var fractionParser = new ElementFractionParser<int, IntegerDomain>(integerParser, integerDomain);
            var fractionField = new FractionField<int, IntegerDomain>(integerDomain);

            // Soma nas n-potências das raízes de um polinómio
            var polInput = "x^6+4*x^4-3*x^3-x";
            var polInputReader = new StringReader(polInput);
            var polSymbolReader = new StringSymbolReader(polInputReader, false);
            var polParser = new UnivariatePolynomialReader<Fraction<int, IntegerDomain>, CharSymbolReader<string>>(
                "x",
                fractionParser,
                fractionField);

            var pol = default(UnivariatePolynomialNormalForm<Fraction<int, IntegerDomain>>);
            if (polParser.TryParsePolynomial(
                polSymbolReader,
                new ElementFractionConversion<int, IntegerDomain>(integerDomain),
                out pol))
            {
                var powerRootsSums = pol.GetRootPowerSums(fractionField);
                for (int i = 0; i < powerRootsSums.GetLength(0); ++i)
                {
                    Console.Write("{0} ", powerRootsSums[i,0]);
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Invalid polynomial: {0}.", polInput);
            }

            var firstInput = "x^3+10*x^2-432*x+5040";
            var secondInput = "x";
            var thirdInput = "x^2-2";

            var polynomialParser = new UnivariatePolynomialReader<int, CharSymbolReader<string>>(
                "x",
                integerParser,
                integerDomain);
            var conversion = new ElementToElementConversion<int>();

            var reader = new StringReader(firstInput);
            var stringSymbolReader = new StringSymbolReader(reader, false);
            var firstPol = default(UnivariatePolynomialNormalForm<int>);
            if (polynomialParser.TryParsePolynomial(
                stringSymbolReader,
                conversion,
                out firstPol))
            {
                reader = new StringReader(secondInput);
                stringSymbolReader = new StringSymbolReader(reader, false);
                var secondPol = default(UnivariatePolynomialNormalForm<int>);
                if (polynomialParser.TryParsePolynomial(
                    stringSymbolReader,
                    conversion,
                    out secondPol))
                {
                    reader = new StringReader(thirdInput);
                    stringSymbolReader = new StringSymbolReader(reader, false);
                    var thirdPol = default(UnivariatePolynomialNormalForm<int>);
                    if (polynomialParser.TryParsePolynomial(
                        stringSymbolReader,
                        conversion,
                        out thirdPol))
                    {
                        var lifting = new LinearLiftAlgorithm();
                        var status = new LinearLiftingStatus<int>(
                            firstPol,
                            secondPol,
                            thirdPol,
                            integerModularField,
                            integerDomain);
                        var liftingResult = lifting.Run(status, 10);
                        var solution = status.GetSolution();
                        //status.ep
                    }
                }
            }

            var input = "x^8 + x^7 + x^4 + x^3 + x + 1";
            integerModularField.Module = 3;
            reader = new StringReader(input);
            stringSymbolReader = new StringSymbolReader(reader, false);

            var parsedPol = default(UnivariatePolynomialNormalForm<int>);
            if (polynomialParser.TryParsePolynomial(
                stringSymbolReader,
                conversion, 
                out parsedPol))
            {
                var finiteFieldFactorization = new FiniteFieldPolFactorizationAlgorithm(
                    new UnivarSquareFreeDecomposition<Fraction<int, IntegerDomain>>(),
                    new DenseCondensationLinSysAlgorithm<int>(integerModularField));

                var factored = finiteFieldFactorization.Run(parsedPol, integerModularField);

                foreach (var factorKvp in factored)
                {
                    Console.WriteLine("{0} => {1}", factorKvp.Key, factorKvp.Value);
                }
            }
        }

        /// <summary>
        /// Teste às matrizes.
        /// </summary>
        public static void Test9()
        {
            var sparseDictionaryMatrix = new SparseDictionaryMatrix<int>(0);
            Console.WriteLine(
                "Linhas: {0}; Colunas: {1}", 
                sparseDictionaryMatrix.GetLength(0), 
                sparseDictionaryMatrix.GetLength(1));

            Console.WriteLine("[0,0] = {0}", sparseDictionaryMatrix[0, 0]);

            sparseDictionaryMatrix[2, 3] = 0;
            sparseDictionaryMatrix[4, 1] = 5;

            Console.WriteLine(
                "Linhas: {0}; Colunas: {1}", 
                sparseDictionaryMatrix.GetLength(0), 
                sparseDictionaryMatrix.GetLength(1));
            Console.WriteLine("[4,1] = {0}", sparseDictionaryMatrix[4, 1]);

            sparseDictionaryMatrix.SwapLines(4, 1);
            Console.WriteLine(
                "Linhas: {0}; Colunas: {1}", 
                sparseDictionaryMatrix.GetLength(0), 
                sparseDictionaryMatrix.GetLength(1));

            Console.WriteLine("[1,1] = {0}", sparseDictionaryMatrix[1, 1]);

            sparseDictionaryMatrix.SwapColumns(3, 5);
            Console.WriteLine(
                "Linhas: {0}; Colunas: {1}", 
                sparseDictionaryMatrix.GetLength(0), 
                sparseDictionaryMatrix.GetLength(1));
        }

        /// <summary>
        /// Cálculo de determinantes e leitura de matrizes.
        /// </summary>
        static void Test8()
        {
            var input = "[[1-x,2],[4, 3-x]]";
            var inputReader = new StringSymbolReader(new StringReader(input), false);
            var integerParser = new IntegerParser<string>();
            var integerDomain = new IntegerDomain();
            var conversion = new ElementToElementConversion<int>();
            var univariatePolParser = new UnivarPolNormalFormParser<int>(
                "x",
                conversion,
                integerParser,
                integerDomain);

            var arrayMatrixFactory = new ArrayMatrixFactory<UnivariatePolynomialNormalForm<int>>();
            var arrayReader = new ConfigMatrixReader<UnivariatePolynomialNormalForm<int>, string, string, CharSymbolReader<string>>(
                2,
                2,
                arrayMatrixFactory);
            arrayReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayReader.AddBlanckSymbolType("blancks");
            arrayReader.SeparatorSymbType = "comma";

            var readed = default(IMatrix<UnivariatePolynomialNormalForm<int>>);
            if (arrayReader.TryParseMatrix(inputReader, univariatePolParser, out readed))
            {
                var polynomialRing = new UnivarPolynomRing<int>("x", integerDomain);
                var permutationDeterminant = new PermutationDeterminantCalculator<UnivariatePolynomialNormalForm<int>>(
                    polynomialRing);

                var computedDeterminant = permutationDeterminant.Run(readed);

                Console.WriteLine("O determinante usando permutações vale: {0}.", computedDeterminant);

                var expansionDeterminant = new ExpansionDeterminantCalculator<UnivariatePolynomialNormalForm<int>>(
                    polynomialRing);

                computedDeterminant = expansionDeterminant.Run(readed);

                Console.WriteLine("O determinante usando expansão vale: {0}.", computedDeterminant);

                Console.WriteLine(readed);
            }
            else
            {
                Console.WriteLine("Errors found.");
            }
        }

        /// <summary>
        /// Testes aos polinómios.
        /// </summary>
        static void Test7()
        {
            var input = "x+y^2*(z-1)^3-x";
            var integerParser = new IntegerParser<string>();

            var inputReader = new StringSymbolReader(new StringReader(input), false);
            var polynomialParser = new PolynomialReader<int, CharSymbolReader<string>>(
                integerParser, 
                new IntegerDomain());
            var readed = default(Polynomial<int>);
            var errors = new List<string>();
            var elementConversion = new ElementToElementConversion<int>();
            if (polynomialParser.TryParsePolynomial(inputReader, elementConversion, errors, out readed))
            {
                Console.WriteLine(readed);
            }
            else
            {
                foreach (var message in errors)
                {
                    Console.WriteLine("Errors parsing polynomial:");
                    Console.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// Teste às matrizes habituais.
        /// </summary>
        static void Test6()
        {
            // Note-se que neste caso estamos na presença de um conjunto de vectores coluna.
            var input = "[[1,-1,2], [3,4,5], [2,1,1]]";

            var reader = new StringReader(input);
            var stringsymbolReader = new StringSymbolReader(reader, true);
            var integerParser = new IntegerParser<string>();

            var arrayMatrixFactory = new ArrayMatrixFactory<int>();
            var arrayMatrixReader = new ConfigMatrixReader<int, string, string, CharSymbolReader<string>>(
                3, 
                3, 
                arrayMatrixFactory);
            arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayMatrixReader.AddBlanckSymbolType("blancks");
            arrayMatrixReader.SeparatorSymbType = "comma";

            var matrix = default(IMatrix<int>);
            var errors = new List<string>();
            if (arrayMatrixReader.TryParseMatrix(stringsymbolReader, integerParser, errors, out matrix))
            {
                Console.WriteLine(matrix);

                var integerDomain = new IntegerDomain();
                var permutationDeterminant = new PermutationDeterminantCalculator<int>(integerDomain);
                var computedDeterminant = permutationDeterminant.Run(matrix);
                Console.WriteLine("O determinante usando permutações vale: {0}.", computedDeterminant);

                var expansionDeterminant = new ExpansionDeterminantCalculator<int>(integerDomain);
                computedDeterminant = expansionDeterminant.Run(matrix);
                Console.WriteLine("O determinante usando expansão vale: {0}.", computedDeterminant);

                var condensationDeterminant = new CondensationDeterminantCalculator<int>(integerDomain);
                computedDeterminant = condensationDeterminant.Run(matrix);
                Console.WriteLine("O determinante usando a condensação vale: {0}.", computedDeterminant);
            }
            else
            {
                foreach (var message in errors)
                {
                    Console.WriteLine("Errors parsing range:");
                    Console.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// Teste às matrizes multidimensionais.
        /// </summary>
        static void Test5()
        {
            var input = "[[1,2,3,4,5],[6,7,8,9,0]]";
            var reader = new StringReader(input);
            var stringsymbolReader = new StringSymbolReader(reader, true);
            var integerParser = new IntegerParser<string>();

            var rangeNoConfig = new RangeNoConfigReader<int, string, string, CharSymbolReader<string>>();
            rangeNoConfig.MapInternalDelimiters("left_bracket", "right_bracket");
            rangeNoConfig.AddBlanckSymbolType("blancks");
            rangeNoConfig.SeparatorSymbType = "comma";

            var multiDimensionalRangeReader = new MultiDimensionalRangeReader<int, string, string, CharSymbolReader<string>>(rangeNoConfig);
            var range = default(MultiDimensionalRange<int>);
            var errors = new List<string>();
            if (multiDimensionalRangeReader.TryParseRange(stringsymbolReader, integerParser, errors, out range))
            {
                var config = new int[][] { new int[] { 1, 1, 4, 3 }, new int[] { 0, 1, 0, 1 } };
                var subRange = range.GetSubMultiDimensionalRange(config);
                Console.WriteLine(subRange);
            }
            else
            {
                foreach (var message in errors)
                {
                    Console.WriteLine("Errors parsing range:");
                    Console.WriteLine(message);
                }
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Teste aos polinómios simétricos.
        /// </summary>
        static void Test4()
        {
            var dictionary = new Dictionary<int, int>();
            dictionary.Add(5, 2);
            dictionary.Add(0, 2);

            var varDictionary = new Dictionary<int, Tuple<bool, string, int>>();
            varDictionary.Add(1, Tuple.Create(true, "s[1]", 0));

            var symmetric = new SymmetricPolynomial<int>(
                new List<string>() { "x", "y", "z", "w" },
                dictionary,
                1,
                new IntegerDomain());

            var rep = symmetric.GetElementarySymmetricRepresentation(varDictionary, new IntegerDomain());
            var expanded = rep.GetExpanded(new IntegerDomain());
            Console.WriteLine(expanded);
        }

        /// <summary>
        /// Teste aos afectadores.
        /// </summary>
        static void Test3()
        {
            var permutaionBoxAffector = new PermutationBoxAffector(new[] { 2, 2, 2 }, 3);
            foreach (var item in permutaionBoxAffector)
            {
                Console.WriteLine(PrintVector(item));
            }

            var combinationBoxAffector = new CombinationBoxAffector(new[] { 2, 2, 2 }, 3);
            foreach (var item in combinationBoxAffector)
            {
                Console.WriteLine(PrintVector(item));
            }

            // Este código permite indicar que o primeiro elemento se pode repetir duas vezes, o segundo três e o terceiro três
            // var permutation = new PermutationAffector(3, 3, new int[] { 2, 3, 3 });
            var dictionary = new Dictionary<int, int>();
            dictionary.Add(2, 3);
            var structureAffector = new StructureAffector(new[] { new[] { 0, 1, 2 }, new[] { 0, 1, 2 }, new[] { 2, 3 } }, dictionary);
            foreach (var item in structureAffector)
            {
                Console.WriteLine(PrintVector(item));
            }

            var count = 0;
            var structureAffectations = new int[][] { new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 } };
            var affector = new StructureAffector(structureAffectations);
            foreach (var aff in affector)
            {
                ++count;
            }

            Console.WriteLine(count);

            var permutation = new PermutationAffector(4);
            count = 0;
            foreach (var perm in permutation)
            {
                ++count;
            }

            var combination = new CombinationAffector(6, 3);
            count = 0;
            foreach (var comb in combination)
            {
                ++count;
            }
        }

        /// <summary>
        /// Teste ao interpretador.
        /// </summary>
        static void Test2()
        {
            Console.WriteLine("Please insert expression to be evaluated:");
            var interpreter = new MathematicsInterpreter();
            var result = interpreter.Interpret(Console.In, Console.Out);
            Console.ReadKey();
        }

        /// <summary>
        /// Contém código arbitrário.
        /// </summary>
        static void Test1()
        {
            var graph = new EdgeListGraph<int, int>();
            graph.AddEdge(0, 1, 1);
            graph.AddEdge(0, 2, 3);
            graph.AddEdge(0, 3, 5);
            graph.AddEdge(1, 2, 1);
            graph.AddEdge(1, 3, 3);
            graph.AddEdge(2, 3, 7);
            graph.AddEdge(2, 4, 6);
            graph.AddEdge(3, 4, 7);

            graph.AddVertex(10);
            graph.AddVertex(11);

            var graphAlgs = graph.GetAlgorithmsProcessor();
            var result = graphAlgs.GetMinimumSpanningTree<double>(
                0, 
                e=>e.Value, 
                Comparer<double>.Default,
                new DoubleField());
        }

        static string PrintVector(IEnumerable<int> vectorToPrint)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            var vectorEnumerator = vectorToPrint.GetEnumerator();
            if (vectorEnumerator.MoveNext())
            {
                resultBuilder.Append(vectorEnumerator.Current);
                while (vectorEnumerator.MoveNext())
                {
                    resultBuilder.AppendFormat(", {0}", vectorEnumerator.Current);
                }
            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }
    }
}
