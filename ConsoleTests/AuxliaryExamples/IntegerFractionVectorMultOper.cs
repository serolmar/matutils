namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using IntFractionVector = Mathematics.IVector<Mathematics.Fraction<int, Mathematics.IntegerDomain>>;

    public class IntegerFractionVectorMultOper
        : IMultiplicationOperation<int, IntFractionVector, IntFractionVector>
    {
        private CoeffFractionMultiplicationOperation<int, IntegerDomain> coeffFracMult;

        public IntegerFractionVectorMultOper()
        {
            this.coeffFracMult = new CoeffFractionMultiplicationOperation<int, IntegerDomain>();
        }

        /// <summary>
        /// Permite multiplicar um inteiro por um vector de valores fraccionários.
        /// </summary>
        /// <param name="left">O inteiro a ser multiplicado.</param>
        /// <param name="right">O vector de valores fraccionários.</param>
        /// <returns>O vector resultante.</returns>
        public IntFractionVector Multiply(
            int left,
            IntFractionVector right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else
            {
                var result = new ArrayVector<Fraction<int, IntegerDomain>>(right.Length);
                for (int i = 0; i < right.Length; ++i)
                {
                    result[i] = this.coeffFracMult.Multiply(left, right[i]);
                }

                return result;
            }
        }
    }
}
