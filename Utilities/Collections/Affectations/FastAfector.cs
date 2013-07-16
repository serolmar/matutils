using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    /// <summary>
    /// Afector that has built-in the capability of decide if there are repeated objects.
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

        public int Count
        {
            get
            {
                return this.count;
            }
        }

        public int NumberOfPlaces
        {
            get
            {
                return this.numberOfPlaces;
            }
        }

        public abstract IEnumerator<int[]> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public abstract class FastAffectorEnumerator : IEnumerator<int[]>
        {
            protected FastAfector thisFastAffector;
            protected int[] currentAffectationIndices;
            protected bool isBeforeStart = true;
            protected bool isAfterEnd = false;

            protected int currentPointer = 0;
            private int state = 0;

            public FastAffectorEnumerator(FastAfector affector)
            {
                this.thisFastAffector = affector;
            }

            public int[] Current
            {
                get { return this.GetCurrent(); }
            }

            public void Dispose()
            {
                this.thisFastAffector = null;
            }

            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

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

            public void Reset()
            {
                this.isAfterEnd = false;
                this.isBeforeStart = true;
                this.currentPointer = 0;
                this.state = 0;
            }

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

            protected abstract bool CheckForCurrAffectIndicesValidity();

            /// <summary>
            /// Verify if current inserted is repeated or not. This function may be used to check if some of the affectation indices can repeat according to a specified
            /// compatibility condition. Example: 1 and 3 may be both affected to the same position.
            /// </summary>
            /// <returns>True if there are over repetitions and false otherwise.</returns>
            protected abstract bool VerifyRepetitions();

            /// <summary>
            /// Increments the counter for repetition verification.
            /// </summary>
            protected abstract void IncrementAffectations();

            /// <summary>
            /// Decrements the counter for repetition verification.
            /// </summary>
            protected abstract void DecrementAffectations();

            /// <summary>
            /// Reset the current index.
            /// </summary>
            protected abstract void ResetPointedIndex();

            /// <summary>
            /// Increment the current index.
            /// </summary>
            /// <returns>True if current can be incremented and false otherwise.</returns>
            protected abstract bool IncrementCurrent();

            protected virtual int[] GetCurrent()
            {
                if (isBeforeStart)
                {
                    throw new Exception("Enumerator is in \"IsBeforeStart\" status.");
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
