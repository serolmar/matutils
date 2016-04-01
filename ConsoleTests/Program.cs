namespace ConsoleTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Numerics;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml;
    using System.Threading.Tasks;
    using Mathematics;
    using Mathematics.MathematicsInterpreter;
    using Utilities;
    using Utilities.Cuda;
    using OdmpProblem;

    class Program
    {
        delegate void Temp();

        static void Main(string[] args)
        {
            try
            {
                var factor = 1034 * 1024.0;
                var memInfo = Utils.GetMemoryInfo();
                Console.WriteLine("Free physical memory: {0}", memInfo.FreePhysicalMemory / factor);
                Console.WriteLine("Free virtual memory: {0}", memInfo.FreeVirtualMemory / factor);
                Console.WriteLine("Total visible memory: {0}", memInfo.TotalVisibleMemorySize / factor);

                var configuration = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                var configurationSection = configuration.GetSection("runtime");
                var raw = configurationSection.SectionInformation.GetRawXml();
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    var reader = new StringReader(raw);
                    var document = new XmlDocument();
                    document.Load(reader);

                    var elements = document.GetElementsByTagName("gcAllowVeryLargeObjects");
                    if (elements.Count == 1)
                    {
                        var enabled = ((XmlElement)elements[0]).GetAttribute("enabled");
                        var outEnabled = false;
                        if (bool.TryParse(enabled, out outEnabled))
                        {
                            Console.WriteLine("Ligado");
                        }
                        else
                        {
                            Console.WriteLine("Desligado");
                        }
                    }
                }

                //var r = new long[int.MaxValue / 2];
                var nr = (long)402653184;

                Console.WriteLine("Tentativa de instanciação de um objecto.");
                var temp = new GeneralLongArray<long>(nr);

                Console.WriteLine("O vector foi instanciado com {0} posições.", nr);
                for (var i = 0L; i < nr; ++i)
                {
                    temp[i] = i;
                }

                Console.WriteLine("Os valores foram atribuídos.");
                for (var i = 0L; i < nr; ++i)
                {
                    if (temp[i] != i) throw new Exception();
                }

                Console.WriteLine("Os valores foram comparados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
            }

            //Console.WriteLine();
            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
        }

        static void Lixo3()
        {
            var fileInfo = new FileInfo(@"Teste\mcca.mtx");
            if (fileInfo.Exists)
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    using (var textReader = new StreamReader(
                        fileStream,
                        Encoding.UTF8))
                    {
                        var symbolReader = new StringSymbolReader(
                            textReader,
                            true,
                            false);
                        var longParser = new LongParser<string>();
                        var doubleParser = new DoubleParser<string>(
                            System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent,
                            System.Globalization.NumberFormatInfo.InvariantInfo);
                        var matrix = default(CoordinateSparseMathMatrix<double>);
                        var matrixReader = new CoordMatrixMarketReader<double>(
                            (l, c, e) => { matrix = new CoordinateSparseMathMatrix<double>(l, c, 0); },
                            (l, c, e) => matrix[l - 1, c - 1] = e);
                        var res = matrixReader.Parse(
                            symbolReader,
                            longParser,
                            doubleParser,
                            new GeneralMapper<string, string>());
                        foreach (var log in res.GetLogs(EParseErrorLevel.ERROR))
                        {
                            Console.WriteLine(log);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("O ficheiro não existe.");
            }
        }

        static void Lixo2()
        {
            // Valor máximo permitido: 536870897 = int.MaxValue / 4 - int.MaxValue / (1 << 27) + int.MaxValue / (1 << 30) - inteiros
            //var temp = new int[536870897];
            //var temp1 = new long[268435448];
            //var temp2 = new BigInteger[134217724];
            //var temp3 = new IntegerSequence[134217724];
            //var temp4 = new int[20][];
            //for (int i = 0; i < 20; ++i)
            //{
            //    temp4[i] = new int[536870897];
            //}

            var fileInfo = new FileInfo("Teste\\gbvrt60.txt");
            if (fileInfo.Exists)
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    using (var textReader = new StreamReader(
                        fileStream,
                        Encoding.UTF8))
                    {
                        var genBankReader = new GenBankReader(textReader);
                        while (genBankReader.ReadNext())
                        {
                            var currentReaded = genBankReader.Current;
                            Console.WriteLine("Nome do locus: {0}", currentReaded.Locus.LocusName);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("O ficheiro não existe.");
            }
        }

        static void Lixo1()
        {
            var number = 8;
            var bitlist = BitList.ReadNumeric(number.ToString());

            var rectangularRegion = new NonIntersectingMergingRegionsSet<int, IMergingRegion<int>>();
            rectangularRegion.Add(
                new MergingRegion<int>(0, 0, 5, 0));
            rectangularRegion.Add(
                new MergingRegion<int>(0, 1, 1, 2));
            rectangularRegion.Add(
                new MergingRegion<int>(2, 1, 3, 2));
            rectangularRegion.Add(
                new MergingRegion<int>(4, 1, 5, 2));
            rectangularRegion.Add(
                new MergingRegion<int>(0, 3, 3, 5));
            rectangularRegion.Add(
                new MergingRegion<int>(4, 3, 6, 6));

            var intersectionRegion = rectangularRegion.GetMergingRegionForCell(0, 4);

            Console.WriteLine(((ulong)2 * uint.MaxValue).ToString().Length);
            var readedNumber = string.Empty;
            //using (var textReader = new StreamReader("bigNum.txt"))
            //{
            //    readedNumber = textReader.ReadToEnd();
            //}

            checked
            {
                var t111 = 19073486328125ul / 2;
                var t112 = 19073486328125ul / 2;
                var kkk = t111 * t112;
            }

            var stopWatch = new Stopwatch();
            //var bigInteger1 = BigInteger.Parse(readedNumber);
            //var bigInteger2 = BigInteger.Parse(readedNumber);
            //stopWatch.Start();
            //var result = bigInteger1 + bigInteger2;
            //stopWatch.Stop();
            //Console.WriteLine(stopWatch.ElapsedMilliseconds);
            //stopWatch.Reset();

            var bigInt1 = default(UlongArrayBigInt);
            var bigInt2 = default(UlongArrayBigInt);
            UlongArrayBigInt.TryParse(readedNumber, out bigInt1);
            UlongArrayBigInt.TryParse(readedNumber, out bigInt2);
            stopWatch.Start();
            var bigRes = bigInt1 + bigInt2;
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();

            stopWatch.Start();
            bigRes = UlongArrayBigInt.ParallelClaAdd(bigInt1, bigInt2);
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();

            var bigIntegerTests = new BigIntegerTests();
            var decomposed = bigIntegerTests.DecomposeNumber("11111111222222233333344444455555566666777778888899999");

            Console.WriteLine("{0}", (0x800000000000000).ToString().Length);
            Console.WriteLine("valor máximo: {0}", ulong.MaxValue);
            Console.WriteLine("semi-valor máximo: {0}", ulong.MaxValue >> 1);
            var dec = (ulong)1000000000000000000;
            var temp = (ulong)999999999999999999;
            checked
            {
                var temp1 = 2 * temp;
                Console.WriteLine("temp: {0}", temp);
                Console.WriteLine("temp1: {0}", temp1);
            }

            var ulongMax = ulong.MaxValue.ToString();
            Console.WriteLine("máximo ulong: {0}", ulongMax.Length);
            uint i = 0xFFFFFFFF;
            uint j = 0xFFFFFFFF;
            // (a*2^32+b)*(c*2^32+d)=(a*c)*2^64+(ad+bc)*2^32+bd
            // Parte superior a 32...

            Console.WriteLine("{0:X}", (ulong)2 * 0xFFFFFFFF);

            ulong k = (ulong)i * (ulong)j;
            uint l = i * j;

            var num1 = default(ulong);
            var num2 = default(ulong);
            var a = num1 >> 32;
            var b = num1 & 0x00000000FFFFFFFF;
            var c = num2 >> 32;
            var d = num2 & 0x00000000FFFFFFFF;

            // Adição de dois números
            // 1010 + 1001 -> diff: 0111
            num1 = 0xFFFFFFFFFFFFFFFF;
            num2 = 0xFFFFFFFFFFFFFFFF;
            var res = default(ulong);
            var carry = default(ulong);
            var diff = ((~num2) + 1);
            if (num2 < diff)
            {
                // Não há risco de sobrecarga
                res = num1 + num2;
                carry = 0;
            }
            else
            {
                res = num2 - diff;
                carry = 1;
            }

            Console.WriteLine("res = {0:X}", res);
            Console.WriteLine("carry = {0:X}", carry);

            Console.WriteLine("m={0:X}", (uint)0xFFFF * (uint)0xFFFF);

            Console.WriteLine("k={0:X}", k);
            Console.WriteLine("l={0:X}", l);
        }

        static void TempCuda()
        {
            try
            {
                // Inicializa CUDA e avalia os dispositivos existentes
                var cudaManager = CudaManager.GetManager();
                if (cudaManager.DevicesCount == 0)
                {
                    Console.WriteLine("Nenhum dispositivo com suporte CUDA foi encontrado.");
                }
                else
                {
                    // Obtém o primeiro dispositivo
                    var device = cudaManager.GetDevice(0);

                    // O contexto é automaticamente colocado como corrente para a linha de fluxo actual
                    var context = device.CrateContext();

                    // Carrega o módulo no contexto actual
                    var module = cudaManager.LoadModule("Data\\AddVector.cu.obj");

                    // Obtém a função a ser chamada
                    var cudaFunc = module.GetCudaFunction("Add");

                    var elemensNum = 10;
                    //var start = 0;
                    var firstVector = new int[elemensNum];
                    var secondVector = new int[elemensNum];
                    var result = new int[elemensNum];
                    for (int i = 0; i < elemensNum; ++i)
                    {
                        firstVector[i] = i + 1;
                        secondVector[i] = elemensNum - i;
                    }

                    // Reserva o primeiro vector
                    var firstCudaVector = default(SCudaDevicePtr);
                    var cudaResult = CudaApi.CudaMemAlloc(
                        ref firstCudaVector,
                        Marshal.SizeOf(typeof(int)) * elemensNum);
                    if (cudaResult != ECudaResult.CudaSuccess)
                    {
                        throw CudaException.GetExceptionFromCudaResult(cudaResult);
                    }

                    // Reserva o segundo vector
                    var secondCudaVector = default(SCudaDevicePtr);
                    cudaResult = CudaApi.CudaMemAlloc(
                        ref secondCudaVector,
                        Marshal.SizeOf(typeof(int)) * elemensNum);
                    if (cudaResult != ECudaResult.CudaSuccess)
                    {
                        throw CudaException.GetExceptionFromCudaResult(cudaResult);
                    }

                    // Reserva o terceiro vector
                    var resultCudaVector = default(SCudaDevicePtr);
                    cudaResult = CudaApi.CudaMemAlloc(
                        ref resultCudaVector,
                        Marshal.SizeOf(typeof(int)) * elemensNum);
                    if (cudaResult != ECudaResult.CudaSuccess)
                    {
                        throw CudaException.GetExceptionFromCudaResult(cudaResult);
                    }

                    // Efectua a cópia do primeiro vector para o dispositivo
                    var handle = GCHandle.Alloc(firstVector, GCHandleType.Pinned);
                    var size = Marshal.SizeOf(typeof(int));
                    var hostPtr = handle.AddrOfPinnedObject();

                    cudaResult = CudaApi.CudaMemcpyHtoD(
                        firstCudaVector,
                        hostPtr,
                        elemensNum * size);
                    if (cudaResult != ECudaResult.CudaSuccess)
                    {
                        throw CudaException.GetExceptionFromCudaResult(cudaResult);
                    }

                    handle.Free();

                    // Efectua a cópia do segundo vector para o dispositivo
                    handle = GCHandle.Alloc(secondVector, GCHandleType.Pinned);
                    hostPtr = handle.AddrOfPinnedObject();

                    cudaResult = CudaApi.CudaMemcpyHtoD(
                        secondCudaVector,
                        hostPtr,
                        elemensNum * size);
                    if (cudaResult != ECudaResult.CudaSuccess)
                    {
                        throw CudaException.GetExceptionFromCudaResult(cudaResult);
                    }

                    handle.Free();

                    // Envia os restantes argumentos para o dispositivo
                    //var integerSize = Marshal.SizeOf(typeof(int));
                    //var startArg = Marshal.AllocHGlobal(integerSize);
                    //Marshal.WriteInt32(startArg, start);
                    //var startArgDevPtr = default(SCudaDevicePtr);
                    //cudaResult = CudaApi.CudaMemAlloc(ref startArgDevPtr, integerSize);
                    //cudaResult = CudaApi.CudaMemcpyHtoD(startArgDevPtr, startArg, integerSize);
                    //Marshal.FreeHGlobal(startArg);

                    //var endArg = Marshal.AllocHGlobal(integerSize);
                    //Marshal.WriteInt32(endArg, elemensNum);
                    //var endArgDevPtr = default(SCudaDevicePtr);
                    //cudaResult = CudaApi.CudaMemAlloc(ref endArgDevPtr, integerSize);
                    //cudaResult = CudaApi.CudaMemcpyHtoD(endArgDevPtr, endArg, integerSize);
                    //Marshal.FreeHGlobal(endArg);

                    var arrayPtr = Utils.AllocUnmanagedPointersArray(
                        new[] { firstCudaVector, secondCudaVector, resultCudaVector });

                    // Realiza a chamada
                    cudaResult = CudaApi.CudaLaunchKernel(
                        cudaFunc.CudaFunction,
                        (uint)elemensNum,
                        1,
                        1,
                        1,
                        1,
                        1,
                        0,
                        new SCudaStream(),
                        arrayPtr.Item1,
                        IntPtr.Zero);

                    cudaResult = CudaApi.CudaCtxSynchronize();

                    // Liberta o conjunto de argumentos alocado
                    Utils.FreeUnmanagedArray(arrayPtr);

                    // Copia de volta o terceiro vector para o anfitrião
                    handle = GCHandle.Alloc(result, GCHandleType.Pinned);
                    hostPtr = handle.AddrOfPinnedObject();
                    cudaResult = CudaApi.CudaMemcpyDtoH(
                        hostPtr,
                        resultCudaVector,
                        size * elemensNum);
                    //if (cudaResult != ECudaResult.CudaSuccess)
                    //{
                    //    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                    //}

                    handle.Free();

                    // Liberta a memória alocada
                    cudaResult = CudaApi.CudaMemFree(firstCudaVector);
                    cudaResult = CudaApi.CudaMemFree(secondCudaVector);
                    cudaResult = CudaApi.CudaMemFree(resultCudaVector);
                    //cudaResult = CudaApi.CudaMemFree(startArgDevPtr);
                    //cudaResult = CudaApi.CudaMemFree(endArgDevPtr);

                    // Remove o módulo do contexto actual
                    cudaManager.UnloadModule(module);

                    // Descarta o contexto
                    context.Dispose();
                }
            }
            catch (CudaException cudaException)
            {
                Console.WriteLine("Ocorreu um erro CUDA: {0}", cudaException.Message);
            }
        }

        static void Temp1()
        {
            var upperBoundsFile = "Data\\Exemplos\\limites_superiores_20.csv";
            var lowerBoundsFile = "Data\\Exemplos\\limites_inferiores_20.csv";

            var comparer = Comparer<double>.Default;
            var ring = new DoubleField();
            var componentBoundsAlgorithm = new ComponentBoundsAlgorithm<double>(
                new IntegerMinWeightTdecomposition<double>(comparer, ring),
                comparer,
                ring);

            var doubleParser = new DoubleParser<string>();
            var csvParser = new CsvFileParser<List<List<double>>, double, string, string>(
                "new_line",
                "semi_colon",
                (i, j) => doubleParser);
            csvParser.AddIgnoreType("carriage_return");

            var upperBounds = new List<List<double>>();
            var lowerBounds = new List<List<double>>();

            var adder = new ListTypeTransposedAdder<double>();
            using (var textReader = new StreamReader(upperBoundsFile))
            {
                var symbolReader = new StringSymbolReader(textReader, true, false);
                csvParser.Parse(symbolReader, upperBounds, adder);
            }

            using (var textReader = new StreamReader(lowerBoundsFile))
            {
                var symbolReader = new StringSymbolReader(textReader, true, false);
                csvParser.Parse(symbolReader, lowerBounds, adder);
            }

            // Conta o número de vértices.
            var countRefs = 0;
            for (int i = 0; i < upperBounds.Count; ++i)
            {
                countRefs += upperBounds[i].Count;
            }

            var result = componentBoundsAlgorithm.Run(20, lowerBounds, upperBounds);
        }

        public void Lixo()
        {
            var watch = new Stopwatch();

            // Consulta que permite obter elementos repetidos numa lista de valores.
            var list = new List<int>();
            var i = 0;
            for (; i < 50000000; ++i)
            {
                list.Add(i);
            }

            list.Add(1);
            ++i;

            for (; i < 100000000; ++i)
            {
                list.Add(i);
            }

            watch.Start();
            //var dic = new HashSet<int>();
            //var contains = false;
            //var listCount = list.Count;
            //for (i = 0; i < listCount; ++i)
            //{
            //    if (dic.Contains(list[i]))
            //    {
            //        contains = true;
            //        i = listCount;
            //    }
            //    else
            //    {
            //        dic.Add(i);
            //    }
            //}

            watch.Stop();
            Console.WriteLine(
                "{0}:{1}:{2}",
                watch.Elapsed.Hours,
                watch.Elapsed.Minutes,
                watch.Elapsed.Seconds);
            //Console.WriteLine(contains);

            watch.Reset();
            watch.Start();
            var cont = false;
            var listLength = list.Count();

            var tasks = new Task[6];
            var dicList = new Queue<HashSet<int>>();
            var addStatus = true;
            var lockObject = new object();

            for (i = 0; i < 6; ++i)
            {
                tasks[i] = new Task(() =>
                {
                    var status = !cont;
                    while (status)
                    {
                        var first = default(HashSet<int>);
                        var second = default(HashSet<int>);
                        var requireStatus = false;
                        lock (lockObject)
                        {
                            if (dicList.Count > 1)
                            {
                                first = dicList.Dequeue();
                                second = dicList.Dequeue();
                                requireStatus = true;
                            }
                        }

                        if (requireStatus)
                        {
                            foreach (var key in second)
                            {
                                if (first.Contains(key))
                                {
                                    cont = true;
                                    status = false;
                                    break;
                                }
                                else
                                {
                                    first.Add(key);
                                }
                            }

                            if (!cont)
                            {
                                lock (lockObject)
                                {
                                    dicList.Enqueue(first);
                                }
                            }
                        }
                        else
                        {
                            status = !cont && addStatus;
                        }
                    }
                });

                tasks[i].Start();
            }

            Parallel.ForEach(Partitioner.Create(0, listLength), (obj, state) =>
            {
                var hashSet = new HashSet<int>();
                if (cont)
                {
                    state.Break();
                }
                else
                {
                    for (int j = obj.Item1; j < obj.Item2; ++j)
                    {
                        if (hashSet.Contains(list[j]))
                        {
                            cont = true;
                            lock (dicList)
                            {
                                dicList.Clear();
                            }

                            state.Break();
                        }
                        else
                        {
                            hashSet.Add(list[j]);
                        }
                    }

                    lock (lockObject)
                    {
                        dicList.Enqueue(hashSet);
                    }
                }
            });

            addStatus = false;
            Task.WaitAll(tasks);

            watch.Stop();
            Console.WriteLine(
                "{0}:{1}:{2}",
                watch.Elapsed.Hours,
                watch.Elapsed.Minutes,
                watch.Elapsed.Seconds);
            Console.WriteLine(cont);
        }

        static void TestOdmpCompatibilityAlgorithm()
        {
            var filePath = @"Data\Matrix5\Matrix5_0.dat";
            var medians = 1;
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                var integerParser = new IntegerParser<string>();
                var doubleParser = new DoubleParser<string>();
                var odmpReader = new OdmpSparseMatrixSetReader<int, int, int, double>(
                    doubleParser,
                    integerParser,
                    integerParser,
                    integerParser);

                using (var fileStream = fileInfo.OpenRead())
                {
                    var sparseMatrixSet = odmpReader.Read(fileStream);
                    var componentsList = new List<ILongSparseMathMatrix<double>>();
                    foreach (var odmpMatrix in sparseMatrixSet)
                    {
                        var firstLine = odmpMatrix.FirstOrDefault();
                        if (firstLine == null)
                        {
                            Console.WriteLine("Existe uma matriz sem linhas.");
                            break;
                        }
                        else
                        {
                            var linesNumber = firstLine.Last().Column + 1;
                            var matrix = new SparseDictionaryMathMatrix<double>(
                                linesNumber,
                                linesNumber,
                                double.MaxValue);
                            foreach (var line in odmpMatrix)
                            {
                                foreach (var column in line)
                                {
                                    if (line.Line != column.Column)
                                    {
                                        matrix[line.Line, column.Column] = column.Value;
                                    }
                                }
                            }

                            componentsList.Add(matrix);
                        }

                        var compatibilityAlgorithm = new OdmpCompatibilityGreedyAlgorithm<double>(
                            Comparer<double>.Default,
                            new DoubleField());
                        var result = compatibilityAlgorithm.Run(1, componentsList);
                        Console.WriteLine(result.Aggregate(0.0, (a, b) => a + b.Cost));
                        Console.WriteLine(
                            "Initial medians: {0}. Final Medians: {1}.",
                            medians,
                            result.Aggregate(0, (a, b) => a + b.Chosen.Count));
                    }
                }
            }
            else
            {
                Console.WriteLine("O ficheiro não existe: {0}", fileInfo.FullName);
            }
        }

        static void Example()
        {
            var filePath = @"..\..\Files\componente_0.txt";
            var labelsReader = new LabelsReader();
            using (var stream = File.OpenRead(filePath))
            {
                var labels = labelsReader.ReadLabels(stream, Encoding.ASCII);
            }
        }

        static void RunObjectTester()
        {
            var tester = new ObjectTester();
            tester.Run(Console.In, Console.Out);
        }

        /// <summary>
        /// Testes adicionais sobre a factorização de polinómios e elevação de factores.
        /// </summary>
        public static void Test20()
        {
            var integerDomain = new BigIntegerDomain();

            // Leitura do polinómio
            var polynomialReader = new BigIntFractionPolReader();
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

            //var polynom = polynomialReader.Read("(2*x+1)*(x+3)^2");
            //var polynom = polynomialReader.Read("(2*x+1)^2*(x+3)^3");
            var polynom = polynomialReader.Read("x^3+10*x^2-432*x+5040");

            var integerPolynomialAlg = new IntegerPolynomialFactorizationAlgorithm<BigInteger>(
                integerDomain,
                new ModularSymmetricBigIntFieldFactory(),
                new BigIntegerPrimeNumbersIteratorFactory(),
                new BigIntLogDoubleApproximationAlg());
            var integerPolFactAlgResult = integerPolynomialAlg.Run(polynom);

            var squareFreeFactorizationAlgorithm = new SquareFreeFractionFactorizationAlg<BigInteger>(
                    integerDomain);
            var squareFreeFactored = squareFreeFactorizationAlgorithm.Run(polynom);

            // Instanciação dos algoritmos
            var resultantAlg = new UnivarPolDeterminantResultantAlg<BigInteger>(new BigIntegerDomain());
            var primesGenerator = new BigIntPrimeNumbsIterator(int.MaxValue, new BigIntSquareRootAlgorithm());

            // Obtém o valor do coeficiente principal e do discriminante.
            //var leadingCoeff = polynom.GetLeadingCoefficient(integerDomain);
            //var discriminant = resultantAlg.Run(
            //    polynom,
            //    polynom.GetPolynomialDerivative(integerDomain));
            //var primesEnumerator = primesGenerator.GetEnumerator();
            var prime = integerDomain.MultiplicativeUnity;
            //var state = true;
            //while (state)
            //{
            //    if (primesEnumerator.MoveNext())
            //    {
            //        var innerPrime = primesEnumerator.Current;
            //        if (!integerDomain.IsAdditiveUnity(integerDomain.Rem(leadingCoeff, innerPrime)) &&
            //            !integerDomain.IsAdditiveUnity(integerDomain.Rem(discriminant, innerPrime)))
            //        {
            //            prime = innerPrime;
            //            state = false;
            //        }
            //    }
            //    else // Todos os primos gerados dividem pelo menos o coeficiente principal e o discriminante
            //    {
            //        Console.WriteLine("Foram esgotados todos os primos disponíveis sem encontrar um que não divida o coeficiente principal e o discriminante.");
            //        state = false;
            //    }
            //}

            // Temporário
            prime = 31;

            // Neste ponto estamos em condições de tentar factorizar o polinómio.
            if (prime > 1)
            {
                // Realiza a factorização.
                var integerModularField = new ModularSymmetricBigIntField(prime);

                // Instancia o algoritmo responsável pela factorização sobre corpos finitos.
                var finiteFieldFactorizationAlg = new FiniteFieldPolFactorizationAlgorithm<BigInteger>(
                    new DenseCondensationLinSysAlgorithm<BigInteger>(integerModularField),
                    integerDomain);

                // Instancia o algoritmo responsável pela elevação multi-factor.
                var modularFactory = new ModularSymmetricBigIntFieldFactory();
                var multiFactorLiftAlg = new MultiFactorLiftAlgorithm<BigInteger>(
                    new LinearLiftAlgorithm<BigInteger>(
                        modularFactory,
                        new UnivarPolEuclideanDomainFactory<BigInteger>(),
                        integerDomain));
                //var factored = finiteFieldFactorizationAlg.Run(polynom, integerModularField);
                var liftedFactors = new Dictionary<BigInteger, IList<UnivariatePolynomialNormalForm<BigInteger>>>();
                //foreach (var factorKvp in factored)
                //{
                //    var multiLiftStatus = new MultiFactorLiftingStatus<BigInteger>(
                //        factorKvp.Value.FactoredPolynomial,
                //        factorKvp.Value,
                //        prime);
                //    var liftResult = multiFactorLiftAlg.Run(multiLiftStatus, 2);
                //    Console.WriteLine("Módulo {0}.", liftResult.LiftingPrimePower);
                //    liftedFactors.Add(factorKvp.Key, liftResult.Factors);

                //    // Teste à fase de pesquisa
                //    var searchAlgorithm = new SearchFactorizationAlgorithm<BigInteger>(
                //        modularFactory,
                //        new BigIntegerDomain());

                //    //Determinar a estimativa
                //    var estimation = Math.Sqrt(factorKvp.Value.FactoredPolynomial.Degree + 1);
                //    var estimationIntegerPart = (1 << factorKvp.Value.FactoredPolynomial.Degree) * 
                //      polynom.GetLeadingCoefficient(integerDomain);
                //    var norm = integerDomain.AdditiveUnity;
                //    foreach (var term in factorKvp.Value.FactoredPolynomial)
                //    {
                //        var termValue = integerDomain.GetNorm(term.Value);
                //        if (integerDomain.Compare(termValue, norm) > 0)
                //        {
                //            norm = termValue;
                //        }
                //    }

                //    estimationIntegerPart = estimationIntegerPart * norm;
                //    var integerPartLog = (int)Math.Floor(BigInteger.Log(estimationIntegerPart) + 2);
                //    var estimationPower = Math.Pow(10, integerPartLog);
                //    var estimationMultiplication = (int)Math.Floor(estimationPower * estimation);
                //    estimationIntegerPart = (estimationIntegerPart * estimationMultiplication) / 
                //      estimationMultiplication;
                //    ++estimationIntegerPart;
                //    var searchResult = searchAlgorithm.Run(liftResult, estimationIntegerPart, 3);
                //}

                // Imprime os resultados para a consola.
                foreach (var liftFactor in liftedFactors)
                {
                    Console.WriteLine("Grau {0}", liftFactor.Key);
                    foreach (var factor in liftFactor.Value)
                    {
                        Console.WriteLine(factor);
                    }

                    var modularField = new ModularSymmetricBigIntField(31 * 31 * 31 * 31);
                    var polRing = new UnivarPolynomRing<BigInteger>("x", modularField);
                    var factorsEnumerator = liftFactor.Value.GetEnumerator();
                    if (factorsEnumerator.MoveNext())
                    {
                        var factor = factorsEnumerator.Current;
                        while (factorsEnumerator.MoveNext())
                        {
                            factor = polRing.Multiply(factor, factorsEnumerator.Current);
                        }

                        Console.WriteLine("Product: {0}", factor);
                    }

                    Console.WriteLine();
                }
            }
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
            //var path = "Data\\Exemplos\\Matriz_Exemplo.dat";
            var fileInfo = new FileInfo("..\\..\\Files\\Matrix.csv");
            if (fileInfo.Exists)
            {
                var parser = new NullableDoubleParser();
                var csvParser = new CsvFileParser<TabularListsItem, Nullable<double>, string, string>(
                    Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE),
                    Utils.GetStringSymbolType(EStringSymbolReaderType.SEMI_COLON),
                    (i, j) => parser);
                csvParser.AddIgnoreType(Utils.GetStringSymbolType(EStringSymbolReaderType.CARRIAGE_RETURN));

                using (var textReader = fileInfo.OpenText())
                {
                    var symbolReader = new StringSymbolReader(textReader, false, false);
                    var table = new TabularListsItem();

                    csvParser.Parse(symbolReader, table, new TabularItemAdder<Nullable<double>>());

                    var costs = new SparseDictionaryMathMatrix<Nullable<double>>(table.Count, table.Count, null);
                    for (int i = 0; i < table.Count; ++i)
                    {
                        var currentLine = table[i];
                        for (int j = 0; j < currentLine.Count; ++j)
                        {
                            costs[i, j] = currentLine[j].GetCellValue<Nullable<double>>();
                        }
                    }

                    var nullableDoubleField = new NullableDoubleField();
                    var integerDoubleConversion = new IntegerNullableDoubleConverter();
                    var precisionComparer = new PrecisionNullableDoubleComparer(0.000001);
                    var simplexAlgorithm = new SimplexAlgorithm<Nullable<double>>(
                        precisionComparer,
                        nullableDoubleField);
                    var linearRelax = new LinearRelaxationAlgorithm<Nullable<double>>(
                        simplexAlgorithm,
                        integerDoubleConversion,
                        nullableDoubleField);

                    var result = linearRelax.Run(costs, 5);

                    // p = 5
                    //var initialSolution = new Nullable<double>[table.Count];
                    //for (int i = 0; i < initialSolution.Length; ++i)
                    //{
                    //    initialSolution[i] = 0;
                    //}

                    //initialSolution[0] = 1;
                    //initialSolution[1] = 0.137905;
                    //initialSolution[4] = 0.262507;
                    //initialSolution[5] = 0.104513;
                    //initialSolution[8] = 0.337386;
                    //initialSolution[9] = 0.084155;
                    //initialSolution[12] = 0.332995;
                    //initialSolution[16] = 0.143789;
                    //initialSolution[20] = 0.121665;
                    //initialSolution[24] = 0.0851576;
                    //initialSolution[64] = 0.266743;
                    //initialSolution[65] = 0.047304;
                    //initialSolution[68] = 0.271134;
                    //initialSolution[72] = 0.395872;
                    //initialSolution[80] = 0.0395245;
                    //initialSolution[128] = 0.106906;
                    //initialSolution[129] = 0.0313958;
                    //initialSolution[132] = 0.0663087;
                    //initialSolution[136] = 0.0560474;
                    //initialSolution[144] = 0.0250102;
                    //initialSolution[192] = 0.0264022;
                    //initialSolution[257] = 0.106054;
                    //initialSolution[260] = 0.0671126;
                    //initialSolution[272] = 0.105553;
                    //initialSolution[320] = 0.137139;
                    //initialSolution[384] = 0.140151;
                    //initialSolution[512] = 0.449401;
                    //initialSolution[513] = 0.134202;
                    //initialSolution[516] = 0.288092;
                    //initialSolution[517] = 0.114046;
                    //initialSolution[520] = 0.213213;
                    //initialSolution[521] = 0.303104;
                    //initialSolution[524] = 0.1657;
                    //initialSolution[528] = 0.134703;
                    //initialSolution[532] = 0.102779;
                    //initialSolution[536] = 0.314371;
                    //initialSolution[576] = 0.283856;
                    //initialSolution[577] = 0.184431;
                    //initialSolution[580] = 0.156918;
                    //initialSolution[588] = 1;
                    //initialSolution[592] = 0.198095;
                    //initialSolution[640] = 0.14019;
                    //initialSolution[644] = 0.121252;
                    //initialSolution[648] = 0.295898;
                    //initialSolution[704] = 0.152219;
                    //initialSolution[768] = 0.159801;
                    //initialSolution[769] = 0.0126368;
                    //initialSolution[772] = 0.413599;
                    //initialSolution[784] = 0.00675249;
                    //initialSolution[832] = 0.152461;
                    //initialSolution[896] = 0.00355069;

                    //var correction = new LinearRelRoundCorrectorAlg<Nullable<double>>(
                    //    Comparer<Nullable<double>>.Default,
                    //    new IntegerNullableDoubleConverter(0.00001),
                    //    new NullableIntegerNearest(),
                    //    new NullableDoubleField());
                    //var result = correction.Run(initialSolution, costs, 2);
                    //Console.WriteLine("Medianas:");
                    //foreach (var chose in result.Chosen)
                    //{
                    //    Console.WriteLine(chose);
                    //}

                    //Console.WriteLine("Custo: {0}", result.Cost);
                }
            }
            else
            {
                Console.WriteLine("O camminho fornecido não existe.");
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

            var primeNumberEnumerator = new IntPrimeNumbersIterator(100, new IntegerSquareRootAlgorithm());
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
            var thirdInput = "(x^2+3*x+2)*(x^2-4*x+3)^3";
            var fourthInput = "x^8+x^6-3*x^4-3*x^3+8*x^2+2*x-5";
            var fifthInput = "3*x^6+5*x^4-4*x^2-9*x+21";

            var integerDomain = new IntegerDomain();
            var bigIntegerDomain = new BigIntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);
            var integerParser = new IntegerParser<string>();
            var bigIntegerParser = new BigIntegerParser<string>();
            var fractionParser = new ElementFractionParser<int>(integerParser, integerDomain);

            var bigIntFractionPolReader = new BigIntFractionPolReader();

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

            var reader = new StringReader(thirdInput);
            var stringSymbolReader = new StringSymbolReader(reader, false);
            var otherFractionParser = new ElementFractionParser<BigInteger>(bigIntegerParser, bigIntegerDomain);
            var otherFractionField = new FractionField<BigInteger>(bigIntegerDomain);
            var otherPolParser = new UnivariatePolynomialReader<
                Fraction<BigInteger>,
                CharSymbolReader<string>>(
                "x",
                otherFractionParser,
                otherFractionField);
            var otherFractionConversion = new BigIntegerFractionToIntConversion();
            var thirdPol = default(UnivariatePolynomialNormalForm<Fraction<BigInteger>>);
            if (otherPolParser.TryParsePolynomial(stringSymbolReader, otherFractionConversion, out thirdPol))
            {
                var univarSquareFreeAlg = new UnivarSquareFreeDecomposition<Fraction<BigInteger>>();
                var result = univarSquareFreeAlg.Run(thirdPol, otherFractionField);
                Console.WriteLine("The squarefree factors are:");
                foreach (var factor in result.Factors)
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
            var fractionField = new FractionField<int>(integerDomain);
            var fractionParser = new FieldDrivenExpressionParser<Fraction<int>>(
                new SimpleElementFractionParser<int>(integerParser, integerDomain),
                fractionField);
            var firstVector = default(IMathVector<Fraction<int>>);
            var secondVector = default(IMathVector<Fraction<int>>);
            var vectorFactory = new ArrayVectorFactory<Fraction<int>>();

            var vectorReader = new ConfigVectorReader<Fraction<int>, string, string, CharSymbolReader<string>>(
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
                    var vectorSpace = new VectorSpace<Fraction<int>>(
                        2,
                        vectorFactory,
                        fractionField);

                    var scalarProd = new OrthoVectorScalarProduct<Fraction<int>>(
                        new FractionComparer<int>(Comparer<int>.Default, integerDomain),
                        fractionField);

                    var thirdVector = new ArrayMathVector<Fraction<int>>(2);
                    thirdVector[0] = new Fraction<int>(1, 1, integerDomain);
                    thirdVector[1] = new Fraction<int>(1, 1, integerDomain);
                    var generator = new VectorSpaceGenerator<Fraction<int>>(2);
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

                    var fractionComparer = new FractionComparer<int>(
                        Comparer<int>.Default,
                        integerDomain);
                    var nearest = new FractionNearestInteger(integerDomain);
                    var lllReductionAlg = new LLLBasisReductionAlgorithm<IMathVector<Fraction<int>>,
                                                                         Fraction<int>,
                                                                         int>(
                           vectorSpace,
                           scalarProd,
                           nearest,
                           new FractionComparer<int>(Comparer<int>.Default, integerDomain));

                    var lllReduced = lllReductionAlg.Run(
                        new IMathVector<Fraction<int>>[] { firstVector, secondVector },
                        new Fraction<int>(4, 3, integerDomain));

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

            var sparseDictionaryMatrix = new SparseDictionaryMathMatrix<int>(10, 10);
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
            var arrayReader = new ConfigMatrixReader<
                UnivariatePolynomialNormalForm<int>, IMathMatrix<UnivariatePolynomialNormalForm<int>>,
                string, string>(
                2,
                2);
            arrayReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayReader.AddBlanckSymbolType("blancks");
            arrayReader.SeparatorSymbType = "comma";

            var readed = default(IMathMatrix<UnivariatePolynomialNormalForm<int>>);
            if (arrayReader.TryParseMatrix(inputReader, univariatePolParser, (i, j) => arrayMatrixFactory.CreateMatrix(i, j), out readed))
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
            var errors = new LogStatus<string, EParseErrorLevel>();
            var elementConversion = new ElementToElementConversion<int>();
            if (polynomialParser.TryParsePolynomial(inputReader, elementConversion, errors, out readed))
            {
                Console.WriteLine(readed);
            }
            else
            {
                Console.WriteLine("Errors parsing polynomial:");
                foreach (var message in errors.GetLogs(EParseErrorLevel.ERROR))
                {
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
            var arrayMatrixReader = new ConfigMatrixReader<int, ArrayMathMatrix<int>, string, string>(
                3,
                3);
            arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayMatrixReader.AddBlanckSymbolType("blancks");
            arrayMatrixReader.SeparatorSymbType = "comma";

            var matrix = default(ArrayMathMatrix<int>);
            var errors = new LogStatus<string, EParseErrorLevel>();
            if (arrayMatrixReader.TryParseMatrix(
                stringsymbolReader,
                integerParser,
                errors,
                (i, j) => new ArrayMathMatrix<int>(i, j),
                out matrix))
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
                foreach (var message in errors.GetLogs())
                {
                    Console.WriteLine("Errors parsing range:");
                    foreach (var innerMessage in message.Value)
                    {
                        Console.WriteLine(innerMessage);
                    }
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

            var rangeNoConfig = new RangeNoConfigReader<int, string, string>();
            rangeNoConfig.MapInternalDelimiters("left_bracket", "right_bracket");
            rangeNoConfig.AddBlanckSymbolType("blancks");
            rangeNoConfig.SeparatorSymbType = "comma";

            var multiDimensionalRangeReader = new MultiDimensionalRangeReader<int, string, string>(rangeNoConfig);
            var range = default(MultiDimensionalRange<int>);
            var errors = new LogStatus<string, EParseErrorLevel>();
            if (multiDimensionalRangeReader.TryParseRange(stringsymbolReader, integerParser, errors, out range))
            {
                var config = new int[][] { new int[] { 1, 1, 4, 3 }, new int[] { 0, 1, 0, 1 } };
                var subRange = range.GetSubMultiDimensionalRange(config);
                Console.WriteLine(subRange);
            }
            else
            {
                Console.WriteLine("Errors parsing range:");
                foreach (var message in errors.GetLogs(EParseErrorLevel.ERROR))
                {
                    Console.WriteLine(message);
                }
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Teste aos afectadores.
        /// </summary>
        static void Test3()
        {
            var swapIndicesGenerator = new PermutationAffector(10);
            foreach (var vector in swapIndicesGenerator)
            {
                Console.WriteLine(PrintVector(vector));
            }

            // Permite encontrar todas as permutações dos índices 0, 1 e 2 podendo estes serem repetidos tantas vezes
            // quantas as indicadas: 0 - repete 2 vezes, 1 - repete 2 vezes, 2 - repete 2 vezes.
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