namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DecimalNearestInteger : INearest<Decimal, Decimal>
    {
        public decimal GetNearest(decimal source)
        {
            return Math.Round(source);
        }
    }
}
