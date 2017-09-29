// -----------------------------------------------------------------------
// <copyright file="PrimesGenerators.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Utiliza o crivo para gerar números primos.
    /// </summary>
    /// <remarks>Implementação elementar.</remarks>
    public class PrimeSieveGenerator
    {
        /// <summary>
        /// Mantém os itens a serem analisados.
        /// </summary>
        private bool[] items;

        /// <summary>
        /// Apontador para o primeiro primo não filtrado.
        /// </summary>
        private int notSievedPointer;

        /// <summary>
        /// O quadrado do número não filtrado.
        /// </summary>
        private int notSievedPrimeSquared;

        /// <summary>
        /// Apontador para o último número primo retornado.
        /// </summary>
        private int lastPrimePointer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        public PrimeSieveGenerator()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        public PrimeSieveGenerator(int max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public int GetNextPrime()
        {
            var len = this.items.Length;
            ++this.lastPrimePointer;
            while (this.lastPrimePointer < this.items.Length &&
                !this.items[this.lastPrimePointer])
            {
                ++this.lastPrimePointer;
            }

            if (this.lastPrimePointer == len)
            {
                return 0;
            }
            else
            {
                var increment = 1;
                if (this.notSievedPointer > 0)
                {
                    increment = 2;
                }

                var lastPseudoPrime = this.lastPrimePointer + 2;
                if (lastPseudoPrime == this.notSievedPrimeSquared)
                {
                    // O encontrou o próximo número composto
                    var prime = this.notSievedPointer + 2;
                    if (increment == 1)
                    {
                        for (var i = this.lastPrimePointer; i < len; i += prime)
                        {
                            this.items[i] = false;
                        }
                    }
                    else
                    {
                        for (var i = this.lastPrimePointer; i < len; i += (prime << 1))
                        {
                            this.items[i] = false;
                        }
                    }

                    this.notSievedPointer += increment;
                    while (!this.items[this.notSievedPointer])
                    {
                        this.notSievedPointer += increment;
                    }

                    this.notSievedPrimeSquared = (this.notSievedPointer + 2) * (this.notSievedPointer + 2);

                    this.lastPrimePointer += increment;
                    while (this.lastPrimePointer < this.items.Length &&
                        !this.items[this.lastPrimePointer])
                    {
                        this.lastPrimePointer += increment;
                    }

                    if (this.lastPrimePointer == len)
                    {
                        return -1;
                    }
                    else
                    {
                        return this.lastPrimePointer + 2;
                    }
                }
                else
                {
                    return lastPseudoPrime;
                }
            }
        }

        /// <summary>
        /// Inicializa o crivo.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        private void Initialize(int max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else
            {
                var innerMax = max - 2;
                this.items = new bool[innerMax];
                for (var i = 0; i < innerMax; ++i)
                {
                    this.items[i] = true;
                }

                this.notSievedPrimeSquared = 4;
                this.lastPrimePointer = -1;
            }
        }
    }

    /// <summary>
    /// Crivo para gerar números primos que elimina compostos ímpares.
    /// </summary>
    public class CompositePrimeSieveGenerator
    {
        /// <summary>
        /// Mantém os itens a serem analisados.
        /// </summary>
        private bool[] items;

        /// <summary>
        /// Último item calculado por linha.
        /// </summary>
        private List<int> lastComputed;

        /// <summary>
        /// Indica que o pedido é relativo ao primeiro primo.
        /// </summary>
        private bool firstPrime;

        /// <summary>
        /// O número inicial da sequência.
        /// </summary>
        private int seqNumb;

        /// <summary>
        /// O índice do primo actual no crivo.
        /// </summary>
        private int primeIndex;

        /// <summary>
        /// A variação horizontal.
        /// </summary>
        private int horizontalDelta;

        /// <summary>
        /// A variação vertical.
        /// </summary>
        private int verticalDelta;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CompositePrimeSieveGenerator"/>.
        /// </summary>
        public CompositePrimeSieveGenerator()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CompositePrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O limite máximo.</param>
        public CompositePrimeSieveGenerator(int max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public int GetNextPrime()
        {
            if (this.firstPrime)
            {
                this.firstPrime = false;
                return 2;
            }
            else
            {
                while (this.primeIndex < this.seqNumb)
                {
                    var result = this.primeIndex;
                    ++this.primeIndex;
                    if (!this.items[result])
                    {
                        return (result << 1) +3;
                    }
                }

                if (this.seqNumb < this.items.Length)
                {
                    this.lastComputed.Add(this.seqNumb);
                    this.seqNumb += this.verticalDelta;
                    if (this.seqNumb > this.items.Length)
                    {
                        this.seqNumb = this.items.Length;
                    }

                    var currHorDelta = this.horizontalDelta;
                    for (var i = this.lastComputed.Count - 1; i >= 0; --i)
                    {
                        var lastComp = this.lastComputed[i];
                        while (lastComp < this.seqNumb)
                        {
                            this.items[lastComp] = true;
                            lastComp += currHorDelta;
                        }

                        currHorDelta -= 2;
                    }

                    this.verticalDelta += 4;
                    this.horizontalDelta += 2;

                    while (this.primeIndex < this.seqNumb)
                    {
                        var result = this.primeIndex;
                        ++this.primeIndex;
                        if (!this.items[result])
                        {
                            return (result << 1) + 3;
                        }
                    }

                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Determina um factor de um número de acordo com a técnica utilizada
        /// no crivo.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O factor.</returns>
        public static uint FindFactor(uint number)
        {
            if (number == 2)
            {
                return 1;
            }
            if ((number & 1) == 0)
            {
                return 2;
            }
            else
            {
                var limit = (number - 1) >> 1;

                // Verifica se o valor de limit é da forma i+j+2ij
                var vd = 8;
                var hd = 3;
                var n = 4;
                var i = 1;
                while (n < limit)
                {
                    if ((limit - n) % hd == 0)
                    {
                        return (uint)((i << 1) + 1);
                    }
                    else
                    {
                        n += vd;
                        vd += 4;
                        hd += 2;
                        ++i;
                    }
                }

                if (limit == n)
                {
                    return (uint)((i << 1) + 1);
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// Inicializa o crivo.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        private void Initialize(int max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else
            {
                this.items = new bool[(max - 1) >> 1];
                this.firstPrime = true;
                this.lastComputed = new List<int>(100);
                this.seqNumb = 3;
                this.horizontalDelta = 3;
                this.verticalDelta = 8;
            }
        }

        /// <summary>
        /// Função que permite imprimir primos.
        /// </summary>
        /// <remarks>
        /// A função serve o propósito de definir uma implementação.
        /// </remarks>
        /// <param name="max">O limite para os primos.</param>
        private static void PrintPrimes(int max)
        {
            var limit = (max - 1) >> 1;

            var items = new bool[limit];
            var n = 3;
            var i = 0;
            var hd = 3;
            var hv = 8;
            while (n < limit)
            {
                var innerVar = n;
                while (innerVar < limit)
                {
                    items[innerVar] = true;
                    innerVar += hd;
                }

                hd += 2;
                n += hv;
                hv += 4;
            }

            for (i = 0; i < limit; ++i)
            {
                if (!items[i])
                {
                    Console.WriteLine((i << 1) + 3);
                }
            }
        }
    }
}
