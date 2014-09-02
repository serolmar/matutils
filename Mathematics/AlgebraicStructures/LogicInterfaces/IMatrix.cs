namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Define as propriedades e métodos essenciais à definição de uma matriz.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas da matriz.</typeparam>
    public interface IMatrix<ObjectType> : IEnumerable<ObjectType>
    {
        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        ObjectType this[int line, int column] { get; set; }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        int GetLength(int dimension);

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        IMatrix<ObjectType> GetSubMatrix(int[] lines, int[] columns);

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        IMatrix<ObjectType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns);

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        void SwapLines(int i, int j);

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        void SwapColumns(int i, int j);

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        void ScalarLineMultiplication(
            int line,
            ObjectType scalar,
            IRing<ObjectType> ring);

        /// <summary>
        /// Substitui a linha especificada por uma combinação linear desta com uma outra. Por exemplo, li = a * li + b * lj, isto é,
        /// a linha i é substituída pela soma do produto de a pela linha i com o produto de b peloa linha j.
        /// </summary>
        /// <param name="i">A linha a ser substituída.</param>
        /// <param name="j">A linha a ser combinada.</param>
        /// <param name="a">O escalar a ser multiplicado pela primeira linha.</param>
        /// <param name="b">O escalar a ser multiplicado pela segunda linha.</param>
        /// <param name="ring">O objecto responsável pelas operações sobre os coeficientes.</param>
        void CombineLines(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring);
    }
}
