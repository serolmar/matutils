using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class MatrixSet<ComponentIndex, Line, Column, T> : ImatrixSet<ComponentIndex, Line, Column, T>
    {
        private Dictionary<ComponentIndex, IMatrix<Line, Column, T>> components;

        public MatrixSet()
            : this(null)
        {
        }

        public MatrixSet(IEqualityComparer<ComponentIndex> componentIndexComparer)
        {
            if (componentIndexComparer != null)
            {
                this.components = new Dictionary<ComponentIndex, IMatrix<Line, Column, T>>(componentIndexComparer);
            }
            else
            {
                this.components = new Dictionary<ComponentIndex, IMatrix<Line, Column, T>>();
            }
        }

        public IMatrix<Line, Column, T> this[ComponentIndex componentIndex]
        {
            get
            {
                IMatrix<Line, Column, T> matrix = null;
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

        internal Dictionary<ComponentIndex, IMatrix<Line, Column, T>> Components
        {
            get
            {
                return this.components;
            }
        }

        public void Set(ComponentIndex componentIndex, IMatrix<Line, Column, T> matrix)
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

        public void Unset(ComponentIndex componentIndex)
        {
            this.components.Remove(componentIndex);
        }

        public void Clear()
        {
            this.components.Clear();
        }

        public IEnumerator<IMatrix<Line, Column, T>> GetEnumerator()
        {
            return this.components.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
