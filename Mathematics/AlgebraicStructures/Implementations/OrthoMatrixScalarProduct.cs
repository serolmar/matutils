namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Aplica o produto escalar a dois vectores linha ou coluna.
    /// </summary>
    public class OrthoMatrixScalarProduct<CoeffType> : IScalarProductSpace<CoeffType, IMatrix<CoeffType>>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IRing<CoeffType> ring;

        public OrthoMatrixScalarProduct(IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }

            this.ring = ring;
        }

        /// <summary>
        /// Multiplica escalarmente dois vectores linha ou coluna.
        /// </summary>
        /// <remarks>
        /// Dois vectores admitem multiplicação caso sejam ambos vectores linha ou ambos vectores coluna e 
        /// tenham o mesmo número de elementos. O produto tensorial invariante não é suportado. Esta função não
        /// tem em linha de conta para os coeficientes métricos a matriz identidade.
        /// </remarks>
        /// <param name="left">O primeiro vector a ser multiplicado.</param>
        /// <param name="right">O segundo vector a ser multiplicado.</param>
        /// <returns>O valor da multiplicação escalar.</returns>
        public CoeffType Multiply(IMatrix<CoeffType> left, IMatrix<CoeffType> right)
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
                var leftLines = left.GetLength(0);
                if (leftLines == 0)
                {
                    throw new ArgumentNullException("Can't multiply empty vectors.");
                }
                else if (leftLines == 1)
                {
                    var rightLines = right.GetLength(0);
                    if (rightLines == 0)
                    {
                        throw new ArgumentException("Can't multiply empty vectors.");
                    }
                    else if (rightLines == 1)
                    {
                        var leftColumns = left.GetLength(1);
                        var rightColumns = right.GetLength(1);
                        if (leftColumns == rightColumns)
                        {
                            if (leftColumns == 0 || rightColumns == 0)
                            {
                                throw new ArgumentNullException("Can't multiply empty vectors.");
                            }
                            else
                            {
                                var result = this.ring.AdditiveUnity;
                                for (int i = 0; i < leftLines; ++i)
                                {
                                    var value = this.ring.Multiply(left[0, i], right[0, i]);
                                    result = this.ring.Add(result, value);
                                }

                                return result;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Can't multiply vectors of different ranks.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Can't multiply a line vector with a column vector.");
                    }
                }
                else
                {
                    var rightLines = right.GetLength(0);
                    if (rightLines == 0)
                    {
                        throw new ArgumentException("Can't multiply empty vectors.");
                    }
                    else if (rightLines == 1)
                    {
                        throw new ArgumentException("Can't multiply a line vector with a column vector.");
                    }
                    else if (rightLines == leftLines)
                    {
                        var leftColumn = left.GetLength(1);
                        var rightColumn = right.GetLength(1);
                        if (leftColumn != 1 || rightColumn != 1)
                        {
                            throw new ArgumentException("Can only multiply line or column vectors but a matrix was provided.");
                        }
                        else
                        {
                            var result = this.ring.AdditiveUnity;
                            for (int i = 0; i < leftLines; ++i)
                            {
                                var value = this.ring.Multiply(left[i, 0], right[i, 0]);
                                result = this.ring.Add(result, value);
                            }

                            return result;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Can't multiply vectors of different ranks.");
                    }
                }
            }
        }
    }
}
