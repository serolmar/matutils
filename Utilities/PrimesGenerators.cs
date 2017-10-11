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
    public class CompositePrimeSieveGenerator
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
        public CompositePrimeSieveGenerator()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PrimeSieveGenerator"/>.
        /// </summary>
        /// <param name="max">O número primo máximo.</param>
        public CompositePrimeSieveGenerator(uint max)
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
                ++this.currentNumb;
                if (this.currentNumb == 2)
                {
                    this.factorMap.Add(this.currentNumb, 2UL);
                    this.foundPrimes.Add(2UL);
                    state = false;
                }
                else if ((this.currentNumb & 1) == 0)
                {
                    this.factorMap.Add(this.currentNumb, 2UL);
                    var f = this.currentNumb >> 1;
                    if (f > 2)
                    {
                        // O próximo número primo é 3
                        if (this.factorMap[f] > 2)
                        {
                            this.factorMap.Add(3 * f, 3);
                        }
                    }
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
            this.currentNumb = 1UL;
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
