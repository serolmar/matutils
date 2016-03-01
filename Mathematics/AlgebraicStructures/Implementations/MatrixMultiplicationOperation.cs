namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa a operação de multiplicaçõa de matrizes.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos que constituem as entradas das matrizes.</typeparam>
    public class MatrixMultiplicationOperation<ObjectType>
        : IMultiplicationOperation<IMatrix<ObjectType>, IMatrix<ObjectType>, IMatrix<ObjectType>>
    {
        /// <summary>
        /// A classe responsável pela multiplicação das entradas da matriz.
        /// </summary>
        IMultiplicationOperation<ObjectType, ObjectType, ObjectType> multiplicationOperation;

        /// <summary>
        /// A classe que será responsável pela adição das entradas da matriz.
        /// </summary>
        IAdditionOperation<ObjectType, ObjectType, ObjectType> additionOperation;

        /// <summary>
        /// A classe que será responsável pela instanciação do resultado.
        /// </summary>
        IMatrixFactory<ObjectType> matrixFactory;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="MatrixMultiplicationOperation{ObjectType}"/>.
        /// </summary>
        /// <param name="matrixFactory">A fábrica responsável pela criação de matrizes.</param>
        /// <param name="additionOperation">O objecto responsável pela soma de coeficientes.</param>
        /// <param name="multiplicationOperation">O objecto responsável pelo produto dos coeficientes.</param>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        public MatrixMultiplicationOperation(IMatrixFactory<ObjectType> matrixFactory,
            IAdditionOperation<ObjectType, ObjectType, ObjectType> additionOperation,
            IMultiplicationOperation<ObjectType, ObjectType, ObjectType> multiplicationOperation)
        {
            if (multiplicationOperation == null)
            {
                throw new ArgumentNullException("multiplicationOperation");
            }
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
                this.multiplicationOperation = multiplicationOperation;
                this.additionOperation = additionOperation;
                this.matrixFactory = matrixFactory;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pela multiplicação das entradas da matriz.
        /// </summary>
        /// <value>
        /// O objecto responsável pela multiplicação das entradas da matriz.
        /// </value>
        public IMultiplicationOperation<ObjectType, ObjectType, ObjectType> MultiplicationOperation
        {
            get
            {
                return this.multiplicationOperation;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pela adição das entradas da matriz.
        /// </summary>
        /// <remarks>
        /// O objecto responsável pela adição das entradas da matriz.
        /// </remarks>
        public IAdditionOperation<ObjectType, ObjectType, ObjectType> AdditionOperation
        {
            get
            {
                return this.additionOperation;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pela instanciação da matriz produto.
        /// </summary>
        /// <value>
        /// O objecto responsável pela instanciação da matriz produto.
        /// </value>
        public IMatrixFactory<ObjectType> MatrixFactory
        {
            get
            {
                return this.matrixFactory;
            }
        }

        /// <summary>
        /// Calcula o produto de duas matrizes.
        /// </summary>
        /// <remarks>
        /// Uma matriz admite multiplicação com outra quando o número de linhas da primeira coincide com o número de
        /// colunas da segunda.
        /// </remarks>
        /// <param name="left">A primeira matriz a ser multiplicada.</param>
        /// <param name="right">A segunda matriz a ser multiplicada.</param>
        /// <returns>O resultado do produto.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se pelo menos um dos argumentos for nulo.
        /// </exception>
        /// <exception cref="ArgumentException">Se as dimenesões das matrizes não corresponderem.</exception>
        public IMatrix<ObjectType> Multiply(IMatrix<ObjectType> left, IMatrix<ObjectType> right)
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
                var leftColumnsDimension = left.GetLength(1);
                var rightLinesDimension = right.GetLength(0);
                var rightColumnsDimension = right.GetLength(1);
                if (leftColumnsDimension != rightLinesDimension)
                {
                    throw new ArgumentException("The number of columns in left matrix must equal the number of lines in right matrix.");
                }
                else
                {
                    var result = this.matrixFactory.CreateMatrix(leftLinesDimension, rightColumnsDimension);
                    for (int i = 0; i < leftLinesDimension; ++i)
                    {
                        for (int j = 0; j < rightColumnsDimension; ++j)
                        {
                            if (leftColumnsDimension > 0)
                            {
                                var addResult = this.multiplicationOperation.Multiply(
                                    left[i,0],
                                    right[0,j]);

                                for (int k = 1; k < leftColumnsDimension; ++k)
                                {
                                    var multResult = this.multiplicationOperation.Multiply(
                                        left[i, k],
                                        right[k, j]);
                                    addResult = this.additionOperation.Add(addResult, multResult);
                                }

                                result[i,j] = addResult;
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}
