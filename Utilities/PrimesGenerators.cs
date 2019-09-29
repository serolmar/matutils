// -----------------------------------------------------------------------
// <copyright file="PrimesGenerators.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections;
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
        /// Apontador para o primeiro número não filtrado.
        /// </summary>
        private uint notSievedPointer;

        /// <summary>
        /// O quadrado do número não filtrado.
        /// </summary>
        private uint notSievedPrimeSquared;

        /// <summary>
        /// Apontador para o último número primo retornado.
        /// </summary>
        private uint lastPrimePointer;

        /// <summary>
        /// O valor do incremento.
        /// </summary>
        private uint increment;

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
        public PrimeSieveGenerator(uint max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public uint GetNextPrime()
        {
            var len = this.items.Length;
            while (this.lastPrimePointer < len &&
                this.items[this.lastPrimePointer])
            {
                this.lastPrimePointer += increment;
            }

            if (this.lastPrimePointer >= len)
            {
                return 0;
            }
            else
            {
                var lastPseudoPrime = this.lastPrimePointer + 2;
                if (lastPseudoPrime == this.notSievedPrimeSquared)
                {
                    // Encontrou o próximo número composto
                    var prime = this.notSievedPointer + 2;
                    if (this.increment > 1)
                    {
                        prime <<= 1;
                        for (var i = this.lastPrimePointer; i < len; i += prime)
                        {
                            this.items[i] = true;
                        }
                    }

                    this.notSievedPointer += this.increment;
                    while (this.items[this.notSievedPointer])
                    {
                        this.notSievedPointer += this.increment;
                    }

                    this.notSievedPrimeSquared = (this.notSievedPointer + 2) * (this.notSievedPointer + 2);

                    this.lastPrimePointer += this.increment;
                    while (this.lastPrimePointer < len &&
                        this.items[this.lastPrimePointer])
                    {
                        this.lastPrimePointer += this.increment;
                    }

                    this.increment = 2;
                    if (this.lastPrimePointer == len)
                    {
                        return 0;
                    }
                    else
                    {
                        var result = this.lastPrimePointer + 2;
                        this.lastPrimePointer += this.increment;
                        return result;
                    }
                }
                else
                {
                    this.lastPrimePointer += this.increment;
                    return lastPseudoPrime;
                }
            }
        }

        /// <summary>
        /// Inicializa o crivo.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        private void Initialize(uint max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else
            {
                var innerMax = max - 2;
                this.items = new bool[innerMax];
                this.increment = 1;
                this.notSievedPointer = 0;
                this.notSievedPrimeSquared = 4;
                this.lastPrimePointer = 0;
            }
        }
    }

    /// <summary>
    /// Utiliza o crivo para gerar números primos.
    /// </summary>
    /// <remarks>Implementação elementar.</remarks>
    public class PrimeSieveGenBinContainerV1
    {
        /// <summary>
        /// Mantém os itens a serem analisados.
        /// </summary>
        private BitArray items;

        /// <summary>
        /// Apontador para o primeiro número não filtrado.
        /// </summary>
        private uint notSievedPointer;

        /// <summary>
        /// O quadrado do número não filtrado.
        /// </summary>
        private uint notSievedPrimeSquared;

        /// <summary>
        /// Apontador para o último número primo retornado.
        /// </summary>
        private uint lastPrimePointer;

        /// <summary>
        /// O valor do incremento.
        /// </summary>
        private uint increment;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        public PrimeSieveGenBinContainerV1()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        public PrimeSieveGenBinContainerV1(uint max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public uint GetNextPrime()
        {
            var len = this.items.Length;
            while (this.lastPrimePointer < len &&
                this.items[(int)this.lastPrimePointer])
            {
                this.lastPrimePointer += increment;
            }

            if (this.lastPrimePointer >= len)
            {
                return 0;
            }
            else
            {
                var lastPseudoPrime = this.lastPrimePointer + 2;
                if (lastPseudoPrime == this.notSievedPrimeSquared)
                {
                    // Encontrou o próximo número composto
                    var prime = (int)this.notSievedPointer + 2;
                    if (this.increment > 1)
                    {
                        prime <<= 1;
                        for (var i = (int)this.lastPrimePointer; i < len; i += prime)
                        {
                            this.items[i] = true;
                        }
                    }

                    this.notSievedPointer += this.increment;
                    while (this.items[(int)this.notSievedPointer])
                    {
                        this.notSievedPointer += this.increment;
                    }

                    this.notSievedPrimeSquared = (this.notSievedPointer + 2) * (this.notSievedPointer + 2);

                    this.lastPrimePointer += this.increment;
                    while (this.lastPrimePointer < len &&
                        this.items[(int)this.lastPrimePointer])
                    {
                        this.lastPrimePointer += this.increment;
                    }

                    this.increment = 2;
                    if (this.lastPrimePointer == len)
                    {
                        return 0;
                    }
                    else
                    {
                        var result = this.lastPrimePointer + 2;
                        this.lastPrimePointer += this.increment;
                        return result;
                    }
                }
                else
                {
                    this.lastPrimePointer += this.increment;
                    return lastPseudoPrime;
                }
            }
        }

        /// <summary>
        /// Inicializa o crivo.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        private void Initialize(uint max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else
            {
                var innerMax = max - 2;
                this.items = new BitArray((int)innerMax);
                this.increment = 1;
                this.notSievedPointer = 0;
                this.notSievedPrimeSquared = 4;
                this.lastPrimePointer = 0;
            }
        }
    }

    /// <summary>
    /// Utiliza o crivo para gerar números primos.
    /// </summary>
    /// <remarks>Implementação elementar com contentor binário.</remarks>
    public class PrimeSieveGenBinContainer
    {
        /// <summary>
        /// Mantém os itens a serem analisados.
        /// </summary>
        private ulong[] items;

        /// <summary>
        /// O número máximo a ser considerado.
        /// </summary>
        private ulong max;

        /// <summary>
        /// Apontador para o primeiro número não filtrado.
        /// </summary>
        private ulong notSievedPointer;

        /// <summary>
        /// O quadrado do número não filtrado.
        /// </summary>
        private ulong notSievedPrimeSquared;

        /// <summary>
        /// O último número primo retornado.
        /// </summary>
        private ulong lastPrimePointer;

        /// <summary>
        /// Mantém o valor do incremento.
        /// </summary>
        private int increment;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        public PrimeSieveGenBinContainer()
        {
            this.Initialize(100UL);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        public PrimeSieveGenBinContainer(ulong max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public ulong GetNextPrime()
        {
            var mainPointer = this.lastPrimePointer >> 6;
            var remPointer = (int)(this.lastPrimePointer & 0x3F);
            while (this.lastPrimePointer < this.max &&
                (this.items[mainPointer] & (1UL << remPointer)) != 0)
            {
                this.lastPrimePointer += (ulong)this.increment;
                mainPointer = this.lastPrimePointer >> 6;
                remPointer = (int)(this.lastPrimePointer & 0x3F);
            }

            if (this.lastPrimePointer >= this.max)
            {
                return 0;
            }
            else
            {
                var lastPseudoPrime = this.lastPrimePointer + 2;
                if (lastPseudoPrime == this.notSievedPrimeSquared)
                {
                    // Encontrou o próximo número composto
                    var prime = this.notSievedPointer + 2;
                    if (this.increment == 1)
                    {
                        for (var i = this.lastPrimePointer; i < this.max; i += prime)
                        {
                            mainPointer = i >> 6;
                            remPointer = (int)(i & 0x3F);
                            this.items[mainPointer] |= (1UL << remPointer);
                        }
                    }
                    else
                    {
                        prime <<= 1;
                        for (var i = this.lastPrimePointer; i < this.max; i += prime)
                        {
                            mainPointer = i >> 6;
                            remPointer = (int)(i & 0x3F);
                            this.items[mainPointer] |= (1UL << remPointer);
                        }
                    }

                    this.notSievedPointer += (ulong)this.increment;
                    mainPointer = this.notSievedPointer >> 6;
                    remPointer = (int)(this.notSievedPointer & 0x3F);
                    while ((this.items[mainPointer] & (1UL << remPointer)) != 0)
                    {
                        this.notSievedPointer += (ulong)this.increment;
                        mainPointer = this.notSievedPointer >> 6;
                        remPointer = (int)(this.notSievedPointer & 0x3F);
                    }

                    this.notSievedPrimeSquared = (this.notSievedPointer + 2) * (this.notSievedPointer + 2);

                    this.lastPrimePointer += (ulong)this.increment;
                    mainPointer = this.lastPrimePointer >> 6;
                    remPointer = (int)(this.lastPrimePointer & 0x3F);
                    while (this.lastPrimePointer < this.max &&
                        (this.items[mainPointer] & (1UL << remPointer)) != 0)
                    {
                        this.lastPrimePointer += (ulong)this.increment;
                        mainPointer = this.lastPrimePointer >> 6;
                        remPointer = (int)(this.lastPrimePointer & 0x3F);
                    }

                    this.increment = 2;
                    if (this.lastPrimePointer == this.max)
                    {
                        return 0;
                    }
                    else
                    {
                        var result = this.lastPrimePointer + 2;
                        this.lastPrimePointer += (ulong)this.increment;
                        return result;
                    }
                }
                else
                {
                    this.lastPrimePointer += (ulong)this.increment;
                    return lastPseudoPrime;
                }
            }
        }

        /// <summary>
        /// Inicializa o crivo.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        private void Initialize(ulong max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else
            {
                var innerMax = (ulong)((max - 2) >> 6);
                var rem = (int)((max - 2) & 0x3F);
                if (rem != 0)
                {
                    ++innerMax;
                }

                this.max = max - 2;
                this.items = new ulong[innerMax];
                this.notSievedPointer = 0UL;
                this.notSievedPrimeSquared = 4UL;
                this.lastPrimePointer = 0UL;
                this.increment = 1;
            }
        }
    }

    /// <summary>
    /// Crivo para gerar números primos que elimina compostos ímpares.
    /// </summary>
    public class LevelOnePrimeSieveGenerator
    {
        /// <summary>
        /// Mantém os itens a serem analisados.
        /// </summary>
        private bool[] items;

        /// <summary>
        /// Indica que o pedido é relativo ao primeiro primo.
        /// </summary>
        private bool firstPrime;

        /// <summary>
        /// A variação horizontal.
        /// </summary>
        private uint horizontalDelta;

        /// <summary>
        /// Apontador para o primeiro número não filtrado.
        /// </summary>
        private uint notSievedPointer;

        /// <summary>
        /// O valor da sequência relativa ao número não filtrado.
        /// </summary>
        private uint notSievedPrimeSeq;

        /// <summary>
        /// Apontador para o último número primo retornado.
        /// </summary>
        private uint lastPrimePointer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        public LevelOnePrimeSieveGenerator()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        public LevelOnePrimeSieveGenerator(uint max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public uint GetNextPrime()
        {
            if (this.firstPrime)
            {
                this.firstPrime = false;
                return 2;
            }
            else
            {
                var len = this.items.Length;
                while (this.lastPrimePointer < len &&
                    this.items[this.lastPrimePointer])
                {
                    ++this.lastPrimePointer;
                }

                if (this.lastPrimePointer >= len)
                {
                    return 0;
                }
                else
                {
                    if (this.lastPrimePointer == this.notSievedPrimeSeq)
                    {
                        // Encontrou o próximo número composto
                        for (var i = this.lastPrimePointer; i < len; i += this.horizontalDelta)
                        {
                            this.items[i] = true;
                        }

                        ++this.notSievedPointer;
                        this.horizontalDelta += 2;
                        while (this.items[this.notSievedPointer])
                        {
                            ++this.notSievedPointer;
                            this.horizontalDelta += 2;
                        }

                        this.notSievedPrimeSeq = 6 * this.notSievedPointer +
                            ((this.notSievedPointer * this.notSievedPointer) << 1) + 3;

                        ++this.lastPrimePointer;
                        while (this.lastPrimePointer < len &&
                            this.items[this.lastPrimePointer])
                        {
                            ++this.lastPrimePointer;
                        }

                        if (this.lastPrimePointer == len)
                        {
                            return 0;
                        }
                        else
                        {
                            var result = (this.lastPrimePointer << 1) + 3;
                            ++this.lastPrimePointer;
                            return result;
                        }
                    }
                    else
                    {
                        var result = (this.lastPrimePointer << 1) + 3;
                        ++this.lastPrimePointer;
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Inicializa o crivo.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        private void Initialize(uint max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else
            {
                var innerMax = (max - 1) >> 1;
                this.items = new bool[innerMax];

                this.firstPrime = true;
                this.notSievedPointer = 0;
                this.notSievedPrimeSeq = 3;
                this.lastPrimePointer = 0;
                this.horizontalDelta = 3;
            }
        }
    }

    /// <summary>
    /// Crivo para gerar números primos que elimina compostos ímpares.
    /// </summary>
    public class LevelTwoPrimeSieveGenerator
    {
        /// <summary>
        /// Mantém os itens módulo 5 a serem analisados.
        /// </summary>
        private bool[] items1;

        /// <summary>
        /// Mantém os itens módulo 1 a serem analisados.
        /// </summary>
        private bool[] items2;

        /// <summary>
        /// O estado de retorno do gerador.
        /// </summary>
        private byte state;

        /// <summary>
        /// A primeira variação horizontal.
        /// </summary>
        private uint delta1;

        /// <summary>
        /// A segunda variação horizontal.
        /// </summary>
        private uint delta2;

        /// <summary>
        /// Apontador para o primeiro número não filtrado.
        /// </summary>
        private uint notSievedPointer;

        /// <summary>
        /// O valor da sequência relativa ao número não filtrado.
        /// </summary>
        private uint notSievedPrimeSeq;

        /// <summary>
        /// Apontador para o último número primo retornado.
        /// </summary>
        private uint lastPrimePointer;

        /// <summary>
        /// Mantém o valor actual.
        /// </summary>
        private uint current;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        public LevelTwoPrimeSieveGenerator()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        public LevelTwoPrimeSieveGenerator(uint max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public uint GetNextPrime()
        {
            if (this.state == 0)
            {
                ++this.state;
                return 2;
            }
            else if (this.state == 1)
            {
                ++this.state;
                return 3;
            }
            else if (this.state == 2)
            {
                var len2 = this.items2.Length;
                while (this.lastPrimePointer < len2 &&
                    this.items1[this.lastPrimePointer] &&
                    this.items2[this.lastPrimePointer])
                {
                    ++this.lastPrimePointer;
                }

                if (this.lastPrimePointer >= len2)
                {
                    this.state = 255;
                    len2 = this.items1.Length;
                    if (this.lastPrimePointer < len2 &&
                        !this.items1[this.lastPrimePointer])
                    {
                        return 6 * this.lastPrimePointer + 5;
                    }
                    else
                    {
                        this.state = 255;
                        return 0;
                    }
                }
                else
                {
                    if (this.lastPrimePointer == this.notSievedPrimeSeq)
                    {
                        // Encontrou o próximo número composto
                        var len1 = this.items1.LongLength;
                        var temp = (this.notSievedPointer + 1) << 1;

                        if (!this.items1[this.notSievedPointer])
                        {
                            var init = this.lastPrimePointer;
                            for (var i = init; i < len2; i += this.delta1)
                            {
                                this.items2[i] = true;
                            }

                            init += temp;
                            for (var i = init; i < len1; i += this.delta1)
                            {
                                this.items1[i] = true;
                            }
                        }

                        if (!this.items2[this.notSievedPointer])
                        {
                            var init = this.lastPrimePointer  + (temp << 1);
                            for (var i = init; i < len2; i += this.delta2)
                            {
                                this.items2[i] = true;
                            }

                            init += 5 + (this.notSievedPointer << 2);
                            for (var i = init; i < len1; i += this.delta2)
                            {
                                this.items1[i] = true;
                            }
                        }

                        ++this.notSievedPointer;
                        this.delta1 += 6;
                        this.delta2 += 6;
                        var control = true;
                        while (control)
                        {
                            if (this.items1[this.notSievedPointer])
                            {
                                if (!this.items2[this.notSievedPointer])
                                {
                                    var init = 7 + this.notSievedPointer *
                                        (14 + 6 * this.notSievedPointer);
                                    for (var i = init; i < len2; i += this.delta2)
                                    {
                                        this.items2[i] = true;
                                    }

                                    init += 5 + (this.notSievedPointer << 2);
                                    for (var i = init; i < len1; i += this.delta2)
                                    {
                                        this.items1[i] = true;
                                    }
                                }

                                ++this.notSievedPointer;
                                this.delta1 += 6;
                                this.delta2 += 6;
                            }
                            else
                            {
                                control = false;
                            }
                        }

                        this.notSievedPrimeSeq = 3 + this.notSievedPointer *
                            (10 + 6 * this.notSievedPointer);

                        if (this.items1[this.lastPrimePointer])
                        {
                            ++this.lastPrimePointer;
                            while (this.lastPrimePointer < len2 &&
                                this.items1[this.lastPrimePointer] &&
                                this.items2[this.lastPrimePointer])
                            {
                                ++this.lastPrimePointer;
                            }

                            if (this.lastPrimePointer == len2)
                            {
                                this.state = 255;
                                len2 = this.items1.Length;
                                if (this.lastPrimePointer < len2 &&
                                    !this.items1[this.lastPrimePointer])
                                {
                                    return 6 * this.lastPrimePointer + 5;
                                }
                                else
                                {
                                    this.state = 255;
                                    return 0;
                                }
                            }
                            else
                            {
                                if (this.items1[this.lastPrimePointer])
                                {
                                    var result = 6 * this.lastPrimePointer + 7;
                                    ++this.lastPrimePointer;
                                    return result;
                                }
                                else
                                {
                                    var result = 6 * this.lastPrimePointer + 5;
                                    if (!this.items2[this.lastPrimePointer])
                                    {
                                        this.current = result + 2;
                                        this.state = 3;
                                    }

                                    ++this.lastPrimePointer;
                                    return result;
                                }
                            }
                        }
                        else
                        {
                            if (this.items1[this.lastPrimePointer])
                            {
                                var result = 6 * this.lastPrimePointer + 7;
                                ++this.lastPrimePointer;
                                return result;
                            }
                            else
                            {
                                var result = 6 * this.lastPrimePointer + 5;
                                if (!this.items2[this.lastPrimePointer])
                                {
                                    this.current = result + 2;
                                    this.state = 3;
                                }

                                ++this.lastPrimePointer;
                                return result;
                            }
                        }
                    }
                    else
                    {
                        if (this.items1[this.lastPrimePointer])
                        {
                            var result = 6 * this.lastPrimePointer + 7;
                            ++this.lastPrimePointer;
                            return result;
                        }
                        else
                        {
                            var result = 6 * this.lastPrimePointer + 5;
                            if (!this.items2[this.lastPrimePointer])
                            {
                                this.current = result + 2;
                                this.state = 3;
                            }

                            ++this.lastPrimePointer;
                            return result;
                        }
                    }
                }
            }
            else if (this.state == 3)
            {
                this.state = 2;
                return this.current;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Inicializa o crivo.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        private void Initialize(uint max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else
            {
                var len = max / 6;
                var rem = max % 6;
                if (rem > 1)
                {
                    this.items2 = new bool[len];
                    if (rem > 4)
                    {
                        ++len;
                    }

                    this.items1 = new bool[len];
                }
                else
                {
                    this.items1 = new bool[len];
                    --len;
                    this.items2 = new bool[len];
                }

                this.state = 0;
                this.notSievedPointer = 0;
                this.notSievedPrimeSeq = 3;
                this.lastPrimePointer = 0;
                this.delta1 = 5;
                this.delta2 = 7;
            }
        }
    }

    /// <summary>
    /// Implementa um gerador de primos de terceiro nível.
    /// </summary>
    public class LevelThreePrimeSieveGenerator
    {
        /// <summary>
        /// Mantém a lista dos primeiros números primos.
        /// </summary>
        private static long[] firstPrimes = new long[]{
            2, 3, 5
        };

        /// <summary>
        /// Mantém os valores da roda de terceiro nível.
        /// </summary>
        /// <remarks>
        /// Tabela de multiplicação modular:
        /// {4, 3, 7, 6, 2, 1, 5, 0},
        ///  {3, 7, 5, 0, 6, 2, 4, 1},
        ///  {7, 5, 4, 1, 0, 6, 3, 2},
        ///  {6, 0, 1, 4, 5, 7, 2, 3},
        ///  {2, 6, 0, 5, 7, 3, 1, 4},
        ///  {1, 2, 6, 7, 3, 4, 0, 5},
        ///  {5, 4, 3, 2, 1, 0, 7, 6},
        ///  {0, 1, 2, 3, 4, 5, 6, 7}
        /// </remarks>
        private static long[] wheel = new long[]{
            7, 11, 13, 17, 19, 23, 29, 31
        };

        /// <summary>
        /// O número máximo a ser considerado.
        /// </summary>
        private long max;

        /// <summary>
        /// O conjunto dos crivos.
        /// </summary>
        private bool[][] items;

        /// <summary>
        /// O vector das diferenças.
        /// </summary>
        private long[] deltas;

        /// <summary>
        /// O apontador para o bloco actual.
        /// </summary>
        private long currentPointer;

        /// <summary>
        /// O produto actual.
        /// </summary>
        private long product;

        /// <summary>
        /// Um apontador para a roda.
        /// </summary>
        private long wheelPointer;

        /// <summary>
        /// O limite de números finais a serem considerados na roda.
        /// </summary>
        private long wheelPointerLimit;

        /// <summary>
        /// O apontador para o número sem crivar.
        /// </summary>
        private long notSievedNumbPointer;

        /// <summary>
        /// Um produto auxiliar.
        /// </summary>
        private long notSievedNumbProd;

        /// <summary>
        /// A posição do primeiro múltiplo sem crivar.
        /// </summary>
        private long notSievedMultPos;

        /// <summary>
        /// Mantém o valor do estado do algoritmo.
        /// </summary>
        private short state;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LevelThreePrimeSieveGenerator"/>.
        /// </summary>
        public LevelThreePrimeSieveGenerator()
        {
            this.Initialize(100L);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LevelThreePrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O número máximo para os primos.</param>
        public LevelThreePrimeSieveGenerator(long max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime less than two.");
            }
            else
            {
                this.Initialize(max);
            }
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public long GetNextPrime()
        {
            if (this.state == 0)
            {
                var current = firstPrimes[this.currentPointer];
                if (current > this.max)
                {
                    this.state = 255;
                    return 0L;
                }
                else
                {
                    ++this.currentPointer;
                    if (this.currentPointer == firstPrimes.LongLength)
                    {
                        this.InitializeSeq();
                        this.state = 1;
                    }

                    return current;
                }
            }
            else if (this.state == 1)
            {
                var len31 = this.items[7].LongLength;
                while (this.currentPointer < len31 &&
                    this.AssertMarks())
                {
                    ++this.currentPointer;
                }

                if (this.currentPointer >= len31)
                {
                    this.wheelPointerLimit = 0;
                    for (var i = 6; i >= 0; --i)
                    {
                        var item = this.items[i];
                        var innerLen = item.LongLength;
                        if (innerLen > len31)
                        {
                            this.wheelPointerLimit = i + 1;
                            i = -1;
                        }
                    }

                    this.wheelPointer = 0L;
                    if (this.wheelPointerLimit == 1)
                    {
                        // Último número
                        this.state = 255;
                        if (!this.items[this.wheelPointer][this.currentPointer])
                        {
                            return 30 * this.currentPointer + wheel[this.wheelPointer];
                        }
                        else
                        {
                            return 0L;
                        };
                    }
                    else if (this.wheelPointerLimit > 1)
                    {
                        for (; this.wheelPointer < this.wheelPointerLimit; ++this.wheelPointer)
                        {
                            if (!this.items[this.wheelPointer][this.currentPointer])
                            {
                                this.state = 3;
                                this.product = 30 * this.currentPointer;
                                return this.product + wheel[this.wheelPointer];
                            }
                        }

                        this.state = 255;
                        return 0L;
                    }
                    else
                    {
                        this.state = 255;
                        return 0L;
                    }
                }
                else
                {
                    if (this.currentPointer == this.notSievedMultPos)
                    {
                        return this.SieveNext();
                    }
                    else
                    {
                        this.product = 30 * this.currentPointer;
                        var current = this.product + wheel[this.wheelPointer];
                        ++this.wheelPointer;
                        for (; this.wheelPointer < 8; ++this.wheelPointer)
                        {
                            if (!this.items[this.wheelPointer][this.currentPointer])
                            {
                                this.state = 2;
                                return current;
                            }
                        }

                        ++this.currentPointer;
                        return current;
                    }
                }
            }
            else if (this.state == 2)
            {
                var current = this.product + wheel[this.wheelPointer];
                ++this.wheelPointer;
                for (; this.wheelPointer < 8; ++this.wheelPointer)
                {
                    if (!this.items[this.wheelPointer][this.currentPointer])
                    {
                        return current;
                    }
                }

                ++this.currentPointer;
                this.state = 1;
                return current;
            }
            else if (this.state == 3)
            {
                ++this.wheelPointer;
                for (; this.wheelPointer < this.wheelPointerLimit; ++this.wheelPointer)
                {
                    if (!this.items[this.wheelPointer][this.currentPointer])
                    {
                        return this.product + wheel[this.wheelPointer];
                    }
                }

                this.state = 255;
                return 0L;
            }
            else
            {
                return 0L;
            }
        }

        /// <summary>
        /// Aplica o próximo filtro e determina o próximo valor.
        /// </summary>
        /// <returns>O valor.</returns>
        private long SieveNext()
        {
            var item = this.items[0];
            if (!item[this.notSievedNumbPointer])
            {
                this.MarkFirstWitouhtVerification();
            }

            this.MarkAllExceptFirst();

            ++this.notSievedNumbPointer;
            this.notSievedNumbProd += 30;
            this.IncreaseAllDeltas();
            var control = true;
            while (control)
            {
                if (item[this.notSievedNumbPointer])
                {
                    this.MarkAllExceptFirst();
                    ++this.notSievedNumbPointer;
                    this.notSievedNumbProd += 30;
                    this.IncreaseAllDeltas();
                }
                else
                {
                    control = false;
                }
            }

            this.notSievedMultPos = 1 + this.notSievedNumbPointer *
                (14 + 30 * this.notSievedNumbPointer);

            // Determina o próximo número
            this.wheelPointer = 8;
            for (var i = 0; i < 8; ++i)
            {
                if (!this.items[i][this.currentPointer])
                {
                    this.wheelPointer = i;
                    i = 8;
                }
            }

            if (this.wheelPointer == 8)
            {
                var len31 = this.items[7].LongLength;
                while (this.currentPointer < len31 &&
                    this.AssertMarks())
                {
                    ++this.currentPointer;
                }

                this.product = 30 * this.currentPointer;
                var current = this.product + wheel[this.wheelPointer];
                ++this.wheelPointer;
                for (; this.wheelPointer < 8; ++this.wheelPointer)
                {
                    if (!this.items[this.wheelPointer][this.currentPointer])
                    {
                        this.state = 2;
                        return current;
                    }
                }

                ++this.currentPointer;
                this.state = 1;
                return current;
            }
            else
            {
                this.product = 30 * this.currentPointer;
                var current = this.product + wheel[this.wheelPointer];
                ++this.wheelPointer;
                for (; this.wheelPointer < 8; ++this.wheelPointer)
                {
                    if (!this.items[this.wheelPointer][this.currentPointer])
                    {
                        this.state = 2;
                        return current;
                    }
                }

                ++this.currentPointer;
                return current;
            }
        }

        /// <summary>
        /// Verifica se todos os itens se encontram marcados.
        /// </summary>
        /// <returns>
        /// Verdadeiro caso os itens se encontrem marcados e falso caso contrário.
        /// </returns>
        private bool AssertMarks()
        {
            var result = true;
            for (var i = 0; i < 8; ++i)
            {
                var vec = this.items[i];
                if (!vec[this.currentPointer])
                {
                    this.wheelPointer = i;
                    result = false;
                    i = 8;
                }
            }

            return result;
        }

        /// <summary>
        /// Marca todos os vectores com excepção do primeiro.
        /// </summary>
        /// <remarks>
        /// O primeiro vector servirá de controlo.
        /// </remarks>
        private void MarkAllExceptFirst()
        {
            var i = this.notSievedNumbPointer;
            var diff2 = i << 1;
            var diff4 = diff2 << 1;
            var diff6 = diff2 + diff4;
            var value = 2 + i * (18 + 30 * i);
            var init = value;

            if (!this.items[1][i])
            {
                var delta = this.deltas[1];
                this.MarkVector(this.items[3], init, delta);
                init += 1 + diff4;
                this.MarkVector(this.items[7], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[5], init, delta);
                init += 2 + diff4;
                this.MarkVector(this.items[0], init, delta);
                init += diff2;
                this.MarkVector(this.items[6], init, delta);
                init += 2 + diff4;
                this.MarkVector(this.items[2], init, delta);
                init += 2 + diff6;
                this.MarkVector(this.items[4], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[1], init, delta);
            }

            if (!this.items[2][i])
            {
                var delta = this.deltas[2];
                init = value + diff2;
                this.MarkVector(this.items[7], init, delta);
                init += 2 + diff4;
                this.MarkVector(this.items[5], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[4], init, delta);
                init += 2 + diff4;
                this.MarkVector(this.items[1], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[0], init, delta);
                init += 1 + diff4;
                this.MarkVector(this.items[6], init, delta);
                init += 3 + diff6;
                this.MarkVector(this.items[3], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[2], init, delta);
            }

            if (!this.items[3][i])
            {
                var delta = this.deltas[3];
                init = value + 1 + diff6;

                this.MarkVector(this.items[6], init, delta);
                init += 3 + diff4;
                this.MarkVector(this.items[0], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[1], init, delta);
                init += 2 + diff4;
                this.MarkVector(this.items[4], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[5], init, delta);
                init += 2 + diff4;
                this.MarkVector(this.items[7], init, delta);
                init += 4 + diff6;
                this.MarkVector(this.items[2], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[3], init, delta);
            }

            if (!this.items[4][i])
            {
                var delta = this.deltas[4];
                init = value + 2 + (diff4 << 1);

                this.MarkVector(this.items[2], init, delta);
                init += 2 + diff4;
                this.MarkVector(this.items[6], init, delta);
                init += 2 + diff2;
                this.MarkVector(this.items[0], init, delta);
                init += 2 + diff4;
                this.MarkVector(this.items[5], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[7], init, delta);
                init += 3 + diff4;
                this.MarkVector(this.items[3], init, delta);
                init += 4 + diff6;
                this.MarkVector(this.items[1], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[4], init, delta);
            }

            if (!this.items[5][i])
            {
                var delta = this.deltas[5];
                init = value + 3 + (diff6 << 1);

                this.MarkVector(this.items[1], init, delta);
                init += 3 + diff4;
                this.MarkVector(this.items[2], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[6], init, delta);
                init += 3 + diff4;
                this.MarkVector(this.items[7], init, delta);
                init += 2 + diff2;
                this.MarkVector(this.items[3], init, delta);
                init += 3 + diff4;
                this.MarkVector(this.items[4], init, delta);
                init += 5 + diff6;
                this.MarkVector(this.items[0], init, delta);
                init += 1 + diff2;
                this.MarkVector(this.items[5], init, delta);
            }

            if (!this.items[6][i])
            {
                var delta = this.deltas[6];
                init = value + 4 + (diff6 << 1) + diff6;

                this.MarkVector(this.items[5], init, delta);
                init += 4 + diff4;
                this.MarkVector(this.items[4], init, delta);
                init += 2 + diff2;
                this.MarkVector(this.items[3], init, delta);
                init += 4 + diff4;
                this.MarkVector(this.items[2], init, delta);
                init += 2 + diff2;
                this.MarkVector(this.items[1], init, delta);
                init += 4 + diff4;
                this.MarkVector(this.items[0], init, delta);
                init += 5 + diff6;
                this.MarkVector(this.items[7], init, delta);
                init += 2 + diff2;
                this.MarkVector(this.items[6], init, delta);
            }

            if (!this.items[7][i])
            {
                var delta = this.deltas[7];
                init = value + 5 + (diff4 << 2) + diff4;

                this.MarkVector(this.items[0], init, delta);
                init += 4 + diff4;
                this.MarkVector(this.items[1], init, delta);
                init += 2 + diff2;
                this.MarkVector(this.items[2], init, delta);
                init += 4 + diff4;
                this.MarkVector(this.items[3], init, delta);
                init += 2 + diff2;
                this.MarkVector(this.items[4], init, delta);
                init += 4 + diff4;
                this.MarkVector(this.items[5], init, delta);
                init += 6 + diff6;
                this.MarkVector(this.items[6], init, delta);
                init += 2 + diff2;
                this.MarkVector(this.items[7], init, delta);
            }
        }

        /// <summary>
        /// Marca o primeiro vector sem efectuar verificação de primalidade.
        /// </summary>
        private void MarkFirstWitouhtVerification()
        {
            var diff2 = (this.notSievedNumbPointer << 1); // 2i
            var diff4 = (diff2 << 1); // 4i
            var diff6 = diff2 + diff4; // 6i
            var init = this.currentPointer;
            var delta = this.deltas[0];

            this.MarkVector(this.items[4], init, delta);
            init += 1 + diff4;
            this.MarkVector(this.items[3], init, delta);
            init += diff2;
            this.MarkVector(this.items[7], init, delta);
            init += 1 + diff4;
            this.MarkVector(this.items[6], init, delta);
            init += 1 + diff2;
            this.MarkVector(this.items[2], init, delta);
            init += 1 + diff4;
            this.MarkVector(this.items[1], init, delta);
            init += 1 + diff6;
            this.MarkVector(this.items[5], init, delta);
            init += 1 + diff2;
            this.MarkVector(this.items[0], init, delta);
        }

        /// <summary>
        /// Marca os múltiplos no vector.
        /// </summary>
        /// <param name="vector">O vector.</param>
        /// <param name="indTerm">O termo independente do polinómio inicializador.</param>
        /// <param name="linTerm">O termo linear do polinómio inicializador.</param>
        /// <param name="delta">A diferença considerada.</param>
        private void MarkVector(
            bool[] vector,
            long indTerm,
            long linTerm,
            long delta)
        {
            var init = indTerm;
            init += (linTerm + this.notSievedNumbProd) * this.notSievedNumbPointer;
            this.MarkVector(vector, init, delta);
        }

        /// <summary>
        /// Marca o vector especificado.
        /// </summary>
        /// <param name="vector">O vector.</param>
        /// <param name="init">O parâmetro inicial.</param>
        /// <param name="delta">A diferença.</param>
        private void MarkVector(
            bool[] vector,
            long init,
            long delta)
        {
            var len = vector.LongLength;
            for (var i = init; i < len; i += delta)
            {
                vector[i] = true;
            }
        }

        /// <summary>
        /// Aumenta os valores de todas as diferenças.
        /// </summary>
        private void IncreaseAllDeltas()
        {
            for (var i = 0; i < 8; ++i)
            {
                this.deltas[i] += 30;
            }
        }

        /// <summary>
        /// Inicializa o algoritmo.
        /// </summary>
        /// <param name="max">O número limite para a geração dos números primos.</param>
        private void Initialize(long max)
        {
            this.max = max;
            this.state = 0;
            this.currentPointer = 0L;
            this.product = 0L;
        }

        /// <summary>
        /// Inicializa a sequência para o início da filtragem.
        /// </summary>
        private void InitializeSeq()
        {
            this.currentPointer = 0L;
            this.notSievedNumbPointer = 0L;
            this.notSievedNumbProd = 0L;
            this.notSievedMultPos = 1;

            this.items = new bool[8][];
            for (var i = 0; i < 8; ++i)
            {
                var diff = this.max - wheel[i];
                var len = diff / 30;
                if (diff % 30 != 0)
                {
                    ++len;
                }

                this.items[i] = new bool[len];
            }

            this.deltas = new long[8];
            Array.Copy(wheel, this.deltas, 8);
        }
    }

    /// <summary>
    /// Algoritmo com roda de nível 3.
    /// </summary>
    public class LevelThreeWheelPrimeGen
    {
        /// <summary>
        /// O perímetro da roda da matriz.
        /// </summary>
        const long matrixWheelSpan = 240L;

        /// <summary>
        /// O perímetro da roda dos valores.
        /// </summary>
        const long valuesWheelSpan = 30L;

        /// <summary>
        /// O tamanho da roda.
        /// </summary>
        const long valuesWheelSize = 8L;

        /// <summary>
        /// Matriz das posições.
        /// </summary>
        private static long[,] posMatrix = new long[,]{
            {0,  1,  2,   3,   4,   5,   6,   7},
            {1, 13, 20,  24,  31,  35,  42,  54},
            {2, 20, 32,  38,  49,  55,  67,  85},
            {3, 24, 38,  45,  58,  65,  79, 100},
            {4, 31, 49,  58,  77,  86, 104, 131},
            {5, 35, 55,  65,  86,  96, 116, 146},
            {6, 42, 67,  79, 104, 116, 141, 177},
            {7, 54, 85, 100, 131, 146, 177, 224}
        };

        /// <summary>
        /// O tamanho das rodas associadas às linhas.
        /// </summary>
        private static long[] wheelSizes = new long[]{
            8, 56, 88, 104, 136, 152, 184, 232
        };

        /// <summary>
        /// Os valores da roda.
        /// </summary>
        private static long[] wheelValues = new long[]{
            1, 7, 11, 13, 17, 19, 23, 29
        };

        /// <summary>
        /// Mantém a lista dos primeiros números primos.
        /// </summary>
        private static long[] firstPrimes = new long[]{
            2, 3, 5
        };

        /// <summary>
        /// O vector com os itens.
        /// </summary>
        private bool[] items;

        /// <summary>
        /// O valor máximo.
        /// </summary>
        private long max;

        /// <summary>
        /// O apontador actual.
        /// </summary>
        private long currentPointer;

        /// <summary>
        /// O apontador limite para os primos.
        /// </summary>
        private long limitPointer;

        /// <summary>
        /// O valor limite para comparação.
        /// </summary>
        private long limitValue;

        /// <summary>
        /// O estado.
        /// </summary>
        private ushort state;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LevelThreeWheelPrimeGen"/>.
        /// </summary>
        public LevelThreeWheelPrimeGen()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LevelThreeWheelPrimeGen"/>.
        /// </summary>
        /// <param name="max">O limite do gerador.</param>
        public LevelThreeWheelPrimeGen(long max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>
        /// O número primo ou zero caso o limite tenha sido atingido.
        /// </returns>
        public long GetNextPrime()
        {
            if (this.state == 0)
            {
                if (this.currentPointer == firstPrimes.LongLength)
                {
                    var len = firstPrimes.LongLength;
                    if (len > 0)
                    {
                        this.SetupWheelPhase();
                        ++this.currentPointer;
                        var result = wheelValues[this.currentPointer];
                        this.state = 1;
                        return result;
                    }
                    else
                    {
                        this.state = 255;
                        return 0L;
                    }
                }
                else
                {
                    var result = firstPrimes[this.currentPointer];
                    ++this.currentPointer;
                    return result;
                }
            }
            else if (state == 1)
            {
                var len = this.items.LongLength;
                while (this.currentPointer < len &&
                    this.items[this.currentPointer])
                {
                    ++this.currentPointer;
                }

                if (this.currentPointer < len)
                {
                    if (this.currentPointer == this.limitValue)
                    {
                        this.MarkMultiples();
                        ++this.limitPointer;
                        while (this.items[this.limitPointer - 1])
                        {
                            ++this.limitPointer;
                        }

                        var currentOffset = this.limitPointer / valuesWheelSize;
                        var rem = this.limitPointer % valuesWheelSize;
                        var prod = currentOffset * matrixWheelSpan;
                        this.limitValue = posMatrix[rem, rem]
                            + ((wheelSizes[rem] << 1) + prod) * currentOffset - 1;

                        while (this.currentPointer < len &&
                            this.items[this.currentPointer])
                        {
                            ++this.currentPointer;
                        }

                        if (this.currentPointer < len)
                        {
                            ++this.currentPointer;
                            var current = wheelValues[this.currentPointer % valuesWheelSize]
                                + (this.currentPointer / valuesWheelSize) * valuesWheelSpan;
                            return current;
                        }
                        else
                        {
                            this.state = 255;
                            return 0L;
                        }
                    }
                    else
                    {
                        ++this.currentPointer;
                        var current = wheelValues[this.currentPointer % valuesWheelSize]
                            + (this.currentPointer / valuesWheelSize) * valuesWheelSpan;
                        return current;
                    }
                }
                else
                {
                    this.state = 255;
                    return 0L;
                }
            }
            else
            {
                return 0L;
            }
        }

        /// <summary>
        /// Obtém o valor associado ao apontador.
        /// </summary>
        /// <param name="pointer">O apontador.</param>
        /// <returns>O valor.</returns>
        private long GetValue(long pointer)
        {
            var result = wheelValues[pointer % valuesWheelSize]
                            + (pointer / valuesWheelSize) * valuesWheelSpan;
            return result;
        }

        /// <summary>
        /// Marca os múltiplos actuais.
        /// </summary>
        private void MarkMultiples()
        {
            var len = this.items.LongLength;
            var currentOffset = this.limitPointer / valuesWheelSize;
            var i = this.limitPointer % valuesWheelSize;
            var prod = currentOffset * matrixWheelSpan;
            var columnOffset = wheelSizes[i] + prod;
            for (var j = i; j < valuesWheelSize; ++j)
            {
                var size = wheelSizes[j];
                var value = posMatrix[i, j] + (size + columnOffset) * currentOffset - 1;
                if (value < len)
                {
                    this.items[value] = true;
                    value += columnOffset;
                    while (value < len)
                    {
                        this.items[value] = true;
                        value += columnOffset;
                    }
                }
                else
                {
                    i = 0;
                    j = valuesWheelSize;
                }
            }

            for (var j = 0L; j < i; ++j)
            {
                var size = wheelSizes[j];
                var value = posMatrix[i, j] + size * currentOffset + columnOffset - 1;
                if (value < len)
                {
                    this.items[value] = true;
                    value += columnOffset;
                    while (value < len)
                    {
                        this.items[value] = true;
                        value += columnOffset;
                    }
                }
                else
                {
                    j = i;
                }
            }
        }

        /// <summary>
        /// Inicializa o gerador.
        /// </summary>
        /// <param name="max">O valor máximo.</param>
        private void Initialize(long max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There are no primes less than 2.");
            }
            else
            {
                this.max = max;
                this.state = 0;
                this.currentPointer = 0L;
            }
        }

        /// <summary>
        /// Inicializa a fase em que se utiliza a roda.
        /// </summary>
        private void SetupWheelPhase()
        {
            var quo = max / 30;
            var rem = (long)(max % 30);
            var len = (long)(quo * 8);
            len += (long)this.GetIndex(rem);
            this.items = new bool[len];

            this.currentPointer = 0L;
            this.limitPointer = 1L;
            this.limitValue = posMatrix[1, 1] - 1;
        }

        /// <summary>
        /// Obtém o índice do maior valor menor ou igual ao
        /// proporcionado.
        /// </summary>
        /// <param name="value">O valor a ser comparado.</param>
        /// <returns>O índice.</returns>
        private int GetIndex(long value)
        {
            var low = 0;
            var high = 7;
            var lowValue = wheelValues[low];
            var highValue = wheelValues[high];
            if (value <= lowValue)
            {
                return low;
            }
            else if (value >= highValue)
            {
                return high;
            }
            else
            {
                while (low != high)
                {
                    if (low + 1 == high)
                    {
                        high = low;
                    }
                    else
                    {
                        var mid = (low + high) / 2;
                        var midValue = wheelValues[mid];
                        if (midValue <= value)
                        {
                            low = mid;
                            lowValue = midValue;
                        }
                        else
                        {
                            high = mid;
                            highValue = midValue;
                        }
                    }
                }

                return low;
            }
        }
    }

    /// <summary>
    /// Crivo para gerar números primos que elimina compostos ímpares.
    /// </summary>
    public class CompPrimeSieveGenBinaryContainer
    {
        /// <summary>
        /// Mantém os itens a serem analisados.
        /// </summary>
        private ulong[] items;

        /// <summary>
        /// Indica que o pedido é relativo ao primeiro primo.
        /// </summary>
        private bool firstPrime;

        /// <summary>
        /// A variação horizontal.
        /// </summary>
        private ulong horizontalDelta;

        /// <summary>
        /// Apontador para o primeiro número não filtrado.
        /// </summary>
        private ulong notSievedPointer;

        /// <summary>
        /// O valor da sequência relativa ao número não filtrado.
        /// </summary>
        private ulong notSievedPrimeSeq;

        /// <summary>
        /// Apontador para o último número primo retornado.
        /// </summary>
        private ulong lastPrimePointer;

        /// <summary>
        /// O valor máximo.
        /// </summary>
        private ulong max;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        public CompPrimeSieveGenBinaryContainer()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        public CompPrimeSieveGenBinaryContainer(ulong max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public ulong GetNextPrime()
        {
            if (this.firstPrime)
            {
                this.firstPrime = false;
                return 2;
            }
            else
            {
                var mainPointer = this.lastPrimePointer >> 6;
                var remPointer = (int)(this.lastPrimePointer & 0x3F);
                while (this.lastPrimePointer < this.max &&
                (this.items[mainPointer] & (1UL << remPointer)) != 0)
                {
                    ++this.lastPrimePointer;
                    mainPointer = this.lastPrimePointer >> 6;
                    remPointer = (int)(this.lastPrimePointer & 0x3F);
                }

                if (this.lastPrimePointer >= this.max)
                {
                    return 0;
                }
                else
                {
                    if (this.lastPrimePointer == this.notSievedPrimeSeq)
                    {
                        // Encontrou o próximo número composto
                        for (var i = this.lastPrimePointer; i < this.max; i += this.horizontalDelta)
                        {
                            mainPointer = i >> 6;
                            remPointer = (int)(i & 0x3F);
                            this.items[mainPointer] |= (1UL << remPointer);
                        }

                        ++this.notSievedPointer;
                        mainPointer = this.notSievedPointer >> 6;
                        remPointer = (int)(this.notSievedPointer & 0x3F);

                        this.horizontalDelta += 2;
                        while ((this.items[mainPointer] & (1UL << remPointer)) != 0)
                        {
                            ++this.notSievedPointer;
                            ++remPointer;
                            if (remPointer == 64)
                            {
                                ++mainPointer;
                                remPointer = 0;
                            }

                            this.horizontalDelta += 2;
                        }

                        this.notSievedPrimeSeq = 6 * this.notSievedPointer +
                            ((this.notSievedPointer * this.notSievedPointer) << 1) + 3;

                        ++this.lastPrimePointer;
                        mainPointer = this.lastPrimePointer >> 6;
                        remPointer = (int)(this.lastPrimePointer & 0x3F);
                        while (this.lastPrimePointer < this.max &&
                        (this.items[mainPointer] & (1UL << remPointer)) != 0)
                        {
                            ++this.lastPrimePointer;
                            mainPointer = this.lastPrimePointer >> 6;
                            remPointer = (int)(this.lastPrimePointer & 0x3F);
                        }

                        if (this.lastPrimePointer == this.max)
                        {
                            return 0;
                        }
                        else
                        {
                            var result = (this.lastPrimePointer << 1) + 3;
                            ++this.lastPrimePointer;
                            return result;
                        }
                    }
                    else
                    {
                        var result = (this.lastPrimePointer << 1) + 3;
                        ++this.lastPrimePointer;
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Inicializa o crivo.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        private void Initialize(ulong max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else
            {
                var innerMax = (max - 1) >> 1;
                var main = innerMax >> 6;
                var rem = innerMax & 0x3F;
                if (rem != 0)
                {
                    ++main;
                }

                this.items = new ulong[main];

                this.max = innerMax;
                this.firstPrime = true;
                this.notSievedPointer = 0;
                this.notSievedPrimeSeq = 3;
                this.lastPrimePointer = 0;
                this.horizontalDelta = 3;
            }
        }
    }

    /// <summary>
    /// Implementa um gerador de primos incremental.
    /// </summary>
    /// <remarks>
    /// O algoritmo não estabelece um limite para os números primos gerados.
    /// </remarks>
    public class IncPrimeSieveGenerator
    {
        /// <summary>
        /// O número actual.
        /// </summary>
        private int primePointer;

        /// <summary>
        /// O número actual a ser processado.
        /// </summary>
        private ulong currentNumb;

        /// <summary>
        /// O mapeamento de factores.
        /// </summary>
        private IDictionary<ulong, ulong> factorMap;

        /// <summary>
        /// Os números primos encontrados.
        /// </summary>
        private InsertionSortedCollection<ulong> foundPrimes;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="IncPrimeSieveGenerator"/>.
        /// </summary>
        public IncPrimeSieveGenerator()
        {
            this.Initialize(() => new Dictionary<ulong, ulong>());
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="IncPrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="lambda">
        /// A expressão responsável pela criação do mapeamento de factores.
        /// </param>
        public IncPrimeSieveGenerator(
            Func<IDictionary<ulong, ulong>> lambda)
        {
            if (lambda == null)
            {
                throw new ArgumentNullException("lamdba");
            }
            else
            {
                this.Initialize(lambda);
            }
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public ulong GetNextPrime()
        {
            var state = true;
            while (state)
            {
                if (this.currentNumb == 2)
                {
                    this.foundPrimes.Add(2UL);
                    ++this.currentNumb;
                    state = false;
                }
                else if (this.currentNumb == 3)
                {
                    this.factorMap.Add(this.currentNumb, 3UL);
                    this.foundPrimes.Add(3UL);
                    this.currentNumb += 2;
                    state = false;
                }
                else if ((this.currentNumb % 3) == 0)
                {
                    this.factorMap.Add(this.currentNumb, 3UL);
                    var f = this.currentNumb / 3;
                    if (f > 3)
                    {
                        // O próximo número primo é 5
                        if (this.factorMap[f] > 3)
                        {
                            this.factorMap.Add(5 * f, 5);
                        }
                    }

                    this.currentNumb += 2;
                }
                else
                {
                    var factor = 0UL;
                    if (this.factorMap.TryGetValue(this.currentNumb, out factor))
                    {
                        var index = this.foundPrimes.IndexOf(factor);
                        var f = this.currentNumb / factor;
                        ++index;
                        var r = this.foundPrimes[index];
                        if (factor < this.factorMap[f])
                        {
                            this.factorMap.Add(r * f, r);
                        }
                    }
                    else
                    {
                        this.factorMap.Add(this.currentNumb, this.currentNumb);
                        this.foundPrimes.Add(this.currentNumb);
                        state = false;
                    }

                    this.currentNumb += 2;
                }
            }

            var prime = this.foundPrimes[this.primePointer];
            ++this.primePointer;
            return prime;
        }

        /// <summary>
        /// Inicializa o estado actual.
        /// </summary>
        /// <param name="lambda">
        /// A expressão responsável pela criação do mapeamento de factores.
        /// </param>
        private void Initialize(
            Func<IDictionary<ulong, ulong>> lambda)
        {
            this.primePointer = 0;
            this.currentNumb = 2UL;
            this.factorMap = lambda.Invoke();
            this.foundPrimes = new InsertionSortedCollection<ulong>(
                Comparer<ulong>.Default);
        }
    }

    /// <summary>
    /// Implementa um crivo de primos linear.
    /// </summary>
    /// <remarks>
    /// O crivo linear permite filtrar os números compostos
    /// apenas uma vez.
    /// </remarks>
    public class LinearPrimeSieveGenerator
    {
        /// <summary>
        /// Mantém os itens a serem analisados.
        /// </summary>
        private bool[] items;

        /// <summary>
        /// Mantém o número corrente em análise.
        /// </summary>
        private uint current;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LinearPrimeSieveGenerator"/>
        /// </summary>
        public LinearPrimeSieveGenerator()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LinearPrimeSieveGenerator"/>
        /// </summary>
        /// <param name="max">O número máximo.</param>
        public LinearPrimeSieveGenerator(uint max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        /// <remarks>TODO: completar a implementação.</remarks>
        public uint GetNextPrime()
        {
            var len = this.items.Length;
            var result = 0U;
            var state = true;
            while (state)
            {
                if (this.current == len)
                {
                    state = false;
                }
                else if (!this.items[this.current])
                {
                    // O número é primo
                    if (this.current == 0)
                    {
                        this.items[2] = true;
                    }
                    else if (this.current == 1)
                    {
                        this.items[4] = true;
                        this.items[7] = true;
                    }
                    else
                    {
                        var p = this.current;
                        var incP = p + 2;
                        p += incP;
                        if (p < len)
                        {
                            this.items[p] = true;
                            p += incP;
                            if (p < len)
                            {
                                this.items[p] = true;
                                incP <<= 1;
                                var pointer = 3U;
                                p += incP;
                                while (pointer <= this.current)
                                {
                                    if (p < len)
                                    {
                                        if (!this.items[pointer])
                                        {
                                            this.items[p] = true;
                                        }

                                        pointer += 2;
                                        p += incP;
                                    }
                                    else
                                    {
                                        // Termina o ciclo
                                        pointer = this.current + 1;
                                    }
                                }
                            }
                        }
                    }

                    result = this.current + 2;
                    ++this.current;
                    state = false;
                }
                else
                {
                    // O número é composto
                    var p = this.current;
                    var n = this.current + 2;
                    var incP = n;
                    p += incP;
                    if (p < len)
                    {
                        this.items[p] = true;
                        if ((n & 1) == 1)
                        {
                            p += incP;
                            if (p < len)
                            {
                                incP <<= 1;
                                this.items[p] = true;

                                var status = n % 3;
                                var pointer = 3U;
                                while (status != 0)
                                {
                                    p += incP;
                                    if (!this.items[pointer])
                                    {
                                        pointer += 2;
                                        if (p < len)
                                        {
                                            this.items[p] = true;
                                            status = n % pointer;
                                        }
                                        else
                                        {
                                            status = 0;
                                        }
                                    }
                                    else
                                    {
                                        pointer += 2;
                                    }
                                }
                            }
                        }
                    }

                    ++this.current;
                }
            }

            return result;
        }

        /// <summary>
        /// Inicializa o estado do gerador.
        /// </summary>
        /// <param name="max">O limite para os números primos.</param>
        private void Initialize(uint max)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else
            {
                this.items = new bool[max - 1];
                this.current = 0;
            }
        }
    }

    /// <summary>
    /// Implementa um crivo para gerar números primos baseado numa roda.
    /// </summary>
    /// <remarks>Aplica uma roda.</remarks>
    public class WheelPrimeSieveGenerator
    {
        /// <summary>
        /// Mantém os itens a serem analisados.
        /// </summary>
        private bool[] items;

        /// <summary>
        /// Mantém a roda.
        /// </summary>
        private ILevelWheel levelWheel;

        /// <summary>
        /// Roda que determina o número primo limite.
        /// </summary>
        private ILevelWheel limitWheel;

        /// <summary>
        /// O nível máximo da roda.
        /// </summary>
        private ulong level;

        /// <summary>
        /// O limite para a extracção de números primos.
        /// </summary>
        private long limit;

        /// <summary>
        /// Variável de estado que indica tratarem-se dos primeiros
        /// números primos gerados pela roda.
        /// </summary>
        /// <remarks>
        /// 0 - A roda actual fornece sempre números primos.
        /// </remarks>
        private byte state;

        /// <summary>
        /// Mantém o valor do máximo.
        /// </summary>
        private long max;

        /// <summary>
        /// A matriz das diferenças.
        /// </summary>
        private IDiffsWheelsMatrix diffsMatrix;

        /// <summary>
        /// A matriz actual das coordenadas.
        /// </summary>
        private IWheel currentCoordMatrix;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="WheelPrimeSieveGenerator"/>.
        /// </summary>
        public WheelPrimeSieveGenerator()
        {
            this.Initialize(100L, 3);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="WheelPrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O valor máximo.</param>
        public WheelPrimeSieveGenerator(long max)
        {
            this.Initialize(max, 3);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="WheelPrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O valor máximo.</param>
        /// <param name="level">O nível da roda a ser utilizada, iniciando em zero.</param>
        public WheelPrimeSieveGenerator(long max, ulong level)
        {
            if (level < 0)
            {
                throw new ArgumentOutOfRangeException("level", "Level must be non-negative.");
            }
            else
            {
                this.Initialize(max, level);
            }
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>
        /// O número primo ou zero caso o limite tenha sido atingido.
        /// </returns>
        public long GetNextPrime()
        {
            if (this.state == 0)
            {
                var wheelLevel = this.levelWheel.Level;
                if (wheelLevel < this.level)
                {
                    var current = this.levelWheel.Current;
                    if (current > max)
                    {
                        this.state = 255;
                        return 0L;
                    }
                    else
                    {
                        this.levelWheel = this.levelWheel.GetNextLevelWheel();
                        return current;
                    }
                }
                else
                {
                    var current = this.levelWheel.Current;
                    if (current > this.max)
                    {
                        this.state = 255;
                        return 0L;
                    }
                    else
                    {
                        this.limitWheel = this.levelWheel.CloneWheel();
                        var limitPrime = limitWheel.Current;
                        this.limit = limitPrime * limitPrime;

                        var length = this.ComputeArraySize(
                            this.max,
                            this.levelWheel);
                        this.items = new bool[length];

                        // Processa o número primo
                        if (this.level < 2)
                        {
                            this.diffsMatrix = new SmallestLevelDiffsWheelsMatrix(
                                (long)this.level);
                            this.levelWheel.MoveRight();
                            this.state = 1;
                        }
                        else
                        {
                            this.BuildWheelsMatrix();
                            this.levelWheel.MoveRight();
                            this.state = 1;
                        }

                        return current;
                    }
                }
            }
            else if (this.state == 1)
            {
                var len = this.items.LongLength;
                while (
                    this.levelWheel.RotateNumb < len &&
                    this.items[this.levelWheel.RotateNumb])
                {
                    this.levelWheel.MoveRight();
                }

                if (this.levelWheel.RotateNumb < len)
                {
                    if (this.levelWheel.Current == this.limit)
                    {
                        this.currentCoordMatrix = this.diffsMatrix.GetNextWheel(
                            this.levelWheel.RotateNumb);
                        this.MarkMultiples();

                        this.limitWheel.MoveRight();
                        while (
                            this.limitWheel.RotateNumb < len &&
                            this.items[this.limitWheel.RotateNumb])
                        {
                            this.currentCoordMatrix = this.diffsMatrix.GetNextWheel(
                            this.levelWheel.RotateNumb);
                            this.limitWheel.MoveRight();
                        }

                        this.limit = this.limitWheel.Current * this.limitWheel.Current;

                        return this.NextPrime();
                    }
                    else
                    {
                        var result = this.levelWheel.Current;
                        this.levelWheel.MoveRight();
                        return result;
                    }
                }
                else
                {
                    this.state = 255;
                    return 0;
                }
            }

            return 0;
        }

        /// <summary>
        /// Constrói a matriz das rodas.
        /// </summary>
        private void BuildWheelsMatrix()
        {
            var size = ((long)this.levelWheel.Size + 2L) >> 1;
            var seqs = new long[size][];
            for (var k = 0; k < size; ++k)
            {
                seqs[k] = new long[size];
            }

            --size;
            var wheel = this.levelWheel.CloneWheel();
            wheel.Reset();

            var current = wheel.Current;
            var coord = 0L;
            var stored = current;

            var w1 = this.levelWheel.CloneWheel();
            w1.Reset();
            var curr1 = w1.Current;
            var w2 = this.levelWheel.CloneWheel();
            w2.Reset();

            var i = 0L;
            while (i < size)
            {
                var array = seqs[i];
                array[0] = i;
                wheel.Reset();

                current = stored;
                coord = 0L;
                var j = 0L;

                w2.Reset();

                var curr2 = w2.Current;

                while (j < size)
                {
                    var prod = curr1 * curr2;
                    while (current < prod)
                    {
                        wheel.MoveRight();
                        current = wheel.Current;
                        ++coord;
                    }

                    array[j] = coord;
                    w2.MoveRight();
                    curr2 = w2.Current;
                    ++j;
                }

                w1.MoveRight();
                curr1 = w1.Current;

                ++i;
            }

            this.diffsMatrix = new GreatestLevelDiffWheelsMatrix(seqs);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        private long NextPrime()
        {
            var len = this.items.LongLength;
            this.levelWheel.MoveRight();
            while (
                this.levelWheel.RotateNumb < len &&
                this.items[this.levelWheel.RotateNumb])
            {
                this.levelWheel.MoveRight();
            }

            if (this.levelWheel.RotateNumb < len)
            {
                var result = this.levelWheel.Current;
                this.levelWheel.MoveRight();
                return result;
            }
            else
            {
                this.state = 255;
                return 0;
            }
        }

        /// <summary>
        /// Marca os múltiplos do número marcado na roda actual.
        /// </summary>
        private void MarkMultiples()
        {
            var coordWheel = this.currentCoordMatrix;
            var len = this.items.LongLength;
            while (coordWheel.Current < len)
            {
                this.items[coordWheel.Current] = true;
                coordWheel.MoveRight();
            }
        }

        /// <summary>
        /// Função interna que permite calcular o número de valores
        /// a serem reservados para o algoritmo.
        /// </summary>
        /// <param name="value">O valor máximo.</param>
        /// <param name="wheel">O roda a ser considerada.</param>
        /// <returns></returns>
        internal ulong InternalComputeSize(
            long value,
            ILevelWheel wheel)
        {
            return this.ComputeArraySize(
                value,
                wheel);
        }

        /// <summary>
        /// Inicializa o crivo.
        /// </summary>
        /// <param name="max">O limite para os números primos gerados.</param>
        /// <param name="k">O patamar da roda.</param>
        private void Initialize(long max, ulong k)
        {
            if (max < 2)
            {
                throw new ArgumentException("There is no prime number less than two.");
            }
            else if (k < 0)
            {
                throw new ArgumentOutOfRangeException("k", "The wheel level must be non-negative.");
            }
            else
            {
                this.levelWheel = new LevelZeroWheel();
                this.level = 0UL;
                this.limit = 0L;
                this.max = max;
                this.level = k;
                this.state = 0;
            }
        }

        /// <summary>
        /// Calcula o tamanho do vector.
        /// </summary>
        /// <param name="quant">O valor máximo.</param>
        /// <param name="wheel">A roda.</param>
        /// <returns>O tamanho do vector.</returns>
        private ulong ComputeArraySize(
            long quant,
            ILevelWheel wheel)
        {
            var span = wheel.Span;
            var size = wheel.Size;
            var quo = (ulong)quant / span;
            var value = 0UL;

            var clonedWheel = wheel.CloneWheel();
            if (quo != 0UL)
            {
                value = quo * size - 1;
                clonedWheel.GotoValue((long)(span * quo));
            }
            else
            {
                clonedWheel.Reset();
            }

            while (clonedWheel.Current < quant)
            {
                ++value;
                clonedWheel.MoveRight();
            }

            return value;
        }
    }

    #region Interfaces Internas

    /// <summary>
    /// Define um roda genérica.
    /// </summary>
    internal interface IWheel
    {
        /// <summary>
        /// Obtém o valor actual da roda.
        /// </summary>
        long Current { get; }

        /// <summary>
        /// Obtém a amplitude da roda.
        /// </summary>
        ulong Size { get; }

        /// <summary>
        /// Move a roda para a direita.
        /// </summary>
        void MoveRight();

        /// <summary>
        /// Move a roda para a esquerda.
        /// </summary>
        void MoveLeft();
    }

    /// <summary>
    /// Define uma roda relativa a números primos.
    /// </summary>
    internal interface ILevelWheel : IWheel
    {
        /// <summary>
        /// Obtém o valor da diferença actual.
        /// </summary>
        long CurrentDiff { get; }

        /// <summary>
        /// Obtém o número de rotações aplicadas à roda.
        /// </summary>
        long RotateNumb { get; }

        /// <summary>
        /// Obtém o nível.
        /// </summary>
        ulong Level { get; }

        /// <summary>
        /// Obtém o ponto de partida da roda.
        /// </summary>
        long StartPoint { get; }

        /// <summary>
        /// Obtém o perímetro da roda.
        /// </summary>
        ulong Span { get; }

        /// <summary>
        /// Retorna a roda ao ponto incial.
        /// </summary>
        void Reset();

        /// <summary>
        /// Obtém a roda associada ao próximo nível.
        /// </summary>
        /// <returns>A roda associada ao próximo nível.</returns>
        ILevelWheel GetNextLevelWheel();

        /// <summary>
        /// Obtém uma cópia da roda actual.
        /// </summary>
        /// <returns>A cópia.</returns>
        ILevelWheel CloneWheel();

        /// <summary>
        /// Move a roda para o valor especificado.
        /// </summary>
        /// <remarks>
        /// Se o valor não pertencer ao espaço gerado pela roda,
        /// esta é colocada no valor seguinte.
        /// </remarks>
        /// <param name="value">O valor.</param>
        void GotoValue(long value);
    }

    /// <summary>
    /// Define uma matriz de diferenças.
    /// </summary>
    internal interface IDiffsWheelsMatrix
    {
        /// <summary>
        /// Obtém a próxima roda.
        /// </summary>
        /// <param name="coord">
        /// A coordenada do número em questão.
        /// </param>
        /// <returns>A roda.</returns>
        IWheel GetNextWheel(long coord);
    }

    #endregion Interfaces Internas

    #region Classes Internas

    /// <summary>
    /// Implementa alguns utilitários relacionados com o crivo de primos.
    /// </summary>
    internal static class PrimesGeneratorUtils
    {
        /// <summary>
        /// Obtém a roda de diferenças especificada no nível.
        /// </summary>
        /// <remarks>
        /// Apenas metade da roda é retornada uma vez que esta é simétrica.
        /// Por exemplo, a roda de nível 3 é constituída pelos oito elementos:
        /// 6, 4, 2, 4, 2, 4, 6, 2
        /// No entanto, apenas 5 são necessários:
        /// 2, 6, 4, 2, 4
        /// O apontador é iniciado no 6 e desloca-se para a direita até ao final.
        /// A partir do final, desloca-se para a esquerda, construindo a totalidade
        /// da roda.
        /// </remarks>
        /// <param name="level">O nível.</param>
        /// <param name="primes">Os números primos filtrados.</param>
        /// <returns>A roda.</returns>
        internal static List<ulong> GetDiffWheel(
            int level,
            List<ulong> primes)
        {
            if (level == 0)
            {
                return new List<ulong> { 1 };
            }
            else if (level == 1)
            {
                primes.Add(2);
                return new List<ulong> { 2 };
            }
            else if (level > 1)
            {
                var currentWheel = new List<ulong>() { 2, 4 };
                primes.Add(2);
                primes.Add(3);

                var primeProd = 1UL;
                var newList = new List<ulong>();
                for (var i = 2; i < level; ++i)
                {
                    var wheelPointer = 1;
                    var wheelInc = 1;

                    var acc = currentWheel[wheelPointer];
                    var pseudoPrime = 1U + acc;
                    var firstPrime = pseudoPrime;

                    var lastPrime = pseudoPrime;
                    var lastPointer = wheelPointer;
                    var lastInc = wheelInc;

                    primes.Add(pseudoPrime);

                    primeProd *= acc;
                    newList.Add(2);

                    var limit = firstPrime * lastPrime;
                    var len = currentWheel.Count;
                    IncrementWheel(len, ref wheelPointer, ref wheelInc);

                    var current = currentWheel[wheelPointer];
                    pseudoPrime += current;
                    acc += current;
                    newList.Add(acc);

                    var j = 1UL;
                    IncrementWheel(len, ref wheelPointer, ref wheelInc);
                    acc = currentWheel[wheelPointer];
                    pseudoPrime += acc;
                    while (pseudoPrime < limit
                        && j < primeProd)
                    {
                        newList.Add(acc);
                        IncrementWheel(len, ref wheelPointer, ref wheelInc);
                        acc = currentWheel[wheelPointer];
                        pseudoPrime += acc;
                        ++j;
                    }

                    if (j < primeProd)
                    {
                        IncrementWheel(len, ref lastPointer, ref lastInc);
                        lastPrime += currentWheel[lastPointer];
                        limit = lastPrime * firstPrime;

                        IncrementWheel(len, ref wheelPointer, ref wheelInc);
                        current = currentWheel[wheelPointer];
                        acc += current;

                        while (j < primeProd)
                        {
                            pseudoPrime += current;
                            IncrementWheel(len, ref wheelPointer, ref wheelInc);
                            current = currentWheel[wheelPointer];

                            if (pseudoPrime == limit)
                            {
                                acc += current;
                                IncrementWheel(len, ref lastPointer, ref lastInc);
                                lastPrime += currentWheel[lastPointer];
                                limit = lastPrime * firstPrime;
                            }
                            else
                            {
                                newList.Add(acc);
                                acc = current;
                                ++j;
                            }
                        }
                    }

                    currentWheel.Clear();
                    var temp = currentWheel;
                    currentWheel = newList;
                    newList = temp;
                }

                return currentWheel;
            }
            else
            {
                throw new ArgumentException("Invalid level in wheel creation.");
            }
        }

        /// <summary>
        /// Obtém as diferenças para uma determinada roda.
        /// </summary>
        /// <remarks>Função usada na investigação de um método.</remarks>
        /// <param name="level">O nível.</param>
        /// <returns>A lista das diferenças.</returns>
        internal static List<List<ulong>> GetWheelDifferences(
            int level)
        {
            var primes = new List<ulong>();
            var wheel = default(ILevelWheel);
            if (level == 0)
            {
                wheel = new LevelZeroWheel();
            }
            else if (level == 1)
            {
                wheel = new LevelOneWheel();
            }
            else
            {
                wheel = new GreatestLevelWheel((ulong)level);
            }

            var current = wheel.Current;

            var coord = 0UL;
            var stored = current;

            var w1 = wheel.CloneWheel();
            var curr1 = w1.Current;
            var w2 = wheel.CloneWheel();

            var sequences = new List<List<ulong>>();
            var i = 0UL;
            while (i < wheel.Size)
            {
                var seq = new List<ulong>();
                seq.Add(i);
                wheel.Reset();
                current = stored;
                coord = 0;
                var j = 0UL;

                w2.Reset();

                var curr2 = w2.Current;

                while (j < wheel.Size)
                {
                    var prod = curr1 * curr2;
                    while (current < prod)
                    {
                        wheel.MoveRight();
                        current = wheel.Current;
                        ++coord;
                    }

                    seq.Add(coord);
                    w2.MoveRight();
                    curr2 = w2.Current;
                    ++j;
                }

                w1.MoveRight();
                curr1 = w1.Current;
                ++i;
                sequences.Add(seq);
            }

            var result = new List<List<ulong>>();
            for (var k = 0; k < sequences.Count; ++k)
            {
                var seq = sequences[k];
                var newSeq = new List<ulong>();
                var prev = seq[0];

                for (var j = 1; j < seq.Count; ++j)
                {
                    newSeq.Add(seq[j] - prev);
                    prev = seq[j];
                }

                result.Add(newSeq);
            }

            return result;
        }

        /// <summary>
        /// Obtém os valores associados à roda.
        /// </summary>
        /// <param name="wheel">A roda.</param>
        /// <returns>Os valores associados à roda.</returns>
        internal static long[] GetWheelValues(ILevelWheel wheel)
        {
            var size = wheel.Size;
            var result = new long[size];
            result[0] = 1L;
            for (var i = 1UL; i < size; ++i)
            {
                result[i] = wheel.Current;
                wheel.MoveRight();
            }

            return result;
        }

        /// <summary>
        /// Obtém a tabela de multiplicação dos números proporcionados
        /// relativamente a um módulo.
        /// </summary>
        /// <param name="values">Os valores.</param>
        /// <param name="module">O módulo.</param>
        /// <returns>O resultado da multiplicação.</returns>
        internal static long[][] GetMultiplicationTable(
            long[] values,
            ulong module)
        {
            if (module == 0)
            {
                throw new ArgumentException("Module can't be zero.");
            }
            else if (module > 1)
            {
                var len = values.LongLength;
                var result = new long[len][];
                for (var i = 0; i < len; ++i)
                {
                    result[i] = new long[i + 1];
                }

                for (var i = 0; i < len; ++i)
                {
                    var innerRes = result[i];
                    var innerLen = innerRes.LongLength;
                    for (var j = 0; j < innerLen; ++j)
                    {
                        var rem = (ulong)(values[i] * values[j]) % module;
                        for (var k = 0L; k < len; ++k)
                        {
                            if ((ulong)values[k] == rem)
                            {
                                innerRes[j] = k;
                                k = len;
                            }
                        }
                    }
                }

                return result;
            }
            else
            {
                var len = values.LongLength;
                var result = new long[len][];
                for (var i = 0; i < len; ++i)
                {
                    result[i] = new long[i + 1];
                }

                return result;
            }
        }

        /// <summary>
        /// Incrementa os parâmetros relativos à roda.
        /// </summary>
        /// <param name="len">O comprimento da roda.</param>
        /// <param name="wheelPointer">O apontador.</param>
        /// <param name="wheelInc">A direcção do incremento.</param>
        private static void IncrementWheel(
            int len,
            ref int wheelPointer,
            ref int wheelInc)
        {
            wheelPointer += wheelInc;
            if (wheelPointer < 0)
            {
                wheelPointer = 1;
                wheelInc = 1;
            }
            else if (wheelPointer == len)
            {
                wheelPointer = len - 2;
                wheelInc = -1;
            }
        }
    }

    /// <summary>
    /// Implementa uma roda geral.
    /// </summary>
    internal class GeneralWheel : IWheel
    {
        /// <summary>
        /// Vector das diferenças.
        /// </summary>
        protected long[] diffs;

        /// <summary>
        /// O apontador para a diferença actual.
        /// </summary>
        protected long currentPointer;

        /// <summary>
        /// O valor actual.
        /// </summary>
        protected long current;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralWheel"/>.
        /// </summary>
        /// <param name="diffs">O vector das diferenças.</param>
        /// <param name="currentPointer">O apontador para a diferença actual.</param>
        /// <param name="current">O valor actual.</param>
        public GeneralWheel(
            long[] diffs,
            long currentPointer,
            long current)
        {
            this.diffs = diffs;
            this.Update(currentPointer, current);
        }

        /// <summary>
        /// Obtém o valor actual da roda.
        /// </summary>
        public long Current
        {
            get
            {
                return this.current;
            }
        }

        /// <summary>
        /// Obtém a amplitude da roda.
        /// </summary>
        public ulong Size
        {
            get
            {
                return (ulong)this.diffs.LongLength;
            }
        }

        /// <summary>
        /// Actualiza a roda.
        /// </summary>
        /// <param name="currentPointer">O apontador actual.</param>
        /// <param name="current">O valor actual.</param>
        public void Update(
            long currentPointer,
            long current)
        {
            this.currentPointer = currentPointer;
            this.current = current;
        }

        /// <summary>
        /// Move a roda para a direita.
        /// </summary>
        public void MoveRight()
        {
            var len = this.diffs.Length;
            ++this.currentPointer;
            if (this.currentPointer == len)
            {
                this.currentPointer = 0;
            }

            this.current += this.diffs[this.currentPointer];
        }

        /// <summary>
        /// Move a roda para a esquerda.
        /// </summary>
        public void MoveLeft()
        {
            var len = this.diffs.Length;
            --this.currentPointer;
            if (this.currentPointer == 0)
            {
                this.currentPointer = len - 1;
            }

            this.current -= this.diffs[this.currentPointer];
        }
    }

    /// <summary>
    /// Define uma roda de nível zero.
    /// </summary>
    internal class LevelZeroWheel : ILevelWheel
    {
        /// <summary>
        /// Mantém o valor actual.
        /// </summary>
        protected long current;

        /// <summary>
        /// Mantém o número de rotações.
        /// </summary>
        private long rotationNumb;

        /// <summary>
        /// Mantém o valor do nível.
        /// </summary>
        protected ulong level;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LevelZeroWheel"/>.
        /// </summary>
        public LevelZeroWheel()
        {
            this.current = 2L;
            this.level = 0UL;
            this.rotationNumb = 0L;
        }

        /// <summary>
        /// Obtém o valor actual da roda.
        /// </summary>
        public long Current
        {
            get
            {
                return this.current;
            }
        }

        /// <summary>
        /// Obtém o número de rotações aplicadas à roda.
        /// </summary>
        public long RotateNumb
        {
            get
            {
                return this.rotationNumb;
            }
        }

        /// <summary>
        /// Obtém a amplitude da roda.
        /// </summary>
        public ulong Size
        {
            get
            {
                return 1UL;
            }
        }

        /// <summary>
        /// Obtém o perímetro da roda.
        /// </summary>
        public ulong Span
        {
            get
            {
                return 1UL;
            }
        }

        /// <summary>
        /// Obtém o valor da diferença actual.
        /// </summary>
        public long CurrentDiff
        {
            get
            {
                return 1L;
            }
        }

        /// <summary>
        /// Obtém o nível.
        /// </summary>
        public ulong Level
        {
            get
            {
                return 0UL;
            }
        }

        /// <summary>
        /// Obtém o ponto de partida da roda.
        /// </summary>
        public long StartPoint
        {
            get
            {
                return 2L;
            }
        }

        /// <summary>
        /// Move a roda para a direita.
        /// </summary>
        public void MoveRight()
        {
            ++this.current;
            ++this.rotationNumb;
        }

        /// <summary>
        /// Move a roda para a esquerda.
        /// </summary>
        public void MoveLeft()
        {
            --this.current;
            --this.rotationNumb;
        }

        /// <summary>
        /// Obtém a roda associada ao próximo nível.
        /// </summary>
        /// <returns>A roda associada ao próximo nível.</returns>
        public ILevelWheel GetNextLevelWheel()
        {
            return new LevelOneWheel();
        }

        /// <summary>
        /// Obtém uma cópia da roda actual.
        /// </summary>
        /// <returns>A cópia.</returns>
        public ILevelWheel CloneWheel()
        {
            return new LevelZeroWheel()
            {
                current = this.current,
                level = this.level,
            };
        }

        /// <summary>
        /// Move a roda para o valor especificado.
        /// </summary>
        /// <remarks>
        /// Se o valor não pertencer ao espaço gerado pela roda,
        /// esta é colocada no valor seguinte.
        /// </remarks>
        /// <param name="value">O valor.</param>
        public void GotoValue(long value)
        {
            this.current = value;
        }

        /// <summary>
        /// Retorna a roda ao ponto incial.
        /// </summary>
        public void Reset()
        {
            this.current = 2L;
            this.rotationNumb = 0L;
        }
    }

    /// <summary>
    /// Define uma roda de nível zero.
    /// </summary>
    internal class LevelOneWheel : ILevelWheel
    {
        /// <summary>
        /// Mantém o valor actual.
        /// </summary>
        protected long current;

        /// <summary>
        /// Mantém o número de rotações.
        /// </summary>
        private long rotateNumb;

        /// <summary>
        /// Mantém o valor do nível.
        /// </summary>
        protected ulong level;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LevelOneWheel"/>.
        /// </summary>
        public LevelOneWheel()
        {
            this.current = 3L;
            this.level = 1UL;
            this.rotateNumb = 0L;
        }

        /// <summary>
        /// Obtém o valor actual da roda.
        /// </summary>
        public long Current
        {
            get
            {
                return this.current;
            }
        }

        /// <summary>
        /// Obém o número de rotações aplicadas à roda.
        /// </summary>
        public long RotateNumb
        {
            get
            {
                return this.rotateNumb;
            }
        }

        /// <summary>
        /// Obtém a amplitude da roda.
        /// </summary>
        public ulong Size
        {
            get
            {
                return 1UL;
            }
        }

        /// <summary>
        /// Obtém o perímetro da roda.
        /// </summary>
        public ulong Span
        {
            get
            {
                return 2UL;
            }
        }

        /// <summary>
        /// Obtém o valor da diferença actual.
        /// </summary>
        public long CurrentDiff
        {
            get
            {
                return 2L;
            }
        }

        /// <summary>
        /// Obtém o nível.
        /// </summary>
        public ulong Level
        {
            get
            {
                return 1UL;
            }
        }

        /// <summary>
        /// Obtém o ponto de partida da roda.
        /// </summary>
        public long StartPoint
        {
            get
            {
                return 3L;
            }
        }

        /// <summary>
        /// Move a roda para a direita.
        /// </summary>
        public void MoveRight()
        {
            this.current += 2L;
            ++this.rotateNumb;
        }

        /// <summary>
        /// Move a roda para a esquerda.
        /// </summary>
        public void MoveLeft()
        {
            this.current -= 2L;
            --this.rotateNumb;
        }

        /// <summary>
        /// Obtém a roda associada ao próximo nível.
        /// </summary>
        /// <returns>A roda associada ao próximo nível.</returns>
        public ILevelWheel GetNextLevelWheel()
        {
            return new GreatestLevelWheel();
        }

        /// <summary>
        /// Obtém uma cópia da roda actual.
        /// </summary>
        /// <returns>A cópia.</returns>
        public ILevelWheel CloneWheel()
        {
            return new LevelOneWheel()
            {
                current = this.current,
                level = this.level,
            };
        }

        /// <summary>
        /// Move a roda para o valor especificado.
        /// </summary>
        /// <remarks>
        /// Se o valor não pertencer ao espaço gerado pela roda,
        /// esta é colocada no valor seguinte.
        /// </remarks>
        /// <param name="value">O valor.</param>
        public void GotoValue(long value)
        {
            if ((value & 1) == 0)
            {
                this.current = value + 1;
            }
            else
            {
                this.current = value;
            }
        }

        /// <summary>
        /// Retorna a roda ao ponto incial.
        /// </summary>
        public void Reset()
        {
            this.current = 3L;
            this.rotateNumb = 0L;
        }
    }

    /// <summary>
    /// Define uma roda simétrica genérica.
    /// </summary>
    internal class GeneralSymmWheel : IWheel
    {
        /// <summary>
        /// Mantém o vector com as diferenças.
        /// </summary>
        protected long[] diffs;

        /// <summary>
        /// Mantém o apontador actual.
        /// </summary>
        protected long currPointer;

        /// <summary>
        /// Mantém o estado do movimento do apontador.
        /// </summary>
        protected bool moveRightState;

        /// <summary>
        /// Mantém o valor actual.
        /// </summary>
        protected long current;

        /// <summary>
        /// Mantém o tamanho da roda.
        /// </summary>
        protected ulong size;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralSymmWheel"/>.
        /// </summary>
        /// <remarks>
        /// A inicialização é delegada para os objectos descendentes.
        /// </remarks>
        protected GeneralSymmWheel() { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralSymmWheel"/>.
        /// </summary>
        /// <param name="diffs">Vector com as diferenças associadas à roda.</param>
        /// <param name="currPointer">O apontador actual.</param>
        /// <param name="moveRight">O estado do movimento do apontador.</param>
        /// <param name="current">O valor actual.</param>
        public GeneralSymmWheel(
            long[] diffs,
            long currPointer,
            bool moveRight,
            long current
            )
        {
            this.diffs = diffs;
            this.size = (ulong)(this.diffs.LongLength - 1) << 1;
            this.Update(
                currPointer,
                moveRight,
                current);
        }

        /// <summary>
        /// Obtém o valor actual da roda.
        /// </summary>
        public long Current
        {
            get
            {
                return this.current;
            }
        }

        /// <summary>
        /// Obtém a amplitude da roda.
        /// </summary>
        public ulong Size
        {
            get
            {
                return this.size;
            }
        }

        /// <summary>
        /// Coloca a roda num novo estado.
        /// </summary>
        /// <param name="currPointer">O apontador.</param>
        /// <param name="moveRight">O esatdo do movimento do apontador.</param>
        /// <param name="current">O valor actual.</param>
        public void Update(
            long currPointer,
            bool moveRight,
            long current)
        {
            this.currPointer = currPointer;
            this.moveRightState = moveRight;
            this.current = current;
        }

        /// <summary>
        /// Move a roda para a direita.
        /// </summary>
        public virtual void MoveRight()
        {
            if (this.moveRightState)
            {
                var len = this.diffs.LongLength;
                ++this.currPointer;
                if (this.currPointer == len)
                {
                    this.moveRightState = false;
                    this.currPointer = len - 2;
                }

                this.current += this.diffs[this.currPointer];
            }
            else
            {
                if (this.currPointer == 0)
                {
                    this.moveRightState = true;
                    this.currPointer = 1L;
                }
                else
                {
                    --this.currPointer;
                }

                this.current += this.diffs[this.currPointer];
            }
        }

        /// <summary>
        /// Move a roda para a esquerda.
        /// </summary>
        public virtual void MoveLeft()
        {
            if (this.moveRightState)
            {
                this.current -= this.diffs[currPointer];
                if (this.currPointer == 0)
                {
                    this.moveRightState = false;
                    this.currPointer = 1L;
                }
                else
                {
                    --this.currPointer;
                }
            }
            else
            {
                this.current -= this.diffs[this.currPointer];
                var len = this.diffs.LongLength;
                ++this.currPointer;
                if (this.currPointer == len)
                {
                    this.moveRightState = true;
                    this.currPointer = len - 2;
                }
            }
        }
    }

    /// <summary>
    /// Define uma roda de nível superior.
    /// </summary>
    internal class GreatestLevelWheel :
        GeneralSymmWheel,
        ILevelWheel
    {
        /// <summary>
        /// Mantém o valor do nível.
        /// </summary>
        protected ulong level;

        /// <summary>
        /// Mantém o perímetro da roda.
        /// </summary>
        protected ulong span;

        /// <summary>
        /// Mantém o número de rotações da roda.
        /// </summary>
        protected long rotateNumb;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GreatestLevelWheel"/>.
        /// </summary>
        public GreatestLevelWheel()
        {
            this.SetupProps(
                new long[] { 2L, 4L },
                1L,
                true,
                5L,
                2UL,
                2UL,
                6UL);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GreatestLevelWheel"/>.
        /// </summary>
        /// <param name="level">O nível da roda.</param>
        public GreatestLevelWheel(ulong level)
        {
            if (level < 2UL)
            {
                throw new ArgumentOutOfRangeException(
                    "level",
                    "Level must be greater or equal than 2.");
            }
            else
            {
                this.SetupProps(
                new long[] { 2L, 4L },
                1L,
                true,
                5L,
                level,
                2UL,
                6UL);
                var currentLevel = 2UL;
                while (currentLevel < level)
                {
                    this.IncreaseLevel();
                    ++currentLevel;
                }
            }
        }

        /// <summary>
        /// Obtém o valor da diferença actual.
        /// </summary>
        public long CurrentDiff
        {
            get
            {
                return this.diffs[this.currPointer];
            }
        }

        /// <summary>
        /// Obtém o número de rotações aplicadas à roda.
        /// </summary>
        public long RotateNumb
        {
            get
            {
                return this.rotateNumb;
            }
        }

        /// <summary>
        /// Obtém o nível.
        /// </summary>
        public ulong Level
        {
            get
            {
                return this.level;
            }
        }

        /// <summary>
        /// Obtém o ponto de partida da roda.
        /// </summary>
        public long StartPoint
        {
            get
            {
                return 1L + this.diffs[1];
            }
        }

        /// <summary>
        /// Obtém o perímetro da roda.
        /// </summary>
        public ulong Span
        {
            get
            {
                return this.span;
            }
        }

        /// <summary>
        /// Obtém a roda associada ao próximo nível.
        /// </summary>
        /// <returns>A roda associada ao próximo nível.</returns>
        public ILevelWheel GetNextLevelWheel()
        {
            var result = new GreatestLevelWheel();
            result.SetupProps(
                this.diffs,
                this.currPointer,
                this.moveRightState,
                this.current,
                this.level,
                this.size,
                this.span);
            result.IncreaseLevel();
            return result;
        }

        /// <summary>
        /// Move a roda para a direita.
        /// </summary>
        public override void MoveRight()
        {
            base.MoveRight();
            ++this.rotateNumb;
        }

        /// <summary>
        /// Move a roda para a esquerda.
        /// </summary>
        public override void MoveLeft()
        {
            base.MoveLeft();
            --this.rotateNumb;
        }

        /// <summary>
        /// Retorna a roda ao ponto incial.
        /// </summary>
        public void Reset()
        {
            this.currPointer = 1L;
            this.current = 1L + this.diffs[1];
            this.moveRightState = true;
            this.rotateNumb = 0L;
        }

        /// <summary>
        /// Aumenta o nível da roda.
        /// </summary>
        public void IncreaseLevel()
        {
            var prod = (this.diffs.LongLength - 1L) * this.diffs[1] + 1L;
            this.size = ((ulong)prod - 1) << 1;
            this.span *= ((ulong)this.diffs[1] + 1UL);
            var len = (long)this.span;

            var prime = 1L + this.diffs[1];
            var wheel = new GreatestLevelWheel()
            {
                diffs = this.diffs,
                current = prime,
                currPointer = 1L,
                moveRightState = true,
                level = this.level
            };

            var newDiffs = new long[prod];
            newDiffs[0] = 2L;
            var temp = wheel.CurrentDiff;
            wheel.MoveRight();
            temp += wheel.CurrentDiff;
            newDiffs[1] = temp;

            var pointerWheel = wheel.CloneWheel();
            var limit = prime * prime;
            var j = 2;
            wheel.MoveRight();
            var acc = wheel.CurrentDiff;
            while (j < prod)
            {
                if (wheel.current == limit)
                {
                    wheel.MoveRight();
                    acc += wheel.CurrentDiff;
                    limit = prime * pointerWheel.Current;
                    pointerWheel.MoveRight();
                    if ((this.current - prime) % len == 0)
                    {
                        this.current = wheel.current;
                    }
                }
                else
                {
                    newDiffs[j++] = acc;
                    wheel.MoveRight();
                    acc = wheel.CurrentDiff;
                }
            }

            this.diffs = newDiffs;
            ++this.level;

            // Restabelece o valor da posição
            this.Reset();
        }

        /// <summary>
        /// Move a roda para o valor especificado.
        /// </summary>
        /// <remarks>
        /// Se o valor não pertencer ao espaço gerado pela roda,
        /// esta é colocada no valor seguinte.
        /// </remarks>
        /// <param name="value">O valor.</param>
        public void GotoValue(long value)
        {
            if (value > this.current)
            {
                var div = (value - this.current) / (long)this.span;
                if (div != 0)
                {
                    this.current += (long)this.span * div;
                    this.rotateNumb += (long)this.size * div;
                }

                while (this.current < value)
                {
                    this.MoveRight();
                }
            }
            else if (value < this.current)
            {
                var div = (this.current - value) / (long)this.span;
                if (div != 0)
                {
                    this.current -= (long)this.span * div;
                    this.rotateNumb -= (long)this.size * div;
                }

                while (this.current > value)
                {
                    this.MoveLeft();
                }

                if (this.current != value)
                {
                    this.MoveRight();
                }
            }
        }

        /// <summary>
        /// Obtém uma cópia da roda actual.
        /// </summary>
        /// <remarks>
        /// O vector que define as diferenças é partilhado entre cópias.
        /// </remarks>
        /// <returns>A cópia.</returns>
        public ILevelWheel CloneWheel()
        {
            var result = new GreatestLevelWheel();
            result.SetupProps(
                this.diffs,
                this.currPointer,
                this.moveRightState,
                this.current,
                this.level,
                this.size,
                this.span);
            return result;
        }

        /// <summary>
        /// Inicializa o estado da roda.
        /// </summary>
        /// <param name="diffs">O vector das diferenças.</param>
        /// <param name="currPointer">O apontador para o vector.</param>
        /// <param name="moveRight">O sentido do movimento do apontador.</param>
        /// <param name="current">O valor actual.</param>
        /// <param name="level">O nível actual.</param>
        /// <param name="size">O tamanho da roda.</param>
        /// <param name="span">O perímetro da roda.</param>
        protected void SetupProps(
            long[] diffs,
            long currPointer,
            bool moveRight,
            long current,
            ulong level,
            ulong size,
            ulong span)
        {
            this.diffs = diffs;
            this.currPointer = currPointer;
            this.moveRightState = moveRight;
            this.current = current;
            this.level = level;
            this.size = size;
            this.span = span;
        }
    }

    /// <summary>
    /// Implementa uma matriz de rodas que definem as próximas diferenças
    /// relativas aos níveis inferiores.
    /// </summary>
    internal class SmallestLevelDiffsWheelsMatrix
        : IDiffsWheelsMatrix
    {
        /// <summary>
        /// Mantém a diferença actual.
        /// </summary>
        protected long[] current;

        /// <summary>
        /// O incremento da roda.
        /// </summary>
        protected long increment;

        /// <summary>
        /// A roda a ser retornada.
        /// </summary>
        protected GeneralWheel wheel;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SmallestLevelDiffsWheelsMatrix"/>.
        /// </summary>
        public SmallestLevelDiffsWheelsMatrix(long level)
        {
            this.increment = level + 1;
            this.current = new long[] { 1L };
            wheel = new GeneralWheel(this.current, 0L, 0L);
        }

        /// <summary>
        /// Obtém a próxima roda.
        /// </summary>
        /// <param name="coord">
        /// A coordenada do número em questão.
        /// </param>
        /// <returns>A roda.</returns>
        public IWheel GetNextWheel(long coord)
        {
            this.current[0] += this.increment;
            this.wheel.Update(0L, coord);
            return this.wheel;
        }
    }

    /// <summary>
    /// Implemneta uma matriz para níveis superiores.
    /// </summary>
    internal class GreatestLevelDiffWheelsMatrix
        : IDiffsWheelsMatrix
    {
        /// <summary>
        /// Mantém a matriz das diferenças.
        /// </summary>
        private long[][] coordsMatrix;

        /// <summary>
        /// Mantém o vector das diferenças actuais.
        /// </summary>
        private long[] currentDiffsArray;

        /// <summary>
        /// Mantém o apontador vertical.
        /// </summary>
        private long verticalPointer;

        /// <summary>
        /// Mantém o estado do deslocamento do apontador.
        /// </summary>
        private bool moveDownState;

        /// <summary>
        /// Mantém a roda actual.
        /// </summary>
        private GeneralSymmWheel wheel;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GreatestLevelDiffWheelsMatrix"/>.
        /// </summary>
        /// <param name="coordsMatrix">A matriz das coordenadas.</param>
        public GreatestLevelDiffWheelsMatrix(long[][] coordsMatrix)
        {
            this.ProcessCoordinatesMatrix(coordsMatrix);
            this.verticalPointer = 0L;
            this.moveDownState = false;
        }

        /// <summary>
        /// Obtém a próxima roda.
        /// </summary>
        /// <param name="coord">
        /// A coordenada do número em questão.
        /// </param>
        /// <returns>A roda.</returns>
        public IWheel GetNextWheel(long coord)
        {
            this.MoveVerticalPointer();
            var size = this.coordsMatrix.GetLongLength(0);
            var currArray = this.coordsMatrix[this.verticalPointer];
            for (var i = 0; i < size; ++i)
            {
                this.currentDiffsArray[i] += currArray[i];
            }

            this.wheel.Update(
                this.verticalPointer,
                this.moveDownState,
                coord);

            return this.wheel;
        }

        /// <summary>
        /// Processa a matriz das coordenadas.
        /// </summary>
        /// <param name="coordsMatrix">A matriz das coordenadas.</param>
        private void ProcessCoordinatesMatrix(
            long[][] coordsMatrix)
        {
            this.SetCurrent(coordsMatrix);
            this.SetHorizontalDifferences(coordsMatrix);
            this.SetVerticalDifferences(coordsMatrix);
            this.SetupWheel();
            this.coordsMatrix = coordsMatrix;
        }

        /// <summary>
        /// Estabelece o vector actual a partir da matriz das diferenças.
        /// </summary>
        /// <param name="coordsMatrix">A matriz das diferenças.</param>
        private void SetCurrent(long[][] coordsMatrix)
        {
            var size = coordsMatrix.GetLongLength(0);
            this.currentDiffsArray = new long[size];
            for (var i = 0; i < size; ++i)
            {
                this.currentDiffsArray[i] = 1L;
            }
        }

        /// <summary>
        /// Estabelece as diferenças horizontais.
        /// </summary>
        /// <param name="coordsMatrix">A matriz das coordenadas.</param>
        private void SetHorizontalDifferences(long[][] coordsMatrix)
        {
            var size = coordsMatrix.GetLongLength(0) - 1;
            for (var i = size; i > 0; --i)
            {
                var array = coordsMatrix[i];
                var prevArray = coordsMatrix[i - 1];
                for (var j = size - 1; j > 0; --j)
                {
                    array[j + 1] = prevArray[j] - prevArray[j - 1];
                }

                array[1] = prevArray[0] - (i - 1);
                array[0] = 2L;
            }

            var outArray = coordsMatrix[0];
            for (var i = size; i > 0; --i)
            {
                outArray[i] = 2L;
            }

            outArray[0] = 2L;
        }

        /// <summary>
        /// Estabelece a matriz das diferenças verticais.
        /// </summary>
        /// <param name="coordsMatrix">A matriz das coordenadas.</param>
        private void SetVerticalDifferences(long[][] coordsMatrix)
        {
            var size = coordsMatrix.GetLongLength(0) - 1;
            for (var i = size; i > 1; --i)
            {
                var currArray = coordsMatrix[i];
                var prevArray = coordsMatrix[i - 1];
                for (var j = size; j > 0; --j)
                {
                    currArray[j] = currArray[j] - prevArray[j];
                }

                currArray[0] = 2L;
            }

            var array = coordsMatrix[1];
            for (var j = size; j > 0; --j)
            {
                array[j] -= 1L;
            }
        }

        /// <summary>
        /// Estabelece a roda inicial.
        /// </summary>
        private void SetupWheel()
        {
            this.wheel = new GeneralSymmWheel(
                this.currentDiffsArray,
                1L,
                true,
                this.currentDiffsArray[0]);
        }

        /// <summary>
        /// Move o apontador vertical.
        /// </summary>
        private void MoveVerticalPointer()
        {
            if (this.moveDownState)
            {
                var size = this.coordsMatrix.GetLongLength(0);
                ++this.verticalPointer;
                if (this.verticalPointer == size)
                {
                    this.verticalPointer = size - 2;
                    this.moveDownState = false;
                }
            }
            else
            {
                if (this.verticalPointer == 0L)
                {
                    this.moveDownState = true;
                    this.verticalPointer = 1L;
                }
                else
                {
                    --this.verticalPointer;
                }
            }
        }
    }

    /// <summary>
    /// Define um enumerador para a roda.
    /// </summary>
    internal class WheelEnum :
        IEnumerator<ulong>
    {
        /// <summary>
        /// A roda.
        /// </summary>
        private ulong[] wheel;

        /// <summary>
        /// O comprimento considerado no vector.
        /// </summary>
        private long length;

        /// <summary>
        /// O índice actual.
        /// </summary>
        private long currIndex;

        /// <summary>
        /// A direcção de movimentação do índice.
        /// </summary>
        private bool rightDirection;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="WheelEnum"/>.
        /// </summary>
        private WheelEnum()
        {
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="WheelEnum"/>.
        /// </summary>
        /// <param name="wheel">A roda.</param>
        /// <param name="length">O comprimento a ser considerado no vector.</param>
        public WheelEnum(ulong[] wheel, long length)
        {
            this.Reset(wheel, length);
        }

        /// <summary>
        /// Obtém o valor actual.
        /// </summary>
        public ulong Current
        {
            get
            {
                return this.wheel[this.currIndex];
            }
        }

        /// <summary>
        /// Obtém o valor actual.
        /// </summary>y
        object IEnumerator.Current
        {
            get
            {
                return this.wheel[this.currIndex];
            }
        }

        /// <summary>
        /// Coloca o iterador no início.
        /// </summary>
        public void Reset()
        {
            this.currIndex = 1;
            this.rightDirection = true;
        }

        /// <summary>
        /// Estabelece um novo vector para o iterador.
        /// </summary>
        /// <param name="wheel">O vector que contém a roda.</param>
        /// <param name="length">O comprimento da roda no vector.</param>
        public void Reset(ulong[] wheel, long length)
        {
            this.wheel = wheel;
            this.length = length;
            this.Reset();
        }

        /// <summary>
        /// Estabelece o iterador a partir de outro.
        /// </summary>
        /// <param name="wheelEnum">O itnerador.</param>
        public void Reset(WheelEnum wheelEnum)
        {
            this.wheel = wheelEnum.wheel;
            this.length = wheelEnum.length;
            this.rightDirection = wheelEnum.rightDirection;
            this.currIndex = wheelEnum.currIndex;
        }

        /// <summary>
        /// Move para o próximo elemento da roda.
        /// </summary>
        /// <returns>Verdadeiro.</returns>
        public bool MoveNext()
        {
            if (this.rightDirection)
            {
                ++this.currIndex;
                if (this.currIndex == this.length)
                {
                    this.currIndex = this.length - 2;
                    this.rightDirection = false;
                }
            }
            else
            {
                --this.currIndex;
                if (this.currIndex == -1)
                {
                    this.currIndex = 1;
                    this.rightDirection = true;
                }
            }

            return true;
        }

        /// <summary>
        /// Cria um clone do enumerador.
        /// </summary>
        /// <returns>O clone do enumerador.</returns>
        public WheelEnum Clone()
        {
            return new WheelEnum()
            {
                wheel = this.wheel,
                currIndex = this.currIndex,
                rightDirection = this.rightDirection,
                length = this.length
            };
        }

        /// <summary>
        /// A função de disposição não tem efeito.
        /// </summary>
        public void Dispose()
        {
        }
    }

    #endregion Classes Internas

    #region Código sem desempenho

    /// <summary>
    /// Implmenetação de um gerador de primos incremental.
    /// </summary>
    /// <remarks>
    /// O algoritmo permite adiar a alocação de memória e não irá considerar os números pares.
    /// </remarks>
    public class NaiveIncPrimeSieveGenerator
    {
        /// <summary>
        /// Número máximo de tuplos vazios que são armazenados.
        /// </summary>
        private const int maximumEmpty = 500;

        /// <summary>
        /// Mantém a lista dos compostos encontrados.
        /// </summary>
        private MutableTuple<uint, int>[] composites;

        /// <summary>
        /// Define o número de compostos no conjunto.
        /// </summary>
        private int compositesCount = 0;

        /// <summary>
        /// Mantém uma lista de tuplos vazios para evitar instanciação.
        /// </summary>
        private List<MutableTuple<uint, int>> emptyTuples;

        /// <summary>
        /// Mantém os factores e respectivas potências tratadas.
        /// </summary>
        private Node storedIndices;

        /// <summary>
        /// Mantém uma lista de nós vazios para evitar novas instâncias.
        /// </summary>
        private Node emptyNodes;

        /// <summary>
        /// Mantém uma lista de números primos ímpares.
        /// </summary>
        private List<uint> oddPrimes;

        /// <summary>
        /// Mantém o número actual.
        /// </summary>
        private uint current;

        /// <summary>
        /// Mantém o índice número primo limite.
        /// </summary>
        private int limitPrime;

        /// <summary>
        /// Mantém o quadrado do número primo limite.
        /// </summary>
        private uint squaredLimitPrime;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="NaiveIncPrimeSieveGenerator"/>.
        /// </summary>
        public NaiveIncPrimeSieveGenerator()
        {
            this.composites = new MutableTuple<uint, int>[4];
            this.emptyTuples = new List<MutableTuple<uint, int>>(maximumEmpty);
            this.oddPrimes = new List<uint>();
            this.storedIndices = null;
            this.emptyNodes = null;
            this.current = 2;
            this.limitPrime = 0;
            this.squaredLimitPrime = 9;
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        public uint GetNextPrime()
        {
            if (this.current == 2)
            {
                ++this.current;
                return 2;
            }
            else if (this.current == 3)
            {
                this.oddPrimes.Add(3);
                this.AddComposite(MutableTuple.Create(9U, 0));
                this.current += 2;
                return 3;
            }
            else
            {
                var state = true;
                while (state)
                {
                    if (this.current == this.squaredLimitPrime)
                    {
                        ++this.limitPrime;
                        var limitPrimeValue = this.oddPrimes[this.limitPrime];
                        this.squaredLimitPrime = limitPrimeValue * limitPrimeValue;

                        // Processa os índices
                        var node = this.storedIndices;
                        if (node != null)
                        {
                            node = this.ProcessNode(node);
                            this.storedIndices = node;
                            if (node != null)
                            {
                                var previousNode = node;
                                node = node.Next;
                                while (node != null)
                                {
                                    node = this.ProcessNode(node);
                                    previousNode.Next = node;

                                    if (node != null)
                                    {
                                        previousNode = node;
                                        node = node.Next;
                                    }
                                }
                            }
                        }

                        // Trata-se de um composto que deverá existir na meda
                        var index = this.Find(this.current);
                        this.ProcessComposite(index);

                        this.current += 2;
                    }
                    else
                    {
                        var index = this.Find(this.current);
                        if (index == -1)
                        {
                            this.ProcessPrime();
                            state = false;
                        }
                        else
                        {
                            this.ProcessComposite(index);
                            this.current += 2;
                        }
                    }
                }

                var result = this.current;
                this.current += 2;
                return result;
            }
        }

        /// <summary>
        /// Processa o número primo.
        /// </summary>
        private void ProcessPrime()
        {
            var newPrimeIndex = this.oddPrimes.Count;
            var currentPrimeIndex = 0;
            this.oddPrimes.Add(this.current);
            var state = true;
            while (state)
            {
                var mul = this.current;
                mul *= this.oddPrimes[currentPrimeIndex];

                var tup = this.GetTuple(mul, currentPrimeIndex);
                this.AddComposite(tup);

                if (currentPrimeIndex == newPrimeIndex)
                {
                    state = false;
                }
                else
                {
                    ++currentPrimeIndex;
                    if (mul > this.squaredLimitPrime)
                    {
                        var addNode = this.GetNode(
                            this.current,
                            newPrimeIndex,
                            currentPrimeIndex);
                        this.AddNode(addNode);
                        state = false;
                    }
                }
            }
        }

        /// <summary>
        /// Processa o número composto especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        private void ProcessComposite(int index)
        {
            var state = true;

            var composite = this.composites[index];
            this.RemoveAt(index);

            var lastIndex = composite.Item2;
            var currentIndex = 0;
            while (state)
            {
                var mul = composite.Item1;
                mul *= this.oddPrimes[currentIndex];
                var tup = this.GetTuple(mul, currentIndex);
                this.AddComposite(tup);

                if (currentIndex == lastIndex)
                {
                    state = false;
                }
                else
                {
                    ++currentIndex;
                    if (mul > this.squaredLimitPrime)
                    {
                        var addNode = this.GetNode(
                            this.current,
                            lastIndex,
                            currentIndex);
                        this.AddNode(addNode);
                        state = false;
                    }
                }
            }
        }

        /// <summary>
        /// Processa um nó na lista ligada.
        /// </summary>
        /// <param name="node">O nó a ser processado.</param>
        /// <returns>O próximo nó ou nulo se este não existir.</returns>
        private Node ProcessNode(Node node)
        {
            var result = node;
            var state = true;
            while (state)
            {
                var numb = result.Numb;
                var primeIndex = result.ProcessedPrime;

                var prime = this.oddPrimes[primeIndex];
                var mul = numb * prime;

                var tup = this.GetTuple(mul, node.ProcessedPrime);
                this.AddComposite(tup);

                if (primeIndex == node.LeastPrimeDiv)
                {
                    result = node.Next;
                    state = false;
                }
                else
                {
                    ++node.ProcessedPrime;
                    if (mul > this.squaredLimitPrime)
                    {
                        state = false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Adiciona o nó à lista ligada.
        /// </summary>
        /// <param name="node">O nó a ser adicionado.</param>
        private void AddNode(Node node)
        {
            var next = this.storedIndices;
            this.storedIndices = node;
            node.Next = next;
        }

        /// <summary>
        /// Adiciona um composto ao conjunto.
        /// </summary>
        /// <param name="composite">O conjunto dos compostos.</param>
        private void AddComposite(
            MutableTuple<uint, int> composite)
        {
            var len = this.composites.LongLength;
            if (this.compositesCount == len)
            {
                this.ReserveCapacity(len << 1);
            }

            this.composites[this.compositesCount] = composite;
            this.HeapifyComposites(0, this.compositesCount);
            ++this.compositesCount;
        }

        /// <summary>
        /// Remove o item no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        private void RemoveAt(int index)
        {
            --this.compositesCount;
            var removed = this.composites[index];
            this.composites[index] = this.composites[this.compositesCount];
            if (index < this.compositesCount - 2 && this.compositesCount > 0)
            {
                this.SiftDown(index, this.compositesCount - 1);
            }

            if (removed != null)
            {
                if (this.emptyTuples.Count < maximumEmpty)
                {
                    this.emptyTuples.Add(removed);
                }
            }
        }

        /// <summary>
        /// Repõe a propriedade de meda no conjunto dos compostos.
        /// </summary>
        /// <param name="start">O valor inicial.</param>
        /// <param name="end">O valor final.</param>
        private void HeapifyComposites(int start, int end)
        {
            var child = end;
            var childVal = this.composites[child];
            while (child > start)
            {
                var parent = this.Parent(child);
                var parentVal = this.composites[parent];
                if (childVal.Item1 < parentVal.Item1)
                {
                    this.composites[child] = parentVal;
                    this.composites[parent] = childVal;
                    child = parent;
                }
                else
                {
                    child = start;
                }
            }
        }

        /// <summary>
        /// Coloca todos os itens na ordem pretendida, assumindo tratar-se
        /// da remolçção de um item da lista.
        /// </summary>
        /// <param name="start">
        /// O primeiro item.
        /// </param>
        /// <param name="end">
        /// O último item.
        /// </param>
        private void SiftDown(int start, int end)
        {
            var root = start;
            var leftRootIndex = this.Left(root);
            while (leftRootIndex <= end)
            {
                var swap = root;
                var swapValue = this.composites[root];
                var childValue = this.composites[leftRootIndex];
                if (childValue.Item1 < swapValue.Item1)
                {
                    swap = leftRootIndex;
                    ++leftRootIndex;
                    if (leftRootIndex <= end)
                    {
                        var temp = this.composites[leftRootIndex];
                        if (temp.Item1 < childValue.Item1)
                        {
                            swap = leftRootIndex;
                            childValue = temp;
                        }
                    }
                }
                else
                {
                    ++leftRootIndex;
                    if (leftRootIndex <= end)
                    {
                        childValue = this.composites[leftRootIndex];
                        if (childValue.Item1 < swapValue.Item1)
                        {
                            swap = leftRootIndex;
                        }
                    }
                }

                if (swap == root)
                {
                    return;
                }
                else
                {
                    this.composites[swap] = swapValue;
                    this.composites[root] = childValue;
                    root = swap;
                    leftRootIndex = this.Left(root);
                }
            }
        }

        /// <summary>
        /// Encontra o índice do tuplo cujo primeiro valor é passado em argumento.
        /// </summary>
        /// <param name="value">O valor a ser pesquisado.</param>
        /// <returns>O índice do tuplo caso existe e nulo caso contrário.</returns>
        private int Find(uint value)
        {
            var cnt = this.compositesCount;
            var searchStack = new Stack<int>();
            searchStack.Push(0);
            while (searchStack.Count > 0)
            {
                var top = searchStack.Pop();
                var current = this.composites[top];
                if (current.Item1 == value)
                {
                    return top;
                }
                else if (current.Item1 < value)
                {
                    var child = this.Left(top);
                    if (child < cnt)
                    {
                        searchStack.Push(child);
                        ++child;
                        if (child < cnt)
                        {
                            searchStack.Push(child);
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Obtém o índice onde se encontra o nó ascendente dado
        /// o nó descendente.
        /// </summary>
        /// <param name="i">O índice do nó descendente.</param>
        /// <returns>O índice do nó ascendente.</returns>
        private int Parent(int i)
        {
            return (i - 1) >> 1;
        }

        /// <summary>
        /// Obtém o índice descendente que se encontra à esquerda.
        /// </summary>
        /// <param name="i">O índice do nó ascendente.</param>
        /// <returns>O índice o nó ascendente que se encontra à esquerda.</returns>
        private int Left(int i)
        {
            return (i << 1) + 1;
        }

        /// <summary>
        /// Obtém o índice do nó descendente que se encontra à direita.
        /// </summary>
        /// <param name="i">O índice do nó ascendente.</param>
        /// <returns>O índice do nó ascendente que se encontra à direita.</returns>
        private int Right(int i)
        {
            return (i + 1) << 1;
        }

        /// <summary>
        /// Obtém um novo tuplo, evitando instâncias desnecessárias.
        /// </summary>
        /// <param name="numb">O número.</param>
        /// <param name="primeIndex">O índice do factor primo.</param>
        /// <returns>O tuplo.</returns>
        private MutableTuple<uint, int> GetTuple(
            uint numb,
            int primeIndex)
        {
            var tuple = default(MutableTuple<uint, int>);
            var count = this.emptyTuples.Count;
            if (count > 0)
            {
                tuple = this.emptyTuples[count - 1];
                tuple.Item1 = numb;
                tuple.Item2 = primeIndex;
                this.emptyTuples.RemoveAt(count - 1);
            }
            else
            {
                tuple = MutableTuple.Create(numb, primeIndex);
            }

            return tuple;
        }

        /// <summary>
        /// Obtém um novo nó, evitando instâncias desnecessárias.
        /// </summary>
        /// <param name="numb">O número composto.</param>
        /// <param name="leastPrimeDiv">O índice do menor divisor primo.</param>
        /// <param name="processedPrime">O índice do próximo primo a ser processado.</param>
        /// <returns>O nó.</returns>
        private Node GetNode(
            uint numb,
            int leastPrimeDiv,
            int processedPrime)
        {
            var node = this.emptyNodes;
            if (node == null)
            {
                node = new Node();
            }
            else
            {
                this.emptyNodes = node.Next;

            }

            node.Numb = numb;
            node.LeastPrimeDiv = leastPrimeDiv;
            node.ProcessedPrime = processedPrime;
            return node;
        }

        /// <summary>
        /// Reserva a capacidade para novos elementos do conjunto
        /// dos números compostos.
        /// </summary>
        /// <param name="capacity">Nova capacidade.</param>
        private void ReserveCapacity(long capacity)
        {
            var array = new MutableTuple<uint, int>[capacity];
            Array.Copy(this.composites, array, this.compositesCount);
            this.composites = array;
        }

        /// <summary>
        /// Função de diagnóstico que premite imprimir o valor dos nós.
        /// </summary>
        /// <remarks>
        /// Serve apenas para diagnóstico.
        /// </remarks>
        /// <returns>O valor textual dos nós.</returns>
        private string GetPrintedNodes()
        {
            if (this.storedIndices == null)
            {
                return "null";
            }
            else
            {
                var builder = new StringBuilder();
                builder.Append(this.storedIndices.Numb);
                var node = this.storedIndices.Next;
                while (node != null)
                {
                    builder.Append("; ");
                    builder.Append(node.Numb);
                    node = node.Next;
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Função que permite averiguar a estrutura de meda.
        /// </summary>
        /// <remarks>
        /// Serve apenas para diagnóstico.
        /// </remarks>
        private void AsserHeapStructure()
        {
            if (this.compositesCount > 1)
            {
                var stack = new Stack<int>();
                stack.Push(0);
                while (stack.Count > 0)
                {
                    var top = stack.Pop();
                    var topValue = this.composites[top].Item1;

                    var child = this.Left(top);
                    if (child < this.compositesCount)
                    {
                        var childValue = this.composites[child].Item1;
                        if (childValue < topValue)
                        {
                            throw new Exception("A estrutura de meda foi violada.");
                        }
                        else
                        {
                            stack.Push(child);
                            ++child;
                            if (child < this.compositesCount)
                            {
                                childValue = this.composites[child].Item1;
                                if (childValue < topValue)
                                {
                                    throw new Exception("A estrutura de meda foi violada.");
                                }
                                else
                                {
                                    stack.Push(child);
                                }
                            }
                        }
                    }
                }

                // Verifica elementos repetidos
                var dic = new Dictionary<uint, int>();
                for (var i = 0; i < this.compositesCount; ++i)
                {
                    if (dic.ContainsKey(this.composites[i].Item1))
                    {
                        throw new Exception("Chave existente.");
                    }
                    else
                    {
                        dic.Add(this.composites[i].Item1, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Permite fazer uma pesquisa aos tuplos existentes.
        /// </summary>
        /// <param name="tup">O valor do primeiro item.</param>
        /// <returns>O tuplo.</returns>
        private MutableTuple<uint, int> FindTuple(uint tup)
        {
            for (var i = 0; i < this.compositesCount; ++i)
            {
                var item = this.composites[i];
                if (item.Item1 == tup)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Define um nó para a lista ligada que irá conter os índices.
        /// </summary>
        private class Node
        {
            /// <summary>
            /// O número.
            /// </summary>
            private uint numb;

            /// <summary>
            /// O menor divisor primo do número.
            /// </summary>
            private int leastPrimeDiv;

            /// <summary>
            /// O índice do número primo processado.
            /// </summary>
            private int processedPrime;

            /// <summary>
            /// Um apontador para o próximo nó.
            /// </summary>
            private Node next = null;

            /// <summary>
            /// Obtém ou atribui o número.
            /// </summary>
            public uint Numb
            {
                get
                {
                    return this.numb;
                }
                set
                {
                    this.numb = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui o menor divisor primo do número.
            /// </summary>
            public int LeastPrimeDiv
            {
                get
                {
                    return this.leastPrimeDiv;
                }
                set
                {
                    this.leastPrimeDiv = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui o índice do número primo processado.
            /// </summary>
            public int ProcessedPrime
            {
                get
                {
                    return this.processedPrime;
                }
                set
                {
                    this.processedPrime = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui o apontador para o próximo nó.
            /// </summary>
            public Node Next
            {
                get
                {
                    return this.next;
                }
                set
                {
                    this.next = value;
                }
            }
        }

        /// <summary>
        /// Implementa um comparador de pares.
        /// </summary>
        /// <remarks>
        /// A comparação é realizada sobre o primeiro elemento.
        /// </remarks>
        private class MutTupComparer :
            IComparer<MutableTuple<uint, int>>
        {
            /// <summary>
            /// Compara dois itens.
            /// </summary>
            /// <param name="x">O primeiro item a ser comparado.</param>
            /// <param name="y">O segundo item a ser comparado.</param>
            /// <returns>
            /// O valor 1 caso o primeiro item seja superior ao segundo, 0 se
            /// ambos forem iguais e -1 caso o primeiro item seja inferior ao segundo.
            /// </returns>
            public int Compare(
                MutableTuple<uint, int> x,
                MutableTuple<uint, int> y)
            {
                return Comparer<uint>.Default.Compare(
                    x.Item1,
                    y.Item1);
            }
        }
    }

    /// <summary>
    /// Crivo para gerar números primos que elimina compostos ímpares.
    /// </summary>
    public class CompositePrimeSieveGeneratorV1
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
        /// Instancia uma nova instância de objectos do tipo <see cref="CompositePrimeSieveGeneratorV1"/>.
        /// </summary>
        public CompositePrimeSieveGeneratorV1()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CompositePrimeSieveGeneratorV1"/>.
        /// </summary>
        /// <param name="max">O limite máximo.</param>
        public CompositePrimeSieveGeneratorV1(int max)
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
                        return (result << 1) + 3;
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

    #endregion Código sem desempenho
}
