using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Transactions;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using SubEntityProject;
//using PTC.Utilities;
using System.Configuration;
using System.Management;
using System.Xml;
using System.Net;

namespace Apagame
{
    class Program
    {
        public static void Main(string[] args)
        {
            var fileName = @"C:\Users\sergio.marques\Desktop\Lib\Daimler\E3Export_complete.xml";
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                using (var readStream = fileInfo.OpenRead())
                {
                    var xmlReader = XmlReader.Create(readStream);
                    var multichannelReader = new UnorderedMutlichannelReader(
                        xmlReader,
                        "root");

                    // A série de veículos
                    var document = multichannelReader.RegisterDocumentReader(
                        "Baureihen",
                        new BaureiheFactory());
                    RegisterBennenung<Baureihe>(document, "baureihe");

                    // A cor
                    document = multichannelReader.RegisterDocumentReader(
                        "Farben",
                        new FarbeFactory());
                    var farbeElementReader = RegisterBennenung<Farbe>(document, "farbe");
                    farbeElementReader.RegisterElementReader<Farbe>(
                        (f, t) => ((Farbe)f).Kurzezeichen = t, f => (Farbe)f,
                        "kurzzeichen",
                        true, true, true,
                        0, 1);

                    // A combinação de cores
                    document = multichannelReader.RegisterDocumentReader(
                        "Farbkombinationen",
                        new FarbkombinationFactory());
                    var farbekombinationenElementReader = RegisterGerneralId<Farbkombination>(
                        document,
                        "farbkombination");
                    farbekombinationenElementReader.RegisterElementReader<Farbkombination>(
                        (e, t) => ((Farbkombination)e).Bezeichnung = t, f => (Farbkombination)f,
                        "bezeichnung",
                        true, true, true,
                        0, 1);
                    var nummerFarbe = farbekombinationenElementReader.RegisterElementReader<Farbkombination>(
                        (e,t)=>{}, f=>(Farbkombination)f,
                        "grundfarbe",
                        true,true,true,
                        0,1);
                    var farbeRefElementreader = nummerFarbe.RegisterElementReader<Farbkombination>(
                        (e, t) => { }, f => (Farbkombination)f,
                        "farbeRef",
                        true, true, true,
                        0, int.MaxValue);
                    farbeElementReader.AttributesReader.RegisterAttribute("", (f, t) => ((Farbkombination)f).GrundfarbeId = t, false);
                    nummerFarbe = farbekombinationenElementReader.RegisterElementReader<Farbkombination>(
                        (e, t) => { }, f => (Farbkombination)f,
                        "zweitfarbe",
                        true, true, true,
                        0, 1);
                    farbeRefElementreader = nummerFarbe.RegisterElementReader<Farbkombination>(
                        (e, t) => { }, f => (Farbkombination)f,
                        "farbeRef",
                        true, true, true,
                        0, int.MaxValue);
                    farbeElementReader.AttributesReader.RegisterAttribute("", (f, t) => ((Farbkombination)f).ZweitfarbeId = t, false);

                    nummerFarbe = farbekombinationenElementReader.RegisterElementReader<Farbkombination>(
                        (e, t) => { }, f => (Farbkombination)f,
                        "drittfarbe",
                        true, true, true,
                        0, 1);
                    farbeRefElementreader = nummerFarbe.RegisterElementReader<Farbkombination>(
                        (e, t) => { }, f => (Farbkombination)f,
                        "farbeRef",
                        true, true, true,
                        0, int.MaxValue);
                    farbeElementReader.AttributesReader.RegisterAttribute("", (f, t) => ((Farbkombination)f).DrittfarbeId = t, false);



                    while (multichannelReader.ReadNext())
                    {
                        Console.WriteLine("Channel: {0}", multichannelReader.Current.Item1);
                        var current = (Baureihe)multichannelReader.Current.Item2;
                        Console.WriteLine("Name: {0}", current.Name);
                    }
                }
            }
            else
            {
                Console.WriteLine(string.Format(
                    "O ficheiro não existe: {0}",
                    fileInfo.FullName));
            }
        }

        static IXmlElementReader<object, T> RegisterGerneralId<T>(
            UnorderedDocumentReader<object> document,
            string seriesItemElementName) where T : GenId
        {
            var result = (IXmlElementReader<object,T>)document.RegisterElementReader<T>(
                (o, t) => { }, o => (T)o,
                seriesItemElementName,
                true, true, true);
            result.AttributesReader.RegisterAttribute("id", (a, t) => ((T)a).Id = t, true);
            result.RegisterElementReader<T>(
                (e, t) => ((T)e).Uid = t, e => (T)e,
                "uid",
                true, true, true,
                0, 1);
            return result;
        }

        static IXmlElementReader<object, T> RegisterBennenung<T>(
            UnorderedDocumentReader<object> document,
            string seriesItemName) where T : GenBenennung
        {
            var result = RegisterGerneralId<T>(document, seriesItemName);
            var elementReader = result.RegisterElementReader<T>(
                (o, t) => { }, b => (T)b,
                "benennung",
                true, true, true,
                0, 1);
            var itemReader = elementReader.RegisterElementReader<T>(
                        (b, t) => { }, b => (T)b,
                        "item",
                        true, true, true,
                        0, int.MaxValue);
            itemReader.RegisterElementReader<T>(
                        (b, t) => ((T)b).SetLanguage(t), b => (T)b,
                        "language",
                        true, true, true,
                        0, int.MaxValue);
            itemReader.RegisterElementReader<T>(
                (b, t) => ((T)b).SetValueBenennungValue(t), b => (T)b,
                "value",
                true, true, true,
                0, int.MaxValue);
            return result;
        }

        static void SortTest()
        {
            var sorter = new MergeSort<int>();
            var vector = new int[] { 3, 2, 4, 1, 5, 0, 1, 3, 9, 1, 9, 2, 4, 6, 7, 34, 56, 6, 3 };
            var secondVector = new int[vector.Length];
            Array.Copy(vector, secondVector, vector.Length);
            var count = sorter.SortCountSwaps(vector, Comparer<int>.Default);
            var bubleSorter = new BubleSort<int>();
            var bubleCount = sorter.SortCountSwaps(secondVector, Comparer<int>.Default);
        }

        static void TesteClient()
        {
            var webClientTest = new WebClientTest();
            var responseHeaders = new Dictionary<string, string>();
            var response = webClientTest.Get();
            Console.WriteLine("Response headers:");
            foreach (var kvp in responseHeaders)
            {
                Console.WriteLine("{0} => {1}", kvp.Key, kvp.Value);
            }

            // Envia o resultado para um ficheiro
            using (var fileWriter = new StreamWriter(@"C:\Users\sergio.marques\Desktop\lixo.html"))
            {
                fileWriter.Write(response);
            }
        }

        static string TrimDelimiteresAndSpaces(string value)
        {
            return value.Trim('"').Trim();
        }

        static void ImdsTemp1()
        {
            var imdsFile = @"C:\Users\sergio.marques\Desktop\Trabalho\IMDS\MDSs accepted.csv";
            var imdsFileInfo = new FileInfo(imdsFile);
            if (imdsFileInfo.Exists)
            {
                using (var fileStream = imdsFileInfo.OpenRead())
                {
                    var fileReader = new StreamReader(
                        fileStream,
                        Encoding.UTF8);
                    var line = fileReader.ReadLine();
                    if (line != null)
                    {
                        var exploded = ExplodeString(line, ';', '"');
                        var header = exploded;
                        var headerMaximum = new int[exploded.Length];
                        var maximumValue = new string[exploded.Length];
                        var headerCount = exploded.Length;
                        var lineNumber = 1;
                        Console.WriteLine("Número de colunas: {0}", headerCount);
                        line = fileReader.ReadLine();
                        while (line != null)
                        {
                            ++lineNumber;
                            exploded = ExplodeString(line, ';', '"');
                            if (exploded.Length != headerCount)
                            {
                                Console.WriteLine("Número errado de colunas na linha {0}: {1}", lineNumber, line.Length);
                                Console.WriteLine(line);
                                return;
                            }
                            else
                            {
                                for (int i = 0; i < headerCount; ++i)
                                {
                                    var current = TrimDelimiteresAndSpaces(exploded[i]);
                                    var currentLength = current.Length;
                                    if (currentLength > headerMaximum[i])
                                    {
                                        headerMaximum[i] = currentLength;
                                        maximumValue[i] = current;
                                    }
                                }
                            }

                            line = fileReader.ReadLine();
                        }

                        for (int i = 0; i < headerCount; ++i)
                        {
                            Console.WriteLine("{0}: {1}; {2}", header[i], headerMaximum[i], maximumValue[i]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ficheiro vazio: nenhuma linha encontrada.");
                    }
                }
            }
            else
            {
                Console.WriteLine("O ficheiro não existe: {0}", imdsFileInfo.FullName);
            }
        }

        static void AslTemp1()
        {
            var csvFileName = @"C:\Users\sergio.marques\Documents\Visual Studio 2010\Projects\Apagame\bin\Debug\Testes\temp.csv";
            var siteReportName = @"C:\Users\sergio.marques\Documents\Visual Studio 2010\Projects\Apagame\bin\Debug\Testes\AFU_site_report.csv";

            var siteReportFileInfo = new FileInfo(siteReportName);
            if (siteReportFileInfo.Exists)
            {
                using (var siteReportFileStream = siteReportFileInfo.OpenRead())
                {
                    var siteReportFileReader = new StreamReader(
                        siteReportFileStream,
                        Encoding.UTF8);
                    var current = 1;
                    var line = siteReportFileReader.ReadLine();
                    while (line != null)
                    {
                        var exploded = ExplodeString(line, ';', '"');
                        if (exploded.Length != 11)
                        {
                            Console.WriteLine("Número de itens na linha {0} : {1}", current, exploded.Length);
                        }

                        ++current;
                        line = siteReportFileReader.ReadLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("O ficheiro não existe: {0}", siteReportFileInfo.FullName);
            }

            var csvFileInfo = new FileInfo(csvFileName);
            if (csvFileInfo.Exists)
            {
                using (var csvFileStream = csvFileInfo.OpenRead())
                {
                    var streamReader = new StreamReader(
                        csvFileStream,
                        Encoding.UTF8);
                    var line = streamReader.ReadLine();
                    while (line != null)
                    {
                        var exploded = ExplodeStringV1(line, ';', '"');
                        line = streamReader.ReadLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("O ficheiro não existe: {0}", csvFileInfo.FullName);
            }
        }

        static void SeineTemp1()
        {
            var folderPath = @"C:\Users\sergio.marques\Desktop\Testes\SEINE\cims\diff";
            var directoryInfo = new DirectoryInfo(folderPath);
            if (directoryInfo.Exists)
            {
                var files = directoryInfo.GetFiles("CIMS-LIMITED-1.20150402*.xml");
                if (files.Length == 0)
                {
                    Console.WriteLine("Nenhum ficheiro foi encontrado.");
                }
                else
                {
                    Console.WriteLine("Número de ficheiros: {0}", files.Length);
                }
            }
            else
            {
                Console.WriteLine("O caminho não existe: {0}", folderPath);
            }
        }
        static void Temp4()
        {
            var fileName = @"Testes\VW_Global_10_09_2014.xml";
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(fileStream);

                    var rootNode = xmlDocument.DocumentElement;
                    foreach (XmlNode node in rootNode.ChildNodes)
                    {
                        if (node.NodeType != XmlNodeType.Element)
                        {
                            Console.WriteLine(node.NodeType);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found: {0}", fileInfo.FullName);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void TextXml2()
        {
            var fileName = @"Testes\MAIN BODY RHD CONVERTIBLE.xml";
            var fileName1 = @"Testes\simple.xml";
            var copied = @"Testes\copy_simple.xml";
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.XmlResolver = null;
                    settings.DtdProcessing = DtdProcessing.Parse;

                    using (var xmlReader = XmlReader.Create(fileStream, settings))
                    {
                        var copyFileInfo = new FileInfo(copied);
                        using (var writeStream = copyFileInfo.OpenWrite())
                        {
                            var xmlWriter = XmlWriter.Create(writeStream);
                            while (xmlReader.Read())
                            {
                                switch (xmlReader.NodeType)
                                {
                                    case XmlNodeType.XmlDeclaration:
                                        xmlWriter.WriteStartDocument();
                                        break;
                                    case XmlNodeType.DocumentType:
                                        var pubid = string.Empty;
                                        var sysid = string.Empty;
                                        if (xmlReader.MoveToAttribute("PUBLIC"))
                                        {
                                            pubid = xmlReader.Value;
                                        }

                                        if (xmlReader.MoveToAttribute("SYSTEM"))
                                        {
                                            sysid = xmlReader.Value;
                                        }

                                        xmlWriter.WriteDocType(xmlReader.Name, pubid, sysid, string.Empty);

                                        break;
                                    case XmlNodeType.CDATA:
                                        xmlWriter.WriteCData(xmlReader.Value);
                                        break;
                                    case XmlNodeType.Element:
                                        xmlWriter.WriteStartElement(xmlReader.Name);
                                        if (xmlReader.MoveToFirstAttribute())
                                        {
                                            xmlWriter.WriteStartAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
                                            xmlWriter.WriteValue(xmlReader.Value);
                                            xmlWriter.WriteEndAttribute();

                                            while (xmlReader.MoveToNextAttribute())
                                            {
                                                xmlWriter.WriteStartAttribute(xmlReader.Prefix, xmlReader.LocalName, xmlReader.NamespaceURI);
                                                xmlWriter.WriteValue(xmlReader.Value);
                                                xmlWriter.WriteEndAttribute();
                                            }
                                        }

                                        break;
                                    case XmlNodeType.EndElement:
                                        xmlWriter.WriteEndElement();
                                        break;
                                    case XmlNodeType.Comment:
                                        xmlWriter.WriteComment(xmlReader.Value);
                                        break;
                                    case XmlNodeType.Text:
                                        break;
                                    case XmlNodeType.Document:
                                        break;
                                    case XmlNodeType.EntityReference:
                                        xmlWriter.WriteEntityRef(xmlReader.Name);
                                        break;
                                    case XmlNodeType.ProcessingInstruction:
                                        xmlWriter.WriteProcessingInstruction(xmlReader.Name, xmlReader.Value);
                                        break;
                                }
                            }

                            xmlWriter.WriteEndDocument();
                            xmlWriter.Close();
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found: {0}", fileInfo.FullName);
            }
        }

        static void TestXml1()
        {
            var fileName = @"Testes\VW Eng. Library_.xml";
            var outputFileName = @"Testes\output.xml";
            var regex = new Regex("^[a-zA-Z0-9]+$");
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(fileStream);

                    var symbols = xmlDocument.SelectNodes("//symbollib/symbol/@name");
                    foreach (var name in symbols)
                    {
                        var attribute = (XmlAttribute)name;
                        var attributeValue = attribute.Value;
                        Console.WriteLine(attributeValue);
                        if (regex.IsMatch(attributeValue))
                        {
                            var pointer = 0;
                            var count = 0;
                            var group = 0;
                            var length = attributeValue.Length;
                            var newAttribute = string.Empty;
                            while (pointer < length)
                            {
                                var currentChar = attributeValue[pointer];
                                newAttribute += currentChar;
                                ++count;
                                if (group < 2)
                                {
                                    if (count == 4)
                                    {
                                        ++group;
                                        if (pointer < length - 1)
                                        {
                                            newAttribute += "-";
                                        }

                                        count = 0;
                                    }
                                }

                                ++pointer;
                            }

                            attribute.Value = newAttribute;
                        }
                    }

                    var outputFileInfo = new FileInfo(outputFileName);
                    using (var outputStream = outputFileInfo.OpenWrite())
                    {
                        var writer = new StreamWriter(outputStream, Encoding.UTF8);
                        xmlDocument.Save(writer);
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found: {0}", fileInfo.FullName);
            }
        }

        static String[] ExplodeString(string line, char separator = ',', char delimiter = '"')
        {
            var result = new List<string>();
            if (line != null)
            {
                var position = 0;
                var state = 0;
                var buffered = string.Empty;
                while (state != -1)
                {
                    if (position < line.Length)
                    {
                        var current = line[position++];
                        if (current == separator)
                        {
                            if (state == 0)
                            {
                                result.Add(buffered);
                                buffered = string.Empty;
                            }
                            else
                            {
                                buffered += current;
                            }
                        }
                        else if (current == delimiter)
                        {
                            if (state == 0)
                            {
                                buffered += current;
                                state = 1;
                            }
                            else
                            {
                                buffered += current;
                                state = 0;
                            }
                        }
                        else
                        {
                            buffered += current;
                        }
                    }
                    else
                    {
                        result.Add(buffered);
                        state = -1;
                    }
                }
            }

            return result.ToArray();
        }

        private static String[] ExplodeStringV1(string line, char separator, char delimiter)
        {
            var result = new List<string>();
            var position = 0;
            var state = 0;
            var buffered = string.Empty;

            // Leitura do primeiro element
            while (state != -1)
            {
                if (position < line.Length)
                {
                    var current = line[position++];
                    if (current == separator)
                    {
                        result.Add(buffered);
                        buffered = string.Empty;
                        state = -1;
                    }
                    else
                    {
                        buffered += current;
                    }
                }
                else
                {
                    return result.ToArray();
                }
            }

            state = 0;
            while (state != -1)
            {
                if (position < line.Length)
                {
                    var current = line[position++];
                    if (current == separator)
                    {
                        if (state == 1)
                        {
                            state = 2;
                        }
                        else
                        {
                            buffered += current;
                            state = 0;
                        }
                    }
                    else if (current == delimiter)
                    {
                        if (state == 2)
                        {
                            result.Add(buffered);
                            buffered = "\"";
                            state = 0;
                        }
                        else
                        {
                            buffered += current;
                            state = 1;
                        }
                    }
                    else
                    {
                        buffered += current;
                        state = 0;
                    }
                }
                else
                {
                    result.Add(buffered);
                    state = -1;
                }
            }

            return result.ToArray();
        }

        static void temp3()
        {
            ManagementObjectSearcher searcher =
            new ManagementObjectSearcher(
            "root\\CIMV2",
            "select * from Win32_PerfFormattedData_PerfOS_Processor",
            new EnumerationOptions(
            null, System.TimeSpan.MaxValue,
            1, true, false, true,
            true, false, true, true));


            var count = searcher.Get().Count;

            var cpuTimes = searcher.Get()
               .Cast<ManagementObject>()
               .Select(mo => new
               {
                   Name = mo["Name"],
                   Usage = mo["PercentProcessorTime"]
               }
               )
               .ToList();

            var query = cpuTimes.Where(x => x.Name.ToString() == "_Total").Select(x => x.Usage);
            var cpuUsage = query.SingleOrDefault();

            Console.WriteLine(cpuUsage);
        }

        static void Temp2()
        {
            var config = (ServiceConfigurationSection)ConfigurationManager.GetSection(
                "ServiceGroup/ServiceConfiguration");
            //Console.ReadKey();

            var folder = config.PluginDirectory.Directory;

            var serviceHost = new ServiceHost(typeof(TestService));
            serviceHost.Open();

            Console.WriteLine(folder);
            Console.ReadKey();
            serviceHost.Close();
        }

        static void Temp1()
        {
            var mainTwistWireKindRegularExpression = new Regex(@"^\s*([Tt]{1}[Ww]{0,1}[0-9]+)(?:/([0-9]+)(?:\s+(.[0-9]+)){0,1}){0,1}\s*$");
            Console.Write("Teste à expressão regular (sair - pára o programa): ");
            var readedText = Console.ReadLine();
            while (readedText != "sair")
            {
                var match = mainTwistWireKindRegularExpression.Match(readedText);
                if (match.Success)
                {
                    Console.WriteLine("O texo é válido de acordo com a expressão. Os grupos são:");
                    for (int i = 0; i < match.Groups.Count; ++i)
                    {
                        Console.WriteLine(match.Groups[i].Value);
                    }
                }
                else
                {
                    Console.WriteLine("O texto não é válido de acordo com a expressão.");
                }

                Console.Write("Teste à expressão regular (sair - pára o programa): ");
                readedText = Console.ReadLine();
            }
        }

        static void Temp()
        {
            var regex = new Regex("");
            // TestDynamicAssembly();
            //var blockingCollection = new BlockingCollection<int>();
            //Task.Factory.StartNew(() =>
            //{
            //    for (int i = 0; i < 100; ++i)
            //    {
            //        blockingCollection.Add(i);
            //    }

            //    blockingCollection.CompleteAdding();
            //});

            //foreach (var item in blockingCollection.GetConsumingEnumerable())
            //{
            //    Console.WriteLine(blockingCollection.Count);
            //}

            //var comparer = new DsiCaeOptionsComparer();
            //var comp = comparer.CompareDsiCaeOptions("!(B/A)", "(A+B)-");
            //Console.WriteLine(comp);
        }

        static void TestDynamicAssembly()
        {
            var appDomain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName("TestAssembly");
            var assemblyBuilder = appDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("TestModule");
            var typeBuilder = moduleBuilder.DefineType("TestClass", TypeAttributes.Public);

            var methodBuilder = typeBuilder.DefineMethod("Sum", MethodAttributes.Public);
            methodBuilder.SetReturnType(typeof(int));
            methodBuilder.SetParameters(typeof(int), typeof(int));

            var ilGen = methodBuilder.GetILGenerator();
            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.Emit(OpCodes.Ldarg_2);
            ilGen.Emit(OpCodes.Add);
            ilGen.Emit(OpCodes.Ret);

            var dynamicType = typeBuilder.CreateType();

            var instance = dynamicType.GetConstructor(new Type[] { }).Invoke(new object[] { });
            var method = dynamicType.GetMethod("Sum");
            var result = method.Invoke(instance, new object[] { 1, 2 });

            Console.WriteLine(result);
        }

        static void TestesLambdas()
        {
            var stopWatch = new Stopwatch();
            var temp = "Um texto mesmo muito longo é mais difícil de analisar. No entanto, com textos pequenos talvez seja possível chegar à mesma conclusão.";
            var startIndex = 4;
            var length = 19;
            stopWatch.Start();
            var subs = temp.Substring(startIndex, length);
            stopWatch.Stop();
            Console.WriteLine("Tempo para '{0}': {1}", subs, stopWatch.Elapsed);

            stopWatch.Reset();
            stopWatch.Start();
            subs = string.Empty;
            var last = startIndex + length;
            for (int i = startIndex; i < last; ++i)
            {
                subs += temp[i];
            }

            stopWatch.Stop();
            Console.WriteLine("Tempo para cópia de '{0}': {1}", subs, stopWatch.Elapsed);

            var alunos = ListaDeClasses();
            stopWatch.Reset();
            stopWatch.Start();
            var queryAlunos = alunos.Where(a => a.Age == 10).Where(a => a.Name.Contains("an")).Where(a => a.Address.Contains("Flores"));
            foreach (var aluno in queryAlunos)
            {
                Console.WriteLine(aluno);
            }

            stopWatch.Stop();
            Console.WriteLine("Time: {0}", stopWatch.Elapsed);

            stopWatch.Reset();
            stopWatch.Start();
            var outra = alunos.Where(a => a.Age == 10);
            outra = outra.Where(a => a.Name.Contains("an"));
            outra = outra.Where(a => a.Address.Contains("Flores"));
            foreach (var aluno in outra)
            {
                Console.WriteLine(aluno);
            }

            stopWatch.Stop();
            Console.WriteLine("Time: {0}", stopWatch.Elapsed);

            stopWatch.Reset();
            stopWatch.Start();
            var filtros = Filtros();
            IEnumerable<TestClass> enumAlunos = alunos;
            for (int i = 0; i < filtros.Count; ++i)
            {
                var filtro = filtros[i];
                if (filtro.Tipo == 1)
                {
                    enumAlunos = enumAlunos.Where(a => a.Age.ToString() == filtro.Value);
                }
                else if (filtro.Tipo == 2)
                {
                    enumAlunos = enumAlunos.Where(a => a.Name.Contains(filtro.Value));
                }
                else if (filtro.Tipo == 3)
                {
                    enumAlunos = enumAlunos.Where(a => a.Address.Contains(filtro.Value));
                }
                else
                {
                    Console.WriteLine("Tipo de filtro {0} não reconhecido.", filtro.Tipo);
                }
            }

            foreach (var aluno in enumAlunos)
            {
                Console.WriteLine(aluno);
            }

            stopWatch.Stop();
            Console.WriteLine("Time: {0}", stopWatch.Elapsed);

            stopWatch.Reset();
            stopWatch.Start();
            var expressao = ConstroiExpressaoDosFiltros(filtros);
            stopWatch.Stop();
            Console.WriteLine("Tempo da construção da expressão lambda: {0}", stopWatch.Elapsed);

            stopWatch.Reset();
            stopWatch.Start();
            var expResultado = alunos.Where(expressao);
            foreach (var aluno in expResultado)
            {
                Console.WriteLine(aluno);
            }

            stopWatch.Stop();
            Console.WriteLine("Time: {0}", stopWatch.Elapsed);
        }

        private static List<Filtro> Filtros()
        {
            var result = new List<Filtro>();
            result.Add(new Filtro() { Tipo = 1, Value = "10" });
            result.Add(new Filtro() { Tipo = 2, Value = "an" });
            result.Add(new Filtro() { Tipo = 3, Value = "Flores" });
            return result;
        }

        private static Func<TestClass, bool> ConstroiExpressaoDosFiltros(List<Filtro> filtros)
        {
            var actual = 0;
            for (var i = 0; i < filtros.Count; ++i)
            {
                var filtro = filtros[i];
                if (filtro.Tipo == 1 || filtro.Tipo == 2 || filtro.Tipo == 3)
                {
                    if (!string.IsNullOrWhiteSpace(filtros[i].Value))
                    {
                        actual = i;
                        i = filtros.Count;
                    }
                }
                else
                {
                    Console.WriteLine("Tipo de filtro {0} não definido.", filtro.Tipo);
                }
            }

            if (actual < filtros.Count)
            {
                var parameter = Expression.Parameter(typeof(TestClass), "tc");
                var filtroActual = filtros[actual];
                var value = Expression.Constant(filtroActual.Value);
                var property = string.Empty;
                var function = string.Empty;
                var arguments = default(Type[]);
                var applyToString = false;
                switch (filtroActual.Tipo)
                {
                    case 1:
                        property = "Age";
                        function = "Equals";
                        arguments = new Type[1] { typeof(string) };
                        applyToString = true;
                        break;
                    case 2:
                        property = "Name";
                        function = "Contains";
                        arguments = new Type[1] { typeof(string) };
                        break;
                    case 3:
                        property = "Address";
                        function = "Contains";
                        arguments = new Type[1] { typeof(string) };
                        break;
                }

                Expression otherValue = otherValue = Expression.Property(parameter, property); ;
                if (applyToString)
                {
                    otherValue = Expression.Call(otherValue, typeof(int).GetMethod("ToString", new Type[] { }), new Expression[] { });
                }

                Expression bodyExpression = Expression.Call(otherValue, typeof(string).GetMethod(function, arguments), new Expression[] { value });
                for (int i = actual + 1; i < filtros.Count; ++i)
                {
                    filtroActual = filtros[i];
                    value = Expression.Constant(filtroActual.Value);
                    property = string.Empty;
                    function = string.Empty;
                    applyToString = false;
                    switch (filtroActual.Tipo)
                    {
                        case 1:
                            property = "Age";
                            function = "Equals";
                            applyToString = true;
                            break;
                        case 2:
                            property = "Name";
                            function = "Contains";
                            break;
                        case 3:
                            property = "Address";
                            function = "Contains";
                            break;
                    }

                    otherValue = otherValue = Expression.Property(parameter, property); ;
                    if (applyToString)
                    {
                        otherValue = Expression.Call(otherValue, typeof(TestClass).GetMethod("ToString"), new Expression[] { });
                    }

                    Expression tempExpression = Expression.Call(otherValue, typeof(string).GetMethod(function), new Expression[] { value });
                    bodyExpression = Expression.AndAlso(bodyExpression, tempExpression);
                }

                var lambda = Expression.Lambda(bodyExpression, new[] { parameter });
                return (Func<TestClass, bool>)lambda.Compile();
            }
            else
            {
                var value = Expression.Constant(true);
                var parameter = Expression.Parameter(typeof(TestClass), "tc");
                var lambda = Expression.Lambda(value, new[] { parameter });
                return (Func<TestClass, bool>)lambda.Compile();
            }

            throw new NotImplementedException();
        }

        private static List<TestClass> ListaDeClasses()
        {
            var result = new List<TestClass>();
            result.Add(new TestClass() { Name = "Adriana Malafaia", Address = "Rua das Flores, Mortágua", Age = 10 });
            result.Add(new TestClass() { Name = "Rita Andrade", Address = "Lugar do Reguila, 1º Esquerdo, Estremoz", Age = 10 });
            result.Add(new TestClass() { Name = "Paulo Mastêlo", Address = "Rua Dr. Alberto Coelho Faúlho, 34, Castro Marim", Age = 11 });
            result.Add(new TestClass() { Name = "Ária Espanhola", Address = "Avenida Asdrúbal Farofa, 2º Direito, 43, Entre-Rios", Age = 12 });
            result.Add(new TestClass() { Name = "Amílcar dos Santos", Address = "Rua de Santo Afinfo, 10, Mosteirô", Age = 10 });
            result.Add(new TestClass() { Name = "Mafalda Amarela", Address = "Porto", Age = 12 });
            result.Add(new TestClass() { Name = "Egas Esteltino", Address = "Lisboa", Age = 10 });
            result.Add(new TestClass() { Name = "Veiga Moniz", Address = "Solar do Amanhecer, Castro Airoso, Castro-D'Aire.", Age = 10 });
            result.Add(new TestClass() { Name = "Constância Armando", Address = "Vivenda Falésia, Rua da Fábrica, 15, Francelos", Age = 12 });
            result.Add(new TestClass() { Name = "Rute Almeida", Address = "Encosta da Paluça, Arouca", Age = 11 });
            return result;
        }

        private static void CoreTests()
        {
            CoreStarter.StartCore();
            Console.WriteLine("Press any key to continue...");
        }

        /// <summary>
        /// Testa a geração de diferenças entre números primos para efeitos de comparação de performances de execução.
        /// </summary>
        private static void TestesPrimosAlgoritmos()
        {
            var primes = new[] { 2, 3, 5, 7, 11, 13 };
            var fileName = "differences";

            var product = 1;
            for (int i = 1; i < primes.Length; ++i)
            {
                product *= primes[i];
            }

            using (var writer = new StreamWriter(fileName))
            {
                writer.Write("2");
                var previousValue = 1;
                var nextValue = 17;
                writer.Write(",{0}", nextValue - previousValue);
                previousValue = nextValue;
                while (nextValue < product)
                {
                    nextValue += 2;
                    var isDivisible = false;
                    for (int i = 1; i < primes.Length; ++i)
                    {
                        if (nextValue % primes[i] == 0)
                        {
                            isDivisible = true;
                            i = primes.Length;
                        }
                    }

                    if (!isDivisible)
                    {
                        writer.Write(",{0}", nextValue - previousValue);
                        previousValue = nextValue;
                    }
                }
            }
        }

        /// <summary>
        /// Experiências com pdf.
        /// </summary>
        private static void Lixo7()
        {
            Regex materialClassificationRegex = new Regex("^[0-9]+(?:\\.[0-9a-zA-Z])*$");

            var match = materialClassificationRegex.IsMatch("5.1.a");
            var file = "Testes/MDSReport_453580869.pdf";

            var pdfReader = new PdfReader(file);
            var pageContents = pdfReader.GetPageContent(1);

            var pagen = pdfReader.GetPageN(3);
            var resources = pagen.GetAsDict(PdfName.LINEAR);

            var strategy = new SimpleTextExtractionStrategy();
            var locationStrategy = new LocationTextExtractionStrategy();
            var pdfReaderContentParser = new PdfReaderContentParser(pdfReader);
            var temp = pdfReaderContentParser.ProcessContent(1, strategy);

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                locationStrategy = new LocationTextExtractionStrategy();
                var locationText = PdfTextExtractor.GetTextFromPage(pdfReader, page, locationStrategy);

                strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                Console.Write(currentText);
            }

            pdfReader.Close();
        }

        /// <summary>
        /// Experiências com bases-de-dados.
        /// </summary>
        private static void Lixo6()
        {
            var insertionNumber = 100;
            var isolationLevel = IsolationLevel.Serializable;

            //var executions = new ExecutionManager();
            //executions.Add(new DatabaseRunner1(isolationLevel, insertionNumber));
            //executions.Add(new DatabaseRunner2(isolationLevel, insertionNumber));
            //executions.ExecuteAll();

            //// Limpeza da base de dados
            //Console.WriteLine("Database cleanup");
            //var dbConnection = new DatabaseConnection();
            //dbConnection.Cleanup();
            //Console.WriteLine("Cleanup ended.");

            insertionNumber = 100;
            isolationLevel = IsolationLevel.Serializable;
            var executions1 = new ExecutionManager();
            executions1.Add(new DatabaseRunner4(isolationLevel, insertionNumber));
            executions1.Add(new DatabaseRunner3(isolationLevel, insertionNumber));
            executions1.ExecuteAll();

            // Limpeza da base de dados
            Console.WriteLine("Database cleanup");
            var dbConnection1 = new DatabaseConnection();
            dbConnection1.Cleanup();
            Console.WriteLine("Cleanup ended.");
        }

        /// <summary>
        /// Experiências com expressões regulares associadas a caminhos de ficheiros.
        /// </summary>
        private static void Lixo5()
        {
            Regex filePathRegularExpression = new Regex("^(?:[\\w]\\:|\\\\)(\\\\[a-zA-Z_\\-\\s0-9\\.]+)+\\\\{0,1}$");

            var match = filePathRegularExpression.Match("C:\\temp\\O meu chorinho\\");
            if (match.Success)
            {
                var group = match.Groups[1].Value;
                Console.WriteLine("Finalmente conseguiste!");
            }
            else
            {
                throw new Exception("Something went very wrong!");
            }
        }

        /// <summary>
        /// Experiências com expressões regulares associadas a endereços de e-mail.
        /// </summary>
        private static void Lixo4()
        {
            var mailRegex = new Regex(@"^(?!\.)(?:""([^""\r\\]|\\[""\r\\])*""|"
            + @"(?:[-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$");

            var match = mailRegex.Match("abc@hotmail.com");
            foreach (var group in match.Groups)
            {
                Console.WriteLine(group);
            }
        }

        /// <summary>
        /// Mais experiências com expressões regulares.
        /// </summary>
        private static void Lixo3()
        {
            var regex = new Regex(".*\\([Rr][Ee][Vv]\\. *(.+)\\).*\\.[xX][lL][sS][xX]{0,1} *$");
            var fileNameRegex = new Regex(".+\\.[xX][lL][sS][xX]{0,1}");

            var fileTemp = "myFile.xlsx";
            if (fileNameRegex.IsMatch(fileTemp))
            {
                Console.WriteLine("First match.");
            }

            var temp = "WH Part Number List_B232_YRL_(REV.2.3).xlsx";
            var match = regex.Match(temp);
            if (match.Success)
            {
                var value = match.Groups[1].Value;
            }
            else
            {
                Console.WriteLine("Match not found.");
            }
        }

        /// <summary>
        /// Experiências com expressões lambda.
        /// </summary>
        private static void Lixo2()
        {
            Expression<Func<TestClass, string>> myExpression = t => t.Name;

            var temporaryParameter = Expression.Parameter(typeof(TestClass), "t");

            Expression<Func<string, string>> myFunc = s => s;
            var parameterExpression1 = Expression.Parameter(typeof(string), "s");
            var constExpression1 = Expression.Constant("const");

            var falseConstant = Expression.Constant(false, typeof(bool));

            // var invokeExpression = Expression.Invoke(myFunc, new[] { parameterExpression1 });

            //var innerExpression1 = Expression.Call(parameterExpression1, myFunc.Body.m, new[] { constExpression1 });

            var expressionTree1 = Expression.Call(myFunc.Body, typeof(string).GetMethod("Contains"), new[] { constExpression1 });
            var myFunctionParameter = myFunc.Parameters[0];

            var otherExpression = Expression.MakeBinary(ExpressionType.OrElse, expressionTree1, falseConstant);
            var lambdaExpressionTree = Expression.Lambda(expressionTree1, new[] { myFunctionParameter });

            // Compilar a expressão
            var lambdaDelegate = lambdaExpressionTree.Compile();

            var value = lambdaDelegate.DynamicInvoke(new[] { "constant" });

            Console.WriteLine(otherExpression);

            Expression<Func<string, string>> initialExpression = s => s;
            var constExpression = Expression.Constant("constant");
            var expressionTree = Expression.Call(initialExpression.Body, typeof(string).GetMethod("Contains"), new[] { constExpression });
            var parameterExpression = Expression.Parameter(typeof(string), "s");

            var lambdaExpression = Expression.Lambda(expressionTree, new[] { parameterExpression });
            Console.WriteLine(lambdaExpression);
        }

        /// <summary>
        /// Experiências genéricas de expressões regulares.
        /// </summary>
        private static void Lixo1()
        {
            Console.WriteLine("Insert your regular expression pattern: ");
            var pattern = Console.ReadLine();
            if (pattern != null)
            {
                var regex = new Regex(pattern);
                Console.WriteLine("Try match (type quit to exit): ");
                var toMatch = Console.ReadLine();
                while (toMatch != null && toMatch.ToLower() != "quit")
                {
                    if (toMatch.ToLower() == "change pattern")
                    {
                        Console.WriteLine("Insert your regular expression pattern: ");
                        pattern = Console.ReadLine();
                        if (pattern != null)
                        {
                            regex = new Regex(pattern);
                            Console.WriteLine("Try match (type quit to exit): ");
                            toMatch = Console.ReadLine();
                        }
                        else
                        {
                            toMatch = null;
                        }
                    }
                    else
                    {
                        var match = regex.Match(toMatch);
                        if (!match.Success)
                        {
                            Console.WriteLine("Input doesn't match pattern {0}.", pattern);
                        }
                        else
                        {
                            Console.WriteLine("Matches:");
                            for (int i = 0; i < match.Groups.Count; ++i)
                            {
                                Console.WriteLine(match.Groups[i].Value);
                            }
                        }

                        Console.WriteLine("Try match (type quit to exit): ");
                        toMatch = Console.ReadLine();
                    }
                }
            }
        }

        /// <summary>
        /// Experiências sobre expressões lambda.
        /// </summary>
        private static void Lixo()
        {
            var list = new List<string>() { "temp1", "temp2" };
            Expression<Func<string, bool>> exp = s => s.Contains("te");

            // Decompose the expression tree.
            ParameterExpression param = (ParameterExpression)exp.Parameters[0];
            var methodCall = (MethodCallExpression)exp.Body;

            var tempExpression = Expression.Call(Expression.Constant("temp"), typeof(string).GetMethod("Contains"), new[] { Expression.Constant("te") });

            //ParameterExpression left = (ParameterExpression)operation.Left;
            //ConstantExpression right = (ConstantExpression)operation.Right;

            //Console.WriteLine("Decomposed expression: {0} => {1} {2} {3}",
            //                  param.Name, left.Name, operation.NodeType, right.Value);
            Console.WriteLine(exp);

            Console.Read();
        }

        /// <summary>
        /// Testes de performance entre vários tipos de ciclos.
        /// </summary>
        private void Test1()
        {
            int arraySize = 2000000;
            int[] array = new int[arraySize];
            Random a = new Random();

            for (int i = 0; i < arraySize; i++)
            {
                array[i] = a.Next();
            }

            Stopwatch sw = Stopwatch.StartNew();
            int maior = array[0];

            sw.Stop();
            Console.WriteLine("Maior: " + maior);
            Console.WriteLine("Sequential Read: " + sw.ElapsedMilliseconds + ("ms"));

            object lockObj = new object();

            sw = Stopwatch.StartNew();
            var par = Partitioner.Create(0, arraySize);

            Parallel.ForEach(par, (range, state) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    Console.WriteLine(i + " - " + Task.CurrentId);
                    if (i > 10000)
                    {
                        state.Stop();
                        return;
                    }
                }
            });

            sw.Stop();
            Console.WriteLine("Maior: " + maior);
            Console.WriteLine("Sequential Read: " + sw.ElapsedMilliseconds + ("ms"));
        }
    }
}