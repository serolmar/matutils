using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class MatrixSet<T> : ImatrixSet<int, int, int, int, T>
    {
        private Dictionary<int, IMatrix<int, int, int, T>> components;

        public MatrixSet()
        {
            this.components = new Dictionary<int, IMatrix<int, int, int, T>>();
        }

        public IMatrix<int, int, int, T> this[int componentIndex]
        {
            get {
                IMatrix<int, int, int, T> matrix = null;
                if (this.components.TryGetValue(componentIndex, out matrix))
                {
                    return matrix;
                }
                else
                {
                    throw new MathematicsException("Matrix index doesn't exist.");
                }
            }
        }

        internal Dictionary<int, IMatrix<int, int, int, T>> Components
        {
            get
            {
                return this.components;
            }
        }

        public void Set(int componentIndex, IMatrix<int, int, int, T> matrix)
        {
            if (matrix == null)
            {
                throw new MathematicsException("A matrix must be provided.");
            }

            if (this.components.ContainsKey(componentIndex))
            {
                this.components[componentIndex] = matrix;
            }
            else
            {
                this.components.Add(componentIndex, matrix);
            }
        }

        public void Unset(int componentIndex)
        {
            this.components.Remove(componentIndex);
        }

        public void Clear()
        {
            this.components.Clear();
        }

        public IEnumerator<IMatrix<int, int, int, T>> GetEnumerator()
        {
            return this.components.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
