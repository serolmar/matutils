using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections.Affectations
{
    public class SwapPermutationsGenerator : IEnumerable<int[]>
    {
        private int elementsNumber;

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

        public int ElementsNumber
        {
            get
            {
                return this.elementsNumber;
            }
        }

        public IEnumerator<int[]> GetEnumerator()
        {
            return new SwapPermutationGeneratorEnumerator(this.elementsNumber);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Public Enumerator Class
        private class SwapPermutationGeneratorEnumerator : IEnumerator<int[]>
        {
            protected int elementsNumber;
            protected int[] currentAffectationIndices;
            protected List<int>[] usedIndices;
            protected bool isBeforeStart = true;
            protected bool isAfterEnd = false;

            private bool disposed = false;
            private int currentPointer = 0;

            public SwapPermutationGeneratorEnumerator(int elementsNumber)
            {
                this.elementsNumber = elementsNumber;
            }

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

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            public void Dispose()
            {
                this.usedIndices = null;
                this.disposed = true;
            }

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
