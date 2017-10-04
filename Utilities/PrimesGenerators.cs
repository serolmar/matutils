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
                    if (this.increment == 1)
                    {
                        for (var i = this.lastPrimePointer; i < len; i += prime)
                        {
                            this.items[i] = true;
                        }
                    }
                    else
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
    public class LinearPrimveSieveGenerator
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
        /// Instancia uma nova instância de objectos do tipo <see cref="LinearPrimveSieveGenerator"/>
        /// </summary>
        public LinearPrimveSieveGenerator()
        {
            this.Initialize(100);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LinearPrimveSieveGenerator"/>
        /// </summary>
        /// <param name="max">O número máximo.</param>
        public LinearPrimveSieveGenerator(uint max)
        {
            this.Initialize(max);
        }

        /// <summary>
        /// Obtém o próximo número primo.
        /// </summary>
        /// <returns>O número primo.</returns>
        /// <remarks>TODO: completar a implementação.</remarks>
        private uint GetNextPrime()
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
                        var p = this.current + 2;
                        var incP = p;
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
                                while (pointer < this.current)
                                {
                                    if (incP < len)
                                    {
                                        if (!this.items[pointer])
                                        {
                                            this.items[incP] = true;
                                        }

                                        pointer += 2;
                                    }
                                    else
                                    {
                                        // Termina o ciclo
                                        pointer = this.current;
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


                    ++this.current;
                }
            }

            throw new NotImplementedException();
            //return result;
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
