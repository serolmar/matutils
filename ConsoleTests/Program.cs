﻿namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities.ExpressionBuilders;
    using Utilities.Collections;
    using Mathematics;
    using Mathematics.Algorithms;
    using Utilities.Parsers;
    using Utilities.Collections.Affectations;
    using Mathematics.MathematicsInterpreter;
    using Mathematics.AlgebraicStructures.Polynomial;

    class Program
    {
        static void Main(string[] args)
        {
            var tester = new ObjectTester();
            tester.Run(Console.In, Console.Out);
        }

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

        }

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

        static void Test2()
        {
            Console.WriteLine("Please insert expression to be evaluated:");
            var interpreter = new MathematicsInterpreter();
            var result = interpreter.Interpret(Console.In, Console.Out);
            Console.ReadKey();
        }

        static void Test1()
        {
            Console.WriteLine("Please insert expression to be evaluated:");
            var input = Console.ReadLine();
            var reader = new StringReader(input);
            var result = new IntegerExpressionParser().Parse(reader);
            // var result = new DoubleExpressionParser().Parse(reader);
            //var result = new BoolExpressionParser().Parse(reader);
            Console.WriteLine("The result is {0}.", result);

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
