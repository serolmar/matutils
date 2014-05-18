namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um afectador capaz de decidir se existem repetições.
    /// </summary>
    public abstract class FastAfector : IEnumerable<int[]>
    {
        /// <summary>
        /// O número máximo de elementos a permutar.
        /// </summary>
        protected int count;

        /// <summary>
        /// O número de lugares para incluir na permutação.
        /// </summary>
        protected int numberOfPlaces;

        /// <summary>
        /// Obtém o número máximo de elementos.
        /// </summary>
        /// <value>
        /// O número máximo de elementos.
        /// </value>
        public int Count
        {
            get
            {
                return this.count;
            }
        }

        /// <summary>
        /// Obtém o número de lugares.
        /// </summary>
        /// <value>
        /// O número e lugares.
        /// </value>
        public int NumberOfPlaces
        {
            get
            {
                return this.numberOfPlaces;
            }
        }

        /// <summary>
        /// Obtém um enumerador para o afectador.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public abstract IEnumerator<int[]> GetEnumerator();

        /// <summary>
        /// Obtém um enumerador não genérico para o afectador.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Implementa um enumerador para o afectador.
        /// </summary>
        public abstract class FastAffectorEnumerator : IEnumerator<int[]>
        {
            /// <summary>
            /// O afectador.
            /// </summary>
            protected FastAfector thisFastAffector;

            /// <summary>
            /// Os índices de afecetação.
            /// </summary>
            protected int[] currentAffectationIndices;

            /// <summary>
            /// Valor que indica se o afectador se encontra antes do início.
            /// </summary>
            protected bool isBeforeStart = true;

            /// <summary>
            /// Valor que indica se o afectador se encontra após o final.
            /// </summary>
            protected bool isAfterEnd = false;

            /// <summary>
            /// O apontador actual.
            /// </summary>
            protected int currentPointer = 0;

            /// <summary>
            /// O estado no qual se encontra o afectador.
            /// </summary>
            private int state = 0;

            /// <summary>
            /// Instancia um novo oobjecto do tipo <see cref="FastAffectorEnumerator"/>.
            /// </summary>
            /// <param name="affector">O afectador.</param>
            public FastAffectorEnumerator(FastAfector affector)
            {
                this.thisFastAffector = affector;
            }

            /// <summary>
            /// Obtém o elemento da colecção especificado pelo enumerador.
            /// </summary>
            /// <returns>O elemento da colecção especificado pelo enumerador.</returns>
            public int[] Current
            {
                get { return this.GetCurrent(); }
            }

            /// <summary>
            /// Descarta o enumerador.
            /// </summary>
            public void Dispose()
            {
                this.thisFastAffector = null;
            }

            /// <summary>
            /// Obtém o elemento da colecção especificado pelo enumerador.
            /// </summary>
            /// <returns>O elemento da colecção especificado pelo enumerador.</returns>
            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            /// <summary>
            /// Avaça o enumerador para o próximo elemento da colecção.
            /// </summary>
            /// <returns>
            /// Verdadeiro caso o enumerador avance e falso caso este se encontre no final da colecção.
            /// </returns>
            public bool MoveNext()
            {
                if (this.isBeforeStart)
                {
                    this.isBeforeStart = false;
                    return this.AdvanceState();
                }
                else
                {
                    if (this.AdvanceState())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            /// <summary>
            /// Inicializa o enumerador.
            /// </summary>
            public void Reset()
            {
                this.isAfterEnd = false;
                this.isBeforeStart = true;
                this.currentPointer = 0;
                this.state = 0;
            }

            /// <summary>
            /// Avança para o próximo estado.
            /// </summary>
            /// <returns>Verdadeiro caso o avanço se efectue e falso caso contrário.</returns>
            protected bool AdvanceState()
            {
                bool go = true;
                while (go)
                {
                    if (this.state == 0)
                    {
                        if (this.currentPointer == this.thisFastAffector.NumberOfPlaces)
                        {
                            if (this.currentPointer == 0)
                            {
                                this.isAfterEnd = true;
                                return false;
                            }
                            this.state = 1;
                            go = false;
                        }
                        else if (this.VerifyRepetitions())
                        {
                            this.IncrementAffectations();
                            ++this.currentPointer;
                            if (this.currentPointer < this.currentAffectationIndices.Length)
                            {
                                this.ResetPointedIndex();
                            }
                        }
                        else
                        {
                            if (!this.IncrementCurrent())
                            {
                                state = 1;
                            }
                        }
                    }
                    else if (this.state == 1)
                    {
                        --this.currentPointer;
                        if (this.currentPointer < 0)
                        {
                            this.isAfterEnd = true;
                            return false;
                        }
                        this.DecrementAffectations();
                        ++this.currentAffectationIndices[this.currentPointer];
                        if (this.CheckForCurrAffectIndicesValidity())
                        {
                            this.state = 0;
                        }
                    }
                }

                return true;
            }

            /// <summary>
            /// Verifica a validade da afectação actual.
            /// </summary>
            /// <returns>Verdadeiro caso a afectação seja vaálida e falso caso contrário.</returns>
            protected abstract bool CheckForCurrAffectIndicesValidity();

            /// <summary>
            /// Verifica se o elemento corrente constitui ou não uma repetição.
            /// </summary>
            /// <returns>Verdadeiro caso existam repetições e falso caso contrário.</returns>
            protected abstract bool VerifyRepetitions();

            /// <summary>
            /// Incrementa o contador para a verificação de repetições.
            /// </summary>
            protected abstract void IncrementAffectations();

            /// <summary>
            /// Decremente o contador para verificação de repetições.
            /// </summary>
            protected abstract void DecrementAffectations();

            /// <summary>
            /// Inicializa o índice actual.
            /// </summary>
            protected abstract void ResetPointedIndex();

            /// <summary>
            /// Incremente o índice actual.
            /// </summary>
            /// <returns>Verdadeiro se o índice pode ser incrementado e falso caso contrário.</returns>
            protected abstract bool IncrementCurrent();

            /// <summary>
            /// Obtém a afectação actual.
            /// </summary>
            /// <returns>A afectação.</returns>
            /// <exception cref="CollectionsException">
            /// Se o enumerador se encontra antes do início ou após o final da colecção.
            /// </exception>
            protected virtual int[] GetCurrent()
            {
                if (this.isBeforeStart)
                {
                    throw new CollectionsException("Enumerator is in \"IsBeforeStart\" status.");
                }

                if (this.isAfterEnd)
                {
                    throw new CollectionsException("Enumerator is in \"IsAfterEnd\" status.");
                }

                //int[] result = new int[this.currentAffectationIndices.Length];
                //for (int i = 0; i < this.currentAffectationIndices.Length; ++i)
                //{
                //    result[i] = this.currentAffectationIndices[i];
                //}

                //return result;

                return this.currentAffectationIndices;
            }
        }
    }
}
