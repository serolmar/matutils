using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public abstract class ADeterminant<ElementsType> : IAlgorithm<IMatrix<ElementsType>, ElementsType>
    {
        protected IRing<ElementsType> ring;

        public ADeterminant(IRing<ElementsType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                this.ring = ring;
            }
        }

        public ElementsType Run(IMatrix<ElementsType> data)
        {
            if (data == null)
            {
                return this.ring.AdditiveUnity;
            }
            else
            {
                var lines = data.GetLength(0);
                var columns = data.GetLength(1);
                if (lines != columns)
                {
                    throw new MathematicsException("Determinants can only be applied to square matrices.");
                }
                else
                {
                    return this.ComputeDeterminant(data);
                }
            }
        }

        /// <summary>
        /// Calcula o determinante da matriz.
        /// </summary>
        /// <param name="data">A matriz.</param>
        /// <returns>O determinante.</returns>
        protected abstract ElementsType ComputeDeterminant(IMatrix<ElementsType> data);
    }
}
