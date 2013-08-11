using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class MatrixMultiplicationOperation<ObjectType, AdditionOperationType, MultiplicationOperationType> : IMultiplicationOperation<IMatrix<ObjectType>>
        where AdditionOperationType : IAdditionOperation<ObjectType>
        where MultiplicationOperationType : IMultiplicationOperation<ObjectType>
    {
        /// <summary>
        /// A classe responsável pela multiplicação das entradas da matriz.
        /// </summary>
        MultiplicationOperationType multiplicationOperation;

        /// <summary>
        /// A classe que será responsável pela adição das entradas da matriz.
        /// </summary>
        AdditionOperationType additionOperation;

        /// <summary>
        /// A classe que será responsável pela instanciação do resultado.
        /// </summary>
        IMatrixFactory<ObjectType> matrixFactory;

        public MatrixMultiplicationOperation(IMatrixFactory<ObjectType> matrixFactory,
            AdditionOperationType additionOperation,
            MultiplicationOperationType multiplicationOperation)
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
                var leftColumnsDimension = right.GetLength(1);
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
