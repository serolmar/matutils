using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdmpProblem
{
    class OdmpMatrixSet<ComponentType, LineType, ColumnType, T> : IOdmpMatrixSet<ComponentType, LineType, ColumnType, T>
    {
        private Dictionary<ComponentType, IOdmpMatrix<ComponentType, LineType, ColumnType, T>> components;

        public OdmpMatrixSet()
            : this(null)
        {
        }

        public OdmpMatrixSet(IEqualityComparer<ComponentType> componentIndexComparer)
        {
            if (componentIndexComparer != null)
            {
                this.components = new Dictionary<ComponentType, IOdmpMatrix<ComponentType, LineType, ColumnType, T>>(componentIndexComparer);
            }
            else
            {
                this.components = new Dictionary<ComponentType, IOdmpMatrix<ComponentType, LineType, ColumnType, T>>();
            }
        }

        public IOdmpMatrix<ComponentType, LineType, ColumnType, T> this[ComponentType componentIndex]
        {
            get
            {
                IOdmpMatrix<ComponentType, LineType, ColumnType, T> matrix = null;
                if (this.components.TryGetValue(componentIndex, out matrix))
                {
                    return matrix;
                }
                else
                {
                    throw new OdmpProblemException("Matrix index doesn't exist.");
                }
            }
        }

        public int Count
        {
            get
            {
                return this.components.Count;
            }
        }

        internal Dictionary<ComponentType, IOdmpMatrix<ComponentType, LineType, ColumnType, T>> Components
        {
            get
            {
                return this.components;
            }
        }

        public void Set(ComponentType componentIndex, IOdmpMatrix<ComponentType, LineType, ColumnType, T> matrix)
        {
            if (matrix == null)
            {
                throw new OdmpProblemException("A matrix must be provided.");
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

        public IEnumerator<IOdmpMatrix<ComponentType, LineType, ColumnType, T>> GetEnumerator()
        {
            return this.components.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
