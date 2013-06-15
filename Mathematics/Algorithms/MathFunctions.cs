namespace Mathematics.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AlgebraicStructures;

    public static class MathFunctions
    {
        public static T Power<T>(T val, int pow, IMultipliable<T> multiplier)
        {
            if (multiplier == null)
            {
                throw new MathematicsException("Parameter multiplier can't be null.");
            }

            if (pow < 0)
            {
                throw new MathematicsException("Can't computer the powers with negative expoents.");
            }

            if (pow == 0)
            {
                return multiplier.MultiplicativeUnity;
            }

            var result = val;
            var rem = pow % 2;
            pow = pow / 2;
            while (pow > 0)
            {
                result = multiplier.Multiply(result, result);
                if (rem == 1)
                {
                    result = multiplier.Multiply(val, result);
                }

                rem = pow % 2;
                pow = pow / 2;
            }

            return result;
        }
    }
}
