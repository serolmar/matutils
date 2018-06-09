// -----------------------------------------------------------------------
// <copyright file="SamplesGenerators.cs" company="Sérgio O. Marques">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define funções que permitem gerar amostras sonoras.
    /// </summary>
    public static class SamplesGenerators
    {
        /// <summary>
        /// Obtém a função sinusoidal com frequência unitária.
        /// </summary>
        /// <returns>A função sinusoidal.</returns>
        public static Func<double, double> GetSineFunc()
        {
            var freqCoeff = 2 * Math.PI;
            return x => Math.Sin(freqCoeff * x);
        }

        /// <summary>
        /// Obtém a função sinusoidal com a frequência e fase especificadas.
        /// </summary>
        /// <param name="freq">A frequência.</param>
        /// <param name="phase">A fase.</param>
        /// <returns>A função sinusoidal.</returns>
        public static Func<double, double> GetSineFunc(
            double freq,
            double phase)
        {
            if (phase == 0)
            {
                var freqCoeff = 2 * Math.PI;
                if (freq != 1)
                {
                    freqCoeff *= freq;
                }

                return x => Math.Sin(freqCoeff * x);
            }
            else
            {
                var freqCoeff = 2 * Math.PI;
                if (freq != 1)
                {
                    freqCoeff *= freq;
                }

                return x => Math.Sin(freqCoeff * x + phase);
            }
        }

        /// <summary>
        /// Obtém a função periódica de frequência unitária onde os picos
        /// máximo e mínimo são determinados pelo parâmetro.
        /// </summary>
        /// <remarks>
        /// A função é ajustada a passar nos picos definidos, recorrendo à
        /// interpolação trigonométrica.
        /// </remarks>
        /// <param name="maxznt">O maximizante.</param>
        /// <param name="minznt">O minimizante.</param>
        /// <returns>A função sinusoidal parametrizada.</returns>
        public static Func<double, double> GetParamSineFunc(
            double maxznt,
            double minznt)
        {
            if (minznt <= 0)
            {
                throw new ArgumentOutOfRangeException("maxznt", "The minimizant must be in the interval (0,1).");
            }
            else if (maxznt >= 1)
            {
                throw new ArgumentOutOfRangeException("maxznt", "The maximizant must be in the interval (0,1).");
            }
            else if (minznt <= maxznt)
            {
                throw new ArgumentException("Maximizant must come before minimizant.");
            }
            else
            {
                var freq = 2 * Math.PI;
                var den1 = 4 * (1 - Math.Cos(freq * (minznt - maxznt)));
                var den2 = 1 - Math.Cos(freq * minznt);
                var den3 = 1 - Math.Cos(freq * maxznt);

                return x => Math.Sin(freq * ((1 - Math.Cos(freq * x)) / den1) *
                    (3 * (1 - Math.Cos(freq * (x - maxznt))) / den2 +
                    (1 - Math.Cos(freq * (x - minznt))) / den3));
            }
        }

        /// <summary>
        /// Obtém uma função periódica de frequência unitária que resulta da cópia
        /// da parte de uma função restrita ao intervalo [0,1).
        /// </summary>
        /// <param name="restrictedFunc">A função cuja restrição ao intervalo [0,1) será usada como cópia.</param>
        /// <returns>A função periódica que resulta da cópia.</returns>
        public static Func<double, double> GetUnitPeriodCopyFunc(
            Func<double, double> restrictedFunc)
        {
            if (restrictedFunc == null)
            {
                throw new ArgumentNullException("restrictedFunc");
            }
            else
            {
                return x => restrictedFunc.Invoke(x - Math.Floor(x));
            }
        }

        /// <summary>
        /// Gera a função cópia a partir de uma função restrita ao intervalo [0,1).
        /// </summary>
        /// <param name="restrictedFunc">A função restrita.</param>
        /// <param name="freq">A frequência da cópia.</param>
        /// <returns>A função cópia.</returns>
        public static Func<double, double> GetCopyFunc(
            Func<double, double> restrictedFunc,
            double freq,
            double phase)
        {
            if (restrictedFunc == null)
            {
                throw new ArgumentNullException("restrictedFunc");
            }
            else if (phase == 0)
            {
                if (freq == 1)
                {
                    return restrictedFunc;
                }
                else
                {
                    return x => restrictedFunc.Invoke(freq * x);
                }
            }
            else
            {
                if (freq == 1)
                {
                    return x => restrictedFunc.Invoke(x + phase);
                }
                else
                {
                    return x => restrictedFunc.Invoke(freq * x + phase);
                }
            }
        }

        /// <summary>
        /// Gera a função resultante da combinação da função periódica de período unitário.
        /// </summary>
        /// <param name="periodicFunc">A função periódica com frequência unitária.</param>
        /// <param name="ampFunc">A função que permite modular a amplitude.</param>
        /// <param name="freqFunc">A função que permite modular a frequência.</param>
        /// <param name="phaseFunc">A função que permite modular a fase.</param>
        /// <returns>A função cópia.</returns>
        public static Func<double, double> GetMixedFunc(
            Func<double, double> periodicFunc,
            Func<double, double> ampFunc,
            Func<double, double> freqFunc,
            Func<double, double> phaseFunc)
        {
            if (periodicFunc == null)
            {
                throw new ArgumentNullException("periodicFunc");
            }
            else if (ampFunc == null)
            {
                if (freqFunc == null)
                {
                    if (phaseFunc == null)
                    {
                        return periodicFunc;
                    }
                    else
                    {
                        return x => periodicFunc.Invoke(
                            x + phaseFunc.Invoke(x));
                    }
                }
                else if (phaseFunc == null)
                {
                    return x => periodicFunc.Invoke(
                        freqFunc.Invoke(x) * x);
                }
                else
                {
                    return x => periodicFunc.Invoke(
                        freqFunc.Invoke(x) * x + phaseFunc.Invoke(x));
                }
            }
            else if (freqFunc == null)
            {
                if (phaseFunc == null)
                {
                    return x => ampFunc.Invoke(x) * periodicFunc.Invoke(x);
                }
                else
                {
                    return x => ampFunc.Invoke(x) * periodicFunc.Invoke(
                        x + phaseFunc.Invoke(x));
                }
            }
            else if (phaseFunc == null)
            {
                return x => ampFunc.Invoke(x) * periodicFunc.Invoke(
                    freqFunc.Invoke(x) * x);
            }
            else
            {
                return x => ampFunc.Invoke(x) * periodicFunc.Invoke(
                    freqFunc.Invoke(x) * x + phaseFunc.Invoke(x));
            }
        }

        /// <summary>
        /// Gera a função cópia a partir de uma função restrita ao intervalo [0,1).
        /// </summary>
        /// <param name="restrictedFunc">A função restrita.</param>
        /// <param name="freq">A frequência da cópia.</param>
        /// <returns>A função cópia.</returns>
        public static Func<double, double> GetCopyFunc(
            Func<double, double> restrictedFunc,
            double freq)
        {
            return x => GetUnitPeriodCopyFunc(restrictedFunc)(freq * x);
        }

        /// <summary>
        /// Obtém a função que resulta da modulação em amplitude de uma função periódica.
        /// </summary>
        /// <param name="periodicFunc">A função periódica.</param>
        /// <param name="ampModFunc">A função que permite modular a amplitude.</param>
        /// <returns>A função modulada.</returns>
        public static Func<double, double> GetAmpModulatedFunc(
            Func<double, double> periodicFunc,
            Func<double, double> ampModFunc)
        {
            return x => periodicFunc.Invoke(x) * ampModFunc.Invoke(x);
        }

        /// <summary>
        /// Otbém uma função cuja fase é modulada.
        /// </summary>
        /// <param name="periodicFunc">A função periódica.</param>
        /// <param name="phaseModFunc">A função responsável pela modulação da fase.</param>
        /// <returns>A função modulada.</returns>
        public static Func<double, double> GetPhaseModulatedFunc(
            Func<double, double> periodicFunc,
            Func<double, double> phaseModFunc)
        {
            if (periodicFunc == null)
            {
                throw new ArgumentNullException("peridocFunction");
            }
            else if (phaseModFunc == null)
            {
                return periodicFunc;
            }
            else
            {
                return x => periodicFunc.Invoke(x + phaseModFunc.Invoke(x));
            }
        }

        /// <summary>
        /// Obtém uma fusção cuja frequência é modulada.
        /// </summary>
        /// <param name="periodicFunc">A função periódica.</param>
        /// <param name="freqModFunc">A função responsável pela modulação da frequência.</param>
        /// <returns>A função modulada.</returns>
        public static Func<double,double> GetFreqModulatedFunc(
            Func<double, double> periodicFunc,
            Func<double, double> freqModFunc)
        {
            if (periodicFunc == null)
            {
                throw new ArgumentNullException("peridocFunction");
            }
            else if (freqModFunc == null)
            {
                return periodicFunc;
            }
            else
            {
                return x => periodicFunc.Invoke(x * freqModFunc.Invoke(x));
            }
        }

        /// <summary>
        /// Gera uma função sincronizada por um oscilador interno.
        /// </summary>
        /// <remarks>
        /// Sempre que a variável atinge o valor da frequência de sincronizaçõa,
        /// a fase da função é reposta a zero.
        /// </remarks>
        /// <param name="periodicFunc">A função a ser sincronizada.</param>
        /// <param name="syncFreq">A frequência do oscilador interno.</param>
        /// <returns>A função resultante.</returns>
        private static Func<double, double> GetSyncFunc(
            Func<double, double> periodicFunc,
            float syncFreq)
        {
            if (periodicFunc == null)
            {
                throw new ArgumentNullException("periodicFunc");
            }
            else
            {
                return x => periodicFunc.Invoke(syncFreq * x - (float)Math.Floor(syncFreq * x));
            }
        }

        /// <summary>
        /// Obtém a composta entre duas funções.
        /// </summary>
        /// <param name="outer">A função externa.</param>
        /// <param name="inner">A função interna.</param>
        /// <returns>A função composta.</returns>
        public static Func<double, double> GetComposite(
            Func<double, double> outer,
            Func<double, double> inner)
        {
            if (outer == null)
            {
                throw new ArgumentNullException("outer");
            }
            else if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }
            else
            {
                return x => outer.Invoke(inner.Invoke(x));
            }
        }

        /// <summary>
        /// Obtém a forma de onda quadrada restrita ao intervalo [0,1).
        /// </summary>
        /// <returns>A função quadrada restrita.</returns>
        public static Func<double, double> GetRestrictedSquareFunc()
        {
            return x =>
            {
                if (x == 0 || x == 0.5)
                {
                    return 0;
                }
                else if (x < 0.5)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            };
        }

        /// <summary>
        /// Obtém a função dente de serra restrita ao intervalo [0,1).
        /// </summary>
        /// <returns>A função dente de serra restrita.</returns>
        public static Func<double, double> GetRestrictedSawToothFunc()
        {
            return x =>
            {
                if (x == 0)
                {
                    return 0;
                }
                else
                {
                    return 2 * x - 1;
                }
            };
        }

        /// <summary>
        /// Obtém a função triangular restrita ao intervalo [0,1).
        /// </summary>
        /// <returns>A função triangular restrita.</returns>
        public static Func<double, double> GetRestrictedTriangFunc()
        {
            return x =>
            {
                if (x < 0.25)
                {
                    return 4 * x;
                }
                else if (x < 0.75)
                {
                    return 2 - 4 * x;
                }
                else
                {
                    return 4 * x - 4;
                }
            };
        }

        /// <summary>
        /// Obtém a função exponencial restrita ao intervalo [0,1).
        /// </summary>
        /// <returns>A função exponencial restrita.</returns>
        public static Func<double, double> GetRestrictedExpFunc()
        {
            return x =>
            {
                if (x == 0)
                {
                    return 0;
                }
                else
                {
                    return 2 * Math.Exp(Math.Log(2) * x) - 1;
                }
            };
        }

        /// <summary>
        /// Obtém a função quadrada cujo degrau é efectuado no valor indicado quando restrita
        /// ao intervalo [0,1).
        /// </summary>
        /// <param name="change">Valor onde a função quadrada passa de positiva para negativa.</param>
        /// <returns>A função quadrada parametrizada restrita.</returns>
        public static Func<double, double> GetRestrictedParamSquareFunc(
            double change)
        {
            if (change <= 0 || change >= 1)
            {
                throw new ArgumentOutOfRangeException("change", "The change value must be in the interval (0,1).");
            }
            else
            {
                return x =>
                {
                    if (x == 0 || x == change)
                    {
                        return 0;
                    }
                    else if (x < change)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                };
            }
        }

        /// <summary>
        /// Obtém a função triangular cujo máximo e mínimo são parametrizados.
        /// </summary>
        /// <param name="maxznt">O maximizante.</param>
        /// <param name="minznt">O minimizante.</param>
        /// <returns>A função triangular parametrizada.</returns>
        public static Func<double, double> GetRestrictedParamTriangFunc(
            double maxznt,
            double minznt)
        {
            if (minznt <= 0)
            {
                throw new ArgumentOutOfRangeException("maxznt", "The minimizant must be in the interval (0,1).");
            }
            else if (maxznt >= 1)
            {
                throw new ArgumentOutOfRangeException("maxznt", "The maximizant must be in the interval (0,1).");
            }
            else if (minznt <= maxznt)
            {
                throw new ArgumentException("Maximizant must come before minimizant.");
            }
            else
            {
                return x =>
                {
                    if (x < maxznt)
                    {
                        return (1 / maxznt) * x;
                    }
                    else if (x < minznt)
                    {
                        var delta = minznt - maxznt;
                        return (minznt + maxznt) / delta - (2 / delta) * x;
                    }
                    else
                    {
                        var delta = 1 - minznt;
                        return (1 / delta) * x - 1 / delta;
                    }
                };
            }
        }

        /// <summary>
        /// Obtém uma forma de onda quadrada cujos degraus são dados por
        /// uma função aleatória.
        /// </summary>
        /// <param name="samplesNumber">O número de amostras no intervalo [0,1).</param>
        /// <returns>A função de onda quadrada.</returns>
        public static Func<double, double> GetRestrictedRandomSquareFunc(
            uint samplesNumber)
        {
            if (samplesNumber == 0)
            {
                throw new ArgumentException("Number of samples must be non zero");
            }
            else
            {
                var samplesArray = new double[samplesNumber];
                var random = new Random();
                for (var i = 0; i < samplesNumber; ++i)
                {
                    samplesArray[i] = 2 * random.NextDouble() - 1;
                }

                return x => samplesArray[(uint)Math.Floor(x * samplesNumber)];
            }
        }

        /// <summary>
        /// Obtém uma função que retorna valores unitários no intervalo [-1,1].
        /// </summary>
        /// <remarks>
        /// A função gera sempre um valor diferente mesmo quando x possui o mesmo valor.
        /// </remarks>
        /// <returns>A função aleatória.</returns>
        public static Func<double, double> GetRandomFunc()
        {
            var random = new Random();
            return x => 2 * random.NextDouble() - 1;
        }

        /// <summary>
        /// Escreve um ficheiro wav com o sinal definido pela função.
        /// </summary>
        /// <param name="func">A função.</param>
        /// <param name="timeInSeconds">O tempo em segundos.</param>
        /// <param name="samplesPerSec">Número de amostras por segundo.</param>
        /// <param name="path">O caminho para o ficheiro wav.</param>
        public static void WriteWav(
            Func<double, double> func,
            double timeInSeconds,
            uint samplesPerSec,
            string path)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            else if (timeInSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException("timeInSeconds", "Time must be a positive number");
            }
            else
            {
                using (var fileStream = new FileStream(
                    path,
                    FileMode.OpenOrCreate,
                    FileAccess.Write))
                {
                    // Dois canais com 4 byte (bits por amostra) por canal proporciona 8 bytes em cada bloco
                    var dataSize = (uint)Math.Floor(samplesPerSec * timeInSeconds) * 8;
                    var byteRate = samplesPerSec << 3;

                    // Porção: cabeçalho
                    fileStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 }, 0, 4); // ChunkID: RIFF
                    fileStream.Write(BitConverter.GetBytes(dataSize + 36), 0, 4); // ChunckSize: datasize + 36
                    fileStream.Write(new byte[] { 0x57, 0x41, 0x56, 0x45 }, 0, 4); // Format: WAVE

                    // Primeira sub-porção: fmt
                    fileStream.Write(new byte[] { 0x66, 0x6d, 0x74, 0x20 }, 0, 4); // SubChunk1ID: ftm\s
                    fileStream.Write(BitConverter.GetBytes(16U), 0, 4); // SubChunk1ID Size: 16
                    fileStream.Write(BitConverter.GetBytes((short)1), 0, 2); //  Audio format: PCM = 1, Microsoft ADPCM = 2, ver mmreg.h
                    fileStream.Write(BitConverter.GetBytes((short)2), 0, 2); // Número de canais
                    fileStream.Write(BitConverter.GetBytes(samplesPerSec), 0, 4); // Número de amostras por segundo
                    fileStream.Write(BitConverter.GetBytes(byteRate), 0, 4); // Número de amostras por segundo x número de canais x bits por amostra / 8
                    fileStream.Write(BitConverter.GetBytes((short)8), 0, 2); // Número de canais x bits por amostra / 8
                    fileStream.Write(BitConverter.GetBytes((short)32), 0, 2); // Bits por amostra

                    // Segunda sub-porção: dados
                    fileStream.Write(new byte[] { 0x64, 0x61, 0x74, 0x61 }, 0, 4);
                    fileStream.Write(BitConverter.GetBytes(dataSize), 0, 4);

                    var delta = 1.0 / samplesPerSec;
                    for (var x = delta; x < timeInSeconds; x += delta)
                    {
                        var res = (float)func.Invoke(x);
                        var bytes = BitConverter.GetBytes(res);
                        fileStream.Write(bytes, 0, 4);
                        fileStream.Write(bytes, 0, 4);
                    }
                }
            }
        }
    }
}
