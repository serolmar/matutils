namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.Text;
    using System.Threading.Tasks;
    using Utilities.Collections;

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
            "left_bar","semi_colon","comma","tild","hat","question_mark","exclamation_mark","left_brace","right_brace","at",
            "cardinal","dollar","pound","chapter","euro","any"};

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
                    "Select numberofprocessors from Win32_ComputerSystem");
                var systemCollection = managementObjectSearcher.Get();
                var systemCollectionEnumerator = systemCollection.GetEnumerator();
                if (systemCollectionEnumerator.MoveNext())
                {
                    var currentValue = systemCollectionEnumerator.Current["numberofprocessors"];
                    this.processors = int.Parse(currentValue.ToString());

                    // Consulta o número de núcleos de processamento.
                    managementObjectSearcher = new ManagementObjectSearcher(
                        "Select numberofcores from Win32_Processor");
                    systemCollection = managementObjectSearcher.Get();
                    var numberOfCores = 0;
                    foreach (var systemInfo in systemCollection)
                    {
                        numberOfCores += int.Parse(systemInfo["numberofcores"].ToString());
                    }

                    this.cores = numberOfCores;
                }
                else
                {
                    throw new UtilitiesDataException("Can't query the system for processor info.");
                }
            }
        }
    }
}
