namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um iterador sobre o conjunto dos números primos.
    /// </summary>
    public class IntPrimeNumbersIterator : IEnumerable<int>
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

        /// <summary>
        /// Algoritmo que permite calcular a parte inteira da raiz quadrada de um número.
        /// </summary>
        private IAlgorithm<int, int> squareRootAlgorithm;

        /// <summary>
        /// Inicializa a classe <see cref="IntPrimeNumbersIterator"/>.
        /// </summary>
        static IntPrimeNumbersIterator()
        {
            // Otbém os números primos e as diferenças a partir dos recursos.
            InitClassFromResources();
        }

        /// <summary>
        /// IInstancia um novo objecto do tipo <see cref="IntPrimeNumbersIterator"/>.
        /// </summary>
        /// <param name="upperLimit">O limite superior.</param>
        /// <param name="squareRootAlgorithm">
        /// O algoritmo responsável pela determinação da parte inteira de raízes quadradas.
        /// </param>
        /// <exception cref="System.ArgumentNullException">Se o algoritmo das raízes quadradas for nulo.</exception>
        public IntPrimeNumbersIterator(int upperLimit, IAlgorithm<int, int> squareRootAlgorithm)
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
            return new PrimeNumbsEnumerator(
                this.upperLimit, 
                firstPrimes, 
                differences,
                this.squareRootAlgorithm);
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

        /// <summary>
        /// Um enumerador para números primos.
        /// </summary>
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

            /// <summary>
            /// Algoritmo que permite calcular a raiz quadrada de um número.
            /// </summary>
            private IAlgorithm<int, int> squareRootAlgorithm;

            /// <summary>
            /// Initializes a new instance of the <see cref="PrimeNumbsEnumerator"/> class.
            /// </summary>
            /// <param name="upperLimit">The upper limit.</param>
            /// <param name="firstPrimes">The first primes.</param>
            /// <param name="differenceNumbers">The difference numbers.</param>
            /// <param name="squareRootAlgorithm">The square root algorithm.</param>
            public PrimeNumbsEnumerator(
                int upperLimit, 
                int[] firstPrimes,
                int[] differenceNumbers,
                IAlgorithm<int, int> squareRootAlgorithm)
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
            public int Current
            {
                get
                {
                    return this.currentPrime;
                }
            }

            /// <summary>
            /// Obtém o elemento da colecção na posição apontada pelo enumerador.
            /// </summary>
            /// <returns>O elemento da colecção apontado pelo enumerador.</returns>
            object IEnumerator.Current
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
            /// Verdadeiro se o enumerador for avançado para o próximo elemento e falso caso se encontre 
            /// no fim da colecção.
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
            /// Retorna o enumerador para a sua posição inicial que se encontra antes do início da colecção.
            /// </summary>
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
