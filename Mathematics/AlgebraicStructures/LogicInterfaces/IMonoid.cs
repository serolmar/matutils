﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IMonoid<T> : ISemigroup<T>
    {
        T AdditiveUnity { get; }
        bool IsAdditiveUnity(T value);
    }
}
