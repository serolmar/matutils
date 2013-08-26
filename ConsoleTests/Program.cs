namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Numerics;
    using System.Text;
    using Mathematics;
    using Mathematics.AlgebraicStructures.Polynomial;
    using Mathematics.MathematicsInterpreter;
    using Utilities.Collections;
    using Utilities.Parsers;

    class Program
    {
        static void Main(string[] args)
        {
            Test12();
            Console.ReadLine();
        }

        static void RunObjectTester()
        {
            var tester = new ObjectTester();
            tester.Run(Console.In, Console.Out);
        }

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

        public static void Test13()
        {
            Console.WriteLine(MathFunctions.Power(2,6, new IntegerDomain()));
            var legendreJacobiAlg = new LegendreJacobiSymbolAlgorithm();
            Console.WriteLine(legendreJacobiAlg.Run(12345, 331));
            Console.WriteLine(legendreJacobiAlg.Run(13, 44));

            var resSol = new ResSolAlgorithm();
            Console.WriteLine(PrintVector(resSol.Run(10, 13)));
            Console.WriteLine(PrintVector(resSol.Run(17, 47)));

            var perfectPowerAlgotithm = new PerfectPowerTestAlgorithm();
            for (int i = 0; i <= 1000000; ++i)
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
            var lagrangeAlg = new LagrangeAlgorithm<int, IntegerDomain>(integerDomain);
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
            var n = 35349384;
            Console.WriteLine("The factors of {0} are:", n);
            var factorsResult = naiveFactorizationAlgorithm.Run(n);
            foreach (var factor in factorsResult)
            {
                Console.WriteLine("{0}^{1}", factor.Key, factor.Value);
            }
        }

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
            var polynomialParser = new UnivariatePolynomialReader<
                Fraction<int, IntegerDomain>, 
                FractionField<int, IntegerDomain>,
                CharSymbolReader<string>>(
                "x", 
                fractionParser, 
                fractionField);

            var fractionConversion = new ElementFractionConversion<int, IntegerDomain>(integerDomain);
            var firstPol = default(UnivariatePolynomialNormalForm<Fraction<int, IntegerDomain>, FractionField<int, IntegerDomain>>);
            if (polynomialParser.TryParsePolynomial(stringSymbolReader, fractionConversion, out firstPol))
            {
                reader = new StringReader(secondInput);
                stringSymbolReader = new StringSymbolReader(reader, false);
                var secondPol = default(UnivariatePolynomialNormalForm<Fraction<int, IntegerDomain>, FractionField<int, IntegerDomain>>);
                if (polynomialParser.TryParsePolynomial(stringSymbolReader, fractionConversion, out secondPol))
                {
                    var polynomialEuclideanDomain = new UnivarPolynomEuclideanDomain<
                        Fraction<int, IntegerDomain>, 
                        FractionField<int, IntegerDomain>>(
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
            var bigIntegerParser = new BigIntegerParser();
            var otherFractionParser = new ElementFractionParser<BigInteger, BigIntegerDomain>(bigIntegerParser, bigIntegerDomain);
            var otherFractionField = new FractionField<BigInteger, BigIntegerDomain>(bigIntegerDomain);
            var otherPolParser = new UnivariatePolynomialReader<
                Fraction<BigInteger, BigIntegerDomain>,
                FractionField<BigInteger, BigIntegerDomain>,
                CharSymbolReader<string>>(
                "x",
                otherFractionParser,
                otherFractionField);
            var otherFractionConversion = new BigIntegerFractionToIntConversion();
            var thirdPol = default(UnivariatePolynomialNormalForm<Fraction<BigInteger, BigIntegerDomain>, FractionField<BigInteger, BigIntegerDomain>>);
            if (otherPolParser.TryParsePolynomial(stringSymbolReader, otherFractionConversion, out thirdPol))
            {
                var univarSquareFreeAlg = new UnivarSquareFreeDecomposition<Fraction<BigInteger, BigIntegerDomain>, FractionField<BigInteger, BigIntegerDomain>>();
                var result = univarSquareFreeAlg.Run(thirdPol);
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
                var divFreeCharPol = new FastDivisionFreeCharPolynomCalculator<int, IntegerDomain>("lambda", integerDomain);
                var computedCharPol = divFreeCharPol.Run(matrix);
                Console.WriteLine("O determinante usando permutações vale: {0}.", computedCharPol);
            }
        }

        public static void Test10()
        {
            var integerSequence = new IntegerSequence();
            integerSequence.Add(0, 10);
            integerSequence.Remove(5,8);
            integerSequence.Remove(1,6);

            foreach (var value in integerSequence)
            {
                Console.Write(" {0}", value);
            }
        }

        public static void Test9()
        {
            var sparseDictionaryMatrix = new SparseDictionaryMatrix<int>(0);
            Console.WriteLine("Linhas: {0}; Colunas: {1}", sparseDictionaryMatrix.GetLength(0), sparseDictionaryMatrix.GetLength(1));
            Console.WriteLine("[0,0] = {0}", sparseDictionaryMatrix[0, 0]);

            sparseDictionaryMatrix[2, 3] = 0;
            sparseDictionaryMatrix[4, 1] = 5;

            Console.WriteLine("Linhas: {0}; Colunas: {1}", sparseDictionaryMatrix.GetLength(0), sparseDictionaryMatrix.GetLength(1));
            Console.WriteLine("[4,1] = {0}", sparseDictionaryMatrix[4, 1]);

            sparseDictionaryMatrix.SwapLines(4, 1);
            Console.WriteLine("Linhas: {0}; Colunas: {1}", sparseDictionaryMatrix.GetLength(0), sparseDictionaryMatrix.GetLength(1));
            Console.WriteLine("[1,1] = {0}", sparseDictionaryMatrix[1, 1]);

            sparseDictionaryMatrix.SwapColumns(3, 5);
            Console.WriteLine("Linhas: {0}; Colunas: {1}", sparseDictionaryMatrix.GetLength(0), sparseDictionaryMatrix.GetLength(1));
        }

        static void Test8()
        {
            var input = "[[1-x,2],[4, 3-x]]";
            var inputReader = new StringSymbolReader(new StringReader(input), false);
            var integerParser = new IntegerParser<string>();
            var integerDomain = new IntegerDomain();
            var conversion = new ElementToElementConversion<int>();
            var univariatePolParser = new UnivarPolNormalFormParser<int, IntegerDomain>(
                "x",
                conversion,
                integerParser,
                integerDomain);

            var arrayMatrixFactory = new ArrayMatrixFactory<UnivariatePolynomialNormalForm<int, IntegerDomain>>();
            var arrayReader = new ConfigMatrixReader<UnivariatePolynomialNormalForm<int, IntegerDomain>, string, string, CharSymbolReader<string>>(
                2,
                2,
                arrayMatrixFactory);
            arrayReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayReader.AddBlanckSymbolType("blancks");
            arrayReader.SeparatorSymbType = "comma";

            var readed = default(IMatrix<UnivariatePolynomialNormalForm<int, IntegerDomain>>);
            if (arrayReader.TryParseMatrix(inputReader, univariatePolParser, out readed))
            {
                var polynomialRing = new UnivarPolynomRing<int, IntegerDomain>("x", integerDomain);
                var permutationDeterminant = new PermutationDeterminantCalculator<
                    UnivariatePolynomialNormalForm<int, IntegerDomain>, 
                    UnivarPolynomRing<int, IntegerDomain>>(polynomialRing);

                var computedDeterminant = permutationDeterminant.Run(readed);

                Console.WriteLine("O determinante usando permutações vale: {0}.", computedDeterminant);

                var expansionDeterminant = new ExpansionDeterminantCalculator<
                    UnivariatePolynomialNormalForm<int, IntegerDomain>, 
                    UnivarPolynomRing<int, IntegerDomain>>(polynomialRing);

                computedDeterminant = expansionDeterminant.Run(readed);

                Console.WriteLine("O determinante usando expansão vale: {0}.", computedDeterminant);

                Console.WriteLine(readed);
            }
            else
            {
                Console.WriteLine("Errors found.");
            }
        }

        static void Test7()
        {
            var input = "x+y^2*(z-1)^3-x";
            var integerParser = new IntegerParser<string>();

            var inputReader = new StringSymbolReader(new StringReader(input), false);
            var polynomialParser = new PolynomialReader<int, IntegerDomain, CharSymbolReader<string>>(integerParser, new IntegerDomain());
            var readed = default(Polynomial<int, IntegerDomain>);
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
                    Console.WriteLine("Errors parsing range:");
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
            var arrayMatrixReader = new ConfigMatrixReader<int, string, string, CharSymbolReader<string>>(3, 3, arrayMatrixFactory);
            arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayMatrixReader.AddBlanckSymbolType("blancks");
            arrayMatrixReader.SeparatorSymbType = "comma";

            var matrix = default(IMatrix<int>);
            var errors = new List<string>();
            if (arrayMatrixReader.TryParseMatrix(stringsymbolReader, integerParser, errors, out matrix))
            {
                Console.WriteLine(matrix);

                var integerDomain = new IntegerDomain();
                var permutationDeterminant = new PermutationDeterminantCalculator<int, IntegerDomain>(integerDomain);
                var computedDeterminant = permutationDeterminant.Run(matrix);
                Console.WriteLine("O determinante usando permutações vale: {0}.", computedDeterminant);

                var expansionDeterminant = new ExpansionDeterminantCalculator<int, IntegerDomain>(integerDomain);
                computedDeterminant = expansionDeterminant.Run(matrix);
                Console.WriteLine("O determinante usando expansão vale: {0}.", computedDeterminant);

                var condensationDeterminant = new CondensationDeterminantCalculator<int, IntegerDomain>(integerDomain);
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

            var symmetric = new SymmetricPolynomial<int, IntegerDomain>(
                new List<string>() { "x", "y", "z", "w" },
                dictionary,
                1,
                new IntegerDomain());

            var rep = symmetric.GetElementarySymmetricRepresentation(varDictionary);
            var expanded = rep.GetExpanded();
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
            Console.WriteLine("Please insert expression to be evaluated:");
            //var input = Console.ReadLine();
            //var reader = new StringReader(input);
            //var result = new IntegerExpressionParser<TextReader>().Parse(reader);
            // var result = new DoubleExpressionParser().Parse(reader);
            //var result = new BoolExpressionParser().Parse(reader);
            //Console.WriteLine("The result is {0}.", result);

            //var ring = new IntegerRing();
            //var polList = new List<Polynomial<int, IntegerRing>>();

            //var p1 = new Polynomial<int, IntegerRing>(3, new[] { 1, 2 }, new[] { "x", "y" }, ring);
            //var p2 = new Polynomial<int, IntegerRing>(2, new[] { 2, 3 }, new[] { "x", "z" }, ring);

            //var p3 = p1.Add(p2);
            //var p4 = p2.Multiply(p3);
            //var p5 = p4.Multiply(p4);

            //polList.Add(p1);
            //polList.Add(p1.Add(p2));
            //polList.Add(p1.Multiply(p2));
            //polList.Add(p4);
            //polList.Add(p5);
            //polList.Add(p5.GetExpanded());
            //var dic = new Dictionary<string, int>();
            //dic.Add("x", 2);
            //polList.Add(p5.Replace(dic));
            //var dic2 = new Dictionary<string, Polynomial<int, IntegerRing>>();
            //dic2.Add("x", p1);
            //polList.Add(p3.Replace(dic2));

            //foreach (var pol in polList)
            //{
            //    Console.WriteLine(pol);
            //}

            //var input = "[[[1,2],[3,4]],[[5,6],[7,8]],[[9,10],[11,       12]]  ]";
            //var input1 = "[[1,2],[[3],4]]";
            //var reader = new StringReader(input);
            //var reader1 = new StringReader(input1);

            //var mr = new MultiDimensionalRange<int>(new[] { 2, 2, 3 });
            //var mrParser = new MultiDimensionalRangeConfigParser<int, string, string, CharSymbolReader>(mr);
            //mrParser.MapInternalDelimiters("left_bracket", "right_bracket");
            //mrParser.MapExternalDelimiters("left_bracket", "right_bracket");
            //mrParser.AddBlanckSymbolType("blancks");
            //mrParser.SeparatorSymbType = "comma";
            //mrParser.ParseRange(new StringSymbolReader(reader, true), new IntegerParser());
            //Console.WriteLine(mr);

            //var mrParser = new MultiDimensionalRangeNoConfigParser<int, string, string, CharSymbolReader>();
            //mrParser.MapInternalDelimiters("left_bracket", "right_bracket");
            //mrParser.MapExternalDelimiters("left_bracket", "right_bracket");
            //mrParser.AddBlanckSymbolType("blancks");
            //mrParser.SeparatorSymbType = "comma";
            //var mr = mrParser.ParseRange(new StringSymbolReader(reader, true), new IntegerParser());
            //Console.WriteLine(mr);


            //var permGen = new SwapPermutationsGenerator(5);
            //var count = 0;
            //foreach (var perm in permGen)
            //{
            //    ++count;
            //}

            //Console.WriteLine(count);
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
