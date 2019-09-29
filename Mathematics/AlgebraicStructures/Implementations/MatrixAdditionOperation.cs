namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa as operações de adição sobre matrizes.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos que constituem as entradas das matrizes.</typeparam>
    public class MatrixAdditionOperation<ObjectType> :
        IAdditionOperation<IMatrix<ObjectType>, IMatrix<ObjectType>, IMatrix<ObjectType>>
    {
        /// <summary>
        /// A classe que será responsável pela adição das entradas da matriz.
        /// </summary>
        protected IAdditionOperation<ObjectType, ObjectType, ObjectType> additionOperation;

        /// <summary>
        /// A classe que será responsável pela instanciação do resultado.
        /// </summary>
        protected IMathMatrixFactory<ObjectType> matrixFactory;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="MatrixAdditionOperation{ObjectType}"/>.
        /// </summary>
        /// <param name="matrixFactory">A fábrica responsável pela criação de instâncias de matrizes.</param>
        /// <param name="additionOperation">O objecto responsável pela adição dos coeficientes.</param>
        /// <exception cref="ArgumentNullException">
        /// Se um dos argumentos for nulo.
        /// </exception>
        public MatrixAdditionOperation(
            IMathMatrixFactory<ObjectType> matrixFactory,
            IAdditionOperation<ObjectType, ObjectType, ObjectType> additionOperation)
        {
            if (additionOperation == null)
            {
                throw new ArgumentNullException("additionOperation");
            }
            else if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else
            {
                this.additionOperation = additionOperation;
                this.matrixFactory = matrixFactory;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pela instanciação da matriz soma.
        /// </summary>
        /// <value>
        /// O objecto responsável pela instanciação da matriz soma.
        /// </value>
        public IMathMatrixFactory<ObjectType> MatrixFactory
        {
            get
            {
                return this.matrixFactory;
            }
        }

        /// <summary>
        /// Calcula a soma de duas matrizes.
        /// </summary>
        /// <remarks>
        /// Duas matrizes podem ser somada no caso de possuirem as mesmas dimensões, isto é, no caso de
        /// conterem o mesmo número de linhas e o mesmo número de colunas.
        /// </remarks>
        /// <param name="left">A primeira matriz.</param>
        /// <param name="right">A segunda matriz.</param>
        /// <returns>O resultado da soma.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se pelo menos um dos argumentos for nulo.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Se as dimensões da matriz não corresponderem.
        /// </exception>
        public IMatrix<ObjectType> Add(IMatrix<ObjectType> left, IMatrix<ObjectType> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var leftLinesDimension = left.GetLength(0);
                var leftColumnsDimension = right.GetLength(1);
                var rightLinesDimension = right.GetLength(0);
                var rightColumnsDimension = right.GetLength(1);
                if (leftLinesDimension != rightLinesDimension)
                {
                    throw new ArgumentException("The number of lines in let matrix must be equal to the number of lines in right matrix.");
                }
                else if (leftColumnsDimension != rightColumnsDimension)
                {
                    throw new ArgumentException("The number of columns in let matrix must be equal to the number of columns in right matrix.");
                }
                else
                {
                    var result = this.matrixFactory.CreateMatrix(leftLinesDimension, leftColumnsDimension);
                    for (int i = 0; i < leftLinesDimension; ++i)
                    {
                        for (int j = 0; j < leftColumnsDimension; ++j)
                        {
                            result[i, j] = this.additionOperation.Add(left[i, j], right[i, j]);
                        }
                    }

                    return result;
                }
            }
        }
    }
}
