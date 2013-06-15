﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures
{
    public interface ISemigroup<T> : IEqualityComparer<T>
    {
        T Add(T left, T right);
    }
}
