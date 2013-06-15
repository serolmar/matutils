using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    /// <summary>
    /// Affects a structure given by a matrix containing the positions to which indices can be affected.
    /// </summary>
    public class StructureAffector : FastAfector
    {
        protected int[][] affectorMatrix;
        private Dictionary<int, int> numberOfPossibleAffectationsByIndice = new Dictionary<int, int>();

        public StructureAffector(ICollection<ICollection<int>> affectorStructure)
        {
            if (affectorStructure == null) throw new ArgumentNullException("affectorStructure");
            if (affectorStructure.Count == 0) throw new ArgumentException("Parameter collection affectorStructure must have elements to affect.");

            this.affectorMatrix = new int[affectorStructure.Count][];
            int pointer = 0;
            InsertionSortedCollection<int> sorter = new InsertionSortedCollection<int>(Comparer<int>.Default);
            int maximum = -1;

            foreach (var item in affectorStructure)
            {
                sorter.Clear();
                foreach (var innerItem in item)
                {
                    if (!sorter.HasElement(innerItem))
                    {
                        sorter.InsertSortElement(innerItem);
                        if (innerItem > maximum)
                        {
                            maximum = innerItem;
                        }
                    }
                }

                this.affectorMatrix[pointer++] = sorter.ToArray();
                this.Count = maximum;
                this.NumberOfPlaces = this.affectorMatrix.Length;
            }
        }

        public StructureAffector(ICollection<ICollection<int>> affectorStructure, ICollection<int> possibleAffectionsByIndice) : this(affectorStructure)
        {
            foreach (var item in possibleAffectionsByIndice)
            {
                if (item < 0) throw new ArgumentException("Every element in parameter possibleAffectationsByIndices must be greater than zero.");
            }
        }

        public override IEnumerator<int[]> GetEnumerator()
        {
            return new StructureAffectorEnumerator(this);
        }

        public class StructureAffectorEnumerator : FastAffectorEnumerator
        {
            //private int[] pointerByIndice;
            private Dictionary<int, int> affectedIndices = new Dictionary<int, int>();

            public StructureAffectorEnumerator(StructureAffector structureAffector) : base(structureAffector)
            {
                this.currentAffectationIndices = new int[structureAffector.NumberOfPlaces];
            //    this.pointerByIndice = new int[structureAffector.NumberOfPlaces];
            //    Array.Clear(this.pointerByIndice,0, structureAffector.NumberOfPlaces);
            }

            protected override void ResetPointedIndex()
            {
                this.currentAffectationIndices[this.currentPointer] = 0;
            }

            protected override bool CheckForCurrAffectIndicesValidity()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                return this.currentAffectationIndices[this.currentPointer] != affector.affectorMatrix[this.currentPointer].Length;
            }

            protected override bool VerifyRepetitions()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                int indexBeingAffected = affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]];

                if (affector.numberOfPossibleAffectationsByIndice.ContainsKey(indexBeingAffected))
                {
                    if (!this.affectedIndices.ContainsKey(indexBeingAffected))
                    {
                        if (affector.numberOfPossibleAffectationsByIndice[indexBeingAffected] == 0)
                        {
                            return false;
                        }
                        else
                        {
                            this.affectedIndices.Add(indexBeingAffected, 1);
                        }
                    }

                    if (this.affectedIndices[indexBeingAffected] == affector.numberOfPossibleAffectationsByIndice[indexBeingAffected])
                    {
                        return false;
                    }
                }
                else
                {
                    if (this.affectedIndices.ContainsKey(indexBeingAffected))
                    {
                        if (this.affectedIndices[indexBeingAffected] == 1)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            protected override bool IncrementCurrent()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                ++this.currentAffectationIndices[this.currentPointer];
                if (this.currentAffectationIndices[this.currentPointer] == affector.affectorMatrix[this.currentPointer].Length) return false;
                return true;
            }

            protected override void IncrementAffectations()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                if (this.affectedIndices.ContainsKey(affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]]))
                {
                    ++this.affectedIndices[affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]]];
                }
                else
                {
                    this.affectedIndices.Add(affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]], 1);
                }
            }

            protected override void DecrementAffectations()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                if (this.affectedIndices.ContainsKey(affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]]))
                {
                    --this.affectedIndices[affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]]];
                }
            }

            protected override int[] GetCurrent()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                if (isBeforeStart)
                {
                    throw new Exception("Enumerator is in \"IsBeforeStart\" status.");
                }
                int[] result = new int[this.currentAffectationIndices.Length];
                for (int i = 0; i < this.currentAffectationIndices.Length; ++i)
                {
                    result[i] =  affector.affectorMatrix[i][this.currentAffectationIndices[i]];
                }

                return result;
            }
        }
    }
}
