namespace Utilities.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Gera códigos BCD com um determinado comprimento.
    /// </summary>
    public class BcdCodeGenerator : IEnumerable<BitArray>
    {
        /// <summary>
        /// O tamanho dos códigos.
        /// </summary>
        private int bcdSize;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BcdCodeGenerator"/>.
        /// </summary>
        /// <param name="bcdSize">O tamanho dos códigos BCD.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Se o tamanho dos códigos BCD for negativo.
        /// </exception>
        public BcdCodeGenerator(int bcdSize)
        {
            if (bcdSize < 0)
            {
                throw new ArgumentOutOfRangeException("bcdSize");
            }
            else
            {
                this.bcdSize = bcdSize;
            }
        }

        /// <summary>
        /// Obtém o enumerador de códigos BCD com um determinado comprimento.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<BitArray> GetEnumerator()
        {
            return new BcdCodeGeneratorEnumerator(this.bcdSize);
        }

        /// <summary>
        /// Obtém o enumerador não genérico.
        /// </summary>
        /// <returns>O enumerador.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Implementa um enumerador para os códigos BCD.
        /// </summary>
        private class BcdCodeGeneratorEnumerator : IEnumerator<BitArray>
        {
            /// <summary>
            /// O tamano dos códigos BCD.
            /// </summary>
            private int bcdSize;

            /// <summary>
            /// O vector de bits.
            /// </summary>
            private BitArray bcdGeneratorArray;

            /// <summary>
            /// O apontador para o vector.
            /// </summary>
            private int bcdPointer;

            /// <summary>
            /// O vector actual.
            /// </summary>
            private BitArray current;

            /// <summary>
            /// Valor que indica o estado do movimento do apontador.
            /// </summary>
            private bool moveState;

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="BcdCodeGeneratorEnumerator"/>.
            /// </summary>
            /// <param name="bcdSize">O tamanho dos códigos BCD.</param>
            public BcdCodeGeneratorEnumerator(int bcdSize)
            {
                this.bcdSize = bcdSize;
                this.bcdGeneratorArray = new BitArray(bcdSize, false);
                this.Reset();
            }

            /// <summary>
            /// Obtém o elemento da colecção apontado pelo enumerador.
            /// </summary>
            /// <exception cref="CollectionsException">
            /// Se o enumerador se encontra fora dos limites da colecção.
            /// </exception>
            /// <returns>O elemento da colecção especificado pelo enumerador.</returns>
            public BitArray Current
            {
                get
                {
                    if (this.bcdPointer == this.bcdSize)
                    {
                        throw new Exception("Enumerator is at the before start position.");
                    }
                    else if (this.bcdPointer == -1)
                    {
                        throw new Exception("Enumerator is at the after end position.");
                    }
                    else
                    {
                        return new BitArray(this.current);
                    }
                }
            }

            /// <summary>
            /// Obtém o elemento da colecção apontado pelo enumerador.
            /// </summary>
            /// <value>O elemento da colecção.</value>
            object IEnumerator.Current
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
            }

            /// <summary>
            /// Avança o enumerador para o próximo elemento da colecção.
            /// </summary>
            /// <returns>
            /// Verdadeiro caso o enumerador avance e falso caso se encontre no final da colecção.
            /// </returns>
            public bool MoveNext()
            {
                if (this.bcdPointer == this.bcdSize)
                {
                    --this.bcdPointer;
                    return true;
                }
                else if (this.bcdPointer == -1)
                {
                    return false;
                }
                else
                {
                    while (true)
                    {
                        if (this.bcdPointer == -1)
                        {
                            return false;
                        }
                        else if (this.moveState) // Move para a direita
                        {
                            var last = this.bcdSize - 1;
                            while (this.bcdPointer < last)
                            {
                                ++this.bcdPointer;
                                this.bcdGeneratorArray[this.bcdPointer] = false;
                            }

                            this.current[this.bcdPointer] = !this.current[this.bcdPointer];
                            this.bcdGeneratorArray[this.bcdPointer] = true;
                            this.moveState = false;
                            return true;
                        }
                        else
                        {
                            while (this.bcdPointer > -1 && this.bcdGeneratorArray[this.bcdPointer])
                            {
                                --this.bcdPointer;
                            }

                            if (this.bcdPointer == -1)
                            {
                                return false;
                            }
                            else
                            {
                                this.bcdGeneratorArray[this.bcdPointer] = true;
                                this.current[this.bcdPointer] = !this.current[this.bcdPointer];
                                this.moveState = true;
                                return true;
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Inicializa o enumerador.
            /// </summary>
            public void Reset()
            {
                this.bcdPointer = this.bcdSize;
                this.bcdGeneratorArray = new BitArray(this.bcdSize, false);
                this.current = new BitArray(this.bcdSize, false);
                this.moveState = true;
            }
        }
    }
}
