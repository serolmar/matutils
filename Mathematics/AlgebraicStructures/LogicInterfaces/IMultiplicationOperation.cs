namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IMultiplicationOperation<in P, in Q, out T>
    {
        T Multiply(P left, Q right);
    }
}
