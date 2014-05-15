﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class IdentityMatrix<ElementType> : IMatrix<ElementType>
    {
        /// <summary>
        /// O anel responsável pela indicação das unidades aditiva e multiplicativa.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// As dimensões da matriz quadrada.
        /// </summary>
        private int dimensions;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="IdentityMatrix{ElementType}"/>.
        /// </summary>
        /// <param name="dimensions">A dimensão da matriz.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Se o anel for nulo.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Se a dimensão for negativa.</exception>
        public IdentityMatrix(int dimensions, IRing<ElementType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (dimensions < 0)
            {
                throw new ArgumentOutOfRangeException("dimensions");
            }
            else {
                this.ring = ring;
                this.dimensions = dimensions;
            }
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão.
        /// </exception>
        public ElementType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.dimensions)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else if (column < 0 || column >= this.dimensions)
                {
                    throw new IndexOutOfRangeException("Parameter line must be non-negative and less than the size of matrix.");
                }
                else
                {
                    if (line == column)
                    {
                        return this.ring.MultiplicativeUnity;
                    }
                    else
                    {
                        return this.ring.AdditiveUnity;
                    }
                }
            }
            set
            {
                throw new MathematicsException("Identity matrix is constant.");
            }
        }

        /// <summary>
        /// Retorna sempre verdadeiro uma vez que a matriz identidade é simétrica.
        /// </summary>
        /// <param name="equalityComparer">O comparador de coeficientes.</param>
        /// <returns>Verdadeiro.</returns>
        public bool IsSymmetric(IEqualityComparer<ElementType> equalityComparer)
        {
            return true;
        }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        /// <exception cref="ArgumentException">Se o valor da dimensão diferir de zero ou um.</exception>
        public int GetLength(int dimension)
        {
            if (dimension == 0 || dimension == 1)
            {
                return this.dimensions;
            }
            else
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ElementType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ElementType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ElementType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<ElementType>(this, lines, columns);
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <remarks>
        /// Como não é possível trocar as linhas ou colunas a uma matriz identidade, esta função resulta sempre em erro.
        /// </remarks>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="MathematicsException">
        /// Sempre.
        /// </exception>
        public void SwapLines(int i, int j)
        {
            throw new MathematicsException("Identity matrix is constant.");
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <remarks>
        /// Como não é possível trocar as linhas ou colunas a uma matriz identidade, esta função resulta sempre em erro.
        /// </remarks>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="MathematicsException">
        /// Sempre.
        /// </exception>
        public void SwapColumns(int i, int j)
        {
            throw new MathematicsException("Identity matrix is constant.");
        }

        /// <summary>
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public IEnumerator<ElementType> GetEnumerator()
        {
            for (int i = 0; i < this.dimensions; ++i)
            {
                for (int j = 0; j < this.dimensions; ++j)
                {
                    if (i == j)
                    {
                        yield return this.ring.MultiplicativeUnity;
                    }
                    else
                    {
                        yield return this.ring.AdditiveUnity;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o enumerador não genércio para a matriz.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
