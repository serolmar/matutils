namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    using System.Management;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using Utilities;
    using System.Numerics;

    /// <summary>
    /// Implementa uma série de funções auxiliares.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// O objecto responsável pela sincronização dos fluxos que pretendem obter a informação de sistema.
        /// </summary>
        private static object machineInfoLockObject = new object();

        /// <summary>
        /// Objecto que contém informação sobre a máquina tal como número de processadores e número de núcleos
        /// de processamento.
        /// </summary>
        private static MachineInfo machineStatus = null;

        /// <summary>
        /// Define todos os tipos de símbolos definidos para o <see cref="StringSymbolReader"/>.
        /// </summary>
        private static string[] stringSymbolReaderTypes = {
            "integer","double","double_exponential","string","blancks","equal","double_equal","plus","double_plus","plus_equal",
            "minus","double_minus","minus_equal","times","double_times","times_equal","over","over_equal","exponential","colon",
            "double_colon","bitwise_and","double_and","and_equal","bitwise_or","double_or","or_equal","less_than","double_less","triple_less",
            "less_equal","great_than","double_great","triple_great","great_equal","left_parenthesis","right_parenthesis","left_bracket","right_bracket","double_quote",
            "quote", "left_bar","semi_colon","comma","tild","hat","question_mark","exclamation_mark","left_brace","right_brace",
            "at", "cardinal","dollar","pound","chapter","euro", "underscore", "right_bar", "point", "mod", "space", "new_line",
            "carriage_return", "start_comment", "end_comment", "line_comment", "any", "eof"};

        /// <summary>
        /// Obtém o estado actual da máquina.
        /// </summary>
        public static MachineInfo MachineStatus
        {
            get
            {
                lock (machineInfoLockObject)
                {
                    if (machineStatus == null)
                    {
                        machineStatus = new MachineInfo();
                    }
                }

                return machineStatus;
            }
        }

        /// <summary>
        /// Obtém o texto para o tipo de símbolo num leitor <see cref="StringSymbolReader"/> a partir de 
        /// um valor enumerado.
        /// </summary>
        /// <param name="type">O valor enumerado.</param>
        /// <returns>O tipo de símbolo.</returns>
        public static string GetStringSymbolType(EStringSymbolReaderType type)
        {
            return stringSymbolReaderTypes[(int)type];
        }

        /// <summary>
        /// Tenta ler valores a partir do texto.
        /// </summary>
        /// <param name="text">O texto.</param>
        /// <param name="cultureInfo">
        /// O objecto que representa a cultura utilizada na leitura dos números e datas.
        /// </param>
        /// <returns>Os valores.</returns>
        public static object GetTextValue(string text, CultureInfo cultureInfo)
        {
            var integerValue = 0;
            if (int.TryParse(text, out integerValue))
            {
                return integerValue;
            }
            else
            {
                var longValue = 0L;
                if (long.TryParse(text, out longValue))
                {
                    return longValue;
                }
                else
                {
                    var bigInteger = default(BigInteger);
                    if (BigInteger.TryParse(text, out bigInteger))
                    {
                        if (bigInteger < 0)
                        {
                            if (bigInteger >= long.MinValue)
                            {
                                if (bigInteger >= int.MinValue)
                                {
                                    return (int)bigInteger;
                                }
                                else
                                {
                                    return (long)bigInteger;
                                }
                            }
                            else
                            {
                                return bigInteger;
                            }
                        }
                        else if (bigInteger <= ulong.MaxValue)
                        {
                            if (bigInteger <= long.MaxValue)
                            {
                                if (bigInteger <= int.MaxValue)
                                {
                                    return (int)bigInteger;
                                }
                                else
                                {
                                    return (long)bigInteger;
                                }
                            }
                            else
                            {
                                return (ulong)bigInteger;
                            }
                        }
                        else
                        {
                            return bigInteger;
                        }
                    }
                    else
                    {
                        var doubleValue = 0.0;
                        if (double.TryParse(text, NumberStyles.Number, cultureInfo, out doubleValue))
                        {
                            return doubleValue;
                        }
                        else
                        {
                            var decimalValue = 0.0M;
                            if (decimal.TryParse(text, out decimalValue))
                            {
                                return decimalValue;
                            }
                            else
                            {
                                var dateTime = default(DateTime);
                                if (DateTime.TryParse(
                                    text,
                                    cultureInfo.DateTimeFormat,
                                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault,
                                    out dateTime))
                                {
                                    return dateTime;
                                }
                                else
                                {
                                    return text;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cria um vector de apontadores em código não gerido.
        /// </summary>
        /// <typeparam name="T">O tipo de dados no vector.</typeparam>
        /// <param name="array">O vector.</param>
        /// <returns>Os objectos a serem passados.</returns>
        public static Tuple<IntPtr, IntPtr[]> AllocUnmanagedPointersArray<T>(T[] array)
        {
            if (array == null)
            {
                return null;
            }
            else
            {
                var arrayLength = array.Length;
                var managedPtrArray = new IntPtr[arrayLength];
                var ptrSize = Marshal.SizeOf(typeof(IntPtr));
                var unmanagedArrayPtr = Marshal.AllocHGlobal(ptrSize * arrayLength);

                // Procede à criação dos objectos em código não gerido
                for (int i = 0; i < arrayLength; ++i)
                {
                    var current = array[i];
                    var managedElementPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
                    managedPtrArray[i] = managedElementPtr;
                    Marshal.StructureToPtr(current, managedElementPtr, false);
                    Marshal.WriteIntPtr(unmanagedArrayPtr, i * ptrSize, managedElementPtr);
                }

                return Tuple.Create(unmanagedArrayPtr, managedPtrArray);
            }
        }

        /// <summary>
        /// Liberta o vector de apontadores reservado na memória não gerida.
        /// </summary>
        /// <param name="arrayPtr">O apontador.</param>
        public static void FreeUnmanagedArray(Tuple<IntPtr, IntPtr[]> arrayPtr)
        {
            if (arrayPtr != null)
            {
                Marshal.FreeHGlobal(arrayPtr.Item1);
                var elements = arrayPtr.Item2;
                var elementsLength = elements.Length;
                for (int i = 0; i < elementsLength; ++i)
                {
                    var current = elements[i];
                    Marshal.FreeHGlobal(current);
                }
            }
        }

        /// <summary>
        /// Obtém a memória virtual disponível em KB.
        /// </summary>
        /// <returns>A memória virtual.</returns>
        public static MemoryInfo GetMemoryInfo()
        {
            var operatingSystemVersion = Environment.OSVersion;
            var platform = operatingSystemVersion.Platform;
            switch (platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    var managementObjectSearcher = new ManagementObjectSearcher(
                        "Select FreePhysicalMemory, FreeVirtualMemory, TotalVisibleMemorySize from Win32_OperatingSystem");
                    var systemCollection = managementObjectSearcher.Get();
                    var systemCollectionEnumerator = systemCollection.GetEnumerator();
                    if (systemCollectionEnumerator.MoveNext())
                    {
                        var freePhysicalMemory = 0L;
                        var freeVirtualMemory = 0L;
                        var totalVisibleMemory = 0L;

                        var currentSearch = systemCollectionEnumerator.Current;
                        var currentValue = currentSearch["FreePhysicalMemory"];
                        freePhysicalMemory = long.Parse(currentValue.ToString());

                        currentValue = currentSearch["FreeVirtualMemory"];
                        freeVirtualMemory = long.Parse(currentValue.ToString());

                        currentValue = currentSearch["TotalVisibleMemorySize"];
                        totalVisibleMemory = long.Parse(currentValue.ToString());

                        return new MemoryInfo()
                        {
                            FreePhysicalMemory = freePhysicalMemory,
                            FreeVirtualMemory = freeVirtualMemory,
                            TotalVisibleMemorySize = totalVisibleMemory
                        };
                    }
                    else
                    {
                        throw new UtilitiesDataException("Can't query the system for processor info.");
                    }
                case PlatformID.MacOSX:
                    throw new UtilitiesException("Unsupported platform: MacOSX");
                case PlatformID.Unix:
                    throw new UtilitiesException("Unsupported platform: Unix");
                case PlatformID.Xbox:
                    throw new UtilitiesException("Unsupported platform: Xbox");
                default:
                    throw new UtilitiesException("Unknown platform.");
            }
        }
    }

    /// <summary>
    /// Classe que possibilita a consulta do número de processadores e núcleos de processamento
    /// existentes na máquina.
    /// </summary>
    public class MachineInfo
    {
        /// <summary>
        /// Número de processadores.
        /// </summary>
        private int processors;

        /// <summary>
        /// Número de núcleos.
        /// </summary>
        private int cores;

        /// <summary>
        /// A memória física total.
        /// </summary>
        private long totalPhysicalMemory;

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="MachineInfo"/>.
        /// </summary>
        internal MachineInfo()
        {
            this.SetMachineInfo();
        }

        /// <summary>
        /// Obtém o núemro de processdores existentes na máquina.
        /// </summary>
        public int Processors
        {
            get
            {
                return this.processors;
            }
        }

        /// <summary>
        /// Obtém o número de núcleos de processamento existentes na máquina.
        /// </summary>
        public int Cores
        {
            get
            {
                return this.cores;
            }
        }

        /// <summary>
        /// Otbém a memória física total.
        /// </summary>
        private long TotalPhysicalMemory
        {
            get
            {
                return this.totalPhysicalMemory;
            }
        }

        /// <summary>
        /// Estabelece os valores informativos.
        /// </summary>
        private void SetMachineInfo()
        {
            var operatingSystemVersion = Environment.OSVersion;
            var platform = operatingSystemVersion.Platform;
            switch (platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    this.SetWindowsMachineInfo();
                    break;
                case PlatformID.MacOSX:
                    throw new UtilitiesException("Unsupported platform: MacOSX");
                case PlatformID.Unix:
                    throw new UtilitiesException("Unsupported platform: Unix");
                case PlatformID.Xbox:
                    throw new UtilitiesException("Unsupported platform: Xbox");
                default:
                    throw new UtilitiesException("Unknown platform.");
            }
        }

        /// <summary>
        /// Estabelece a informação da máquina em ambiente Windows.
        /// </summary>
        private void SetWindowsMachineInfo()
        {
            // Consulta o número de processadores.
            var managementObjectSearcher = new ManagementObjectSearcher(
                "Select NumberOfProcessors, TotalPhysicalMemory from Win32_ComputerSystem");
            var systemCollection = managementObjectSearcher.Get();
            var systemCollectionEnumerator = systemCollection.GetEnumerator();
            if (systemCollectionEnumerator.MoveNext())
            {
                var currentSearch = systemCollectionEnumerator.Current;
                var currentValue = currentSearch["NumberOfProcessors"];
                this.processors = int.Parse(currentValue.ToString());

                currentValue = currentSearch["TotalPhysicalMemory"];
                this.totalPhysicalMemory = long.Parse(currentValue.ToString());

                // Consulta o número de núcleos de processamento.
                managementObjectSearcher = new ManagementObjectSearcher(
                    "Select NumberOfCores from Win32_Processor");
                systemCollection = managementObjectSearcher.Get();
                var numberOfCores = 0;
                foreach (var systemInfo in systemCollection)
                {
                    numberOfCores += int.Parse(systemInfo["NumberOfCores"].ToString());
                }

                this.cores = numberOfCores;
            }
            else
            {
                throw new UtilitiesDataException("Can't query the system for processor info.");
            }
        }
    }

    /// <summary>
    /// Mantém a informação sobre o estado da memória.
    /// </summary>
    public struct MemoryInfo
    {
        /// <summary>
        /// A quantidade de memória física livre (KB).
        /// </summary>
        private long freePhysicalMemory;

        /// <summary>
        /// A quantidade de memória virtual livre (KB).
        /// </summary>
        private long freeVirtualMemory;

        /// <summary>
        /// O tamanho total da memória visível (KB).
        /// </summary>
        private long totalVisibleMemorySize;

        /// <summary>
        /// Obtém a quantidade de memória física livre (KB).
        /// </summary>
        public long FreePhysicalMemory
        {
            get
            {
                return this.freePhysicalMemory;
            }
            internal set
            {
                this.freePhysicalMemory = value;
            }
        }

        /// <summary>
        /// Obtém a quantidade de memória virtual livre.
        /// </summary>
        public long FreeVirtualMemory
        {
            get
            {
                return this.freeVirtualMemory;
            }
            internal set
            {
                this.freeVirtualMemory = value;
            }
        }

        /// <summary>
        /// Obtém o tamanho total da memória visível.
        /// </summary>
        public long TotalVisibleMemorySize
        {
            get
            {
                return this.totalVisibleMemorySize;
            }
            internal set
            {
                this.totalVisibleMemorySize = value;
            }
        }
    }
}
