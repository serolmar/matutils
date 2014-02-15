﻿namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um iterador sobre o conjunto dos números primos.
    /// </summary>
    public class PrimeNumbersIterator : IEnumerable<int>
    {
        /// <summary>
        /// Os primeiros números primos.
        /// </summary>
        private static int[] firstPrimes;

        /// <summary>
        /// As diferenças entre os números analisados para primalidade.
        /// </summary>
        private static int[] differences;

        /// <summary>
        /// O limite superior do iterador.
        /// </summary>
        private int upperLimit;

        static PrimeNumbersIterator()
        {
            // Otbém os números primos e as diferenças a partir dos recursos.
            InitClassFromResources();
        }

        public PrimeNumbersIterator(int upperLimit)
        {
            this.upperLimit = upperLimit;
        }

        /// <summary>
        /// Obtém o limite superior associado ao iterador actual.
        /// </summary>
        public int UpperLimit
        {
            get
            {
                return this.upperLimit;
            }
        }

        /// <summary>
        /// Obtém o enumrador associado ao iterador.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<int> GetEnumerator()
        {
            return new PrimeNumbsEnumerator(this.upperLimit, firstPrimes, differences);
        }

        /// <summary>
        /// Obtém o enumerador não genérico.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Obtém os primeiros números primos a partir dos dados incluídos como recurso.
        /// </summary>
        /// <returns>Os primeiros números primos.</returns>
        private static void InitClassFromResources()
        {
            firstPrimes = new int[] { 2, 3, 5 };
            differences = new int[] { 2, 6, 4, 2 };
        }

        /// <summary>
        /// Obtém um vector de iteiros a partir de texto com números separados por vírgulas.
        /// </summary>
        /// <param name="text">O texto.</param>
        /// <returns>O vector de inteiros.</returns>
        private static int[] GetIntegersFromString(string text)
        {
            var resultList = new List<int>();
            var readed = string.Empty;
            for (int i = 0; i < text.Length; ++i)
            {
                var current = text[i];
                if (current == ',')
                {
                    resultList.Add(int.Parse(readed));
                    readed = string.Empty;
                }
                else
                {
                    readed += current;
                }
            }

            resultList.Add(int.Parse(readed));
            return resultList.ToArray();
        }

        private class PrimeNumbsEnumerator : IEnumerator<int>
        {
            /// <summary>
            /// Os primeiros números primos.
            /// </summary>
            private int[] firstPrimes;

            /// <summary>
            /// As diferenças que permitem gerar todos os números que não são divisíveis pelos primeiros
            /// números primos.
            /// </summary>
            private int[] differenceNumbers;

            /// <summary>
            /// O limite superior do iterador.
            /// </summary>
            private int upperLimit;

            /// <summary>
            /// O número primo actual.
            /// </summary>
            private int currentPrime;

            /// <summary>
            /// O número sequencial.
            /// </summary>
            private int sequenceNumber;

            /// <summary>
            /// O apontador para a colecção.
            /// </summary>
            private int collectionPointer;

            /// <summary>
            /// A direcção segundo a qual se desloca o apontador.
            /// </summary>
            private bool direction;

            /// <summary>
            /// A raiz quadrada actual.
            /// </summary>
            private int squareRoot;

            /// <summary>
            /// O próximo quadrado perfeito.
            /// </summary>
            private int nextPerfectSquare;

            /// <summary>
            /// O próximo número ímpar a ser adicionado ao quadrado perfeito de modo a obter
            /// o quadrado perfeito seguinte.
            /// </summary>
            private int oddNumber;

            /// <summary>
            /// O apontador para a colecção com os primeiros números primos.
            /// </summary>
            private int firstPrimesPointer;

            public PrimeNumbsEnumerator(
                int upperLimit, 
                int[] firstPrimes,
                int[] differenceNumbers)
            {
                this.firstPrimes = firstPrimes;
                this.differenceNumbers = differenceNumbers;
                this.upperLimit = upperLimit;
                this.Reset();
            }

            /// <summary>
            /// Obtém o valor actual. Se o enumerador se encontrar antes do início da colecção
            /// é retornado o valor unitário e se se encontrar após o final da colecção, é retornado
            /// o último número primo encontrado.
            /// </summary>
            public int Current
            {
                get
                {
                    return this.currentPrime;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.currentPrime;
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (this.currentPrime >= this.upperLimit)
                {
                    return false;
                }
                else if (this.firstPrimesPointer < this.firstPrimes.Length - 1)
                {
                    ++this.firstPrimesPointer;
                    var aux = this.firstPrimes[this.firstPrimesPointer];
                    if (aux >= this.upperLimit)
                    {
                        return false;
                    }
                    else
                    {
                        this.currentPrime = aux;
                        return true;
                    }
                }
                else if (this.firstPrimesPointer == this.firstPrimes.Length - 1)
                {
                    this.collectionPointer = 1;
                    this.currentPrime = 1 + differences[this.collectionPointer];
                    if (this.currentPrime < this.upperLimit)
                    {
                        ++this.firstPrimesPointer;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (this.firstPrimesPointer == this.firstPrimes.Length)
                    {
                        var nextPrimeNumber = 1 + this.differenceNumbers[this.collectionPointer];
                        this.squareRoot = (int)Math.Floor(Math.Sqrt(nextPrimeNumber));
                        this.nextPerfectSquare = (squareRoot + 1) * (squareRoot + 1);
                        this.oddNumber = 2 * this.squareRoot + 3;
                        ++this.firstPrimesPointer;
                    }

                    while (true)
                    {
                        this.sequenceNumber = this.sequenceNumber +
                            this.differenceNumbers[this.collectionPointer];
                        while (this.sequenceNumber >= this.nextPerfectSquare)
                        {
                            ++this.squareRoot;
                            this.nextPerfectSquare += this.oddNumber;
                            this.oddNumber += 2;
                        }

                        if (this.sequenceNumber >= this.upperLimit)
                        {
                            return false;
                        }
                        else
                        {
                            if (this.direction)
                            {
                                if (this.collectionPointer == differences.Length - 1)
                                {
                                    --this.collectionPointer;
                                    this.direction = false;
                                }
                                else
                                {
                                    ++this.collectionPointer;
                                }
                            }
                            else
                            {
                                if (this.collectionPointer == 0)
                                {
                                    ++this.collectionPointer;
                                    this.direction = true;
                                }
                                else
                                {
                                    --this.collectionPointer;
                                }
                            }

                            if (this.NaivePrimalityCheck(this.sequenceNumber))
                            {
                                this.currentPrime = this.sequenceNumber;
                                return true;
                            }
                        }
                    }
                }
            }

            public void Reset()
            {
                this.sequenceNumber = 1;
                this.collectionPointer = 0;
                this.firstPrimesPointer = -1;
                this.currentPrime = 1;
                this.direction = true;
            }

            /// <summary>
            /// Testa o número face à respectiva primalidade.
            /// </summary>
            /// <param name="number">O número a ser testado.</param>
            /// <returns>Veradeiro caso o número passe o teste e falso caso contrário.</returns>
            private bool NaivePrimalityCheck(int number)
            {
                for (int i = 0; i < this.firstPrimes.Length; ++i)
                {
                    var currentPrime = this.firstPrimes[i];
                    if (currentPrime <= this.squareRoot)
                    {
                        if (number % currentPrime == 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        i = this.firstPrimes.Length;
                    }
                }

                var pointer = 1;
                var direction = true;
                var sequence = 1;
                while (sequence <= this.squareRoot)
                {
                    sequence += differences[pointer];
                    if (direction)
                    {
                        if (pointer == differences.Length - 1)
                        {
                            --pointer;
                            direction = false;
                        }
                        else
                        {
                            ++pointer;
                        }
                    }
                    else
                    {
                        if (pointer == 0)
                        {
                            ++pointer;
                            direction = true;
                        }
                        else
                        {
                            --pointer;
                        }
                    }

                    if (number % sequence == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}