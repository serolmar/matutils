namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class TransportationMatrixTreeNode
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public TransportationMatrixTreeNode Next { get; set; }

        public override bool Equals(object obj)
        {
            var innerObj = obj as TransportationMatrixTreeNode;
            if (innerObj == null)
            {
                return false;
            }

            return this.Line == innerObj.Line && this.Column == innerObj.Column;
        }

        public override int GetHashCode()
        {
            return (this.Line.GetHashCode() ^ this.Column.GetHashCode()).GetHashCode();
        }
    }
}
