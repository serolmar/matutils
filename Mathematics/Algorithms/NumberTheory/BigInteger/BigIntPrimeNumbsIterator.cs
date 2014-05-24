namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Implementa um iterador para números primos.
    /// </summary>
    public class BigIntPrimeNumbsIterator : IEnumerable<BigInteger>
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
        private BigInteger upperLimit;

        /// <summary>
        /// Algoritmo que permite calcular a parte inteira da raiz quadrada de um número.
        /// </summary>
        private IAlgorithm<BigInteger, BigInteger> squareRootAlgorithm;

        static BigIntPrimeNumbsIterator()
        {
            // Otbém os números primos e as diferenças a partir dos recursos.
            InitClassFromResources();
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BigIntPrimeNumbsIterator"/>.
        /// </summary>
        /// <param name="upperLimit">O limite superior.</param>
        /// <param name="squareRootAlgorithm">
        /// O algoritmo que permite calcular a parte inteira de raízes quadradas.
        /// </param>
        /// <exception cref="ArgumentNullException">Se o algoritmo das raízes quadradas for nulo.</exception>
        public BigIntPrimeNumbsIterator(
            BigInteger upperLimit,
            IAlgorithm<BigInteger, BigInteger> squareRootAlgorithm)
        {
            if (squareRootAlgorithm == null)
            {
                throw new ArgumentNullException("squareRootAlgorithm");
            }
            else
            {
                this.squareRootAlgorithm = squareRootAlgorithm;
                this.upperLimit = upperLimit;
            }
        }

        /// <summary>
        /// Obtém o limite superior associado ao iterador actual.
        /// </summary>
        /// <value>O limite superior.</value>
        public BigInteger UpperLimit
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
        public IEnumerator<BigInteger> GetEnumerator()
        {
            return new BigIntPrimeNumbsEnumerator(
                this.upperLimit, 
                firstPrimes, 
                differences,
                this.squareRootAlgorithm);
        }

        /// <summary>
        /// Obtém o enumerador não genérico.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Obtém os primeiros números primos a partir dos dados incluídos como recurso.
        /// </summary>
        /// <returns>Os primeiros números primos.</returns>
        private static void InitClassFromResources()
        {
            firstPrimes = new int[] { 2, 3, 5 }; // GetIntegersFromString(Resources.StartPrimes);
            differences = new int[] { 2, 6, 4, 2, 4 };// GetIntegersFromString(Resources.PrimeDifferences);
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

        /// <summary>
        /// Define um enumerador para os números primos.
        /// </summary>
        private class BigIntPrimeNumbsEnumerator : IEnumerator<BigInteger>
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
            private BigInteger upperLimit;

            /// <summary>
            /// O número primo actual.
            /// </summary>
            private BigInteger currentPrime;

            /// <summary>
            /// O número sequencial.
            /// </summary>
            private BigInteger sequenceNumber;

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
            private BigInteger squareRoot;

            /// <summary>
            /// O próximo quadrado perfeito.
            /// </summary>
            private BigInteger nextPerfectSquare;

            /// <summary>
            /// O próximo número ímpar a ser adicionado ao quadrado perfeito de modo a obter
            /// o quadrado perfeito seguinte.
            /// </summary>
            private BigInteger oddNumber;

            /// <summary>
            /// O apontador para a colecção com os primeiros números primos.
            /// </summary>
            private int firstPrimesPointer;

            /// <summary>
            /// O algoritmo que permite calcular a parte inteira da raiz quadrada de um número.
            /// </summary>
            private IAlgorithm<BigInteger, BigInteger> squareRootAlgorithm;

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="BigIntPrimeNumbsEnumerator"/>.
            /// </summary>
            /// <param name="upperLimit">O limite superior.</param>
            /// <param name="firstPrimes">Os primeiros primos.</param>
            /// <param name="differenceNumbers">A lista de diferenças entre os primeiros números primos.</param>
            /// <param name="squareRootAlgorithm">
            /// O algoritmo que permite calcular a parte inteira da raiz quadrada.
            /// </param>
            public BigIntPrimeNumbsEnumerator(
                BigInteger upperLimit, 
                int[] firstPrimes,
                int[] differenceNumbers,
                IAlgorithm<BigInteger, BigInteger> squareRootAlgorithm)
            {
                this.firstPrimes = firstPrimes;
                this.differenceNumbers = differenceNumbers;
                this.upperLimit = upperLimit;
                this.squareRootAlgorithm = squareRootAlgorithm;
                this.Reset();
            }

            /// <summary>
            /// Obtém o valor actual. Se o enumerador se encontrar antes do início da colecção
            /// é retornado o valor unitário e se se encontrar após o final da colecção, é retornado
            /// o último número primo encontrado.
            /// </summary>
            /// <value>O valor actual.</value>
            public BigInteger Current
            {
                get
                {
                    return this.currentPrime;
                }
            }

            /// <summary>
            /// Obtém o objecto na posição actual do enumerador.
            /// </summary>
            /// <returns>O elemento da colecção apontado pelo enumerador.</returns>
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.currentPrime;
                }
            }

            /// <summary>
            /// Descarta o enumerador.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Avança o enumerador para o próximo elemento da colecção.
            /// </summary>
            /// <returns>
            /// Verdadeiro caso o enumerador avance para o próximo elemento e falso caso esteja no final da colecção.
            /// </returns>
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
                        this.squareRoot = this.squareRootAlgorithm.Run(nextPrimeNumber);
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

            /// <summary>
            /// Retorna o enumerador à sua posição inicial que se encontra antes do início da colecção.
            /// </summary>
            public void Reset()
            {
                this.sequenceNumber = BigInteger.One;
                this.collectionPointer = 0;
                this.firstPrimesPointer = -1;
                this.currentPrime = BigInteger.One;
                this.direction = true;
            }

            /// <summary>
            /// Testa o número face à respectiva primalidade.
            /// </summary>
            /// <param name="number">O número a ser testado.</param>
            /// <returns>Veradeiro caso o número passe o teste e falso caso contrário.</returns>
            private bool NaivePrimalityCheck(BigInteger number)
            {
                var pointer = 1;
                var direction = true;
                var sequence = BigInteger.One;
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

                    if ((number % sequence).IsZero)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
