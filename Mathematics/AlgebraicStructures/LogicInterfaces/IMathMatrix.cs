namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Define as propriedades e métodos matemáticos essenciais numa matriz.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas da matriz.</typeparam>
    public interface IMathMatrix<ObjectType> : IMatrix<ObjectType>
    {
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

    /// <summary>
    /// Define as propriedades e métodos matemáticos essenciais numa matriz.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas da matriz.</typeparam>
    public interface ILongMathMatrix<ObjectType> : IMathMatrix<ObjectType>, ILongMatrix<ObjectType>
    {
        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        void ScalarLineMultiplication(
            long line,
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
            long i,
            long j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring);
    }

    /// <summary>
    /// Representa uma matriz quadrada.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public interface ISquareMathMatrix<CoeffType> 
        : ISquareMatrix<CoeffType>, IMathMatrix<CoeffType>
    {
    }

    /// <summary>
    /// Define as propriedades e métodos essenciais a uma matriz esparsa.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    public interface ISparseMathMatrix<ObjectType> 
        : ISparseMatrix<ObjectType, ISparseMatrixLine<ObjectType>>, 
        IMathMatrix<ObjectType>
    {
    }

    /// <summary>
    /// Define as propriedades e métodos essenciais a uma matriz esparsa.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    public interface ILongSparseMathMatrix<ObjectType>
        : ILongSparseMatrix<ObjectType, ILongSparseMatrixLine<ObjectType>>,
        ILongMathMatrix<ObjectType>
    {
    }
}
