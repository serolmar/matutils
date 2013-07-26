using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures.MultiDimensionalRange
{
    internal class SubMultiDimensionalRange<ObjectType> : IMultiDimensionalRange<ObjectType>
    {
        private IMultiDimensionalRange<ObjectType> multiDimensionalRange;

        private IMultiDimensionalRange<int> subMultiDimensionalRangeDefinition;

        public SubMultiDimensionalRange(
            IMultiDimensionalRange<ObjectType> multiDimensionalRange,
            IMultiDimensionalRange<int> subMultiDimensionalRangeDefinition)
        {
            if (subMultiDimensionalRangeDefinition == null)
            {
                throw new ArgumentNullException("subMultiDimensionalRangeDefinition");
            }
            else
            {
                if (subMultiDimensionalRangeDefinition.Rank == multiDimensionalRange.Rank)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new ArgumentException("Sub multidimensional range definition must match the original multidimensional range.");
                }
            }
        }

        public ObjectType this[int[] coords]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Rank
        {
            get { throw new NotImplementedException(); }
        }

        public int GetLength(int dimension)
        {
            throw new NotImplementedException();
        }

        public IMatrix<ObjectType> GetSubMultiDimensionalRange(IMultiDimensionalRange<ObjectType> subRangeConfiguration)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ObjectType> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
