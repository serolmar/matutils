namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    public class NullableIntegerNearest : INearest<Nullable<double>,int>
    {
        public int GetNearest(Nullable<double> source)
        {
            if (source.HasValue)
            {
                return (int)Math.Round(source.Value);
            }
            else
            {
                throw new Exception("No argument was supplied.");
            }
        }
    }
}
