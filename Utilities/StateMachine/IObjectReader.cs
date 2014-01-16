// -----------------------------------------------------------------------
// <copyright file="IReader.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IObjectReader<out Object>
    {
        Object Peek();
        Object Get();
        void UnGet();
        bool IsAtEOF();
    }
}
