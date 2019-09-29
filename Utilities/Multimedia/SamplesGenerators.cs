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
    public static class SamplesGeneratorFunctions
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
        /// <param name="phase">A fase.</param>
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
                var unitCopy = GetUnitPeriodCopyFunc(restrictedFunc);
                if (freq == 1)
                {
                    return unitCopy;
                }
                else
                {
                    return x => unitCopy.Invoke(freq * x);
                }
            }
            else
            {
                var unitCopy = GetUnitPeriodCopyFunc(restrictedFunc);
                if (freq == 1)
                {
                    return x => unitCopy.Invoke(x + phase);
                }
                else
                {
                    return x => unitCopy.Invoke(freq * x + phase);
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
            var f = GetUnitPeriodCopyFunc(restrictedFunc);
            return x => f.Invoke(freq * x);
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
        /// Obtém a função dente de serra como nos sintetizadores moog.
        /// </summary>
        /// <returns>A função dente de serra.</returns>
        public static Func<double, double> GetRestrictedMoogSawFunc()
        {
            return x =>
            {
                if (x < 0.5)
                {
                    return 4 * x - 1;
                }
                else if (x == 0.5)
                {
                    return 0.5;
                }
                else
                {
                    return 1.0 - 2.0 * x;
                }
            };
        }

        /// <summary>
        /// Obtém a função exp. como definida no "triple oscillator" do LMMS.
        /// </summary>
        /// <returns>A função exp.</returns>
        public static Func<double, double> GetRestrictedLmmsExpFunc()
        {
            return x =>
            {
                if (x == 0)
                {
                    return -0.5;
                }
                else if (x < 0.5)
                {
                    return -1.0 + 8.0 * x * x;
                }
                else if (x == 0.5)
                {
                    return 0.75;
                }
                else
                {
                    return 1.0 - x;
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
                if (x < 0.5)
                {
                    return (Math.Exp(x) - 1) / (Math.Exp(0.5) - 1);
                }
                else
                {
                    return (Math.Exp(1 - x) - 1) / (Math.Exp(0.5) - 1);
                }
            };
        }

        /// <summary>
        /// Obtém a função exponencial restrita ao intervalo [0,1).
        /// </summary>
        /// <returns>A função exponencial restrita.</returns>
        public static Func<double, double> GetRestrictedExpFuncV1()
        {
            return x =>
            {
                if (x == 0)
                {
                    return 0;
                }
                else
                {
                    return 2 * (Math.Exp(x) - 1) / (Math.Exp(1) - 1) - 1;
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
        /// Obtém a função co-seno definida no intervalo [0,1) na qual o máximo
        /// constitui um valor nesse intervalo, sendo os argumentos interpolados.
        /// </summary>
        /// <param name="max">O valor do máximo.</param>
        /// <returns>A função.</returns>
        public static Func<double, double> GetRestrictedMaxInterpolateSineFunc(
            double max)
        {
            if (max <= 0 || max > 1)
            {
                throw new ArgumentOutOfRangeException(
                    "max",
                    "Max must be a value between 0  and 1.");
            }
            else
            {
                return x =>
                {
                    var t = (-x + max + 0.5) * x;
                    t = t / max;
                    t -= 0.25;
                    return Math.Sin(2 * Math.PI * t);
                };
            }
        }

        /// <summary>
        /// Obtém a função seno cuja parametrização é dada por uma
        /// espécie de interpolação racional.
        /// </summary>
        /// <param name="r">O parâmetro.</param>
        /// <returns>A função.</returns>
        public static Func<double, double> GetRestrictedParamSineFunc(
            double r)
        {
            if (r < 0 || r >= 1)
            {
                throw new ArgumentOutOfRangeException("r", "Parameter must be between 0 and 1.");
            }
            else
            {
                return x =>
                {
                    var res = (3 * r - 3) * x / ((6 * r - 3) * x - 3 * r);
                    return Math.Sin(2 * Math.PI * res);
                };
            }
        }

        /// <summary>
        /// Obtém a função parabólica restrita com vértice no ponto (0,5; 1).
        /// </summary>
        /// <returns>A função parabólica restrita.</returns>
        public static Func<double, double> GetRestrictedParabolicFunc()
        {
            return x =>
            {
                return x * (1 - x) / 0.25;
            };
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
        /// Obtém a função que representa um impulso restrito ao intervalo [0,1).
        /// </summary>
        /// <param name="value">O valor onde será encontrado o impulso.</param>
        /// <param name="delta">O erro em x de modo a que o impulso seja considerado.</param>
        /// <returns>A função restrita.</returns>
        public static Func<double, double> GetRestrictedPulse(
            double value,
            double delta)
        {
            if (value < 0 || value >= 1)
            {
                throw new ArgumentOutOfRangeException(
                    "value",
                    "Pulse value must be greater or equal than zero and less than 1.");
            }
            else if (delta < 0)
            {
                throw new ArgumentException("Delta must be a positive value.");
            }
            else
            {
                return x =>
                {
                    if (Math.Abs(x - value) <= delta)
                    {
                        return 1.0;
                    }
                    else
                    {
                        return 0.0;
                    }
                };
            }
        }

        /// <summary>
        /// Obtém um conjunto de impulsos restritos ao intervalo [0,1).
        /// </summary>
        /// <param name="values">O conjunto de valores.</param>
        /// <param name="delta">O erro em x de modo que o impulso seja considerado.</param>
        /// <returns>A função.</returns>
        public static Func<double, double> GetRestrictedPulse(
            double[] values,
            double delta)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            else if (delta < 0)
            {
                throw new ArgumentException("Delta must be a positive value.");
            }
            else
            {
                var len = values.LongLength;
                Array.Sort(values);
                return x =>
                {
                    var low = 0L;
                    var high = len - 1;
                    while (low + 1 < high)
                    {
                        var sum = high + low;
                        var intermediaryIndex = sum >> 1;
                        var intermediaryElement = values[intermediaryIndex];
                        if (Math.Abs(x - intermediaryElement) <= delta)
                        {
                            return 1.0;
                        }
                        else if (intermediaryElement < x)
                        {
                            low = intermediaryIndex;
                        }
                        else
                        {
                            high = intermediaryIndex;
                        }
                    }

                    var val = values[low];
                    if (Math.Abs(x - val) <= delta)
                    {
                        return 1.0;
                    }
                    else
                    {
                        val = values[high];
                        if (Math.Abs(x - val) <= delta)
                        {
                            return 1.0;
                        }
                        else
                        {
                            return 0.0;
                        }
                    }
                };
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
        /// Polinómios trigonométricos de ângulo múltiplo de primeira espécie.
        /// </summary>
        /// <param name="n">A ordem do polinómio.</param>
        /// <returns>A função que representa o polinómio.</returns>
        public static Func<double, double> GetTchePol(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentException("The order of polynomial must be positive.");
            }
            else
            {
                var pols = new Func<double, double>[]{
                x=>x, // 1
                x=>2 * x * x-1, // 2
                x=> x * (4 * x * x - 3), // 3
                x=>
                {
                    var sq = x * x;
                    return 8 * sq * (sq - 1) + 1;
                }, // 4
                x=>
                {
                    var sq = x * x;
                    return x * (sq * (16 * sq - 20) + 5);
                }, // 5
                x=>
                {
                    var sq = x * x;
                    return sq * (sq * (32 * sq - 48) + 18) - 1;
                }, // 6
                x=>
                {
                    var sq = x * x;
                    var res = 64.0;
                    res *= sq;
                    res -= 112;
                    res *= sq;
                    res += 56;
                    res *= sq;
                    res -= 7;
                    res *= x;
                    return res;
                }, // 7
                x=>
                {
                    var sq = x * x;
                    var coeffs = new double[] { 128, -256, 160, -32, 1 };
                    var res = coeffs[0];
                    for(var i = 1; i < 5; ++i)
                    {
                        res *= sq;
                        res += coeffs[i];
                    }

                    return res;
                }, // 8
                x=>
                {
                    var sq = x * x;
                    var coeffs = new double[] { 256, -576, 432, -120, 9 };
                    var res = coeffs[0];
                    for(var i = 1; i < 5; ++i)
                    {
                        res *= sq;
                        res += coeffs[i];
                    }

                    res *= x;
                    return res;
                }, // 9
                x=>
                {
                    var sq = x * x;
                    var coeffs = new double[] { 512, -1280, 1120, -400, 50, -1 };
                    var res = coeffs[0];
                    for(var i = 1; i < 6; ++i)
                    {
                        res *= sq;
                        res += coeffs[i];
                    }

                    return res;
                }, // 10
                x=>
                {
                    var sq = x * x;
                    var coeffs = new double[] { 1024, -2816, 2816, -1232, 220, -11 };
                    var res = coeffs[0];
                    for(var i = 1; i < 6; ++i)
                    {
                        res *= sq;
                        res += coeffs[i];
                    }

                    res *= x;
                    return res;
                } // 11
            };

                return GetTchePol(
                    n,
                    pols);
            }
        }

        /// <summary>
        /// Polinómios trigonométricos de ângulo múltiplo de primeira espécie.
        /// </summary>
        /// <remarks>Trata-se de uma função recursiva.</remarks>
        /// <param name="n">A ordem do polinómio.</param>
        /// <param name="initials">Os polinómios iniciais.</param>
        /// <returns>A função que representa o polinómio.</returns>
        private static Func<double, double> GetTchePol(
            int n,
            Func<double, double>[] initials)
        {
            var len = initials.LongLength;
            if (n <= len)
            {
                return initials[n - 1];
            }
            else
            {
                if ((n & 1) == 0)
                {
                    var half = n >> 1;
                    var halfPol = GetTchePol(
                        half,
                        initials);
                    return x =>
                    {
                        var res = halfPol.Invoke(x);
                        return 2 * res * res - 1;
                    };
                }
                else
                {
                    var half = (n - 1) >> 1;
                    var halfPol = GetTchePol(half, initials);
                    var halfPolNext = GetTchePol(half + 1, initials);
                    return x =>
                    {
                        var res = 2 * halfPol.Invoke(x);
                        res *= halfPolNext.Invoke(x);
                        return res - x;
                    };
                }
            }
        }
    }

    /// <summary>
    /// Define funções que permitem processar amostras sonoras.
    /// </summary>
    public static class SamplesProcessorFunctions
    {
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
        public static Func<double, double> GetFreqModulatedFunc(
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
        /// Obtém a função que resulta da multiplicação de duas funções.
        /// </summary>
        /// <param name="func1">O primeiro argumento na função.</param>
        /// <param name="func2">O segundo argumento na função.</param>
        /// <returns>A função produto.</returns>
        public static Func<double, double> GetMultFunc(
            Func<double, double> func1,
            Func<double, double> func2)
        {
            return x => func1.Invoke(x) * func2.Invoke(x);
        }

        /// <summary>
        /// Obtém a função soma.
        /// </summary>
        /// <param name="func1">O primeiro argumento na função.</param>
        /// <param name="func2">O segundo argumento na função.</param>
        /// <returns>A função soma.</returns>
        public static Func<double, double> GetAddFunc(
            Func<double, double> func1,
            Func<double, double> func2)
        {
            if (func1 == null)
            {
                throw new ArgumentNullException("func1");
            }
            else if (func2 == null)
            {
                throw new ArgumentNullException("func2");
            }
            else
            {
                return x => func1.Invoke(x) + func2.Invoke(x);
            }
        }

        /// <summary>
        /// Obtém a composta rectroactiva entre duas funções.
        /// </summary>
        /// <remarks>
        /// y[n] = func(y[n-1])
        /// A função não depende do valor de x introduzido.
        /// </remarks>
        /// <param name="func">A função externa.</param>
        /// <param name="startValue">O valor inicial do processo de rectroacção.</param>
        /// <returns>A função composta.</returns>
        public static Func<double, double> GetFeedbackComposite(
            Func<double, double> func,
            double startValue = 0.0)
        {
            if (func == null)
            {
                throw new ArgumentNullException("outer");
            }
            else
            {
                var innerX = startValue;

                return x =>
                {
                    var result = func.Invoke(innerX);
                    innerX = result;
                    return result;
                };
            }
        }

        /// <summary>
        /// Obtém a composta rectroactiva entre duas funções.
        /// </summary>
        /// <remarks>
        /// y[n] = func(x[n], y[n-1])
        /// A função não depende do valor de x introduzido como argumento.
        /// </remarks>
        /// <param name="func">A função externa.</param>
        /// <param name="startValue">O valor inicial do processo de rectroacção.</param>
        /// <returns>A função composta.</returns>
        public static Func<double, double> GetFeedbackComposite(
            Func<double, double, double> func,
            double startValue = 0.0)
        {
            if (func == null)
            {
                throw new ArgumentNullException("outer");
            }
            else
            {
                var innerX = startValue;

                return x =>
                {
                    innerX = func.Invoke(x, innerX);
                    return innerX;
                };
            }
        }

        /// <summary>
        /// Obtém a composta rectroactiva entre duas funções.
        /// </summary>
        /// <remarks>
        /// y[n] = func(feedback(y[n-1]))
        /// A função não depende do valor de x introduzido como argumento.
        /// </remarks>
        /// <param name="func">A função externa.</param>
        /// <param name="feedback">A função de retroacção.</param>
        /// <param name="startValue">O valor inicial do processo de rectroacção.</param>
        /// <returns>A função composta.</returns>
        public static Func<double, double> GetFeedbackComposite(
            Func<double, double> func,
            Func<double, double> feedback,
            double startValue = 0.0)
        {
            if (func == null)
            {
                throw new ArgumentNullException("outer");
            }
            else
            {
                var innerX = startValue;

                return x =>
                {
                    var result = func.Invoke(innerX);
                    innerX = feedback.Invoke(x);
                    return result;
                };
            }
        }

        /// <summary>
        /// Obtém o atraso de uma amostra aplicado sobre a função.
        /// </summary>
        /// <param name="func">A função.</param>
        /// <param name="startValue">O valor inicial.</param>
        /// <returns>A função com o atraso de uma amostra.</returns>
        public static Func<double, double> GetDelay(
            Func<double, double> func,
            double startValue = 0.0)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            else
            {
                var previous = startValue;
                return x =>
                {
                    var result = previous;
                    previous = func.Invoke(x);
                    return result;
                };
            }
        }

        /// <summary>
        /// Obtém um conector de múltiplos retardos.
        /// </summary>
        /// <remarks>
        /// A dimensão do vector de entrada define a ordem dos retardos.
        /// No caso de ser proporcionado um vector vazio para os valores de entrada,
        /// será considerada a função sem retardo. Assim, o delegado retornado pela
        /// função retornará, para além dos valores dos retardos, o valor obtido
        /// para a função sem retardo.
        /// </remarks>
        /// <param name="func">A função de entrada.</param>
        /// <param name="startValues">Os valores iniciais.</param>
        /// <returns>A função com os retardos.</returns>
        public static Func<double, double[]> GetDelay(
            Func<double, double> func,
            double[] startValues)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            else if (startValues == null)
            {
                throw new ArgumentNullException("startValues");
            }
            else
            {
                var length = startValues.LongLength;
                if (length == 0)
                {
                    var result = new double[1];
                    return x =>
                    {
                        result[0] = func.Invoke(x);
                        return result;
                    };
                }
                else
                {
                    var innerStartValues = new double[length];
                    Array.Copy(startValues, innerStartValues, length);

                    var result = new double[length + 1];
                    return x =>
                    {
                        Array.Copy(innerStartValues, 0, result, 1, length);
                        result[0] = func.Invoke(x);
                        Array.Copy(result, innerStartValues, length);
                        return result;
                    };
                }
            }
        }

        /// <summary>
        /// Sínteise por guia de ondas simples.
        /// </summary>
        /// <param name="freq">A frequência do sinal resultante.</param>
        /// <param name="sampleFreq">A frequência de amostragem.</param>
        /// <param name="f">A função inicial.</param>
        /// <returns>A função que implementa o algoritmo.</returns>
        static Func<double, double> KarStrAlg(
            double freq,
            double sampleFreq,
            Func<double, double> f)
        {
            if (f == null)
            {
                throw new ArgumentNullException("f");
            }
            else
            {
                // (N + 1/2)F = fs approx NxF=fs
                var n = (long)(sampleFreq / freq);
                if (n < 2)
                {
                    throw new ArgumentException("Sampling frequency must be as twice as frequency.");
                }
                else
                {
                    var random = new Random();
                    var initialVals = new double[n];
                    for (var i = 0; i < n; ++i)
                    {
                        initialVals[i] = f.Invoke(i / sampleFreq);
                    }

                    var readPointer = 1;
                    var writePointer = 0;

                    return x =>
                    {
                        var res = initialVals[readPointer];
                        ++readPointer;
                        if (readPointer == n)
                        {
                            readPointer = 0;
                        }

                        res += initialVals[writePointer];
                        res *= 0.5;

                        initialVals[writePointer] = res;
                        ++writePointer;
                        if (writePointer == n)
                        {
                            writePointer = 0;
                        }

                        return res;
                    };
                }
            }
        }

        /// <summary>
        /// Obtém um resolutor de equações às diferenças, úteis
        /// no caso de filtros e processadores similares.
        /// </summary>
        /// <param name="func">A função da qual são geradas as amostras.</param>
        /// <param name="xCoeff">O coeficiente multiplicativo da função.</param>
        /// <param name="xDelayCoeffs">
        /// Coeficientes associados às ordens inferiores da função de entrada.</param>
        /// <param name="yDelayCoeffs">
        /// Coeficientes associados às ordens inferiores da função de saída.
        /// </param>
        /// <param name="startXCoeffs">
        /// Os coeficientes iniciais d função.
        /// </param>
        /// <param name="startYCoeffs">
        /// </param>
        /// <returns>O resultor da equação.</returns>
        /// <remarks>
        /// Designando os xDelayCoeffs por 'a' e os yDelayCoeffs
        /// por 'b', tem-se:
        /// y[n]=a[1]*y[n-1]+...+ a[k]*y[n-k] + xCoeff*x[n] + b[1]*x[n-1]+...+b[l]*x[n-l].
        /// </remarks>
        public static Func<double, double> GetDifferenceEquationSolver(
            Func<double, double> func,
            double xCoeff,
            double[] xDelayCoeffs,
            double[] yDelayCoeffs,
            double[] startXCoeffs,
            double[] startYCoeffs)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            else if (xDelayCoeffs == null)
            {
                throw new ArgumentNullException("xDelayCoeffs");
            }
            else if (yDelayCoeffs == null)
            {
                throw new ArgumentNullException("yDelayCoeffs");
            }
            else if (startXCoeffs == null)
            {
                throw new ArgumentNullException("startXCoeffs");
            }
            else if (startYCoeffs == null)
            {
                throw new ArgumentNullException("startYCoeffs");
            }
            else
            {
                var xDelayLen = xDelayCoeffs.LongLength;
                var yDelayLen = yDelayCoeffs.LongLength;
                var startXLen = startXCoeffs.LongLength;
                var startYLen = startYCoeffs.LongLength;
                if (xDelayLen == 0 && yDelayLen == 0)
                {
                    throw new ArgumentException("At least one coefficient must be provided.");
                }
                else if (xDelayLen == startXLen
                    || yDelayLen == startYLen)
                {
                    var innerXDelay = new double[xDelayLen];
                    var innerYDelay = new double[yDelayLen];
                    var innerXStart = new double[startXLen];
                    var innerYStart = new double[startYLen];

                    Array.Copy(xDelayCoeffs, innerXDelay, xDelayLen);
                    Array.Copy(yDelayCoeffs, innerYDelay, yDelayLen);
                    Array.Copy(startXCoeffs, innerXStart, xDelayLen);
                    Array.Copy(startYCoeffs, innerYStart, yDelayLen);

                    return t =>
                    {
                        var x = func.Invoke(t);
                        var res = xCoeff * x;
                        if (xDelayLen > 0)
                        {
                            var i = xDelayLen - 1;
                            res += innerXStart[i] * innerXDelay[i];
                            --i;
                            for (; i >= 0; --i)
                            {
                                var curr = innerXStart[i];
                                res += innerXDelay[i] * curr;
                                innerXStart[i + 1] = curr;
                            }

                            innerXStart[0] = x;
                        }

                        if (yDelayLen > 0)
                        {
                            var i = yDelayLen - 1;
                            res += innerYStart[i] * innerYDelay[i];
                            --i;
                            for (; i >= 0; --i)
                            {
                                var curr = innerYStart[i];
                                res += curr * innerYDelay[i];
                                innerYStart[i + 1] = curr;
                            }

                            innerYStart[0] = res;
                        }

                        return res;
                    };
                }
                else
                {
                    throw new ArgumentException(
                        "Start coefficients don't match delay coeeficients number.");
                }
            }
        }

        /// <summary>
        /// Obtém o integrador aproximado de uma função.
        /// </summary>
        /// <param name="func">A função.</param>
        /// <param name="delta">O valor do intervalo.</param>
        /// <returns>A função que representa o integral aproximado.</returns>
        public static Func<double, double> GetIntegrator(
            Func<double, double> func,
            double delta)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            else if (delta <= 0)
            {
                throw new ArgumentException("Delta must be a positive value.");
            }
            else
            {
                var res = 0.0;
                var curr = 0.0;
                return x =>
                {
                    curr += delta;
                    res += func.Invoke(curr) * delta;
                    return res;
                };
            }
        }

        /// <summary>
        /// Obtém uma função que poderá servir como envolvente.
        /// </summary>
        /// <param name="soundFunc">
        /// A função que define o som da nota.
        /// </param>
        /// <param name="duration">
        /// A duração da nota.
        /// </param>
        /// <param name="attack">
        /// O período em que o som passa do silêncio para o seu volume máximo.
        /// </param>
        /// <param name="hold">
        /// O período de tempo em que o som se encontra no máximo.
        /// </param>
        /// <param name="decay">
        /// O período de decaimento para a sustentação.
        /// </param>
        /// <param name="sustain">O valor da sustentação.</param>
        /// <param name="release">
        /// O período de libertação do som até ao silêncio.
        /// </param>
        /// <returns>A função que representa o envelope.</returns>
        public static Func<double, double> CreateNote(
            Func<double, double> soundFunc,
            double duration,
            double attack,
            double hold,
            double decay,
            double sustain,
            double release)
        {
            if (soundFunc == null)
            {
                throw new ArgumentNullException("soundFunc");
            }
            else if (
               duration < 0 ||
               attack < 0 ||
               hold < 0 ||
               decay < 0 ||
               sustain < 0 ||
               release < 0)
            {
                throw new ArgumentException("Envelope values must be non-negative.");
            }
            else if (sustain > 1)
            {
                throw new ArgumentException("Sustain must be a value less than 1.");
            }
            else
            {
                return x =>
                {
                    var period = attack;
                    if (x <= period)
                    {
                        return (x / attack) * soundFunc.Invoke(x);
                    }
                    else
                    {
                        period += hold;
                        if (x <= period)
                        {
                            return soundFunc.Invoke(x);
                        }
                        else
                        {
                            var prev = period;
                            period += decay;
                            if (x <= duration)
                            {
                                var dec = (x - prev) * (sustain - 1) / decay + 1;
                                return dec * soundFunc.Invoke(x);
                            }
                            else
                            {
                                prev = period;
                                period = duration - release;
                                if (x <= period)
                                {
                                    return sustain;
                                }
                                else
                                {
                                    period += release;
                                    if (x <= period)
                                    {
                                        var dec = (prev - x) * sustain / release + sustain;
                                        return dec * soundFunc.Invoke(x);
                                    }
                                    else
                                    {
                                        return 0.0;
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Escreve um ficheiro wav com o sinal definido pela função.
        /// </summary>
        /// <param name="func">A função.</param>
        /// <param name="timeInSeconds">O tempo em segundos.</param>
        /// <param name="samplesPerSec">Número de amostras por segundo.</param>
        /// <param name="channels">O número de canais.</param>
        /// <param name="path">O caminho para o ficheiro wav.</param>
        public static void WriteWav(
            Func<double, double> func,
            double timeInSeconds,
            uint samplesPerSec,
            ushort channels,
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
                    var dataSize = (uint)Math.Floor(samplesPerSec * timeInSeconds) * 4 * channels;
                    var byteRate = ((channels * 32) >> 3) * samplesPerSec;

                    // Porção: cabeçalho
                    fileStream.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 }, 0, 4); // ChunkID: RIFF
                    fileStream.Write(BitConverter.GetBytes(dataSize + 36), 0, 4); // ChunckSize: datasize + 36
                    fileStream.Write(new byte[] { 0x57, 0x41, 0x56, 0x45 }, 0, 4); // Format: WAVE

                    // Primeira sub-porção: fmt
                    fileStream.Write(new byte[] { 0x66, 0x6d, 0x74, 0x20 }, 0, 4); // SubChunk1ID: ftm\s
                    fileStream.Write(BitConverter.GetBytes(16U), 0, 4); // SubChunk1ID Size: 16
                    fileStream.Write(BitConverter.GetBytes((short)3), 0, 2); //  Audio format: PCM = 1, Microsoft ADPCM = 2, ver mmreg.h
                    fileStream.Write(BitConverter.GetBytes(channels), 0, 2); // Número de canais
                    fileStream.Write(BitConverter.GetBytes(samplesPerSec), 0, 4); // Número de amostras por segundo
                    fileStream.Write(BitConverter.GetBytes(byteRate), 0, 4); // Número de amostras por segundo x número de canais x bits por amostra / 8
                    fileStream.Write(BitConverter.GetBytes((channels * 32) >> 3), 0, 2); // Número de canais x bits por amostra / 8
                    fileStream.Write(BitConverter.GetBytes((short)32), 0, 2); // Bits por amostra

                    // Segunda sub-porção: dados
                    fileStream.Write(new byte[] { 0x64, 0x61, 0x74, 0x61 }, 0, 4);
                    fileStream.Write(BitConverter.GetBytes(dataSize), 0, 4);

                    var delta = 1.0 / samplesPerSec;
                    for (var x = delta; x < timeInSeconds; x += delta)
                    {
                        var res = (float)func.Invoke(x);
                        var bytes = BitConverter.GetBytes(res);
                        for (var i = 0; i < channels; ++i)
                        {
                            fileStream.Write(bytes, 0, 4);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a leitura de um fluxo WAV.
        /// </summary>
        /// <remarks>
        /// Tal função poderá ser usada apenas em ficheiros pequenos.
        /// </remarks>
        /// <param name="stream">O  fluxo.</param>
        /// <returns>A matriz de bytes com as amostras por canal e a taxa de amostragem.</returns>
        public static Tuple<float[,], uint> ReadWav(
            Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else
            {
                var buffer = new byte[4];
                ReadFormat(stream, buffer);
                ValidateFormat(
                    buffer,
                    new byte[] { 0x52, 0x49, 0x46, 0x46 });
                var chunkSize = ReadUInt32(stream, buffer);
                ReadFormat(stream, buffer);
                ValidateFormat(
                    buffer,
                    new byte[] { 0x57, 0x41, 0x56, 0x45 });

                ReadFormat(stream, buffer);
                ValidateFormat(buffer, new byte[] { 0x66, 0x6d, 0x74, 0x20 });
                var subchunkSize = ReadUInt32(stream, buffer);
                var audioFormat = ReadShort(stream, buffer);
                if (audioFormat != 1)
                {
                    throw new UtilitiesException("Only PCM is supported.");
                }

                var channels = ReadShort(stream, buffer);
                if (channels == 0)
                {
                    throw new UtilitiesException("No channel in stream.");
                }

                var samplesPerSec = ReadUInt32(stream, buffer);
                var byteRate = ReadUInt32(stream, buffer);
                var blockAlign = ReadShort(stream, buffer);
                var bitsPerSample = ReadShort(stream, buffer);
                if ((bitsPerSample & 7) != 0)
                {
                    throw new UtilitiesException("Bits per sample must be a multiple of 8.");
                }

                if (bitsPerSample > 32)
                {
                    throw new UtilitiesException("Bits per sample must be less or equal than 32.");
                }

                ReadFormat(stream, buffer);
                ValidateFormat(
                    buffer,
                    new byte[] { 0x64, 0x61, 0x74, 0x61 });
                var dataSize = ReadUInt32(stream, buffer);

                // Processamento dos dados
                var bytesPerSample = bitsPerSample >> 3;
                var maxVal = (float)(int.MaxValue);
                var len = dataSize / blockAlign;
                var result = new float[channels, len];

                if (bitsPerSample == 32)
                {
                    for (var i = 0; i < len; ++i)
                    {
                        for (var j = 0; j < channels; ++j)
                        {
                            var readed = stream.Read(
                                buffer,
                                0,
                                4);
                            if (readed != bytesPerSample)
                            {
                                throw new UtilitiesException("Invalid file.");
                            }

                            var temp = BitConverter.ToInt32(buffer, 0);
                            result[j, i] = temp / maxVal;
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < 4; ++i)
                    {
                        buffer[i] = 0;
                    }

                    for (var i = 0; i < len; ++i)
                    {
                        for (var j = 0; j < channels; ++j)
                        {
                            var readed = stream.Read(
                                buffer,
                                4 - bytesPerSample,
                                bytesPerSample);
                            if (readed != bytesPerSample)
                            {
                                throw new UtilitiesException("Invalid file.");
                            }

                            var temp = BitConverter.ToInt32(buffer, 0);
                            result[j, i] = temp / maxVal;
                        }
                    }
                }

                return Tuple.Create(result, samplesPerSec);
            }
        }

        #region Funçõe privadas

        /// <summary>
        /// Efectua a leitura de formatdos.
        /// </summary>
        /// <param name="stream">A fonte de dados de origem da leitura.</param>
        /// <param name="buffer">O vector de armazenamento temporário da leitura.</param>
        private static void ReadFormat(
            Stream stream,
            byte[] buffer)
        {
            var readed = stream.Read(buffer, 0, 4);
            if (readed != 4)
            {
                throw new UtilitiesException("Invalid file.");
            }
        }

        /// <summary>
        /// Valida o formato lido.
        /// </summary>
        /// <param name="buffer">O vector que contém o formato.</param>
        /// <param name="validation">O validador.</param>
        private static void ValidateFormat(
            byte[] buffer,
            byte[] validation)
        {
            for (var i = 0; i < 4; ++i)
            {
                if (buffer[i] != validation[i])
                {
                    throw new UtilitiesException("Invalid file.");
                }
            }
        }

        /// <summary>
        /// Efectua a leitura de um número inteiro.
        /// </summary>
        /// <param name="stream">A fonte de dados de origem da leitura.</param>
        /// <param name="buffer">O vector de armazenamento temporário da leitura.</param>
        /// <returns>O número.</returns>
        private static uint ReadUInt32(
            Stream stream,
            byte[] buffer)
        {
            var readed = stream.Read(buffer, 0, 4);
            if (readed != 4)
            {
                throw new UtilitiesException("Invalid file.");
            }

            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Efectua a leitura de um inteiro pequeno.
        /// </summary>
        /// <param name="stream">A fonte de dados de origem da leitura.</param>
        /// <param name="buffer">O vector de armazenamento temporário da leitura.</param>
        /// <returns>O inteiro pequeno.</returns>
        private static short ReadShort(
            Stream stream,
            byte[] buffer)
        {
            var readed = stream.Read(buffer, 0, 2);
            if (readed != 2)
            {
                throw new UtilitiesException("Invalid file.");
            }

            return BitConverter.ToInt16(buffer, 0);
        }

        #endregion Funções privadas
    }
}
