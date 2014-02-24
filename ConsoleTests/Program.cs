namespace ConsoleTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Numerics;
    using System.Text;
    using Mathematics;
    using Mathematics.AlgebraicStructures.Polynomial;
    using Mathematics.MathematicsInterpreter;
    using Utilities.Collections;
    using Utilities;
    using System.Linq.Expressions;
    using OdmpProblem;

    class Program
    {
        delegate void Temp();

        static void Main(string[] args)
        {
            Test20();
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
        /// Testes a alguns algoritmos de decisão.
        /// </summary>
        public static void Test21()
        {
            var integerDomain = new IntegerDomain();
            var decimalField = new DecimalField();
            var vectorFactory = new ArrayVectorFactory<decimal>();
            var decimalComparer = Comparer<decimal>.Default;
            var nearest = new DecimalNearestInteger();

            var scalarProd = new OrthoVectorScalarProduct<decimal>(
                        decimalComparer,
                        decimalField);

            var integerDecimalConverter = new IntegerDecimalConverter();

            //var lllReductionAlg = new LLLBasisReductionAlgorithm<IVector<decimal>, decimal, int>(
            //    new VectorSpace<decimal>(3, vectorFactory, decimalField),
            //    scalarProd,
            //    nearest,
            //    Comparer<decimal>.Default);

            //var vectorSet = new IVector<decimal>[3];
            //vectorSet[0] = new ArrayVector<decimal>(new decimal[] { 1, 1, 1 });
            //vectorSet[1] = new ArrayVector<decimal>(new decimal[] { -1, 0, 2 });
            //vectorSet[2] = new ArrayVector<decimal>(new decimal[] { 3, 5, 6 });

            //var reduced = lllReductionAlg.Run(vectorSet, 3M / 4);

            //var dim = 4;
            //var vectorSet = new IVector<decimal>[dim];
            //var lllReductionAlg = new LLLBasisReductionAlgorithm<IVector<decimal>, decimal, int>(
            //    new VectorSpace<decimal>(dim, vectorFactory, decimalField),
            //    scalarProd,
            //    nearest,
            //    Comparer<decimal>.Default);

            //vectorSet[0] = new ArrayVector<decimal>(new decimal[] { 1, 1, 7, 2 });
            //vectorSet[1] = new ArrayVector<decimal>(new decimal[] { 9, 8, 4, 6 });
            //vectorSet[2] = new ArrayVector<decimal>(new decimal[] { 1, 8, 5, 7 });
            //vectorSet[3] = new ArrayVector<decimal>(new decimal[] { 2, 3, 1, 1 });

            //var reduced = lllReductionAlg.Run(vectorSet, 3M / 4);

            var subsetSumAlg = new SubsetSumLLLReductionAlgorithm<int, decimal>(
                vectorFactory,
                scalarProd,
                nearest,
                Comparer<decimal>.Default,
                integerDecimalConverter,
                decimalField);

            var vectorReader = new IntegerArrayVectorReader();
            var vector = new[] { 366, 385, 392, 401, 422, 437 };
            var result = subsetSumAlg.Run(vector, 1215, 3M / 4);

            Console.WriteLine(PrintVector(result));
        }

        /// <summary>
        /// Testes adicionais sobre a factorização de polinómios e elevação de factores.
        /// </summary>
        public static void Test20()
        {
            var integerDomain = new BigIntegerDomain();

            // Leitura do polinómio
            var polynomialReader = new BigIntegerPolynomialReader();
            //var testPol = polynomialReader.Read("x^3+10*x^2-432*x+5040");
            //var firstFactor = polynomialReader.Read("x");
            //var secondFactor = polynomialReader.Read("x^2-2");
            //var modIntField = new ModularSymmetricBigIntField(5);
            //var liftInput = new LinearLiftingStatus<BigInteger>(
            //    testPol,
            //    firstFactor,
            //    secondFactor,
            //    modIntField,
            //    integerDomain);
            //var liftAlg = new LinearLiftAlgorithm<BigInteger>();
            //var liftAlgRes = liftAlg.Run(liftInput, 10);

            var polynom = polynomialReader.Read("2*x^4+8*x^3+17*x^2+18*x+4");

            // Instanciação dos algoritmos
            var resultantAlg = new UnivarPolDeterminantResultantAlg<BigInteger>(new BigIntegerDomain());
            var primesGenerator = new BigIntPrimeNumbsIterator(int.MaxValue);

            // Obtém o valor do coeficiente principal e do discriminante.
            var leadingCoeff = polynom.GetLeadingCoefficient(integerDomain);
            var discriminant = resultantAlg.Run(
                polynom,
                polynom.GetPolynomialDerivative(integerDomain));
            var primesEnumerator = primesGenerator.GetEnumerator();
            var prime = integerDomain.MultiplicativeUnity;
            var state = true;
            while (state)
            {
                if (primesEnumerator.MoveNext())
                {
                    var innerPrime = primesEnumerator.Current;
                    if (!integerDomain.IsAdditiveUnity(integerDomain.Rem(leadingCoeff, innerPrime)) &&
                        !integerDomain.IsAdditiveUnity(integerDomain.Rem(discriminant, innerPrime)))
                    {
                        prime = innerPrime;
                        state = false;
                    }
                }
                else // Todos os primos gerados dividem pelo menos o coeficiente principal e o discriminante
                {
                    Console.WriteLine("Foram esgotados todos os primos disponíveis sem encontrar um que não divida o coeficiente principal e o discriminante.");
                    state = false;
                }
            }

            // Temporário
            prime = 31;

            // Neste ponto estamos em condições de tentar factorizar o polinómio.
            if (prime > 1)
            {
                // Realiza a factorização.
                var integerModularField = new ModularSymmetricBigIntField(prime);

                // Instancia o algoritmo responsável pela factorização sobre corpos finitos.
                var finiteFieldFactorizationAlg = new FiniteFieldPolFactorizationAlgorithm<BigInteger>(
                    new UnivarSquareFreeDecomposition<Fraction<BigInteger, IEuclidenDomain<BigInteger>>>(),
                    new BigIntegerToIntegerConversion(),
                    new DenseCondensationLinSysAlgorithm<BigInteger>(integerModularField));

                // Instancia o algoritmo responsável pela elevação multi-factor.
                var modularFactory = new ModularSymmetricBigIntFieldFactory();
                var multiFactorLiftAlg = new MultiFactorLiftAlgorithm<BigInteger>(
                    new LinearLiftAlgorithm<BigInteger>(
                        modularFactory,
                        new UnivarPolEuclideanDomainFactory<BigInteger>(),
                        integerDomain));
                var factored = finiteFieldFactorizationAlg.Run(polynom, integerModularField, integerDomain);
                var liftedFactors = new Dictionary<BigInteger, IList<UnivariatePolynomialNormalForm<BigInteger>>>();
                foreach (var factorKvp in factored)
                {
                    var multiLiftStatus = new MultiFactorLiftingStatus<BigInteger>(
                        polynom,
                        factorKvp.Value,
                        prime);
                    var liftResult = multiFactorLiftAlg.Run(multiLiftStatus, 2);
                    Console.WriteLine("Módulo {0}.", liftResult.LiftingPrimePower);
                    liftedFactors.Add(factorKvp.Key, liftResult.Factors);

                    // Teste à fase de pesquisa
                    var searchAlgorithm = new SearchFactorizationAlgorithm<BigInteger>(
                        modularFactory,
                        new BigIntegerDomain());
                    var searchResult = searchAlgorithm.Run(liftResult, 1288, 3);
                }

                // Imprime os resultados para a consola.
                foreach (var liftFactor in liftedFactors)
                {
                    Console.WriteLine("Grau {0}", liftFactor.Key);
                    foreach (var factor in liftFactor.Value)
                    {
                        Console.WriteLine(factor);
                    }

                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Testa a simplificação de expressões lógicas.
        /// </summary>
        public static void Test19()
        {
            var booleanInput = new BooleanMinimalFormInOut();

            var combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            booleanInput.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[0] = EBooleanMinimalFormOutStatus.ON;
            booleanInput.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            booleanInput.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[0] = EBooleanMinimalFormOutStatus.ON;
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            booleanInput.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[0] = EBooleanMinimalFormOutStatus.ON;
            combination[3] = EBooleanMinimalFormOutStatus.ON;
            booleanInput.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            combination[3] = EBooleanMinimalFormOutStatus.ON;
            booleanInput.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[1] = EBooleanMinimalFormOutStatus.ON;
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            combination[3] = EBooleanMinimalFormOutStatus.ON;
            booleanInput.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            combination[4] = EBooleanMinimalFormOutStatus.ON;
            booleanInput.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[0] = EBooleanMinimalFormOutStatus.ON;
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            combination[3] = EBooleanMinimalFormOutStatus.ON;
            combination[4] = EBooleanMinimalFormOutStatus.ON;
            booleanInput.Add(combination, EBooleanMinimalFormOutStatus.ON);

            var algorithm = new BooleanMinimalFormAlgorithm();
            var result = algorithm.Run(booleanInput);
            foreach (var logicComb in result)
            {
                Console.WriteLine(logicComb.LogicInput);
            }
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
            Console.WriteLine(PrintVector(result.Medians)); ;

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
            // Faz a leitura da matriz dos custos a partir de um ficheiro de csv
            //var fileInfo = new FileInfo("..\\..\\Files\\Matrix.csv");
            //if (fileInfo.Exists)
            //{
            //    var dataProvider = new DataReaderProvider<IParse<Nullable<double>, string, string>>(
            //            new NullableDoubleParser());
            //    var csvParser = new CsvFileParser<TabularListsItem, Nullable<double>, string, string>(
            //        "new_line",
            //        "semi_colon",
            //        dataProvider);
            //    csvParser.AddIgnoreType("carriage_return");

            //    using (var textReader = fileInfo.OpenText())
            //    {
            //        var symbolReader = new StringSymbolReader(textReader, false, false);
            //        var table = new TabularListsItem();

            //        csvParser.Parse(symbolReader, table, new TabularItemAdder<Nullable<double>>());

            //        var costs = new SparseDictionaryMatrix<Nullable<double>>(table.Count, table.Count, null);
            //        for (int i = 0; i < table.Count; ++i)
            //        {
            //            var currentLine = table[i];
            //            for (int j = 0; j < currentLine.Count; ++j)
            //            {
            //                costs[i, j] = currentLine[j].GetCellValue<Nullable<double>>();
            //            }
            //        }

            //        // p = 5
            //        var initialSolution = new Nullable<double>[table.Count];
            //        for (int i = 0; i < initialSolution.Length; ++i)
            //        {
            //            initialSolution[i] = 0;
            //        }

            //        initialSolution[0] = 1;
            //        initialSolution[21] = 0.5;
            //        initialSolution[36] = 0.5;
            //        initialSolution[39] = 1;
            //        initialSolution[45] = 0.5;
            //        initialSolution[71] = 0.5;
            //        initialSolution[98] = 1;

            //        var correction = new LinearRelRoundCorrectorAlg<Nullable<double>>(
            //            Comparer<Nullable<double>>.Default,
            //            new IntegerNullableDoubleConverter(),
            //            new NullableIntegerNearest(),
            //            new NullableDoubleField());
            //        var result = correction.Run(initialSolution, costs, 1);
            //        Console.WriteLine("Medianas:");
            //        foreach (var chose in result.Chosen)
            //        {
            //            Console.WriteLine(chose);
            //        }

            //        Console.WriteLine("Custo: {0}", result.Cost);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("O camminho fornecido não existe.");
            //}

            // Outro exemplo
            var fileInfo = new FileInfo("..\\..\\Files\\Matrix1.csv");
            if (fileInfo.Exists)
            {
                var dataProvider = new DataReaderProvider<IParse<Nullable<double>, string, string>>(
                        new NullableDoubleParser());
                var csvParser = new CsvFileParser<TabularListsItem, Nullable<double>, string, string>(
                    "new_line",
                    "semi_colon",
                    dataProvider);
                csvParser.AddIgnoreType("carriage_return");

                using (var textReader = fileInfo.OpenText())
                {
                    var symbolReader = new StringSymbolReader(textReader, false, false);
                    var table = new TabularListsItem();

                    csvParser.Parse(symbolReader, table, new TabularItemAdder<Nullable<double>>());

                    var costs = new SparseDictionaryMatrix<Nullable<double>>(table.Count, table.Count, null);
                    for (int i = 0; i < table.Count; ++i)
                    {
                        var currentLine = table[i];
                        for (int j = 0; j < currentLine.Count; ++j)
                        {
                            costs[i, j] = currentLine[j].GetCellValue<Nullable<double>>();
                        }
                    }

                    // p = 5
                    var initialSolution = new Nullable<double>[table.Count];
                    for (int i = 0; i < initialSolution.Length; ++i)
                    {
                        initialSolution[i] = 0;
                    }

                    initialSolution[0] = 1;
                    initialSolution[1] = 0.137905;
                    initialSolution[4] = 0.262507;
                    initialSolution[5] = 0.104513;
                    initialSolution[8] = 0.337386;
                    initialSolution[9] = 0.084155;
                    initialSolution[12] = 0.332995;
                    initialSolution[16] = 0.143789;
                    initialSolution[20] = 0.121665;
                    initialSolution[24] = 0.0851576;
                    initialSolution[64] = 0.266743;
                    initialSolution[65] = 0.047304;
                    initialSolution[68] = 0.271134;
                    initialSolution[72] = 0.395872;
                    initialSolution[80] = 0.0395245;
                    initialSolution[128] = 0.106906;
                    initialSolution[129] = 0.0313958;
                    initialSolution[132] = 0.0663087;
                    initialSolution[136] = 0.0560474;
                    initialSolution[144] = 0.0250102;
                    initialSolution[192] = 0.0264022;
                    initialSolution[257] = 0.106054;
                    initialSolution[260] = 0.0671126;
                    initialSolution[272] = 0.105553;
                    initialSolution[320] = 0.137139;
                    initialSolution[384] = 0.140151;
                    initialSolution[512] = 0.449401;
                    initialSolution[513] = 0.134202;
                    initialSolution[516] = 0.288092;
                    initialSolution[517] = 0.114046;
                    initialSolution[520] = 0.213213;
                    initialSolution[521] = 0.303104;
                    initialSolution[524] = 0.1657;
                    initialSolution[528] = 0.134703;
                    initialSolution[532] = 0.102779;
                    initialSolution[536] = 0.314371;
                    initialSolution[576] = 0.283856;
                    initialSolution[577] = 0.184431;
                    initialSolution[580] = 0.156918;
                    initialSolution[588] = 1;
                    initialSolution[592] = 0.198095;
                    initialSolution[640] = 0.14019;
                    initialSolution[644] = 0.121252;
                    initialSolution[648] = 0.295898;
                    initialSolution[704] = 0.152219;
                    initialSolution[768] = 0.159801;
                    initialSolution[769] = 0.0126368;
                    initialSolution[772] = 0.413599;
                    initialSolution[784] = 0.00675249;
                    initialSolution[832] = 0.152461;
                    initialSolution[896] = 0.00355069;

                    var correction = new LinearRelRoundCorrectorAlg<Nullable<double>>(
                        Comparer<Nullable<double>>.Default,
                        new IntegerNullableDoubleConverter(0.00001),
                        new NullableIntegerNearest(),
                        new NullableDoubleField());
                    var result = correction.Run(initialSolution, costs, 2);
                    Console.WriteLine("Medianas:");
                    foreach (var chose in result.Chosen)
                    {
                        Console.WriteLine(chose);
                    }

                    Console.WriteLine("Custo: {0}", result.Cost);
                }
            }
            else
            {
                Console.WriteLine("O camminho fornecido não existe.");
            }

            // Estabelece os leitores e os campos
            var arrayMatrixReader = new DoubleArrayMatrixReader();
            var sparseMatrixReader = new DoubleSparseMatrixReader();
            var arrayVectorReader = new DoubleArrayVectorReader();
            var doubleField = new DoubleField();

            var costsMatrixInput = "[[0,0,0,0,0],[3,0,0,0,0],[5,0,0,0,0],[2,1,0,0,0],[1,2,4,0,0]]";
            var costsMatrix = sparseMatrixReader.ReadArray(5, 5, costsMatrixInput);
            var linearRelaxation = new LinearRelaxationAlgorithm<double>(
                new SimplexAlgorithm<double>(Comparer<double>.Default, doubleField),
                new IntegerDoubleConverter(),
                doubleField);
            var linearRelaxationResult = linearRelaxation.Run(costsMatrix, 3);

            var inputConstraintsMatrix = "[[1,0,3],[0,2,2]]";
            var inputConstraintsVector = "[4,12,8]";
            var inputObjectiveFunc = "[-3,-2]";
            var cost = 0.0;
            var nonBasicVariables = new[] { 0, 1 };
            var basicVariables = new[] { 2, 3, 4 };

            var constraintsMatrix = arrayMatrixReader.ReadArray(3, 2, inputConstraintsMatrix);
            var constraintsVector = arrayVectorReader.ReadVector(3, inputConstraintsVector);

            var objectiveFunction = arrayVectorReader.ReadVector(2, inputObjectiveFunc);
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
                    var squareMatrix = (matrix as ArrayMatrix<Fraction<BigInteger, BigIntegerDomain>>).AsSquare();
                    var result = systemSolver.Run(squareMatrix, independentVector);
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
        /// Teste aos algoritmos relacionados com números primos e teoria dos números.
        /// </summary>
        public static void Test13()
        {
            var fastIntLogComputation = new FastBinaryLogIntegerPartAlg();
            Console.WriteLine(fastIntLogComputation.Run(1));

            var stopWatch = new Stopwatch();
            var integerLogarithmComputation = new BaseLogIntegerPart<BigInteger>(
                new BigIntegerDomain());
            stopWatch.Start();
            var computation = integerLogarithmComputation.Run(2, BigInteger.Pow(2, 100));
            stopWatch.Stop();
            Console.WriteLine("O valor do logaritmo foi {0} calculado em {1} ms.", computation, stopWatch.ElapsedMilliseconds);

            stopWatch.Reset();
            stopWatch.Start();
            computation = integerLogarithmComputation.Run(BigInteger.Pow(2, 50000), BigInteger.Pow(2, 100000));
            stopWatch.Stop();
            Console.WriteLine("O valor do logaritmo foi {0} calculado em {1} ms.", computation, stopWatch.ElapsedMilliseconds);

            stopWatch.Reset();
            var fastBigIntLogComputation = new FastBigIntBinaryLogIntPartAlg();
            stopWatch.Start();
            computation = fastBigIntLogComputation.Run(BigInteger.Pow(2, 10000000));
            stopWatch.Stop();
            Console.WriteLine(
                "O valor do logaritmo foi {0} calculado em {1} ms.",
                computation,
                stopWatch.ElapsedMilliseconds);

            stopWatch.Reset();
            var fasterBigIntLogComputation = new FasterBigIntBinaryLogIntPartAlg();
            stopWatch.Start();
            computation = fasterBigIntLogComputation.Run(BigInteger.Pow(2, 10000000));
            stopWatch.Stop();
            Console.WriteLine(
                "O valor do logaritmo foi {0} calculado em {1} ms.",
                computation,
                stopWatch.ElapsedMilliseconds);

            stopWatch.Reset();
            stopWatch.Start();
            var log = BigInteger.Log(BigInteger.Pow(2, 10000000)) / Math.Log(2);
            stopWatch.Stop();
            Console.WriteLine(
                "O valor do logaritmo foi {0} calculado em {1} ms.",
                (int)Math.Floor(log),
                stopWatch.ElapsedMilliseconds);

            var bigIntegerSquareRootAlg = new BigIntSquareRootAlgorithm();
            var bigIntSquareRoot = bigIntegerSquareRootAlg.Run(86467898987098776);
            Console.WriteLine(bigIntSquareRoot);

            var integerDomain = new IntegerDomain();
            var congruences = new List<Congruence<int>>()
            {
                new Congruence<int>(2, 1),
                new Congruence<int>(3,2),
                new Congruence<int>(4, 3)
            };

            var chineseAlg = new ChineseRemainderAlgorithm<int>(1);
            var congruence = chineseAlg.Run(congruences, integerDomain);
            Console.WriteLine(congruence);

            var generalRoot = new GenericIntegerNthRootAlgorithm<BigInteger>(new BigIntegerDomain());
            var rootIndex = 5;
            var radicandValue = 1419877;
            var sqrtRes = generalRoot.Run(rootIndex, radicandValue);
            Console.WriteLine("A raiz de indice {0} de {1} vale {2}.", rootIndex, radicandValue, sqrtRes);

            var quadraticSieve = new QuadraticFieldSieve<int>(
                new IntegerSquareRootAlgorithm(),
                new ModularSymmetricIntFieldFactory(),
                new PrimeNumbersIteratorFactory(),
                integerDomain);
            var temp = quadraticSieve.Run(13459, 200, 100);
            Console.WriteLine("[{0},{1}]", temp.Item1, temp.Item2);

            var eulerFunction = new EulerTotFuncAlg<int>(
                new IntegerSquareRootAlgorithm(),
                new PrimeNumbersIteratorFactory(),
                integerDomain);
            Console.WriteLine(eulerFunction.Run(1937));

            var perfectPowerAlgotithm = new IntPerfectPowerTestAlg(
                new PrimeNumbersIteratorFactory());
            for (int i = 0; i <= 100; ++i)
            {
                if (perfectPowerAlgotithm.Run(i))
                {
                    Console.WriteLine(i);
                }
            }

            var aksPrimalityTest = new AksPrimalityTest();
            var n = 13459;
            for (int i = 1; i < 100; ++i)
            {
                if (aksPrimalityTest.Run(i))
                {
                    Console.WriteLine("{0} is prime", i);
                }
                else
                {
                    Console.WriteLine("{0} isn't prime", i);
                }
            }

            var pollardRhoAlg = new PollardRhoAlgorithm<int>(
                new ModularIntegerFieldFactory(),
                integerDomain);
            //n = 38;
            var pollardResult = pollardRhoAlg.Run(n);
            var pollardBlockedResult = pollardRhoAlg.Run(n, 10);
            Console.WriteLine("[{0}, {1}]", pollardResult.Item1, pollardResult.Item2);
            Console.WriteLine("[{0}, {1}]", pollardBlockedResult.Item1, pollardBlockedResult.Item2);

            Console.WriteLine(MathFunctions.Power(2, 6, new IntegerDomain()));
            var legendreJacobiAlg = new LegendreJacobiSymbolAlgorithm<int>(integerDomain);
            Console.WriteLine(legendreJacobiAlg.Run(12345, 331));
            Console.WriteLine(legendreJacobiAlg.Run(13, 44));

            var resSol = new ResSolAlgorithm<int>(new ModularIntegerFieldFactory(), integerDomain);
            Console.WriteLine(PrintVector(resSol.Run(10, 13)));
            Console.WriteLine(PrintVector(resSol.Run(17, 47)));

            Console.WriteLine("E agora sobre os números grandes.");
            var bigintegerPerfeectPowAlg = new BigIntPerfectPowerTestAlg(
                new GenericIntegerNthRootAlgorithm<BigInteger>(new BigIntegerDomain()),
                new BigIntegerPrimeNumbersIteratorFactory());
            for (int i = 0; i <= 100; ++i)
            {
                if (bigintegerPerfeectPowAlg.Run(i))
                {
                    Console.WriteLine(i);
                }
            }

            var primeNumberEnumerator = new IntPrimeNumbersIterator(100);
            foreach (var primeNumber in primeNumberEnumerator)
            {
                Console.WriteLine(primeNumber);
            }

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

            var naiveFactorizationAlgorithm = new NaiveIntegerFactorizationAlgorithm<int>(
                new IntegerSquareRootAlgorithm(), new IntegerDomain());
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
            var fourthInput = "x^8+x^6-3*x^4-3*x^3+8*x^2+2*x-5";
            var fifthInput = "3*x^6+5*x^4-4*x^2-9*x+21";

            var integerDomain = new IntegerDomain();
            var bigIntegerDomain = new BigIntegerDomain();
            var fractionField = new FractionField<int, IntegerDomain>(integerDomain);
            var integerParser = new IntegerParser<string>();
            var bigIntegerParser = new BigIntegerParser<string>();
            var fractionParser = new ElementFractionParser<int, IntegerDomain>(integerParser, integerDomain);

            // Leitura dos polinómios como sendo constituídos por inteiros grandes
            var integerReader = new StringReader(fourthInput);
            var integerSymbolReader = new StringSymbolReader(integerReader, false);
            var integerPolynomialParser = new UnivariatePolynomialReader<BigInteger, CharSymbolReader<string>>(
                "x",
                bigIntegerParser,
                bigIntegerDomain);

            var integerConversion = new BigIntegerToIntegerConversion();
            var fourthIntegerPol = default(UnivariatePolynomialNormalForm<BigInteger>);
            if (integerPolynomialParser.TryParsePolynomial(
                integerSymbolReader,
                integerConversion,
                out fourthIntegerPol))
            {
                integerReader = new StringReader(fifthInput);
                integerSymbolReader = new StringSymbolReader(integerReader, false);
                var fifthIntegerPol = default(UnivariatePolynomialNormalForm<BigInteger>);
                if (integerPolynomialParser.TryParsePolynomial(
                integerSymbolReader,
                integerConversion,
                out fifthIntegerPol))
                {
                    var pseudoDomain = new UnivarPolynomPseudoDomain<BigInteger>("x", bigIntegerDomain);
                    var quoAndRem = pseudoDomain.GetQuotientAndRemainder(fourthIntegerPol, fifthIntegerPol);
                    Console.WriteLine("Quociente: {0}", quoAndRem.Quotient);
                    Console.WriteLine("Resto: {0}", quoAndRem.Remainder);

                    var resultantAlg = new UnivarPolSubResultantAlg<BigInteger>();
                    var resultantResult = resultantAlg.Run(fourthIntegerPol, fifthIntegerPol, bigIntegerDomain);
                    Console.WriteLine(resultantResult);
                }
                else
                {
                    Console.WriteLine("Ocorreu um erro durante a leitura do segundo polinomio.");
                }
            }
            else
            {
                Console.WriteLine("Ocorreu um erro durante a leitura do primeiro polinomio.");
            }

            // Leitura dos polinómios como sendo consituídos por fracções
            var reader = new StringReader(firstInput);
            var stringSymbolReader = new StringSymbolReader(reader, false);
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

            var arrayMatrixFactory = new ArraySquareMatrixFactory<int>();
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
                var integerDomain = new IntegerDomain();
                var divFreeCharPol = new FastDivisionFreeCharPolynomCalculator<int>(
                    "lambda",
                    integerDomain);
                var computedCharPol = divFreeCharPol.Run(matrix as ISquareMatrix<int>);
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
                    Console.Write("{0} ", powerRootsSums[i, 0]);
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
                        var lifting = new LinearLiftAlgorithm<int>(
                            new ModularIntegerFieldFactory(),
                            new UnivarPolEuclideanDomainFactory<int>(),
                            integerDomain);
                        var status = new LinearLiftingStatus<int>(
                            firstPol,
                            secondPol,
                            thirdPol,
                            integerModularField.Module);
                        var liftingResult = lifting.Run(status, 10);
                        //status.ep
                    }
                }
            }

            var input = "x^3+2";
            integerModularField.Module = 5;
            reader = new StringReader(input);
            stringSymbolReader = new StringSymbolReader(reader, false);

            var parsedPol = default(UnivariatePolynomialNormalForm<int>);
            if (polynomialParser.TryParsePolynomial(
                stringSymbolReader,
                conversion,
                out parsedPol))
            {
                var finiteFieldFactorization = new FiniteFieldPolFactorizationAlgorithm<int>(
                    new UnivarSquareFreeDecomposition<Fraction<int, IEuclidenDomain<int>>>(),
                    new ElementToElementConversion<int>(),
                    new DenseCondensationLinSysAlgorithm<int>(integerModularField));

                var factored = finiteFieldFactorization.Run(parsedPol, integerModularField, new IntegerDomain());

                foreach (var factorKvp in factored)
                {
                    Console.WriteLine("{0} => {1}", factorKvp.Key, factorKvp.Value);
                }
            }
        }

        /// <summary>
        /// Teste às matrizes, incluindo o algoritmo LLL.
        /// </summary>
        public static void Test9()
        {
            var firstInput = "[1,2]";
            var secondInput = "[2,1]";
            var reader = new StringReader(firstInput);
            var stringsymbolReader = new StringSymbolReader(reader, false);
            var integerDomain = new IntegerDomain();
            var integerParser = new IntegerParser<string>();
            var fractionField = new FractionField<int, IntegerDomain>(integerDomain);
            var fractionParser = new FractionExpressionParser<int, IntegerDomain>(integerParser, fractionField);
            var firstVector = default(IVector<Fraction<int, IntegerDomain>>);
            var secondVector = default(IVector<Fraction<int, IntegerDomain>>);
            var vectorFactory = new ArrayVectorFactory<Fraction<int, IntegerDomain>>();

            var vectorReader = new ConfigVectorReader<Fraction<int, IntegerDomain>, string, string, CharSymbolReader<string>>(
                2,
                vectorFactory);
            vectorReader.MapInternalDelimiters("left_bracket", "right_bracket");
            vectorReader.AddBlanckSymbolType("blancks");
            vectorReader.SeparatorSymbType = "comma";

            var errors = new List<string>();
            if (vectorReader.TryParseVector(stringsymbolReader, fractionParser, errors, out firstVector))
            {
                reader = new StringReader(secondInput);
                stringsymbolReader = new StringSymbolReader(reader, false);
                if (vectorReader.TryParseVector(stringsymbolReader, fractionParser, errors, out secondVector))
                {
                    var vectorSpace = new VectorSpace<Fraction<int, IntegerDomain>>(
                        2,
                        vectorFactory,
                        fractionField);

                    var scalarProd = new OrthoVectorScalarProduct<Fraction<int, IntegerDomain>>(
                        new FractionComparer<int, IntegerDomain>(Comparer<int>.Default, integerDomain),
                        fractionField);

                    var thirdVector = new ArrayVector<Fraction<int, IntegerDomain>>(2);
                    thirdVector[0] = new Fraction<int, IntegerDomain>(1, 1, integerDomain);
                    thirdVector[1] = new Fraction<int, IntegerDomain>(1, 1, integerDomain);
                    var generator = new VectorSpaceGenerator<Fraction<int, IntegerDomain>>(2);
                    generator.Add(firstVector);
                    generator.Add(secondVector);
                    generator.Add(thirdVector);

                    var orthogonalized = generator.GetOrthogonalizedBase(
                        fractionField,
                        vectorSpace,
                        scalarProd);

                    foreach (var basisVector in orthogonalized)
                    {
                        Console.WriteLine(PrintVector(basisVector));
                    }

                    var fractionComparer = new FractionComparer<int, IntegerDomain>(
                        Comparer<int>.Default,
                        integerDomain);
                    var nearest = new FractionNearestInteger(integerDomain);
                    var lllReductionAlg = new LLLBasisReductionAlgorithm<IVector<Fraction<int, IntegerDomain>>,
                                                                         Fraction<int, IntegerDomain>,
                                                                         int>(
                           vectorSpace,
                           scalarProd,
                           nearest,
                           new FractionComparer<int, IntegerDomain>(Comparer<int>.Default, integerDomain));

                    var lllReduced = lllReductionAlg.Run(
                        new IVector<Fraction<int, IntegerDomain>>[] { firstVector, secondVector },
                        new Fraction<int, IntegerDomain>(4, 3, integerDomain));

                    for (int i = 0; i < lllReduced.Length; ++i)
                    {
                        Console.WriteLine(PrintVector(lllReduced[i]));
                    }
                }
                else
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
            else
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }

            var sparseDictionaryMatrix = new SparseDictionaryMatrix<int>(10, 10);
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
            var structureAffectations = new int[][] { 
                new int[] { 1, 2, 3 }, 
                new int[] { 1, 2, 3 }, 
                new int[] { 1, 2, 3 } };
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
                e => e.Value,
                Comparer<double>.Default,
                new DoubleField());
        }

        static string PrintVector<T>(IEnumerable<T> vectorToPrint)
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
