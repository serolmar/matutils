namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MatrixAdditionOperation<ObjectType, AdditionOperationType> : IAdditionOperation<IMatrix<ObjectType>>
        where AdditionOperationType : IAdditionOperation<ObjectType>
    {
        /// <summary>
        /// A classe que será responsável pela adição das entradas da matriz.
        /// </summary>
        AdditionOperationType additionOperation;

        /// <summary>
        /// A classe que será responsável pela instanciação do resultado.
        /// </summary>
        IMatrixFactory<ObjectType> matrixFactory;

        public MatrixAdditionOperation(IMatrixFactory<ObjectType> matrixFactory, AdditionOperationType additionOperation)
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
        public IMatrixFactory<ObjectType> MatrixFactory
        {
            get
            {
                return this.matrixFactory;
            }
        }

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
