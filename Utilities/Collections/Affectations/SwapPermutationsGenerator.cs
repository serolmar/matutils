namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um permutador que reocrre apenas a transposições.
    /// </summary>
    public class SwapPermutationsGenerator : IEnumerable<int[]>
    {
        /// <summary>
        /// O número de elementos a permutar.
        /// </summary>
        private int elementsNumber;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SwapPermutationsGenerator"/>.
        /// </summary>
        /// <param name="elementsNumber">O número de elementos.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o número de elementos a permutar não for positivo.
        /// </exception>
        public SwapPermutationsGenerator(int elementsNumber)
        {
            if (this.elementsNumber < 0)
            {
                throw new IndexOutOfRangeException("The number of elements to permute must be non negative.");
            }
            else
            {
                this.elementsNumber = elementsNumber;
            }
        }

        /// <summary>
        /// Obtém o número de elementos.
        /// </summary>
        /// <value>
        /// O número de elementos.
        /// </value>
        public int ElementsNumber
        {
            get
            {
                return this.elementsNumber;
            }
        }

        /// <summary>
        /// Retorna um enumerador que itera a colecção.
        /// </summary>
        /// <returns>
        /// O enumerador.
        /// </returns>
        public IEnumerator<int[]> GetEnumerator()
        {
            return new SwapPermutationGeneratorEnumerator(this.elementsNumber);
        }

        /// <summary>
        /// Retorna um enumerador não genérico.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Public Enumerator Class

        /// <summary>
        /// Implementa um enumerador para o permutador baseado em transposições.
        /// </summary>
        private class SwapPermutationGeneratorEnumerator : IEnumerator<int[]>
        {
            /// <summary>
            /// O número de elementos
            /// </summary>
            protected int elementsNumber;

            /// <summary>
            /// Os índices actuais de afectação.
            /// </summary>
            protected int[] currentAffectationIndices;

            /// <summary>
            /// Os índices utilizados.
            /// </summary>
            protected List<int>[] usedIndices;

            /// <summary>
            /// Valor que indica se o enumerador se encontra antes do início da colecção.
            /// </summary>
            protected bool isBeforeStart = true;

            /// <summary>
            /// Valor que indica se o enumerador se encontra após o final da colecção.
            /// </summary>
            protected bool isAfterEnd = false;

            /// <summary>
            /// Valor que indica se o enumerador foi descartado.
            /// </summary>
            private bool disposed = false;

            /// <summary>
            /// O apontador actual.
            /// </summary>
            private int currentPointer = 0;

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="SwapPermutationGeneratorEnumerator"/>.
            /// </summary>
            /// <param name="elementsNumber">O número de elementos.</param>
            public SwapPermutationGeneratorEnumerator(int elementsNumber)
            {
                this.elementsNumber = elementsNumber;
            }

            /// <summary>
            /// Obtém o elemento da colecção apontado pelo enumerador.
            /// </summary>
            /// <exception cref="CollectionsException">
            /// Se o enumerador foi descartado ou se encontra fora dos limites da colecção.
            /// </exception>
            /// <returns>O elemento da colecção especificado pelo enumerador.</returns>
            public int[] Current
            {
                get
                {
                    if (this.disposed)
                    {
                        throw new CollectionsException("Enumerator has been disposed.");
                    }
                    else if (this.isBeforeStart)
                    {
                        throw new CollectionsException("Enumerator is in before start position.");
                    }
                    else if (this.isAfterEnd)
                    {
                        throw new CollectionsException("Enumerator is in after end position.");
                    }
                    else
                    {
                        return this.currentAffectationIndices;
                    }
                }
            }

            /// <summary>
            /// Obtém o elemento da colecção apontado pelo enumerador.
            /// </summary>
            /// <value>O elemento da colecção.</value>
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            /// <summary>
            /// Descarta o enumerador.
            /// </summary>
            public void Dispose()
            {
                this.usedIndices = null;
                this.disposed = true;
            }

            /// <summary>
            /// Avança o enumerador para o próximo elemento da colecção.
            /// </summary>
            /// <returns>
            /// Verdadeiro caso o enumerador avance e falso caso se encontre no final da colecção.
            /// </returns>
            /// <exception cref="CollectionsException">Se o enumerador foi descartado.</exception>
            public bool MoveNext()
            {
                if (this.disposed)
                {
                    throw new CollectionsException("Enumerator has been disposed.");
                }
                else if (!this.isAfterEnd)
                {
                    if (this.isBeforeStart)
                    {
                        if (this.elementsNumber == 0)
                        {
                            this.isBeforeStart = false;
                            this.isAfterEnd = true;
                            return false;
                        }
                        else
                        {
                            this.isBeforeStart = false;
                            if (this.currentAffectationIndices == null)
                            {
                                this.currentAffectationIndices = new int[elementsNumber];
                            }

                            if (this.usedIndices == null)
                            {
                                this.usedIndices = new List<int>[elementsNumber - 1];
                            }

                            for (int i = 0; i < this.elementsNumber - 1; ++i)
                            {
                                this.currentAffectationIndices[i] = i;
                                this.usedIndices[i] = new List<int>();
                            }

                            this.currentAffectationIndices[elementsNumber - 1] = elementsNumber - 1;
                            return true;
                        }
                    }
                    else
                    {
                        var result = true;
                        var state = 0;
                        this.currentPointer = this.elementsNumber - 2;
                        while (state != -1)
                        {
                            if (this.currentPointer == -1)
                            {
                                result = false;
                                state = -1;
                            }
                            else
                            {
                                var indexToSwap = this.GetSwapIndex();
                                if (indexToSwap != -1)
                                {
                                    this.usedIndices[this.currentPointer].Add(this.currentAffectationIndices[this.currentPointer]);
                                    for (int i = this.currentPointer + 1; i < this.elementsNumber - 1; ++i)
                                    {
                                        this.usedIndices[i].Clear();
                                    }

                                    var toSwap = this.currentAffectationIndices[indexToSwap];
                                    this.currentAffectationIndices[indexToSwap] = this.currentAffectationIndices[this.currentPointer];
                                    this.currentAffectationIndices[this.currentPointer] = toSwap;
                                    result = true;
                                    state = -1;
                                }
                                else
                                {
                                    --this.currentPointer;
                                }
                            }
                        }

                        return result;
                    }
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Inicializa o enumerador.
            /// </summary>
            public void Reset()
            {
                if (this.disposed)
                {
                }
                else
                {
                    this.isBeforeStart = true;
                    this.isAfterEnd = false;
                }
            }

            /// <summary>
            /// Obtém o índice que deverá ser trocado.
            /// </summary>
            /// <returns>O índice.</returns>
            private int GetSwapIndex()
            {
                var indexToSwap = -1;
                var currentUsedIndices = this.usedIndices[this.currentPointer];
                var minValue = 0;
                for (int i = this.currentPointer + 1; i < this.currentAffectationIndices.Length; ++i)
                {
                    if (!currentUsedIndices.Contains(this.currentAffectationIndices[i]))
                    {
                        indexToSwap = i;
                        minValue = this.currentAffectationIndices[i];
                        i = this.currentAffectationIndices.Length;
                    }
                }

                for (int i = indexToSwap + 1; i < this.currentAffectationIndices.Length; ++i)
                {
                    var affectationsValue = this.currentAffectationIndices[i];
                    if (!currentUsedIndices.Contains(this.currentAffectationIndices[i]) && affectationsValue < minValue)
                    {
                        indexToSwap = i;
                        minValue = this.currentAffectationIndices[i];
                    }
                }

                return indexToSwap;
            }
        }

        #endregion
    }
}
