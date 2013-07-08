using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class MatrixSet<ComponentType, LineType, ColumnType, T> : ImatrixSet<ComponentType, LineType, ColumnType, T>
    {
        private Dictionary<ComponentType, IMatrix<ComponentType, LineType, ColumnType, T>> components;

        public MatrixSet()
            : this(null)
        {
        }

        public MatrixSet(IEqualityComparer<ComponentType> componentIndexComparer)
        {
            if (componentIndexComparer != null)
            {
                this.components = new Dictionary<ComponentType, IMatrix<ComponentType, LineType, ColumnType, T>>(componentIndexComparer);
            }
            else
            {
                this.components = new Dictionary<ComponentType, IMatrix<ComponentType, LineType, ColumnType, T>>();
            }
        }

        public IMatrix<ComponentType, LineType, ColumnType, T> this[ComponentType componentIndex]
        {
            get
            {
                IMatrix<ComponentType, LineType, ColumnType, T> matrix = null;
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

        internal Dictionary<ComponentType, IMatrix<ComponentType, LineType, ColumnType, T>> Components
        {
            get
            {
                return this.components;
            }
        }

        public void Set(ComponentType componentIndex, IMatrix<ComponentType, LineType, ColumnType, T> matrix)
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

        public void Unset(ComponentType componentIndex)
        {
            this.components.Remove(componentIndex);
        }

        public void Clear()
        {
            this.components.Clear();
        }

        public IEnumerator<IMatrix<ComponentType, LineType, ColumnType, T>> GetEnumerator()
        {
            return this.components.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
