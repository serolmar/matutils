namespace Mathematics
{
    using System;
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
        private static int[] firstPrimes = new[] { 2, 3, 5 };

        /// <summary>
        /// As diferenças a serem analisadas quando são testados os números quanto à primalidade.
        /// </summary>
        private static int[] differenceNumbers = new[] { 6, 4, 2, 4, 2, 4, 6, 2 };

        /// <summary>
        /// O limite superior do iterador.
        /// </summary>
        private int upperLimit;

        public PrimeNumbersIterator(int upperLimit)
        {
            this.upperLimit = upperLimit;
        }

        /// <summary>
        /// Obtém o limite superior associado ao iterador.
        /// </summary>
        public int UpperLimit
        {
            get
            {
                return this.upperLimit;
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new PrimesNumberEnumerator(this.upperLimit, firstPrimes, differenceNumbers);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Implementa o enumerador para os números primos.
        /// </summary>
        private class PrimesNumberEnumerator : IEnumerator<int>
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
            /// O apontador para a colecção com os primeiros números primos.
            /// </summary>
            private int firstPrimesPointer;

            public PrimesNumberEnumerator(int upperLimit, int[] firstPrimes, int[] differenceNumbers)
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

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
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
                else
                {
                    while (true)
                    {
                        this.sequenceNumber = this.sequenceNumber +
                            this.differenceNumbers[this.collectionPointer];
                        if (this.sequenceNumber >= this.upperLimit)
                        {
                            return false;
                        }
                        else
                        {
                            this.collectionPointer = (this.collectionPointer + 1) %
                                this.differenceNumbers.Length;
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
            }

            /// <summary>
            /// Testa ingenuamente se o número especificado é primo.
            /// </summary>
            /// <param name="number">O número a testar.</param>
            /// <returns>Verdadeiro se o número for primo e falso caso seja composto.</returns>
            private bool NaivePrimalityCheck(int number)
            {
                var sequenceNumber = 1;
                var differencePointer = 0;
                sequenceNumber += this.differenceNumbers[differencePointer];
                var sqrt = (int)Math.Floor(Math.Sqrt(number));
                while (sequenceNumber <= sqrt)
                {
                    if (number % sequenceNumber == 0)
                    {
                        return false;
                    }
                    else
                    {
                        differencePointer = (differencePointer + 1) % this.differenceNumbers.Length;
                        sequenceNumber += this.differenceNumbers[differencePointer];
                    }
                }

                return true;
            }
        }
    }
}
