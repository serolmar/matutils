namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
        IAdditionOperation<ObjectType> additionOperation;

        /// <summary>
        /// A classe que será responsável pela instanciação do resultado.
        /// </summary>
        IMatrixFactory<ObjectType> matrixFactory;

        public MatrixMultiplicationOperation(IMatrixFactory<ObjectType> matrixFactory,
            IAdditionOperation<ObjectType> additionOperation,
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
        public IAdditionOperation<ObjectType> AdditionOperation
        {
            get
            {
                return this.additionOperation;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pela instanciação da matriz produto.
        /// </summary>
        public IMatrixFactory<ObjectType> MatrixFactory
        {
            get
            {
                return this.matrixFactory;
            }
        }

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
