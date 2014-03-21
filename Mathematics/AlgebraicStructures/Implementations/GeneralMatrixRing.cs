namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define operações de anel sobre matrizes genéricas.
    /// </summary>
    /// <remarks>
    /// As matrizes sobre as quais é possível operar terão de ser quadradas. Caso contrário, resultarão em erro.
    /// </remarks>
    /// <typeparam name="CoeffType">O tipo de coeficientes da matriz.</typeparam>
    public class GeneralMatrixRing<CoeffType> : GeneralMatrixGroup<CoeffType>, IRing<IMatrix<CoeffType>>
    {
        protected IRing<CoeffType> coeffsRing;

        protected IMultiplicationOperation<IMatrix<CoeffType>, IMatrix<CoeffType>, IMatrix<CoeffType>> matrixMult;

        public GeneralMatrixRing(
            int dimension,
            IMatrixFactory<CoeffType> matrixFactory,
            IRing<CoeffType> coeffsRing)
            : base(dimension, dimension, matrixFactory, coeffsRing)
        {
            if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixMult");
            }
            else
            {
                this.coeffsRing = coeffsRing;
                this.matrixMult = new MatrixMultiplicationOperation<CoeffType>(
                    matrixFactory,
                    this.coeffsRing,
                    this.coeffsRing);
            }
        }

        public IMatrix<CoeffType> MultiplicativeUnity
        {
            get
            {
                return new IdentityMatrix<CoeffType>(this.lines, this.coeffsRing);
            }
        }

        public IMatrix<CoeffType> AddRepeated(IMatrix<CoeffType> element, int times)
        {
            return MathFunctions.AddPower(element, times, this);
        }

        public bool IsMultiplicativeUnity(IMatrix<CoeffType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else if (this.lines != value.GetLength(0))
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} lines.", this.lines));
            }
            else if (this.columns != value.GetLength(1))
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} columns.", this.columns));
            }
            else
            {
                for (int i = 0; i < this.lines; ++i)
                {
                    var diagonal = value[i, i];
                    if (!this.coeffsRing.IsMultiplicativeUnity(diagonal))
                    {
                        return false;
                    }
                }

                for (int i = 0; i < this.lines; ++i)
                {
                    for (int j = 0; j < i; ++j)
                    {
                        var current = value[i, j];
                        if (!this.coeffsRing.IsAdditiveUnity(current))
                        {
                            return false;
                        }
                    }

                    for (int j = i + 1; j < i; ++j)
                    {
                        var current = value[i, j];
                        if (!this.coeffsRing.IsAdditiveUnity(current))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public IMatrix<CoeffType> Multiply(IMatrix<CoeffType> left, IMatrix<CoeffType> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (this.lines != left.GetLength(0) || this.lines != right.GetLength(0))
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} lines.", this.lines));
            }
            else if (this.columns != left.GetLength(1) || this.columns != right.GetLength(1))
            {
                throw new MathematicsException(
                    string.Format("Expecting a matrix with {0} columns.", this.columns));
            }
            else
            {
                return this.matrixMult.Multiply(left, right);
            }
        }
    }
}
