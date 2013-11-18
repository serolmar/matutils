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
        private int bcdSize;

        private BitArray bcdGeneratorArray;

        public BcdCodeGenerator(int bcdSize)
        {
            if (bcdSize < 0)
            {
                throw new ArgumentOutOfRangeException("bcdSize");
            }
            else
            {
                this.bcdSize = bcdSize;
                this.bcdGeneratorArray = new BitArray(bcdSize, false);
            }
        }

        /// <summary>
        /// Obtém o enumerador de códigos BCD com um determinado comprimento.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<BitArray> GetEnumerator()
        {
            return new BcdCodeGeneratorEnumerator(this.bcdSize, this.bcdGeneratorArray);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class BcdCodeGeneratorEnumerator : IEnumerator<BitArray>
        {
            private int bcdSize;

            private BitArray bcdGeneratorArray;

            private int bcdPointer;

            private BitArray current;

            private bool moveState;

            public BcdCodeGeneratorEnumerator(int bcdSize, BitArray bcdGeneratorArray)
            {
                this.bcdSize = bcdSize;
                this.bcdGeneratorArray = bcdGeneratorArray;
                this.Reset();
            }

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

            object IEnumerator.Current
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
