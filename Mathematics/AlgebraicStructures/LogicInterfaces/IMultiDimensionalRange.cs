using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMultiDimensionalRange<ObjectType> : IEnumerable<ObjectType>
    {
        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="coords">As coordenadas do elemento onde se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        ObjectType this[int[] coords] { get; set; }

        /// <summary>
        /// Obtém o número de dimensões associados à matriz multidimensional.
        /// </summary>
        int Rank { get; }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        int GetLength(int dimension);

        /// <summary>
        /// Obtém a submatriz multidimensional indicada no argumento.
        /// </summary>
        /// <param name="subRangeConfiguration">As correnadas dos elementos que constituem a submatriz multidimensional.</param>
        /// <returns>A submatriz multidimensional procurada.</returns>
        IMatrix<ObjectType> GetSubMultiDimensionalRange(IMultiDimensionalRange<ObjectType> subRangeConfiguration);
    }
}
