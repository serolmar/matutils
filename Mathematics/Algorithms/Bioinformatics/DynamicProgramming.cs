// -----------------------------------------------------------------------
// <copyright file="DynamicProgramming.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------


namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using Utilities;

    /// <summary>
    /// Implementa o algoritmo que permite obter o tamanho da subsequência comum
    /// mais longa entre duas sequências.
    /// </summary>
    /// <remarks>
    /// Neste caso, apenas o comprimento da maior ou maiores sequências comuns
    /// é calculado.
    /// </remarks>
    /// <typeparam name="T">
    /// O tipo dos objectos que constituem as entradas da primeira sequência.
    /// </typeparam>
    /// <typeparam name="P">
    /// O tipo dos objectos que constituem as entradas da segunda sequência.
    /// </typeparam>
    /// <typeparam name="Q">
    /// O objecto responsável pela contagem.
    /// </typeparam>
    public class LongestCommonSeqLen<T, P, Q>
        : IAlgorithm<IEnumerable<T>, IEnumerable<P>, Q>
    {
        /// <summary>
        /// Mantém o objecto responsável pela sincronização.
        /// </summary>
        private readonly object lockObject;

        /// <summary>
        /// Mantém o objecto responsável pela contagem.
        /// </summary>
        private IIntegerNumber<Q> integerNumber;

        /// <summary>
        /// Define a função responsável pela comparação dos elementos
        /// entre sequências.
        /// </summary>
        private Func<T, P, bool> sequenceEqualityComparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LongestCommonSeqLen{T, P, Q}"/>.
        /// </summary>
        /// <param name="sequenceEqualityComparer">
        /// A função responsável pela comparação dos elementos entre as duas
        /// sequências.
        /// </param>
        /// <param name="integerNumber">O objecto responsável pela contagem.</param>
        public LongestCommonSeqLen(
            Func<T, P, bool> sequenceEqualityComparer,
            IIntegerNumber<Q> integerNumber)
        {
            if (sequenceEqualityComparer == null)
            {
                throw new ArgumentNullException("sequenceEqualityComparer");
            }
            else if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else
            {
                this.lockObject = new object();
                this.sequenceEqualityComparer = sequenceEqualityComparer;
                this.integerNumber = integerNumber;
            }
        }

        /// <summary>
        /// Mantém o objecto responsável pela contagem.
        /// </summary>
        public IIntegerNumber<Q> IntegerNumber
        {
            get
            {
                return this.integerNumber;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Integer number can't be null.");
                }
                else
                {
                    lock (this.lockObject)
                    {
                        this.integerNumber = value;
                    }
                }
            }
        }

        /// <summary>
        /// Define a função responsável pela comparação dos elementos
        /// entre sequências.
        /// </summary>
        public Func<T, P, bool> SequenceEqualityComparer
        {
            get
            {
                return this.sequenceEqualityComparer;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Sequence equality comaprer can't be null.");
                }
                else
                {
                    lock (this.lockObject)
                    {
                        this.sequenceEqualityComparer = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho da maior subsequência comum entre duas sequências.
        /// </summary>
        /// <param name="first">A primeira sequência.</param>
        /// <param name="second">A segunda sequência.</param>
        /// <returns>O tamanho da maior subsequência comum.</returns>
        public Q Run(IEnumerable<T> first, IEnumerable<P> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else
            {
                var innerIntegerNumber = default(IIntegerNumber<Q>);
                var innerSequenceComparer = default(Func<T, P, bool>);
                lock (this.lockObject)
                {
                    innerIntegerNumber = this.integerNumber;
                    innerSequenceComparer = this.sequenceEqualityComparer;
                }

                var firstEnum = first.GetEnumerator();
                if (firstEnum.MoveNext())
                {
                    var secondEnum = second.GetEnumerator();
                    if (secondEnum.MoveNext())
                    {
                        var tab = new GeneralLongList<Q>();
                        var firstCurrent = firstEnum.Current;
                        var secondCurrent = secondEnum.Current;
                        var diag = innerIntegerNumber.AdditiveUnity;
                        if (innerSequenceComparer.Invoke(firstCurrent, secondCurrent))
                        {
                            diag = innerIntegerNumber.MultiplicativeUnity;
                            tab.Add(diag);
                        }
                        else
                        {
                            tab.Add(innerIntegerNumber.AdditiveUnity);
                        }

                        while (secondEnum.MoveNext())
                        {
                            secondCurrent = secondEnum.Current;
                            if (innerSequenceComparer.Invoke(firstCurrent, secondCurrent))
                            {
                                tab.Add(innerIntegerNumber.MultiplicativeUnity);
                            }
                            else
                            {
                                tab.Add(innerIntegerNumber.AdditiveUnity);
                            }
                        }

                        // Continuação do código
                        while (firstEnum.MoveNext())
                        {
                            secondEnum.Reset();
                            firstCurrent = firstEnum.Current;

                            secondEnum.MoveNext();
                            secondCurrent = secondEnum.Current;

                            diag = innerIntegerNumber.AdditiveUnity;
                            if (innerSequenceComparer.Invoke(
                                firstCurrent,
                                secondCurrent))
                            {
                                diag = innerIntegerNumber.MultiplicativeUnity;
                                tab[0] = diag;
                            }

                            var j = 1U;
                            while (secondEnum.MoveNext())
                            {
                                secondCurrent = secondEnum.Current;
                                if (innerSequenceComparer.Invoke(
                                firstCurrent,
                                secondCurrent))
                                {
                                    var aux = tab[j];
                                    tab[j] = innerIntegerNumber.Successor(diag);
                                    diag = aux;
                                }
                                else
                                {
                                    diag = tab[j];
                                    if (integerNumber.Compare(tab[j], tab[j - 1]) < 0)
                                    {
                                        tab[j] = tab[j - 1];
                                    }
                                }

                                ++j;
                            }
                        }

                        return tab[tab.Count - 1];
                    }
                    else
                    {
                        return innerIntegerNumber.AdditiveUnity;
                    }
                }
                else
                {
                    return innerIntegerNumber.AdditiveUnity;
                }
            }
        }
    }

    /// <summary>
    /// Implementa o algoritmo que permite obter todas as subsequências comuns
    /// mais longas entre duas sequências.
    /// </summary>
    /// <remarks>
    /// Todas as maiores subsquências comuns são determinadas, retornando os
    /// índices que correspondem.
    /// </remarks>
    /// <typeparam name="T">
    /// O tipo dos objectos que constituem as entradas da primeira sequência.
    /// </typeparam>
    /// <typeparam name="P">
    /// O tipo dos objectos que constituem as entradas da segunda sequência.
    /// </typeparam>
    public class AllLongestCommonSequence<T, P>
        : IAlgorithm<ILongList<T>, ILongList<P>, T, P, IEnumerable<Tuple<ILongList<T>, ILongList<P>>>>
    {
        /// <summary>
        /// Mantém o objecto responsável pela sincronização.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// Define a função responsável pela comparação dos elementos
        /// entre sequências.
        /// </summary>
        private Func<T, P, bool> sequenceEqualityComparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="AllLongestCommonSequence{T, P}"/>.
        /// </summary>
        /// <param name="sequenceEqualityComparer">
        /// A função responsável pela comparação dos elementos entre as duas
        /// sequências.
        /// </param>
        public AllLongestCommonSequence(
            Func<T, P, bool> sequenceEqualityComparer)
        {
            if (sequenceEqualityComparer == null)
            {
                throw new ArgumentNullException("sequenceEqualityComparer");
            }
            else
            {
                this.lockObject = new object();
                this.sequenceEqualityComparer = sequenceEqualityComparer;
            }
        }

        /// <summary>
        /// Define a função responsável pela comparação dos elementos
        /// entre sequências.
        /// </summary>
        public Func<T, P, bool> SequenceEqualityComparer
        {
            get
            {
                return this.sequenceEqualityComparer;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Sequence equality comaprer can't be null.");
                }
                else
                {
                    lock (this.lockObject)
                    {
                        this.sequenceEqualityComparer = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém as maiores subsequências comuns entre duas sequências.
        /// </summary>
        /// <param name="first">A primeira sequência.</param>
        /// <param name="second">A segunda sequência.</param>
        /// <param name="firstRepl">O carácter que será usado como substituição na primeira sequência.</param>
        /// <param name="secondRepl">O carácter que será usado como substituição na segunda sequência</param>
        /// <returns>O tamanho da maior subsequência comum.</returns>
        public IEnumerable<Tuple<ILongList<T>, ILongList<P>>> Run(
            ILongList<T> first,
            ILongList<P> second,
            T firstRepl,
            P secondRepl)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else if (firstRepl == null)
            {
                throw new ArgumentNullException("firstRepl");
            }
            else if (secondRepl == null)
            {
                throw new ArgumentNullException("secondRepl");
            }
            else
            {
                var innerSequenceComparer = default(Func<T, P, bool>);
                lock (this.lockObject)
                {
                    innerSequenceComparer = this.sequenceEqualityComparer;
                }

                var firstCount = first.LongCount();
                if (firstCount == 0)
                {
                    return Enumerable.Empty<Tuple<ILongList<T>, ILongList<P>>>();
                }
                else
                {
                    var secondCount = second.LongCount();
                    if (secondCount == 0)
                    {
                        return Enumerable.Empty<Tuple<ILongList<T>, ILongList<P>>>();
                    }
                    else
                    {
                        var directionMatrix = new Direction[firstCount, secondCount];

                        // O processo é executado por colunas
                        if (firstCount < secondCount)
                        {
                            var tab = new ulong[firstCount];
                            var secondChar = second[0];

                            // Primeira coluna
                            for (var i = 0L; i < firstCount; ++i)
                            {
                                var firstChar = first[i];
                                if (this.sequenceEqualityComparer(firstChar, secondChar))
                                {
                                    directionMatrix[i, 0] = Direction.DIAG;
                                    tab[i] = 1;
                                }
                                else
                                {
                                    directionMatrix[i, 0] = Direction.LEFT | Direction.UP;
                                }
                            }

                            // Restantes colunas necessitam comparações mais elaboradas
                            for (var j = 1L; j < secondCount; ++j)
                            {
                                secondChar = second[j];

                                // Primeira linha
                                var firstChar = first[0];
                                if (this.sequenceEqualityComparer(firstChar, secondChar))
                                {
                                    var tabVal = tab[0];
                                    if (tabVal == 1L)
                                    {
                                        directionMatrix[0, j] = Direction.UP | Direction.DIAG;
                                    }
                                    else
                                    {
                                        directionMatrix[0, j] = Direction.DIAG;
                                    }

                                    tab[0] = 1L;
                                }
                                else
                                {
                                    var tabVal = tab[0];
                                    if (tabVal == 1L)
                                    {
                                        directionMatrix[0, j] = Direction.UP;
                                    }
                                    else
                                    {
                                        directionMatrix[0, j] = Direction.LEFT | Direction.UP;
                                    }
                                }

                                // Linhas restantes
                                var prevTab = tab[0];
                                for (var i = 1L; i < firstCount; ++i)
                                {
                                    var anteriorTab = tab[i - 1];
                                    var currTab = tab[i];

                                    if (this.sequenceEqualityComparer(firstChar, secondChar))
                                    {
                                        var diagVal = prevTab + 1L;
                                        if (diagVal > currTab)
                                        {
                                            if (diagVal > anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.DIAG;
                                                tab[i] = diagVal;
                                            }
                                            else if (diagVal == anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.DIAG | Direction.UP;
                                                tab[i] = diagVal;
                                            }
                                            else
                                            {
                                                directionMatrix[i, j] = Direction.UP;
                                                tab[i] = anteriorTab;
                                            }
                                        }
                                        else if (diagVal == currTab)
                                        {
                                            if (diagVal > anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.DIAG | Direction.LEFT;
                                                tab[i] = diagVal;
                                            }
                                            else if (diagVal == anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.DIAG | Direction.UP | Direction.LEFT;
                                                tab[i] = diagVal;
                                            }
                                            else
                                            {
                                                directionMatrix[i, j] = Direction.UP;
                                                tab[i] = anteriorTab;
                                            }
                                        }
                                        else
                                        {
                                            if (currTab < anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.UP;
                                                tab[i] = anteriorTab;
                                            }
                                            else if (currTab == anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.LEFT | Direction.UP;
                                            }
                                            else
                                            {
                                                directionMatrix[i, j] = Direction.LEFT;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (currTab < anteriorTab)
                                        {
                                            directionMatrix[i, j] = Direction.UP;
                                            tab[i] = anteriorTab;
                                        }
                                        else if (currTab == anteriorTab)
                                        {
                                            directionMatrix[i, j] = Direction.LEFT | Direction.UP;
                                        }
                                        else
                                        {
                                            directionMatrix[i, j] = Direction.LEFT;
                                        }
                                    }

                                    prevTab = anteriorTab;
                                }
                            }
                        }
                        else // O processo é executado por linhas
                        {
                            var tab = new ulong[secondCount];
                            var firstChar = first[0];

                            // Primeira linha
                            for (var j = 0L; j < secondCount; ++j)
                            {
                                var secondChar = second[j];
                                if (this.sequenceEqualityComparer(firstChar, secondChar))
                                {
                                    directionMatrix[0, j] = Direction.DIAG;
                                    tab[j] = 1;
                                }
                                else
                                {
                                    directionMatrix[0, j] = Direction.LEFT | Direction.UP;
                                }
                            }

                            // Restantes linhas
                            for (var i = 0L; i < firstCount; ++i)
                            {
                                firstChar = first[i];

                                // Primeira coluna
                                var secondChar = second[0];
                                if (this.sequenceEqualityComparer(firstChar, secondChar))
                                {
                                    var tabVal = tab[0];
                                    if (tabVal == 1L)
                                    {
                                        directionMatrix[i, 0] = Direction.UP | Direction.DIAG;
                                    }
                                    else
                                    {
                                        directionMatrix[i, 0] = Direction.DIAG;
                                    }

                                    tab[0] = 1L;
                                }
                                else
                                {
                                    var tabVal = tab[0];
                                    if (tabVal == 1L)
                                    {
                                        directionMatrix[i, 0] = Direction.UP;
                                    }
                                    else
                                    {
                                        directionMatrix[i, 0] = Direction.LEFT | Direction.UP;
                                    }
                                }

                                // Restantes colunas
                                var prevTab = tab[0];
                                for (var j = 1L; j < secondCount; ++j)
                                {
                                    var anteriorTab = tab[j - 1];
                                    var currTab = tab[j];

                                    if (this.sequenceEqualityComparer(firstChar, secondChar))
                                    {
                                        var diagVal = prevTab + 1L;
                                        if (diagVal > currTab)
                                        {
                                            if (diagVal > anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.DIAG;
                                                tab[j] = diagVal;
                                            }
                                            else if (diagVal == anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.DIAG | Direction.LEFT;
                                                tab[j] = diagVal;
                                            }
                                            else
                                            {
                                                directionMatrix[i, j] = Direction.LEFT;
                                                tab[j] = anteriorTab;
                                            }
                                        }
                                        else if (diagVal == currTab)
                                        {
                                            if (diagVal > anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.DIAG | Direction.UP;
                                                tab[j] = diagVal;
                                            }
                                            else if (diagVal == anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.DIAG | Direction.LEFT | Direction.UP;
                                                tab[j] = diagVal;
                                            }
                                            else
                                            {
                                                directionMatrix[i, j] = Direction.LEFT;
                                                tab[j] = anteriorTab;
                                            }
                                        }
                                        else
                                        {
                                            if (currTab < anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.LEFT;
                                                tab[j] = anteriorTab;
                                            }
                                            else if (currTab == anteriorTab)
                                            {
                                                directionMatrix[i, j] = Direction.LEFT | Direction.UP;
                                            }
                                            else
                                            {
                                                directionMatrix[i, j] = Direction.UP;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (currTab < anteriorTab)
                                        {
                                            directionMatrix[i, j] = Direction.LEFT;
                                            tab[j] = anteriorTab;
                                        }
                                        else if (currTab == anteriorTab)
                                        {
                                            directionMatrix[i, j] = Direction.LEFT | Direction.UP;
                                        }
                                        else
                                        {
                                            directionMatrix[i, j] = Direction.UP;
                                        }
                                    }

                                    prevTab = anteriorTab;
                                }
                            }
                        }

                        return new SolutionEnumerable(
                            first,
                            second,
                            firstRepl,
                            secondRepl,
                            directionMatrix);
                    }
                }
            }
        }

        #region Tipos privados

        /// <summary>
        /// Enumera as direcções de deslocamento a serem armazenadas.
        /// </summary>
        private enum Direction
        {
            /// <summary>
            /// Sequência oriunda da diagonal.
            /// </summary>
            STOP = 0,

            /// <summary>
            /// Sequência oriunda de ambas as direcções.
            /// </summary>
            LEFT = 1,

            /// <summary>
            /// Sequência oriunda de cima.
            /// </summary>
            UP = 2,

            /// <summary>
            /// Sequência oriunda da esquerda.
            /// </summary>
            DIAG = 4
        }

        /// <summary>
        /// Implementa um enumerável para a solução.
        /// </summary>
        private class SolutionEnumerable
            : IEnumerable<Tuple<ILongList<T>, ILongList<P>>>
        {
            /// <summary>
            /// Mantém a primeira sequência.
            /// </summary>
            private ILongList<T> firstItem;

            /// <summary>
            /// Mantém a segunda sequência.
            /// </summary>
            private ILongList<P> secondItem;

            /// <summary>
            /// Mantém as lista das direcções.
            /// </summary>
            private Direction[,] directions;

            /// <summary>
            /// O item que servirá de substituição na primeira sequência.
            /// </summary>
            private T firstReplacement;

            /// <summary>
            /// O item que servirá de substituição na segunda sequência.
            /// </summary>
            private P secondReplacement;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="SolutionEnumerable"/>.
            /// </summary>
            /// <param name="firstItem">A primeira sequência.</param>
            /// <param name="secondItem">A segunda sequência.</param>
            /// <param name="firstReplacement">O carácter de substituição na primeira sequência.</param>
            /// <param name="secondReplacement">O carácter de substituição na segunda sequência.</param>
            /// <param name="directions">A matriz das direcções.</param>
            public SolutionEnumerable(
                ILongList<T> firstItem,
                ILongList<P> secondItem,
                T firstReplacement,
                P secondReplacement,
                Direction[,] directions)
            {
                this.firstItem = firstItem;
                this.secondItem = secondItem;
                this.firstReplacement = firstReplacement;
                this.secondReplacement = secondReplacement;
                this.directions = directions;
            }

            /// <summary>
            /// Obtém o enumerador.
            /// </summary>
            /// <returns>O enumerador.</returns>
            public IEnumerator<Tuple<ILongList<T>, ILongList<P>>> GetEnumerator()
            {
                return new SolutionEnumerator(
                    this.firstItem,
                    this.secondItem,
                    this.firstReplacement,
                    this.secondReplacement,
                    this.directions);
            }

            /// <summary>
            /// Obtém o enumerador não genérico.
            /// </summary>
            /// <returns>O enumerador.</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new SolutionEnumerator(
                    this.firstItem,
                    this.secondItem,
                    this.firstReplacement,
                    this.secondReplacement,
                    this.directions);
            }
        }

        /// <summary>
        /// Implementa o enumerador para a solução.
        /// </summary>
        private class SolutionEnumerator : IEnumerator<Tuple<ILongList<T>, ILongList<P>>>
        {
            /// <summary>
            /// Mantém a primeira sequência.
            /// </summary>
            private ILongList<T> firstItem;

            /// <summary>
            /// Mantém a segunda sequência.
            /// </summary>
            private ILongList<P> secondItem;

            /// <summary>
            /// Mantém o carácter de substituição da primeira sequência.
            /// </summary>
            private T firstReplacement;

            /// <summary>
            /// Mantém o carácter de substituição na segunda sequência.
            /// </summary>
            private P secondReplacement;

            /// <summary>
            /// Mantém a matriz das direcções.
            /// </summary>
            private Direction[,] directions;

            /// <summary>
            /// Mantém um valor que indica se o enumerador foi iniciado.
            /// </summary>
            private bool isBeforeStart;

            /// <summary>
            /// Mantém um valor que indica se o iterador se encontra no final.
            /// </summary>
            private bool isAfterEnd;

            /// <summary>
            /// Mantém um valor que indica se o iterador foi descartado.
            /// </summary>
            private bool disposed;

            /// <summary>
            /// Pilha que contém as bifurcações.
            /// </summary>
            Stack<Tuple<long, long, Direction>> bifStack;

            /// <summary>
            /// Mantém o resultado actual da enumeração na primeira sequência.
            /// </summary>
            private ILongList<T> firstSeqResult;

            /// <summary>
            /// Mantém o resultado actual da enumeração na segunda sequência.
            /// </summary>
            private ILongList<P> secondSeqResult;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="SolutionEnumerator"/>.
            /// </summary>
            /// <param name="firstItem">A primeira sequência.</param>
            /// <param name="secondItem">A segunda sequência.</param>
            /// <param name="firstReplacement">O carácter de substituição na primeira sequência.</param>
            /// <param name="secondReplacement">O carácter de substituição na segunda sequência.</param>
            /// <param name="directions">A matriz das direcções.</param>
            /// <param name="subseqLength">O tamanho da subsequência.</param>
            public SolutionEnumerator(
                ILongList<T> firstItem,
                ILongList<P> secondItem,
                T firstReplacement,
                P secondReplacement,
                Direction[,] directions)
            {
                this.firstItem = firstItem;
                this.secondItem = secondItem;
                this.firstReplacement = firstReplacement;
                this.secondReplacement = secondReplacement;
                this.directions = directions;
                this.isBeforeStart = true;
                this.isAfterEnd = false;

                this.bifStack = new Stack<Tuple<long, long, Direction>>();
                this.firstSeqResult = new GeneralLongList<T>();
                this.secondSeqResult = new GeneralLongList<P>();
            }

            /// <summary>
            /// Obtém o valor actual do enumerador.
            /// </summary>
            public Tuple<ILongList<T>, ILongList<P>> Current
            {
                get
                {
                    if (this.isBeforeStart)
                    {
                        throw new InvalidOperationException("Enumerator was not started.");
                    }
                    else if (this.isAfterEnd)
                    {
                        throw new InvalidOperationException("Enumerator is after end.");
                    }
                    else if (this.disposed)
                    {
                        throw new ObjectDisposedException("SolutionEnumerator");
                    }
                    else
                    {
                        return Tuple.Create(this.firstSeqResult, this.secondSeqResult);
                    }
                }
            }

            /// <summary>
            /// Descarta o enumerador.
            /// </summary>
            public void Dispose()
            {
                this.disposed = true;
                this.directions = null;
                this.bifStack = null;
                this.firstSeqResult = null;
                this.secondSeqResult = null;
            }

            /// <summary>
            /// Obtém o valor actual do enumerador como objecto.
            /// </summary>
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (this.isBeforeStart)
                    {
                        throw new InvalidOperationException("Enumerator was not started.");
                    }
                    else if (this.isAfterEnd)
                    {
                        throw new InvalidOperationException("Enumerator is after end.");
                    }
                    else if (this.disposed)
                    {
                        throw new ObjectDisposedException("SolutionEnumerator");
                    }
                    else
                    {
                        var firstResult = new GeneralLongArray<T>(this.firstItem.LongCount());
                        var secondResult = new GeneralLongArray<P>(this.secondItem.LongCount());
                        return Tuple.Create(firstSeqResult, secondSeqResult);
                    }
                }
            }

            /// <summary>
            /// Move para o próximo item do enumerador.
            /// </summary>
            /// <returns>
            /// Verdadeiro se o movimento for bem sucedido e falso caso contrário.
            /// </returns>
            public bool MoveNext()
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("SolutionEnumerator");
                }
                else if (this.isAfterEnd)
                {
                    return false;
                }
                else if (this.isBeforeStart)
                {
                    this.isBeforeStart = false;
                    return this.Initialize();
                }
                else
                {
                    return this.Advance();
                }
            }

            /// <summary>
            /// Reinicia o enumerador.
            /// </summary>
            public void Reset()
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("SolutionEnumerator");
                }
                else
                {
                    this.isAfterEnd = false;
                    this.isBeforeStart = true;
                    this.bifStack.Clear();
                    this.firstSeqResult.Clear();
                    this.secondSeqResult.Clear();
                }
            }

            /// <summary>
            /// Inicializa o enumerador.
            /// </summary>
            private bool Initialize()
            {
                var firstCoord = this.directions.GetLongLength(0) - 1;
                var secondCoord = this.directions.GetLongLength(1) - 1;

                short state = 0;
                while (state == 0)
                {
                    var direction = this.directions[firstCoord, secondCoord];
                    if ((direction & Direction.DIAG) != 0)
                    {
                        this.firstSeqResult.Insert(0, this.firstItem[firstCoord]);
                        this.secondSeqResult.Insert(0, this.secondItem[secondCoord]);

                        direction ^= Direction.DIAG;
                        if (direction != Direction.STOP)
                        {
                            this.bifStack.Push(
                                Tuple.Create(firstCoord, secondCoord, direction));
                        }

                        --firstCoord;
                        --secondCoord;
                    }
                    else if ((direction & Direction.UP) != 0)
                    {
                        this.firstSeqResult.Insert(0, this.firstItem[firstCoord]);
                        this.secondSeqResult.Insert(0, this.secondReplacement);

                        direction ^= Direction.UP;
                        if (direction != Direction.STOP)
                        {
                            this.bifStack.Push(
                                Tuple.Create(firstCoord, secondCoord, direction));
                        }

                        --secondCoord;
                    }
                    else if (direction == Direction.LEFT)
                    {
                        this.firstSeqResult.Insert(0, this.firstReplacement);
                        this.secondSeqResult.Insert(0, this.secondItem[secondCoord]);

                        --firstCoord;
                    }

                    if (firstCoord == 0)
                    {
                        state = 1;
                        for(var i = 0L; i < secondCoord; ++i)
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else if (secondCoord == 0)
                    {
                        state = 1;
                    }

                }

                return true;
            }

            /// <summary>
            /// Avança o enumerador para o próximo item.
            /// </summary>
            /// <returns>Verdadeiro se o movimento foi bem sucedido e falso caso contrário.</returns>
            private bool Advance()
            {
                if (this.bifStack.Count == 0)
                {
                    this.isAfterEnd = true;
                    return false;
                }
                else
                {
                    //var top = this.bifStack.Pop();
                    //var index = top.Item3;
                    //var firstCoord = top.Item1 - 1;
                    //var secondCoord = top.Item2;

                    //var state = true;
                    //while (state)
                    //{
                    //    var direction = directions[firstCoord, secondCoord];
                    //    if (direction == Direction.DIAG)
                    //    {
                    //        this.currentItems[index--] = this.items[firstCoord];
                    //        --firstCoord;
                    //        --secondCoord;
                    //        if (index <= 0)
                    //        {
                    //            state = false;
                    //        }
                    //    }
                    //    else if (direction == Direction.LEFT)
                    //    {
                    //        --secondCoord;
                    //    }
                    //    else if (direction == Direction.UP)
                    //    {
                    //        --firstCoord;
                    //    }
                    //    else if (direction == Direction.BOTH)
                    //    {
                    //        this.bifStack.Push(
                    //            Tuple.Create(firstCoord, secondCoord, index));
                    //        --secondCoord;
                    //    }
                    //    else if (direction == Direction.STOP)
                    //    {
                    //        if (this.bifStack.Count == 0)
                    //        {
                    //            this.isAfterEnd = true;
                    //            return false;
                    //        }
                    //        else
                    //        {
                    //            top = this.bifStack.Pop();
                    //            index = top.Item3;
                    //            firstCoord = top.Item1 - 1;
                    //            secondCoord = top.Item2;
                    //        }
                    //    }
                    //}

                    return true;
                }
            }
        }

        #endregion Tipos privados
    }


    //public class AllLongestCommonSequence<T, P>
    //    : IAlgorithm<ILongList<T>, ILongList<P>, IEnumerable<T[]>>
    //{
    //    /// <summary>
    //    /// Mantém o objecto responsável pela sincronização.
    //    /// </summary>
    //    private object lockObject;

    //    /// <summary>
    //    /// Define a função responsável pela comparação dos elementos
    //    /// entre sequências.
    //    /// </summary>
    //    private Func<T, P, bool> sequenceEqualityComparer;

    //    /// <summary>
    //    /// Instancia uma nova instância de objectos do tipo <see cref="AllLongestCommonSequence{T, P}"/>.
    //    /// </summary>
    //    /// <param name="sequenceEqualityComparer">
    //    /// A função responsável pela comparação dos elementos entre as duas
    //    /// sequências.
    //    /// </param>
    //    public AllLongestCommonSequence(
    //        Func<T, P, bool> sequenceEqualityComparer)
    //    {
    //        if (sequenceEqualityComparer == null)
    //        {
    //            throw new ArgumentNullException("sequenceEqualityComparer");
    //        }
    //        else
    //        {
    //            this.lockObject = new object();
    //            this.sequenceEqualityComparer = sequenceEqualityComparer;
    //        }
    //    }

    //    /// <summary>
    //    /// Define a função responsável pela comparação dos elementos
    //    /// entre sequências.
    //    /// </summary>
    //    public Func<T, P, bool> SequenceEqualityComparer
    //    {
    //        get
    //        {
    //            return this.sequenceEqualityComparer;
    //        }
    //        set
    //        {
    //            if (value == null)
    //            {
    //                throw new MathematicsException("Sequence equality comaprer can't be null.");
    //            }
    //            else
    //            {
    //                lock (this.lockObject)
    //                {
    //                    this.sequenceEqualityComparer = value;
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Obtém as maiores subsequências comuns entre duas sequências.
    //    /// </summary>
    //    /// <param name="first">A primeira sequência.</param>
    //    /// <param name="second">A segunda sequência.</param>
    //    /// <returns>O tamanho da maior subsequência comum.</returns>
    //    public IEnumerable<T[]> Run(ILongList<T> first, ILongList<P> second)
    //    {
    //        if (first == null)
    //        {
    //            throw new ArgumentNullException("first");
    //        }
    //        else if (second == null)
    //        {
    //            throw new ArgumentNullException("second");
    //        }
    //        else
    //        {
    //            var innerSequenceComparer = default(Func<T, P, bool>);
    //            lock (this.lockObject)
    //            {
    //                innerSequenceComparer = this.sequenceEqualityComparer;
    //            }

    //            var firstCount = first.LongCount();
    //            if (firstCount == 0)
    //            {
    //                return Enumerable.Empty<T[]>();
    //            }
    //            else
    //            {
    //                var secondCount = second.LongCount();
    //                if (secondCount == 0)
    //                {
    //                    return Enumerable.Empty<T[]>();
    //                }
    //                else
    //                {
    //                    var tab = new ulong[secondCount];
    //                    var dirMatrix = new Direction[firstCount, secondCount];

    //                    var diag = 0UL;
    //                    var firstCurrent = first[0];
    //                    for (var j = 0; j < secondCount; ++j)
    //                    {
    //                        if (innerSequenceComparer.Invoke(firstCurrent, second[j]))
    //                        {
    //                            diag = 1UL;
    //                            tab[j] = diag;
    //                            dirMatrix[0, j] = Direction.DIAG;
    //                        }
    //                        else
    //                        {
    //                            tab[j] = diag;
    //                            dirMatrix[0, j] = Direction.LEFT;
    //                        }
    //                    }

    //                    for (var i = 1; i < firstCount; ++i)
    //                    {
    //                        firstCurrent = first[i];
    //                        diag = 0UL;
    //                        if (innerSequenceComparer.Invoke(
    //                            firstCurrent,
    //                            second[0]))
    //                        {
    //                            diag = 1UL;
    //                            tab[0] = diag;
    //                            dirMatrix[i, 0] = Direction.DIAG;
    //                        }
    //                        else
    //                        {
    //                            dirMatrix[i, 0] = Direction.UP;
    //                        }

    //                        for (var j = 1; j < secondCount; ++j)
    //                        {
    //                            if (innerSequenceComparer.Invoke(
    //                            firstCurrent,
    //                            second[j]))
    //                            {
    //                                var aux = tab[j];
    //                                tab[j] = diag + 1;
    //                                diag = aux;
    //                                dirMatrix[i, j] = Direction.DIAG;
    //                            }
    //                            else
    //                            {
    //                                diag = tab[j];
    //                                if (tab[j] < tab[j - 1])
    //                                {
    //                                    tab[j] = tab[j - 1];
    //                                    dirMatrix[i, j] = Direction.LEFT;
    //                                }
    //                                else
    //                                {
    //                                    if (tab[j] == tab[j - 1])
    //                                    {
    //                                        dirMatrix[i, j] = Direction.BOTH;
    //                                    }
    //                                    else
    //                                    {
    //                                        dirMatrix[i, j] = Direction.UP;
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }

    //                    var len = tab[secondCount - 1];
    //                    if (len == 0)
    //                    {
    //                        return Enumerable.Empty<T[]>();
    //                    }
    //                    else
    //                    {
    //                        //this.RemoveCycles(dirMatrix);
    //                        return new SolutionEnumerable(
    //                            first,
    //                            dirMatrix,
    //                            len);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Remove os ciclos da matriz das direcções quando esta é encarada
    //    /// como um grafo.
    //    /// </summary>
    //    /// <param name="directions">A matriz das direcções.</param>
    //    private void RemoveCycles(Direction[,] directions)
    //    {
    //        var firstDim = directions.GetLongLength(0);
    //        var secondDim = directions.GetLongLength(1);
    //        var visited = new bool[firstDim, secondDim];

    //        --firstDim;
    //        --secondDim;
    //        var queue = new Queue<Tuple<long, long>>();
    //        queue.Enqueue(Tuple.Create(firstDim, secondDim));
    //        visited[firstDim, secondDim] = true;

    //        while (queue.Count > 0)
    //        {
    //            var coords = queue.Dequeue();
    //            var coordX = coords.Item1;
    //            var coordY = coords.Item2;
    //            var direction = directions[coordX, coordY];
    //            if (direction == Direction.DIAG)
    //            {
    //                if (coordX > 0 && coordY > 0)
    //                {
    //                    --coordX;
    //                    --coordY;
    //                    if (!visited[coordX, coordY])
    //                    {
    //                        visited[coordX, coordY] = true;
    //                        queue.Enqueue(Tuple.Create(coordX, coordY));
    //                    }
    //                }
    //            }
    //            else if (direction == Direction.LEFT)
    //            {
    //                if (coordY > 0)
    //                {
    //                    var prevY = coordY;
    //                    --coordY;
    //                    if (visited[coordX, coordY])
    //                    {
    //                        directions[coordX, prevY] = Direction.STOP;
    //                    }
    //                    else
    //                    {
    //                        visited[coordX, coordY] = true;
    //                        queue.Enqueue(Tuple.Create(coordX, coordY));
    //                    }
    //                }
    //            }
    //            else if (direction == Direction.UP)
    //            {
    //                if (coordX > 0)
    //                {
    //                    var prevX = coordX;
    //                    --coordX;
    //                    if (visited[coordX, coordY])
    //                    {
    //                        directions[prevX, coordY] = Direction.STOP;
    //                    }
    //                    else
    //                    {
    //                        visited[coordX, coordY] = true;
    //                        queue.Enqueue(Tuple.Create(coordX, coordY));
    //                    }
    //                }
    //            }
    //            else if (direction == Direction.BOTH)
    //            {
    //                if (coordY > 0)
    //                {
    //                    if (coordX > 0)
    //                    {
    //                        var prevX = coordX;
    //                        var prevY = coordY;
    //                        --coordX;
    //                        --coordY;
    //                        if (visited[prevX, coordY])
    //                        {
    //                            if (visited[coordX, prevY])
    //                            {
    //                                directions[prevX, prevY] = Direction.STOP;
    //                            }
    //                            else
    //                            {
    //                                directions[prevX, prevY] = Direction.UP;
    //                                visited[coordX, prevY] = true;
    //                                queue.Enqueue(Tuple.Create(coordX, prevY));
    //                            }
    //                        }
    //                        else if (visited[coordX, prevY])
    //                        {
    //                            directions[prevX, prevY] = Direction.LEFT;
    //                            visited[prevX, coordY] = true;
    //                            queue.Enqueue(Tuple.Create(coordX, prevY));
    //                        }
    //                        else
    //                        {
    //                            visited[prevX, coordY] = true;
    //                            queue.Enqueue(Tuple.Create(prevX, coordY));

    //                            visited[coordX, prevY] = true;
    //                            queue.Enqueue(Tuple.Create(coordX, prevY));
    //                        }
    //                    }
    //                    else
    //                    {
    //                        var prevY = coordY;
    //                        --coordY;
    //                        if (visited[coordX, coordY])
    //                        {
    //                            directions[coordX, prevY] = Direction.STOP;
    //                        }
    //                        else
    //                        {
    //                            directions[coordX, prevY] = Direction.LEFT;
    //                            visited[coordX, coordY] = true;
    //                            queue.Enqueue(Tuple.Create(coordX, coordY));
    //                        }
    //                    }
    //                }
    //                else if (coordX > 0)
    //                {
    //                    var prevX = coordX;
    //                    --coordX;
    //                    if (visited[coordX, coordY])
    //                    {
    //                        directions[prevX, coordY] = Direction.STOP;
    //                    }
    //                    else
    //                    {
    //                        directions[prevX, coordY] = Direction.UP;
    //                        visited[coordX, coordY] = true;
    //                        queue.Enqueue(Tuple.Create(coordX, coordY));
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    #region Tipos privados

    //    /// <summary>
    //    /// Enumera as direcções de deslocamento a serem armazenadas.
    //    /// </summary>
    //    private enum Direction
    //    {
    //        /// <summary>
    //        /// Sequência oriunda da diagonal.
    //        /// </summary>
    //        DIAG = 0,

    //        /// <summary>
    //        /// Sequência oriunda de ambas as direcções.
    //        /// </summary>
    //        BOTH = 1,

    //        /// <summary>
    //        /// Sequência oriunda de cima.
    //        /// </summary>
    //        UP = 2,

    //        /// <summary>
    //        /// Sequência oriunda da esquerda.
    //        /// </summary>
    //        LEFT = 3,

    //        /// <summary>
    //        /// Símbolo de paragem para evitar ciclos.
    //        /// </summary>
    //        STOP = 4
    //    }

    //    /// <summary>
    //    /// Implementa um enumerável para a solução.
    //    /// </summary>
    //    private class SolutionEnumerable
    //        : IEnumerable<T[]>
    //    {
    //        /// <summary>
    //        /// Mantém a lista dos itens.
    //        /// </summary>
    //        private ILongList<T> items;

    //        /// <summary>
    //        /// Mantém as lista das direcções.
    //        /// </summary>
    //        private Direction[,] directions;

    //        /// <summary>
    //        /// Obtém o tamanho da subsequência.
    //        /// </summary>
    //        private ulong subseqLength;

    //        /// <summary>
    //        /// Instancia uma nova instância de objectos do tipo <see cref="SolutionEnumerable"/>.
    //        /// </summary>
    //        /// <param name="items">A lista dos itens.</param>
    //        /// <param name="directions">A matriz das direcções.</param>
    //        /// <param name="subseqLength">O tamanho da subsequência.</param>
    //        public SolutionEnumerable(
    //            ILongList<T> items,
    //            Direction[,] directions,
    //            ulong subseqLength)
    //        {
    //            this.items = items;
    //            this.directions = directions;
    //            this.subseqLength = subseqLength;
    //        }

    //        /// <summary>
    //        /// Obtém o enumerador.
    //        /// </summary>
    //        /// <returns>O enumerador.</returns>
    //        public IEnumerator<T[]> GetEnumerator()
    //        {
    //            return new SolutionEnumerator(
    //                this.items,
    //                this.directions,
    //                this.subseqLength);
    //        }

    //        /// <summary>
    //        /// Obtém o enumerador não genérico.
    //        /// </summary>
    //        /// <returns>O enumerador.</returns>
    //        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //        {
    //            return new SolutionEnumerator(
    //                this.items,
    //                this.directions,
    //                this.subseqLength);
    //        }
    //    }

    //    /// <summary>
    //    /// Implementa o enumerador para a solução.
    //    /// </summary>
    //    private class SolutionEnumerator : IEnumerator<T[]>
    //    {
    //        /// <summary>
    //        /// Mantém a lista dos itens.
    //        /// </summary>
    //        private IList<T> items;

    //        /// <summary>
    //        /// Mantém a matriz das direcções.
    //        /// </summary>
    //        private Direction[,] directions;

    //        /// <summary>
    //        /// Mantém um valor que indica se o enumerador foi iniciado.
    //        /// </summary>
    //        private bool isBeforeStart;

    //        /// <summary>
    //        /// Mantém um valor que indica se o iterador se encontra no final.
    //        /// </summary>
    //        private bool isAfterEnd;

    //        /// <summary>
    //        /// Mantém um valor que indica se o iterador foi descartado.
    //        /// </summary>
    //        private bool disposed;

    //        /// <summary>
    //        /// Mantém a solução actual.
    //        /// </summary>
    //        private T[] currentItems;

    //        /// <summary>
    //        /// Pilha que contém as bifurcações.
    //        /// </summary>
    //        Stack<Tuple<int, int, long>> bifStack;

    //        /// <summary>
    //        /// Instancia uma nova instância de objectos do tipo <see cref="SolutionEnumerator"/>.
    //        /// </summary>
    //        /// <param name="items">A lista de itens.</param>
    //        /// <param name="directions">A matriz das direcções.</param>
    //        /// <param name="subseqLength">O tamanho da subsequência.</param>
    //        public SolutionEnumerator(
    //            ILongList<T> items,
    //            Direction[,] directions,
    //            ulong subseqLength)
    //        {
    //            this.items = items;
    //            this.directions = directions;
    //            this.isBeforeStart = true;
    //            this.isAfterEnd = false;

    //            this.currentItems = new T[subseqLength];
    //            this.bifStack = new Stack<Tuple<int, int, long>>();
    //        }

    //        /// <summary>
    //        /// Obtém o valor actual do enumerador.
    //        /// </summary>
    //        public T[] Current
    //        {
    //            get
    //            {
    //                if (this.isBeforeStart)
    //                {
    //                    throw new InvalidOperationException("Enumerator was not started.");
    //                }
    //                else if (this.isAfterEnd)
    //                {
    //                    throw new InvalidOperationException("Enumerator is after end.");
    //                }
    //                else if (this.disposed)
    //                {
    //                    throw new ObjectDisposedException("SolutionEnumerator");
    //                }
    //                else
    //                {
    //                    var len = this.currentItems.LongLength;
    //                    var result = new T[len];
    //                    Array.Copy(this.currentItems, result, len);
    //                    return result;
    //                }
    //            }
    //        }

    //        /// <summary>
    //        /// Descarta o enumerador.
    //        /// </summary>
    //        public void Dispose()
    //        {
    //            this.disposed = true;
    //            this.currentItems = null;
    //            this.directions = null;
    //            this.bifStack = null;
    //        }

    //        /// <summary>
    //        /// Obtém o valor actual do enumerador como objecto.
    //        /// </summary>
    //        object System.Collections.IEnumerator.Current
    //        {
    //            get
    //            {
    //                if (this.isBeforeStart)
    //                {
    //                    throw new InvalidOperationException("Enumerator was not started.");
    //                }
    //                else if (this.isAfterEnd)
    //                {
    //                    throw new InvalidOperationException("Enumerator is after end.");
    //                }
    //                else if (this.disposed)
    //                {
    //                    throw new ObjectDisposedException("SolutionEnumerator");
    //                }
    //                else
    //                {
    //                    var len = this.currentItems.LongLength;
    //                    var result = new T[len];
    //                    Array.Copy(this.currentItems, result, len);
    //                    return result;
    //                }
    //            }
    //        }

    //        /// <summary>
    //        /// Move para o próximo item do enumerador.
    //        /// </summary>
    //        /// <returns>
    //        /// Verdadeiro se o movimento for bem sucedido e falso caso contrário.
    //        /// </returns>
    //        public bool MoveNext()
    //        {
    //            if (this.disposed)
    //            {
    //                throw new ObjectDisposedException("SolutionEnumerator");
    //            }
    //            else if (this.isAfterEnd)
    //            {
    //                return false;
    //            }
    //            else if (this.isBeforeStart)
    //            {
    //                this.Initialize();
    //                this.isBeforeStart = false;
    //                return true;
    //            }
    //            else
    //            {
    //                return this.Advance();
    //            }
    //        }

    //        /// <summary>
    //        /// Reinicia o enumerador.
    //        /// </summary>
    //        public void Reset()
    //        {
    //            if (this.disposed)
    //            {
    //                throw new ObjectDisposedException("SolutionEnumerator");
    //            }
    //            else
    //            {
    //                this.isAfterEnd = false;
    //                this.isBeforeStart = true;
    //                this.bifStack.Clear();
    //            }
    //        }

    //        /// <summary>
    //        /// Inicializa o enumerador.
    //        /// </summary>
    //        private void Initialize()
    //        {
    //            var index = this.currentItems.LongLength - 1;
    //            var firstCoord = this.directions.GetLength(0) - 1;
    //            var secondCoord = this.directions.GetLength(1) - 1;

    //            while (index >= 0)
    //            {
    //                var direction = directions[firstCoord, secondCoord];
    //                if (direction == Direction.DIAG)
    //                {
    //                    this.currentItems[index--] = this.items[firstCoord];
    //                    --firstCoord;
    //                    --secondCoord;
    //                }
    //                else if (direction == Direction.LEFT)
    //                {
    //                    --secondCoord;
    //                }
    //                else if (direction == Direction.UP)
    //                {
    //                    --firstCoord;
    //                }
    //                else
    //                {
    //                    this.bifStack.Push(
    //                        Tuple.Create(firstCoord, secondCoord, index));
    //                    --secondCoord;
    //                }
    //            }
    //        }

    //        /// <summary>
    //        /// Avança o enumerador para o próximo item.
    //        /// </summary>
    //        /// <returns>Verdadeiro se o movimento foi bem sucedido e falso caso contrário.</returns>
    //        private bool Advance()
    //        {
    //            if (this.bifStack.Count == 0)
    //            {
    //                this.isAfterEnd = true;
    //                return false;
    //            }
    //            else
    //            {
    //                var top = this.bifStack.Pop();
    //                var index = top.Item3;
    //                var firstCoord = top.Item1 - 1;
    //                var secondCoord = top.Item2;

    //                var state = true;
    //                while (state)
    //                {
    //                    var direction = directions[firstCoord, secondCoord];
    //                    if (direction == Direction.DIAG)
    //                    {
    //                        this.currentItems[index--] = this.items[firstCoord];
    //                        --firstCoord;
    //                        --secondCoord;
    //                        if (index <= 0)
    //                        {
    //                            state = false;
    //                        }
    //                    }
    //                    else if (direction == Direction.LEFT)
    //                    {
    //                        --secondCoord;
    //                    }
    //                    else if (direction == Direction.UP)
    //                    {
    //                        --firstCoord;
    //                    }
    //                    else if (direction == Direction.BOTH)
    //                    {
    //                        this.bifStack.Push(
    //                            Tuple.Create(firstCoord, secondCoord, index));
    //                        --secondCoord;
    //                    }else if(direction == Direction.STOP)
    //                    {
    //                        if(this.bifStack.Count == 0)
    //                        {
    //                            this.isAfterEnd = true;
    //                            return false;
    //                        }
    //                        else
    //                        {
    //                            top = this.bifStack.Pop();
    //                            index = top.Item3;
    //                            firstCoord = top.Item1 - 1;
    //                            secondCoord = top.Item2;
    //                        }
    //                    }
    //                }

    //                return true;
    //            }
    //        }
    //    }

    //    #endregion Tipos privados
    //}

    /// <summary>
    /// Implementa o algoritmo que permite obter uma das subsequências comuns
    /// mais longas entre duas sequências.
    /// </summary>
    /// <remarks>
    /// Uma das maiores sequências comuns é retornada. Trata-se de uma abordagem
    /// mais rápida quando não é necessária a determinação de todas as subsequências.
    /// </remarks>
    /// <typeparam name="T">
    /// O tipo dos objectos que constituem as entradas da primeira sequência.
    /// </typeparam>
    /// <typeparam name="P">
    /// O tipo dos objectos que constituem as entradas da segunda sequência.
    /// </typeparam>
    public class LongestCommonSequence<T, P>
        : IAlgorithm<IList<T>, IList<P>, T[]>
    {
        /// <summary>
        /// Mantém o objecto responsável pela sincronização.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// Define a função responsável pela comparação dos elementos
        /// entre sequências.
        /// </summary>
        private Func<T, P, bool> sequenceEqualityComparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LongestCommonSequence{T, P}"/>.
        /// </summary>
        /// <param name="sequenceEqualityComparer">
        /// A função responsável pela comparação dos elementos entre as duas
        /// sequências.
        /// </param>
        public LongestCommonSequence(
            Func<T, P, bool> sequenceEqualityComparer)
        {
            if (sequenceEqualityComparer == null)
            {
                throw new ArgumentNullException("sequenceEqualityComparer");
            }
            else
            {
                this.lockObject = new object();
                this.sequenceEqualityComparer = sequenceEqualityComparer;
            }
        }

        /// <summary>
        /// Define a função responsável pela comparação dos elementos
        /// entre sequências.
        /// </summary>
        public Func<T, P, bool> SequenceEqualityComparer
        {
            get
            {
                return this.sequenceEqualityComparer;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Sequence equality comaprer can't be null.");
                }
                else
                {
                    lock (this.lockObject)
                    {
                        this.sequenceEqualityComparer = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém a maior subsequência comum entre duas sequências.
        /// </summary>
        /// <param name="first">A primeira sequência.</param>
        /// <param name="second">A segunda sequência.</param>
        /// <returns>O tamanho da maior subsequência comum.</returns>
        public T[] Run(IList<T> first, IList<P> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else
            {
                var innerSequenceComparer = default(Func<T, P, bool>);
                lock (this.lockObject)
                {
                    innerSequenceComparer = this.sequenceEqualityComparer;
                }

                var firstCount = first.LongCount();
                if (firstCount == 0)
                {
                    return new T[0];
                }
                else
                {
                    var secondCount = second.LongCount();
                    if (secondCount == 0)
                    {
                        return new T[0];
                    }
                    else
                    {
                        var tab = new long[secondCount];
                        var dirMatrix = new Direction[firstCount, secondCount];

                        var diag = 0L;
                        var firstCurrent = first[0];
                        for (var j = 0; j < secondCount; ++j)
                        {
                            if (innerSequenceComparer.Invoke(firstCurrent, second[j]))
                            {
                                diag = 1L;
                                tab[j] = diag;
                                dirMatrix[0, j] = Direction.DIAG;
                            }
                            else
                            {
                                tab[j] = diag;
                                dirMatrix[0, j] = Direction.LEFT;
                            }
                        }

                        for (var i = 1; i < firstCount; ++i)
                        {
                            firstCurrent = first[i];
                            diag = 0L;
                            if (innerSequenceComparer.Invoke(
                                firstCurrent,
                                second[0]))
                            {
                                diag = 1L;
                                tab[0] = diag;
                                dirMatrix[i, 0] = Direction.DIAG;
                            }
                            else
                            {
                                dirMatrix[i, 0] = Direction.UP;
                            }

                            for (var j = 1; j < secondCount; ++j)
                            {
                                if (innerSequenceComparer.Invoke(
                                firstCurrent,
                                second[j]))
                                {
                                    var aux = tab[j];
                                    tab[j] = diag + 1;
                                    diag = aux;
                                    dirMatrix[i, j] = Direction.DIAG;
                                }
                                else
                                {
                                    diag = tab[j];
                                    if (tab[j] <= tab[j - 1])
                                    {
                                        tab[j] = tab[j - 1];
                                        dirMatrix[i, j] = Direction.LEFT;
                                    }
                                    else
                                    {
                                        dirMatrix[i, j] = Direction.UP;
                                    }
                                }
                            }
                        }

                        var len = tab[secondCount - 1];
                        if (len == 0)
                        {
                            return new T[0];
                        }
                        else
                        {
                            return this.GetSolution(
                                len,
                                first,
                                dirMatrix);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o valor da solução.
        /// </summary>
        /// <param name="length">O tamanho da sequência.</param>
        /// <param name="first">A primeira sequência.</param>
        /// <param name="directions">A matriz das direcções.</param>
        /// <returns>A solução do problema.</returns>
        private T[] GetSolution(
            long length,
            IList<T> first,
            Direction[,] directions)
        {
            var result = new T[length];
            var index = length - 1;
            var firstCoord = directions.GetLength(0) - 1;
            var secondCoord = directions.GetLength(1) - 1;

            while (index >= 0)
            {
                var direction = directions[firstCoord, secondCoord];
                if (direction == Direction.DIAG)
                {
                    result[index--] = result[firstCoord];
                    --firstCoord;
                    --secondCoord;
                }
                else if (direction == Direction.LEFT)
                {
                    --secondCoord;
                }
                else if (direction == Direction.UP)
                {
                    --firstCoord;
                }
            }

            return result;
        }

        #region Tipos privados

        /// <summary>
        /// Enumera as direcções de deslocamento a serem armazenadas.
        /// </summary>
        private enum Direction
        {
            /// <summary>
            /// Sequência oriunda da diagonal.
            /// </summary>
            DIAG = 0,

            /// <summary>
            /// Sequência oriunda de cima.
            /// </summary>
            UP = 2,

            /// <summary>
            /// Sequência oriunda da esquerda.
            /// </summary>
            LEFT = 3
        }

        #endregion Tipos privados
    }

    /// <summary>
    /// Implementa o algoritmo que permite obter o caminho que define o alinhamento
    /// entre duas sequências de modo que o número de edições seja o menor possível.
    /// </summary>
    /// <remarks>
    /// Por exemplo, dadas as sequências AGCCA e ACGCA possuem, como maior subsequência
    /// comum, a sequência AGCA e o seguinte alinhamento:
    /// A-GCCA
    /// ACGC-A
    /// Este alinhamento correponde ao caminho
    /// (-1,-1)->(0,0)->(0,1)->(1,2)->(2,3)->(3,3)->(4,4)
    /// Em termos de diferenças:
    /// (1,1)->(0,1)->(1,1)->(1,1)->(1,0)->(1,1)
    /// O algoritmo retorna o vector de edições que permite construir a segunda sequência
    /// a partir da primeira
    /// Mantém A, Adiciona C, Mantém G, Mantém C, Apaga C, Mantém A
    /// Note-se que se pode dar o caso da substituição de uma letra por outra.
    /// </remarks>
    /// <typeparam name="T">
    /// O tipo dos objectos que constituem as entradas da primeira sequência.
    /// </typeparam>
    /// <typeparam name="P">
    /// O tipo dos objectos que constituem as entradas da segunda sequência.
    /// </typeparam>
    /// <typeparam name="W">
    /// O tipo dos objectos que constituem os pesos das transições.
    /// </typeparam>
    /// <typeparam name="Q">
    /// O objecto responsável pela determinação das posições, considerando valores relativos
    /// às posições anteriores na enumeração.
    /// </typeparam>
    public class AllShortestEditSequences<T, P, W, Q>
        : IAlgorithm<
            IEnumerable<T>,
            IEnumerable<P>,
            Func<T, P, W>,
            IEnumerable<Tuple<AllShortestEditSequences<T, P, W, Q>.EditionStatus, Q, Q>>>
    {
        /// <summary>
        /// Mantém o objecto responsável pela sincronização.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// Mantém o objecto responsável pela actualização dos pesos.
        /// </summary>
        private IMonoid<W> weightsMonoid;

        /// <summary>
        /// Mantém o objecto responsável pela comparação de pesos.
        /// </summary>
        private IComparer<W> weightsComparer;

        /// <summary>
        /// Mantém o objecto responsável pelas operações sobre as posições.
        /// </summary>
        private IIntegerNumber<Q> posIntegerNumber;

        /// <summary>
        /// Define a função responsável pela comparação dos elementos
        /// entre sequências.
        /// </summary>
        private Func<T, P, bool> sequenceEqualityComparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="AllShortestEditSequences{T, P, W, Q}"/>.
        /// </summary>
        /// <param name="sequenceEqualityComparer">O comparador entre elementos das sequências.</param>
        /// <param name="monoid">O objecto responsável pelas operações sobre os pesos.</param>
        /// <param name="posIntegerNumber">
        /// O objecto responsável pelas operações sobre as posições nos enumeráveis.
        /// </param>
        public AllShortestEditSequences(
            Func<T, P, bool> sequenceEqualityComparer,
            IMonoid<W> monoid,
            IIntegerNumber<Q> posIntegerNumber)
        {
            if (sequenceEqualityComparer == null)
            {
                throw new ArgumentNullException("sequenceEqualityComparer");
            }
            else if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (posIntegerNumber == null)
            {
                throw new ArgumentException("posIntegerNumber");
            }
            else
            {
                this.lockObject = new object();
                this.sequenceEqualityComparer = sequenceEqualityComparer;
                this.weightsMonoid = monoid;
                this.weightsComparer = Comparer<W>.Default;
                this.posIntegerNumber = posIntegerNumber;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="AllShortestEditSequences{T, P, W, Q}"/>.
        /// </summary>
        /// <param name="sequenceEqualityComparer">O comparador entre elementos das sequências.</param>
        /// <param name="monoid">O objecto responsável pelas operações sobre os pesos.</param>
        /// <param name="weightsComparer">O objecto responsável pela comparação de pesos.</param>
        /// <param name="posIntegerNumber">
        /// O objecto responsável pelas operações sobre as posições nos enumeráveis.
        /// </param>
        public AllShortestEditSequences(
            Func<T, P, bool> sequenceEqualityComparer,
            IMonoid<W> monoid,
            IComparer<W> weightsComparer,
            IIntegerNumber<Q> posIntegerNumber)
        {
            if (sequenceEqualityComparer == null)
            {
                throw new ArgumentNullException("sequenceEqualityComparer");
            }
            else if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (posIntegerNumber == null)
            {
                throw new ArgumentException("posIntegerNumber");
            }
            else
            {
                this.lockObject = new object();
                this.sequenceEqualityComparer = sequenceEqualityComparer;
                this.weightsMonoid = monoid;
                this.weightsComparer = Comparer<W>.Default;
                this.posIntegerNumber = posIntegerNumber;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pela sincronização.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this.lockObject;
            }
        }

        /// <summary>
        /// Obtém ou atribui o objecto responsável pela operação sobre os pesos.
        /// </summary>
        public IMonoid<W> WeightsMonoid
        {
            get
            {
                return this.weightsMonoid;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Weights monoid can't be null.");
                }
                else
                {
                    lock (this.lockObject)
                    {
                        this.weightsMonoid = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o comparador de pesos.
        /// </summary>
        public IComparer<W> WeightsComparer
        {
            get
            {
                return this.weightsComparer;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Weights comparer can't be null.");
                }
                else
                {
                    lock (this.lockObject)
                    {
                        this.weightsComparer = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o objecto responsável pelas operações sobre as posições.
        /// </summary>
        public IIntegerNumber<Q> PosIntegerNumber
        {
            get
            {
                return this.posIntegerNumber;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Position integer number can't be null.");
                }
                else
                {
                    lock (this.lockObject)
                    {
                        this.posIntegerNumber = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui a função responsável pela comparação dos elementos
        /// entre sequências.
        /// </summary>
        public Func<T, P, bool> SequenceEqualityComparer
        {
            get
            {
                return this.sequenceEqualityComparer;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Sequence equality comaprer can't be null.");
                }
                else
                {
                    lock (this.lockObject)
                    {
                        this.sequenceEqualityComparer = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o caminho que representa a maior subsequência comum entre 
        /// duas sequências.
        /// </summary>
        /// <param name="first">A primeira sequência.</param>
        /// <param name="second">A segunda sequência.</param>
        /// <param name="third">O peso atribuído ao alinhamento encontrado.</param>
        /// <returns>O tamanho da maior subsequência comum.</returns>
        public IEnumerable<Tuple<AllShortestEditSequences<T, P, W, Q>.EditionStatus, Q, Q>> Run(
            IEnumerable<T> first,
            IEnumerable<P> second,
            Func<T, P, W> third)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else if (third == null)
            {
                throw new ArgumentNullException("third");
            }
            else
            {
                var innerSeqComparer = default(Func<T, P, bool>);
                var innerMonoid = default(IMonoid<W>);
                var innerWeightsComparer = default(IComparer<W>);
                var innerPosIntegerNumber = default(IIntegerNumber<Q>);
                lock (this.lockObject)
                {
                    innerSeqComparer = this.sequenceEqualityComparer;
                    innerMonoid = this.weightsMonoid;
                    innerWeightsComparer = this.weightsComparer;
                    innerPosIntegerNumber = this.posIntegerNumber;
                }

                throw new NotImplementedException();
            }
        }

        #region Public classes

        /// <summary>
        /// Enumeração dos estados de edição.
        /// </summary>
        public enum EditionStatus
        {
            /// <summary>
            /// Adição.
            /// </summary>
            ADD = 0,

            /// <summary>
            /// Remoção.
            /// </summary>
            DELETE = 1,

            /// <summary>
            /// Substituição.
            /// </summary>
            REPLACE = 2
        }

        #endregion Public classes
    }
}
